(function () {
    "use strict";

    const LOCATION_STORAGE_KEY = 'gm_location_id';
    const BTN_WIDTH = 175;

    // Returns the integer locationId from the current URL's ?locationId= param, or null.
    function getQueryLocationId() {
        const params = new URLSearchParams(window.location.search);
        const v = parseInt(params.get('locationId') || '0', 10);
        return v || null;
    }

    // Initialise the location SelectBox in the pagehead.
    // On change: persist to localStorage and reload the page with ?locationId=X.
    async function initLocationPicker() {
        const currentId = getQueryLocationId();

        let locations = [];
        try {
            locations = await $.getJSON('/api/locations/WarehouseLocationsList');
        } catch (ex) {
            console.warn('[WarehouseDashboard] Location prefetch failed', ex);
        }

        $('#wdLocation').dxSelectBox({
            dataSource:   locations,
            valueExpr:    'LocationId',
            displayExpr:  function (item) { return item ? item.Name + ' \u2013 ' + item.LocationId : ''; },
            searchEnabled: true,
            placeholder:  'Select location\u2026',
            width:        'auto',
            value:        currentId,
            onValueChanged: function (e) {
                const newId = e.value || null;
                if (newId) {
                    localStorage.setItem(LOCATION_STORAGE_KEY, String(newId));
                    window.location.href = '/Warehouse?locationId=' + newId;
                } else {
                    localStorage.removeItem(LOCATION_STORAGE_KEY);
                    window.location.href = '/Warehouse';
                }
            }
        });
    }

    function initActionButtons() {
        var $actions = $("#wdActions");

        var buttons = [
            { text: "New Load",         icon: "add",     href: "/Warehouse/LoadType" },
            { text: "New Weight Sheet", icon: "doc",     href: "/Warehouse/WeightSheet" },
            { text: "End Of Day",       icon: "check",   href: "/Warehouse/EndOfDay" },
            { text: "Lots",             icon: "folder",  href: "/GrowerDelivery/WeightSheetLots" }
        ];

        buttons.forEach(function (b) {
            $("<div>").dxButton({
                text: b.text,
                icon: b.icon,
                width: BTN_WIDTH,
                stylingMode: "outlined",
                type: "default",
                onClick: function () {
                    window.location.href = b.href;
                }
            }).appendTo($actions);
        });
    }

    document.addEventListener("DOMContentLoaded", function () {
        initLocationPicker();
        initActionButtons();
    });
})();
