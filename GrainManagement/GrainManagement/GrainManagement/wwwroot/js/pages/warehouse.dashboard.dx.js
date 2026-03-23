(function () {
    "use strict";

    const BTN_WIDTH = 175;

    function formatId(id) {
        var s = String(id);
        if (s.length < 7) return s;
        return s.substring(0, 3) + "-" + s.substring(3, 6) + "-" + s.substring(6);
    }

    function getLocationId() {
        var el = document.getElementById("gmWdContent");
        return el ? parseInt(el.dataset.locationId, 10) || 0 : 0;
    }

    function initActionButtons() {
        var $actions = $("#wdActions");

        var buttons = [
            { text: "New Weight Sheet", icon: "doc",     href: "/Warehouse/LoadType" },
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

    function initOpenWeightSheetsGrid(locationId) {
        $("#wdOpenWsGrid").dxDataGrid({
            dataSource: {
                store: {
                    type: "array",
                    key: "WeightSheetId",
                    data: []
                }
            },
            showBorders: true,
            showRowLines: true,
            columnAutoWidth: true,
            wordWrapEnabled: true,
            noDataText: "No open weight sheets",
            paging: { enabled: false },
            sorting: { mode: "single" },
            columns: [
                {
                    dataField: "WeightSheetId",
                    caption: "WS #",
                    alignment: "center",
                    sortOrder: "desc",
                    calculateCellValue: function (row) {
                        return formatId(row.WeightSheetId);
                    }
                },
                {
                    dataField: "WeightSheetType",
                    caption: "Type",
                    width: 80
                },
                {
                    dataField: "ItemDescription",
                    caption: "Item"
                },
                {
                    dataField: "SplitGroupDescription",
                    caption: "Split Group"
                },
                {
                    dataField: "AccountName",
                    caption: "Account"
                },
                {
                    dataField: "LotDescription",
                    caption: "Lot",
                    visible: false
                },
                {
                    dataField: "LoadCount",
                    caption: "Loads",
                    width: 60,
                    alignment: "center"
                },
                {
                    dataField: "HaulerName",
                    caption: "Hauler",
                    calculateCellValue: function (row) {
                        return row.HaulerName || "Grower";
                    }
                },
                {
                    dataField: "CustomRateDescription",
                    caption: "Rate Type",
                    calculateCellValue: function (row) {
                        return row.CustomRateDescription || row.RateType || "";
                    }
                },
                {
                    dataField: "CreationDate",
                    caption: "Created",
                    width: 95,
                    alignment: "center"
                }
            ],
            onRowClick: function (e) {
                if (e.rowType === "data") {
                    window.location.href = "/GrowerDelivery/WeightSheetDeliveryLoads?wsId=" + e.data.WeightSheetId;
                }
            },
            onRowPrepared: function (e) {
                if (e.rowType === "data") {
                    e.rowElement.css("cursor", "pointer");

                    var type = (e.data.WeightSheetType || "").toLowerCase();
                    var isAlt = e.rowIndex % 2 === 1;

                    if (type === "delivery") {
                        e.rowElement.addClass("gm-ws-row--delivery");
                        if (isAlt) e.rowElement.addClass("gm-ws-row--alt");
                    } else if (type === "transfer") {
                        e.rowElement.addClass("gm-ws-row--transfer");
                        if (isAlt) e.rowElement.addClass("gm-ws-row--alt");
                    }
                }
            }
        });

        if (locationId > 0) {
            loadOpenWeightSheets(locationId);
        }
    }

    function loadOpenWeightSheets(locationId) {
        $.getJSON("/api/GrowerDelivery/OpenWeightSheets?locationId=" + locationId)
            .done(function (data) {
                var grid = $("#wdOpenWsGrid").dxDataGrid("instance");
                if (grid) {
                    grid.option("dataSource", data);
                }
            })
            .fail(function () {
                console.error("Failed to load open weight sheets");
            });
    }

    document.addEventListener("DOMContentLoaded", function () {
        initActionButtons();

        var locationId = getLocationId();
        initOpenWeightSheetsGrid(locationId);
    });
})();
