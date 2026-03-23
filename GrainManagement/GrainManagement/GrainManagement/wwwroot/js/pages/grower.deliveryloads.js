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
        wsHauler:    "#dlWsHauler",
        wsRateType:  "#dlWsRateType",
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
    var _wsHeader = null; // weight sheet header data from first row
    var _selectedLotId = null; // lot selected in the reassign grid

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
        $(sel.wsSplit).text(row.SplitName || "—");
        $(sel.wsAccount).text(row.PrimaryAccountName || "—");

        // Hauler details
        $(sel.wsHauler).text(row.HaulerName || "Grower");
        $(sel.wsRateType).text(row.CustomRateDescription || row.RateType || "—");

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
            columns: [
                {
                    dataField: "TransactionId",
                    caption: "Txn ID",
                    width: 100,
                    dataType: "number",
                    sortOrder: "desc",
                    sortIndex: 0,
                },
                {
                    dataField: "CreationDate",
                    caption: "Date",
                    width: 100,
                    dataType: "string",
                },
                {
                    dataField: "ContainerDescription",
                    caption: "Bin",
                    width: 140,
                },
                {
                    dataField: "InWeight",
                    caption: "In Weight",
                    width: 110,
                    dataType: "number",
                    format: { type: "fixedPoint", precision: 0 },
                },
                {
                    dataField: "OutWeight",
                    caption: "Out Weight",
                    width: 110,
                    dataType: "number",
                    format: { type: "fixedPoint", precision: 0 },
                },
                {
                    dataField: "Net",
                    caption: "Net",
                    width: 110,
                    dataType: "number",
                    format: { type: "fixedPoint", precision: 0 },
                    cssClass: "fw-bold",
                },
                {
                    dataField: "Attr1Value",
                    caption: "Protein",
                    width: 90,
                    dataType: "number",
                    format: { type: "fixedPoint", precision: 2 },
                    calculateCellValue: function (row) {
                        return row.Attr1Value || null;
                    },
                },
                {
                    dataField: "Attr2Value",
                    caption: "Moisture",
                    width: 90,
                    dataType: "number",
                    format: { type: "fixedPoint", precision: 2 },
                    calculateCellValue: function (row) {
                        return row.Attr2Value || null;
                    },
                },
                {
                    dataField: "Notes",
                    caption: "Notes",
                    width: 160,
                },
                {
                    caption: "",
                    width: 80,
                    alignment: "center",
                    cellTemplate: function (container, options) {
                        $("<a>")
                            .addClass("btn btn-outline-primary btn-sm")
                            .text("Edit")
                            .on("click", function () {
                                openAttrModal(options.data);
                            })
                            .appendTo(container);
                    },
                },
            ],
            summary: {
                totalItems: [
                    { column: "Net", summaryType: "sum", valueFormat: { type: "fixedPoint", precision: 0 }, displayFormat: "Total: {0}" },
                    { column: "LoadId", summaryType: "count", displayFormat: "Loads: {0}" },
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
        })
        .fail(function (xhr) {
            var msg = xhr.responseJSON && xhr.responseJSON.message
                ? xhr.responseJSON.message
                : "Failed to load delivery loads.";
            showAlert(msg, "danger");
        });
    }

    // ── Hauler select (DevExtreme) ───────────────────────────────────────────
    function initHaulerSelect() {
        $(sel.haulerSelect).dxSelectBox({
            dataSource: {
                store: {
                    type: "array",
                    key: "Id",
                    data: [],
                },
            },
            displayExpr: "Description",
            valueExpr: "Id",
            placeholder: "Select hauler…",
            searchEnabled: true,
            showClearButton: true,
        });

        // Load hauler list
        $.getJSON("/api/Lookups/Haulers").done(function (data) {
            var sb = $(sel.haulerSelect).dxSelectBox("instance");
            sb.option("dataSource", data);
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

    // ── Hauler edit modal ────────────────────────────────────────────────────
    function openHaulerModal() {
        $(sel.haulerError).attr("hidden", true);
        var sb = $(sel.haulerSelect).dxSelectBox("instance");
        sb.option("value", _wsHeader ? _wsHeader.HaulerId : null);
        _haulerModal.show();
    }

    function saveHauler() {
        var sb = $(sel.haulerSelect).dxSelectBox("instance");
        var haulerId = sb.option("value") || 0;

        $(sel.haulerSaveBtn).prop("disabled", true).text("Saving…");
        $(sel.haulerError).attr("hidden", true);

        $.ajax({
            url: "/api/GrowerDelivery/WeightSheet/" + _wsId,
            method: "PATCH",
            contentType: "application/json",
            data: JSON.stringify({ HaulerId: haulerId }),
        })
        .done(function (resp) {
            _haulerModal.hide();
            $(sel.wsHauler).text(resp.HaulerName || "Grower");
            if (_wsHeader) {
                _wsHeader.HaulerId = haulerId;
                _wsHeader.HaulerName = resp.HaulerName;
            }
            showAlert("Hauler updated.", "success");
        })
        .fail(function () {
            $(sel.haulerError).text("Failed to save hauler.").removeAttr("hidden");
        })
        .always(function () {
            $(sel.haulerSaveBtn).prop("disabled", false).text("Save");
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
