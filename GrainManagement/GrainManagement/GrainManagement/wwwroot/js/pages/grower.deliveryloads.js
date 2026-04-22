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
        // reprint modal
        reprintModal:       "#dlReprintModal",
        reprintTxnId:       "#dlReprintTxnId",
        reprintPrinterSel:  "#dlReprintPrinterSelect",
        reprintSendBtn:     "#dlReprintSendBtn",
        reprintError:       "#dlReprintError",
        browserPrintBtn:    "#dlBrowserPrintBtn",
        // void modal
        voidModal:    "#dlVoidModal",
        voidTxnId:    "#dlVoidTxnId",
        voidReason:   "#dlVoidReason",
        voidPin:      "#dlVoidPin",
        voidError:    "#dlVoidError",
        voidSaveBtn:  "#dlVoidSaveBtn",
        // restore modal
        restoreModal:   "#dlRestoreModal",
        restoreTxnId:   "#dlRestoreTxnId",
        restorePin:     "#dlRestorePin",
        restoreError:   "#dlRestoreError",
        restoreSaveBtn: "#dlRestoreSaveBtn",
    };

    var _locationId = 0;
    var _wsId = 0;
    // WeightSheet lifecycle status (see warehouse.WeightSheetStatuses):
    //   0 = Open                  — New Load visible, edits allowed
    //   1 = PendingNotFinished    — No New Load, edits still allowed
    //   2 = PendingFinished       — No New Load, edits still allowed
    //   3 = Closed                — No New Load, no edits at all
    var _wsStatusId = 0;
    var _modal = null;
    var _haulerModal = null;
    var _lotModal = null;
    var _binModal = null;
    var _wsHeader = null;
    var _selectedLotId = null;
    var _binTargetRow = null;   // load row currently being assigned a bin
    var _selectedBinId = null;
    var _voidModal = null;
    var _restoreModal = null;
    var _reprintModal = null;
    var _connectedPrinters = [];  // populated via SignalR

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
        _voidModal = new bootstrap.Modal(document.getElementById("dlVoidModal"));
        _restoreModal = new bootstrap.Modal(document.getElementById("dlRestoreModal"));
        _reprintModal = new bootstrap.Modal(document.getElementById("dlReprintModal"));
        initBinGrid();
        initVoidRestore();
        initReprint();

        // Print Summary button — uses the internal WeightSheetId from the
        // loaded header, not the As400Id-shaped value carried in the URL.
        $("#dlPrintSummaryBtn").on("click", function () {
            var internalWsId = _wsHeader && _wsHeader.WeightSheetId;
            if (internalWsId) {
                window.open("/api/printjobs/intake-weight-sheet/" + internalWsId + "/pdf?original=true", "_blank");
            }
        });

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
        $(sel.lotPin).on("keydown", function (e) { if (e.key === "Enter") { e.preventDefault(); $(sel.lotSaveBtn).click(); } });
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

    // ── Status gating ───────────────────────────────────────────────────────
    // Applied every time the WS header loads so the UI reflects the current
    // lifecycle state of the sheet. Called from populateHeader.
    //   - StatusId > 0 hides the "New Load" button (no adds once Pending).
    //   - StatusId == 3 (Closed) hides the edit affordances (hauler / lot /
    //     comment save) and isEditingLocked() below short-circuits the load
    //     grid cell click so inline edits and the deep-link navigation that
    //     opens full-edit mode are both suppressed.
    function applyStatusGates() {
        // Hide the New Load button on any non-Open status.
        if (_wsStatusId > 0) {
            $(sel.newLoadBtn).attr("hidden", true).hide();
        } else {
            $(sel.newLoadBtn).removeAttr("hidden").show();
        }

        // Closed — lock down header editing too. Pending states (1,2) still
        // allow load edits, so we leave them alone.
        if (_wsStatusId >= 3) {
            $(sel.editHaulerBtn).attr("hidden", true).hide();
            $(sel.editLotBtn).attr("hidden", true).hide();
            $(sel.editLotLink).attr("hidden", true).hide();
            $(sel.wsComment).prop("readonly", true);
            $(sel.saveCommentBtn).hide();
        }
    }

    // True when the weight sheet is closed and no edits are allowed.
    function isEditingLocked() {
        return _wsStatusId >= 3;
    }

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
        _wsStatusId = (typeof row.StatusId === "number") ? row.StatusId : 0;
        applyStatusGates();

        var fmtId = row.WsAs400Id ? String(row.WsAs400Id) : formatId(row.WeightSheetId);
        $(sel.wsIdFmt).text(fmtId);

        // Lot details — show As400Id if available
        if (row.LotId && row.LotServerId && row.LotLocationId) {
            $(sel.wsLotNumber).text(row.LotAs400Id ? String(row.LotAs400Id) : formatId(row.LotId));
        } else {
            $(sel.wsLotNumber).text("—");
        }
        $(sel.wsCrop).text(row.CropName || "—");
        $("#dlWsSplitId").text(row.SplitGroupId || "—");
        $(sel.wsSplit).text(row.SplitName || "—");
        $(sel.wsAccount).text(row.PrimaryAccountName || "—");

        // Weightmaster & Date
        $("#dlWsWeightmaster").text(row.WeightmasterName || "—");
        $("#dlWsDate").text(row.CreationDate || "—");

        // BOL / Hauler details
        var rt = row.RateType || "N";
        $(sel.wsRateType).text(BOL_LABELS[rt] || BOL_LABELS["N"] || "None");
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
    function formatId(id) {
        var s = String(id);
        if (s.length < 7) return s;
        return s.substring(0, 3) + '-' + s.substring(3, 6) + '-' + s.substring(6);
    }

    function isRowComplete(d) {
        return !!d.OutWeight && !!d.ContainerDescription && d.Attr1Value > 0;
    }

    function isRowCompleteMissingProtein(d) {
        return !!d.OutWeight && !!d.ContainerDescription && !(d.Attr1Value > 0);
    }

    function initGrid() {
        $(sel.grid).dxDataGrid({
            dataSource: [],
            keyExpr: "TransactionId",
            height: "calc(100vh - 210px)",
            showBorders: true,
            columnAutoWidth: true,
            rowAlternationEnabled: false,
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
                // Completed status
                {
                    caption: "Completed",
                    width: 120,
                    alignment: "center",
                    allowFiltering: true,
                    allowSorting: true,
                    allowEditing: false,
                    cellTemplate: function (container, options) {
                        var d = options.data;
                        var complete = isRowComplete(d);
                        var missingProtein = isRowCompleteMissingProtein(d);
                        var cls, text;
                        if (complete) {
                            cls = "bg-success"; text = "Completed";
                        } else if (missingProtein) {
                            cls = "bg-warning text-dark"; text = "Completed";
                        } else {
                            cls = "bg-danger"; text = "Not Complete";
                        }
                        $("<span>").addClass("badge " + cls).css({ "font-size": "0.8em", "display": "inline-block", "min-width": "90px", "text-align": "center" }).text(text)
                            .appendTo(container);
                    },
                },
                {
                    dataField: "TransactionId",
                    caption: "Load ID",
                    width: 110,
                    dataType: "number",
                    sortOrder: "desc",
                    sortIndex: 0,
                    allowEditing: false,
                    cellTemplate: function (container, options) {
                        $("<a>")
                            .attr("href", "/GrowerDelivery/Index?wsId=" + _wsId + "&txnId=" + options.data.TransactionId)
                            .text(formatId(options.data.TransactionId))
                            .css({ color: "#0d6efd", textDecoration: "underline", cursor: "pointer", fontSize: "0.85em" })
                            .appendTo(container);
                    },
                },
                {
                    dataField: "StartedAt",
                    caption: "Time In",
                    width: 130,
                    dataType: "datetime",
                    allowEditing: false,
                    cellTemplate: function (container, options) {
                        var val = options.data.StartedAt;
                        if (val) {
                            var d = new Date(val);
                            $("<span>").text(d.toLocaleString(undefined, { month: "2-digit", day: "2-digit", year: "numeric", hour: "numeric", minute: "2-digit", hour12: true }))
                                .css("font-size", "0.85em").appendTo(container);
                        }
                    },
                },
                {
                    dataField: "CompletedAt",
                    caption: "Time Out",
                    width: 130,
                    dataType: "datetime",
                    allowEditing: false,
                    cellTemplate: function (container, options) {
                        var val = options.data.CompletedAt;
                        if (val) {
                            var d = new Date(val);
                            $("<span>").text(d.toLocaleString(undefined, { month: "2-digit", day: "2-digit", year: "numeric", hour: "numeric", minute: "2-digit", hour12: true }))
                                .css("font-size", "0.85em").appendTo(container);
                        }
                    },
                },
                {
                    dataField: "BOL",
                    caption: "BOL",
                    width: 90,
                    allowEditing: false,
                },
                // Bin — click to open bin picker, yellow if not set
                {
                    dataField: "ContainerDescription",
                    caption: "Bin",
                    allowEditing: false,
                    cellTemplate: function (container, options) {
                        var val = options.data.ContainerDescription;
                        var $cell = $("<span>").text(val || "— select —")
                            .css({ cursor: "pointer", textDecoration: "underline dotted", display: "block", padding: "2px 4px" })
                            .on("click", function (e) { e.stopPropagation(); openBinModal(options.data); });
                        if (!val) container.css("background-color", "#fff9c4");
                        container.append($cell);
                    },
                },
                {
                    dataField: "InWeight",
                    caption: "In Wt",
                    width: 80,
                    dataType: "number",
                    format: { type: "fixedPoint", precision: 0 },
                    allowEditing: false,
                    cellTemplate: function (container, options) {
                        var val = options.data.InWeight;
                        var manual = options.data.StartIsManual ? " M" : "";
                        $("<span>").text(val != null ? Number(val).toLocaleString() + manual : "").appendTo(container);
                    },
                },
                {
                    dataField: "OutWeight",
                    caption: "Out Wt",
                    width: 80,
                    dataType: "number",
                    format: { type: "fixedPoint", precision: 0 },
                    allowEditing: false,
                    cellTemplate: function (container, options) {
                        var val = options.data.OutWeight;
                        var manual = options.data.EndIsManual ? " M" : "";
                        $("<span>").text(val != null ? Number(val).toLocaleString() + manual : "").appendTo(container);
                    },
                },
                {
                    dataField: "Net",
                    caption: "Net",
                    width: 80,
                    dataType: "number",
                    format: { type: "fixedPoint", precision: 0 },
                    cssClass: "fw-bold",
                    allowEditing: false,
                },
                // Protein — inline editable, highlighted if not set
                {
                    dataField: "Attr1Value",
                    caption: "Protein",
                    width: 90,
                    dataType: "number",
                    allowEditing: false,
                    cellTemplate: function (container, options) {
                        var d = options.data;
                        var val = d.Attr1Value;
                        var display = (val != null && val > 0) ? Number(val).toFixed(2) : "";
                        var $cell = $("<span>").text(display)
                            .css({ display: "block", textAlign: "right", cursor: "pointer", padding: "2px 4px" })
                            .on("click", function (e) { e.stopPropagation(); openAttrInlineEdit(d, 1, container); });
                        if (!val || val <= 0) {
                            // Orange highlight if row is otherwise complete, yellow if not
                            var urgent = isRowCompleteMissingProtein(d);
                            container.css({
                                "background-color": urgent ? "#ffcc80" : "#fff9c4",
                                "border": urgent ? "2px solid #e65100" : "none",
                            });
                            if (!display) $cell.text("*").css({ color: "#e65100", fontWeight: "bold", fontSize: "1.2em" });
                        }
                        container.append($cell);
                    },
                },
                // Moisture — inline editable
                {
                    dataField: "Attr2Value",
                    caption: "Moisture",
                    width: 90,
                    dataType: "number",
                    allowEditing: false,
                    cellTemplate: function (container, options) {
                        var val = options.data.Attr2Value;
                        var display = (val != null && val > 0) ? Number(val).toFixed(2) : "";
                        var $cell = $("<span>").text(display)
                            .css({ display: "block", textAlign: "right", cursor: "pointer", padding: "2px 4px" })
                            .on("click", function (e) { e.stopPropagation(); openAttrInlineEdit(options.data, 2, container); });
                        container.append($cell);
                    },
                },
                {
                    dataField: "Notes",
                    caption: "Notes",
                    minWidth: 200,
                    allowEditing: false,
                    cellTemplate: function (container, options) {
                        var val = options.data.Notes;
                        $("<span>").text(val || "")
                            .css({ display: "block", cursor: "pointer", padding: "2px 4px", fontStyle: val ? "normal" : "italic", color: val ? "inherit" : "#999" })
                            .appendTo(container);
                    },
                },
                // Print button
                {
                    caption: "Print",
                    width: 70,
                    alignment: "center",
                    allowFiltering: false,
                    allowSorting: false,
                    allowEditing: false,
                    cellTemplate: function (container, options) {
                        $("<button>")
                            .addClass("btn btn-outline-primary btn-sm")
                            .attr("title", "Print / Reprint ticket")
                            .html('<i class="dx-icon dx-icon-print"></i>')
                            .on("click", function (e) { e.stopPropagation(); openReprintModal(options.data); })
                            .appendTo(container);
                    },
                },
            ],
            onCellClick: function (e) {
                if (e.rowType !== "data") return;

                var col = e.column;
                var df = col ? (col.dataField || "") : "";
                var caption = col ? (col.caption || "") : "";
                var tag = (e.event.target.tagName || "").toLowerCase();

                // Closed weight sheet — no edits, no full-edit navigation.
                // Still let the user click Print so they can re-issue tickets.
                if (isEditingLocked() && caption !== "Print") {
                    return;
                }

                // Protein — open inline edit
                if (df === "Attr1Value" || caption === "Protein") {
                    openAttrInlineEdit(e.data, 1, $(e.cellElement));
                    return;
                }
                // Moisture — open inline edit
                if (df === "Attr2Value" || caption === "Moisture") {
                    openAttrInlineEdit(e.data, 2, $(e.cellElement));
                    return;
                }
                // Bin — open bin picker
                if (df === "ContainerDescription" || caption === "Bin") {
                    openBinModal(e.data);
                    return;
                }
                // Notes — open inline edit
                if (df === "Notes" || caption === "Notes") {
                    openNotesInlineEdit(e.data, $(e.cellElement));
                    return;
                }
                // Print, Completed — no navigation
                if (caption === "Print" || caption === "Completed") return;

                // Skip if clicking buttons/inputs
                if (tag === "button" || tag === "input" || tag === "i" ||
                    $(e.event.target).closest("button").length > 0) return;

                var d = e.data;
                var url = "/GrowerDelivery/Index?wsId=" + _wsId + "&txnId=" + d.TransactionId;
                window.location.href = url;
            },
            onRowPrepared: function (e) {
                if (e.rowType !== "data") return;
                var d = e.data;
                if (d && d._isVoided) {
                    e.rowElement.css({ opacity: 0.45, textDecoration: "line-through" });
                } else if (d && isRowComplete(d)) {
                    e.rowElement.css("background-color", "#e8f5e9"); // light green
                } else if (d && isRowCompleteMissingProtein(d)) {
                    e.rowElement.css("background-color", "#fff9c4"); // light yellow
                } else if (d) {
                    e.rowElement.css("background-color", "#fce4ec"); // light pink
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
            // Fetch void eligibility for each unique TransactionId
            var txnIds = [];
            var txnIdSet = {};
            (data || []).forEach(function (r) {
                if (r.TransactionId && !txnIdSet[r.TransactionId]) {
                    txnIdSet[r.TransactionId] = true;
                    txnIds.push(r.TransactionId);
                }
            });

            var eligibilityMap = {};
            var promises = txnIds.map(function (txnId) {
                return $.ajax({
                    url: "/api/GrowerDelivery/" + txnId + "/void-eligibility",
                    method: "GET",
                    dataType: "json",
                }).done(function (elig) {
                    eligibilityMap[txnId] = elig;
                }).fail(function () {
                    // If eligibility check fails, hide buttons
                    eligibilityMap[txnId] = { CanVoid: false, CanRestore: false, IsVoided: false };
                });
            });

            $.when.apply($, promises).always(function () {
                // Enrich rows with void eligibility flags
                (data || []).forEach(function (r) {
                    var e = eligibilityMap[r.TransactionId] || {};
                    r._canVoid   = !!e.CanVoid;
                    r._canRestore = !!e.CanRestore;
                    r._isVoided  = !!e.IsVoided;
                });

                var grid = $(sel.grid).dxDataGrid("instance");
                grid.option("dataSource", data);
                grid.refresh();
                updateStats(data);
            });
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
            $("#dlStatTotalNet").text("Net: 0 lbs");
            return;
        }
        var rows = _wsId > 0 ? data.filter(function (r) { return r.WeightSheetId === _wsId; }) : data;
        var total = rows.length;
        var complete = rows.filter(function (r) {
            return isRowComplete(r) || isRowCompleteMissingProtein(r);
        }).length;
        var totalNet = rows.reduce(function (sum, r) {
            // Only count loads that have a completed weight (OutWeight or DirectQty)
            if (r.OutWeight == null && r.Net == null) return sum;
            return sum + (r.Net || 0);
        }, 0);
        $("#dlStatTotal").text(total + " load" + (total !== 1 ? "s" : ""));
        $("#dlStatComplete").text(complete + " complete");
        $("#dlStatTotalNet").text("Net: " + Number(totalNet).toLocaleString(undefined, { maximumFractionDigits: 0 }) + " lbs");
    }

    // ── Inline attribute edit (Protein / Moisture) ───────────────────────────
    // attrTypeId: 1 = Protein, 2 = Moisture (for local UI state only)
    // The server resolves the actual AttributeTypeId from AttributeCode.
    function openAttrInlineEdit(rowData, attrTypeId, cellContainer) {
        var attrCode = attrTypeId === 1 ? "PROTEIN" : "MOISTURE";
        var currentVal = attrTypeId === 1 ? rowData.Attr1Value : rowData.Attr2Value;
        var maxVal = attrTypeId === 1 ? 20 : 50;
        var saving = false;

        cellContainer.empty();
        var $input = $("<input>")
            .attr({ type: "number", min: 0, max: maxVal, step: 0.01 })
            .val(currentVal > 0 ? currentVal : "")
            .css({ width: "100%", fontSize: "13px", padding: "2px 4px", textAlign: "right" })
            .appendTo(cellContainer);

        // Defer focus to next tick so the grid doesn't steal it
        setTimeout(function () { $input.focus().select(); }, 0);

        function commitValue() {
            if (saving) return;
            saving = true;

            var newVal = parseFloat($input.val());
            if (isNaN(newVal) || newVal < 0 || newVal > maxVal) {
                loadData();
                return;
            }
            var saveVal = newVal === 0 ? null : newVal;
            $.ajax({
                url: "/api/GrowerDelivery/WeightSheetDeliveryLoads/UpdateAttribute",
                method: "POST",
                contentType: "application/json",
                data: JSON.stringify({
                    TransactionId: rowData.TransactionId,
                    AttributeCode: attrCode,
                    DecimalValue: saveVal
                })
            })
            .done(function () { console.log("[DeliveryLoads] Attribute saved."); })
            .fail(function (xhr) { console.error("[DeliveryLoads] Attribute save failed:", xhr.status, xhr.responseText); })
            .always(function () { loadData(); });
        }

        $input.on("keydown", function (e) {
            if (e.key === "Enter") {
                e.preventDefault();
                $input.off("blur"); // prevent blur from double-firing
                commitValue();
            } else if (e.key === "Escape") {
                e.preventDefault();
                $input.off("blur");
                loadData();
            }
        });

        $input.on("blur", function () {
            commitValue();
        });
    }

    // ── Inline notes edit ─────────────────────────────────────────────────────
    function openNotesInlineEdit(rowData, cellContainer) {
        var currentVal = rowData.Notes || "";
        var saving = false;

        cellContainer.empty();
        var $input = $("<input>")
            .attr({ type: "text", maxlength: 500, placeholder: "Add notes…" })
            .val(currentVal)
            .css({ width: "100%", fontSize: "13px", padding: "2px 4px" })
            .appendTo(cellContainer);

        setTimeout(function () { $input.focus().select(); }, 0);

        function commitNotes() {
            if (saving) return;
            saving = true;
            var newVal = ($input.val() || "").trim();
            // Skip save if unchanged
            if (newVal === (currentVal || "").trim()) { loadData(); return; }
            $.ajax({
                url: "/api/GrowerDelivery/WeightSheetDeliveryLoads/UpdateNotes",
                method: "POST",
                contentType: "application/json",
                data: JSON.stringify({
                    TransactionId: rowData.TransactionId,
                    Notes: newVal || null
                })
            })
            .fail(function (xhr) { console.error("[DeliveryLoads] Notes save failed:", xhr.status, xhr.responseText); })
            .always(function () { loadData(); });
        }

        $input.on("keydown", function (e) {
            if (e.key === "Enter") {
                e.preventDefault();
                $input.off("blur");
                commitNotes();
            } else if (e.key === "Escape") {
                e.preventDefault();
                $input.off("blur");
                loadData();
            }
        });

        $input.on("blur", function () { commitNotes(); });
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
                ContainerId: _selectedBinId,
                TransactionId: _binTargetRow.TransactionId
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
    var BOL_LABELS = { N: "None", U: "Universal", A: "Along Side the Field", F: "Farm Storage", C: "Custom" };

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
        setTimeout(function () { $(sel.lotPin).focus(); }, 500);
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

    // ── Void / Restore ─────────────────────────────────────────────────────

    function initVoidRestore() {
        $(sel.voidSaveBtn).on("click", submitVoid);
        $(sel.restoreSaveBtn).on("click", submitRestore);
        $(sel.voidPin).on("keydown", function (e) { if (e.key === "Enter") { e.preventDefault(); $(sel.voidSaveBtn).click(); } });
        $(sel.restorePin).on("keydown", function (e) { if (e.key === "Enter") { e.preventDefault(); $(sel.restoreSaveBtn).click(); } });
    }

    function openVoidModal(rowData) {
        $(sel.voidTxnId).val(rowData.TransactionId);
        $(sel.voidReason).val("");
        $(sel.voidPin).val("");
        $(sel.voidError).attr("hidden", true);
        $(sel.voidSaveBtn).prop("disabled", false).text("Void Ticket");
        _voidModal.show();
        setTimeout(function () { $(sel.voidPin).focus(); }, 500);
    }

    function openRestoreModal(rowData) {
        $(sel.restoreTxnId).val(rowData.TransactionId);
        $(sel.restorePin).val("");
        $(sel.restoreError).attr("hidden", true);
        $(sel.restoreSaveBtn).prop("disabled", false).text("Restore Ticket");
        _restoreModal.show();
        setTimeout(function () { $(sel.restorePin).focus(); }, 500);
    }

    function submitVoid() {
        var txnId  = $(sel.voidTxnId).val();
        var reason = $(sel.voidReason).val().trim();
        var pin    = parseInt($(sel.voidPin).val(), 10);

        if (!reason) {
            $(sel.voidError).text("Void reason is required.").removeAttr("hidden");
            return;
        }
        if (!pin || pin <= 0) {
            $(sel.voidError).text("A valid PIN is required.").removeAttr("hidden");
            return;
        }

        $(sel.voidSaveBtn).prop("disabled", true).text("Voiding…");
        $(sel.voidError).attr("hidden", true);

        $.ajax({
            url: "/api/GrowerDelivery/" + txnId + "/void",
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify({ Pin: pin, VoidReason: reason }),
        })
        .done(function () {
            _voidModal.hide();
            showAlert("Ticket " + txnId + " has been voided.", "success");
            loadData();
        })
        .fail(function (xhr) {
            var msg = xhr.responseJSON && xhr.responseJSON.message
                ? xhr.responseJSON.message : "Failed to void ticket.";
            $(sel.voidError).text(msg).removeAttr("hidden");
        })
        .always(function () {
            $(sel.voidSaveBtn).prop("disabled", false).text("Void Ticket");
        });
    }

    function submitRestore() {
        var txnId = $(sel.restoreTxnId).val();
        var pin   = parseInt($(sel.restorePin).val(), 10);

        if (!pin || pin <= 0) {
            $(sel.restoreError).text("A valid PIN is required.").removeAttr("hidden");
            return;
        }

        $(sel.restoreSaveBtn).prop("disabled", true).text("Restoring…");
        $(sel.restoreError).attr("hidden", true);

        $.ajax({
            url: "/api/GrowerDelivery/" + txnId + "/restore",
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify({ Pin: pin }),
        })
        .done(function () {
            _restoreModal.hide();
            showAlert("Ticket " + txnId + " has been restored.", "success");
            loadData();
        })
        .fail(function (xhr) {
            var msg = xhr.responseJSON && xhr.responseJSON.message
                ? xhr.responseJSON.message : "Failed to restore ticket.";
            $(sel.restoreError).text(msg).removeAttr("hidden");
        })
        .always(function () {
            $(sel.restoreSaveBtn).prop("disabled", false).text("Restore Ticket");
        });
    }

    // ── Reprint ──────────────────────────────────────────────────────────────

    function initReprint() {
        $(sel.reprintSendBtn).on("click", submitReprint);
        $(sel.browserPrintBtn).on("click", browserPrint);
        loadPrinterList();
    }

    var _printConn = null;
    var _printConnReady = false;
    var _printersLoaded = false;

    function loadPrinterList() {
        if (typeof signalR === "undefined") return;

        _printConn = new signalR.HubConnectionBuilder()
            .withUrl("/hubs/print")
            .withAutomaticReconnect()
            .build();

        _printConn.on("PrinterListReceived", function (data) {
            var printers = (data.printers || []).map(function (p) {
                return { id: data.serviceId + ":" + p.printerId, name: p.displayName || p.printerId };
            });
            _connectedPrinters = _connectedPrinters.concat(printers);
            _printersLoaded = true;
            populatePrinterDropdown();
        });

        _printConn.on("PrintServiceStatusChanged", function () {
            // If no services connected, mark as loaded (empty)
            if (_printConn) {
                _printConn.invoke("GetConnectedPrintServices").then(function (ids) {
                    if (!ids || ids.length === 0) {
                        _printersLoaded = true;
                        populatePrinterDropdown();
                    }
                }).catch(function () {});
            }
        });

        _printConn.start().then(function () {
            _printConnReady = true;
        }).catch(function () {
            console.warn("[DeliveryLoads] Could not connect to PrintHub.");
            _printConnReady = true;
            _printersLoaded = true;
        });
    }

    function populatePrinterDropdown() {
        var $sel = $(sel.reprintPrinterSel);
        $sel.empty();
        $sel.append('<option value="__default__">Default (Inbound role)</option>');
        _connectedPrinters.forEach(function (p) {
            $sel.append($("<option>").val(p.id).text(p.name));
        });
        // Enable controls
        $(sel.reprintSendBtn).prop("disabled", false);
        $(sel.browserPrintBtn).prop("disabled", false);
    }

    function showPrinterSpinner() {
        var $sel = $(sel.reprintPrinterSel);
        $sel.empty().append('<option value="">Loading printers…</option>');
        $(sel.reprintSendBtn).prop("disabled", true);
    }

    function openReprintModal(rowData) {
        $(sel.reprintTxnId).val(rowData.TransactionId);
        $(sel.reprintError).attr("hidden", true);
        $(sel.reprintSendBtn).prop("disabled", true).text("Print");

        if (_printersLoaded) {
            // Already have printer list
            populatePrinterDropdown();
            _reprintModal.show();
            return;
        }

        // Show spinner while waiting for printers
        showPrinterSpinner();
        _reprintModal.show();

        // Request printer list if connection is ready
        if (_printConnReady && _printConn) {
            _connectedPrinters = [];
            _printConn.invoke("RequestPrinterList").catch(function () {});
        }

        // Poll until printers arrive or timeout (5s)
        var attempts = 0;
        var maxAttempts = 25; // 25 x 200ms = 5s
        var poll = setInterval(function () {
            attempts++;
            if (_printersLoaded) {
                clearInterval(poll);
                populatePrinterDropdown();
            } else if (attempts >= maxAttempts) {
                clearInterval(poll);
                _printersLoaded = true;
                populatePrinterDropdown();
                if (_connectedPrinters.length === 0) {
                    $(sel.reprintError).text("No print services responded. You can still use Browser Print or the default printer.").removeAttr("hidden");
                }
            }
        }, 200);
    }

    function submitReprint() {
        var txnId = $(sel.reprintTxnId).val();
        var printerId = $(sel.reprintPrinterSel).val();

        $(sel.reprintSendBtn).prop("disabled", true).text("Printing…");
        $(sel.reprintError).attr("hidden", true);

        var url;
        if (!printerId || printerId === "__default__") {
            // Use the role-based default printer
            url = "/api/printing/reprint/" + encodeURIComponent(txnId) + "?role=Inbound";
        } else {
            url = "/api/printing/printer/" + encodeURIComponent(printerId) + "/print-ticket/" + encodeURIComponent(txnId);
        }

        $.ajax({
            url: url,
            method: "POST",
        })
        .done(function () {
            _reprintModal.hide();
            showAlert("Print job sent for ticket " + txnId + ".", "success");
        })
        .fail(function (xhr) {
            var msg = xhr.responseJSON && xhr.responseJSON.error
                ? xhr.responseJSON.error : "Print failed.";
            $(sel.reprintError).text(msg).removeAttr("hidden");
        })
        .always(function () {
            $(sel.reprintSendBtn).prop("disabled", false).text("Print");
        });
    }

    function browserPrint() {
        var txnId = $(sel.reprintTxnId).val();
        if (!txnId) return;
        // Open the PDF in a new tab — the browser's native print dialog will handle it
        window.open("/api/printjobs/load-ticket/" + encodeURIComponent(txnId) + "/pdf", "_blank");
        _reprintModal.hide();
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
