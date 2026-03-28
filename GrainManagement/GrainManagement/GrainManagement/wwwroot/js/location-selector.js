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
                // Only one location available — auto-select it
                fetch("/api/LocationContextApi/select", {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({ LocationId: availableIds[0] })
                })
                .then(function (r) { return r.json(); })
                .then(function (result) {
                    if (result.HasLocation) {
                        if (window.GM && GM.setLocationId) GM.setLocationId(availableIds[0]);
                        window.location.reload();
                    }
                });
            }
        })
        .catch(function (err) {
            console.warn("Location selector error:", err);
        });

    // Handle location change
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
                // Sync to global cookie before reload
                if (window.GM && GM.setLocationId) GM.setLocationId(locId);
                // Reload page to reflect new location capabilities
                window.location.reload();
            }
        })
        .catch(function (err) {
            console.error("Failed to select location:", err);
        });
    });
})();
