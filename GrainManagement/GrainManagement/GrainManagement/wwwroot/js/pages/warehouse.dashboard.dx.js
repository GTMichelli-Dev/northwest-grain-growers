(function () {
    "use strict";

    function parseJsonAttr(el, name) {
        const raw = el.getAttribute(name);
        if (!raw) return [];
        try { return JSON.parse(raw); } catch { return []; }
    }

    // Read either PascalCase or camelCase (and fall back safely)
    function pick(obj, ...keys) {
        for (const k of keys) {
            if (obj && Object.prototype.hasOwnProperty.call(obj, k)) return obj[k];
        }
        return undefined;
    }

    // ------------ Accordion animation (height) ------------
    function animateOpen(body) {
        // Ensure visible
        body.style.display = "";
        body.style.overflow = "hidden";

        // Start at 0
        body.style.height = "0px";

        // Force reflow
        body.offsetHeight;

        // Expand to scrollHeight
        const target = body.scrollHeight;
        body.style.height = target + "px";

        // After transition: back to auto so page can grow naturally
        const onEnd = (ev) => {
            if (ev.propertyName !== "height") return;
            body.style.height = ""; // auto
            body.style.overflow = "";
            body.removeEventListener("transitionend", onEnd);
        };
        body.addEventListener("transitionend", onEnd);
    }

    function animateClose(body) {
        body.style.overflow = "hidden";

        // Set current explicit height so it can animate to 0
        const start = body.scrollHeight;
        body.style.height = start + "px";

        // Force reflow
        body.offsetHeight;

        // Animate to 0
        body.style.height = "0px";

        const onEnd = (ev) => {
            if (ev.propertyName !== "height") return;
            body.removeEventListener("transitionend", onEnd);
        };
        body.addEventListener("transitionend", onEnd);
    }

    function repaintGrids(sec) {
        sec.querySelectorAll(".dx-datagrid").forEach(gridEl => {
            const inst = DevExpress.ui.dxDataGrid.getInstance(gridEl);
            if (inst) inst.repaint();
        });
    }

    function updateToggleButtonState(root) {
        const btn = document.getElementById("gmWdToggleAll");
        if (!btn) return;

        const sections = root.querySelectorAll(".gm-wdSec");
        const anyClosed = Array.from(sections).some(sec => sec.classList.contains("gm-wdSec--closed"));

        if (anyClosed) {
            btn.textContent = "Expand All";
            btn.dataset.state = "expand";
            btn.setAttribute("aria-expanded", "false");
        } else {
            btn.textContent = "Collapse All";
            btn.dataset.state = "collapse";
            btn.setAttribute("aria-expanded", "true");
        }
    }

    function openSection(root, sec) {
        const body = sec.querySelector("[data-acc-body]");
        if (!body) return;
        if (!sec.classList.contains("gm-wdSec--closed")) return;

        sec.classList.remove("gm-wdSec--closed");
        sec.classList.add("gm-wdSec--open");

        animateOpen(body);

        // Repaint after animation end
        setTimeout(() => repaintGrids(sec), 260);
    }

    function closeSection(root, sec) {
        const body = sec.querySelector("[data-acc-body]");
        if (!body) return;
        if (sec.classList.contains("gm-wdSec--closed")) return;

        animateClose(body);

        sec.classList.add("gm-wdSec--closed");
        sec.classList.remove("gm-wdSec--open");
    }

    function wireAccordion(root) {
        // Individual section toggles
        root.querySelectorAll("[data-acc-btn]").forEach(btn => {
            btn.addEventListener("click", () => {
                const sec = btn.closest(".gm-wdSec");
                const body = sec?.querySelector("[data-acc-body]");
                if (!sec || !body) return;

                const isClosed = sec.classList.contains("gm-wdSec--closed");

                if (isClosed) {
                    openSection(root, sec);
                } else {
                    closeSection(root, sec);

                    // Repaint remaining open grids after close
                    setTimeout(() => {
                        root.querySelectorAll(".gm-wdSec.gm-wdSec--open").forEach(repaintGrids);
                    }, 260);
                }

                updateToggleButtonState(root);
            });
        });

        // Single Expand/Collapse All toggle
        const toggleBtn = document.getElementById("gmWdToggleAll");
        if (toggleBtn) {
            toggleBtn.addEventListener("click", () => {
                const state = (toggleBtn.dataset.state || "expand").toLowerCase();
                const shouldExpand = state === "expand";

                const sections = root.querySelectorAll(".gm-wdSec");
                sections.forEach(sec => {
                    if (shouldExpand) openSection(root, sec);
                    else closeSection(root, sec);
                });

                // After animations: repaint open grids and update label/state
                setTimeout(() => {
                    root.querySelectorAll(".gm-wdSec.gm-wdSec--open").forEach(repaintGrids);
                    updateToggleButtonState(root);
                }, 280);
            });
        }

        // Initialize correct label on load
        updateToggleButtonState(root);
    }

    function commonGridOptions(data) {
        return {
            dataSource: data,
            showBorders: true,
            columnAutoWidth: true,
            allowColumnResizing: true,
            columnResizingMode: "widget",
            hoverStateEnabled: true,

            filterRow: { visible: true, applyFilter: "auto" },
            headerFilter: { visible: true },
            searchPanel: { visible: true, highlightCaseSensitive: false, width: 320, placeholder: "Search…" },

            paging: { enabled: true, pageSize: 25 },
            pager: { showInfo: true, showPageSizeSelector: true, allowedPageSizes: [10, 25, 50, 100] },

            scrolling: { mode: "standard" },

            onRowClick: function (e) {
                const href = pick(e.data, "_href", "_Href");
                if (href) window.location.href = href;
            },

            onRowPrepared: function (e) {
                if (e.rowType !== "data") return;

                // DevExtreme supplies rowElement as a jQuery-wrapped element sometimes
                const el = e.rowElement?.get ? e.rowElement.get(0) : e.rowElement;
                if (!el || !el.classList) return;

                const cls = pick(e.data, "_rowClass", "_RowClass");
                if (cls) el.classList.add(cls);
            }
        };
    }

    function init() {
        const root = document.getElementById("gmWarehouseDashboard");
        if (!root) return;

        wireAccordion(root);

        const yardRaw = parseJsonAttr(root, "data-yard");
        const intakeRaw = parseJsonAttr(root, "data-intake");
        const transferRaw = parseJsonAttr(root, "data-transfer");
        const outboundRaw = parseJsonAttr(root, "data-outbound");

        // Normalize each dataset without relying on casing
        const yard = yardRaw.map(x => {
            const id = pick(x, "id", "Id");
            return {
                ...x,
                id,
                _href: `/Yard/Details/${id}`,
                _rowClass: "gm-row-yard"
            };
        });

        const intake = intakeRaw.map(x => {
            const id = pick(x, "weightSheetId", "WeightSheetId");
            return {
                ...x,
                id,
                _href: `/WeightSheets/Details/${id}`,
                _rowClass: "gm-row-intake"
            };
        });

        const transfer = transferRaw.map(x => {
            const id = pick(x, "weightSheetId", "WeightSheetId");
            const dir = pick(x, "direction", "Direction") || "";
            return {
                ...x,
                id,
                _href: `/Transfers/Details/${id}`,
                _rowClass: (dir.toString().toLowerCase() === "inbound")
                    ? "gm-row-transfer-in"
                    : "gm-row-transfer-out"
            };
        });

        const outbound = outboundRaw.map(x => {
            const id = pick(x, "bolId", "BolId");
            return {
                ...x,
                id,
                _href: `/Outbound/Details/${id}`,
                _rowClass: "gm-row-outbound"
            };
        });

        // Columns: use calculateCellValue so casing never matters
        function col(fieldCamel, fieldPascal, caption, dataType) {
            return {
                caption,
                dataType,
                calculateCellValue: (row) => pick(row, fieldCamel, fieldPascal)
            };
        }

        $("#grid-yard").dxDataGrid({
            ...commonGridOptions(yard),
            keyExpr: "id",
            columns: [
                col("isOpen", "IsOpen", "Open/Closed", "boolean"),
                col("ticketNumber", "TicketNumber", "Ticket"),
                col("location", "Location", "Location"),
                col("commodity", "Commodity", "Commodity"),
                col("hauler", "Hauler", "Hauler"),
                col("inTimeLocal", "InTimeLocal", "In Time"),
                col("comment", "Comment", "Comment")
            ]
        });

        $("#grid-intake").dxDataGrid({
            ...commonGridOptions(intake),
            keyExpr: "id",
            columns: [
                col("isOpen", "IsOpen", "Open/Closed", "boolean"),
                col("weightSheetNumber", "WeightSheetNumber", "Weight Sheet #"),
                col("location", "Location", "Location"),
                col("commodity", "Commodity", "Commodity"),
                col("lot", "Lot", "Lot"),
                col("hauler", "Hauler", "Hauler"),
                col("landlord", "Landlord", "Landlord"),
                col("totalTrucks", "TotalTrucks", "Total # Trucks", "number"),
                col("trucksInYard", "TrucksInYard", "# Trucks In Yard", "number"),
                col("comment", "Comment", "Comment")
            ]
        });

        $("#grid-transfer").dxDataGrid({
            ...commonGridOptions(transfer),
            keyExpr: "id",
            columns: [
                col("isOpen", "IsOpen", "Open/Closed", "boolean"),
                col("direction", "Direction", "Inbound/Outbound"),
                col("weightSheetNumber", "WeightSheetNumber", "Weight Sheet #"),
                col("source", "Source", "Source"),
                col("destination", "Destination", "Destination"),
                col("commodity", "Commodity", "Commodity"),
                col("hauler", "Hauler", "Hauler"),
                col("totalTrucks", "TotalTrucks", "Total # Trucks", "number"),
                col("trucksInYard", "TrucksInYard", "# Trucks In Yard", "number"),
                col("comment", "Comment", "Comment")
            ]
        });

        $("#grid-outbound").dxDataGrid({
            ...commonGridOptions(outbound),
            keyExpr: "id",
            columns: [
                col("isOpen", "IsOpen", "Open/Closed", "boolean"),
                col("bolNumber", "BolNumber", "BOL #"),
                col("commodity", "Commodity", "Commodity"),
                col("hauler", "Hauler", "Hauler"),
                col("origin", "Origin", "Origin"),
                col("destination", "Destination", "Destination")
            ]
        });
    }

    document.addEventListener("DOMContentLoaded", init);
})();
