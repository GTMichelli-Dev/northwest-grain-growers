(function () {
    "use strict";

    // ── Selectors ────────────────────────────────────────────────────────────
    var sel = {
        alert:       "#dlAlert",
        listPanel:   "#dlListPanel",
        grid:        "#dlLoadsGrid",
        newLoadBtn:  "#dlNewLoadBtn",
        // WS header
        wsHeader:    "#dlWsHeader",
        wsIdFmt:     "#dlWsIdFormatted",
        wsLotNumber: "#dlWsLotNumber",
        wsCrop:      "#dlWsCrop",
        wsSplit:     "#dlWsSplit",
        wsAccount:   "#dlWsAccount",
        wsHauler:         "#dlWsHauler",
        wsRateType:       "#dlWsRateType",
        wsMilesDetail:    "#dlWsMilesDetail",
        wsMiles:          "#dlWsMiles",
        wsCalcRateDetail: "#dlWsCalcRateDetail",
        wsCalcRate:       "#dlWsCalcRate",
        wsCustomDescDetail: "#dlWsCustomDescDetail",
        wsCustomDesc:     "#dlWsCustomDesc",
        wsCustomRateDetail: "#dlWsCustomRateDetail",
        wsCustomRate:     "#dlWsCustomRate",
        wsComment:   "#dlWsComment",
        editLotBtn:  "#dlEditLotBtn",
        editHaulerBtn: "#dlEditHaulerBtn",
        // attr modal
        attrModal:   "#dlAttrModal",
        attrTxnId:   "#dlAttrTxnId",
        attr1Label:  "#dlAttr1Label",
        attr1Value:  "#dlAttr1Value",
        attr2Label:  "#dlAttr2Label",
        attr2Value:  "#dlAttr2Value",
        attrError:   "#dlAttrError",
        attrSaveBtn: "#dlAttrSaveBtn",
        // hauler modal
        haulerModal:   "#dlHaulerModal",
        haulerSelect:  "#dlHaulerSelect",
        haulerError:   "#dlHaulerError",
        haulerSaveBtn: "#dlHaulerSaveBtn",
        // lot modal
        lotModal:           "#dlLotModal",
        lotGrid:            "#dlLotGrid",
        lotPin:             "#dlLotPin",
        lotCurrentLotNum:   "#dlLotCurrentLotNum",
        lotCurrentCrop:     "#dlLotCurrentCrop",
        lotCurrentSplit:    "#dlLotCurrentSplit",
        lotCurrentAccount:  "#dlLotCurrentAccount",
        lotError:           "#dlLotError",
        lotSaveBtn:         "#dlLotSaveBtn",
        newLotBtn:          "#dlNewLotBtn",
        editLotLink:        "#dlEditLotLink",
        saveCommentBtn:     "#dlSaveCommentBtn",
    };

    var _locationId = 0;
    var _wsId = 0;
    var _modal = null;
    var _haulerModal = null;
    var _lotModal = null;
    var _binModal = null;
    var _wsHeader = null;
    var _selectedLotId = null;
    var _binTargetRow = null;   // load row currently being assigned a bin
    var _selectedBinId = null;

    // ── Init ─────────────────────────────────────────────────────────────────
    $(function () {
        _locationId = GM.getLocationId();

        _wsId = parseInt(new URLSearchParams(window.location.search).get("wsId"), 10) || 0;

        // Update New Load button to pass current weight sheet
        if (_wsId > 0) {
            $(sel.newLoadBtn).attr("href", "/GrowerDelivery/Index?wsId=" + _wsId);
        }

        if (!_locationId) {
            showAlert("Please select a location from the Warehouse dashboard first.", "warning");
            return;
        }

        _modal = new bootstrap.Modal(document.getElementById("dlAttrModal"));
        _haulerModal = new bootstrap.Modal(document.getElementById("dlHaulerModal"));
        _lotModal = new bootstrap.Modal(document.getElementById("dlLotModal"));
        _binModal = new bootstrap.Modal(document.getElementById("dlBinModal"));
        initBinGrid();

        initGrid();
        initHaulerSelect();
        initLotGrid();

        // Load WS header independently so it shows even with zero loads
        if (_wsId > 0) loadWsHeader();

        loadData();

        $(sel.attrSaveBtn).on("click", saveAttributes);
        $(sel.editHaulerBtn).on("click", openHaulerModal);
        $(sel.editLotBtn).on("click", openLotModal);
        $(sel.haulerSaveBtn).on("click", saveHauler);
        $(sel.lotSaveBtn).on("click", saveLot);
        $(sel.saveCommentBtn).on("click", saveComment);

        // Show save button when comment text changes
        $(sel.wsComment).on("input", function () {
            var current = $(this).val().trim();
            var original = (_wsHeader ? _wsHeader.WsNotes : null) || "";
            if (current !== original) {
                $(sel.saveCommentBtn).show();
            } else {
                $(sel.saveCommentBtn).hide();
            }
        });
    });

    // ── Format composite ID (e.g. 604063000004 → 604-063-000004) ──────────────
    function formatId(id) {
        var s = String(id);
        if (s.length < 7) return s;
        return s.substring(0, 3) + "-" + s.substring(3, 6) + "-" + s.substring(6);
    }

    // ── Load WS header from dedicated endpoint (works even with zero loads) ──
    function loadWsHeader() {
        $.ajax({
            url: "/api/GrowerDelivery/WeightSheet/" + _wsId,
            method: "GET",
            dataType: "json",
        })
        .done(function (row) {
            populateHeader(row);
        })
        .fail(function () {
            // Silently fall back — header just stays hidden
        });
    }

    // ── Populate WS header ──────────────────────────────────────────────────
    function populateHeader(row) {
        if (!row) return;

        _wsHeader = row;

        var fmtId = formatId(row.WeightSheetId);
        $(sel.wsIdFmt).text(fmtId);

        // Lot details
        if (row.LotId && row.LotServerId && row.LotLocationId) {
            $(sel.wsLotNumber).text(formatId(row.LotId));
        } else {
            $(sel.wsLotNumber).text("—");
        }
        $(sel.wsCrop).text(row.CropName || "—");
        $("#dlWsSplitId").text(row.SplitGroupId || "—");
        $(sel.wsSplit).text(row.SplitName || "—");
        $(sel.wsAccount).text(row.PrimaryAccountName || "—");

        // BOL / Hauler details
        var rt = row.RateType;
        $(sel.wsRateType).text(BOL_LABELS[rt] || rt || "—");
        $(sel.wsHauler).text(row.HaulerName || (rt === "U" ? "N/A" : "—"));

        var isAF = rt === "A" || rt === "F";
        var isC  = rt === "C";

        $(sel.wsMilesDetail).prop("hidden", !isAF);
        $(sel.wsMiles).text(row.Miles != null ? row.Miles : "—");

        $(sel.wsCalcRateDetail).prop("hidden", true); // fetched async below
        if (isAF && row.Miles) {
            $.getJSON("/api/Lookups/HaulerRateForMiles?rateType=" + rt + "&miles=" + row.Miles)
                .done(function (data) {
                    $(sel.wsCalcRate).text("$" + data.Rate.toFixed(2) + " (up to " + data.MaxDistance + " mi)");
                    $(sel.wsCalcRateDetail).prop("hidden", false);
                });
        }

        $(sel.wsCustomDescDetail).prop("hidden", !isC);
        $(sel.wsCustomDesc).text(row.CustomRateDescription || "—");
        $(sel.wsCustomRateDetail).prop("hidden", !isC);
        $(sel.wsCustomRate).text(row.Rate != null ? "$" + Number(row.Rate).toFixed(2) : "—");

        // Comment (textarea)
        $(sel.wsComment).val(row.WsNotes || "");
        $(sel.saveCommentBtn).hide();

        $(sel.wsHeader).removeAttr("hidden");

        // Update Edit Lot Properties link
        updateEditLotLink();
    }

    // ── Grid ─────────────────────────────────────────────────────────────────
    function initGrid() {
        $(sel.grid).dxDataGrid({
            dataSource: [],
            keyExpr: "LoadId",
            height: "calc(100vh - 280px)",
            showBorders: true,
            columnAutoWidth: true,
            rowAlternationEnabled: true,
            hoverStateEnabled: true,
            wordWrapEnabled: false,
            allowColumnResizing: true,
            paging: { pageSize: 50 },
            pager: {
                showPageSizeSelector: true,
                allowedPageSizes: [25, 50, 100],
                showInfo: true,
            },
            filterRow: { visible: true },
            headerFilter: { visible: true },
            sorting: { mode: "multiple" },
            editing: {
                mode: "cell",
                allowUpdating: true,
                selectTextOnEditStart: true,
            },
            onRowUpdating: function (e) {
                e.cancel = true; // handled manually via cellTemplate edit
            },
            columns: [
                // Complete checkbox
                {
                    caption: "✓",
                    width: 40,
                    alignment: "center",
                    allowFiltering: false,
                    allowSorting: false,
                    cellTemplate: function (container, options) {
                        var d = options.data;
                        var complete = !!d.OutWeight && !!d.ContainerDescription &&
                                       d.Attr1Value > 0 && d.Attr2Value > 0;
                        $("<input>").attr({ type: "checkbox", disabled: true })
                            .prop("checked", complete)
                            .appendTo(container);
                    },
                },
                {
                    dataField: "TransactionId",
                    caption: "Load ID",
                    width: 100,
                    dataType: "number",
                    sortOrder: "desc",
                    sortIndex: 0,
                    allowEditing: false,
                },
                {
                    dataField: "CreationDate",
                    caption: "Date",
                    width: 100,
                    dataType: "string",
                    allowEditing: false,
                },
                // Bin — click to open bin picker
                {
                    dataField: "ContainerDescription",
                    caption: "Bin",
                    width: 140,
                    allowEditing: false,
                    cellTemplate: function (container, options) {
                        var val = options.data.ContainerDescription;
                        var $cell = $("<span>").text(val || "— select —")
                            .css({ cursor: "pointer", textDecoration: "underline dotted" })
                            .on("click", function () { openBinModal(options.data); });
                        if (!val) $cell.css("background-color", "#ffc0cb");
                        container.append($cell);
                    },
                },
                {
                    dataField: "InWeight",
                    caption: "In Weight",
                    width: 110,
                    dataType: "number",
                    format: { type: "fixedPoint", precision: 0 },
                    allowEditing: false,
                },
                {
                    dataField: "OutWeight",
                    caption: "Out Weight",
                    width: 110,
                    dataType: "number",
                    format: { type: "fixedPoint", precision: 0 },
                    allowEditing: false,
                    cellTemplate: function (container, options) {
                        var val = options.data.OutWeight;
                        var $cell = $("<span>").text(val != null ? Number(val).toLocaleString() : "");
                        if (!val) $cell.css("background-color", "#ffc0cb");
                        container.append($cell);
                    },
                },
                {
                    dataField: "Net",
                    caption: "Net",
                    width: 100,
                    dataType: "number",
                    format: { type: "fixedPoint", precision: 0 },
                    cssClass: "fw-bold",
                    allowEditing: false,
                },
                // Protein — inline editable, pink if null/0
                {
                    dataField: "Attr1Value",
                    caption: "Protein",
                    width: 90,
                    dataType: "number",
                    allowEditing: true,
                    editorOptions: { min: 0, max: 40, format: "#0.00", step: 0 },
                    cellTemplate: function (container, options) {
                        var val = options.data.Attr1Value;
                        var display = (val != null && val > 0) ? Number(val).toFixed(2) : "";
                        var $cell = $("<span>").text(display)
                            .css({ display: "block", textAlign: "right", cursor: "pointer" });
                        if (!val || val <= 0) $cell.css("background-color", "#ffc0cb");
                        $cell.on("click", function () {
                            openAttrInlineEdit(options.data, 1, container);
                        });
                        container.append($cell);
                    },
                },
                // Moisture — inline editable, pink if null/0
                {
                    dataField: "Attr2Value",
                    caption: "Moisture",
                    width: 90,
                    dataType: "number",
                    allowEditing: true,
                    editorOptions: { min: 0, max: 40, format: "#0.00", step: 0 },
                    cellTemplate: function (container, options) {
                        var val = options.data.Attr2Value;
                        var display = (val != null && val > 0) ? Number(val).toFixed(2) : "";
                        var $cell = $("<span>").text(display)
                            .css({ display: "block", textAlign: "right", cursor: "pointer" });
                        if (!val || val <= 0) $cell.css("background-color", "#ffc0cb");
                        $cell.on("click", function () {
                            openAttrInlineEdit(options.data, 2, container);
                        });
                        container.append($cell);
                    },
                },
                {
                    dataField: "Notes",
                    caption: "Notes",
                    width: 160,
                    allowEditing: false,
                },
            ],
            summary: {
                totalItems: [
                    { column: "Net", summaryType: "sum", valueFormat: { type: "fixedPoint", precision: 0 }, displayFormat: "Total: {0}" },
                    { column: "TransactionId", summaryType: "count", displayFormat: "Loads: {0}" },
                ],
            },
            onRowPrepared: function (e) {
                if (e.rowType !== "data") return;
                if (e.data && e.data.ClosedAt) {
                    e.rowElement.css({ opacity: 0.6 });
                }
            },
        });

        $(sel.listPanel).removeAttr("hidden");
    }

    // ── Data loading ─────────────────────────────────────────────────────────
    function loadData() {
        var params = { locationId: _locationId };
        if (_wsId > 0) params.wsId = _wsId;

        $.ajax({
            url: "/api/GrowerDelivery/WeightSheetDeliveryLoads",
            data: params,
            method: "GET",
            dataType: "json",
        })
        .done(function (data) {
            var grid = $(sel.grid).dxDataGrid("instance");
            grid.option("dataSource", data);
            grid.refresh();
            updateStats(data);
        })
        .fail(function (xhr) {
            var msg = xhr.responseJSON && xhr.responseJSON.message
                ? xhr.responseJSON.message
                : "Failed to load delivery loads.";
            showAlert(msg, "danger");
        });
    }

    // ── Stats badges ─────────────────────────────────────────────────────────
    function updateStats(data) {
        if (!data || !data.length) {
            $("#dlStatTotal").text("0 loads");
            $("#dlStatComplete").text("0 complete");
            return;
        }
        // filter to current wsId rows only (data may contain multiple sheets when wsId=0)
        var rows = _wsId > 0 ? data.filter(function (r) { return r.WeightSheetId === _wsId; }) : data;
        var total = rows.length;
        var complete = rows.filter(function (r) {
            return !!r.OutWeight && !!r.ContainerDescription && r.Attr1Value > 0 && r.Attr2Value > 0;
        }).length;
        $("#dlStatTotal").text(total + " load" + (total !== 1 ? "s" : ""));
        $("#dlStatComplete").text(complete + " complete");
    }

    // ── Inline attribute edit (Protein / Moisture) ───────────────────────────
    function openAttrInlineEdit(rowData, attrTypeId, cellContainer) {
        var currentVal = attrTypeId === 1 ? rowData.Attr1Value : rowData.Attr2Value;
        var label = attrTypeId === 1 ? "Protein" : "Moisture";

        // Replace cell content with a small input
        cellContainer.empty();
        var $input = $("<input>")
            .attr({ type: "number", min: 0, max: 40, step: 0.01 })
            .val(currentVal > 0 ? currentVal : "")
            .css({ width: "100%", fontSize: "13px", padding: "2px 4px", textAlign: "right" })
            .appendTo(cellContainer)
            .focus()
            .on("blur keydown", function (e) {
                if (e.type === "keydown" && e.key !== "Enter" && e.key !== "Escape") return;
                if (e.type === "keydown" && e.key === "Escape") { loadData(); return; }

                var newVal = parseFloat($input.val());
                if (isNaN(newVal) || newVal < 0 || newVal > 40) {
                    loadData(); return;
                }
                var saveVal = newVal === 0 ? null : newVal;
                $.ajax({
                    url: "/api/GrowerDelivery/WeightSheetDeliveryLoads/UpdateAttribute",
                    method: "POST",
                    contentType: "application/json",
                    data: JSON.stringify({
                        TransactionId: rowData.TransactionId,
                        AttributeTypeId: attrTypeId,
                        DecimalValue: saveVal
                    })
                }).always(function () { loadData(); });
            });
    }

    // ── Bin selection modal ───────────────────────────────────────────────────
    function initBinGrid() {
        $("#dlBinGrid").dxDataGrid({
            dataSource: [],
            keyExpr: "ContainerId",
            height: 320,
            showBorders: true,
            columnAutoWidth: true,
            rowAlternationEnabled: true,
            hoverStateEnabled: true,
            selection: { mode: "single" },
            filterRow: { visible: true },
            columns: [
                { dataField: "LocationDescription", caption: "Storage Location", width: 180 },
                { dataField: "ContainerDescription", caption: "Bin", width: 160 },
                { dataField: "Notes", caption: "Notes" },
            ],
            onSelectionChanged: function (e) {
                var rows = e.selectedRowsData;
                _selectedBinId = rows.length > 0 ? rows[0].ContainerId : null;
                $("#dlBinSaveBtn").prop("disabled", !_selectedBinId);
            },
        });

        $("#dlBinSaveBtn").on("click", saveBin);
    }

    function openBinModal(rowData) {
        _binTargetRow = rowData;
        _selectedBinId = null;
        $("#dlBinSaveBtn").prop("disabled", true);
        $("#dlBinError").prop("hidden", true);

        $.getJSON("/api/Lookups/ContainerBins?locationId=" + _locationId)
            .done(function (data) {
                var grid = $("#dlBinGrid").dxDataGrid("instance");
                grid.option("dataSource", data);
                // Pre-select current bin if set
                if (rowData.ContainerId) {
                    grid.option("selectedRowKeys", [rowData.ContainerId]);
                    _selectedBinId = rowData.ContainerId;
                    $("#dlBinSaveBtn").prop("disabled", false);
                }
                _binModal.show();
            })
            .fail(function () {
                showAlert("Failed to load bins.", "danger");
            });
    }

    function saveBin() {
        if (!_binTargetRow || !_selectedBinId) return;

        $("#dlBinSaveBtn").prop("disabled", true).text("Saving…");
        $("#dlBinError").prop("hidden", true);

        $.ajax({
            url: "/api/GrowerDelivery/WeightSheetDeliveryLoads/UpdateBin",
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                LoadId: _binTargetRow.LoadId,
                ContainerId: _selectedBinId
            })
        })
        .done(function () {
            _binModal.hide();
            loadData();
        })
        .fail(function (xhr) {
            var msg = xhr.responseJSON && xhr.responseJSON.message ? xhr.responseJSON.message : "Failed to assign bin.";
            $("#dlBinError").text(msg).prop("hidden", false);
        })
        .always(function () {
            $("#dlBinSaveBtn").prop("disabled", false).text("Assign Bin");
        });
    }

    // ── BOL Type modal (DevExtreme) ──────────────────────────────────────────
    var BOL_LABELS = { U: "Universal", A: "Along Side the Field", F: "Farm Storage", C: "Custom" };

    function initHaulerSelect() {
        var haulerDs = new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Id",
                load: function () { return $.getJSON("/api/Lookups/Haulers"); }
            })
        });

        // BOL type dropdown
        $("#dlBolTypeSelect").dxSelectBox({
            items: [
                { value: "U", text: "Universal" },
                { value: "A", text: "Along Side the Field" },
                { value: "F", text: "Farm Storage" },
                { value: "C", text: "Custom" }
            ],
            valueExpr: "value",
            displayExpr: "text",
            placeholder: "Select BOL type…",
            onValueChanged: function (e) {
                var val = e.value;
                var hints = {
                    U: "Universal BOL — no hauler or rate needed.",
                    A: "Along Side the Field — hauler and miles required.",
                    F: "Farm Storage — hauler and miles required.",
                    C: "Custom — hauler, rate description, and rate required."
                };
                $("#dlBolTypeHint").text(hints[val] || "").prop("hidden", !val);
                $("#dlBolHaulerMiles").prop("hidden", val !== "A" && val !== "F");
                $("#dlBolCustom").prop("hidden", val !== "C");
                $("#dlBolCalcRate").val("");
                $("#dlBolCalcRateGroup").prop("hidden", true);
            }
        });

        // Hauler for A/F
        $("#dlBolHauler").dxSelectBox({
            dataSource: haulerDs,
            valueExpr: "Id",
            displayExpr: "Description",
            searchEnabled: true,
            placeholder: "Select hauler…"
        });

        // Miles for A/F
        $("#dlBolMiles").dxNumberBox({
            min: 0,
            format: "#0",
            placeholder: "Miles…",
            inputAttr: { style: "text-align:right;font-size:15px;" },
            onValueChanged: async function (e) {
                if (e.value === null || e.value === undefined) {
                    $("#dlBolCalcRate").val("");
                    $("#dlBolCalcRateGroup").prop("hidden", true);
                    return;
                }
                var rt = $("#dlBolTypeSelect").dxSelectBox("instance").option("value");
                try {
                    var data = await $.getJSON("/api/Lookups/HaulerRateForMiles?rateType=" + rt + "&miles=" + e.value);
                    $("#dlBolCalcRate").val("$" + data.Rate.toFixed(2) + " (up to " + data.MaxDistance + " mi)");
                    $("#dlBolCalcRateGroup").prop("hidden", false);
                } catch (_) {
                    $("#dlBolCalcRate").val("No rate found for this mileage");
                    $("#dlBolCalcRateGroup").prop("hidden", false);
                }
            }
        });

        // Custom hauler
        $("#dlBolCustomHauler").dxSelectBox({
            dataSource: new DevExpress.data.DataSource({
                store: new DevExpress.data.CustomStore({
                    key: "Id",
                    load: function () { return $.getJSON("/api/Lookups/Haulers"); }
                })
            }),
            valueExpr: "Id",
            displayExpr: "Description",
            searchEnabled: true,
            placeholder: "Select hauler…"
        });

        // Custom rate
        $("#dlBolCustomRate").dxNumberBox({
            min: 0,
            format: "#,##0.00",
            placeholder: "0.00",
            inputAttr: { style: "text-align:right;font-size:15px;" }
        });
    }

    // ── Lot grid (DevExtreme) ─────────────────────────────────────────────────
    function initLotGrid() {
        $(sel.lotGrid).dxDataGrid({
            dataSource: [],
            keyExpr: "LotId",
            height: 300,
            showBorders: true,
            columnAutoWidth: true,
            rowAlternationEnabled: true,
            hoverStateEnabled: true,
            selection: { mode: "single" },
            filterRow: { visible: true },
            sorting: { mode: "single" },
            paging: { pageSize: 20 },
            columns: [
                {
                    dataField: "LotId",
                    caption: "Lot #",
                    width: 130,
                    calculateCellValue: function (row) {
                        return formatId(row.LotId);
                    },
                    sortOrder: "asc",
                },
                { dataField: "CropName", caption: "Crop", width: 120 },
                { dataField: "SplitGroupDescription", caption: "Split", width: 140 },
                { dataField: "AccountName", caption: "Account", width: 160 },
                { dataField: "ItemDescription", caption: "Item", width: 140 },
                { dataField: "LotDescription", caption: "Description", width: 160 },
            ],
            onSelectionChanged: function (e) {
                var rows = e.selectedRowsData;
                if (rows.length > 0) {
                    _selectedLotId = rows[0].LotId;
                    $(sel.lotSaveBtn).prop("disabled", false);
                } else {
                    _selectedLotId = null;
                    $(sel.lotSaveBtn).prop("disabled", true);
                }
            },
        });
    }

    function loadLotGridData() {
        $.getJSON("/api/GrowerDelivery/OpenLots?locationId=" + _locationId).done(function (data) {
            var grid = $(sel.lotGrid).dxDataGrid("instance");
            grid.option("dataSource", data);
            grid.refresh();
        });
    }

    // ── BOL Type modal open / save ────────────────────────────────────────────
    function openHaulerModal() {
        $(sel.haulerError).attr("hidden", true);
        // Pre-fill current values
        var rt = _wsHeader ? _wsHeader.RateType : null;
        $("#dlBolTypeSelect").dxSelectBox("instance").option("value", rt);
        if (rt === "A" || rt === "F") {
            $("#dlBolHauler").dxSelectBox("instance").option("value", _wsHeader.HaulerId || null);
            $("#dlBolMiles").dxNumberBox("instance").option("value", _wsHeader.Miles || null);
        } else if (rt === "C") {
            $("#dlBolCustomHauler").dxSelectBox("instance").option("value", _wsHeader.HaulerId || null);
            $("#dlBolCustomRateDesc").val(_wsHeader.CustomRateDescription || "");
            $("#dlBolCustomRate").dxNumberBox("instance").option("value", _wsHeader.Rate || null);
        }
        _haulerModal.show();
    }

    function saveHauler() {
        var bolType = $("#dlBolTypeSelect").dxSelectBox("instance").option("value");
        if (!bolType) {
            $(sel.haulerError).text("Please select a BOL type.").removeAttr("hidden");
            return;
        }

        var payload = { RateType: bolType };

        if (bolType === "U") {
            payload.HaulerId = 0;
            payload.Miles = 0;
            payload.Rate = 0;
            payload.CustomRateDescription = "Universal";
        } else if (bolType === "A" || bolType === "F") {
            var haulerId = $("#dlBolHauler").dxSelectBox("instance").option("value");
            var miles = $("#dlBolMiles").dxNumberBox("instance").option("value");
            if (!haulerId || !miles) {
                $(sel.haulerError).text("Hauler and miles are required.").removeAttr("hidden");
                return;
            }
            payload.HaulerId = haulerId;
            payload.Miles = miles;
            payload.CustomRateDescription = bolType === "A" ? "Along Side the Field" : "Farm Storage";
        } else if (bolType === "C") {
            var cHaulerId = $("#dlBolCustomHauler").dxSelectBox("instance").option("value");
            var cRateDesc = $("#dlBolCustomRateDesc").val();
            var cRate = $("#dlBolCustomRate").dxNumberBox("instance").option("value");
            if (!cHaulerId || !cRateDesc || !cRate) {
                $(sel.haulerError).text("Hauler, rate description, and rate are all required.").removeAttr("hidden");
                return;
            }
            payload.HaulerId = cHaulerId;
            payload.CustomRateDescription = cRateDesc;
            payload.Rate = cRate;
        }

        $(sel.haulerSaveBtn).prop("disabled", true).text("Saving…");
        $(sel.haulerError).attr("hidden", true);

        $.ajax({
            url: "/api/GrowerDelivery/WeightSheet/" + _wsId,
            method: "PATCH",
            contentType: "application/json",
            data: JSON.stringify(payload),
        })
        .done(function () {
            _haulerModal.hide();
            loadWsHeader();
            showAlert("BOL type updated.", "success");
        })
        .fail(function () {
            $(sel.haulerError).text("Failed to save BOL type.").removeAttr("hidden");
        })
        .always(function () {
            $(sel.haulerSaveBtn).prop("disabled", false).text("Update BOL Type");
        });
    }

    // ── Save comment ──────────────────────────────────────────────────────
    function saveComment() {
        var notes = $(sel.wsComment).val().trim();

        $(sel.saveCommentBtn).prop("disabled", true);

        $.ajax({
            url: "/api/GrowerDelivery/WeightSheet/" + _wsId,
            method: "PATCH",
            contentType: "application/json",
            data: JSON.stringify({ Notes: notes || "" }),
        })
        .done(function () {
            if (_wsHeader) _wsHeader.WsNotes = notes || null;
            $(sel.saveCommentBtn).hide();
            showAlert("Comment saved.", "success");
        })
        .fail(function () {
            showAlert("Failed to save comment.", "danger");
        })
        .always(function () {
            $(sel.saveCommentBtn).prop("disabled", false);
        });
    }

    // ── Lot reassign modal ──────────────────────────────────────────────────
    function openLotModal() {
        $(sel.lotError).attr("hidden", true);
        $(sel.lotPin).val("");
        _selectedLotId = null;
        $(sel.lotSaveBtn).prop("disabled", true).text("Reassign Lot");

        // Populate current lot info bar
        var lotId = _wsHeader ? _wsHeader.LotId : null;
        if (lotId && _wsHeader.LotServerId && _wsHeader.LotLocationId) {
            $(sel.lotCurrentLotNum).text(formatId(lotId));
            $(sel.lotCurrentCrop).text(_wsHeader.CropName ? "| " + _wsHeader.CropName : "");
            $(sel.lotCurrentSplit).text(_wsHeader.SplitName ? "| " + _wsHeader.SplitName : "");
            $(sel.lotCurrentAccount).text(_wsHeader.PrimaryAccountName ? "| " + _wsHeader.PrimaryAccountName : "");
        } else {
            $(sel.lotCurrentLotNum).text("None assigned");
            $(sel.lotCurrentCrop).text("");
            $(sel.lotCurrentSplit).text("");
            $(sel.lotCurrentAccount).text("");
        }

        // Set New Lot button href with return params
        $(sel.newLotBtn).attr("href", "/GrowerDelivery/WeightSheetLots?createNew=true&returnTo=deliveryLoads&wsId=" + _wsId);

        // Refresh lot grid data and clear selection
        loadLotGridData();
        var grid = $(sel.lotGrid).dxDataGrid("instance");
        if (grid) grid.clearSelection();

        _lotModal.show();
    }

    // ── Update Edit Lot Properties link in header ─────────────────────────
    function updateEditLotLink() {
        var lotId = _wsHeader ? _wsHeader.LotId : null;
        if (lotId) {
            $(sel.editLotLink)
                .attr("href", "/GrowerDelivery/WeightSheetLots?editLotId=" + lotId
                    + "&returnTo=deliveryLoads&wsId=" + _wsId)
                .removeAttr("hidden");
        } else {
            $(sel.editLotLink).attr("hidden", true);
        }
    }

    function saveLot() {
        if (!_selectedLotId) {
            $(sel.lotError).text("Select a lot from the grid.").removeAttr("hidden");
            return;
        }

        // Don't save if selecting the same lot
        if (_wsHeader && _selectedLotId === _wsHeader.LotId) {
            _lotModal.hide();
            return;
        }

        var pin = parseInt($(sel.lotPin).val(), 10) || 0;
        if (pin <= 0) {
            $(sel.lotError).text("PIN is required to change the lot.").removeAttr("hidden");
            $(sel.lotPin).focus();
            return;
        }

        $(sel.lotSaveBtn).prop("disabled", true).text("Saving…");
        $(sel.lotError).attr("hidden", true);

        $.ajax({
            url: "/api/GrowerDelivery/WeightSheet/" + _wsId,
            method: "PATCH",
            contentType: "application/json",
            data: JSON.stringify({ LotId: _selectedLotId, Pin: pin }),
        })
        .done(function (resp) {
            _lotModal.hide();
            // Refresh the full header with new lot details
            if (_wsHeader) {
                _wsHeader.LotId = resp.LotId;
                _wsHeader.LotDescription = resp.LotDescription;
                _wsHeader.LotServerId = resp.LotServerId;
                _wsHeader.LotLocationId = resp.LotLocationId;
                _wsHeader.CropName = resp.CropName;
                _wsHeader.SplitName = resp.SplitName;
                _wsHeader.PrimaryAccountName = resp.PrimaryAccountName;
            }
            populateHeader(_wsHeader);
            showAlert("Lot reassigned.", "success");
        })
        .fail(function (xhr) {
            var msg = xhr.responseJSON && xhr.responseJSON.message
                ? xhr.responseJSON.message
                : "Failed to save lot.";
            $(sel.lotError).text(msg).removeAttr("hidden");
        })
        .always(function () {
            $(sel.lotSaveBtn).prop("disabled", false).text("Reassign Lot");
            $(sel.lotPin).val("");
        });
    }

    // ── Attribute edit modal ─────────────────────────────────────────────────
    function openAttrModal(row) {
        $(sel.attrTxnId).val(row.TransactionId);

        // Set labels from API data (fallback to generic names)
        $(sel.attr1Label).text(row.Attr1Description || "Protein");
        $(sel.attr2Label).text(row.Attr2Description || "Moisture");

        // Pre-fill current values
        $(sel.attr1Value).val(row.Attr1Value > 0 ? row.Attr1Value : "");
        $(sel.attr2Value).val(row.Attr2Value > 0 ? row.Attr2Value : "");

        $(sel.attrError).attr("hidden", true);

        _modal.show();
    }

    function saveAttributes() {
        var txnId = parseInt($(sel.attrTxnId).val(), 10);
        var val1 = parseFloat($(sel.attr1Value).val()) || null;
        var val2 = parseFloat($(sel.attr2Value).val()) || null;

        // Validate: exclude values <= 0
        if (val1 !== null && val1 <= 0) {
            $(sel.attrError).text("Protein must be greater than 0.").removeAttr("hidden");
            return;
        }
        if (val2 !== null && val2 <= 0) {
            $(sel.attrError).text("Moisture must be greater than 0.").removeAttr("hidden");
            return;
        }

        $(sel.attrError).attr("hidden", true);
        $(sel.attrSaveBtn).prop("disabled", true).text("Saving…");

        var calls = [];

        // Save attribute type 1 (Protein)
        calls.push($.ajax({
            url: "/api/GrowerDelivery/WeightSheetDeliveryLoads/UpdateAttribute",
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                TransactionId: txnId,
                AttributeTypeId: 1,
                DecimalValue: val1,
            }),
        }));

        // Save attribute type 2 (Moisture)
        calls.push($.ajax({
            url: "/api/GrowerDelivery/WeightSheetDeliveryLoads/UpdateAttribute",
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                TransactionId: txnId,
                AttributeTypeId: 2,
                DecimalValue: val2,
            }),
        }));

        $.when.apply($, calls)
            .done(function () {
                _modal.hide();
                loadData(); // refresh grid
                showAlert("Quality attributes saved.", "success");
            })
            .fail(function () {
                $(sel.attrError).text("Failed to save attributes. Please try again.").removeAttr("hidden");
            })
            .always(function () {
                $(sel.attrSaveBtn).prop("disabled", false).text("Save");
            });
    }

    // ── Helpers ──────────────────────────────────────────────────────────────
    function showAlert(msg, type) {
        $(sel.alert)
            .removeClass("alert-success alert-danger alert-warning alert-info")
            .addClass("alert-" + type)
            .text(msg)
            .removeAttr("hidden");

        if (type === "success") {
            setTimeout(function () { $(sel.alert).attr("hidden", true); }, 4000);
        }
    }

})();
