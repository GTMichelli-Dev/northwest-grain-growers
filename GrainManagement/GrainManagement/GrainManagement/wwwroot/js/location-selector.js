// Location Selector — populates dropdown and handles location switching
(function () {
    "use strict";

    var dropdown = document.getElementById("gm-location-select");
    var currentLabel = document.getElementById("gm-location-current");
    if (!dropdown) return;

    var availableIds = [];

    // Load available locations into dropdown
    fetch("/api/LocationContextApi/available")
        .then(function (r) { return r.json(); })
        .then(function (locations) {
            locations.forEach(function (loc) {
                availableIds.push(loc.LocationId);
                var opt = document.createElement("option");
                opt.value = loc.LocationId;
                opt.textContent = loc.Name + " (" + loc.LocationId + ")";
                opt.dataset.warehouse = loc.UseForWarehouse;
                opt.dataset.seed = loc.UseForSeed;
                dropdown.appendChild(opt);
            });

            // Now load current selection
            return fetch("/api/LocationContextApi/current");
        })
        .then(function (r) { return r.json(); })
        .then(function (current) {
            if (current.HasLocation) {
                // If current location is not in the allowed list, clear it and go home
                if (availableIds.indexOf(current.LocationId) === -1) {
                    fetch("/api/LocationContextApi/clear", { method: "POST" })
                        .then(function () { window.location.href = "/"; });
                    return;
                }
                dropdown.value = current.LocationId;
                // Sync to global cookie so all pages can read it
                if (window.GM && GM.setLocationId) GM.setLocationId(current.LocationId);
            } else if (availableIds.length === 1) {
                // Only one location available — auto-select it. After the
                // cookie is set, send the user to the home page so every
                // module's nav reflects the new location's capabilities
                // from a clean slate.
                fetch("/api/LocationContextApi/select", {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({ LocationId: availableIds[0] })
                })
                .then(function (r) { return r.json(); })
                .then(function (result) {
                    if (result.HasLocation) {
                        if (window.GM && GM.setLocationId) GM.setLocationId(availableIds[0]);
                        window.location.href = "/";
                    }
                });
            }
        })
        .catch(function (err) {
            console.warn("Location selector error:", err);
        });

    // Handle location change. After switching, we send the operator to
    // the home page rather than reloading in place — the new location
    // may not even support the page they were on (e.g. a Seed-only site
    // when they were on the warehouse dashboard), and the index page is
    // the canonical landing for whatever modules the new site enables.
    dropdown.addEventListener("change", function () {
        var locId = parseInt(dropdown.value, 10);
        if (!locId) return;

        fetch("/api/LocationContextApi/select", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ LocationId: locId })
        })
        .then(function (r) { return r.json(); })
        .then(function (result) {
            if (result.HasLocation) {
                if (window.GM && GM.setLocationId) GM.setLocationId(locId);
                window.location.href = "/";
            }
        })
        .catch(function (err) {
            console.error("Failed to select location:", err);
        });
    });
})();
