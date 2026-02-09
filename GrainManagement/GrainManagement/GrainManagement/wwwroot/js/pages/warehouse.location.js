(function () {
    "use strict";

    const cookieLocation = "GM.SelectedWarehouseLocationId";
    const selectId = "gmLocationSelect";
    const noLocationId = "gmNoLocationSet";

    const endpoint = "/api/locations/WarehouseLocationsList";

    function setCookie(name, value, days) {
        const d = new Date();
        d.setTime(d.getTime() + (days * 24 * 60 * 60 * 1000));
        document.cookie = `${name}=${encodeURIComponent(value)}; expires=${d.toUTCString()}; path=/; SameSite=Lax`;
    }

    function getCookie(name) {
        const m = document.cookie.match(new RegExp("(^| )" + name + "=([^;]+)"));
        return m ? decodeURIComponent(m[2]) : null;
    }

    function clearCookie(name) {
        document.cookie = `${name}=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/; SameSite=Lax`;
    }

    function showNoLocation(show) {
        const el = document.getElementById(noLocationId);
        if (el) el.style.display = show ? "block" : "none";
    }

    async function fetchLocations() {
        const resp = await fetch(endpoint, { headers: { "Accept": "application/json" } });
        if (!resp.ok) throw new Error("Failed to load warehouse locations");
        return await resp.json();
    }

    function normalize(list) {
        return (list || []).map(x => ({
            id: x.LocationId ?? x.locationId ?? x.Id ?? x.id,
            name: x.Name ?? x.name ?? "",
            district: x.District ?? x.district ?? ""
        })).filter(x => x.id != null);
    }

    function render(selectEl, locations, hasSavedSelection) {
        selectEl.innerHTML = "";

        if (!hasSavedSelection) {
            const warn = document.createElement("option");
            warn.value = "";
            warn.textContent = "⚠ Please select a location";
            warn.disabled = true;
            warn.selected = true;
            selectEl.appendChild(warn);
            selectEl.classList.add("gm-loc-select--required");
        } else {
            selectEl.classList.remove("gm-loc-select--required");
        }

        for (const loc of locations) {
            const opt = document.createElement("option");
            opt.value = String(loc.id);
            opt.textContent = `${loc.id} – ${loc.name}${loc.district ? " (" + loc.district + ")" : ""}`;
            selectEl.appendChild(opt);
        }
    }

    async function init() {
        const selectEl = document.getElementById(selectId);
        if (!selectEl) return;

        // Default locked until we validate cookie against actual location list
        showNoLocation(false);

        let locations;
        try {
            locations = normalize(await fetchLocations());
        } catch (e) {
            console.error(e);
            selectEl.innerHTML = `<option value="">Error loading locations</option>`;
            selectEl.disabled = true;
            clearCookie(cookieLocation);
            showNoLocation(true);
            document.body.classList.add("gm-js-ready");
            return;
        }

        if (!locations.length) {
            selectEl.innerHTML = `<option value="">No warehouse locations</option>`;
            selectEl.disabled = true;
            clearCookie(cookieLocation);
            showNoLocation(true);
            document.body.classList.add("gm-js-ready");
            return;
        }

        const saved = getCookie(cookieLocation);
        const exists = saved && locations.some(l => String(l.id) === String(saved));

        if (!exists) clearCookie(cookieLocation);

        render(selectEl, locations, !!exists);

        if (exists) {
            selectEl.value = saved;
            showNoLocation(false);
        } else {
            showNoLocation(true);
        }

        selectEl.addEventListener("change", () => {
            const val = selectEl.value;
            if (!val) return;

            setCookie(cookieLocation, val, 365);
            selectEl.classList.remove("gm-loc-select--required");
            showNoLocation(false);

            // Let other scripts react (warehouse.index.js listens if you want to add it later)
            window.dispatchEvent(new CustomEvent("gm:locationChanged", { detail: { locationId: val } }));
        });

        // Anti-flash guard
        document.body.classList.add("gm-js-ready");
    }

    document.addEventListener("DOMContentLoaded", () => {
        init().catch(console.error);
    });
})();
