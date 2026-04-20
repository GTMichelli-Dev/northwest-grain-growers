(function () {
    'use strict';

    // ── Helpers ─────────────────────────────────────────────────────────────

    function formatLotId(id) {
        var s = String(id);
        if (s.length < 7) return s;
        return s.substring(0, 3) + '-' + s.substring(3, 6) + '-' + s.substring(6);
    }

    // ── Selectors ────────────────────────────────────────────────────────────

    const SEL = {
        form:           '#gmGdForm',
        alert:          '#gmGdAlert',
        submit:         '#gmGdSubmitTop',

        // Scale partial hidden inputs (owned by _ScaleWeight)
        scaleValue:     '#gwSwValue',

        // Weight sheet panels
        wsPanel:            '#gdWsPanel',
        wsList:             '#gdWsList',
        newWsBtn:           '#gdNewWsBtn',
        newWsPanel:         '#gdNewWsPanel',
        newWsError:         '#gdNewWsError',
        createWsBtn:        '#gdCreateWsBtn',
        cancelNewWsBtn:     '#gdCancelNewWsBtn',
        weightSheetUid:     '#gdWeightSheetUid',

        // New WS lot create form (inside _NewWeightSheet)
        nwsLotList:         '#gdNwsLotList',
        nwsAccount:         '#gdNwsAccount',
        nwsSplitGroup:      '#gdNwsSplitGroup',
        nwsLotDesc:         '#gdNwsLotDesc',
        nwsCreateError:     '#gdNwsCreateError',
        createWsLotBtn:     '#gdCreateWsLotBtn',
        nwsSelectedLotName: '#gdNwsSelectedLotName',

        // WS header (shown when wsId is provided)
        wsHeader:       '#gdWsHeader',
        wsIdFmt:        '#gdWsIdFormatted',
        wsLot:          '#gdWsLot',
        wsHauler:       '#gdWsHauler',

        // Quantity method
        qtyMethod:      '#gdQtyMethod',
        qtyMethodId:    '#gdQtyMethodId',
        qtyMethodCode:  '#gdQtyMethodCode',

        // Scale mode (TRUCK_SCALE)
        scaleMode:      '#gdScaleMode',
        captureGross:   '#gdCaptureGross',
        grossDisplay:   '#gdGrossDisplay',
        grossTime:      '#gdGrossTime',
        grossRow:       '#gdGrossRow',
        startQty:       '#gdStartQty',
        startedAt:      '#gdStartedAt',
        startQtyIsManual:'#gdStartQtyIsManual',
        grossSourceBadge:'#gdGrossSourceBadge',

        captureTare:    '#gdCaptureTare',
        tareDisplay:    '#gdTareDisplay',
        tareTime:       '#gdTareTime',
        tareRow:        '#gdTareRow',
        endQty:         '#gdEndQty',
        completedAt:    '#gdCompletedAt',
        endQtyIsManual: '#gdEndQtyIsManual',
        tareSourceBadge:'#gdTareSourceBadge',

        scaleNetRow:    '#gdScaleNetRow',
        netDisplay:     '#gdNetDisplay',

        // Direct mode (MANUAL/RAIL/BULKLOAD)
        directMode:     '#gdDirectMode',
        enterAmountBtn: '#gdEnterAmountBtn',
        directDisplay:  '#gdDirectDisplay',
        directTime:     '#gdDirectTime',
        directQty:      '#gdDirectQty',
        directStartedAt:'#gdDirectStartedAt',
        directCompletedAt:'#gdDirectCompletedAt',

        // Enter Amount Modal
        enterAmountModal:   '#gdEnterAmountModal',
        directAmountInput:  '#gdDirectAmountInput',
        directPinInput:     '#gdDirectPinInput',
        directAmountError:  '#gdDirectAmountError',
        directAmountConfirm:'#gdDirectAmountConfirm',

        // Capture Weight Modal
        captureWeightModal: '#gdCaptureWeightModal',
        captureScaleList:   '#gdCaptureScaleList',
        captureManualBtn:   '#gdCaptureManualBtn',
        captureManualPanel: '#gdCaptureManualPanel',
        captureManualInput: '#gdCaptureManualInput',
        capturePinInput:    '#gdCapturePinInput',
        captureManualConfirm:'#gdCaptureManualConfirm',
        captureWeightError: '#gdCaptureWeightError',

        // Hidden FK inputs
        accountId:      '#gdAccountId',
        productId:      '#gdProductId',
        lotId:          '#gdLotId',
        locationId:     '#gdLocationId',
        splitGroupId:   '#gdSplitGroupId',
        containerId:    '#gdContainerId',

        // New lot modal
        newLotBtn:      '#gdNewLotBtn',
        newLotModal:    '#gdNewLotModal',
        newLotProduct:  '#gdNewLotProductName',
        newLotDesc:     '#gdNewLotDesc',
        newLotError:    '#gdNewLotError',
        newLotSave:     '#gdNewLotSave',
    };

    // ── Cookie helpers (delegate to global GM namespace) ──────────────────

    function getLocationCookie() { return GM.getLocationId(); }
    function setLocationCookie(val) { GM.setLocationId(val); }

    // ── State ────────────────────────────────────────────────────────────────

    let grossCaptured         = false;
    let currentProductName    = null;
    let newLotModalInstance   = null;
    let currentWeightSheetUid = null;
    let selectedNwsLotId      = null;   // lot selected in the New WS panel
    let selectedNwsLotDesc    = null;
    let activeWsId            = null;   // the weight sheet loaded from cookie/URL
    let _nwsPin               = null;  // PIN captured from popup for new WS creation
    let editTxnId             = null;   // set when editing an existing delivery
    let editOriginalWeights   = null;  // { StartQty, EndQty, DirectQty } at load time
    let bolModalInstance      = null;

    // Weight method state
    let currentQtyMethodId    = null;
    let currentQtyMethodCode  = null;
    let quantityMethods       = [];     // loaded from API
    let quantitySourceTypes   = [];     // loaded from API
    let cachedScales          = [];     // scales at current location
    let enterAmountModalInst  = null;
    let captureWeightModalInst= null;
    let captureTarget         = null;   // 'start' or 'end' — which qty the modal is capturing
    let locationName          = null;   // location description for source tracking
    let lastPinUserId         = null;   // user ID from last successful PIN validation
    let lastPinUserName       = null;   // user name from last successful PIN validation
    let lastStartScaleDesc    = null;   // scale description used for StartQty capture
    let lastEndScaleDesc      = null;   // scale description used for EndQty capture

    // ── Cookie helper for WsId ──────────────────────────────────────────────

    function getWsIdCookie() {
        var match = document.cookie.match(/(?:^|;\s*)GrainMgmt_WsId=(\d+)/);
        return match ? parseInt(match[1], 10) : 0;
    }

    // ── Init ─────────────────────────────────────────────────────────────────

    $(function () {
        var locationId = GM.getLocationId();
        if (!locationId) {
            showAlert('Please select a location from the Warehouse dashboard first.', 'warning');
            return;
        }

        $(SEL.locationId).val(locationId);

        // Check for active weight sheet: cookie first, then URL param
        var wsId = getWsIdCookie()
                || parseInt(new URLSearchParams(window.location.search).get('wsId'), 10) || 0;

        if (wsId > 0) {
            // Weight sheet is known — hide the WS list, show form directly
            activeWsId = wsId;
            $(SEL.wsPanel).prop('hidden', true);
            $('#gdNwsPanel').prop('hidden', true);
            $('#gdFormBody').prop('hidden', false);
            loadWsHeader(locationId, wsId);
        } else {
            // No active WS — show the WS list as before
            $(SEL.wsPanel).prop('hidden', false);
            $('#gdFormBody').prop('hidden', false);
            refreshWeightSheets(locationId);
        }

        // Check for edit mode early so init functions can detect it
        var txnId = parseInt(new URLSearchParams(window.location.search).get('txnId'), 10) || 0;
        if (txnId > 0) editTxnId = txnId;

        // Preload bins in parallel with everything else
        var binPromise = $.getJSON('/api/locations/' + locationId + '/Containers').catch(function () { return []; });

        initSelectBoxes(binPromise);
        initNwsAccountPicker();
        initNwsSplitGroupPicker();
        initQtyMethodPicker(locationId);
        wireWeightCapture();
        wireDirectQtyModal();
        wireCaptureWeightModal(locationId);
        wireNewLotModal();
        wireWeightSheetPanel();
        wireBolChangeModal();
        wireSubmit();

        // ── Edit mode: load existing transaction if txnId is in URL ─────────
        // Defer until async inits (qty method picker, selectboxes) have resolved
        if (editTxnId) {
            setTimeout(function () { loadExistingDelivery(editTxnId); }, 500);
        }
    });

    // ── Weight sheet header (shown when wsId is in URL) ─────────────────────

    function formatWsId(id) {
        var s = String(id);
        if (s.length < 7) return s;
        return s.substring(0, 3) + '-' + s.substring(3, 6) + '-' + s.substring(6);
    }

    var BOL_LABELS = { N: 'None', U: 'Universal', A: 'Along Side Field', F: 'Farm Storage', C: 'Custom' };

    function loadWsHeader(locationId, wsId) {
        $.ajax({
            url: '/api/GrowerDelivery/WeightSheet/' + wsId,
            method: 'GET',
            dataType: 'json',
        })
        .done(function (ws) {
            if (!ws) return;
            var fmtId = ws.WsAs400Id ? String(ws.WsAs400Id) : formatWsId(ws.WeightSheetId);
            $(SEL.wsIdFmt).text(fmtId);
            $('#gdWsLotId').text(ws.LotId ? (ws.LotAs400Id ? String(ws.LotAs400Id) : formatLotId(ws.LotId)) : '\u2014');
            $('#gdWsCrop').text(ws.CropName || '\u2014');
            $('#gdWsAccount').text(ws.PrimaryAccountName || '\u2014');
            $('#gdWsSplit').text(ws.SplitName || '\u2014');

            // Hauler (always shown)
            $('#gdWsHauler').text(ws.HaulerName || 'Grower');

            // BOL Type
            $('#gdWsBolType').text(BOL_LABELS[ws.RateType] || ws.RateType || 'Not Set');

            // Show/hide conditional BOL detail fields
            var rt = ws.RateType;
            var isAF = rt === 'A' || rt === 'F';
            var isC  = rt === 'C';
            var showDetails = isAF || isC;

            $('#gdWsBolDetailsRow').prop('hidden', !showDetails);
            $('#gdWsMilesField').prop('hidden', !isAF);
            $('#gdWsMiles').text(ws.Miles != null ? ws.Miles : '\u2014');
            $('#gdWsCalcRateField').prop('hidden', !isAF);
            $('#gdWsCalcRate').text(ws.Rate != null ? '$' + Number(ws.Rate).toFixed(2) : '\u2014');
            $('#gdWsCustomDescField').prop('hidden', !isC);
            $('#gdWsCustomDesc').text(ws.CustomRateDescription || '\u2014');
            $('#gdWsCustomRateField').prop('hidden', !isC);
            $('#gdWsCustomRate').text(ws.Rate != null ? '$' + Number(ws.Rate).toFixed(2) : '\u2014');

            // Set back links to delivery loads for this weight sheet
            var backUrl = '/GrowerDelivery/WeightSheetDeliveryLoads?wsId=' + ws.WeightSheetId;
            $('#gdCancelBtn').attr('href', backUrl);
            $('.gm-module-bar--intake').attr('href', backUrl);

            $(SEL.wsHeader).removeAttr('hidden');

            // Pre-fill hidden form fields from the weight sheet
            if (ws.LotId) $(SEL.lotId).val(ws.LotId);
            if (ws.LocationId) $(SEL.locationId).val(ws.LocationId);

            // Store the WS RowUid for load submission — fetch from open weight sheets
            $.getJSON('/api/GrowerDelivery/OpenWeightSheets', { locationId: locationId })
                .done(function (sheets) {
                    var match = sheets.find(function (s) { return s.WeightSheetId === ws.WeightSheetId; });
                    if (match && match.RowUid) {
                        selectWeightSheet(match.RowUid);
                    }
                });
        })
        .fail(function () {
            console.warn('[GrowerDelivery] Failed to load WS header for wsId=' + wsId);
        });
    }

    // ── SelectBox initialization ─────────────────────────────────────────────

    async function initSelectBoxes(binPromise) {
        // ── Account (Grower) ──────────────────────────────────────────────────
        $('#gdAccount').dxSelectBox({
            dataSource: new DevExpress.data.DataSource({
                store: new DevExpress.data.CustomStore({
                    key:  'AccountId',
                    load: () => $.getJSON('/api/Lookups/Accounts')
                })
            }),
            valueExpr:    'AccountId',
            displayExpr:  'Name',
            searchEnabled: true,
            placeholder:  'Select grower…',
            onValueChanged: function (e) {
                $(SEL.accountId).val(e.value ?? '');
            }
        });

        // ── Product ───────────────────────────────────────────────────────────
        $('#gdProduct').dxSelectBox({
            dataSource: new DevExpress.data.DataSource({
                store: new DevExpress.data.CustomStore({
                    key:  'ProductId',
                    load: () => $.getJSON('/api/Lookups/WarehouseItems')
                })
            }),
            valueExpr:    'ItemId',
            displayExpr:  'Name',
            searchEnabled: true,
            placeholder:  'Select Item…',
            onValueChanged: function (e) {
                $(SEL.productId).val(e.value ?? '');
                currentProductName = e.component.option('text') || null;
                refreshLots();
                // Enable + New Lot button only after a product is selected
                $(SEL.newLotBtn)
                    .prop('disabled', !e.value)
                    .attr('title', e.value ? 'Create a new lot for this product' : 'Select a product first');
            }
        });

        // ── Lot ───────────────────────────────────────────────────────────────
        $('#gdLot').dxSelectBox({
            dataSource: new DevExpress.data.DataSource({
                store: new DevExpress.data.CustomStore({
                    key:  'Id',
                    load: function () {
                        const productId = $(SEL.productId).val();
                        const qs = productId ? '?productId=' + productId : '';
                        return $.getJSON('/api/Lookups/Lots' + qs);
                    }
                })
            }),
            valueExpr:    'Id',
            displayExpr:  'LotNumber',
            searchEnabled: true,
            placeholder:  'Select lot…',
            onValueChanged: function (e) {
                $(SEL.lotId).val(e.value ?? '');
            }
        });

        // ── Split Group (optional) ────────────────────────────────────────────
        $('#gdSplitGroup').dxSelectBox({
            dataSource: new DevExpress.data.DataSource({
                store: new DevExpress.data.CustomStore({
                    key:  'SplitGroupId',
                    load: () => $.getJSON('/api/Lookups/SplitGroups')
                })
            }),
            valueExpr:    'SplitGroupId',
            displayExpr:  'Name',
            searchEnabled: true,
            showClearButton: true,
            placeholder:  'None',
            onValueChanged: function (e) {
                $(SEL.splitGroupId).val(e.value ?? '');
            }
        });

        // ── Bin (optional) — preloaded for the current location ───────────────
        var rawBins = await binPromise;
        var binData = (rawBins || []).map(function (b) {
            var name = b.Description || b.ContainerDescription || '';
            if (b.LocationDescription) name = b.LocationDescription + ' — ' + name;
            return { Id: b.ContainerId, Name: name };
        });

        $('#gdContainer').dxSelectBox({
            dataSource: binData,
            valueExpr:    'Id',
            displayExpr:  'Name',
            searchEnabled: true,
            showClearButton: true,
            placeholder:  'None',
            onValueChanged: function (e) {
                $(SEL.containerId).val(e.value ?? '');
            }
        });
    }

    function refreshLots() {
        const instance = $('#gdLot').dxSelectBox('instance');
        if (!instance) return;
        instance.reset();
        $(SEL.lotId).val('');
        instance.getDataSource().reload();
    }

    // ── New Lot Modal ─────────────────────────────────────────────────────────

    function wireNewLotModal() {
        // Bootstrap modal instance (lazily created)
        function getModal() {
            if (!newLotModalInstance) {
                const el = document.querySelector(SEL.newLotModal);
                if (el) newLotModalInstance = new bootstrap.Modal(el);
            }
            return newLotModalInstance;
        }

        // Open modal — pre-fill the product name
        $(SEL.newLotBtn).on('click', function () {
            $(SEL.newLotProduct).text(currentProductName || '—');
            $(SEL.newLotDesc).val('');
            $(SEL.newLotError).prop('hidden', true).text('');
            getModal()?.show();
        });

        // Save new lot
        $(SEL.newLotSave).on('click', async function () {
            const productId = parseInt($(SEL.productId).val(), 10);
            const desc      = $(SEL.newLotDesc).val().trim();
            const splitId   = parseInt($(SEL.splitGroupId).val(), 10) || null;

            if (!desc) {
                $(SEL.newLotError).text('Lot description is required.').prop('hidden', false);
                return;
            }

            const btn = $(SEL.newLotSave);
            btn.prop('disabled', true).text('Creating…');
            $(SEL.newLotError).prop('hidden', true);

            try {
                const resp = await fetch('/api/Lookups/Lots', {
                    method:  'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body:    JSON.stringify({
                        ProductId:      productId,
                        LotDescription: desc,
                        SplitGroupId:   splitId,
                    })
                });

                if (!resp.ok) {
                    const detail = await tryParseError(resp);
                    $(SEL.newLotError).text('Error: ' + detail).prop('hidden', false);
                    return;
                }

                const created = await resp.json();   // { id, lotNumber }

                // Inject the new lot into the SelectBox and select it
                const lotInstance = $('#gdLot').dxSelectBox('instance');
                await lotInstance.getDataSource().reload();
                lotInstance.option('value', created.id);
                $(SEL.lotId).val(created.id);

                getModal()?.hide();

            } catch (ex) {
                $(SEL.newLotError).text('Network error: ' + ex.message).prop('hidden', false);
            } finally {
                btn.prop('disabled', false).text('Create Lot');
            }
        });
    }

    // ══════════════════════════════════════════════════════════════════════════
    // QUANTITY METHOD PICKER
    // ══════════════════════════════════════════════════════════════════════════

    const DIRECT_CODES = ['MANUAL', 'RAIL', 'BULKLOADER'];

    function isDirectMode() { return DIRECT_CODES.indexOf(currentQtyMethodCode) >= 0; }

    async function initQtyMethodPicker(locationId) {
        // Load quantity methods for this location
        try {
            quantityMethods = await $.getJSON('/api/Lookups/QuantityMethods?locationId=' + locationId);
        } catch (_) {
            quantityMethods = [];
        }

        // Load source types (global)
        try {
            quantitySourceTypes = await $.getJSON('/api/Lookups/QuantitySourceTypes');
        } catch (_) {
            quantitySourceTypes = [];
        }

        // Load location name for source tracking
        try {
            var locations = await $.getJSON('/api/Scale/Locations');
            var loc = locations.find(function (l) { return l.LocationId === parseInt(locationId, 10); });
            locationName = loc ? loc.Name : 'Unknown';
        } catch (_) {
            locationName = 'Unknown';
        }

        $(SEL.qtyMethod).dxSelectBox({
            dataSource: quantityMethods,
            valueExpr: 'QuantityMethodId',
            displayExpr: 'Description',
            placeholder: 'Select weight method…',
            value: findMethodIdByCode('TRUCK_SCALE'),
            onValueChanged: function (e) {
                var method = quantityMethods.find(function (m) { return m.QuantityMethodId === e.value; });
                currentQtyMethodId   = e.value || null;
                currentQtyMethodCode = method ? method.Code : null;
                $(SEL.qtyMethodId).val(e.value || '');
                $(SEL.qtyMethodCode).val(currentQtyMethodCode || '');
                switchWeightMode(currentQtyMethodCode);
            }
        });

        // Set initial state — default to TRUCK_SCALE, fall back to first available
        var defaultId   = findMethodIdByCode('TRUCK_SCALE');
        var defaultCode = 'TRUCK_SCALE';
        if (!defaultId && quantityMethods.length > 0) {
            defaultId   = quantityMethods[0].QuantityMethodId;
            defaultCode = quantityMethods[0].Code;
        }
        if (defaultId) {
            currentQtyMethodId   = defaultId;
            currentQtyMethodCode = defaultCode;
            $(SEL.qtyMethodId).val(defaultId);
            $(SEL.qtyMethodCode).val(defaultCode);
            $(SEL.qtyMethod).dxSelectBox('instance').option('value', defaultId);
        }
        switchWeightMode(currentQtyMethodCode || 'TRUCK_SCALE');
    }

    function findMethodIdByCode(code) {
        var m = quantityMethods.find(function (x) { return x.Code === code; });
        return m ? m.QuantityMethodId : null;
    }

    function findSourceTypeIdByCode(code) {
        var s = quantitySourceTypes.find(function (x) { return x.Code === code; });
        return s ? s.QuantitySourceTypeId : null;
    }

    function currentMethodDescription() {
        var m = quantityMethods.find(function (x) { return x.QuantityMethodId === currentQtyMethodId; });
        return m ? m.Description : null;
    }

    function switchWeightMode(code) {
        if (editTxnId) {
            // In edit mode, just show/hide the right panel — don't reset values
            if (DIRECT_CODES.indexOf(code) >= 0) {
                $(SEL.scaleMode).prop('hidden', true);
                $(SEL.directMode).prop('hidden', false);
            } else {
                $(SEL.scaleMode).prop('hidden', false);
                $(SEL.directMode).prop('hidden', true);
            }
            return;
        }
        $(SEL.submit).prop('hidden', true);
        if (DIRECT_CODES.indexOf(code) >= 0) {
            // DirectQty mode
            $(SEL.scaleMode).prop('hidden', true);
            $(SEL.directMode).prop('hidden', false);
            // Reset scale fields
            resetScaleFields();
        } else {
            // Scale mode (TRUCK_SCALE)
            $(SEL.scaleMode).prop('hidden', false);
            $(SEL.directMode).prop('hidden', true);
            // Reset direct fields
            resetDirectFields();
        }
    }

    function resetScaleFields() {
        grossCaptured = false;
        lastStartScaleDesc = null;
        lastEndScaleDesc   = null;
        $(SEL.startQty).val('');
        $(SEL.endQty).val('');
        $(SEL.startedAt).val('');
        $(SEL.completedAt).val('');
        $(SEL.startQtyIsManual).val('');
        $(SEL.endQtyIsManual).val('');
        $(SEL.grossDisplay).text('— lbs');
        $(SEL.tareDisplay).text('— lbs');
        $(SEL.netDisplay).text('— lbs');
        $(SEL.grossTime).text('—');
        $(SEL.tareTime).text('—');
        $(SEL.grossRow).removeClass('gm-gd-weight-cell--captured');
        $(SEL.tareRow).prop('hidden', true).removeClass('gm-gd-weight-cell--captured');
        $(SEL.scaleNetRow).prop('hidden', true);
        $(SEL.captureTare).prop('disabled', true);
        $(SEL.grossSourceBadge).prop('hidden', true).text('');
        $(SEL.tareSourceBadge).prop('hidden', true).text('');
    }

    function resetDirectFields() {
        $(SEL.directQty).val('');
        $(SEL.directStartedAt).val('');
        $(SEL.directCompletedAt).val('');
        $(SEL.directDisplay).text('— lbs');
        $(SEL.directTime).text('—');
    }

    // ══════════════════════════════════════════════════════════════════════════
    // WEIGHT CAPTURE (scale mode)
    // ══════════════════════════════════════════════════════════════════════════

    function wireWeightCapture() {
        // Open capture weight modal for Inbound
        $(SEL.captureGross).on('click', function () {
            captureTarget = 'start';
            openCaptureWeightModal();
        });

        // Open capture weight modal for Outbound
        $(SEL.captureTare).on('click', function () {
            captureTarget = 'end';
            openCaptureWeightModal();
        });
    }

    function applyScaleWeight(weight, isManual, scaleDesc) {
        var now = new Date();
        if (captureTarget === 'start') {
            lastStartScaleDesc = isManual ? null : (scaleDesc || null);
            if (weight <= 0) {
                showAlert('Inbound weight must be greater than 0.', 'danger');
                return false;
            }
            $(SEL.startQty).val(weight);
            $(SEL.startedAt).val(now.toISOString());
            $(SEL.grossDisplay).text(fmtWeight(weight));
            $(SEL.grossTime).text(fmtTime(now));
            $(SEL.grossRow).addClass('gm-gd-weight-cell--captured');
            $(SEL.startQtyIsManual).val(isManual ? '1' : '');
            var grossBadgeText = isManual ? 'Manual' : (scaleDesc || '');
            $(SEL.grossSourceBadge).text(grossBadgeText).prop('hidden', !grossBadgeText);

            grossCaptured = true;
            $(SEL.tareRow).prop('hidden', false);
            $(SEL.captureTare).prop('disabled', false);
            $(SEL.submit).prop('hidden', false);

            // Recalculate net if outbound was already captured
            updateScaleNet();
        } else {
            lastEndScaleDesc = isManual ? null : (scaleDesc || null);
            var startQty = parseInt($(SEL.startQty).val(), 10);
            if (isManual) {
                if (weight > startQty) {
                    showAlert('Manual outbound weight cannot be greater than inbound weight (' + fmtWeight(startQty) + ').', 'danger');
                    return false;
                }
                if (weight < 0) {
                    showAlert('Outbound weight must be 0 or greater.', 'danger');
                    return false;
                }
            }
            $(SEL.endQty).val(weight);
            $(SEL.completedAt).val(now.toISOString());
            $(SEL.tareDisplay).text(fmtWeight(weight));
            $(SEL.tareTime).text(fmtTime(now));
            $(SEL.tareRow).addClass('gm-gd-weight-cell--captured');
            $(SEL.endQtyIsManual).val(isManual ? '1' : '');
            var tareBadgeText = isManual ? 'Manual' : (scaleDesc || '');
            $(SEL.tareSourceBadge).text(tareBadgeText).prop('hidden', !tareBadgeText);

            updateScaleNet();
        }
        return true;
    }

    function updateScaleNet() {
        var gross = parseInt($(SEL.startQty).val(), 10);
        var tare  = parseInt($(SEL.endQty).val(), 10);

        if (!isNaN(gross) && !isNaN(tare)) {
            $(SEL.netDisplay).text(fmtWeight(Math.abs(gross - tare)));
            $(SEL.scaleNetRow).prop('hidden', false);
        }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // ENTER AMOUNT MODAL (DirectQty mode)
    // ══════════════════════════════════════════════════════════════════════════

    function wireDirectQtyModal() {
        $(SEL.enterAmountBtn).on('click', function () {
            if (!enterAmountModalInst) {
                enterAmountModalInst = new bootstrap.Modal(document.querySelector(SEL.enterAmountModal));
            }
            $(SEL.directAmountInput).val('');
            $(SEL.directPinInput).val('');
            $(SEL.directAmountError).prop('hidden', true);
            enterAmountModalInst.show();
        });

        $(SEL.directAmountConfirm).on('click', async function () {
            var amount = parseInt($(SEL.directAmountInput).val(), 10);
            var pin    = $(SEL.directPinInput).val().trim();

            if (isNaN(amount) || amount <= 0) {
                $(SEL.directAmountError).text('Weight must be greater than 0.').prop('hidden', false);
                return;
            }
            if (!pin) {
                $(SEL.directAmountError).text('PIN is required.').prop('hidden', false);
                return;
            }

            // Validate PIN
            var pinResult = await validatePin(parseInt(pin, 10));
            if (!pinResult.valid) {
                $(SEL.directAmountError).text(pinResult.message).prop('hidden', false);
                return;
            }

            lastPinUserId   = pinResult.userId;
            lastPinUserName = pinResult.userName;

            var now = new Date();
            $(SEL.directQty).val(amount);
            $(SEL.directStartedAt).val(now.toISOString());
            $(SEL.directCompletedAt).val(now.toISOString());
            $(SEL.directDisplay).text(fmtWeight(amount));
            $(SEL.directTime).text(fmtTime(now));
            $(SEL.submit).prop('hidden', false);

            enterAmountModalInst.hide();
        });
    }

    // ══════════════════════════════════════════════════════════════════════════
    // CAPTURE WEIGHT MODAL (Scale mode — lists scales + manual entry)
    // ══════════════════════════════════════════════════════════════════════════

    function wireCaptureWeightModal(locationId) {
        // Load available scales at this location
        loadCachedScales();

        $(SEL.captureManualBtn).on('click', function () {
            $(SEL.captureManualPanel).prop('hidden', false);
            $(SEL.captureManualInput).val('');
            $(SEL.capturePinInput).val('');
            $(SEL.captureWeightError).prop('hidden', true);
        });

        $(SEL.captureManualConfirm).on('click', async function () {
            var weight = parseInt($(SEL.captureManualInput).val(), 10);
            var pin    = $(SEL.capturePinInput).val().trim();

            if (isNaN(weight) || weight < 0) {
                $(SEL.captureWeightError).text('Weight must be 0 or greater.').prop('hidden', false);
                return;
            }
            if (captureTarget === 'start' && weight <= 0) {
                $(SEL.captureWeightError).text('Inbound weight must be greater than 0.').prop('hidden', false);
                return;
            }
            if (captureTarget === 'end') {
                var startQty = parseInt($(SEL.startQty).val(), 10);
                if (weight > startQty) {
                    $(SEL.captureWeightError).text('Outbound weight cannot exceed inbound weight (' + fmtWeight(startQty) + ').').prop('hidden', false);
                    return;
                }
            }
            if (!pin) {
                $(SEL.captureWeightError).text('PIN is required for manual entry.').prop('hidden', false);
                return;
            }

            var pinResult = await validatePin(parseInt(pin, 10));
            if (!pinResult.valid) {
                $(SEL.captureWeightError).text(pinResult.message).prop('hidden', false);
                return;
            }

            lastPinUserId   = pinResult.userId;
            lastPinUserName = pinResult.userName;

            var ok = applyScaleWeight(weight, true, null);
            if (ok) {
                captureWeightModalInst.hide();
            }
        });
    }

    async function loadCachedScales() {
        try {
            cachedScales = await $.getJSON('/api/Scale/CachedScales');
        } catch (_) {
            cachedScales = [];
        }
    }

    // Scale status colors matching /Scales page (scales.css)
    var SCALE_STYLES = {
        error:  { bg: '#ffc0cb', border: '#e28aa0', color: '#000', label: 'No Connection',   labelColor: '#c62828' },
        motion: { bg: '#fff9c4', border: '#e6d95e', color: '#000', label: 'Motion',           labelColor: '#e65100' },
        ok:     { bg: '#d4edda', border: '#a3d5b1', color: '#000', label: 'Stable',           labelColor: '#2e7d32' }
    };

    function getScaleStatus(s) {
        if (!s.Ok) return 'error';
        if (s.Motion) return 'motion';
        return 'ok';
    }

    var _scalePollTimer = null;

    function renderScaleList() {
        var listEl = $(SEL.captureScaleList);
        listEl.empty();

        if (cachedScales.length === 0) {
            listEl.html('<span class="text-muted small">No scales available at this location.</span>');
            return;
        }

        cachedScales.forEach(function (s) {
            var status = getScaleStatus(s);
            var style  = SCALE_STYLES[status];
            var weight = Math.round(s.Weight || 0);

            // Disable if: error, motion, or weight < 1000
            var isDisabled = status !== 'ok' || weight < 1000;
            var disabledReason = '';

            if (status === 'motion') {
                isDisabled = true;
                // Motion: still show weight but don't allow selection
            }

            if (status === 'ok' && weight < 1000) {
                isDisabled = true;
                disabledReason = '<span style="color:#c62828;font-size:0.8em;">Below 1,000 lbs minimum</span>';
            }

            // For outbound capture: also disable if weight > inbound weight
            if (captureTarget === 'end' && status === 'ok') {
                var inboundWeight = parseInt($(SEL.startQty).val(), 10);
                if (!isNaN(inboundWeight) && weight > inboundWeight) {
                    isDisabled = true;
                    disabledReason = '<span style="color:#c62828;font-size:0.8em;">Exceeds inbound weight (' + fmtWeight(inboundWeight) + ')</span>';
                }
            }

            // Build button content — always show weight unless error/no connection
            var descSpan  = '<strong>' + escapeHtml(s.Description) + '</strong>';
            var weightSpan = status === 'error'
                ? '<span>—</span>'
                : '<span style="font-size:1.1em;font-weight:600;">' + fmtWeight(weight) + '</span>';
            var statusSpan = '<span style="color:' + style.labelColor + ';font-weight:600;font-size:0.85em;">' + style.label + '</span>';

            var btn = $('<button type="button" class="btn text-start w-100"></button>')
                .css({
                    'background-color': style.bg,
                    'border': '1px solid ' + style.border,
                    'color': style.color,
                    'padding': '10px 14px',
                    'display': 'flex',
                    'justify-content': 'space-between',
                    'align-items': 'center',
                    'gap': '12px',
                    'opacity': isDisabled ? '0.65' : '1'
                })
                .html(
                    '<div>' + descSpan + '<br/>' + statusSpan +
                    (disabledReason ? '<br/>' + disabledReason : '') +
                    '</div>' +
                    '<div style="text-align:right;">' + weightSpan + '</div>'
                )
                .prop('disabled', isDisabled)
                .on('click', function () {
                    var ok = applyScaleWeight(weight, false, s.Description);
                    if (ok) captureWeightModalInst.hide();
                });

            listEl.append(btn);
        });
    }

    function stopScalePoll() {
        if (_scalePollTimer) {
            clearInterval(_scalePollTimer);
            _scalePollTimer = null;
        }
    }

    async function openCaptureWeightModal() {
        if (!captureWeightModalInst) {
            captureWeightModalInst = new bootstrap.Modal(document.querySelector(SEL.captureWeightModal));

            // Stop polling when the modal is closed by any means (X, backdrop, ESC, programmatic)
            document.querySelector(SEL.captureWeightModal).addEventListener('hidden.bs.modal', stopScalePoll);
        }
        $(SEL.captureManualPanel).prop('hidden', true);
        $(SEL.captureWeightError).prop('hidden', true);
        $(SEL.captureManualInput).val('');
        $(SEL.capturePinInput).val('');

        // Initial load and render
        await loadCachedScales();
        renderScaleList();

        // Start live polling while modal is open
        stopScalePoll();
        _scalePollTimer = setInterval(async function () {
            await loadCachedScales();
            renderScaleList();
        }, 1000);

        captureWeightModalInst.show();
    }

    // ── PIN validation helper ───────────────────────────────────────────────

    async function validatePin(pin) {
        try {
            var resp = await fetch('/api/GrowerDelivery/ValidatePin?pin=' + encodeURIComponent(pin));
            if (resp.ok) {
                var data = await resp.json();
                return { valid: true, userId: data.UserId, userName: data.UserName };
            }
            var err = await tryParseError(resp);
            return { valid: false, message: err };
        } catch (ex) {
            return { valid: false, message: 'Network error: ' + ex.message };
        }
    }

    // ── Form submission ──────────────────────────────────────────────────────

    var _weightEditPinModal = null;
    var _pendingSubmitPayload = null;

    function wireSubmit() {
        _weightEditPinModal = new bootstrap.Modal(document.getElementById('gdWeightEditPinModal'));

        $('#gdWeightEditPin').on('keydown', function (e) { if (e.key === 'Enter') { e.preventDefault(); $('#gdWeightEditPinConfirm').click(); } });
        $('#gdWeightEditPinConfirm').on('click', function () {
            var pin = parseInt($('#gdWeightEditPin').val(), 10);
            if (!pin || pin <= 0) {
                $('#gdWeightEditPinError').text('A valid PIN is required.').removeAttr('hidden');
                return;
            }
            _weightEditPinModal.hide();
            if (_pendingSubmitPayload) {
                _pendingSubmitPayload.WeightEditPin = pin;
                doSubmit(_pendingSubmitPayload);
                _pendingSubmitPayload = null;
            }
        });

        $(SEL.form).on('submit', async function (e) {
            e.preventDefault();
            hideAlert();

            const payload = buildPayload();
            const err = validate(payload);
            if (err) {
                showAlert(err, 'danger');
                return;
            }

            // Check if existing weights were modified — require PIN
            if (editTxnId && editOriginalWeights) {
                var startChanged = editOriginalWeights.StartQty != null && (payload.StartQty || null) != editOriginalWeights.StartQty;
                var endChanged   = editOriginalWeights.EndQty != null && (payload.EndQty || null) != editOriginalWeights.EndQty;
                if (startChanged || endChanged) {
                    // Show changes to the operator
                    var changes = [];
                    if (startChanged) changes.push('In Weight: ' + (editOriginalWeights.StartQty || 0).toLocaleString() + ' → ' + (payload.StartQty || 0).toLocaleString());
                    if (endChanged) changes.push('Out Weight: ' + (editOriginalWeights.EndQty || 0).toLocaleString() + ' → ' + (payload.EndQty || 0).toLocaleString());
                    $('#gdWeightEditPinChanges').html('<strong>Changes:</strong><br>' + changes.join('<br>'));
                    $('#gdWeightEditPin').val('');
                    $('#gdWeightEditPinError').attr('hidden', true);
                    _pendingSubmitPayload = payload;
                    _weightEditPinModal.show();
                    setTimeout(function () { $('#gdWeightEditPin').focus(); }, 500);
                    return;
                }
            }

            doSubmit(payload);
        });
    }

    async function doSubmit(payload) {
            const btn = $(SEL.submit);
            btn.prop('disabled', true).text('Saving…');

            try {
                var url = editTxnId
                    ? '/api/GrowerDelivery/' + editTxnId
                    : '/api/GrowerDelivery';
                var method = editTxnId ? 'PUT' : 'POST';

                const resp = await fetch(url, {
                    method:  method,
                    headers: { 'Content-Type': 'application/json' },
                    body:    JSON.stringify(payload)
                });

                if (resp.ok) {
                    const result = await resp.json();

                    // Only print on new creates, or edits where weights changed
                    var shouldPrint = !editTxnId; // always print new
                    if (editTxnId && editOriginalWeights) {
                        var p = payload;
                        shouldPrint = (p.StartQty || null) !== (editOriginalWeights.StartQty || null)
                                   || (p.EndQty || null) !== (editOriginalWeights.EndQty || null)
                                   || (p.DirectQty || null) !== (editOriginalWeights.DirectQty || null);
                    }

                    if (shouldPrint && result && result.id) {
                        try {
                            await fetch('/api/printing/print-ticket/' + encodeURIComponent(result.id) + '?role=Inbound', {
                                method: 'POST'
                            });
                        } catch (printErr) {
                            console.warn('Print request failed:', printErr);
                        }
                    }

                    // Navigate back to the weight sheet delivery loads page.
                    // If the server spilled this load onto a fresh weight sheet
                    // because the target was already at the 25-load cap, use
                    // the NEW sheet's id so the user lands on the sheet their
                    // load actually ended up on.
                    var targetWsId = (result && result.spilledToWeightSheetId)
                        ? result.spilledToWeightSheetId
                        : activeWsId;
                    if (targetWsId) {
                        window.location.href = '/GrowerDelivery/WeightSheetDeliveryLoads?wsId=' + targetWsId;
                        return;
                    }
                } else {
                    const detail = await tryParseError(resp);
                    showAlert('Save failed: ' + detail, 'danger');
                }
            } catch (ex) {
                showAlert('Network error: ' + ex.message, 'danger');
            } finally {
                btn.prop('disabled', false).text(editTxnId ? 'Update Delivery' : 'Save Delivery');
            }
    }

    // ── Edit mode: populate form from existing transaction ─────────────────

    // Formats a numeric transaction id like 604041000013 as "604-041-000013"
    function formatTxnId(id) {
        if (id == null) return '';
        var s = String(id);
        if (s.length < 7) return s;
        return s.slice(0, 3) + '-' + s.slice(3, 6) + '-' + s.slice(6);
    }

    function loadExistingDelivery(txnId) {
        $.ajax({
            url: '/api/GrowerDelivery/' + txnId,
            method: 'GET',
            dataType: 'json',
        })
        .done(function (d) {
            // Show formatted Load ID next to Weight Sheet ID (edit mode only)
            if (d.TransactionId) {
                $('#gdLoadIdValue').text(formatTxnId(d.TransactionId));
                $('#gdLoadIdDisplay').prop('hidden', false);
            }

            // Hidden FK fields
            $(SEL.lotId).val(d.LotId || '');
            $(SEL.locationId).val(d.LocationId || '');

            // Notes
            $('#gdNotes').val(d.Notes || '');

            // Quantities
            if (d.IsTruck) {
                // Scale mode — populate hidden inputs + display elements
                if (d.StartQty != null) {
                    $(SEL.startQty).val(d.StartQty);
                    $(SEL.grossDisplay).text(fmtWeight(d.StartQty));
                    $(SEL.grossRow).addClass('gm-gd-weight-cell--captured');
                    if (d.StartedAt) {
                        $(SEL.startedAt).val(d.StartedAt);
                        $(SEL.grossTime).text(new Date(d.StartedAt).toLocaleTimeString(undefined, { hour: '2-digit', minute: '2-digit', second: '2-digit' }));
                    }
                    if (d.StartQtyLocationQuantityMethodDescription)
                        $(SEL.grossSourceBadge).text(d.StartQtyLocationQuantityMethodDescription).prop('hidden', false);
                    grossCaptured = true;
                    $(SEL.tareRow).prop('hidden', false);
                    $(SEL.captureTare).prop('disabled', false);
                }
                if (d.EndQty != null) {
                    $(SEL.endQty).val(d.EndQty);
                    $(SEL.tareDisplay).text(fmtWeight(d.EndQty));
                    $(SEL.tareRow).addClass('gm-gd-weight-cell--captured');
                    if (d.CompletedAt) {
                        $(SEL.completedAt).val(d.CompletedAt);
                        $(SEL.tareTime).text(new Date(d.CompletedAt).toLocaleTimeString(undefined, { hour: '2-digit', minute: '2-digit', second: '2-digit' }));
                    }
                    if (d.EndQtyLocationQuantityMethodDescription)
                        $(SEL.tareSourceBadge).text(d.EndQtyLocationQuantityMethodDescription).prop('hidden', false);
                }
                // Show net if both weights present
                updateScaleNet();
            } else {
                // Direct mode
                if (d.DirectQty != null) {
                    $(SEL.directQty).val(d.DirectQty);
                    $(SEL.directDisplay).text(fmtWeight(d.DirectQty));
                }
                if (d.StartedAt) $(SEL.directStartedAt).val(d.StartedAt);
                if (d.CompletedAt) $(SEL.directCompletedAt).val(d.CompletedAt);
            }

            // Container — set first container if present
            if (d.ToContainers && d.ToContainers.length > 0) {
                var cid = d.ToContainers[0].ContainerId;
                $(SEL.containerId).val(cid);
                var binInstance = $('#gdContainer').data('dxSelectBox');
                if (binInstance) binInstance.option('value', cid);
            }

            // Grain quality attributes
            var attrs = d.Attributes || {};
            if (attrs.MOISTURE != null) $('#gdMoisture').val(attrs.MOISTURE);
            if (attrs.PROTEIN != null) $('#gdProtein').val(attrs.PROTEIN);

            // Transport attributes
            if (attrs.BOL != null) $('#gdBOL').val(attrs.BOL);
            if (attrs.TRUCK_ID != null) $('#gdTruckId').val(attrs.TRUCK_ID);
            if (attrs.DRIVER != null) $('#gdDriver').val(attrs.DRIVER);

            // Quantity method snapshots
            if (d.StartQtyLocationQuantityMethodId)
                currentQtyMethodId = d.StartQtyLocationQuantityMethodId;
            else if (d.DirectQtyLocationQuantityMethodId)
                currentQtyMethodId = d.DirectQtyLocationQuantityMethodId;

            // Store original weights to detect changes for print logic
            editOriginalWeights = {
                StartQty:  d.StartQty,
                EndQty:    d.EndQty,
                DirectQty: d.DirectQty,
            };

            // Update submit button text
            $(SEL.submit).text('Update Delivery').prop('hidden', false);
        })
        .fail(function (xhr) {
            console.error('[GrowerDelivery] Edit load failed:', xhr.status, xhr.responseText);
            var msg = xhr.responseJSON && xhr.responseJSON.message
                ? xhr.responseJSON.message : 'Failed to load delivery for editing.';
            showAlert(msg, 'danger');
        });
    }

    function buildPayload() {
        function numOrNull(id) {
            const v = parseFloat($('#' + id).val());
            return isNaN(v) ? null : v;
        }
        function strOrNull(id) {
            const v = $('#' + id).val()?.trim();
            return v || null;
        }

        var payload = {
            LotId:          intOrNull(SEL.lotId),
            ProductId:      intOrNull(SEL.productId),
            AccountId:      intOrNull(SEL.accountId),
            LocationId:     intOrNull(SEL.locationId),
            SplitGroupId:   intOrNull(SEL.splitGroupId) || null,
            ToContainers:   (function () {
                var cid = intOrNull(SEL.containerId);
                return cid ? [{ ContainerId: cid, Percent: 100 }] : null;
            })(),
            WeightSheetUid: $(SEL.weightSheetUid).val() || null,
            Notes:         strOrNull('gdNotes')        || null,

            // Grain quality
            Moisture:      numOrNull('gdMoisture') || null,
            Protein:       numOrNull('gdProtein') || null,

            // Transport / load attributes
            BOL:           strOrNull('gdBOL'),
            TruckId:       (strOrNull('gdTruckId') || '').toUpperCase() || null,
            Driver:        strOrNull('gdDriver'),
        };

        var manualSourceTypeId = findSourceTypeIdByCode('MANUAL');
        var scaleSourceTypeId  = findSourceTypeIdByCode('SCALE');

        var methodDesc = currentMethodDescription();

        if (isDirectMode()) {
            // DirectQty mode
            payload.DirectQty                = numOrNull('gdDirectQty') || null;
            payload.StartedAt                = strOrNull('gdDirectStartedAt') || null;
            payload.CompletedAt              = strOrNull('gdDirectCompletedAt') || null;
            payload.DirectQtyMethodId        = currentQtyMethodId;
            payload.DirectQtySourceTypeId    = manualSourceTypeId;
            payload.DirectQtyLocation        = locationName;
            payload.DirectQtySourceDescription = lastPinUserName || 'Manual Entry';
            payload.CreatedByUserId          = lastPinUserId;

            // Quantity method snapshot — direct modes are never scale-based
            payload.DirectQtyLocationQuantityMethodId          = currentQtyMethodId;
            payload.DirectQtyLocationQuantityMethodDescription = methodDesc;
        } else {
            // Scale mode (TRUCK_SCALE)
            payload.StartQty    = numOrNull('gdStartQty') || null;
            payload.EndQty      = numOrNull('gdEndQty')   || null;
            payload.StartedAt   = strOrNull('gdStartedAt') || null;
            payload.CompletedAt = strOrNull('gdCompletedAt') || null;

            var startIsManual = $(SEL.startQtyIsManual).val() === '1';
            var endIsManual   = $(SEL.endQtyIsManual).val() === '1';

            payload.StartQtyMethodId        = currentQtyMethodId;
            payload.StartQtySourceTypeId    = startIsManual ? manualSourceTypeId : scaleSourceTypeId;
            payload.StartQtyLocation        = locationName;
            payload.StartQtySourceDescription = startIsManual ? 'Manual Entry' : 'Scale';

            payload.EndQtyMethodId          = currentQtyMethodId;
            payload.EndQtySourceTypeId      = endIsManual ? manualSourceTypeId : scaleSourceTypeId;
            payload.EndQtyLocation          = locationName;
            payload.EndQtySourceDescription = endIsManual ? (lastPinUserName || 'Manual Entry') : 'Scale';

            // Quantity method snapshot — use scale description if from scale, else method description
            payload.StartQtyLocationQuantityMethodId          = currentQtyMethodId;
            payload.StartQtyLocationQuantityMethodDescription = startIsManual ? methodDesc : (lastStartScaleDesc || methodDesc);

            payload.EndQtyLocationQuantityMethodId            = currentQtyMethodId;
            payload.EndQtyLocationQuantityMethodDescription   = endIsManual ? methodDesc : (lastEndScaleDesc || methodDesc);

            if (startIsManual || endIsManual) {
                payload.CreatedByUserId = lastPinUserId;
            }
        }

        return payload;
    }

    function intOrNull(selector) {
        const v = parseInt($(selector).val(), 10);
        return isNaN(v) ? null : v;
    }

    function validate(p) {
        if (!p.LotId)      return 'Lot is required.';
        if (!p.LocationId) return 'Location is required.';

        if (isDirectMode()) {
            if (!p.DirectQty || p.DirectQty <= 0)
                return 'Weight must be entered (use the Enter Amount button).';
        } else {
            if (!p.StartQty || p.StartQty <= 0)
                return 'Inbound weight must be captured before saving.';
            // EndQty is optional on save — only required for load completion
            if (p.EndQty != null && p.StartQty < p.EndQty)
                return 'Inbound weight must be greater than or equal to outbound weight.';
        }

        // TruckId required for truck loads (not direct mode)
        if (!isDirectMode() && !p.TruckId) return 'Truck ID is required.';

        return null;
    }

    async function tryParseError(resp) {
        try {
            const j = await resp.json();
            return j.message || j.title || resp.statusText;
        } catch {
            return resp.statusText;
        }
    }

    // ── Alert helpers ────────────────────────────────────────────────────────

    function showAlert(msg, type) {
        const el = $(SEL.alert);
        el.removeClass('alert-success alert-danger')
          .addClass('alert alert-' + type)
          .text(msg)
          .prop('hidden', false);
        el[0].scrollIntoView({ behavior: 'smooth', block: 'nearest' });
    }

    function hideAlert() {
        $(SEL.alert).prop('hidden', true).text('');
    }

    // ── Formatters ───────────────────────────────────────────────────────────

    function fmtWeight(w) {
        return Number(w).toLocaleString(undefined, { maximumFractionDigits: 0 }) + ' lbs';
    }

    function fmtTime(date) {
        return date.toLocaleTimeString(undefined, { hour: '2-digit', minute: '2-digit', second: '2-digit' });
    }

    // ── Weight sheet panel ───────────────────────────────────────────────────

    async function refreshWeightSheets(locationId) {
        $(SEL.wsList).html('<span class="text-muted small fst-italic">Loading…</span>');
        selectWeightSheet(null);

        let sheets = [];
        try {
            sheets = await $.getJSON('/api/GrowerDelivery/OpenWeightSheets?locationId=' + locationId);
        } catch (ex) {
            $(SEL.wsList).html('<span class="text-danger small">Failed to load weight sheets.</span>');
            return;
        }

        renderWeightSheets(sheets);
    }

    function renderWeightSheets(sheets) {
        const list = $(SEL.wsList);

        if (!sheets.length) {
            list.html(
                '<div class="gm-gd-ws-empty">' +
                    '<span class="text-muted small">No open weight sheets for this location.</span>' +
                    ' <span class="text-muted small">Create one to associate loads.</span>' +
                '</div>'
            );
            return;
        }

        const rows = sheets.map(function (ws) {
            const lot    = ws.LotDescription ? escapeHtml(ws.LotDescription) : '<span class="text-muted">—</span>';
            const status = ws.Status         ? escapeHtml(ws.Status)         : '';
            const isSelected = currentWeightSheetUid === ws.WeightSheetUid;

            return (
                '<div class="gm-gd-ws-row' + (isSelected ? ' gm-gd-ws-row--selected' : '') + '" ' +
                      'data-uid="' + escapeAttr(ws.WeightSheetUid) + '">' +
                    '<div class="gm-gd-ws-row__id">#' + formatLotId(ws.WeightSheetId) + '</div>' +
                    '<div class="gm-gd-ws-row__date">' + escapeHtml(ws.CreationDate) + '</div>' +
                    '<div class="gm-gd-ws-row__lot">' + lot + '</div>' +
                    '<div class="gm-gd-ws-row__status">' + status + '</div>' +
                    '<button type="button" class="btn btn-sm ' +
                        (isSelected ? 'btn-primary gm-gd-ws-row__deselect' : 'btn-outline-primary gm-gd-ws-row__select') +
                        '">' +
                        (isSelected ? 'Selected ✓' : 'Select') +
                    '</button>' +
                '</div>'
            );
        }).join('');

        list.html(rows);

        // Wire row select/deselect buttons
        list.find('.gm-gd-ws-row__select').on('click', function () {
            const uid = $(this).closest('.gm-gd-ws-row').data('uid');
            selectWeightSheet(uid);
            // Re-render to reflect new selection
            const locationId = $(SEL.locationId).val();
            if (locationId) refreshWeightSheets(locationId);
        });

        list.find('.gm-gd-ws-row__deselect').on('click', function () {
            selectWeightSheet(null);
            const locationId = $(SEL.locationId).val();
            if (locationId) refreshWeightSheets(locationId);
        });
    }

    function selectWeightSheet(uid) {
        currentWeightSheetUid = uid || null;
        $(SEL.weightSheetUid).val(uid || '');
    }

    function wireWeightSheetPanel() {

        // ── Open the New WS panel — show PIN popup first ──────────────────────
        var newWsPinModal = new bootstrap.Modal(document.getElementById('gdNewWsPinModal'));

        $(SEL.newWsBtn).on('click', function () {
            _nwsPin = null;
            $('#gdNewWsPinInput').val('');
            $('#gdNewWsPinError').attr('hidden', true);
            newWsPinModal.show();
            setTimeout(function () { $('#gdNewWsPinInput').focus(); }, 500);
        });

        $('#gdNewWsPinInput').on('keydown', function (e) { if (e.key === 'Enter') { e.preventDefault(); $('#gdNewWsPinConfirmBtn').click(); } });
        $('#gdNewWsPinConfirmBtn').on('click', function () {
            var pin = parseInt($('#gdNewWsPinInput').val(), 10);
            if (!pin || pin <= 0) {
                $('#gdNewWsPinError').text('A valid PIN is required.').removeAttr('hidden');
                return;
            }
            $.ajax({
                url: '/api/GrowerDelivery/ValidatePin?pin=' + pin,
                method: 'GET',
                dataType: 'json',
            })
            .done(function () {
                _nwsPin = pin;
                newWsPinModal.hide();
                // Now show the New WS panel
                $(SEL.wsPanel).prop('hidden', true);
                $(SEL.newWsPanel).prop('hidden', false);
                setNwsLot(null, null);
                resetNwsCreateForm();
                var locationId = $(SEL.locationId).val();
                if (locationId) refreshNwsLots(locationId);
            })
            .fail(function (xhr) {
                var msg = xhr.responseJSON && xhr.responseJSON.message
                    ? xhr.responseJSON.message : 'Invalid PIN.';
                $('#gdNewWsPinError').text(msg).removeAttr('hidden');
            });
        });

        // ── Back to weight sheet list ─────────────────────────────────────────
        $(SEL.cancelNewWsBtn).on('click', function () {
            $(SEL.newWsPanel).prop('hidden', true);
            $(SEL.wsPanel).prop('hidden', false);
        });

        // ── Create weight sheet for selected lot ──────────────────────────────
        $(SEL.createWsBtn).on('click', async function () {
            const locationId = parseInt($(SEL.locationId).val(), 10);

            if (!locationId) {
                $(SEL.newWsError).text('No location selected.').prop('hidden', false);
                return;
            }
            if (!selectedNwsLotId) {
                $(SEL.newWsError).text('Please select a weight sheet lot above.').prop('hidden', false);
                return;
            }

            if (!_nwsPin) {
                $(SEL.newWsError).text('PIN is required. Please try again.').prop('hidden', false);
                return;
            }

            const btn = $(SEL.createWsBtn);
            btn.prop('disabled', true).text('Creating…');
            $(SEL.newWsError).prop('hidden', true);

            try {
                const resp = await fetch('/api/GrowerDelivery/WeightSheets', {
                    method:  'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body:    JSON.stringify({ LocationId: locationId, LotId: selectedNwsLotId, Pin: _nwsPin }),
                });

                if (!resp.ok) {
                    const detail = await tryParseError(resp);
                    $(SEL.newWsError).text('Error: ' + detail).prop('hidden', false);
                    return;
                }

                const created = await resp.json();
                selectWeightSheet(created.WeightSheetUid);

                $(SEL.newWsPanel).prop('hidden', true);
                $(SEL.wsPanel).prop('hidden', false);
                refreshWeightSheets(locationId);

            } catch (ex) {
                $(SEL.newWsError).text('Network error: ' + ex.message).prop('hidden', false);
            } finally {
                btn.prop('disabled', false).text('Create Weight Sheet');
            }
        });

        // ── Create new weight sheet lot ───────────────────────────────────────
        $(SEL.createWsLotBtn).on('click', async function () {
            const locationId  = parseInt($(SEL.locationId).val(), 10);
            const splitGroupId = parseInt(
                $(SEL.nwsSplitGroup).dxSelectBox('option', 'value'), 10) || null;

            if (!locationId) {
                $(SEL.nwsCreateError).text('No location selected.').prop('hidden', false);
                return;
            }
            if (!splitGroupId) {
                $(SEL.nwsCreateError).text('Please select a split group.').prop('hidden', false);
                return;
            }

            const btn = $(SEL.createWsLotBtn);
            btn.prop('disabled', true).text('Creating…');
            $(SEL.nwsCreateError).prop('hidden', true);

            try {
                const resp = await fetch('/api/GrowerDelivery/WeightSheetLots', {
                    method:  'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body:    JSON.stringify({ LocationId: locationId, SplitGroupId: splitGroupId }),
                });

                if (!resp.ok) {
                    const detail = await tryParseError(resp);
                    $(SEL.nwsCreateError).text('Error: ' + detail).prop('hidden', false);
                    return;
                }

                const created = await resp.json();   // { LotId, LotDescription }

                // Auto-select the new lot and refresh the list
                setNwsLot(created.LotId, created.LotDescription);
                resetNwsCreateForm();
                refreshNwsLots(locationId);

            } catch (ex) {
                $(SEL.nwsCreateError).text('Network error: ' + ex.message).prop('hidden', false);
            } finally {
                btn.prop('disabled', false).text('+ Create Lot');
            }
        });
    }

    // ── NWS lot list ─────────────────────────────────────────────────────────

    async function refreshNwsLots(locationId) {
        $(SEL.nwsLotList).html('<span class="text-muted small fst-italic">Loading…</span>');

        let lots = [];
        try {
            lots = await $.getJSON('/api/GrowerDelivery/OpenLots?locationId=' + locationId);
        } catch (ex) {
            $(SEL.nwsLotList).html('<span class="text-danger small">Failed to load lots.</span>');
            return;
        }

        renderNwsLots(lots);
    }

    function renderNwsLots(lots) {
        const list = $(SEL.nwsLotList);

        if (!lots.length) {
            list.html(
                '<div class="gm-gd-ws-empty">' +
                    '<span class="text-muted small">No weight sheet lots found at this location.</span>' +
                    ' <span class="text-muted small">Create one below.</span>' +
                '</div>'
            );
            return;
        }

        const rows = lots.map(function (lot) {
            const desc  = lot.LotDescription        ? escapeHtml(lot.LotDescription)        : '—';
            const sg    = lot.SplitGroupDescription ? escapeHtml(lot.SplitGroupDescription) : '';
            const isSel = selectedNwsLotId === lot.LotId;

            return (
                '<div class="gm-gd-ws-row' + (isSel ? ' gm-gd-ws-row--selected' : '') + '" ' +
                      'data-lot-id="' + lot.LotId + '" data-lot-desc="' + escapeAttr(lot.LotDescription || '') + '">' +
                    '<div class="gm-gd-ws-row__id">#' + formatLotId(lot.LotId) + '</div>' +
                    '<div class="gm-gd-ws-row__lot">' + desc + '</div>' +
                    '<div class="gm-gd-ws-row__status">' + sg + '</div>' +
                    '<button type="button" class="btn btn-sm ' +
                        (isSel ? 'btn-primary gm-gd-nws__deselect' : 'btn-outline-primary gm-gd-nws__select') +
                        '">' +
                        (isSel ? 'Selected ✓' : 'Use This Lot') +
                    '</button>' +
                '</div>'
            );
        }).join('');

        list.html(rows);

        list.find('.gm-gd-nws__select').on('click', function () {
            const row  = $(this).closest('.gm-gd-ws-row');
            setNwsLot(parseInt(row.data('lot-id'), 10), row.data('lot-desc'));
            const locationId = $(SEL.locationId).val();
            if (locationId) refreshNwsLots(locationId);
        });

        list.find('.gm-gd-nws__deselect').on('click', function () {
            setNwsLot(null, null);
            const locationId = $(SEL.locationId).val();
            if (locationId) refreshNwsLots(locationId);
        });
    }

    function setNwsLot(id, desc) {
        selectedNwsLotId   = id   || null;
        selectedNwsLotDesc = desc || null;
        const hasLot = !!selectedNwsLotId;
        $(SEL.createWsBtn).prop('disabled', !hasLot);
        $(SEL.nwsSelectedLotName).text(hasLot ? 'Lot: ' + selectedNwsLotDesc : '');
    }

    // ── NWS create-lot form helpers ───────────────────────────────────────────

    function initNwsAccountPicker() {
        $(SEL.nwsAccount).dxSelectBox({
            dataSource: new DevExpress.data.DataSource({
                store: new DevExpress.data.CustomStore({
                    key:  'AccountId',
                    load: () => $.getJSON('/api/Lookups/Accounts'),
                })
            }),
            valueExpr:    'AccountId',
            displayExpr:  'Name',
            searchEnabled: true,
            placeholder:  'Select account…',
            showClearButton: true,
            onValueChanged: function (e) {
                // Reset split group when account changes
                const sgInst = $(SEL.nwsSplitGroup).dxSelectBox('instance');
                if (sgInst) {
                    sgInst.reset();
                    sgInst.getDataSource().reload();
                }
                $(SEL.nwsLotDesc).val('');
            }
        });
    }

    function initNwsSplitGroupPicker() {
        $(SEL.nwsSplitGroup).dxSelectBox({
            dataSource: new DevExpress.data.DataSource({
                store: new DevExpress.data.CustomStore({
                    key:  'SplitGroupId',
                    load: function () {
                        const accountId = $(SEL.nwsAccount).dxSelectBox('option', 'value');
                        if (!accountId) return Promise.resolve([]);
                        return $.getJSON('/api/GrowerDelivery/SplitGroupsByAccount?accountId=' + accountId);
                    }
                })
            }),
            valueExpr:    'SplitGroupId',
            displayExpr:  function (item) {
                return item ? item.SplitGroupId + ' - ' + item.SplitGroupDescription : '';
            },
            searchEnabled: true,
            placeholder:  'Select split group…',
            showClearButton: true,
            onValueChanged: function (e) {
                // Auto-fill lot description from the selected split group's description
                const desc = e.component.option('text') || '';
                $(SEL.nwsLotDesc).val(desc);
            }
        });
    }

    function resetNwsCreateForm() {
        const acctInst = $(SEL.nwsAccount).dxSelectBox('instance');
        const sgInst   = $(SEL.nwsSplitGroup).dxSelectBox('instance');
        if (acctInst) acctInst.reset();
        if (sgInst)   sgInst.reset();
        $(SEL.nwsLotDesc).val('');
        $(SEL.nwsCreateError).prop('hidden', true).text('');
    }

    // ── Utility ──────────────────────────────────────────────────────────────

    function escapeHtml(str) {
        return String(str)
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;');
    }

    function escapeAttr(str) {
        return String(str).replace(/"/g, '&quot;');
    }

    // ══════════════════════════════════════════════════════════════════════════
    // CHANGE LOT MODAL
    // ══════════════════════════════════════════════════════════════════════════

    var lotChangeModalInstance = null;
    var selectedChangeLotId = null;

    $(document).on('click', '#gdChangeLotBtn', function () {
        if (!lotChangeModalInstance) {
            lotChangeModalInstance = new bootstrap.Modal(document.getElementById('gdLotChangeModal'));
            initLotChangeGrid();
        }
        selectedChangeLotId = null;
        $('#gdLotChangeSaveBtn').prop('disabled', true);
        $('#gdLotChangePin').val('');
        $('#gdLotChangeError').prop('hidden', true);
        loadLotChangeGrid();
        lotChangeModalInstance.show();
    });

    function initLotChangeGrid() {
        $('#gdLotChangeGrid').dxDataGrid({
            dataSource: [],
            keyExpr: 'LotId',
            showBorders: true,
            showRowLines: true,
            rowAlternationEnabled: true,
            columnAutoWidth: true,
            paging: { pageSize: 20 },
            pager: { showPageSizeSelector: true, allowedPageSizes: [10, 20, 50], showInfo: true },
            columns: [
                { dataField: 'LotId', caption: 'Lot ID', width: 140,
                    customizeText: function (e) { return formatLotId(e.value); } },
                { dataField: 'LotDescription', caption: 'Lot Description' },
                { dataField: 'ItemDescription', caption: 'Item', width: 120 },
                { dataField: 'SplitGroupDescription', caption: 'Split Group' },
                { type: 'buttons', width: 80, buttons: [{
                    text: 'Select',
                    cssClass: 'btn btn-sm btn-primary',
                    onClick: function (e) {
                        selectedChangeLotId = e.row.data.LotId;
                        $('#gdLotChangeSaveBtn').prop('disabled', false);
                        // Highlight selected row
                        var grid = $('#gdLotChangeGrid').dxDataGrid('instance');
                        grid.option('selectedRowKeys', [selectedChangeLotId]);
                    }
                }]}
            ],
            selection: { mode: 'single' }
        });

        // Search filter
        $('#gdLotChangeSearch').on('input', function () {
            var term = ($(this).val() || '').trim();
            var grid = $('#gdLotChangeGrid').dxDataGrid('instance');
            if (!grid) return;
            if (!term) { grid.clearFilter(); return; }
            grid.filter([
                ['LotDescription', 'contains', term], 'or',
                ['SplitGroupDescription', 'contains', term], 'or',
                ['ItemDescription', 'contains', term]
            ]);
        });
    }

    function loadLotChangeGrid() {
        var locationId = GM.getLocationId();
        if (!locationId) return;
        $.getJSON('/api/GrowerDelivery/OpenLots?locationId=' + locationId)
            .done(function (data) {
                var grid = $('#gdLotChangeGrid').dxDataGrid('instance');
                if (grid) grid.option('dataSource', data);
            });
    }

    // Save lot change
    $('#gdLotChangeSaveBtn').on('click', function () {
        if (!activeWsId || !selectedChangeLotId) return;

        var pin = parseInt($('#gdLotChangePin').val(), 10);
        if (!pin) {
            $('#gdLotChangeError').text('PIN is required to change the lot.').prop('hidden', false);
            return;
        }

        $.ajax({
            url: '/api/GrowerDelivery/WeightSheet/' + activeWsId,
            method: 'PATCH',
            contentType: 'application/json',
            data: JSON.stringify({ LotId: selectedChangeLotId, Pin: pin })
        })
        .done(function () {
            lotChangeModalInstance.hide();
            loadWsHeader(GM.getLocationId(), activeWsId);
        })
        .fail(function (xhr) {
            var msg = xhr.responseJSON && xhr.responseJSON.message
                ? xhr.responseJSON.message : 'Failed to change lot.';
            $('#gdLotChangeError').text(msg).prop('hidden', false);
        });
    });

    // ══════════════════════════════════════════════════════════════════════════
    // CHANGE BOL TYPE MODAL
    // ══════════════════════════════════════════════════════════════════════════

    function wireBolChangeModal() {
        bolModalInstance = new bootstrap.Modal(document.getElementById('gdBolModal'));

        // BOL Type select
        $('#gdBolTypeSelect').dxSelectBox({
            dataSource: [
                { code: 'N', label: 'None' },
                { code: 'U', label: 'Universal' },
                { code: 'A', label: 'Along Side Field' },
                { code: 'F', label: 'Farm Storage' },
                { code: 'C', label: 'Custom' }
            ],
            valueExpr: 'code',
            displayExpr: 'label',
            placeholder: 'Select BOL type…',
            onValueChanged: function (e) {
                var val = e.value;
                var hints = {
                    N: 'No BOL type — no hauler or rate needed.',
                    U: 'Universal BOL — no hauler or rate needed.',
                    A: 'Along Side Field — hauler and miles required.',
                    F: 'Farm Storage — hauler and miles required.',
                    C: 'Custom — hauler, rate description, and rate required.'
                };
                $('#gdBolTypeHint').text(hints[val] || '').prop('hidden', !val);
                $('#gdBolHaulerMiles').prop('hidden', val !== 'A' && val !== 'F');
                $('#gdBolCustom').prop('hidden', val !== 'C');
                // Reset calc rate on type change
                $('#gdBolCalcRate').val('');
                $('#gdBolCalcRateGroup').prop('hidden', true);
            }
        });

        // Hauler selects for A/F
        $('#gdBolHauler').dxSelectBox({
            dataSource: new DevExpress.data.DataSource({
                store: new DevExpress.data.CustomStore({
                    key: 'Id',
                    load: function () { return $.getJSON('/api/Lookups/Haulers'); }
                })
            }),
            valueExpr: 'Id',
            displayExpr: 'Description',
            searchEnabled: true,
            placeholder: 'Select hauler…'
        });

        // Miles for A/F
        $('#gdBolMiles').dxNumberBox({
            min: 0,
            placeholder: 'Miles\u2026',
            format: '#0',
            inputAttr: { style: 'text-align:right;font-size:15px;' },
            onValueChanged: async function (e) {
                if (e.value === null || e.value === undefined) {
                    $('#gdBolCalcRate').val('');
                    $('#gdBolCalcRateGroup').prop('hidden', true);
                    return;
                }
                var rt = $('#gdBolTypeSelect').dxSelectBox('instance').option('value');
                try {
                    var data = await $.getJSON('/api/Lookups/HaulerRateForMiles?rateType=' + rt + '&miles=' + e.value);
                    $('#gdBolCalcRate').val('$' + data.Rate.toFixed(2) + ' (up to ' + data.MaxDistance + ' mi)');
                    $('#gdBolCalcRateGroup').prop('hidden', false);
                } catch (_) {
                    $('#gdBolCalcRate').val('No rate found for this mileage');
                    $('#gdBolCalcRateGroup').prop('hidden', false);
                }
            }
        });

        // Custom hauler
        $('#gdBolCustomHauler').dxSelectBox({
            dataSource: new DevExpress.data.DataSource({
                store: new DevExpress.data.CustomStore({
                    key: 'Id',
                    load: function () { return $.getJSON('/api/Lookups/Haulers'); }
                })
            }),
            valueExpr: 'Id',
            displayExpr: 'Description',
            searchEnabled: true,
            placeholder: 'Select hauler…'
        });

        // Custom rate
        $('#gdBolCustomRate').dxNumberBox({
            min: 0,
            format: '#,##0.00',
            placeholder: '0.00'
        });

        // Open modal
        $(document).on('click', '#gdChangeBolBtn', function () {
            $('#gdBolError').prop('hidden', true);
            bolModalInstance.show();
        });

        // Save
        $('#gdBolSaveBtn').on('click', function () {
            if (!activeWsId) return;

            var bolType = $('#gdBolTypeSelect').dxSelectBox('instance').option('value');
            if (!bolType) {
                $('#gdBolError').text('Please select a BOL type.').prop('hidden', false);
                return;
            }

            var payload = { RateType: bolType === 'N' ? null : bolType };

            if (bolType === 'N') {
                payload.HaulerId = 0;
                payload.Miles = 0;
                payload.Rate = 0;
                payload.CustomRateDescription = null;
            } else if (bolType === 'U') {
                payload.HaulerId = 0;
                payload.Miles = 0;
                payload.Rate = 0;
                payload.CustomRateDescription = 'Universal';
            } else if (bolType === 'A' || bolType === 'F') {
                var haulerId = $('#gdBolHauler').dxSelectBox('instance').option('value');
                var miles = $('#gdBolMiles').dxNumberBox('instance').option('value');
                if (!haulerId || !miles) {
                    $('#gdBolError').text('Hauler and miles are required.').prop('hidden', false);
                    return;
                }
                payload.HaulerId = haulerId;
                payload.Miles = miles;
                payload.CustomRateDescription = bolType === 'A' ? 'Along Side the Field' : 'Farm Storage';
            } else if (bolType === 'C') {
                var cHaulerId = $('#gdBolCustomHauler').dxSelectBox('instance').option('value');
                var cRateDesc = $('#gdBolCustomRateDesc').val();
                var cRate = $('#gdBolCustomRate').dxNumberBox('instance').option('value');
                if (!cHaulerId || !cRateDesc || !cRate) {
                    $('#gdBolError').text('Hauler, rate description, and rate are required.').prop('hidden', false);
                    return;
                }
                payload.HaulerId = cHaulerId;
                payload.CustomRateDescription = cRateDesc;
                payload.Rate = cRate;
            }

            $.ajax({
                url: '/api/GrowerDelivery/WeightSheet/' + activeWsId,
                method: 'PATCH',
                contentType: 'application/json',
                data: JSON.stringify(payload)
            })
            .done(function () {
                bolModalInstance.hide();
                // Refresh the header to show updated BOL
                loadWsHeader(GM.getLocationId(), activeWsId);
            })
            .fail(function (xhr) {
                var msg = xhr.responseJSON && xhr.responseJSON.message
                    ? xhr.responseJSON.message
                    : 'Failed to update BOL type.';
                $('#gdBolError').text(msg).prop('hidden', false);
            });
        });
    }

})();
