/**
 * gm.eod.js — End Of Day orchestrator (single + multi-location).
 *
 * Public API:
 *   GM.eod.runSingle(locationId)
 *       Runs EOD for one location: generate PDF, preview, PIN, finalize.
 *
 *   GM.eod.runMultiLocation({ startingLocationId? })
 *       Starts with the operator's current location, then iterates any
 *       other accessible location that still has open weight sheets.
 *       PDF preview + per-location confirm in a single shared modal.
 *       Returns to the starting location implicitly — the cookie is
 *       never touched, all calls are routed via locationId in the URL.
 *
 *   GM.eod.promptForStaleStart(locationId)
 *       Asks the operator whether to launch the multi-location loop
 *       because prior-day open weight sheets were detected. Used by the
 *       warehouse dashboard's SignalR auto-trigger.
 *
 * The single-modal UI is injected on first use into <body>, so this file
 * works on any page that loads it (the nav button, the dashboard).
 */
(function () {
    "use strict";
    window.GM = window.GM || {};

    // Back-forward cache (bfcache) defence. Chrome aggressively keeps a
    // page's JS context alive across Back / Forward navigations, which
    // means the EOD orchestrator's modal element, in-flight fetches,
    // and operatorChoiceResolver would all be restored when the user
    // hits Back after fixing a load. A fresh runMultiLocation call on a
    // bfcache-restored page got confused by the leftover state and the
    // spinner hung at "Looking up locations…". Forcing a reload on
    // restore guarantees every visit starts from a clean DOM.
    window.addEventListener('pageshow', function (evt) {
        if (evt.persisted) {
            window.location.reload();
        }
    });

    var STATE = {
        modalEl: null,
        bsModal: null,
        currentBlobUrl: null,
        operatorChoiceResolver: null,
        // Set true when the summary screen is shown, so that closing the
        // modal navigates the page back to its index — refreshing the
        // dashboard's WS grid so closed weight sheets disappear.
        reloadOnClose: false,
    };

    // ── HTML for the orchestrator modal (built once, reused) ───────────────
    function ensureModal() {
        if (STATE.modalEl) return STATE.modalEl;
        var html =
            '<div class="modal fade" id="gm-eod-modal" tabindex="-1" data-bs-backdrop="static" data-bs-keyboard="false" aria-hidden="true">' +
              '<div class="modal-dialog modal-dialog-centered modal-xl" style="max-width:1100px;">' +
                '<div class="modal-content" style="height:88vh;">' +
                  '<div class="modal-header bg-success text-white">' +
                    '<h5 class="modal-title">End Of Day</h5>' +
                    '<button type="button" class="btn-close btn-close-white" id="gm-eod-cancel-x" aria-label="Cancel"></button>' +
                  '</div>' +
                  '<div class="modal-body p-0 d-flex flex-column" style="overflow:hidden;">' +
                    '<div id="gm-eod-progress" class="px-3 py-2 border-bottom bg-light small d-flex align-items-center" style="gap:10px;">' +
                      '<span id="gm-eod-step-text" class="fw-bold">Preparing…</span>' +
                      '<div class="progress flex-grow-1" style="height:8px;"><div id="gm-eod-step-bar" class="progress-bar bg-success" style="width:0%"></div></div>' +
                    '</div>' +
                    '<div id="gm-eod-stage" class="flex-grow-1 position-relative" style="background:#222;">' +
                      '<div id="gm-eod-spinner" class="position-absolute top-50 start-50 translate-middle text-light text-center">' +
                        '<div class="spinner-border" role="status" style="width:3rem;height:3rem;"></div>' +
                        '<div class="mt-2" id="gm-eod-spinner-text">Working…</div>' +
                      '</div>' +
                      '<iframe id="gm-eod-pdf" style="width:100%;height:100%;border:0;display:none;background:#fff;"></iframe>' +
                      '<div id="gm-eod-audit" class="p-3 bg-white text-dark" style="display:none;height:100%;overflow:auto;"></div>' +
                      '<div id="gm-eod-summary" class="p-3 bg-white text-dark" style="display:none;height:100%;overflow:auto;"></div>' +
                    '</div>' +
                  '</div>' +
                  '<div class="modal-footer" id="gm-eod-footer">' +
                    '<button type="button" class="btn btn-outline-danger me-auto" id="gm-eod-cancel-all">Cancel All</button>' +
                    '<button type="button" class="btn btn-warning" id="gm-eod-skip">Skip This Location</button>' +
                    '<button type="button" class="btn btn-success" id="gm-eod-confirm">Confirm &amp; Continue</button>' +
                    '<button type="button" class="btn btn-secondary" id="gm-eod-close" style="display:none;" data-bs-dismiss="modal">Close</button>' +
                  '</div>' +
                '</div>' +
              '</div>' +
            '</div>';
        var wrap = document.createElement('div');
        wrap.innerHTML = html;
        document.body.appendChild(wrap.firstChild);
        STATE.modalEl = document.getElementById('gm-eod-modal');
        STATE.bsModal = new bootstrap.Modal(STATE.modalEl);

        document.getElementById('gm-eod-confirm').addEventListener('click', function () {
            resolveChoice('confirm');
        });
        document.getElementById('gm-eod-skip').addEventListener('click', function () {
            resolveChoice('skip');
        });
        // Cancel All / X — resolve the pending operator choice (if any)
        // AND always close the modal. Without the explicit hide(), the X
        // did nothing during spinner phases (Preparing / Auditing /
        // Building PDF / Emailing) because no operator resolver was
        // awaiting and the modal was set up with data-bs-backdrop="static"
        // + data-bs-keyboard="false", which disables every other dismiss
        // path.
        document.getElementById('gm-eod-cancel-all').addEventListener('click', function () {
            resolveChoice('cancel');
            STATE.bsModal.hide();
        });
        document.getElementById('gm-eod-cancel-x').addEventListener('click', function () {
            resolveChoice('cancel');
            STATE.bsModal.hide();
        });
        // Audit-row click → cancel the EOD sweep, switch the global location
        // selector to the row's location (so the destination loads page sees
        // the right context), then jump to that weight sheet's loads page.
        // Delegated off the audit container because rows are re-rendered
        // per location.
        document.getElementById('gm-eod-audit').addEventListener('click', function (e) {
            var tr = e.target.closest('tr.gm-eod-audit-row');
            if (!tr) return;
            var wsId = tr.getAttribute('data-ws-id');
            if (!wsId) return;
            var basePath = (tr.getAttribute('data-ws-type') === 'transfer')
                ? '/GrowerDelivery/WeightSheetTransferLoads'
                : '/GrowerDelivery/WeightSheetDeliveryLoads';
            var url = basePath + '?wsId=' + encodeURIComponent(wsId);
            // Disarm the close-time refresh — we're navigating away.
            STATE.reloadOnClose = false;

            var locId = parseInt(tr.getAttribute('data-loc-id'), 10) || 0;
            if (!locId) {
                window.location.href = url;
                return;
            }
            // Persist the location server-side (the loads page reads from
            // the LocationContext cookie via ILocationContext) and update
            // the client-side cookie so the navbar selector and any page
            // helpers (GM.getLocationId) reflect it on the next page load.
            // Navigate even if the select call fails — the loads page will
            // surface a "select a location" message rather than silently
            // misbehave.
            fetch('/api/LocationContextApi/select', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ LocationId: locId })
            }).then(function (r) { return r.ok ? r.json() : null; })
              .then(function (result) {
                  if (result && result.HasLocation && window.GM && typeof GM.setLocationId === 'function') {
                      GM.setLocationId(locId);
                  }
              })
              .catch(function () { /* ignore — fall through to navigate */ })
              .finally(function () { window.location.href = url; });
        });
        // When the modal is dismissed after the summary was shown, send the
        // operator to the warehouse weight-sheets dashboard. EOD work is
        // always summarized there (open WS grid, etc.) — reloading whatever
        // page they happened to launch EOD from (e.g. the home tiles at /)
        // wouldn't show the closed weight sheets. Replace() — not href —
        // so the back button doesn't take them back into the orchestrator.
        STATE.modalEl.addEventListener('hidden.bs.modal', function () {
            if (!STATE.reloadOnClose) return;
            STATE.reloadOnClose = false;
            window.location.replace('/WeightSheets');
        });
        return STATE.modalEl;
    }

    // ── Modal state helpers ────────────────────────────────────────────────
    function hideAllStages() {
        document.getElementById('gm-eod-spinner').style.display = 'none';
        document.getElementById('gm-eod-pdf').style.display = 'none';
        document.getElementById('gm-eod-audit').style.display = 'none';
        document.getElementById('gm-eod-summary').style.display = 'none';
    }
    function showSpinner(text) {
        document.getElementById('gm-eod-spinner-text').textContent = text || 'Working…';
        hideAllStages();
        document.getElementById('gm-eod-spinner').style.display = '';
        setFooterMode('working');
    }
    function showPdf(blobUrl) {
        document.getElementById('gm-eod-pdf').src = blobUrl;
        hideAllStages();
        document.getElementById('gm-eod-pdf').style.display = '';
        setFooterMode('confirm');
    }
    function showAudit(html, mode) {
        var a = document.getElementById('gm-eod-audit');
        a.innerHTML = html;
        hideAllStages();
        a.style.display = '';
        setFooterMode(mode); // 'audit-soft' or 'audit-hard'
    }
    function showSummary(html) {
        var s = document.getElementById('gm-eod-summary');
        s.innerHTML = html;
        hideAllStages();
        s.style.display = '';
        setFooterMode('done');
        // Arm the navigate-on-close behavior. Reset only when the modal is
        // actually hidden, so a Cancel-All path (which also lands on the
        // summary) still triggers the refresh.
        STATE.reloadOnClose = true;
    }
    function setFooterMode(mode) {
        var confirmBtn = document.getElementById('gm-eod-confirm');
        var skipBtn = document.getElementById('gm-eod-skip');
        var cancelAllBtn = document.getElementById('gm-eod-cancel-all');
        var closeBtn = document.getElementById('gm-eod-close');
        if (mode === 'working') {
            confirmBtn.style.display = 'none';
            skipBtn.style.display = 'none';
            cancelAllBtn.style.display = '';
            closeBtn.style.display = 'none';
        } else if (mode === 'confirm') {
            confirmBtn.style.display = '';
            confirmBtn.textContent = 'Confirm & Continue';
            skipBtn.style.display = '';
            cancelAllBtn.style.display = '';
            closeBtn.style.display = 'none';
        } else if (mode === 'audit-soft') {
            // Issues exist but are protein-only (operator can override).
            confirmBtn.style.display = '';
            confirmBtn.textContent = 'Continue Anyway';
            skipBtn.style.display = '';
            cancelAllBtn.style.display = '';
            closeBtn.style.display = 'none';
        } else if (mode === 'audit-hard') {
            // Hard issues (incomplete loads). Operator must Skip or Cancel.
            confirmBtn.style.display = 'none';
            skipBtn.style.display = '';
            cancelAllBtn.style.display = '';
            closeBtn.style.display = 'none';
        } else if (mode === 'done') {
            confirmBtn.style.display = 'none';
            skipBtn.style.display = 'none';
            cancelAllBtn.style.display = 'none';
            closeBtn.style.display = '';
        }
    }
    function setProgress(stepText, pct) {
        document.getElementById('gm-eod-step-text').textContent = stepText;
        document.getElementById('gm-eod-step-bar').style.width = (pct || 0) + '%';
    }

    function awaitOperatorChoice() {
        return new Promise(function (resolve) {
            STATE.operatorChoiceResolver = resolve;
        });
    }
    function resolveChoice(choice) {
        var r = STATE.operatorChoiceResolver;
        STATE.operatorChoiceResolver = null;
        if (r) r(choice);
    }
    function clearBlob() {
        if (STATE.currentBlobUrl) {
            try { URL.revokeObjectURL(STATE.currentBlobUrl); } catch (e) {}
            STATE.currentBlobUrl = null;
        }
    }
    function escapeHtml(s) {
        return String(s == null ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }

    // ── Network ────────────────────────────────────────────────────────────
    //
    // Per-fetch timeout via AbortController. Without this a hanging
    // server endpoint left the EOD modal spinning forever — the operator
    // had no way out except to restart the browser. With it, a stalled
    // endpoint trips the timeout, the chain rejects, and the outer
    // .catch in runMultiLocation surfaces "End Of Day failed: …" so the
    // operator can see what hung and retry. Keep the cap generous (45s)
    // because the PDF endpoint touches DevExpress rendering on the server
    // and can legitimately take 20+ seconds on a large day.
    var FETCH_TIMEOUT_MS = 45000;
    function fetchWithTimeout(url, init, timeoutMs) {
        var ctrl = new AbortController();
        var t = setTimeout(function () { ctrl.abort(); }, timeoutMs || FETCH_TIMEOUT_MS);
        var opts = Object.assign({}, init || {}, { signal: ctrl.signal });
        return fetch(url, opts).finally(function () { clearTimeout(t); })
            .catch(function (err) {
                if (err && err.name === 'AbortError') {
                    throw new Error('Request timed out: ' + url);
                }
                throw err;
            });
    }

    function fetchPdf(locationId) {
        return fetchWithTimeout('/api/printjobs/eod/' + encodeURIComponent(locationId) + '/pdf')
            .then(function (r) {
                if (!r.ok) {
                    return r.text().then(function (t) {
                        throw new Error('HTTP ' + r.status + ': ' + (t || r.statusText));
                    });
                }
                return r.blob();
            });
    }
    function finalize(locationId, pin) {
        return fetchWithTimeout('/api/printjobs/eod/' + encodeURIComponent(locationId) + '/finalize', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ Pin: pin })
        }).then(function (r) {
            if (!r.ok) {
                return r.text().then(function (t) {
                    throw new Error('HTTP ' + r.status + ': ' + (t || r.statusText));
                });
            }
            return r.json();
        });
    }
    function fetchAccessible() {
        return fetchWithTimeout('/api/LocationContextApi/available')
            .then(function (r) { return r.ok ? r.json() : []; });
    }
    function fetchAudit(locationId) {
        return fetchWithTimeout('/api/GrowerDelivery/EndOfDayCheck?locationId=' + encodeURIComponent(locationId))
            .then(function (r) {
                if (!r.ok) {
                    return r.text().then(function (t) {
                        throw new Error('HTTP ' + r.status + ': ' + (t || r.statusText));
                    });
                }
                return r.json();
            });
    }
    // Build the per-location audit display. softOnly=true means every issue
    // is "No Protein Set" — operator can override and continue. Otherwise
    // there's at least one incomplete load and the operator must skip or
    // cancel (handled by setFooterMode('audit-hard') hiding the confirm btn).
    function renderAuditTable(rows, locId, locName, softOnly) {
        var bodyRows = rows.map(function (r) {
            var status = r.Status || '';
            var rowClass = 'gm-eod-audit-row';
            if (status.indexOf('Not Complete') >= 0 && status.indexOf('No Protein') >= 0) {
                rowClass += ' table-danger';
            } else if (status === 'Not Complete') {
                rowClass += ' table-warning';
            } else if (status === 'No Protein Set') {
                rowClass += ' table-info';
            }
            // data-ws-id / data-ws-type / data-loc-id drive the row-click
            // navigation in ensureModal — clicking a row cancels the EOD
            // sweep, switches the global location selector to this row's
            // location, and jumps to the weight sheet's loads page so the
            // operator can fix the issue without unwinding through the modal.
            return '<tr class="' + rowClass + '" style="cursor:pointer;"' +
                ' data-ws-id="' + escapeHtml(r.WeightSheetId || '') + '"' +
                ' data-ws-type="' + escapeHtml((r.WeightSheetType || '').toLowerCase()) + '"' +
                ' data-loc-id="' + escapeHtml(locId || '') + '"' +
                ' title="Click to open this weight sheet (cancels End Of Day)">' +
                '<td>' + escapeHtml(r.WeightSheetIdDisplay || r.WeightSheetId || '') + '</td>' +
                '<td>' + escapeHtml(status) + '</td>' +
                '<td>' + escapeHtml(r.WeightSheetType || '') + '</td>' +
                '<td>' + escapeHtml(r.LotIdDisplay || '') + '</td>' +
                '<td>' + escapeHtml(r.LotDescription || '') + '</td>' +
                '<td class="text-end">' + (r.TotalLoads || 0) + '</td>' +
                '<td class="text-end">' + (r.IncompleteLoadCount || 0) + '</td>' +
                '<td class="text-end">' + (r.MissingProteinCount || 0) + '</td>' +
            '</tr>';
        }).join('');
        var alertHtml = softOnly
            ? '<div class="alert alert-warning mb-3">Protein has not been entered for some loads. ' +
              'Once these weight sheets close, protein can no longer be edited &mdash; ' +
              'click a row to open that weight sheet and enter protein now, ' +
              '<strong>Skip This Location</strong> to come back to it later, ' +
              'or <strong>Continue Anyway</strong> to close without protein. ' +
              '<strong>Cancel All</strong> to stop the End Of Day run.</div>'
            : '<div class="alert alert-danger mb-3">The following weight sheets have <strong>incomplete loads</strong> ' +
              'that must be finished before this location can be closed. ' +
              '<strong>Skip This Location</strong> to continue with the rest, <strong>Cancel All</strong> to stop, ' +
              'or click a row to open that weight sheet.</div>';
        return '<h5 class="mb-3">' + escapeHtml(locName) + ' &mdash; Weight Sheet Audit</h5>' +
            alertHtml +
            '<div class="table-responsive">' +
                '<table class="table table-sm table-bordered align-middle mb-0">' +
                    '<thead class="table-light"><tr>' +
                        '<th>WS #</th><th>Status</th><th>Type</th>' +
                        '<th>Lot #</th><th>Lot</th>' +
                        '<th class="text-end">Loads</th>' +
                        '<th class="text-end">Incomplete</th>' +
                        '<th class="text-end">No Protein</th>' +
                    '</tr></thead>' +
                    '<tbody>' + bodyRows + '</tbody>' +
                '</table>' +
            '</div>';
    }
    function fetchCandidates(locationIds) {
        if (!locationIds.length) return Promise.resolve([]);
        return fetchWithTimeout('/api/printjobs/eod/candidates?ids=' + locationIds.join(','))
            .then(function (r) { return r.ok ? r.json() : []; });
    }
    function fetchCurrentLocationId() {
        // The /current endpoint serializes PascalCase, matching the rest
        // of the LocationContextApi surface. The same is true for
        // /available and /eod/candidates — every property read from
        // those payloads in this file is PascalCase by design.
        return fetchWithTimeout('/api/LocationContextApi/current')
            .then(function (r) { return r.ok ? r.json() : null; })
            .then(function (j) { return j && j.HasLocation ? j.LocationId : 0; })
            .catch(function () { return 0; });
    }

    // ── Per-location step ──────────────────────────────────────────────────
    // Runs the audit→PDF preview→finalize sequence for one location, using
    // a PIN supplied by the caller (collected once at the top of the multi-
    // location loop). Returns a result row used in the final summary.
    // Candidate properties are PascalCase — see fetchCandidates.
    //
    // Choice values from the orchestrator buttons are normalized:
    //   'confirm' →  proceed (PDF if from audit step, finalize if from PDF step)
    //   'skip'    →  status 'skipped'
    //   'cancel'  →  status 'cancelled' — caller stops the sweep on this
    function runOneLocation(cand, idx, total, pin) {
        var locId = cand.LocationId;
        var label = (cand.Name || ('Location ' + locId));
        var openCount = cand.OpenCount || 0;
        function rowOf(extra) {
            return Object.assign({
                locationId: locId, name: label, openCount: openCount
            }, extra);
        }

        // Step 1: audit. Cheap DB query — fail fast on issues so the
        // operator doesn't wait through PDF generation only to discover
        // the location can't be closed.
        setProgress('Location ' + idx + ' of ' + total + ': ' + label
            + ' — checking weight sheets…', Math.round((idx - 1) / total * 100));
        showSpinner('Auditing weight sheets at ' + label + '…');

        return fetchAudit(locId).then(function (rows) {
            rows = rows || [];
            if (rows.length === 0) return null; // clean — skip the audit display
            var softOnly = rows.every(function (r) {
                return (r.Status || '') === 'No Protein Set';
            });
            showAudit(renderAuditTable(rows, locId, label, softOnly), softOnly ? 'audit-soft' : 'audit-hard');
            setProgress('Location ' + idx + ' of ' + total + ': ' + label
                + ' — review audit', Math.round((idx - 0.85) / total * 100));
            return awaitOperatorChoice();
        }).then(function (auditChoice) {
            // null = clean (no audit shown), 'confirm' = soft override.
            // Both proceed to PDF.
            if (auditChoice === 'skip')   return rowOf({ status: 'skipped' });
            if (auditChoice === 'cancel') return rowOf({ status: 'cancelled' });

            // Step 2: PDF preview.
            setProgress('Location ' + idx + ' of ' + total + ': ' + label
                + ' — generating PDF…', Math.round((idx - 0.6) / total * 100));
            showSpinner('Building EOD PDF for ' + label + '…');
            return fetchPdf(locId).then(function (blob) {
                clearBlob();
                STATE.currentBlobUrl = URL.createObjectURL(blob);
                setProgress('Location ' + idx + ' of ' + total + ': ' + label
                    + ' — review and confirm', Math.round((idx - 0.4) / total * 100));
                showPdf(STATE.currentBlobUrl);
                return awaitOperatorChoice();
            }).then(function (choice) {
                if (choice === 'skip')   return rowOf({ status: 'skipped' });
                if (choice === 'cancel') return rowOf({ status: 'cancelled' });

                // Step 3: finalize.
                setProgress('Location ' + idx + ' of ' + total + ': ' + label
                    + ' — emailing & closing…', Math.round((idx - 0.2) / total * 100));
                showSpinner('Emailing weight sheets and closing at ' + label + '…');
                return finalize(locId, pin).then(function (resp) {
                    return rowOf({
                        status: 'done',
                        closed: resp.closed || 0,
                        emailed: resp.emailed || 0,
                        failures: resp.failures || []
                    });
                }).catch(function (err) {
                    return rowOf({
                        status: 'error',
                        error: (err && err.message) ? err.message : String(err)
                    });
                });
            });
        }).catch(function (err) {
            return rowOf({
                status: 'error',
                error: (err && err.message) ? err.message : String(err)
            });
        });
    }

    // ── Summary renderer ───────────────────────────────────────────────────
    function renderSummary(results, startingLocationId) {
        var rows = results.map(function (r) {
            var badgeClass, badgeText;
            switch (r.status) {
                case 'done':      badgeClass = 'bg-success'; badgeText = 'Closed'; break;
                case 'skipped':   badgeClass = 'bg-secondary'; badgeText = 'Skipped'; break;
                case 'cancelled': badgeClass = 'bg-warning text-dark'; badgeText = 'Cancelled'; break;
                case 'error':     badgeClass = 'bg-danger';  badgeText = 'Error'; break;
                case 'no-loads':  badgeClass = 'bg-info text-dark'; badgeText = 'No Loads'; break;
                default:          badgeClass = 'bg-light text-dark'; badgeText = r.status || '?'; break;
            }
            var detail = '';
            if (r.status === 'done') {
                detail = 'closed ' + (r.closed || 0) + ', emailed ' + (r.emailed || 0);
                if (r.failures && r.failures.length) {
                    detail += '<br><span class="text-danger small">' +
                        r.failures.map(escapeHtml).join('<br>') + '</span>';
                }
            } else if (r.status === 'error') {
                detail = '<span class="text-danger small">' + escapeHtml(r.error || '') + '</span>';
            } else if (r.status === 'no-loads') {
                detail = '<span class="text-muted small">no open weight sheets &mdash; nothing to close</span>';
            }
            var startMark = (r.locationId === startingLocationId) ? ' <em class="small text-muted">(starting location)</em>' : '';
            return '<tr>' +
                '<td>' + escapeHtml(r.name) + startMark + '</td>' +
                '<td>' + (r.openCount || 0) + '</td>' +
                '<td><span class="badge ' + badgeClass + '">' + badgeText + '</span></td>' +
                '<td>' + detail + '</td>' +
            '</tr>';
        }).join('');
        return '<h5 class="mb-3">End Of Day Summary</h5>' +
            '<table class="table table-sm table-striped">' +
              '<thead><tr><th>Location</th><th>Open WSs</th><th>Status</th><th>Detail</th></tr></thead>' +
              '<tbody>' + rows + '</tbody>' +
            '</table>' +
            '<p class="text-muted small mb-0">Returned to starting location.</p>';
    }

    // Reset any leftover state from a previous run. Called at the very
    // top of runSingle/runMultiLocation so a page restored from the
    // browser's back-forward cache (which preserves JS state across
    // back-button navigations) doesn't carry forward a stale resolver
    // or blob URL from the prior run that the operator never finished.
    //
    // Also nukes any orphaned Bootstrap modal-backdrop elements and
    // strips the modal-open class from <body>. A previously-failed run
    // can leave these behind (especially when the EOD modal was open
    // and the page navigated away mid-flight via the audit-row click).
    // Without the cleanup, Bootstrap's next .show() call thinks a modal
    // is still open, refuses to show a fresh backdrop, and the PIN
    // dialog ends up hidden behind the stale backdrop.
    function resetState() {
        STATE.operatorChoiceResolver = null;
        STATE.reloadOnClose = false;
        clearBlob();
        try {
            document.querySelectorAll('.modal-backdrop').forEach(function (b) {
                // Only sweep orphans — backdrops attached to a visible
                // modal stay put.
                if (!document.querySelector('.modal.show')) b.remove();
            });
            if (!document.querySelector('.modal.show')) {
                document.body.classList.remove('modal-open');
                document.body.style.removeProperty('overflow');
                document.body.style.removeProperty('padding-right');
            }
        } catch (e) { /* defensive — never let cleanup throw */ }
    }

    // ── Public: single-location flow ───────────────────────────────────────
    //
    // Order intentionally puts PIN entry BEFORE opening the EOD modal so
    // the two modals never stack — the previous design wedged a small
    // PIN dialog on top of the large EOD modal and a single off-by-one
    // z-index or orphan backdrop made the PIN invisible. Now the PIN
    // modal is the only modal on screen; once it resolves, the EOD
    // modal opens with the answer already in hand.
    function runSingle(locationId) {
        if (!locationId) return Promise.reject(new Error('locationId required'));
        resetState();
        console.log('[GM.eod] runSingle: starting (locationId=' + locationId + ')');

        // Silent candidate fetch — no spinner yet because no modal is
        // open. The per-fetch timeout (45s) will surface an error if
        // the server hangs.
        return fetchCandidates([locationId]).then(function (cs) {
            var cand = (cs && cs.length)
                ? cs[0]
                : { LocationId: locationId, Name: 'Location ' + locationId, OpenCount: 0 };

            if ((cand.OpenCount || 0) === 0) {
                // No work — just show a quick summary modal, no PIN.
                ensureModal();
                STATE.bsModal.show();
                showSummary('<div class="alert alert-success mb-0">No open weight sheets to close at '
                    + escapeHtml(cand.Name) + '.</div>');
                return null;
            }

            // PIN FIRST (no EOD modal open yet → no stacking).
            console.log('[GM.eod] runSingle: requesting PIN');
            return GM.requestPin({
                title: 'Confirm End Of Day',
                prompt: 'Enter your PIN to email weight sheets and close them at ' + cand.Name + '.',
                forcePrompt: true
            }).then(function (pinRes) {
                // PIN accepted — NOW open the EOD modal and run the loop.
                console.log('[GM.eod] runSingle: PIN accepted, opening EOD modal');
                ensureModal();
                STATE.bsModal.show();
                return runOneLocation(cand, 1, 1, pinRes.pin);
            }, function (err) {
                if (err && err.message === 'cancelled') {
                    console.log('[GM.eod] runSingle: PIN cancelled');
                    return null;
                }
                throw err;
            }).then(function (row) {
                if (!row) return null;
                showSummary(renderSummary([row], locationId));
                clearBlob();
                return row;
            });
        }).catch(function (err) {
            console.error('[GM.eod] runSingle chain rejected:', err);
            // EOD modal may not be open — fall back to alert if not.
            if (document.getElementById('gm-eod-modal')
                && document.getElementById('gm-eod-modal').classList.contains('show')) {
                showSummary('<div class="alert alert-danger mb-0"><strong>End Of Day failed.</strong><br>'
                    + escapeHtml(err.message || String(err)) + '</div>');
            } else {
                alert('End Of Day failed: ' + (err.message || err));
            }
        });
    }

    // ── Public: multi-location flow ────────────────────────────────────────
    //
    // PIN is requested BEFORE the EOD modal opens (after a silent
    // candidate fetch so the prompt can name the actual queue size).
    // The PIN and EOD modals never stack — eliminates every z-index /
    // orphan-backdrop edge case that used to silently hide the PIN.
    function runMultiLocation(opts) {
        opts = opts || {};
        resetState();

        var startingId = opts.startingLocationId || 0;
        console.log('[GM.eod] runMultiLocation: starting (startingId=' + startingId + ')');

        // Silent (no-modal) fetch chain. The per-fetch 45s timeout
        // surfaces any hang as an alert below.
        var startPromise = startingId
            ? Promise.resolve(startingId)
            : fetchCurrentLocationId();

        return startPromise.then(function (startId) {
            startingId = startId || 0;
            console.log('[GM.eod] /current resolved → startingId=' + startingId);
            return fetchAccessible().then(function (locs) {
                var ids = (locs || []).map(function (l) { return l.LocationId; });
                if (startingId && ids.indexOf(startingId) < 0) ids.unshift(startingId);
                console.log('[GM.eod] /available resolved → ids=' + ids.join(','));
                return fetchCandidates(ids);
            });
        }).then(function (candidates) {
            console.log('[GM.eod] /candidates resolved →', candidates);

            var byId = {};
            candidates.forEach(function (c) { byId[c.LocationId] = c; });
            var queue = [];
            if (startingId && byId[startingId] && (byId[startingId].OpenCount || 0) > 0) {
                queue.push(byId[startingId]);
            }
            candidates.forEach(function (c) {
                if (c.LocationId !== startingId && (c.OpenCount || 0) > 0) queue.push(c);
            });

            var noLoadResults = [];
            candidates.forEach(function (c) {
                if ((c.OpenCount || 0) === 0) {
                    noLoadResults.push({
                        locationId: c.LocationId,
                        name: c.Name || ('Location ' + c.LocationId),
                        openCount: 0,
                        status: 'no-loads'
                    });
                }
            });

            console.log('[GM.eod] queue.length=' + queue.length
                + ', noLoadResults.length=' + noLoadResults.length);

            if (queue.length === 0) {
                // No work — open the EOD modal just to render the
                // summary so the operator sees confirmation. No PIN
                // needed because nothing is being closed.
                ensureModal();
                STATE.bsModal.show();
                if (noLoadResults.length) {
                    showSummary(renderSummary(noLoadResults, startingId));
                } else {
                    showSummary('<div class="alert alert-info mb-0">No accessible locations found.</div>');
                }
                return;
            }

            // PIN FIRST — no EOD modal open yet, so the PIN is the only
            // modal on screen. Once it resolves we open the EOD modal
            // and start the per-location loop.
            console.log('[GM.eod] requesting PIN for ' + queue.length + ' location(s)');
            return GM.requestPin({
                title: 'Confirm End Of Day',
                prompt: 'Enter your PIN to email weight sheets and close them at '
                    + queue.length + ' location' + (queue.length === 1 ? '' : 's')
                    + ' with open weight sheets.',
                forcePrompt: true
            }).then(function (pinRes) {
                var pin = pinRes.pin;
                console.log('[GM.eod] PIN accepted, opening EOD modal');
                ensureModal();
                STATE.bsModal.show();
                setProgress('Preparing…', 0);

                var results = [];
                var p = Promise.resolve();
                var cancelled = false;
                queue.forEach(function (cand, idx) {
                    p = p.then(function () {
                        if (cancelled) return null;
                        return runOneLocation(cand, idx + 1, queue.length, pin);
                    }).then(function (row) {
                        if (!row) return;
                        results.push(row);
                        if (row.status === 'cancelled') cancelled = true;
                    });
                });
                return p.then(function () {
                    clearBlob();
                    showSummary(renderSummary(results.concat(noLoadResults), startingId));
                });
            }, function (err) {
                // PIN cancelled — there's no EOD modal open, so nothing
                // to hide. Any other PIN error bubbles to the outer
                // .catch as a failed sweep.
                if (err && err.message === 'cancelled') {
                    console.log('[GM.eod] PIN cancelled');
                    return;
                }
                throw err;
            });
        }).catch(function (err) {
            console.error('[GM.eod] runMultiLocation chain rejected:', err);
            var msg = err && err.message ? err.message : String(err);
            // EOD modal may not be open if the failure happened during
            // the silent fetch phase — fall back to an alert.
            if (document.getElementById('gm-eod-modal')
                && document.getElementById('gm-eod-modal').classList.contains('show')) {
                showSummary('<div class="alert alert-danger mb-0"><strong>End Of Day failed.</strong><br>'
                    + escapeHtml(msg) + '</div>');
            } else {
                alert('End Of Day failed: ' + msg);
            }
        });
    }

    // ── Public: prompt to start because of stale prior-day WSs ─────────────
    function promptForStaleStart(locationId, count) {
        if (!locationId || !count) return;
        // One-shot guard so a SignalR reconnect / manual refresh doesn't
        // re-prompt within the same session.
        var key = 'gm.eod.staleAck.' + locationId;
        try {
            if (window.sessionStorage.getItem(key) === '1') return;
        } catch (e) {}
        var msg = count + ' open weight sheet' + (count === 1 ? '' : 's')
            + ' from a previous day ' + (count === 1 ? 'is' : 'are')
            + ' still open at this location. Run End Of Day now?';
        if (!window.confirm(msg)) {
            try { window.sessionStorage.setItem(key, '1'); } catch (e) {}
            return;
        }
        try { window.sessionStorage.setItem(key, '1'); } catch (e) {}
        runMultiLocation({ startingLocationId: locationId });
    }

    window.GM.eod = {
        runSingle: runSingle,
        runMultiLocation: runMultiLocation,
        promptForStaleStart: promptForStaleStart
    };
})();
