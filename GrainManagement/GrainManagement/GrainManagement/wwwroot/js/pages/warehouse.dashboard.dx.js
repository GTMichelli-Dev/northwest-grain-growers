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
            { text: "New Weight Sheet", icon: "doc",    href: "/WeightSheets/LoadType" },
            { text: "F.O.B Load",       icon: "export", href: "/WeightSheets/ShipLoad" },
            { text: "Lots",             icon: "folder", href: "/GrowerDelivery/WeightSheetLots" }
        ];

        buttons.forEach(function (b) {
            $("<div>").dxButton({
                text: b.text,
                icon: b.icon,
                width: BTN_WIDTH,
                stylingMode: "outlined",
                type: "default",
                onClick: function () { window.location.href = b.href; }
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
            noDataText: "No Weight Sheets",
            paging: { enabled: false },
            sorting: { mode: "single" },
            columns: [
                {
                    dataField: "As400Id",
                    caption: "WS #",
                    alignment: "center",
                    sortOrder: "desc",
                    calculateCellValue: function (row) {
                        return row.As400Id ? String(row.As400Id) : formatId(row.WeightSheetId);
                    }
                },
                {
                    // Combined type — "Seed Transfer", "Warehouse Delivery", etc.
                    caption: "Type",
                    width: 150,
                    calculateCellValue: function (row) {
                        var wsType = (row.WeightSheetType || "").toLowerCase();
                        var lotType = row.LotType;
                        var flavor = lotType === 0 ? "Seed" : (lotType === 1 ? "Warehouse" : "");
                        if (wsType === "transfer") return (flavor ? flavor + " " : "") + "Transfer";
                        if (wsType === "delivery") return (flavor ? flavor + " " : "") + "Delivery";
                        return row.WeightSheetType || "";
                    }
                },
                {
                    dataField: "LoadCount",
                    caption: "Loads",
                    width: 70,
                    alignment: "center"
                },
                {
                    dataField: "LoadsInYard",
                    caption: "In Yard",
                    width: 80,
                    alignment: "center"
                },
                {
                    dataField: "ItemDescription",
                    caption: "Item"
                },
                {
                    // Source for Received transfers, Destination for Shipped.
                    // Hidden value for Delivery WSs (the column stays in the
                    // grid layout but the cell is empty).
                    caption: "Source / Dest",
                    calculateCellValue: function (row) {
                        if ((row.WeightSheetType || "").toLowerCase() !== "transfer") return "";
                        // Receiving = current location is destination → show source.
                        // Shipping  = current location is source → show destination.
                        if (row.DestinationLocationId === row.LocationId) return row.SourceLocationName || "";
                        if (row.SourceLocationId      === row.LocationId) return row.DestinationLocationName || "";
                        return "";
                    }
                },
                {
                    caption: "Lot #",
                    width: 110,
                    calculateCellValue: function (row) {
                        if (row.LotAs400Id) return String(row.LotAs400Id);
                        return row.LotId ? String(row.LotId) : "";
                    }
                },
                {
                    dataField: "HaulerName",
                    caption: "Hauler",
                    calculateCellValue: function (row) {
                        if (row.HaulerName) return row.HaulerName;
                        return (row.WeightSheetType || "").toLowerCase() === "delivery" ? "Grower" : "";
                    }
                },
                {
                    dataField: "WsNotes",
                    caption: "Notes"
                }
            ],
            onRowClick: function (e) {
                if (e.rowType === "data") {
                    var type = (e.data.WeightSheetType || "").toLowerCase();
                    var page = type === "delivery"
                        ? "/GrowerDelivery/WeightSheetDeliveryLoads"
                        : "/GrowerDelivery/WeightSheetTransferLoads";
                    window.location.href = page + "?wsId=" + e.data.WeightSheetId;
                }
            },
            onRowPrepared: function (e) {
                if (e.rowType === "data") {
                    e.rowElement.css("cursor", "pointer");

                    // Color rows by LotType + WeightSheetType:
                    //   Delivery + 0 = Seed       → translucent seed green
                    //   Delivery + 1 = Warehouse  → translucent warehouse gold
                    //   Transfer + 0 = Seed       → translucent transfer-seed teal
                    //   Transfer + 1 = Warehouse  → translucent transfer-warehouse rust
                    var lotType = e.data.LotType;
                    var wsType  = (e.data.WeightSheetType || "").toLowerCase();
                    var isTransfer = wsType === "transfer";
                    if (lotType === 0) {
                        e.rowElement.addClass(isTransfer ? "gm-ws-row--transfer-seed" : "gm-ws-row--seed");
                    } else if (lotType === 1) {
                        e.rowElement.addClass(isTransfer ? "gm-ws-row--transfer-warehouse" : "gm-ws-row--warehouse");
                    }
                }
            }
        });

        if (locationId > 0) {
            loadOpenWeightSheets(locationId);
            // Auto-refresh the open WS grid on any WS mutation broadcast for
            // this location (push from WarehouseHub via WeightSheetNotifier).
            if (window.gmWarehouseRealtime && typeof window.gmWarehouseRealtime.onWeightSheetUpdated === "function") {
                window.gmWarehouseRealtime.onWeightSheetUpdated(function () {
                    loadOpenWeightSheets(locationId);
                }, locationId);
            }
        }
    }

    // ── Status filter state + session cookie persistence ────────────────────
    //
    // Status buckets:
    //   open    = StatusId IN (0,1)  — default, no date range
    //   pending = StatusId = 2       — date range applies
    //   closed  = StatusId = 3       — date range applies
    //   all     = any status         — date range applies
    //
    // When the user switches to any non-Open bucket, a From/To date range is
    // revealed and its selection is persisted to the wdWsFilter cookie along
    // with the chosen bucket. The cookie is NOT consulted while "open" is
    // active — that bucket always shows the live worklist with no date gate.
    var COOKIE_NAME = "wdWsFilter";
    var _currentLocationId = 0;
    var _currentBucket = "open";
    var _currentFromDate = "";
    var _currentToDate = "";
    var _rawWeightSheets = []; // unfiltered data from the API

    function readCookie(name) {
        var prefix = name + "=";
        var parts = document.cookie ? document.cookie.split(";") : [];
        for (var i = 0; i < parts.length; i++) {
            var c = parts[i].replace(/^\s+/, "");
            if (c.indexOf(prefix) === 0) {
                return decodeURIComponent(c.substring(prefix.length));
            }
        }
        return "";
    }

    function writeCookie(name, value) {
        var expires = new Date();
        expires.setFullYear(expires.getFullYear() + 1);
        document.cookie = name + "=" + encodeURIComponent(value)
            + "; expires=" + expires.toUTCString()
            + "; path=/; SameSite=Lax";
    }

    function savePersistedFilter() {
        // Only the non-Open view has a meaningful persisted state.
        if (_currentBucket === "open") {
            writeCookie(COOKIE_NAME, JSON.stringify({ bucket: "open" }));
            return;
        }
        writeCookie(COOKIE_NAME, JSON.stringify({
            bucket:   _currentBucket,
            fromDate: _currentFromDate || "",
            toDate:   _currentToDate || "",
        }));
    }

    function loadPersistedFilter() {
        var raw = readCookie(COOKIE_NAME);
        if (!raw) return;
        try {
            var obj = JSON.parse(raw);
            if (!obj || typeof obj !== "object") return;
            var b = String(obj.bucket || "").toLowerCase();
            if (b === "open" || b === "pending" || b === "closed" || b === "all") {
                _currentBucket = b;
            }
            if (typeof obj.fromDate === "string") _currentFromDate = obj.fromDate;
            if (typeof obj.toDate === "string")   _currentToDate   = obj.toDate;
        } catch (ex) { /* stale/broken cookie — ignore */ }
    }

    function setBucketButtonActive(bucket) {
        $("#wdWsStatusToolbar [data-wd-status]").each(function () {
            var isActive = $(this).attr("data-wd-status") === bucket;
            $(this).toggleClass("active", isActive);
        });
    }

    function bucketTitle(bucket) {
        switch (bucket) {
            case "pending": return "Finished Weight Sheets";
            case "closed":  return "Closed Weight Sheets";
            case "all":     return "All Weight Sheets";
            default:        return "Open Weight Sheets";
        }
    }

    function applyBucketToUI() {
        setBucketButtonActive(_currentBucket);
        $("#wdWsGridTitle").text(bucketTitle(_currentBucket));

        // Date range is only visible/meaningful on non-Open buckets.
        if (_currentBucket === "open") {
            $("#wdWsDateRange").hide();
        } else {
            $("#wdWsDateRange").css("display", "flex");
            $("#wdWsFromDate").val(_currentFromDate || "");
            $("#wdWsToDate").val(_currentToDate || "");
        }
    }

    function initStatusFilterToolbar() {
        loadPersistedFilter();
        applyBucketToUI();

        $("#wdWsStatusToolbar").on("click", "[data-wd-status]", function () {
            var b = $(this).attr("data-wd-status");
            if (!b || b === _currentBucket) return;
            _currentBucket = b;
            applyBucketToUI();
            savePersistedFilter();
            if (_currentLocationId > 0) loadOpenWeightSheets(_currentLocationId);
        });

        $("#wdWsFromDate").on("change", function () {
            _currentFromDate = $(this).val() || "";
            savePersistedFilter();
            if (_currentLocationId > 0) loadOpenWeightSheets(_currentLocationId);
        });

        $("#wdWsToDate").on("change", function () {
            _currentToDate = $(this).val() || "";
            savePersistedFilter();
            if (_currentLocationId > 0) loadOpenWeightSheets(_currentLocationId);
        });

        $("#wdWsTypeFilter").on("change", function () {
            applyTypeFilterToGrid();
        });
    }

    function loadOpenWeightSheets(locationId) {
        _currentLocationId = locationId;
        var url = "/api/GrowerDelivery/OpenWeightSheets"
            + "?locationId=" + encodeURIComponent(locationId)
            + "&statusBucket=" + encodeURIComponent(_currentBucket);
        if (_currentBucket !== "open") {
            if (_currentFromDate) url += "&fromDate=" + encodeURIComponent(_currentFromDate);
            if (_currentToDate)   url += "&toDate="   + encodeURIComponent(_currentToDate);
        }
        $.getJSON(url)
            .done(function (data) {
                _rawWeightSheets = data || [];
                applyTypeFilterToGrid();
            })
            .fail(function () {
                console.error("Failed to load open weight sheets");
            });
    }

    function applyTypeFilterToGrid() {
        var grid = $("#wdOpenWsGrid").dxDataGrid("instance");
        if (!grid) return;

        var typeFilter = ($("#wdWsTypeFilter").val() || "all").toLowerCase();
        var filtered = _rawWeightSheets;

        if (typeFilter !== "all") {
            filtered = _rawWeightSheets.filter(function (ws) {
                var wsType = (ws.WeightSheetType || "").toLowerCase();
                var lotType = ws.LotType; // 0=Seed, 1=Warehouse, null=unknown
                switch (typeFilter) {
                    case "seed-intake":
                        return wsType === "delivery" && lotType === 0;
                    case "warehouse-intake":
                        return wsType === "delivery" && lotType === 1;
                    case "seed-transfer":
                        return wsType === "transfer" && lotType === 0;
                    case "warehouse-transfer":
                        return wsType === "transfer" && lotType === 1;
                    default:
                        return true;
                }
            });
        }

        grid.option("dataSource", filtered);
    }

    document.addEventListener("DOMContentLoaded", function () {
        // Reset the cached PIN whenever the operator lands on the WS
        // dashboard. The dashboard is the "logged-out" home for kiosk
        // operators, so any follow-up sensitive action (new lot, void,
        // manual entry, etc.) must re-prompt for a fresh PIN rather than
        // ride a stale session. Done here (not at IIFE top) because this
        // script tag is parsed inside @RenderBody, before gm.pin-prompt.js
        // is loaded — so window.GM isn't defined until DOMContentLoaded.
        if (window.GM && typeof window.GM.clearLastPin === "function") {
            window.GM.clearLastPin();
        }

        initActionButtons();

        var locationId = getLocationId();
        _currentLocationId = locationId;
        initStatusFilterToolbar();
        initOpenWeightSheetsGrid(locationId);
    });
})();
