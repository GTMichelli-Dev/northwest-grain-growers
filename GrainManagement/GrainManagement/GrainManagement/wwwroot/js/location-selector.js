// Location Selector — populates dropdown and handles location switching
(function () {
    "use strict";

    var dropdown = document.getElementById("gm-location-select");
    var currentLabel = document.getElementById("gm-location-current");
    if (!dropdown) return;

    // Load available locations into dropdown
    fetch("/api/LocationContextApi/available")
        .then(function (r) { return r.json(); })
        .then(function (locations) {
            locations.forEach(function (loc) {
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
                dropdown.value = current.LocationId;
                // Sync to global cookie so all pages can read it
                if (window.GM && GM.setLocationId) GM.setLocationId(current.LocationId);
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
