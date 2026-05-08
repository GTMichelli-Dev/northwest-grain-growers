(function () {
    'use strict';

    // Single transfer-load entry form. Layout + scale-capture flow mirrors the
    // warehouse load page (grower.delivery.js). Submits to /api/Transfer.

    var SEL = {
        alert:           '#tlAlert',
        form:            '#tlForm',
        formBody:        '#tlFormBody',
        loadDetails:     '#tlLoadDetails',

        moduleBar:       '#tlModuleBar',
        moduleBarLabel:  '#tlModuleBarLabel',

        // Header
        wsHeader:        '#tlWsHeader',
        wsIdFormatted:   '#tlWsIdFormatted',
        wsVariety:       '#tlWsVariety',
        wsSource:        '#tlWsSource',
        wsDest:          '#tlWsDest',
        wsDate:          '#tlWsDate',
        wsHaulerWrap:    '#tlWsHaulerWrap',
        wsMilesWrap:     '#tlWsMilesWrap',
        wsRateWrap:      '#tlWsRateWrap',
        wsHauler:        '#tlWsHauler',
        wsMiles:         '#tlWsMiles',
        wsRate:          '#tlWsRate',
        wsCustomDescWrap:'#tlWsCustomDescWrap',
        wsCustomDesc:    '#tlWsCustomDesc',

        // Method + bin
        qtyMethod:       '#tlQtyMethod',
        qtyMethodId:     '#tlQtyMethodId',
        qtyMethodCode:   '#tlQtyMethodCode',
        container:       '#tlContainer',
        containerId:     '#tlContainerId',

        // Scale mode
        scaleMode:       '#tlScaleMode',
        captureGross:    '#tlCaptureGross',
        grossDisplay:    '#tlGrossDisplay',
        grossTime:       '#tlGrossTime',
        grossRow:        '#tlGrossRow',
        startQty:        '#tlStartQty',
        startedAt:       '#tlStartedAt',
        startQtyIsManual:'#tlStartQtyIsManual',
        grossSourceBadge:'#tlGrossSourceBadge',

        captureTare:     '#tlCaptureTare',
        tareDisplay:     '#tlTareDisplay',
        tareTime:        '#tlTareTime',
        tareRow:         '#tlTareRow',
        endQty:          '#tlEndQty',
        completedAt:     '#tlCompletedAt',
        endQtyIsManual:  '#tlEndQtyIsManual',
        tareSourceBadge: '#tlTareSourceBadge',

        scaleNetRow:     '#tlScaleNetRow',
        netDisplay:      '#tlNetDisplay',

        // Direct mode
        directMode:      '#tlDirectMode',
        enterAmountBtn:  '#tlEnterAmountBtn',
        directDisplay:   '#tlDirectDisplay',
        directTime:      '#tlDirectTime',
        directQty:       '#tlDirectQty',
        directStartedAt: '#tlDirectStartedAt',
        directCompletedAt:'#tlDirectCompletedAt',

        // Enter Amount modal
        enterAmountModal:    '#tlEnterAmountModal',
        directAmountInput:   '#tlDirectAmountInput',
        directPinInput:      '#tlDirectPinInput',
        directPinWrap:       '#tlDirectPinWrap',
        directAmountError:   '#tlDirectAmountError',
        directAmountConfirm: '#tlDirectAmountConfirm',

        // Capture Weight modal
        captureWeightModal:  '#tlCaptureWeightModal',
        captureScaleList:    '#tlCaptureScaleList',
        captureManualBtn:    '#tlCaptureManualBtn',
        captureManualPanel:  '#tlCaptureManualPanel',
        captureManualInput:  '#tlCaptureManualInput',
        capturePinInput:     '#tlCapturePinInput',
        capturePinWrap:      '#tlCapturePinWrap',
        captureManualConfirm:'#tlCaptureManualConfirm',
        captureWeightError:  '#tlCaptureWeightError',

        // Load details
        protein:  '#tlProtein',
        moisture: '#tlMoisture',
        bol:      '#tlBol',
        truckId:  '#tlTruckId',
        driver:   '#tlDriver',
        notes:    '#tlNotes',

        // Action buttons live in the WS header at the top of the page. The
        // bottom-row duplicates are kept hidden in the markup but no longer
        // wired — all selectors below point at the *Top ids.
        cancelBtn:   '#tlCancelBtnTop',
        saveLoadBtn: '#tlSaveLoadBtnTop',
        moveLoadBtn: '#tlMoveLoadBtnTop',
    };

    // ── State ─────────────────────────────────────────────────────────────
    var _params = new URLSearchParams(window.location.search);
    var _wsId   = parseInt(_params.get('wsId'), 10) || 0;
    var _pin    = parseInt(_params.get('pin'), 10) || 0;
    var _editTxnId = parseInt(_params.get('txnId'), 10) || 0;
    var _moveModalInst = null;
    var _selectedMoveTargetUid = null;
    var _selectedMoveTargetType = null;   // 'Transfer' | 'Intake'

    var _wsRowUid       = null;
    var _wsHasHauler    = false;
    var _currentLocId   = 0;
    var _currentLocName = '';
    // Read-only lockdown: when the active transfer weight sheet is Finished
    // (StatusId 2) or Closed (StatusId 3) the entire load form is read-only
    // — Save / Move / Delete hide, every input is disabled, and the in-form
    // Capture / Enter Amount buttons hide. The save / move / delete handlers
    // also bail on this flag. Name kept as _wsClosed to minimize churn.
    var _wsClosed       = false;
    // 'Received' (this location is the destination, In > Out — same as intake)
    // or 'Shipped' (this location is the source, In < Out — empty truck arrives,
    // gets loaded, leaves heavier).
    var _direction      = 'Received';

    var _quantityMethods    = [];
    var _quantitySourceTypes = [];
    var _currentMethodId    = null;
    var _currentMethodCode  = null;
    var _captureTarget      = null;   // 'start' | 'end'
    var _grossCaptured      = false;
    var _lastStartScaleDesc = null;
    var _lastEndScaleDesc   = null;
    var _cachedScales       = [];
    var _scalePollTimer     = null;
    var _captureModalInst   = null;
    var _enterAmountModalInst = null;
    var _lastPinUserId   = null;

    var DIRECT_CODES = ['MANUAL', 'RAIL', 'BULKLOADER'];
    var PRIVILEGE_MANUAL_ENTRY = 6;

    // ── Boot ──────────────────────────────────────────────────────────────
    $(function () {
        if (!_wsId) { window.location.href = '/WeightSheets'; return; }
        $(SEL.cancelBtn).attr('href', '/GrowerDelivery/WeightSheetTransferLoads?wsId=' + _wsId);
        $(SEL.moduleBar).attr('href', '/GrowerDelivery/WeightSheetTransferLoads?wsId=' + _wsId);

        initLocation()
            .then(loadWeightSheet)
            .then(initPickers)
            .then(wireEvents)
            .then(function () {
                if (_editTxnId > 0) {
                    return loadExistingLoad(_editTxnId);
                }
            });
    });

    async function initLocation() {
        try {
            var current = await $.getJSON('/api/LocationContextApi/current');
            if (current && current.HasLocation && current.LocationId) {
                _currentLocId   = current.LocationId;
                _currentLocName = current.LocationName || current.Name || '';
            }
        } catch (ex) { /* fall through */ }
    }

    async function loadWeightSheet() {
        try {
            var data = await $.getJSON('/api/Transfer/Loads?wsId=' + _wsId);
            if (!data || !data.WeightSheet) {
                showAlert('Weight sheet not found.', 'danger');
                return;
            }
            renderHeader(data.WeightSheet);
            // Pull lot type for color flavoring.
            var ws = await $.getJSON('/api/GrowerDelivery/WeightSheet/' + _wsId).catch(function () { return null; });
            if (ws) applyTransferFlavor(ws);
        } catch (ex) {
            showAlert('Could not load weight sheet.', 'danger');
        }
    }

    function renderHeader(ws) {
        if (!ws) return;
        _wsRowUid    = ws.RowUid;
        _wsHasHauler = ws.HaulerId != null;
        _direction   = ws.Direction || 'Received';

        var wsLabel = ws.As400Id ? String(ws.As400Id) : ('#' + ws.WeightSheetId);
        $('#tlWsIdFormattedTop').text(wsLabel);
        $('#tlWsDirection').text(_direction === 'Received' ? 'Receiving' : 'Shipping');
        $(SEL.wsVariety).text((ws.Item && ws.Item.Description) || '—');
        $(SEL.wsSource).text((ws.SourceLocation && ws.SourceLocation.Name) || '—');
        $(SEL.wsDest).text((ws.DestinationLocation && ws.DestinationLocation.Name) || '—');
        $(SEL.wsDate).text(ws.CreationDate || '—');

        $(SEL.wsHaulerWrap).prop('hidden', !_wsHasHauler);
        $(SEL.wsMilesWrap).prop('hidden',  !_wsHasHauler);
        $(SEL.wsRateWrap).prop('hidden',   !_wsHasHauler);
        $(SEL.wsHauler).text(_wsHasHauler ? (ws.HaulerName || '—') : '—');
        $(SEL.wsMiles).text(ws.Miles != null ? ws.Miles : '—');
        $(SEL.wsRate).text(ws.Rate != null ? '$' + Number(ws.Rate).toFixed(2) : '—');

        $(SEL.wsCustomDescWrap).prop('hidden', !ws.CustomRateDescription);
        $(SEL.wsCustomDesc).text(ws.CustomRateDescription || '');

        $(SEL.wsHeader).prop('hidden', false);
    }

    function applyTransferFlavor(ws) {
        var $bar = $(SEL.moduleBar);
        var $lbl = $(SEL.moduleBarLabel);
        $bar.removeClass('gm-module-bar--transfer gm-module-bar--transfer-seed gm-module-bar--transfer-warehouse');
        if (ws.LotType === 0) {
            $bar.addClass('gm-module-bar--transfer-seed');
            $lbl.text('SEED TRANSFER LOAD');
        } else if (ws.LotType === 1) {
            $bar.addClass('gm-module-bar--transfer-warehouse');
            $lbl.text('WAREHOUSE TRANSFER LOAD');
        } else {
            $bar.addClass('gm-module-bar--transfer');
            $lbl.text('TRANSFER LOAD');
        }
        // Reveal once the correct color/label have been applied so the user
        // never sees a default-color flash on first paint.
        $bar.prop('hidden', false);

        // Finished or Closed transfer WS — clamp to read-only.
        _wsClosed = (ws.StatusId >= 2);
        if (_wsClosed) applyClosedLockdown(ws.StatusId);
    }

    // Apply the read-only lockdown to the transfer load form. Idempotent.
    // Hides the top-bar action buttons (Save / Move / Delete) so only Cancel
    // remains, disables every input/select/textarea inside the form, hides
    // the weight-capture / enter-amount triggers, and surfaces a banner
    // directly under the module bar.
    function applyClosedLockdown(statusId) {
        var stateLabel = (statusId === 2) ? 'finished' : 'closed';
        if (!$('#tlClosedBanner').length) {
            var $banner = $(
                '<div id="tlClosedBanner" class="alert alert-warning mb-0 rounded-0 text-center">' +
                '<strong>This weight sheet is ' + stateLabel + '.</strong> ' +
                'View only — no changes can be saved. Click Cancel to leave.' +
                '</div>'
            );
            var $moduleBar = $(SEL.moduleBar);
            if ($moduleBar.length) $banner.insertAfter($moduleBar);
            else $('#tlPageRoot, body').first().prepend($banner);
        }
        // Hide top-bar edit actions; keep #tlCancelBtnTop visible.
        $(SEL.saveLoadBtn).prop('hidden', true).hide();
        $(SEL.moveLoadBtn).prop('hidden', true).hide();
        $('#tlDeleteLoadBtnTop').prop('hidden', true).hide();
        // Read-only form — disable inputs, selects, textareas inside the form.
        $(SEL.form).find('input, select, textarea').prop('disabled', true);
        // Hide the capture / enter-amount triggers entirely.
        $(SEL.captureGross).prop('hidden', true).hide();
        $(SEL.captureTare).prop('hidden', true).hide();
        $(SEL.enterAmountBtn).prop('hidden', true).hide();
        // Defense: also disable modal-internal confirms in case a modal
        // somehow opens. Their parent modals shouldn't appear since the
        // triggers above are gone.
        $(SEL.captureManualBtn).prop('disabled', true);
        $(SEL.captureManualConfirm).prop('disabled', true);
        $(SEL.directAmountConfirm).prop('disabled', true);
    }

    // ── Pickers (method + bin) ────────────────────────────────────────────
    async function initPickers() {
        if (!_currentLocId) return;

        // Methods + source types
        try { _quantityMethods    = await $.getJSON('/api/Lookups/QuantityMethods?locationId=' + _currentLocId); }
        catch (_) { _quantityMethods = []; }
        try { _quantitySourceTypes = await $.getJSON('/api/Lookups/QuantitySourceTypes'); }
        catch (_) { _quantitySourceTypes = []; }

        $(SEL.qtyMethod).dxSelectBox({
            dataSource:  _quantityMethods,
            valueExpr:   'QuantityMethodId',
            displayExpr: 'Description',
            placeholder: 'Select weight method…',
            value:       findMethodIdByCode('TRUCK_SCALE'),
            onValueChanged: function (e) {
                var m = _quantityMethods.find(function (x) { return x.QuantityMethodId === e.value; });
                _currentMethodId   = e.value || null;
                _currentMethodCode = m ? m.Code : null;
                $(SEL.qtyMethodId).val(e.value || '');
                $(SEL.qtyMethodCode).val(_currentMethodCode || '');
                switchWeightMode(_currentMethodCode);
            },
        });

        var defaultId   = findMethodIdByCode('TRUCK_SCALE');
        var defaultCode = 'TRUCK_SCALE';
        if (!defaultId && _quantityMethods.length > 0) {
            defaultId   = _quantityMethods[0].QuantityMethodId;
            defaultCode = _quantityMethods[0].Code;
        }
        if (defaultId) {
            _currentMethodId   = defaultId;
            _currentMethodCode = defaultCode;
            $(SEL.qtyMethodId).val(defaultId);
            $(SEL.qtyMethodCode).val(defaultCode);
            $(SEL.qtyMethod).dxSelectBox('instance').option('value', defaultId);
        }
        switchWeightMode(_currentMethodCode || 'TRUCK_SCALE');

        // Bin picker — show just the bin description (matches the intake page).
        try {
            var bins = await $.getJSON('/api/Lookups/ContainerBins?locationId=' + _currentLocId) || [];
            var binData = bins.map(function (b) {
                return { Id: b.ContainerId, Name: b.ContainerDescription || b.Description || '' };
            });
            $(SEL.container).dxSelectBox({
                dataSource: binData,
                valueExpr: 'Id',
                displayExpr: 'Name',
                searchEnabled: true,
                showClearButton: true,
                placeholder: 'None',
                onValueChanged: function (e) { $(SEL.containerId).val(e.value ?? ''); },
            });
        } catch (_) { /* bins optional */ }

        $(SEL.formBody).prop('hidden', false);
        $(SEL.loadDetails).prop('hidden', false);
        focusTruckIdIfEmpty();
    }

    // ── Field-progression chain ─────────────────────────────────────────
    // Truck ID → Bin → Protein → (capture buttons as needed) → BOL → Save.
    // Bin is a DX SelectBox; focus targets its inner .dx-texteditor-input.
    // After Protein, the chain skips to whichever capture/weight step is
    // still missing, so a fully-weighed load skips straight to BOL.
    function advanceFocusFrom(currentEl) {
        if (!currentEl) return;
        var $next = null;
        if (currentEl.id === 'tlTruckId') {
            $next = $('#tlContainer').find('.dx-texteditor-input').first();
        } else if ($(currentEl).closest('#tlContainer').length) {
            $next = $('#tlProtein');
        } else if (currentEl.id === 'tlProtein'
                || currentEl.id === 'tlCaptureGross'
                || currentEl.id === 'tlCaptureTare'
                || currentEl.id === 'tlEnterAmountBtn') {
            $next = nextWeightOrBolTarget();
        } else if (currentEl.id === 'tlBol') {
            $next = $(SEL.notes);
        } else if ($(currentEl).is(SEL.notes)) {
            $next = $(SEL.saveLoadBtn);
        }
        focusTarget($next);
    }

    function nextWeightOrBolTarget() {
        if (typeof isDirectMode === 'function' && isDirectMode()) {
            var direct = parseFloat($(SEL.directQty).val()) || 0;
            if (direct <= 0 && $(SEL.enterAmountBtn).is(':visible')) return $(SEL.enterAmountBtn);
        } else {
            var startQty = parseFloat($(SEL.startQty).val()) || 0;
            if (startQty <= 0 && $(SEL.captureGross).is(':visible')) return $(SEL.captureGross);
            var endQty = parseFloat($(SEL.endQty).val()) || 0;
            if (endQty <= 0 && $(SEL.captureTare).is(':visible')) return $(SEL.captureTare);
        }
        // Every weight is captured. Park focus on BOL only if it's still
        // empty — otherwise route to Notes (BOL → Notes → Save chain).
        var $bol = $('#tlBol');
        if (($bol.val() || '').trim() === '') return $bol;
        return $(SEL.notes);
    }

    function focusTarget($target) {
        if (!$target || !$target.length) return;
        if ($target.prop('disabled')) return;
        if (!$target.is(':visible')) return;
        $target.trigger('focus');
        var el = $target[0];
        if (el && el.tagName === 'INPUT' && (el.type === 'text' || el.type === 'number')) {
            try { el.select(); } catch (e) { /* ignore */ }
        }
    }

    function advanceFocusAfterWeight() {
        setTimeout(function () { focusTarget(nextWeightOrBolTarget()); }, 200);
    }

    // Auto-focus the Truck ID when the load form is first ready, but only
    // if it's empty — operator usually starts there.
    function focusTruckIdIfEmpty() {
        setTimeout(function () {
            var $tid = $(SEL.truckId);
            if (!$tid.length) return;
            if ($tid.prop('disabled')) return;
            if (!$tid.is(':visible')) return;
            if (($tid.val() || '').trim() !== '') return;
            $tid.trigger('focus');
        }, 200);
    }

    function findMethodIdByCode(code) {
        var m = _quantityMethods.find(function (x) { return x.Code === code; });
        return m ? m.QuantityMethodId : null;
    }

    function findSourceTypeIdByCode(code) {
        var s = _quantitySourceTypes.find(function (x) { return x.Code === code; });
        return s ? s.QuantitySourceTypeId : null;
    }

    function isDirectMode() { return DIRECT_CODES.indexOf(_currentMethodCode) >= 0; }

    function switchWeightMode(code) {
        $(SEL.saveLoadBtn).prop('hidden', true);
        if (DIRECT_CODES.indexOf(code) >= 0) {
            $(SEL.scaleMode).prop('hidden', true);
            $(SEL.directMode).prop('hidden', false);
            resetScaleFields();
        } else {
            $(SEL.scaleMode).prop('hidden', false);
            $(SEL.directMode).prop('hidden', true);
            resetDirectFields();
        }
    }

    function resetScaleFields() {
        _grossCaptured = false;
        _lastStartScaleDesc = null;
        _lastEndScaleDesc   = null;
        $(SEL.startQty).val('');
        $(SEL.endQty).val('');
        $(SEL.startedAt).val('');
        $(SEL.completedAt).val('');
        $(SEL.startQtyIsManual).val('');
        $(SEL.endQtyIsManual).val('');
        $(SEL.grossDisplay).text('— lbs');
        $(SEL.tareDisplay).text('— lbs');
        $(SEL.netDisplay).text('— lbs');
        $(SEL.grossTime).text('');
        $(SEL.tareTime).text('');
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
        $(SEL.directTime).text('');
    }

    // ── Edit existing load: prefill form from /api/Transfer/{txnId} ───────
    async function loadExistingLoad(txnId) {
        try {
            var d = await $.getJSON('/api/Transfer/' + txnId);
            if (!d) return;

            // Switch to the matching method (truck vs direct).
            if (d.IsTruck) {
                var truckMethodId = findMethodIdByCode('TRUCK_SCALE') || _currentMethodId;
                $(SEL.qtyMethod).dxSelectBox('instance').option('value', truckMethodId);
            } else {
                var directMethodId = findMethodIdByCode('MANUAL') || findMethodIdByCode('RAIL') || findMethodIdByCode('BULKLOADER');
                if (directMethodId) $(SEL.qtyMethod).dxSelectBox('instance').option('value', directMethodId);
            }

            // Source descriptions per quantity field — used to repopulate the
            // "Manual" / scale-name badge on each weight cell.
            var manualSrcId = findSourceTypeIdByCode('MANUAL');
            var sourceByField = {};
            (d.Sources || []).forEach(function (s) { sourceByField[s.QuantityField] = s; });

            function badgeFor(field) {
                var s = sourceByField[field];
                if (!s) return { text: '', isManual: false };
                var isManual = manualSrcId != null && s.SourceTypeId === manualSrcId;
                // SourceDescription is the operator-facing label: scale name on
                // a real capture, "Manual entry" / user name on a manual one.
                var text = isManual ? 'Manual' : (s.SourceDescription || '');
                return { text: text, isManual: isManual };
            }

            // Weights
            if (d.IsTruck) {
                if (d.StartQty != null) {
                    $(SEL.startQty).val(d.StartQty);
                    $(SEL.startedAt).val(d.StartedAt || '');
                    $(SEL.grossDisplay).text(fmtWeight(d.StartQty));
                    $(SEL.grossRow).addClass('gm-gd-weight-cell--captured');
                    $(SEL.tareRow).prop('hidden', false);
                    $(SEL.captureTare).prop('disabled', false);
                    _grossCaptured = true;

                    var startBadge = badgeFor('START');
                    $(SEL.startQtyIsManual).val(startBadge.isManual ? '1' : '');
                    _lastStartScaleDesc = startBadge.isManual ? null : (sourceByField.START && sourceByField.START.SourceDescription) || null;
                    $(SEL.grossSourceBadge).text(startBadge.text).prop('hidden', !startBadge.text);
                }
                if (d.EndQty != null) {
                    $(SEL.endQty).val(d.EndQty);
                    $(SEL.completedAt).val(d.CompletedAt || '');
                    $(SEL.tareDisplay).text(fmtWeight(d.EndQty));
                    $(SEL.tareRow).addClass('gm-gd-weight-cell--captured');
                    updateScaleNet();

                    var endBadge = badgeFor('END');
                    $(SEL.endQtyIsManual).val(endBadge.isManual ? '1' : '');
                    _lastEndScaleDesc = endBadge.isManual ? null : (sourceByField.END && sourceByField.END.SourceDescription) || null;
                    $(SEL.tareSourceBadge).text(endBadge.text).prop('hidden', !endBadge.text);
                }
            } else if (d.DirectQty != null) {
                $(SEL.directQty).val(d.DirectQty);
                $(SEL.directStartedAt).val(d.StartedAt || '');
                $(SEL.directCompletedAt).val(d.CompletedAt || '');
                $(SEL.directDisplay).text(fmtWeight(d.DirectQty));
            }

            // Bin (single-container split for transfers).
            var firstContainer = (d.ToContainers && d.ToContainers.length > 0) ? d.ToContainers[0] : null;
            if (firstContainer) {
                $(SEL.containerId).val(firstContainer.ContainerId);
                try {
                    $(SEL.container).dxSelectBox('instance').option('value', firstContainer.ContainerId);
                } catch (_) { /* picker may still be initializing */ }
            }

            // Attributes (Protein, Moisture, BOL, TruckId, Driver) + Notes
            var a = d.Attributes || {};
            if (a.PROTEIN  != null) $(SEL.protein).val(a.PROTEIN);
            if (a.MOISTURE != null) $(SEL.moisture).val(a.MOISTURE);
            if (a.BOL      != null) $(SEL.bol).val(a.BOL);
            if (a.TRUCK_ID != null) $(SEL.truckId).val(a.TRUCK_ID);
            if (a.DRIVER   != null) $(SEL.driver).val(a.DRIVER);
            if (d.Notes)            $(SEL.notes).val(d.Notes);

            // Show Load ID in the title row + reveal Save + Move (Move requires
            // the load to be saved/existing). Skip these reveals on a closed
            // WS — applyClosedLockdown hid them and we don't want to undo that.
            $('#tlLoadIdValue').text(d.TransactionId);
            $('#tlLoadIdDisplay').prop('hidden', false);
            if (!_wsClosed) {
                $(SEL.saveLoadBtn).prop('hidden', false).text('Update Load');
                $(SEL.moveLoadBtn).prop('hidden', false);

                // Reveal Delete only when this load hasn't been weighed out yet.
                // Same precondition the server enforces — once an EndQty,
                // DirectQty, or CompletedAt is on the row the load is final.
                var weighedOut = (d.EndQty != null) || (d.DirectQty != null) || !!d.CompletedAt;
                $('#tlDeleteLoadBtnTop').prop('hidden', weighedOut);
            }
        } catch (ex) {
            showAlert('Could not load existing load: ' + (ex.statusText || ex.message), 'danger');
        }
    }

    // ── Wire events / capture flow ────────────────────────────────────────
    function wireEvents() {
        $(SEL.captureGross).on('click', function () {
            _captureTarget = 'start';
            openCaptureWeightModal();
        });
        $(SEL.captureTare).on('click', function () {
            _captureTarget = 'end';
            openCaptureWeightModal();
        });

        // Capture modal — manual entry sub-panel. Gate priv 6 (Manual Entry)
        // BEFORE revealing the weight input, then capture only the weight.
        $(SEL.captureManualBtn).on('click', function () {
            GM.requestPin({
                title: 'Enter PIN for Manual Entry',
                prompt: 'Manual weight entry requires the Manual Entry privilege.',
                requiredPrivilegeId: PRIVILEGE_MANUAL_ENTRY
            })
            .then(function (result) {
                _lastPinUserId = result.userId;
                $(SEL.captureManualPanel).prop('hidden', false);
                $(SEL.captureManualInput).val('');
                $(SEL.captureWeightError).prop('hidden', true);
                setTimeout(function () { $(SEL.captureManualInput).trigger('focus'); }, 100);
            })
            .catch(function () { /* cancelled or insufficient privilege */ });
        });
        $(SEL.captureManualConfirm).on('click', confirmManualScaleEntry);

        // Direct (Enter Amount)
        $(SEL.enterAmountBtn).on('click', openEnterAmountModal);
        $(SEL.directAmountConfirm).on('click', confirmDirectAmount);

        // Save
        $(SEL.saveLoadBtn).on('click', saveLoad);

        // Form keyboard handling — see grower.delivery.js for the full
        // rationale. Enter chain: Truck ID → Bin → Protein → (Capture
        // buttons as needed) → BOL → Save Load. ESC = Cancel.
        $(SEL.form).on('keydown', function (e) {
            if (e.key === 'Escape' || e.keyCode === 27) {
                var $cancel = $('#tlCancelBtnTop');
                if ($cancel.length && $cancel.is(':visible') && !$cancel.prop('disabled')) {
                    e.preventDefault();
                    $cancel.trigger('click');
                }
                return;
            }
            if (e.key !== 'Enter' && e.keyCode !== 13) return;
            var t = e.target;
            if (!t) return;

            // Save Load focused → trigger save explicitly via .click() so
            // the click handler ($(SEL.saveLoadBtn).on('click', saveLoad))
            // runs. The form has no submit handler that would otherwise
            // pick this up.
            if (t.id === 'tlSaveLoadBtnTop') {
                e.preventDefault();
                t.click();
                return;
            }

            var tag = (t.tagName || '').toUpperCase();
            // Notes textarea participates in the chain — Enter advances
            // to Save Load instead of inserting a newline. Other textareas
            // would keep newline behavior.
            if (tag === 'TEXTAREA' && !$(t).is(SEL.notes)) return;
            if (t.id === 'tlCaptureGross' || t.id === 'tlCaptureTare' || t.id === 'tlEnterAmountBtn') return;
            if (tag === 'BUTTON' || t.type === 'submit' || t.type === 'button') return;

            e.preventDefault();
            advanceFocusFrom(t);
        });
        // The form has no submit handler today, but block default submit
        // anyway so a stray Enter from a focused field can't trigger a
        // page reload.
        $(SEL.form).on('submit', function (e) { e.preventDefault(); });

        // Move Load — gate priv 2 (Move Loads) BEFORE opening the candidate
        // picker. Selection alone now enables the Confirm button.
        $(SEL.moveLoadBtn).on('click', function () {
            if (_wsClosed) return; // closed-WS defense — button should be hidden anyway
            GM.requestPin({
                title: 'Enter PIN to Move Load',
                prompt: 'Moving a load requires the Move Loads privilege.',
                requiredPrivilegeId: 2
            })
            .then(function (result) {
                _movePinValidated = result.pin;
                openMoveModal();
            })
            .catch(function () { /* cancelled or insufficient privilege */ });
        });
        $('#tlMoveConfirmBtn').on('click', confirmMove);

        // Delete Load — Bootstrap confirm, then PIN gate (priv 14, admin
        // priv 7 bypass), then DELETE. The button itself is only revealed
        // by loadExistingLoad when the load isn't weighed out.
        var deleteLoadModalEl = document.getElementById('tlDeleteLoadModal');
        var deleteLoadModalInst = deleteLoadModalEl ? new bootstrap.Modal(deleteLoadModalEl) : null;
        $('#tlDeleteLoadBtnTop').on('click', function () {
            if (!_editTxnId || !deleteLoadModalInst) return;
            if (_wsClosed) return; // closed-WS defense — button should be hidden anyway
            deleteLoadModalInst.show();
        });
        $('#tlDeleteLoadConfirmBtn').on('click', function () {
            if (!_editTxnId) return;
            if (deleteLoadModalInst) deleteLoadModalInst.hide();
            // Wait for the modal close animation before raising the PIN
            // prompt so backdrop stacking stays clean.
            setTimeout(performDeleteTransferLoad, 250);
        });
    }
    var _movePinValidated = null;

    function performDeleteTransferLoad() {
        var txnId = _editTxnId;
        if (!txnId) return;

        GM.requestPin({
            title: 'Enter PIN to Delete Load',
            prompt: 'Deleting a load requires the Delete Load privilege.',
            requiredPrivilegeId: 14, // PRIV_DELETE_LOAD
            forcePrompt: true
        })
        .then(function (pinResult) {
            return $.ajax({
                url: '/api/GrowerDelivery/' + encodeURIComponent(txnId),
                method: 'DELETE',
                contentType: 'application/json',
                data: JSON.stringify({ Pin: pinResult.pin })
            });
        })
        .then(function () {
            // Land back on the transfer loads grid for this WS, since the
            // deleted load no longer makes sense to keep editing.
            var qp = new URLSearchParams(window.location.search);
            var wsId = qp.get('wsId');
            window.location.href = wsId
                ? '/GrowerDelivery/WeightSheetTransferLoads?wsId=' + encodeURIComponent(wsId)
                : '/WeightSheets';
        })
        .catch(function (err) {
            if (!err) return;
            if (err.message === 'cancelled' || err.message === 'superseded') return;
            var msg = (err.responseJSON && err.responseJSON.message)
                ? err.responseJSON.message
                : (err.status
                    ? 'Delete failed (HTTP ' + err.status + ').'
                    : (err.message || 'Delete failed.'));
            showAlert(msg, 'danger');
        });
    }

    // ── Capture Weight modal ──────────────────────────────────────────────
    async function openCaptureWeightModal() {
        if (!_captureModalInst) {
            _captureModalInst = new bootstrap.Modal(document.querySelector(SEL.captureWeightModal));
            document.querySelector(SEL.captureWeightModal).addEventListener('hidden.bs.modal', stopScalePoll);

            // After the Select Scale popup closes — by capture, by X, by
            // ESC, or by backdrop — drop focus into BOL when it's empty
            // so the operator's next keystroke goes there. If BOL is
            // already filled, fall through to Notes (BOL → Notes → Save).
            document.querySelector(SEL.captureWeightModal).addEventListener('hidden.bs.modal', function () {
                var $bol = $('#tlBol');
                if (($bol.val() || '').trim() === '') focusTarget($bol);
                else                                  focusTarget($(SEL.notes));
            });
        }
        $(SEL.captureManualPanel).prop('hidden', true);
        $(SEL.captureWeightError).prop('hidden', true);
        $(SEL.captureManualInput).val('');

        await loadCachedScales();
        renderScaleList();
        stopScalePoll();
        _scalePollTimer = setInterval(async function () {
            await loadCachedScales();
            renderScaleList();
        }, 1000);

        _captureModalInst.show();
    }

    function stopScalePoll() {
        if (_scalePollTimer) {
            clearInterval(_scalePollTimer);
            _scalePollTimer = null;
        }
    }

    async function loadCachedScales() {
        try { _cachedScales = await $.getJSON('/api/Scale/CachedScales'); }
        catch (_) { _cachedScales = []; }
    }

    var SCALE_STYLES = {
        error:  { bg: '#ffc0cb', border: '#e28aa0', label: 'No Connection',   labelColor: '#c62828' },
        motion: { bg: '#fff9c4', border: '#e6d95e', label: 'Motion',           labelColor: '#e65100' },
        ok:     { bg: '#d4edda', border: '#a3d5b1', label: 'Stable',           labelColor: '#2e7d32' },
    };

    function getScaleStatus(s) {
        if (!s.Ok) return 'error';
        if (s.Motion) return 'motion';
        return 'ok';
    }

    function renderScaleList() {
        var $list = $(SEL.captureScaleList);
        $list.empty();
        if (_cachedScales.length === 0) {
            $list.html('<span class="text-muted small">No scales available at this location.</span>');
            return;
        }
        _cachedScales.forEach(function (s) {
            var status = getScaleStatus(s);
            var style  = SCALE_STYLES[status];
            var weight = Math.round(s.Weight || 0);
            var disabled = status !== 'ok' || weight < 1000;
            var disabledReason = '';
            if (status === 'ok' && weight < 1000) {
                disabledReason = '<span style="color:#c62828;font-size:0.8em;">Below 1,000 lbs minimum</span>';
            }
            if (_captureTarget === 'end' && status === 'ok') {
                var inboundWeight = parseInt($(SEL.startQty).val(), 10);
                if (!isNaN(inboundWeight)) {
                    if (_direction === 'Received' && weight > inboundWeight) {
                        // Receiving: empty truck — Out must be < In.
                        disabled = true;
                        disabledReason = '<span style="color:#c62828;font-size:0.8em;">Exceeds inbound weight (' + fmtWeight(inboundWeight) + ')</span>';
                    } else if (_direction === 'Shipped' && weight < inboundWeight) {
                        // Shipping: loaded truck — Out must be > In.
                        disabled = true;
                        disabledReason = '<span style="color:#c62828;font-size:0.8em;">Below inbound weight (' + fmtWeight(inboundWeight) + ')</span>';
                    }
                }
            }
            var weightSpan = status === 'error'
                ? '<span>—</span>'
                : '<span style="font-size:1.1em;font-weight:600;">' + fmtWeight(weight) + '</span>';
            var statusSpan = '<span style="color:' + style.labelColor + ';font-weight:600;font-size:0.85em;">' + style.label + '</span>';
            var btn = $('<button type="button" class="btn text-start w-100"></button>')
                .css({
                    'background-color': style.bg,
                    'border': '1px solid ' + style.border,
                    'padding': '10px 14px',
                    'display': 'flex',
                    'justify-content': 'space-between',
                    'align-items': 'center',
                    'gap': '12px',
                    'opacity': disabled ? '0.65' : '1',
                })
                .html(
                    '<div><strong>' + escapeHtml(s.Description) + '</strong><br/>' + statusSpan +
                        (disabledReason ? '<br/>' + disabledReason : '') + '</div>' +
                    '<div style="text-align:right;">' + weightSpan + '</div>'
                )
                .prop('disabled', disabled)
                .on('click', function () {
                    var ok = applyScaleWeight(weight, false, s.Description);
                    if (ok) {
                        _captureModalInst.hide();
                        advanceFocusAfterWeight();
                    }
                });
            $list.append(btn);
        });
    }

    function confirmManualScaleEntry() {
        var weight = parseInt($(SEL.captureManualInput).val(), 10);

        if (isNaN(weight) || weight < 0) {
            $(SEL.captureWeightError).text('Weight must be 0 or greater.').prop('hidden', false);
            return;
        }
        if (_captureTarget === 'start' && weight <= 0) {
            $(SEL.captureWeightError).text('Inbound weight must be greater than 0.').prop('hidden', false);
            return;
        }
        if (_captureTarget === 'end') {
            var startQty = parseInt($(SEL.startQty).val(), 10);
            if (!isNaN(startQty)) {
                if (_direction === 'Received' && weight > startQty) {
                    // Receiving: full → empty, Out must be less than In.
                    $(SEL.captureWeightError).text('Outbound weight cannot exceed inbound weight (' + fmtWeight(startQty) + ').').prop('hidden', false);
                    return;
                }
                if (_direction === 'Shipped' && weight < startQty) {
                    // Shipping: empty → full, Out must be greater than In.
                    $(SEL.captureWeightError).text('Outbound weight must exceed inbound weight (' + fmtWeight(startQty) + ').').prop('hidden', false);
                    return;
                }
            }
        }
        // PIN was validated upfront when the user clicked "Manual Entry" —
        // see the captureManualBtn handler. _lastPinUserId already set.
        var ok = applyScaleWeight(weight, true, null);
        if (ok) {
            _captureModalInst.hide();
            advanceFocusAfterWeight();
        }
    }

    function applyScaleWeight(weight, isManual, scaleDesc) {
        var now = new Date();
        if (_captureTarget === 'start') {
            $(SEL.startQty).val(weight);
            $(SEL.startedAt).val(now.toISOString());
            $(SEL.grossDisplay).text(fmtWeight(weight));
            $(SEL.grossTime).text(fmtTime(now));
            $(SEL.grossRow).addClass('gm-gd-weight-cell--captured');
            $(SEL.startQtyIsManual).val(isManual ? '1' : '');
            _lastStartScaleDesc = scaleDesc;
            var grossBadge = isManual ? 'Manual' : (scaleDesc || '');
            $(SEL.grossSourceBadge).text(grossBadge).prop('hidden', !grossBadge);
            $(SEL.tareRow).prop('hidden', false);
            $(SEL.captureTare).prop('disabled', false);
            _grossCaptured = true;
            // First weight in is enough to allow saving (will be completed
            // when out is captured, or saved as in-only and finished later).
            $(SEL.saveLoadBtn).prop('hidden', false);
        } else {
            $(SEL.endQty).val(weight);
            $(SEL.completedAt).val(now.toISOString());
            $(SEL.tareDisplay).text(fmtWeight(weight));
            $(SEL.tareTime).text(fmtTime(now));
            $(SEL.tareRow).addClass('gm-gd-weight-cell--captured');
            $(SEL.endQtyIsManual).val(isManual ? '1' : '');
            _lastEndScaleDesc = scaleDesc;
            var tareBadge = isManual ? 'Manual' : (scaleDesc || '');
            $(SEL.tareSourceBadge).text(tareBadge).prop('hidden', !tareBadge);
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

    // ── Enter Amount modal (Direct mode) ──────────────────────────────────
    // PIN is captured upfront (priv 6 Manual Entry) before the modal opens —
    // the dialog only collects the weight value.
    function openEnterAmountModal() {
        GM.requestPin({
            title: 'Enter PIN for Manual Entry',
            prompt: 'Manual weight entry requires the Manual Entry privilege.',
            requiredPrivilegeId: PRIVILEGE_MANUAL_ENTRY
        })
        .then(function (result) {
            _lastPinUserId = result.userId;
            if (!_enterAmountModalInst) {
                _enterAmountModalInst = new bootstrap.Modal(document.querySelector(SEL.enterAmountModal));
            }
            $(SEL.directAmountInput).val('');
            $(SEL.directAmountError).prop('hidden', true);
            _enterAmountModalInst.show();
        })
        .catch(function () { /* cancelled or insufficient privilege */ });
    }

    function confirmDirectAmount() {
        var amount = parseInt($(SEL.directAmountInput).val(), 10);
        if (isNaN(amount) || amount <= 0) {
            $(SEL.directAmountError).text('Weight must be greater than 0.').prop('hidden', false);
            return;
        }
        var now = new Date();
        $(SEL.directQty).val(amount);
        $(SEL.directStartedAt).val(now.toISOString());
        $(SEL.directCompletedAt).val(now.toISOString());
        $(SEL.directDisplay).text(fmtWeight(amount));
        $(SEL.directTime).text(fmtTime(now));
        $(SEL.saveLoadBtn).prop('hidden', false);
        _enterAmountModalInst.hide();
        advanceFocusAfterWeight();
    }

    // ── Save load → POST /api/Transfer ────────────────────────────────────
    async function saveLoad() {
        if (!_wsRowUid) { showAlert('Weight sheet not loaded.', 'danger'); return; }
        if (_wsClosed)  { showAlert('This weight sheet is closed and cannot be edited.', 'danger'); return; }

        var isTruck = !isDirectMode();
        var startQty = isTruck ? (parseInt($(SEL.startQty).val(), 10) || null) : null;
        var endQty   = isTruck ? (parseInt($(SEL.endQty).val(), 10) || null) : null;
        var direct   = !isTruck ? (parseInt($(SEL.directQty).val(), 10) || null) : null;

        if (isTruck && !startQty)   { showAlert('Capture an in-weight first.', 'danger'); return; }
        if (!isTruck && !direct)    { showAlert('Enter a direct quantity first.', 'danger'); return; }

        var truckIdVal = $.trim($(SEL.truckId).val());
        if (!truckIdVal) { showAlert('Truck ID is required.', 'danger'); return; }
        // Direction rule: Receiving (full → empty) requires In > Out;
        // Shipping (empty → full) requires In < Out.
        if (isTruck && endQty != null) {
            if (_direction === 'Received' && endQty >= startQty) {
                showAlert('Receiving transfer: outbound weight must be less than inbound weight.', 'danger');
                return;
            }
            if (_direction === 'Shipped' && endQty <= startQty) {
                showAlert('Shipping transfer: outbound weight must be greater than inbound weight.', 'danger');
                return;
            }
        }

        var startIsManual = $(SEL.startQtyIsManual).val() === '1';
        var endIsManual   = $(SEL.endQtyIsManual).val()   === '1';

        var manualSrcId    = findSourceTypeIdByCode('MANUAL');
        var scaleSrcId     = findSourceTypeIdByCode('SCALE');
        var manualMethodId = findMethodIdByCode('MANUAL') || _currentMethodId;

        var payload = {
            WeightSheetUid: _wsRowUid,
            LocationId:     _currentLocId,
            StartQty:       startQty,
            EndQty:         endQty,
            DirectQty:      direct,
            StartQtyMethodId:           isTruck ? _currentMethodId : null,
            StartQtySourceTypeId:       isTruck ? (startIsManual ? manualSrcId : scaleSrcId) : null,
            StartQtyLocation:           isTruck ? _currentLocName : null,
            StartQtySourceDescription:  isTruck ? (startIsManual ? 'Manual entry' : (_lastStartScaleDesc || 'Scale')) : null,
            StartQtyLocationQuantityMethodId:          isTruck ? _currentMethodId : null,
            StartQtyLocationQuantityMethodDescription: isTruck ? methodDescription() : null,
            EndQtyMethodId:             (isTruck && endQty) ? _currentMethodId : null,
            EndQtySourceTypeId:         (isTruck && endQty) ? (endIsManual ? manualSrcId : scaleSrcId) : null,
            EndQtyLocation:             (isTruck && endQty) ? _currentLocName : null,
            EndQtySourceDescription:    (isTruck && endQty) ? (endIsManual ? 'Manual entry' : (_lastEndScaleDesc || 'Scale')) : null,
            EndQtyLocationQuantityMethodId:          (isTruck && endQty) ? _currentMethodId : null,
            EndQtyLocationQuantityMethodDescription: (isTruck && endQty) ? methodDescription() : null,
            DirectQtyMethodId:          !isTruck ? (manualMethodId || _currentMethodId) : null,
            DirectQtySourceTypeId:      !isTruck ? manualSrcId : null,
            DirectQtyLocation:          !isTruck ? _currentLocName : null,
            DirectQtySourceDescription: !isTruck ? 'Manual entry' : null,
            DirectQtyLocationQuantityMethodId:          !isTruck ? _currentMethodId : null,
            DirectQtyLocationQuantityMethodDescription: !isTruck ? methodDescription() : null,
            ToContainers: $(SEL.containerId).val()
                ? [{ ContainerId: parseInt($(SEL.containerId).val(), 10), Percent: 100 }]
                : null,
            BOL:        $.trim($(SEL.bol).val()),
            TruckId:    $.trim($(SEL.truckId).val()),
            Driver:     $.trim($(SEL.driver).val()),
            Notes:      $.trim($(SEL.notes).val()),
            Protein:    parseFloat($(SEL.protein).val()) || null,
            Moisture:   parseFloat($(SEL.moisture).val()) || null,
            CreatedByUserId: _lastPinUserId,
            RefType:    'WeightSheet',
            RefId:      _wsRowUid,
            // Use only the captured timestamps. If they're missing (e.g. only
            // an in-weight was captured), send null and let the server default
            // — this avoids mixing a "now" StartedAt with an earlier captured
            // CompletedAt, which trips CK_InventoryTxn_CompletedAfterStarted.
            StartedAt:   isTruck ? ($(SEL.startedAt).val()   || null) : ($(SEL.directStartedAt).val()   || null),
            CompletedAt: isTruck ? ($(SEL.completedAt).val() || null) : ($(SEL.directCompletedAt).val() || null),
        };

        var btn = $(SEL.saveLoadBtn);
        var isEdit = _editTxnId > 0;
        btn.prop('disabled', true).text(isEdit ? 'Updating…' : 'Saving…');
        try {
            const url    = isEdit ? '/api/Transfer/' + _editTxnId : '/api/Transfer';
            const method = isEdit ? 'PUT' : 'POST';
            const resp = await fetch(url, {
                method: method,
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload),
            });
            if (!resp.ok) {
                const err = await tryParseError(resp);
                showAlert((isEdit ? 'Update' : 'Save') + ' failed: ' + err, 'danger');
                return;
            }
            window.location.href = '/GrowerDelivery/WeightSheetTransferLoads?wsId=' + _wsId;
        } catch (ex) {
            showAlert('Network error: ' + ex.message, 'danger');
        } finally {
            btn.prop('disabled', false).text(isEdit ? 'Update Load' : 'Save Load');
        }
    }

    // ── Move Load modal ───────────────────────────────────────────────────
    // PIN is captured upfront via GM.requestPin BEFORE this opens, so the
    // dialog only handles the destination-WS selection.
    async function openMoveModal() {
        if (!_editTxnId) return;
        if (!_moveModalInst) {
            _moveModalInst = new bootstrap.Modal(document.getElementById('tlMoveModal'));
        }
        _selectedMoveTargetUid = null;
        $('#tlMoveError').prop('hidden', true).text('');
        $('#tlMoveCrossWarn').prop('hidden', true);
        $('#tlMoveConfirmBtn').prop('disabled', true);

        try {
            var data = await $.getJSON('/api/Transfer/' + _editTxnId + '/move-candidates');
            renderMoveCandidates(data.Source || {}, data.Candidates || []);
        } catch (ex) {
            $('#tlMoveError').text('Could not load candidates: ' + (ex.statusText || ex.message)).prop('hidden', false);
        }
        _moveModalInst.show();
    }

    function renderMoveCandidates(source, candidates) {
        var rows = candidates.map(function (c) {
            // For transfers we show the direction (Receiving / Shipping); for
            // delivery WSs we show the lot number so the operator can tell
            // them apart at a glance.
            var isTransfer = (c.WeightSheetType || '').toLowerCase() === 'transfer';
            return {
                RowUid:          c.RowUid,
                WeightSheetId:   c.As400Id || c.WeightSheetId,
                WsType:          isTransfer ? 'Transfer' : 'Intake',
                Detail:          isTransfer
                                    ? (c.Direction || '—')
                                    : ('Lot ' + (c.LotDescription || c.LotId || '—')),
                Variety:         c.Variety || '—',
                Source:          c.SourceName || (isTransfer ? '—' : ''),
                Destination:     c.DestinationName || (isTransfer ? '—' : ''),
                LoadCount:       c.LoadCount,
                CreationDate:    c.CreationDate,
                EffectiveItemId: c.EffectiveItemId,
                _SourceItemId:   source.EffectiveItemId,
            };
        });

        var $grid = $('#tlMoveCandidatesGrid');
        var existing;
        try { existing = $grid.dxDataGrid('instance'); } catch (e) { existing = null; }

        var options = {
            dataSource: rows,
            keyExpr:    'RowUid',
            columns: [
                { dataField: 'WeightSheetId', caption: 'WS #' },
                { dataField: 'WsType',        caption: 'WS Type', width: 90 },
                { dataField: 'Detail',        caption: 'Detail' },
                { dataField: 'Variety',       caption: 'Variety' },
                { dataField: 'Source',        caption: 'Source' },
                { dataField: 'Destination',   caption: 'Destination' },
                { dataField: 'LoadCount',     caption: 'Loads', width: 80, alignment: 'right' },
                { dataField: 'CreationDate',  caption: 'Created', dataType: 'date', width: 110,
                  customizeText: window.gmDxServerTime('date') },
            ],
            selection:       { mode: 'single' },
            showBorders:     true,
            columnAutoWidth: true,
            paging:          { enabled: false },
            onSelectionChanged: function (e) {
                var row = (e.selectedRowsData || [])[0];
                _selectedMoveTargetUid  = row ? row.RowUid : null;
                _selectedMoveTargetType = row ? row.WsType : null;

                // Cross-variety warning when destination's item differs from
                // source's. Compares EffectiveItemId so it works whether the
                // target is a Transfer (item on WS) or Delivery (item on lot).
                if (row && row.EffectiveItemId && row._SourceItemId && row.EffectiveItemId !== row._SourceItemId) {
                    $('#tlMoveCrossWarnText').text(
                        'Destination weight sheet has a different variety (' + (row.Variety || '—') +
                        '). The move will still be allowed but is recorded in the audit trail.');
                    $('#tlMoveCrossWarn').prop('hidden', false);
                } else {
                    $('#tlMoveCrossWarn').prop('hidden', true);
                }

                $('#tlMoveConfirmBtn').prop('disabled', !_selectedMoveTargetUid);
            },
        };
        if (existing) existing.option(options);
        else          $grid.dxDataGrid(options);
    }

    async function confirmMove() {
        if (!_selectedMoveTargetUid) {
            $('#tlMoveError').text('Select a destination weight sheet.').prop('hidden', false);
            return;
        }
        var pin = _movePinValidated;
        if (!pin) {
            // Should never happen — modal can't open without an upfront gate.
            $('#tlMoveError').text('PIN validation lost. Please re-open the Move dialog.').prop('hidden', false);
            return;
        }
        var btn = $('#tlMoveConfirmBtn');
        btn.prop('disabled', true).text('Moving…');
        try {
            const resp = await fetch('/api/Transfer/' + _editTxnId + '/move', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    TargetWeightSheetUid: _selectedMoveTargetUid,
                    Pin: pin,
                }),
            });
            if (!resp.ok) {
                const err = await tryParseError(resp);
                $('#tlMoveError').text('Move failed: ' + err).prop('hidden', false);
                return;
            }
            const result = await resp.json();
            if (_moveModalInst) _moveModalInst.hide();
            // Land on the destination WS's loads page — pick the right list
            // page based on the target's WS type.
            var listPage = (_selectedMoveTargetType || '').toLowerCase() === 'intake'
                ? '/GrowerDelivery/WeightSheetDeliveryLoads'
                : '/GrowerDelivery/WeightSheetTransferLoads';
            window.location.href = listPage + '?wsId=' + result.toWeightSheetId;
        } catch (ex) {
            $('#tlMoveError').text('Network error: ' + ex.message).prop('hidden', false);
        } finally {
            btn.prop('disabled', false).text('Move Load');
        }
    }

    function methodDescription() {
        var m = _quantityMethods.find(function (x) { return x.QuantityMethodId === _currentMethodId; });
        return m ? m.Description : null;
    }

    // ── PIN validation ────────────────────────────────────────────────────
    async function validatePin(pin) {
        try {
            var resp = await fetch('/api/GrowerDelivery/ValidatePin?pin=' + encodeURIComponent(pin));
            if (resp.ok) {
                var data = await resp.json();
                return { valid: true, userId: data.UserId, userName: data.UserName, privileges: data.Privileges || [] };
            }
            var err = await tryParseError(resp);
            return { valid: false, message: err };
        } catch (ex) {
            return { valid: false, message: 'Network error: ' + ex.message };
        }
    }

    // ── Tiny helpers ──────────────────────────────────────────────────────
    function fmtWeight(n) {
        if (n == null || isNaN(n)) return '— lbs';
        return Number(n).toLocaleString() + ' lbs';
    }
    function fmtTime(d) {
        // Render in the configured server timezone so capture-time displays
        // are consistent across operators / browsers.
        return window.gmFormatServerTime(d, 'timeShort');
    }
    function escapeHtml(s) {
        return String(s == null ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;');
    }
    function showAlert(msg, level) {
        var $el = $(SEL.alert);
        $el.removeClass('alert-success alert-danger alert-warning alert-info')
           .addClass('alert-' + (level || 'info'))
           .text(msg)
           .prop('hidden', false);
        if (level === 'success') {
            setTimeout(function () { $el.prop('hidden', true); }, 3000);
        }
    }
    async function tryParseError(resp) {
        try {
            var body = await resp.json();
            if (body && body.message) return body.message;
            return resp.status + ' ' + resp.statusText;
        } catch (_) {
            return resp.status + ' ' + resp.statusText;
        }
    }

})();
