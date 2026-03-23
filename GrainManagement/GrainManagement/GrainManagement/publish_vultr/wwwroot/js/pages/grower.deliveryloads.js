(function () {
    "use strict";

    // ── Selectors ────────────────────────────────────────────────────────────
    var sel = {
        alert:       "#dlAlert",
        listPanel:   "#dlListPanel",
        grid:        "#dlLoadsGrid",
        newLoadBtn:  "#dlNewLoadBtn",
        // modal
        attrModal:   "#dlAttrModal",
        attrTxnId:   "#dlAttrTxnId",
        attr1Label:  "#dlAttr1Label",
        attr1Value:  "#dlAttr1Value",
        attr2Label:  "#dlAttr2Label",
        attr2Value:  "#dlAttr2Value",
        attrError:   "#dlAttrError",
        attrSaveBtn: "#dlAttrSaveBtn",
    };

    var _locationId = 0;
    var _wsId = 0;
    var _modal = null;

    // ── Init ─────────────────────────────────────────────────────────────────
    $(function () {
        _locationId = parseInt(localStorage.getItem("gm_location_id"), 10) || 0;
        _wsId = parseInt(new URLSearchParams(window.location.search).get("wsId"), 10) || 0;

        if (!_locationId) {
            showAlert("Please select a location from the Warehouse dashboard first.", "warning");
            return;
        }

        _modal = new bootstrap.Modal(document.getElementById("dlAttrModal"));

        initGrid();
        loadData();

        $(sel.attrSaveBtn).on("click", saveAttributes);
    });

    // ── Grid ─────────────────────────────────────────────────────────────────
    function initGrid() {
        $(sel.grid).dxDataGrid({
            dataSource: [],
            keyExpr: "LoadId",
            height: "calc(100vh - 200px)",
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
                    dataField: "WeightSheetId",
                    caption: "WS#",
                    width: 80,
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
                    dataField: "LotDescription",
                    caption: "Lot",
                    width: 160,
                },
                {
                    dataField: "ContainerDescription",
                    caption: "Container",
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
