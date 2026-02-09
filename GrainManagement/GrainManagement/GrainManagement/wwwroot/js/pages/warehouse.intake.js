(function () {
    "use strict";

  




    window.gmWarehouseModeInit = window.gmWarehouseModeInit || {};

    // Expose an object: { ensureInitialized, refresh, applySnapshot }
    window.gmWarehouseModeInit.intake = (function () {

        // ---- DOM selectors (MUST match your cshtml IDs) ----
        const sel = {
            trucks: "#gridTrucksInYard",
            open: "#gridOpenCerts",
            closed: "#gridClosedCerts",
            loads: "#gridLoads",
            form: "#wsDetailForm",
            certIdLink: "#wsCertIdLink",
            lot: "#wsLot",
            net: "#wsNet"
        };

        let trucksGrid, openGrid, closedGrid, loadsGrid, detailForm;
        let currentSnapshot = null;
        let wired = false;

        // ---------- helpers ----------
        function elExists(selector) {
            return !!document.querySelector(selector);
        }

        function gridsExistInDom() {
            return elExists(sel.trucks) && elExists(sel.open) && elExists(sel.closed) && elExists(sel.loads);
        }

        function gridsAreInitialized() {
            return $(sel.trucks).data("dxDataGrid")
                && $(sel.open).data("dxDataGrid")
                && $(sel.closed).data("dxDataGrid")
                && $(sel.loads).data("dxDataGrid");
        }

        // Support both camelCase and PascalCase payloads
        function pick(snapshot, camel, pascal) {
            if (!snapshot) return undefined;
            if (snapshot[camel] !== undefined) return snapshot[camel];
            if (snapshot[pascal] !== undefined) return snapshot[pascal];
            return undefined;
        }

        // ---------- init ----------
        function ensureInitialized() {
            if (!gridsExistInDom()) return false;

            // If partial reloaded, old instances are gone => recreate
            if (!gridsAreInitialized()) {
                initGrids();
                if (!wired) {
                    wireEvents();
                    wired = true;
                }
            } else {
                // Re-grab instances in case
                trucksGrid = $(sel.trucks).dxDataGrid("instance");
                openGrid = $(sel.open).dxDataGrid("instance");
                closedGrid = $(sel.closed).dxDataGrid("instance");
                loadsGrid = $(sel.loads).dxDataGrid("instance");
                detailForm = $(sel.form).dxForm?.("instance") || null;
            }

            return true;
        }

        function initGrids() {
            // IMPORTANT: These calls CREATE the grids.
            // If you already have full column definitions from your old file,
            // paste them into the options below.

            $(sel.trucks).dxDataGrid({
                dataSource: [],
                keyExpr: "id",          // adjust if your DTO uses "Id"
                height: 220,
                showBorders: true,
                columnAutoWidth: true,
                rowAlternationEnabled: true,
                hoverStateEnabled: true,
                paging: { enabled: false },
                columns: [
                    { dataField: "bol", caption: "BOL", width: 90 },     // adjust casing to your DTO
                    { dataField: "customer", caption: "Customer" },
                    { dataField: "bin", caption: "Bin" },
                    { dataField: "moist", caption: "Moist", width: 70 },
                    { dataField: "protein", caption: "Protein", width: 80 },
                    { dataField: "carrier", caption: "Carrier" },
                    { dataField: "crop", caption: "Crop", width: 70 }
                ]
            });

            $(sel.open).dxDataGrid({
                dataSource: [],
                keyExpr: "lot",
                height: 220,
                showBorders: true,
                columnAutoWidth: true,
                rowAlternationEnabled: true,
                hoverStateEnabled: true,
                paging: { enabled: false },
                focusedRowEnabled: true,
                selection: { mode: "single" },
                columns: [
                    { dataField: "lot", caption: "Lot", width: 90 },
                    { dataField: "customer", caption: "Customer" },
                    { dataField: "net", caption: "Net", width: 90 }
                ]
            });

            $(sel.closed).dxDataGrid({
                dataSource: [],
                keyExpr: "lot",
                height: 220,
                showBorders: true,
                columnAutoWidth: true,
                rowAlternationEnabled: true,
                hoverStateEnabled: true,
                paging: { enabled: false },
                focusedRowEnabled: true,
                selection: { mode: "single" },
                columns: [
                    { dataField: "lot", caption: "Lot", width: 90 },
                    { dataField: "customer", caption: "Customer" },
                    { dataField: "net", caption: "Net", width: 90 }
                ]
            });

            $(sel.loads).dxDataGrid({
                dataSource: [],
                keyExpr: "loadId",      // adjust to your DTO
                height: "100%",
                showBorders: true,
                columnAutoWidth: true,
                rowAlternationEnabled: true,
                hoverStateEnabled: true,
                paging: { enabled: false },
                columns: [
                    { dataField: "loadId", caption: "Load", width: 80 },
                    { dataField: "gross", caption: "Gross", width: 90 },
                    { dataField: "tare", caption: "Tare", width: 90 },
                    { dataField: "net", caption: "Net", width: 90 }
                ]
            });

            // Optional: form
            if (elExists(sel.form) && $(sel.form).dxForm) {
                $(sel.form).dxForm({
                    formData: {},
                    colCount: 2,
                    labelLocation: "top",
                    items: [
                        { dataField: "customer", label: { text: "Customer" }, editorOptions: { readOnly: true } },
                        { dataField: "carrier", label: { text: "Carrier" }, editorOptions: { readOnly: true } },
                        { dataField: "bin", label: { text: "Bin" }, editorOptions: { readOnly: true } },
                        { dataField: "crop", label: { text: "Crop" }, editorOptions: { readOnly: true } }
                    ]
                });
            }

            // Grab instances
            trucksGrid = $(sel.trucks).dxDataGrid("instance");
            openGrid = $(sel.open).dxDataGrid("instance");
            closedGrid = $(sel.closed).dxDataGrid("instance");
            loadsGrid = $(sel.loads).dxDataGrid("instance");
            detailForm = $(sel.form).dxForm?.("instance") || null;
        }

        function wireEvents() {
            if (!openGrid) return;

            // When selecting an open cert, load weight sheet details
            openGrid.on("selectionChanged", function (e) {
                const key = (e.selectedRowKeys && e.selectedRowKeys.length) ? e.selectedRowKeys[0] : null;
                showWeightSheet(key || 0);
            });
        }

        // ---------- snapshot application ----------
        function applySnapshot(snapshot) {
            currentSnapshot = snapshot || null;
            if (!snapshot) return;

            // Support camelCase / PascalCase
            const trucks = pick(snapshot, "trucksInYard", "TrucksInYard") || [];
            const open = pick(snapshot, "openCerts", "OpenCerts") || [];
            const closed = pick(snapshot, "closedCerts", "ClosedCerts") || [];
            const loads = pick(snapshot, "loads", "Loads") || []; // optional, depends on your DTO

            // Update grids safely
            try { trucksGrid?.option("dataSource", trucks); } catch { }
            try { openGrid?.option("dataSource", open); } catch { }
            try { closedGrid?.option("dataSource", closed); } catch { }
            try { loadsGrid?.option("dataSource", loads); } catch { }

            // Default selection
            const firstLot = open.length ? (open[0].lot ?? open[0].Lot) : null;
            if (firstLot) {
                try {
                    openGrid.selectRows([firstLot], false);
                    openGrid.option("focusedRowKey", firstLot);
                } catch { }
                showWeightSheet(firstLot);
            } else {
                showWeightSheet(0);
            }
        }

        function showWeightSheet(lot) {
            // Replace this with your existing implementation.
            // For now, we update the right-side header fields if present.

            // Find cert data from snapshot.OpenCerts/openCerts
            const open = pick(currentSnapshot, "openCerts", "OpenCerts") || [];
            const rec = open.find(x => (x.lot ?? x.Lot) === lot) || null;

            try { document.querySelector(sel.lot).textContent = rec ? (rec.lot ?? rec.Lot) : "----"; } catch { }
            try { document.querySelector(sel.net).textContent = rec ? (rec.net ?? rec.Net ?? 0) : "0"; } catch { }

            // Form
            if (detailForm) {
                try { detailForm.option("formData", rec || {}); } catch { }
            }
        }

        // ---------- refresh ----------
        async function refresh() {
            if (!ensureInitialized()) return;

            // If SignalR helper exists, ask it for a snapshot.
            // It should call applySnapshot(snapshot) when it receives it.
            if (window.gmWarehouseSignalR?.requestIntakeSnapshot) {
                await window.gmWarehouseSignalR.requestIntakeSnapshot();
                return;
            }

            // Fallback: if you have a dummy snapshot provider, call it here.
            // Otherwise do nothing.
        }

        return {
            ensureInitialized,
            refresh,
            applySnapshot
        };
    })();

})();
