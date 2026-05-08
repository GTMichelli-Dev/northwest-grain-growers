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

        // End Of Day was here; it now lives in the global navbar via gm.eod.js.
        // F.O.B Load hidden for now — re-enable by restoring its row.
        // gateNewWs=true runs the prior-day-open guard before navigating so
        // the operator hits the same alert the save endpoint enforces,
        // before filling out the whole new-WS form.
        var buttons = [
            { text: "New Weight Sheet", icon: "doc",    href: "/WeightSheets/LoadType", gateNewWs: true },
            { text: "Lots",             icon: "folder", href: "/GrowerDelivery/WeightSheetLots" }
        ];

        buttons.forEach(function (b) {
            $("<div>").dxButton({
                text: b.text,
                icon: b.icon,
                width: BTN_WIDTH,
                stylingMode: "outlined",
                type: "default",
                onClick: b.gateNewWs
                    ? function () { gateAndNavigateNewWs(b.href); }
                    : function () { window.location.href = b.href; }
            }).appendTo($actions);
        });
    }

    // Calls /api/GrowerDelivery/Location/{id}/AddWeightSheetCheck and either
    // navigates to the new-WS form or notifies the operator with the same
    // message the save endpoint would have returned (e.g. "open weight
    // sheets from a previous day — run End Of Day first").
    //
    // Uses fetch (not $.ajax) deliberately: the success path returns 200
    // with an empty body, which jQuery's dataType:"json" would treat as a
    // parse error and route to .fail() with xhr.status=200, surfacing the
    // confusing "Cannot create… (HTTP 200)" message even on a clean OK.
    var _newWsChecking = false;
    async function gateAndNavigateNewWs(href) {
        if (_newWsChecking) return;
        var locationId = getLocationId();
        if (!locationId) {
            DevExpress.ui.notify(
                "Please select a location from the Warehouse dashboard first.",
                "warning", 4000);
            return;
        }
        _newWsChecking = true;
        try {
            var resp = await fetch(
                "/api/GrowerDelivery/Location/" + locationId + "/AddWeightSheetCheck",
                { headers: { "Accept": "application/json" } });
            if (resp.ok) {
                window.location.href = href;
                return;
            }
            var msg;
            try {
                var data = await resp.json();
                msg = (data && data.message) || ("Cannot create a new weight sheet (HTTP " + resp.status + ").");
            } catch (parseErr) {
                msg = "Cannot create a new weight sheet (HTTP " + resp.status + ").";
            }
            DevExpress.ui.notify({
                message: msg,
                width: 480,
                shading: false
            }, "error", 6000);
        } catch (ex) {
            DevExpress.ui.notify({
                message: "Network error checking new-weight-sheet eligibility: " + ex.message,
                width: 480,
                shading: false
            }, "error", 6000);
        } finally {
            _newWsChecking = false;
        }
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
            // Search is driven by the inline #wdWsSearch input in the
            // toolbar (wired below) — keep the in-grid panel hidden so we
            // get a single, consistent filter row. searchByText still works
            // when the panel itself isn't visible.
            searchPanel: { visible: false, highlightSearchText: true },
            columns: [
                {
                    // Print button — only visible when the Closed bucket is
                    // active. Clicking opens #wdWsPreviewModal with the WS
                    // PDF in an iframe + explicit Print / Download buttons.
                    name: "wsPrintCol",
                    caption: "",
                    width: 70,
                    alignment: "center",
                    visible: false,
                    allowSorting: false,
                    allowFiltering: false,
                    allowSearch: false,
                    cellTemplate: function (container, options) {
                        var row = options.data || {};
                        $("<button>")
                            .addClass("btn btn-outline-primary btn-sm")
                            .attr("title", "View / Print weight sheet")
                            .html('<i class="dx-icon dx-icon-print"></i>')
                            .on("click", function (e) {
                                e.stopPropagation();
                                openWsPreview(row);
                            })
                            .appendTo(container);
                    }
                },
                {
                    // Lifecycle status — hidden unless the All bucket is
                    // active (the bucket-specific buckets already imply
                    // status). Toggled by applyWsStatusColumnVisibility.
                    name: "wsStatusCol",
                    caption: "Status",
                    width: 90,
                    alignment: "center",
                    visible: false,
                    calculateCellValue: function (row) {
                        var s = row.StatusId;
                        if (s === 0 || s === 1) return "Open";
                        if (s === 2) return "Finished";
                        if (s === 3) return "Closed";
                        return "";
                    }
                },
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
            },
            // Highlight the In Yard cell pink when any loads are still in
            // yard for this weight sheet — operator attention. Overrides
            // the row's lot/type tint so the alert reads at a glance.
            // Uses setProperty(..., 'important') because the row tints
            // (.gm-ws-row--seed > td, etc.) are themselves declared with
            // !important — jQuery .css() can't beat that.
            onCellPrepared: function (e) {
                if (e.rowType !== "data") return;
                if (e.column.dataField !== "LoadsInYard") return;
                if ((e.value || 0) > 0) {
                    var el = e.cellElement[0] || e.cellElement;
                    el.style.setProperty("background-color", "pink", "important");
                    el.style.setProperty("color", "black", "important");
                    el.style.setProperty("font-weight", "bold", "important");
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

        // Date range is only visible/meaningful on Closed / All buckets.
        // Open shows live worklist (no dates). Finished is treated the same
        // — operators are expected to close every finished WS the same day,
        // so a date filter would just hide things they should be acting on.
        if (_currentBucket === "open" || _currentBucket === "pending") {
            $("#wdWsDateRange").hide();
        } else {
            $("#wdWsDateRange").css("display", "flex");
            $("#wdWsFromDate").val(_currentFromDate || "");
            $("#wdWsToDate").val(_currentToDate || "");
        }

        applyWsStatusColumnVisibility();
    }

    // The lifecycle Status column is only meaningful when the All bucket
    // is active — every other bucket already filters to a single status
    // value. The Print column is only meaningful on Closed (those are the
    // WSs the operator might want to re-issue). Toggle both safely; bails
    // when the grid isn't built yet (applyBucketToUI runs once before
    // initOpenWeightSheetsGrid).
    function applyWsStatusColumnVisibility() {
        var grid;
        try { grid = $("#wdOpenWsGrid").dxDataGrid("instance"); } catch (e) { return; }
        if (!grid) return;
        grid.columnOption("wsStatusCol", "visible", _currentBucket === "all");
        grid.columnOption("wsPrintCol",  "visible", _currentBucket === "closed");
    }

    // ── WS print preview ─────────────────────────────────────────────────
    // Opens a PDF in #wdWsPreviewFrame inside #wdWsPreviewModal. Two
    // entry points:
    //   openWsPreview(row)        → single-WS PDF via the existing
    //                               GET /api/printjobs/intake-weight-sheet/{id}/pdf
    //                               (used by the per-row Print button on
    //                               the Closed bucket).
    //   openCombinedWsPreview()   → POST every visible row's WS id to
    //                               /api/printjobs/weight-sheets/combined-pdf
    //                               and load the merged PDF as a blob URL
    //                               (used by the toolbar Print button).
    // Print/download UI is intentionally not in the modal — the browser's
    // built-in PDF viewer toolbar exposes both, which gives operators
    // consistent controls regardless of where the PDF originated.
    var _wsPreviewModal = null;
    var _wsPreviewBlobUrl = null;

    function freeWsPreviewBlob() {
        if (_wsPreviewBlobUrl) {
            try { URL.revokeObjectURL(_wsPreviewBlobUrl); } catch (e) {}
            _wsPreviewBlobUrl = null;
        }
    }

    // Show the spinner with a label, hide the iframe.
    function showWsPreviewSpinner(label) {
        document.getElementById("wdWsPreviewFrame").style.display = "none";
        document.getElementById("wdWsPreviewSpinnerText").textContent = label || "Loading…";
        document.getElementById("wdWsPreviewSpinner").style.display = "";
    }
    // Hide the spinner, reveal the iframe.
    function hideWsPreviewSpinner() {
        document.getElementById("wdWsPreviewSpinner").style.display = "none";
        document.getElementById("wdWsPreviewFrame").style.display = "";
    }

    function openWsPreview(row) {
        if (!row || !row.WeightSheetId) return;
        var wsLabel = row.As400Id ? String(row.As400Id) : formatId(row.WeightSheetId);
        var url = "/api/printjobs/intake-weight-sheet/" + encodeURIComponent(row.WeightSheetId) + "/pdf?original=true";
        ensureWsPreviewWired();
        freeWsPreviewBlob();
        $("#wdWsPreviewLabel").text("Weight Sheet " + wsLabel);
        showWsPreviewSpinner("Loading weight sheet…");
        // The iframe's "load" event fires once the PDF stream finishes —
        // ensureWsPreviewWired hooks it to hide the spinner.
        document.getElementById("wdWsPreviewFrame").src = url;
        _wsPreviewModal.show();
    }

    async function openCombinedWsPreview() {
        var grid;
        try { grid = $("#wdOpenWsGrid").dxDataGrid("instance"); } catch (e) { return; }
        if (!grid) return;

        // Visible rows reflect the active bucket / type filter / search +
        // any sort. Headers (rowType !== "data") are skipped.
        var ids = grid.getVisibleRows()
            .filter(function (r) { return r.rowType === "data" && r.data && r.data.WeightSheetId; })
            .map(function (r) { return r.data.WeightSheetId; });

        if (!ids.length) {
            DevExpress.ui.notify({ message: "No weight sheets to print.", width: 320 }, "info", 3000);
            return;
        }

        ensureWsPreviewWired();
        $("#wdWsPreviewLabel").text("Weight Sheets — " + ids.length + " sheet" + (ids.length === 1 ? "" : "s"));
        // Spinner up immediately — combining many WSs into one PDF can take
        // several seconds, so the operator needs visible "still working"
        // feedback while the server renders.
        document.getElementById("wdWsPreviewFrame").src = "about:blank";
        showWsPreviewSpinner("Building " + ids.length + " weight sheet" + (ids.length === 1 ? "" : "s") + " into a single PDF…");
        _wsPreviewModal.show();

        try {
            var resp = await fetch("/api/printjobs/weight-sheets/combined-pdf", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ WeightSheetIds: ids })
            });
            if (!resp.ok) {
                var msg;
                try { msg = (await resp.json()).message; } catch (e2) { msg = "HTTP " + resp.status; }
                DevExpress.ui.notify({ message: "Failed to build combined PDF: " + msg, width: 480 }, "error", 6000);
                _wsPreviewModal.hide();
                return;
            }
            var blob = await resp.blob();
            freeWsPreviewBlob();
            _wsPreviewBlobUrl = URL.createObjectURL(blob);
            // The iframe load event will hide the spinner once the PDF
            // viewer has finished rendering. Until then keep the spinner
            // visible — server response received != browser ready to show.
            document.getElementById("wdWsPreviewFrame").src = _wsPreviewBlobUrl;
        } catch (ex) {
            DevExpress.ui.notify({ message: "Network error: " + ex.message, width: 480 }, "error", 6000);
            _wsPreviewModal.hide();
        }
    }

    function ensureWsPreviewWired() {
        if (_wsPreviewModal) return;
        var modalEl = document.getElementById("wdWsPreviewModal");
        if (!modalEl) return;
        _wsPreviewModal = new bootstrap.Modal(modalEl);

        // Iframe finished loading the PDF (single or merged) — swap the
        // spinner out for the rendered content. Skip when src was reset
        // to about:blank during cleanup so we don't flash the iframe with
        // a blank page.
        document.getElementById("wdWsPreviewFrame").addEventListener("load", function () {
            var src = document.getElementById("wdWsPreviewFrame").src || "";
            if (src && src !== "about:blank" && src.indexOf("about:") !== 0) {
                hideWsPreviewSpinner();
            }
        });

        // Free the iframe src + any blob URL on close so the PDF stops
        // downloading in the background, the blob is GC'd, and a stale
        // frame doesn't flash on the next open.
        $(modalEl).on("hidden.bs.modal", function () {
            document.getElementById("wdWsPreviewFrame").src = "about:blank";
            freeWsPreviewBlob();
            // Reset visual state so the next open starts with the spinner
            // ready to be shown again.
            hideWsPreviewSpinner();
            document.getElementById("wdWsPreviewFrame").style.display = "none";
        });
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

        // Toolbar Print → combine every visible row's weight sheet into one
        // PDF and open it in the preview modal. Uses the grid's visible-rows
        // snapshot, so the active bucket / type / date filters / search +
        // sort all carry through to what gets printed.
        $("#wdWsPrintAllBtn").on("click", openCombinedWsPreview);

        // Inline grid search — drives the dxDataGrid's built-in search via
        // searchByText. Bails silently when the grid isn't built yet (the
        // input fires "input" only on user typing, so the grid will exist
        // by the time this runs).
        $("#wdWsSearch").on("input", function () {
            var grid;
            try { grid = $("#wdOpenWsGrid").dxDataGrid("instance"); } catch (e) { return; }
            if (!grid) return;
            grid.searchByText($(this).val() || "");
        });
    }

    function loadOpenWeightSheets(locationId) {
        _currentLocationId = locationId;
        var url = "/api/GrowerDelivery/OpenWeightSheets"
            + "?locationId=" + encodeURIComponent(locationId)
            + "&statusBucket=" + encodeURIComponent(_currentBucket);
        // Date range gates only Closed / All. Open and Finished (pending)
        // never get date params so the operator sees the full live set.
        if (_currentBucket === "closed" || _currentBucket === "all") {
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
        // Sync the Status column visibility to the (possibly persisted)
        // bucket now that the grid exists — applyBucketToUI ran during
        // initStatusFilterToolbar before the grid was built and the call
        // there silently no-op'd.
        applyWsStatusColumnVisibility();

        // Stale-WS auto-trigger. Asks the warehouse hub how many open
        // weight sheets at this location were created on a previous
        // server-day. >0 means yesterday's work never got closed out, so
        // we offer to launch the multi-location EOD loop. The
        // confirm-prompt is gated by sessionStorage inside gm.eod.js so
        // a SignalR reconnect or page refresh doesn't re-prompt within
        // the same browser tab.
        if (locationId && window.gmWarehouseRealtime
            && typeof window.gmWarehouseRealtime.checkPriorDayOpenWeightSheets === "function") {
            window.gmWarehouseRealtime
                .checkPriorDayOpenWeightSheets(locationId)
                .then(function (count) {
                    if (count > 0 && window.GM && window.GM.eod
                        && typeof window.GM.eod.promptForStaleStart === "function") {
                        window.GM.eod.promptForStaleStart(locationId, count);
                    }
                });
        }
    });
})();
