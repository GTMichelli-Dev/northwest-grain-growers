/**
 * gm.pin-prompt.js — Single, shared PIN entry modal for the whole app.
 *
 * Use it from any page like this:
 *
 *   GM.requestPin({
 *       title: "Enter PIN",                     // optional, defaults to "Enter PIN"
 *       prompt: "Enter your PIN to void load",   // optional, body text
 *       requiredPrivilegeId: 10                  // optional, server-side priv check
 *   })
 *   .then(function (result) {
 *       // result = { pin, userId, userName }
 *   })
 *   .catch(function (err) {
 *       // user cancelled OR invalid PIN OR missing privilege
 *   });
 *
 * The HTML the modal binds to lives in Views/Shared/_GmPinPromptModal.cshtml,
 * which _Layout includes once. Pages must NOT define their own copy of the
 * gm-pin-prompt-* IDs.
 */
var GM = window.GM || {};

(function (ns) {
    "use strict";

    var SEL = {
        modal:   "#gm-pin-prompt-modal",
        title:   "#gm-pin-prompt-label",
        prompt:  "#gm-pin-prompt-text",
        input:   "#gm-pin-prompt-input",
        error:   "#gm-pin-prompt-error",
        submit:  "#gm-pin-prompt-submit",
    };

    var _bsModal = null;
    var _resolve = null;
    var _reject  = null;
    var _opts    = null;

    // Privilege id that bypasses every other priv check (RemoteAdmin / "admin").
    // Mirrors GrainManagement.Constants.Privileges.RemoteAdmin (server-side).
    var ADMIN_PRIV_ID = 7;

    // ── PIN cache ──────────────────────────────────────────────────────────
    // Last successful PIN validation. Persisted to sessionStorage so the
    // operator's PIN survives page navigations within the same tab — without
    // it, every nav (e.g. WS edit → New Lot screen) would re-prompt even when
    // the same operator already validated.
    //
    // Shape: { pin, userId, userName, privileges: [int...], expiresAt: ms }
    //
    // The TTL is short (10 minutes) so a forgotten kiosk session doesn't stay
    // unlocked indefinitely. Closing the tab also drops it (sessionStorage).
    var CACHE_KEY = "gm.pinCache.v1";
    var CACHE_TTL_MS = 10 * 60 * 1000;

    function loadCache() {
        try {
            var raw = window.sessionStorage.getItem(CACHE_KEY);
            if (!raw) return null;
            var obj = JSON.parse(raw);
            if (!obj || !obj.pin || !obj.expiresAt || obj.expiresAt < Date.now()) {
                window.sessionStorage.removeItem(CACHE_KEY);
                return null;
            }
            return obj;
        } catch (e) { return null; }
    }
    function saveCache(entry) {
        try {
            entry.expiresAt = Date.now() + CACHE_TTL_MS;
            window.sessionStorage.setItem(CACHE_KEY, JSON.stringify(entry));
        } catch (e) { /* sessionStorage unavailable — degrade silently */ }
    }
    function clearCache() {
        try { window.sessionStorage.removeItem(CACHE_KEY); } catch (e) {}
    }

    // Hydrate the in-memory cache from sessionStorage on script load so the
    // first GM.requestPin call after a navigation can short-circuit.
    ns.lastPin = loadCache();

    // Drop the cached PIN. Call after a sensitive action when you want the
    // next gated action to re-prompt for a fresh keypad entry.
    ns.clearLastPin = function () { ns.lastPin = null; clearCache(); };

    // True if the cached PIN's user holds requiredPrivilegeId or the admin
    // override priv. Returns false when there's no cache.
    function cachedSatisfies(requiredPrivilegeId) {
        if (!ns.lastPin || !ns.lastPin.privileges) return false;
        var privs = ns.lastPin.privileges;
        for (var i = 0; i < privs.length; i++) {
            if (privs[i] === requiredPrivilegeId || privs[i] === ADMIN_PRIV_ID) {
                return true;
            }
        }
        return false;
    }

    function ensureBound() {
        if (_bsModal) return _bsModal;
        var el = document.querySelector(SEL.modal);
        if (!el) {
            // Layout didn't render the partial — fail loudly so the dev fixes it.
            throw new Error(
                "GM.requestPin: shared PIN modal not found. " +
                "Make sure _Layout.cshtml includes _GmPinPromptModal.cshtml."
            );
        }
        _bsModal = new bootstrap.Modal(el);

        $(SEL.submit).on("click", submit);
        $(SEL.input).on("keydown", function (e) {
            if (e.key === "Enter") { e.preventDefault(); submit(); }
        });
        // Stacked-modal z-index fix. When PIN opens on top of another visible
        // modal (e.g. the EOD orchestrator's gm-eod-modal, which uses
        // data-bs-backdrop="static"), Bootstrap 5.3 does not bump z-index for
        // the second modal — it ends up at the same 1055 as the first and
        // the underlying modal-content visually covers the smaller PIN
        // dialog. Bump the PIN modal above whatever's already showing, and
        // bump ONLY the new backdrop Bootstrap appends — touching the
        // existing backdrop would leave it elevated above the underlying
        // modal's content after PIN closes, locking the screen.
        $(el).on("show.bs.modal", function () {
            var others = document.querySelectorAll(".modal.show");
            if (!others.length) return; // not stacked — default styling is fine
            var maxZ = 1055;
            others.forEach(function (m) {
                var z = parseInt(getComputedStyle(m).zIndex, 10) || 1055;
                if (z > maxZ) maxZ = z;
            });
            el.style.zIndex = (maxZ + 10);
            // Snapshot pre-existing backdrops so we can identify the new one
            // Bootstrap appends on the deferred tick.
            var preExisting = new Set(document.querySelectorAll(".modal-backdrop"));
            setTimeout(function () {
                document.querySelectorAll(".modal-backdrop").forEach(function (b) {
                    if (!preExisting.has(b)) {
                        b.style.zIndex = (maxZ + 5);
                    }
                });
            }, 0);
        });
        // Reject the in-flight promise on dismiss-without-submit, and reset
        // any stacked-modal z-index override so the next open recalculates.
        $(el).on("hidden.bs.modal", function () {
            el.style.zIndex = "";
            // Sweep the fallback-backdrop the timeout sanity check may
            // have added — Bootstrap doesn't know about it so it would
            // leave it behind and dim the next page forever.
            document.querySelectorAll('.modal-backdrop.gm-pin-fallback-bd').forEach(function (b) {
                b.remove();
            });
            if (_reject) {
                var reject = _reject;
                _resolve = null; _reject = null;
                reject(new Error("cancelled"));
            }
        });
        // Focus the PIN input as soon as Bootstrap's open animation finishes.
        // Using shown.bs.modal (not a setTimeout race) makes this reliable
        // even when the modal is stacked on top of another modal — operator
        // can start keying the PIN without an extra click.
        $(el).on("shown.bs.modal", function () {
            var input = document.querySelector(SEL.input);
            if (input) input.focus();
        });
        return _bsModal;
    }

    function submit() {
        var pin = parseInt($(SEL.input).val(), 10) || 0;
        if (pin <= 0) {
            $(SEL.error).text("PIN is required.").removeAttr("hidden");
            return;
        }

        $(SEL.submit).prop("disabled", true).text("Checking…");
        $(SEL.error).attr("hidden", true);

        var url = "/api/GrowerDelivery/ValidatePin?pin=" + encodeURIComponent(pin);
        if (_opts && _opts.requiredPrivilegeId) {
            url += "&requiredPrivilegeId=" + encodeURIComponent(_opts.requiredPrivilegeId);
        }

        $.getJSON(url)
            .done(function (data) {
                var result = {
                    pin: pin,
                    userId: data.UserId,
                    userName: data.UserName,
                    privileges: Array.isArray(data.Privileges) ? data.Privileges : [],
                };
                ns.lastPin = result;
                saveCache(result);
                _bsModal.hide();
                var resolve = _resolve;
                _resolve = null; _reject = null;
                if (resolve) resolve(result);
            })
            .fail(function (xhr) {
                var msg = (xhr.responseJSON && xhr.responseJSON.message)
                    ? xhr.responseJSON.message
                    : (xhr.status === 403
                        ? "User does not have permission for this action."
                        : "Invalid or inactive PIN.");
                $(SEL.error).text(msg).removeAttr("hidden");
                $(SEL.input).val("").trigger("focus");
            })
            .always(function () {
                $(SEL.submit).prop("disabled", false).text("Continue");
            });
    }

    /**
     * Show the shared PIN prompt and resolve with { pin, userId, userName,
     * privileges } on validation success. Rejects on cancel or PIN failure.
     *
     * Options:
     *   title                — modal heading (default "Enter PIN")
     *   prompt               — body text (default "Enter your PIN to continue.")
     *   requiredPrivilegeId  — server validates the PIN holds this priv
     *   forcePrompt          — when true, skip the cache and always re-prompt
     *                           (use after a sensitive action, when you want
     *                            a fresh keypad entry from the operator)
     *
     * If a previously validated PIN (GM.lastPin) already holds the required
     * privilege (or the admin override), the promise resolves immediately
     * without showing the modal — no double-prompting for the same operator.
     */
    ns.requestPin = function (opts) {
        opts = opts || {};
        console.log('[GM.requestPin] called with opts=', opts);
        return new Promise(function (resolve, reject) {
            // Reuse the cached PIN if it satisfies the priv requirement.
            if (!opts.forcePrompt
                && opts.requiredPrivilegeId
                && cachedSatisfies(opts.requiredPrivilegeId)) {
                console.log('[GM.requestPin] cache hit — resolving without modal');
                resolve(ns.lastPin);
                return;
            }

            try { ensureBound(); }
            catch (e) {
                console.error('[GM.requestPin] ensureBound failed:', e);
                reject(e);
                return;
            }

            // Cancel any in-flight request first.
            if (_reject) { _reject(new Error("superseded")); }
            _resolve = resolve;
            _reject  = reject;
            _opts    = opts;

            $(SEL.title).text(opts.title || "Enter PIN");
            $(SEL.prompt).text(opts.prompt || "Enter your PIN to continue.");
            $(SEL.input).val("");
            $(SEL.error).attr("hidden", true).text("");
            $(SEL.submit).prop("disabled", false).text("Continue");

            console.log('[GM.requestPin] showing modal');
            _bsModal.show();
            // Sanity check: if Bootstrap silently failed to render the
            // modal (orphan backdrop blocking, modal already considered
            // shown internally, etc.) the user is left staring at the
            // EOD spinner with no PIN dialog. After 300ms confirm the
            // modal element has the `show` class; if not, force it
            // visible via direct DOM manipulation and create our own
            // backdrop above any leftover ones.
            setTimeout(function () {
                var modalEl = document.querySelector(SEL.modal);
                if (!modalEl) return;
                if (!modalEl.classList.contains('show')) {
                    console.warn('[GM.requestPin] modal still not shown after 300ms — forcing visibility');
                    modalEl.classList.add('show');
                    modalEl.style.display = 'block';
                    modalEl.style.zIndex = '1080';
                    modalEl.removeAttribute('aria-hidden');
                    modalEl.setAttribute('aria-modal', 'true');
                    modalEl.setAttribute('role', 'dialog');
                    document.body.classList.add('modal-open');
                    if (!document.querySelector('.modal-backdrop.gm-pin-fallback-bd')) {
                        var bd = document.createElement('div');
                        bd.className = 'modal-backdrop fade show gm-pin-fallback-bd';
                        bd.style.zIndex = '1070';
                        document.body.appendChild(bd);
                    }
                    var input = document.querySelector(SEL.input);
                    if (input) input.focus();
                }
            }, 300);
            // Focus is wired in ensureBound via shown.bs.modal — no need for
            // a fragile setTimeout race here.
        });
    };
})(GM);
