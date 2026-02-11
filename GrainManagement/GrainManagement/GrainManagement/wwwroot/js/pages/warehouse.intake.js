/* warehouse.intake.js */
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

        // safely get either camelCase or PascalCase
        function pick(obj, camel, pascal) {
            if (!obj) return undefined;
            if (Object.prototype.hasOwnProperty.call(obj, camel)) return obj[camel];
            if (Object.prototype.hasOwnProperty.call(obj, pascal)) return obj[pascal];
            return undefined;
        }

        function toNum(v) {
            const n = Number(v);
            return Number.isFinite(n) ? n : 0;
        }

        function getCookie(name) {
            const m = document.cookie.match(new RegExp("(^| )" + name.replace(".", "\\.") + "=([^;]+)"));
            return m ? decodeURIComponent(m[2]) : "";
        }

        // normalize Truck row to one consistent shape (PascalCase)
        function normalizeTruckRow(r) {
            const Id = pick(r, "id", "Id") ?? pick(r, "truckId", "TruckId") ?? null;
            const Bol = pick(r, "bol", "Bol") ?? "";
            const Customer = pick(r, "customer", "Customer") ?? "";
            const Bin = pick(r, "bin", "Bin") ?? "";
            const Moist = pick(r, "moist", "Moist") ?? pick(r, "moisture", "Moisture") ?? 0;
            const Protein = pick(r, "protein", "Protein") ?? 0;
            const Carrier = pick(r, "carrier", "Carrier") ?? "";
            const Crop = pick(r, "crop", "Crop") ?? "";

            // DevExtreme key: prefer Id, else Bol, else stable JSON string
            const _key = (Id != null ? String(Id) : (Bol ? "BOL:" + String(Bol) : JSON.stringify(r)));

            return { _key, Id, Bol, Customer, Bin, Moist: toNum(Moist), Protein: toNum(Protein), Carrier, Crop };
        }

        // normalize Cert row
        function normalizeCertRow(r, detailsByLot) {
            const Lot = pick(r, "lot", "Lot") ?? "";
            const Customer = pick(r, "customer", "Customer") ?? "";

            const d = Lot && detailsByLot ? detailsByLot[Lot] : null;
            const Net =
                pick(r, "net", "Net") ??
                (d ? (pick(d, "net", "Net") ?? 0) : 0);

            return { Lot, Customer, Net: toNum(Net) };
        }

        // normalize Details row for the form
        function normalizeDetailsRow(d) {
            if (!d) return null;

            return {
                Lot: pick(d, "lot", "Lot") ?? "",
                Customer: pick(d, "customer", "Customer") ?? "",
                Carrier: pick(d, "carrier", "Carrier") ?? "",
                Bin: pick(d, "bin", "Bin") ?? "",
                Crop: pick(d, "crop", "Crop") ?? "",
                Grower: pick(d, "grower", "Grower") ?? "",
                WeightCertificateId: pick(d, "weightCertificateId", "WeightCertificateId") ?? null,
                Net: toNum(pick(d, "net", "Net") ?? 0),
                Moist: toNum(pick(d, "moist", "Moist") ?? pick(d, "moisture", "Moisture") ?? 0),
                Protein: toNum(pick(d, "protein", "Protein") ?? 0)
            };
        }

        // normalize Load row
        function normalizeLoadRow(r) {
            const LoadId = pick(r, "loadId", "LoadId") ?? null;
            const Id = pick(r, "id", "Id") ?? null;
            const Bol = pick(r, "bol", "Bol") ?? "";
            const Gross = pick(r, "gross", "Gross") ?? 0;
            const Tare = pick(r, "tare", "Tare") ?? 0;
            const Net = pick(r, "net", "Net") ?? (toNum(Gross) - toNum(Tare));

            // key preference: LoadId, Id, Bol, fallback
            const _key =
                (LoadId != null ? "LOAD:" + String(LoadId) :
                    (Id != null ? "ID:" + String(Id) :
                        (Bol ? "BOL:" + String(Bol) : JSON.stringify(r))));

            return {
                _key,
                LoadId,
                Id,
                Bol,
                Gross: toNum(Gross),
                Tare: toNum(Tare),
                Net: toNum(Net)
            };
        }

        function normalizeSnapshot(snapshot) {
            const trucksRaw = pick(snapshot, "trucksInYard", "TrucksInYard") || [];
            const openRaw = pick(snapshot, "openCerts", "OpenCerts") || [];
            const closedRaw = pick(snapshot, "closedCerts", "ClosedCerts") || [];

            const detailsByLotRaw = pick(snapshot, "detailsByLot", "DetailsByLot") || {};
            const loadsByLotRaw = pick(snapshot, "loadsByLot", "LoadsByLot") || {};

            // normalize dictionaries to PascalCase Details + Loads
            const detailsByLot = {};
            Object.keys(detailsByLotRaw || {}).forEach(lot => {
                detailsByLot[lot] = normalizeDetailsRow(detailsByLotRaw[lot]);
            });

            const loadsByLot = {};
            Object.keys(loadsByLotRaw || {}).forEach(lot => {
                const arr = loadsByLotRaw[lot] || [];
                loadsByLot[lot] = arr.map(normalizeLoadRow);
            });

            const trucks = (trucksRaw || []).map(normalizeTruckRow);
            const open = (openRaw || []).map(r => normalizeCertRow(r, detailsByLot));
            const closed = (closedRaw || []).map(r => normalizeCertRow(r, detailsByLot));

            return { trucks, open, closed, detailsByLot, loadsByLot };
        }

        // ---------- init ----------
        function ensureInitialized() {
            if (wired) return true;

            // if we're not on the intake partial, don't do anything
            if (!elExists(sel.trucks) || !elExists(sel.open) || !elExists(sel.closed) || !elExists(sel.loads)) {
                return false;
            }

            initGrids();
            wireEvents();
            wired = true;
            return true;
        }

        function initGrids() {
            // Trucks grid
            $(sel.trucks).dxDataGrid({
                dataSource: [],
                keyExpr: "_key",
                height: 220,
                showBorders: true,
                columnAutoWidth: true,
                rowAlternationEnabled: true,
                hoverStateEnabled: true,
                paging: { enabled: false },
                columns: [
                    { dataField: "Bol", caption: "BOL", width: 90 },
                    { dataField: "Customer", caption: "Customer" },
                    { dataField: "Bin", caption: "Bin" },
                    { dataField: "Moist", caption: "Moist", width: 70 },
                    { dataField: "Protein", caption: "Protein", width: 80 },
                    { dataField: "Carrier", caption: "Carrier" },
                    { dataField: "Crop", caption: "Crop", width: 70 }
                ]
            });

            // Open certs
            $(sel.open).dxDataGrid({
                dataSource: [],
                keyExpr: "Lot",
                height: 220,
                showBorders: true,
                columnAutoWidth: true,
                rowAlternationEnabled: true,
                hoverStateEnabled: true,
                paging: { enabled: false },
                focusedRowEnabled: true,
                selection: { mode: "single" },
                columns: [
                    { dataField: "Lot", caption: "Lot", width: 90 },
                    { dataField: "Customer", caption: "Customer" },
                    { dataField: "Net", caption: "Net", width: 90 }
                ]
            });

            // Closed certs
            $(sel.closed).dxDataGrid({
                dataSource: [],
                keyExpr: "Lot",
                height: 220,
                showBorders: true,
                columnAutoWidth: true,
                rowAlternationEnabled: true,
                hoverStateEnabled: true,
                paging: { enabled: false },
                focusedRowEnabled: true,
                selection: { mode: "single" },
                columns: [
                    { dataField: "Lot", caption: "Lot", width: 90 },
                    { dataField: "Customer", caption: "Customer" },
                    { dataField: "Net", caption: "Net", width: 90 }
                ]
            });

            // Loads grid
            $(sel.loads).dxDataGrid({
                dataSource: [],
                keyExpr: "_key",
                height: "100%",
                showBorders: true,
                columnAutoWidth: true,
                rowAlternationEnabled: true,
                hoverStateEnabled: true,
                paging: { enabled: false },
                columns: [
                    { dataField: "Bol", caption: "BOL", width: 90 },
                    { dataField: "Gross", caption: "Gross", width: 90 },
                    { dataField: "Tare", caption: "Tare", width: 90 },
                    { dataField: "Net", caption: "Net", width: 90 }
                ]
            });

            // Optional: form
            if (elExists(sel.form) && $(sel.form).dxForm) {
                $(sel.form).dxForm({
                    validationGroup: null,
                    formData: {},
                    colCount: 2,
                    labelLocation: "top",
                    items: [
                        { dataField: "Customer", label: { text: "Customer" }, editorOptions: { readOnly: true } },
                        { dataField: "Carrier", label: { text: "Carrier" }, editorOptions: { readOnly: true } },
                        { dataField: "Bin", label: { text: "Bin" }, editorOptions: { readOnly: true } },
                        { dataField: "Crop", label: { text: "Crop" }, editorOptions: { readOnly: true } },
                        { dataField: "Grower", label: { text: "Grower" }, editorOptions: { readOnly: true } },
                        { dataField: "Moist", label: { text: "Moist" }, editorOptions: { readOnly: true } },
                        { dataField: "Protein", label: { text: "Protein" }, editorOptions: { readOnly: true } }
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
            // selecting an open lot updates the right side
            openGrid?.on("selectionChanged", function (e) {
                const lot = e.selectedRowKeys?.[0] ?? null;
                showWeightSheet(lot || 0);
            });

            // selecting a closed lot also updates the right side
            closedGrid?.on("selectionChanged", function (e) {
                const lot = e.selectedRowKeys?.[0] ?? null;
                showWeightSheet(lot || 0);
            });
        }

        // ---------- snapshot application ----------
        function applySnapshot(snapshot) {
            currentSnapshot = snapshot || null;
            if (!snapshot) return;

            const n = normalizeSnapshot(snapshot);

            trucksGrid?.option("dataSource", n.trucks);
            openGrid?.option("dataSource", n.open);
            closedGrid?.option("dataSource", n.closed);

            // select first open cert by default
            const firstLot = n.open.length ? n.open[0].Lot : null;
            if (firstLot) {
                openGrid.selectRows([firstLot], false);
                openGrid.option("focusedRowKey", firstLot);
                showWeightSheet(firstLot);
            } else {
                showWeightSheet(0);
            }
        }

        function showWeightSheet(lot) {
            const snap = currentSnapshot;
            if (!snap) {
                // clear UI
                if (document.querySelector(sel.lot)) document.querySelector(sel.lot).textContent = "----";
                if (document.querySelector(sel.net)) document.querySelector(sel.net).textContent = "0";
                if (document.querySelector(sel.certIdLink)) document.querySelector(sel.certIdLink).textContent = "#----";
                detailForm?.option("formData", {});
                loadsGrid?.option("dataSource", []);
                return;
            }

            const n = normalizeSnapshot(snap);

            const lotKey = (lot && lot !== 0) ? String(lot) : "";
            const d = lotKey ? n.detailsByLot?.[lotKey] : null;
            const loads = lotKey ? (n.loadsByLot?.[lotKey] || []) : [];

            if (document.querySelector(sel.lot)) document.querySelector(sel.lot).textContent = lotKey || "----";
            if (document.querySelector(sel.net)) document.querySelector(sel.net).textContent = String(d?.Net ?? 0);

            if (document.querySelector(sel.certIdLink)) {
                const id = d?.WeightCertificateId;
                document.querySelector(sel.certIdLink).textContent = id ? `#${id}` : "#----";
            }

            detailForm?.option("formData", d || {});
            loadsGrid?.option("dataSource", loads);
        }

        // ---------- refresh ----------
        async function refresh() {
            if (!ensureInitialized()) return;

            // Preferred: SignalR (if present)
            if (window.gmWarehouseSignalR?.requestIntakeSnapshot) {
                await window.gmWarehouseSignalR.requestIntakeSnapshot();
                return;
            }

            // Fallback: HTTP snapshot
            const locStr = getCookie("GM.SelectedWarehouseLocationId");
            const locationId = parseInt(locStr, 10) || 0;
            if (locationId < 1) return;

            const resp = await fetch(`/api/warehouse/intake/snapshot?locationId=${locationId}`, {
                headers: { "Accept": "application/json" }
            });

            if (!resp.ok) throw new Error("Snapshot fetch failed: " + resp.status);
            const snapshot = await resp.json();
            applySnapshot(snapshot);
        }

        // ---------- public API ----------
        return {
            ensureInitialized,
            refresh,
            applySnapshot
        };

    })();

})();
