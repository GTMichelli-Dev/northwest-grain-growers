(function () {
  "use strict";

  window.gmWarehouseModeInit = window.gmWarehouseModeInit || {};

  window.gmWarehouseModeInit.intake = (function () {

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

    function elExists(selector) { return !!document.querySelector(selector); }
    function gridsExistInDom() { return elExists(sel.trucks) && elExists(sel.open) && elExists(sel.closed) && elExists(sel.loads); }
    function gridsAreInitialized() {
      return $(sel.trucks).data("dxDataGrid")
        && $(sel.open).data("dxDataGrid")
        && $(sel.closed).data("dxDataGrid")
        && $(sel.loads).data("dxDataGrid");
    }

    // Support both camelCase and PascalCase payloads
    function pick(obj, camel, pascal) {
      if (!obj) return undefined;
      if (obj[camel] !== undefined) return obj[camel];
      if (obj[pascal] !== undefined) return obj[pascal];
      return undefined;
    }

    function getCookie(name) {
      const m = document.cookie.match(new RegExp("(^| )" + name.replace(".", "\\.") + "=([^;]+)"));
      return m ? decodeURIComponent(m[2]) : "";
    }

    function toNumber(v) {
      const n = Number(v);
      return Number.isFinite(n) ? n : 0;
    }

    function ensureInitialized() {
      if (!gridsExistInDom()) return false;
      const btn = document.getElementById("btnNewIntakeTruck");
      if (btn && !btn.dataset.wired) {
        btn.addEventListener("click", openNewTruckPartial);
        btn.dataset.wired = "true";
      }
      if (!gridsAreInitialized()) {
        initGrids();
        if (!wired) {
          wireEvents();
          wired = true;
        }
      } else {
        trucksGrid = $(sel.trucks).dxDataGrid("instance");
        openGrid = $(sel.open).dxDataGrid("instance");
        closedGrid = $(sel.closed).dxDataGrid("instance");
        loadsGrid = $(sel.loads).dxDataGrid("instance");
        detailForm = $(sel.form).dxForm?.("instance") || null;
      }

      return true;
    }

    function initGrids() {
      // Trucks In Yard: show Grower (from Customer until DTO changes)
      $(sel.trucks).dxDataGrid({
        dataSource: [],
        keyExpr: "Id",
        height: 220,
        showBorders: true,
        columnAutoWidth: true,
        rowAlternationEnabled: true,
        hoverStateEnabled: true,
        paging: { enabled: false },
        columns: [
          { dataField: "Bol", caption: "BOL", width: 90 },
          { dataField: "Customer", caption: "Grower" },
          { dataField: "Bin", caption: "Bin" },
          { dataField: "Moist", caption: "Moist", width: 70 },
          { dataField: "Protein", caption: "Protein", width: 80 },
          { dataField: "Carrier", caption: "Carrier" },
          { dataField: "Crop", caption: "Crop", width: 70 }
        ]
      });

      // Open / Closed certs: show Grower (from Customer), Net from DetailsByLot
      const certGridOptions = {
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
          { dataField: "Lot", caption: "Lot", width: 110 },
          { dataField: "Customer", caption: "Grower" },
          { dataField: "Net", caption: "Net", width: 110 }
        ]
      };

      $(sel.open).dxDataGrid(certGridOptions);
      $(sel.closed).dxDataGrid(certGridOptions);

      // Loads: include timestamp + moist + protein + running net + total row
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
          {
            dataField: "WeighedAtUtc",
            caption: "Time",
            width: 140,
            dataType: "date",
            format: "shortTime"
          },
          { dataField: "Bol", caption: "BOL", width: 90 },
          { dataField: "Bin", caption: "Bin" },
          { dataField: "Moist", caption: "Moist", width: 70 },
          { dataField: "Protein", caption: "Protein", width: 80 },
          { dataField: "Gross", caption: "Gross", width: 90 },
          { dataField: "Tare", caption: "Tare", width: 90 },
          { dataField: "Net", caption: "Net", width: 90 },
          { dataField: "RunningNet", caption: "Run Net", width: 100 }
        ],
        onRowPrepared: function (e) {
          if (e.rowType !== "data") return;
          if (e.data && e.data.IsSummary) {
            e.rowElement.css({ "font-weight": "700" });
          }
        }
      });

      // Weight Certificate detail form: Grower only; Moist + Protein on same line
      $(sel.form).dxForm({
        formData: {},
        colCount: 2,
        labelLocation: "top",
        items: [
          { dataField: "Grower", label: { text: "Grower" }, editorOptions: { readOnly: true } },
          { dataField: "Carrier", label: { text: "Carrier" }, editorOptions: { readOnly: true } },

          { dataField: "Bin", label: { text: "Bin" }, editorOptions: { readOnly: true } },
          { dataField: "Commodity", label: { text: "Crop" }, editorOptions: { readOnly: true } },

          // same row: Moist | Protein
          { dataField: "Moist", label: { text: "Moist" }, editorOptions: { readOnly: true } },
          { dataField: "Protein", label: { text: "Protein" }, editorOptions: { readOnly: true } }
        ]
      });

      trucksGrid = $(sel.trucks).dxDataGrid("instance");
      openGrid = $(sel.open).dxDataGrid("instance");
      closedGrid = $(sel.closed).dxDataGrid("instance");
      loadsGrid = $(sel.loads).dxDataGrid("instance");
      detailForm = $(sel.form).dxForm("instance");
    }

    function wireEvents() {
      openGrid?.on("selectionChanged", function (e) {
        const lot = e.selectedRowKeys?.[0] ?? 0;
        showWeightSheet(lot);
      });




      closedGrid?.on("selectionChanged", function (e) {
        const lot = e.selectedRowKeys?.[0] ?? 0;
        showWeightSheet(lot);
      });
    }

  
   async function openNewTruckPartial() {
     if (window.gmWarehouse?.initMode) {
       await window.gmWarehouse.initMode("newtruck");
     }
   }






    function applySnapshot(snapshot) {
      currentSnapshot = snapshot || null;
      if (!snapshot) return;

      const trucks = pick(snapshot, "trucksInYard", "TrucksInYard") || [];
      const open = pick(snapshot, "openCerts", "OpenCerts") || [];
      const closed = pick(snapshot, "closedCerts", "ClosedCerts") || [];
      const detailsByLot = pick(snapshot, "detailsByLot", "DetailsByLot") || {};

      // Add Net onto cert rows from details
      const withNet = (arr) => (arr || []).map(c => {
        const lot = pick(c, "lot", "Lot");
        const d = detailsByLot[lot];
        return { ...c, Net: d ? pick(d, "net", "Net") : 0 };
      });

      trucksGrid?.option("dataSource", trucks);
      openGrid?.option("dataSource", withNet(open));
      closedGrid?.option("dataSource", withNet(closed));

      // default selection: first open lot
      const firstLot = open?.length ? pick(open[0], "lot", "Lot") : 0;
      if (firstLot) {
        openGrid.selectRows([firstLot], false);
        openGrid.option("focusedRowKey", firstLot);
      }

      showWeightSheet(firstLot || 0);
    }

    function showWeightSheet(lot) {
      const snap = currentSnapshot;
      if (!snap) return;

      const detailsByLot = pick(snap, "detailsByLot", "DetailsByLot") || {};
      const loadsByLot = pick(snap, "loadsByLot", "LoadsByLot") || {};

      const d = lot ? detailsByLot[lot] : null;
      const loadsRaw = lot ? (loadsByLot[lot] || []) : [];

      // Ensure loads have a stable _key (Bol + time + idx)
      const loads = (loadsRaw || []).map((l, idx) => {
        const bol = pick(l, "bol", "Bol") || 0;
        const t = pick(l, "weighedAtUtc", "WeighedAtUtc") || pick(l, "timestampUtc", "TimestampUtc") || null;
        return { _key: `${bol}|${t || ""}|${idx}`, ...l };
      });

      document.querySelector(sel.lot).textContent = lot ? String(lot) : "----";
      document.querySelector(sel.net).textContent = d ? String(pick(d, "net", "Net") ?? 0) : "0";
      document.querySelector(sel.certIdLink).textContent = d && pick(d, "weightCertificateId", "WeightCertificateId") ? `#${pick(d, "weightCertificateId", "WeightCertificateId")}` : "#----";

      detailForm?.option("formData", d || {});
      loadsGrid?.option("dataSource", loads);
    }

    async function refresh() {
      if (!ensureInitialized()) return;

      // SignalR (if available)
      if (window.gmWarehouseSignalR?.requestIntakeSnapshot) {
        await window.gmWarehouseSignalR.requestIntakeSnapshot();
        return;
      }

      // HTTP fallback
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

    return {
      ensureInitialized,
      refresh,
      applySnapshot
    };
  })();
})();
