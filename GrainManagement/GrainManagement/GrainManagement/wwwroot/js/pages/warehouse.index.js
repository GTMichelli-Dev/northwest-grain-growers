(function () {
    "use strict";
    let activeMode = null;

    const cookieLocation = "GM.SelectedWarehouseLocationId";
    const cookieMode = "GM.WarehouseMode";

    const ids = {
        noLocation: "gmNoLocationSet",
        noMode: "gmNoModeSet",
        shell: "gmWsShell"
    };

  const modePartials = {
    intake: "/Warehouse/ModePartial?mode=intake",
    transfer: "/Warehouse/ModePartial?mode=transfer",
    outbound: "/Warehouse/ModePartial?mode=outbound",
    kiosk: "/Warehouse/KioskCamera",
    newtruck: "/Warehouse/ModePartial?mode=newtruck"
};

const modeScripts = {
    intake: "/js/pages/warehouse.intake.js",
    transfer: "/js/pages/warehouse.transfer.js",
    outbound: "/js/pages/warehouse.outbound.js",
    kiosk: "/js/pages/warehouse.kiosk.js",
    newtruck: "/js/pages/warehouse.newtruck.js"
};


    function getCookie(name) {
        const m = document.cookie.match(new RegExp("(^| )" + name + "=([^;]+)"));
        return m ? decodeURIComponent(m[2]) : null;
    }

    function setCookie(name, value, days) {
        const d = new Date();
        d.setTime(d.getTime() + (days * 24 * 60 * 60 * 1000));
        document.cookie = `${name}=${encodeURIComponent(value)}; expires=${d.toUTCString()}; path=/; SameSite=Lax`;
    }

    function show(id, yes) {
        const el = document.getElementById(id);
        if (el) el.style.display = yes ? "block" : "none";
    }

    function setActiveModeButton(mode) {
        document.querySelectorAll(".gm-modebtn").forEach(b => {
            b.classList.toggle("is-active", b.dataset.mode === mode);
        });
    }

    function hasLocation() {
        return !!getCookie(cookieLocation);
    }

    async function loadPartial(mode) {
        const url = modePartials[mode];
        if (!url) return;

        const shell = document.getElementById(ids.shell);
        if (!shell) return;

        const resp = await fetch(url, { headers: { "X-Requested-With": "XMLHttpRequest" } });
        if (!resp.ok) throw new Error(`Failed to load partial for mode: ${mode}`);
        shell.innerHTML = await resp.text();
    }

   function loadScriptOnce(url) {
     return new Promise((resolve, reject) => {

        const base = new URL(url, location.origin).href;

        const isDev =
            location.hostname === "localhost" ||
            location.hostname === "127.0.0.1";

        const finalUrl = isDev
            ? base + (base.includes("?") ? "&" : "?") + "v=" + Date.now()
            : base;

        // In prod, prevent duplicate loading
        if (!isDev &&
            [...document.scripts].some(s => s.src === base)) {
            return resolve();
        }
        else
        {
            console.log(`Loading script: ${finalUrl}`);
        }

        const s = document.createElement("script");
        s.src = finalUrl;
        s.async = true;
        s.onload = () => resolve();
        s.onerror = () => reject(new Error("Failed to load " + url));
        document.body.appendChild(s);
      });
    }


    async function initMode(mode) {
        

        if (activeMode === mode) {
            const mod = window.gmWarehouseModeInit?.[mode];
            mod?.ensureInitialized?.();
            mod?.refresh?.();
            return;
        }


    

        if (!hasLocation()) {
            show(ids.noLocation, true);
            show(ids.noMode, false);
            show(ids.shell, false);
            setActiveModeButton(null);
            return;
        }

        show(ids.noLocation, false);
        show(ids.noMode, false);

        setActiveModeButton(mode);
        // Do not persist transient modes
        const persistable = (mode !== "newtruck");
        if (persistable) {
          setCookie(cookieMode, mode, 365);
        } else {
          // Optional: force cookie back to intake while viewing transient mode
          setCookie(cookieMode, "intake", 365);
        }

        // hide shell while loading
        show(ids.shell, false);

        await loadPartial(mode);

        // show after loaded
        show(ids.shell, true);

        window.gmWarehouseModeInit = window.gmWarehouseModeInit || {};
        window.gmWarehouseModeInit = window.gmWarehouseModeInit || {};
        const alreadyLoaded = !!window.gmWarehouseModeInit?.[mode];

        const jsUrl = modeScripts[mode];
        if (jsUrl && !alreadyLoaded) {
          await loadScriptOnce(jsUrl);
        }


        const mod = window.gmWarehouseModeInit[mode];

        // Support either "function mode init" OR "object with ensureInitialized/refresh"
        if (typeof mod === "function") {
            mod();
        } else if (mod && typeof mod.ensureInitialized === "function") {
            mod.ensureInitialized();
            if (typeof mod.refresh === "function") mod.refresh();
        }
        activeMode = mode;
    }

    window.gmWarehouse = window.gmWarehouse || {};
    window.gmWarehouse.initMode = initMode;


    function wireButtons() {
        const container = document.querySelector(".gm-pagehead__modes");
        if (!container) return;

        container.addEventListener("click", (e) => {
            const btn = e.target.closest(".gm-modebtn");
            if (!btn) return;

            e.preventDefault();
            e.stopPropagation();

            initMode(btn.dataset.mode).catch(console.error);
        });
    }


    function bootDefault() {
        // If location not set, show location message only
        if (!hasLocation()) {
            show(ids.noLocation, true);
            show(ids.noMode, false);
            show(ids.shell, false);
            return;
        }

        // location exists but no mode yet
        const savedMode = (getCookie(cookieMode) || "").toLowerCase();
        const isValid = !!modePartials[savedMode];
        if (!isValid) {
            show(ids.noLocation, false);
            show(ids.noMode, true);
            show(ids.shell, false);
            setActiveModeButton(null);
            return;
        }

     

        // restore last mode
        initMode(savedMode).catch(console.error);
    }

    document.addEventListener("DOMContentLoaded", () => {
        wireButtons();
        bootDefault();
    });
})();
