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
        cancelDelivery: '#gdCancelDeliveryBtn',

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
        // gdDirectPinInput removed — PIN captured upfront via GM.requestPin.
        directAmountError:  '#gdDirectAmountError',
        directAmountConfirm:'#gdDirectAmountConfirm',

        // Capture Weight Modal
        captureWeightModal: '#gdCaptureWeightModal',
        captureScaleList:   '#gdCaptureScaleList',
        captureManualBtn:   '#gdCaptureManualBtn',
        captureManualPanel: '#gdCaptureManualPanel',
        captureManualInput: '#gdCaptureManualInput',
        // gdCapturePinInput removed — PIN captured upfront via GM.requestPin.
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
    // True for the FIRST load on a WS the user just created in this session.
    // While true, the manual-capture / direct-amount modals skip the PIN re-prompt
    // and reuse _nwsPin. Cleared after the first load saves successfully.
    let _firstLoadOnNewWsPin  = null;
    let editTxnId             = null;   // set when editing an existing delivery
    let editOriginalWeights   = null;  // { StartQty, EndQty, DirectQty } at load time
    let bolModalInstance      = null;

    // ── End Dump (REQUIRE_DUMP_TYPE LocationAttribute) ─────────────────
    // Set on init from /api/locations/{id}/RequireDumpType. When true:
    //  - Edit mode shows an inline checkbox prefilled from the existing
    //    IS_END_DUMP transaction attribute.
    //  - Create mode keeps the checkbox visible too, plus pops a
    //    Bootstrap modal on submit asking the operator to confirm.
    let _requireDumpType      = false;
    let _endDumpModalInstance = null;
    // Pending submit closure used to resume the save after the operator
    // answers the End Dump prompt. Cleared as soon as it fires.
    let _endDumpResolver      = null;

    // Closed-WS lockdown: when the active weight sheet is Finished
    // (StatusId 2) or Closed (StatusId 3) the entire load form is read-only
    // — Save / Move / Delete buttons hide, every input/select/textarea is
    // disabled, and the in-form Capture / Enter Amount buttons can't open
    // their modals. The submit handler also bails on this flag as a defense
    // if a button somehow gets re-enabled. Name kept as _wsClosed to
    // minimize churn; semantically it's "WS is no longer open for edits".
    let _wsClosed             = false;

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

        // Check for active weight sheet: URL param wins, falling back to
        // the cookie. The previous order (cookie first) caused a stale
        // wsId cookie from an earlier session to override the explicit
        // wsId in the URL — which is exactly what happens when an operator
        // clicks a load row in WeightSheetDeliveryLoads and the editor
        // ended up showing some other WS's header.
        var urlParamsForInit = new URLSearchParams(window.location.search);
        var urlWsId = parseInt(urlParamsForInit.get('wsId'), 10) || 0;
        var wsId = urlWsId > 0 ? urlWsId : (getWsIdCookie() || 0);

        // New-WS hand-off: NewWeightSheet adds ?newWs=1&pin=<pin> when it
        // navigates here after creating a fresh WS. Seed the in-page first-
        // load PIN so the operator doesn't have to re-key it for the very
        // first load on this WS. Always strip both params from the URL
        // afterwards so a refresh / share can't reuse a stale pin.
        if (urlParamsForInit.get('newWs') === '1') {
            var carriedNewWsPin = parseInt(urlParamsForInit.get('pin'), 10) || 0;
            if (carriedNewWsPin > 0) _firstLoadOnNewWsPin = carriedNewWsPin;
            urlParamsForInit.delete('newWs');
            urlParamsForInit.delete('pin');
            var cleanQs = urlParamsForInit.toString();
            var cleanUrl = window.location.pathname + (cleanQs ? '?' + cleanQs : '');
            try { window.history.replaceState({}, '', cleanUrl); } catch (e) { /* old browser — ignore */ }
        }

        if (wsId > 0) {
            // Weight sheet is known — hide the WS list, show form directly.
            // The module bar stays hidden until loadWsHeader recolors it by
            // LotType (avoids a default-brown flash before the seed/warehouse
            // override paints).
            activeWsId = wsId;
            $(SEL.wsPanel).prop('hidden', true);
            $('#gdNwsPanel').prop('hidden', true);
            $('#gdFormBody').prop('hidden', false);
            loadWsHeader(locationId, wsId);
        } else {
            // No active WS — show the WS list as before, and reveal the
            // module bar with its default rendering (no LotType available yet).
            $('#gdModuleBar').prop('hidden', false);
            $(SEL.wsPanel).prop('hidden', false);
            $('#gdFormBody').prop('hidden', false);
            refreshWeightSheets(locationId);
        }

        // Auto-focus Truck ID if it's empty — operator usually starts here.
        // Wrapped in a timeout inside the helper to give DX widgets time
        // to render so the focus call doesn't race with their init.
        focusTruckIdIfEmpty();

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
        wireBolScanButton(locationId);
        wireSubmit();
        wireEndDumpUI(locationId);

        // ── Edit mode: load existing transaction if txnId is in URL ─────────
        // Defer until async inits (qty method picker, selectboxes) have resolved
        if (editTxnId) {
            setTimeout(function () { loadExistingDelivery(editTxnId); }, 500);
        }
    });

    // ── End Dump UI (REQUIRE_DUMP_TYPE → IS_END_DUMP) ──────────────────
    function wireEndDumpUI(locationId) {
        if (!locationId) return;
        $.getJSON('/api/locations/' + encodeURIComponent(locationId) + '/RequireDumpType')
            .done(function (resp) {
                _requireDumpType = !!(resp && resp.RequireDumpType);
                if (!_requireDumpType) return;

                // Inject the inline checkbox just above the Save button so
                // the operator sees a single visible toggle. Edit mode
                // prefills from the load's existing IS_END_DUMP after the
                // form is populated; create mode starts unchecked.
                var $row = $(
                    '<div id="gdEndDumpRow" class="form-check form-switch mb-2 mt-2"' +
                    ' style="margin-left:.5rem;">' +
                    '<input class="form-check-input" type="checkbox" id="gdIsEndDump">' +
                    '<label class="form-check-label" for="gdIsEndDump">' +
                    '<strong>End Dump</strong></label>' +
                    '</div>'
                );
                var $btn = $(SEL.submit);
                if ($btn.length) $btn.before($row);

                // Build the create-mode confirmation modal. Reused across
                // every Save Load click while the page is loaded.
                var modalHtml =
                    '<div class="modal fade" id="gmGdEndDumpModal" tabindex="-1" aria-hidden="true" data-bs-backdrop="static">' +
                      '<div class="modal-dialog modal-dialog-centered">' +
                        '<div class="modal-content">' +
                          '<div class="modal-header bg-info text-white">' +
                            '<h5 class="modal-title">End Dump?</h5>' +
                          '</div>' +
                          '<div class="modal-body">' +
                            '<p class="mb-0">Is this load an <strong>end dump</strong>?</p>' +
                          '</div>' +
                          '<div class="modal-footer">' +
                            '<button type="button" class="btn btn-outline-secondary" id="gmGdEndDumpNo">No</button>' +
                            '<button type="button" class="btn btn-info text-white" id="gmGdEndDumpYes">Yes</button>' +
                          '</div>' +
                        '</div>' +
                      '</div>' +
                    '</div>';
                $(document.body).append(modalHtml);
                _endDumpModalInstance = new bootstrap.Modal(document.getElementById('gmGdEndDumpModal'));
                $('#gmGdEndDumpYes').on('click', function () { resolveEndDump(true); });
                $('#gmGdEndDumpNo' ).on('click', function () { resolveEndDump(false); });
            });
    }

    function resolveEndDump(answer) {
        $('#gdIsEndDump').prop('checked', !!answer);
        if (_endDumpModalInstance) _endDumpModalInstance.hide();
        var resolver = _endDumpResolver;
        _endDumpResolver = null;
        if (typeof resolver === 'function') resolver(answer);
    }

    /// Fetches the existing IS_END_DUMP value for a load and prefills the
    /// inline checkbox. Safe to call when the End Dump UI hasn't been
    /// injected yet (no-op).
    function prefillEndDumpForEdit(txnId) {
        if (!_requireDumpType || !txnId) return;
        $.getJSON('/api/GrowerDelivery/' + encodeURIComponent(txnId) + '/IsEndDump')
            .done(function (resp) {
                $('#gdIsEndDump').prop('checked', !!(resp && resp.IsEndDump === true));
            });
    }

    // ── Weight sheet header (shown when wsId is in URL) ─────────────────────

    function formatWsId(id) {
        var s = String(id);
        if (s.length < 7) return s;
        return s.substring(0, 3) + '-' + s.substring(3, 6) + '-' + s.substring(6);
    }

    var BOL_LABELS = { N: 'None', U: 'Universal', A: 'Along Side Field', F: 'Farm Storage', C: 'Custom' };

    // ── Field-progression chain ─────────────────────────────────────────
    // Truck ID → Bin → Protein → (capture buttons as needed) → BOL → Save.
    // Bin is a DX SelectBox; focus targets its inner .dx-texteditor-input.
    // After Protein, the chain skips to whichever capture/weight step is
    // still missing, so a fully-weighed load skips straight to BOL.
    function advanceFocusFrom(currentEl) {
        if (!currentEl) return;
        var $next = null;
        if (currentEl.id === 'gdTruckId') {
            $next = $('#gdContainer').find('.dx-texteditor-input').first();
        } else if ($(currentEl).closest('#gdContainer').length) {
            $next = $('#gdProtein');
        } else if (currentEl.id === 'gdProtein'
                || currentEl.id === 'gdCaptureGross'
                || currentEl.id === 'gdCaptureTare'
                || currentEl.id === 'gdEnterAmountBtn') {
            $next = nextWeightOrBolTarget();
        } else if (currentEl.id === 'gdBOL') {
            $next = $('#gdNotes');
        } else if (currentEl.id === 'gdNotes') {
            $next = $('#gmGdSubmitTop');
        }
        focusTarget($next);
    }

    // Returns the next "weight stage" element to focus based on what's
    // already captured. Order:
    //   Truck mode  : In Capture (no StartQty) → Out Capture (no EndQty)
    //                  → BOL (if empty) → Save Load.
    //   Direct mode : Enter Amount (no DirectQty)
    //                  → BOL (if empty) → Save Load.
    // Once every weight is captured we only park focus on BOL when it's
    // still blank — otherwise the operator goes straight to Save Load.
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
        var $bol = $('#gdBOL');
        if (($bol.val() || '').trim() === '') return $bol;
        // BOL filled — slot Notes between BOL and Save so the operator
        // can type a comment by pressing Enter once more.
        return $('#gdNotes');
    }

    // Focus a jQuery target if present, visible, and enabled. Selects the
    // input contents on text/number inputs so the operator can immediately
    // overtype.
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

    // Called by capture / direct-amount handlers after a successful
    // weight commit so focus naturally advances without an extra Tab.
    function advanceFocusAfterWeight() {
        // setTimeout lets the modal finish hiding before we re-focus,
        // otherwise the modal-trapping logic can yank focus back.
        setTimeout(function () { focusTarget(nextWeightOrBolTarget()); }, 200);
    }

    // Auto-focus the Truck ID when the load form is first ready, but only
    // if it's empty — the operator typically arrives ready to scan/key a
    // truck id, so saving a click is real ergonomics. Skipped on closed/
    // finished WSs (input is disabled) and when the field already has a
    // value (e.g. editing an existing load).
    function focusTruckIdIfEmpty() {
        setTimeout(function () {
            var $tid = $('#gdTruckId');
            if (!$tid.length) return;
            if ($tid.prop('disabled')) return;
            if (!$tid.is(':visible')) return;
            if (($tid.val() || '').trim() !== '') return;
            $tid.trigger('focus');
        }, 200);
    }

    // Apply the closed-WS lockdown to the load form. Idempotent. Hides the
    // top-bar action buttons (Save / Move / Delete) so only Cancel remains,
    // disables every input/select/textarea inside the form, hides the
    // weight-capture / enter-amount triggers (so the operator can't even
    // attempt to capture), and surfaces a banner.
    function applyClosedLockdown(statusId) {
        // Status 2 = Finished, 3 = Closed. The lockdown is identical; only
        // the banner wording differs so the operator knows which state they
        // landed in (and that re-open is the only path back from Finished).
        var stateLabel = (statusId === 2) ? 'finished' : 'closed';
        if (!$('#gdClosedBanner').length) {
            // Place directly under the module bar (e.g. "WAREHOUSE GROWER
            // DELIVERY") so the closed-WS notice sits on top of the page
            // body but below the section banner. Falls back to prepending
            // the gm-gd wrapper if the module bar isn't present.
            var $banner = $(
                '<div id="gdClosedBanner" class="alert alert-warning mb-0 rounded-0 text-center">' +
                '<strong>This weight sheet is ' + stateLabel + '.</strong> ' +
                'View only — no changes can be saved. Click Cancel to leave.' +
                '</div>'
            );
            var $moduleBar = $('#gdModuleBar');
            if ($moduleBar.length) $banner.insertAfter($moduleBar);
            else $('.gm-gd').prepend($banner);
        }
        // Hide top-bar edit actions; keep #gdCancelDeliveryBtn visible.
        $('#gmGdSubmitTop, #gdMoveLoadBtn, #gdDeleteLoadBtn').prop('hidden', true).hide();
        // Read-only form: disable inputs, selects, textareas inside the form.
        $('#gmGdForm').find('input, select, textarea').prop('disabled', true);
        // Hide the capture / enter-amount triggers entirely — there's no
        // useful "disabled" state for these, the operator just wants them
        // gone. The captured-weight values remain visible (read-only) so
        // the load history is still reviewable.
        $('#gdCaptureGross, #gdCaptureTare, #gdEnterAmountBtn').prop('hidden', true).hide();
        // Defense: also disable the modal-internal confirm buttons in case
        // a modal somehow opens. Their parent modals shouldn't ever appear
        // since the triggers above are gone.
        $('#gdCaptureManualBtn, #gdDirectAmountConfirm, #gdCaptureManualConfirm,' +
          '#gdBolScanBtn').prop('disabled', true);
    }

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
            $('#gdModuleBar').attr('href', backUrl);

            // Recolor the module bar by LotType (0=Seed, 1=Warehouse) so
            // seed/warehouse weight sheets are visually distinct, then unhide.
            // Strip prior color/icon variants in case the user navigates between
            // sheets without a full reload.
            var $bar  = $('#gdModuleBar');
            var $icon = $('#gdModuleBarIcon');
            var $lbl  = $('#gdModuleBarLabel');
            $bar.removeClass('gm-module-bar--seed gm-module-bar--warehouse gm-module-bar--intake gm-module-bar--transfer');
            $icon.removeClass('gm-icon--seed gm-icon--warehouse gm-icon--delivery gm-icon--transfer');
            if (ws.LotType === 0) {
                $bar.addClass('gm-module-bar--seed');
                $icon.addClass('gm-icon--seed');
                $lbl.text('SEED GROWER DELIVERY');
            } else if (ws.LotType === 1) {
                $bar.addClass('gm-module-bar--warehouse');
                $icon.addClass('gm-icon--warehouse');
                $lbl.text('WAREHOUSE GROWER DELIVERY');
            } else {
                // Unknown lot type — fall back to the original intake style.
                $bar.addClass('gm-module-bar--intake');
                $icon.addClass('gm-icon--delivery');
                $lbl.text('GROWER DELIVERY');
            }
            $bar.prop('hidden', false);

            $(SEL.wsHeader).removeAttr('hidden');

            // Pre-fill hidden form fields from the weight sheet
            if (ws.LotId) $(SEL.lotId).val(ws.LotId);
            if (ws.LocationId) $(SEL.locationId).val(ws.LocationId);

            // Finished or Closed WS — clamp to read-only. Sets _wsClosed and
            // disables the form. Idempotent so re-calling loadWsHeader (e.g.
            // after a SignalR refresh) keeps the lockdown applied.
            _wsClosed = (ws.StatusId >= 2);
            if (_wsClosed) applyClosedLockdown(ws.StatusId);

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
        // Gate priv 6 (Manual Entry) BEFORE opening the dialog. If the user
        // already validated when creating this brand-new WS, _firstLoadOnNewWsPin
        // carries that pin forward — skip the prompt.
        $(SEL.enterAmountBtn).on('click', function () {
            var openDialog = function (pin, userId, userName) {
                if (!enterAmountModalInst) {
                    enterAmountModalInst = new bootstrap.Modal(document.querySelector(SEL.enterAmountModal));
                }
                $(SEL.directAmountInput).val('');
                $(SEL.directAmountError).prop('hidden', true);
                lastPinUserId   = userId   || lastPinUserId;
                lastPinUserName = userName || lastPinUserName;
                _directAmountPinValidated = pin;
                enterAmountModalInst.show();
            };

            if (_firstLoadOnNewWsPin) {
                openDialog(_firstLoadOnNewWsPin, lastPinUserId, lastPinUserName);
                return;
            }

            // Always force a fresh PIN entry — manual weight entry is a
            // legal-record-affecting action (the "M" flag prints on the
            // weight sheet), so each occurrence must be attributable to
            // a person typing their PIN at the keypad. The cached PIN
            // (10-min) is bypassed here on purpose.
            GM.requestPin({
                title: 'Enter PIN for Manual Entry',
                prompt: 'Manual weight entry requires the Manual Entry privilege.',
                requiredPrivilegeId: PRIVILEGE_MANUAL_ENTRY,
                forcePrompt: true
            })
            .then(function (result) { openDialog(result.pin, result.userId, result.userName); })
            .catch(function () { /* cancelled or insufficient privilege */ });
        });

        $(SEL.directAmountConfirm).on('click', function () {
            var amount = parseInt($(SEL.directAmountInput).val(), 10);
            if (isNaN(amount) || amount <= 0) {
                $(SEL.directAmountError).text('Weight must be greater than 0.').prop('hidden', false);
                return;
            }
            // PIN was validated upfront — see the Enter Amount click handler.
            var now = new Date();
            $(SEL.directQty).val(amount);
            $(SEL.directStartedAt).val(now.toISOString());
            $(SEL.directCompletedAt).val(now.toISOString());
            $(SEL.directDisplay).text(fmtWeight(amount));
            $(SEL.directTime).text(fmtTime(now));
            $(SEL.submit).prop('hidden', false);

            enterAmountModalInst.hide();
            advanceFocusAfterWeight();
        });
    }
    var _directAmountPinValidated = null;

    // ══════════════════════════════════════════════════════════════════════════
    // CAPTURE WEIGHT MODAL (Scale mode — lists scales + manual entry)
    // ══════════════════════════════════════════════════════════════════════════

    function wireCaptureWeightModal(locationId) {
        // Load available scales at this location
        loadCachedScales();

        // Gate priv 6 (Manual Entry) BEFORE revealing the manual-entry panel.
        // The new-WS short-circuit reuses the upfront-validated PIN.
        $(SEL.captureManualBtn).on('click', function () {
            var revealPanel = function () {
                $(SEL.captureManualPanel).prop('hidden', false);
                $(SEL.captureManualInput).val('');
                $(SEL.captureWeightError).prop('hidden', true);
                setTimeout(function () { $(SEL.captureManualInput).trigger('focus'); }, 100);
            };

            if (_firstLoadOnNewWsPin) { revealPanel(); return; }

            // Force a fresh PIN every time the manual-entry panel is
            // opened — see the matching note on the Enter Amount handler
            // above. Skipping the cache here so the keypad prompt is the
            // gate, not a 10-minute-old session.
            GM.requestPin({
                title: 'Enter PIN for Manual Entry',
                prompt: 'Manual weight entry requires the Manual Entry privilege.',
                requiredPrivilegeId: PRIVILEGE_MANUAL_ENTRY,
                forcePrompt: true
            })
            .then(function (result) {
                lastPinUserId   = result.userId;
                lastPinUserName = result.userName;
                revealPanel();
            })
            .catch(function () { /* cancelled or insufficient privilege */ });
        });

        $(SEL.captureManualConfirm).on('click', function () {
            var weight = parseInt($(SEL.captureManualInput).val(), 10);

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
            // PIN validated upfront — see the captureManualBtn click handler.
            var ok = applyScaleWeight(weight, true, null);
            if (ok) {
                captureWeightModalInst.hide();
                advanceFocusAfterWeight();
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
                    if (ok) {
                        captureWeightModalInst.hide();
                        advanceFocusAfterWeight();
                    }
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

            // After the Select Scale popup closes — by capture, by X, by
            // ESC, or by backdrop — drop focus into BOL when it's empty so
            // the operator's next keystroke goes there. If BOL is already
            // filled, fall through to Notes (chain: BOL → Notes → Save).
            document.querySelector(SEL.captureWeightModal).addEventListener('hidden.bs.modal', function () {
                var $bol = $('#gdBOL');
                if (($bol.val() || '').trim() === '') focusTarget($bol);
                else                                  focusTarget($('#gdNotes'));
            });
        }
        $(SEL.captureManualPanel).prop('hidden', true);
        $(SEL.captureWeightError).prop('hidden', true);
        $(SEL.captureManualInput).val('');

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
                return {
                    valid:      true,
                    userId:     data.UserId,
                    userName:   data.UserName,
                    privileges: data.Privileges || [],
                };
            }
            var err = await tryParseError(resp);
            return { valid: false, message: err };
        } catch (ex) {
            return { valid: false, message: 'Network error: ' + ex.message };
        }
    }

    // PrivilegeId 6 = "Manual Entry" — required for users who manually type in
    // a weight value (rather than capturing from a scale).
    var PRIVILEGE_MANUAL_ENTRY = 6;

    // ── Form submission ──────────────────────────────────────────────────────

    var _weightEditPinModal = null;
    var _pendingSubmitPayload = null;

    function wireSubmit() {
        _weightEditPinModal = new bootstrap.Modal(document.getElementById('gdWeightEditPinModal'));

        // Cancel button — bail out of the delivery form back to the weight sheet list.
        $(SEL.cancelDelivery).on('click', function () {
            window.location.href = '/WeightSheets';
        });

        wireMoveLoad();
        wireDeleteLoad();

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

        // Form keyboard handling.
        //   • Enter advances along Truck ID → Bin → Protein → (capture
        //     buttons as needed) → BOL → Save Load. The form never submits
        //     on Enter from a field — it submits only when Save Load is
        //     focused and Enter is pressed (or when the operator clicks).
        //   • ESC triggers the Cancel button (cancel the in-progress load).
        //   • Capture buttons (gdCaptureGross/Tare, gdEnterAmountBtn) keep
        //     their browser-default Enter activation — pressing Enter on
        //     a focused capture button opens the capture modal.
        //   • Textareas keep Enter for line breaks.
        $(SEL.form).on('keydown', function (e) {
            // ESC = Cancel the in-progress load.
            if (e.key === 'Escape' || e.keyCode === 27) {
                var $cancel = $('#gdCancelDeliveryBtn');
                if ($cancel.length && $cancel.is(':visible') && !$cancel.prop('disabled')) {
                    e.preventDefault();
                    $cancel.trigger('click');
                }
                return;
            }
            if (e.key !== 'Enter' && e.keyCode !== 13) return;
            var t = e.target;
            if (!t) return;

            // Save Load focused → trigger save explicitly. Calling .click()
            // fires the click event and also triggers form submit, but our
            // submit handler runs preventDefault and then calls the save
            // flow once.
            if (t.id === 'gmGdSubmitTop') {
                e.preventDefault();
                t.click();
                return;
            }

            var tag = (t.tagName || '').toUpperCase();
            // Notes is a single-row textarea used as a comment field —
            // Enter advances the chain (→ Save Load) instead of inserting
            // a newline. Other textareas would keep newline behavior.
            if (tag === 'TEXTAREA' && t.id !== 'gdNotes') return;
            // Capture/Enter Amount buttons: let the browser activate them
            // on Enter so the operator can keyboard-drive the capture.
            if (t.id === 'gdCaptureGross' || t.id === 'gdCaptureTare' || t.id === 'gdEnterAmountBtn') return;
            // Other plain buttons: also let browser default fire.
            if (tag === 'BUTTON' || t.type === 'submit' || t.type === 'button') return;

            e.preventDefault();
            advanceFocusFrom(t);
        });

        $(SEL.form).on('submit', async function (e) {
            e.preventDefault();
            hideAlert();

            // Closed-WS defense — applyClosedLockdown already hid the submit
            // button, but guard here too in case anything re-enables it.
            if (_wsClosed) {
                showAlert('This weight sheet is closed and cannot be edited.', 'danger');
                return;
            }

            const payload = buildPayload();
            const err = validate(payload);
            if (err) {
                showAlert(err, 'danger');
                return;
            }

            // End Dump prompt (create-mode only). Edit mode reads the
            // inline checkbox directly in buildPayload — no modal.
            if (_requireDumpType && !editTxnId && _endDumpModalInstance) {
                e.preventDefault?.();
                _endDumpResolver = function (answer) {
                    payload.IsEndDump = !!answer;
                    proceedAfterEndDump(payload);
                };
                _endDumpModalInstance.show();
                return;
            }
            if (_requireDumpType && editTxnId) {
                payload.IsEndDump = $('#gdIsEndDump').is(':checked');
            }

            proceedAfterEndDump(payload);
        });

        function proceedAfterEndDump(payload) {
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
        }
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

                    // First-load-on-new-WS PIN reuse only applies to that one
                    // load. Clear it so any subsequent load (on this same page
                    // load, however unlikely) re-prompts for a PIN.
                    _firstLoadOnNewWsPin = null;

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
                btn.prop('disabled', false).text(editTxnId ? 'Update Load' : 'Save Load');
            }
    }

    // ── Move Load (between weight sheets, audited) ─────────────────────────

    var _moveLoadModalInst = null;
    var _movePinPromptInst = null;
    var _moveLoadSelectedUid = null;
    var _moveLoadSourceLotId = null;
    var _moveLoadSourceVariety = '';
    // PIN captured by the upfront prompt and reused for the /move POST so the
    // operator only enters their PIN once.
    var _movePinValidated = null;

    // PrivilegeId 2 = Move Loads. Mirrors PrivilegeIdMoveLoads in the controller.
    var PRIVILEGE_MOVE_LOADS = 2;

    // ── Delete unweighed-out load ──────────────────────────────────────────
    // Shown only when editing an existing load that hasn't been weighed
    // out (loadExistingDelivery flips the button visible based on EndQty /
    // DirectQty / CompletedAt). The Bootstrap confirm modal gates the
    // PIN prompt; the server enforces priv 14 (DeleteLoad, with admin
    // priv 7 bypass) and the not-weighed-out precondition again.
    var _deleteLoadModalInst = null;
    function wireDeleteLoad() {
        var modalEl = document.getElementById('gdDeleteLoadModal');
        if (!modalEl) return;
        _deleteLoadModalInst = new bootstrap.Modal(modalEl);

        $('#gdDeleteLoadBtn').on('click', function () {
            if (!editTxnId) return;
            if (_wsClosed) return; // closed-WS defense — button should be hidden anyway
            _deleteLoadModalInst.show();
        });

        $('#gdDeleteLoadConfirmBtn').on('click', function () {
            if (!editTxnId) return;
            _deleteLoadModalInst.hide();
            // Wait for the modal close animation before raising the PIN
            // prompt so backdrop stacking stays clean.
            setTimeout(performDeleteLoad, 250);
        });
    }

    function performDeleteLoad() {
        var txnId = editTxnId;
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
            // Land back on the loads grid that owns this WS, since the
            // deleted load no longer makes sense to keep editing.
            var qp = new URLSearchParams(window.location.search);
            var wsId = qp.get('wsId');
            window.location.href = wsId
                ? '/GrowerDelivery/WeightSheetDeliveryLoads?wsId=' + encodeURIComponent(wsId)
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

    function wireMoveLoad() {
        var modalEl = document.getElementById('gdMoveLoadModal');
        if (!modalEl) return;
        _moveLoadModalInst = new bootstrap.Modal(modalEl);

        $('#gdMoveLoadBtn').on('click', openMovePinPrompt);
        $('#gdMoveCreateWsBtn').on('click', moveLoadCreateNewWs);
        $('#gdMoveConfirmBtn').on('click', moveLoadConfirm);

        // Reset selection when the move modal closes so a re-open starts fresh
        $(modalEl).on('hidden.bs.modal', function () {
            _moveLoadSelectedUid = null;
            _moveLoadSourceLotId = null;
            _moveLoadSourceVariety = '';
            _movePinValidated = null;
            $('#gdMoveConfirmBtn').prop('disabled', true);
            $('#gdMoveError').prop('hidden', true).text('');
            $('#gdMoveLotWarn').prop('hidden', true);
        });
    }

    // Move-Load gate — runs the shared GM.requestPin against priv 2 (Move
    // Loads). The cached PIN is reused on the actual move PATCH so the
    // operator only enters it once per move.
    function openMovePinPrompt() {
        if (!editTxnId) return;
        if (_wsClosed) return; // closed-WS defense — button should be hidden anyway
        _movePinValidated = null;
        GM.requestPin({
            title: 'Enter PIN to Move Load',
            prompt: 'Moving a load requires the Move Loads privilege.',
            requiredPrivilegeId: PRIVILEGE_MOVE_LOADS
        })
        .then(function (result) {
            _movePinValidated = result.pin;
            openMoveLoadModal();
        })
        .catch(function () { /* cancelled or insufficient privilege */ });
    }

    async function openMoveLoadModal() {
        if (!editTxnId) return;
        $('#gdMoveError').prop('hidden', true).text('');
        $('#gdMoveLotWarn').prop('hidden', true);
        _moveLoadSelectedUid = null;
        _moveLoadSourceLotId = null;
        _moveLoadSourceVariety = '';
        $('#gdMoveConfirmBtn').prop('disabled', true);

        // Dispose any existing grid instance before re-initializing — DevExtreme
        // doesn't reset cleanly otherwise and a second open can leave the grid
        // empty or duplicated.
        try {
            var prev = $('#gdMoveCandidatesGrid').dxDataGrid('instance');
            if (prev) prev.dispose();
        } catch (e) { /* nothing to dispose */ }
        $('#gdMoveCandidatesGrid').empty();

        // Show the modal FIRST so the grid initializes inside a visible
        // container — Bootstrap modal hidden state gives DevExtreme a 0×0 box
        // and the grid silently renders 0 rows.
        _moveLoadModalInst.show();

        // Defer the grid build + data fetch until the modal's "shown" event
        // fires. If the event has already fired (cached modal), the listener
        // never re-runs, so add a fallback timeout.
        var modalEl = document.getElementById('gdMoveLoadModal');
        var built = false;
        function buildGridAndFetch() {
            if (built) return;
            built = true;

            $('#gdMoveCandidatesGrid').dxDataGrid({
                dataSource:    [],
                keyExpr:       'RowUid',
                showBorders:   true,
                columnAutoWidth:true,
                paging:        { enabled: true, pageSize: 10 },
                sorting:       { mode: 'single' },
                selection:     { mode: 'single' },
                noDataText:    'No open weight sheets are available.',
                columns: [
                    { dataField:'WeightSheetId', caption:'WS #', sortOrder:'desc',
                      customizeText: function (c) { return formatWsId(c.value); } },
                    { dataField:'As400Id',       caption:'Agvantage #' },
                    { dataField:'CreationDate',  caption:'Created', dataType:'date',
                      customizeText: window.gmDxServerTime('date') },
                    { dataField:'StatusId',      caption:'Status', width:120,
                      customizeText: function (c) { return c.value === 1 ? 'Finished' : 'Open'; } },
                    { dataField:'LotId',         caption:'Lot #', width:120 },
                    { dataField:'Variety',       caption:'Variety' },
                    { dataField:'LoadCount',     caption:'Loads',  width:90, alignment:'right' },
                ],
                onSelectionChanged: function (e) {
                    var key = (e.selectedRowKeys && e.selectedRowKeys[0]) || null;
                    var row = (e.selectedRowsData && e.selectedRowsData[0]) || null;
                    _moveLoadSelectedUid = key;
                    $('#gdMoveConfirmBtn').prop('disabled', !key);
                    refreshCrossLotWarning(row);
                },
            });

            $.getJSON('/api/GrowerDelivery/' + editTxnId + '/move-candidates')
                .done(function (resp) {
                    var rows = Array.isArray(resp) ? resp : (resp.Candidates || []);
                    _moveLoadSourceLotId   = (resp && resp.SourceLotId)   || null;
                    _moveLoadSourceVariety = (resp && resp.SourceVariety) || '';
                    var inst = $('#gdMoveCandidatesGrid').dxDataGrid('instance');
                    if (inst) {
                        inst.option('dataSource', rows);
                        // Force a recalc — guards against DevExtreme rendering
                        // with stale 0-height container measurements.
                        inst.updateDimensions();
                    }
                })
                .fail(function (xhr) {
                    var msg = (xhr && xhr.responseJSON && xhr.responseJSON.message)
                        || (xhr && xhr.statusText)
                        || 'Failed to load candidates.';
                    $('#gdMoveError').text(msg).prop('hidden', false);
                });
        }

        $(modalEl).one('shown.bs.modal', buildGridAndFetch);
        // Fallback if the modal is already visible (re-open case)
        setTimeout(buildGridAndFetch, 250);
    }

    // Show or hide the cross-lot/variety danger banner based on the selected
    // candidate. Doesn't disable the move — the operator can proceed; the
    // MOVE_LOAD audit row records both source and destination IDs.
    function refreshCrossLotWarning(candidate) {
        var $warn = $('#gdMoveLotWarn');
        var $text = $('#gdMoveLotWarnText');
        if (!candidate || !_moveLoadSourceLotId) {
            $warn.prop('hidden', true);
            return;
        }
        if (candidate.LotId === _moveLoadSourceLotId) {
            $warn.prop('hidden', true);
            return;
        }
        var srcVar = _moveLoadSourceVariety || 'unknown';
        var dstVar = candidate.Variety || 'unknown';
        var msg = 'You are moving this load to a different lot — variety changes from "'
                + srcVar + '" to "' + dstVar + '".';
        $text.text(msg);
        $warn.prop('hidden', false);
    }

    // Auto-perform the move when returning from the New WS round-trip. The
    // operator picked the upstream lot/WS type; we already have a validated
    // PIN and the new WS id. Just resolve its RowUid and POST /move.
    async function autoPerformMove(targetWeightSheetId) {
        if (!editTxnId || !targetWeightSheetId) return;
        if (!_movePinValidated) {
            // Defensive: lost PIN somehow (shouldn't happen). Fall back to
            // pre-selecting in the picker so the operator re-enters.
            openMoveLoadModalForWs(targetWeightSheetId);
            return;
        }
        try {
            // Resolve the new WS's RowUid via the candidates list (cheapest
            // available endpoint that returns RowUid for an in-scope sheet).
            var resp = await $.getJSON('/api/GrowerDelivery/' + editTxnId + '/move-candidates');
            var rows = Array.isArray(resp) ? resp : (resp.Candidates || []);
            var match = rows.find(function (r) { return r.WeightSheetId === targetWeightSheetId; });
            if (!match) {
                // Couldn't find the new WS in candidates (different location?
                // closed already?). Fall back to the picker so the operator
                // sees an explanation in the grid.
                openMoveLoadModalForWs(targetWeightSheetId);
                return;
            }

            var moveResp = await fetch('/api/GrowerDelivery/' + editTxnId + '/move', {
                method:  'POST',
                headers: { 'Content-Type': 'application/json' },
                body:    JSON.stringify({ TargetWeightSheetUid: match.RowUid, Pin: _movePinValidated }),
            });
            if (!moveResp.ok) {
                var detail = await tryParseError(moveResp);
                showAlert('Move failed: ' + detail, 'danger');
                return;
            }
            var result = await moveResp.json();
            if (result && result.toWeightSheetId) {
                window.location.href = '/GrowerDelivery/WeightSheetDeliveryLoads?wsId=' + result.toWeightSheetId;
            } else {
                window.location.href = '/WeightSheets';
            }
        } catch (ex) {
            showAlert('Network error during move: ' + ex.message, 'danger');
        }
    }

    // Open the move modal and pre-select the candidate row matching the given
    // WeightSheetId. Used as a fallback when autoPerformMove can't proceed.
    function openMoveLoadModalForWs(targetWeightSheetId) {
        if (!_moveLoadModalInst) return;
        openMoveLoadModal();
        // Poll briefly for the grid to fill (the candidates fetch is async).
        var attempts = 0;
        var iv = setInterval(function () {
            attempts++;
            var inst;
            try { inst = $('#gdMoveCandidatesGrid').dxDataGrid('instance'); } catch (e) { inst = null; }
            var rows = inst ? (inst.option('dataSource') || []) : [];
            if (rows && rows.length) {
                var match = rows.find(function (r) { return r.WeightSheetId === targetWeightSheetId; });
                if (match) {
                    _moveLoadSelectedUid = match.RowUid;
                    inst.selectRows([match.RowUid], false);
                    $('#gdMoveConfirmBtn').prop('disabled', false);
                    refreshCrossLotWarning(match);
                }
                clearInterval(iv);
            } else if (attempts > 30) {
                // 30 × 100ms = 3s — give up; the modal stays open with no preselect.
                clearInterval(iv);
            }
        }, 100);
    }

    function moveLoadCreateNewWs() {
        // Navigate through /WeightSheets/LoadType → /GrowerDelivery/NewWeightSheet
        // (or TransferIn) so the operator can pick lot type, WS type, and an
        // existing or new lot. The PIN already validated upstream rides along
        // in the URL so the operator doesn't have to re-enter it. The flow
        // returns to this page with ?moveTo=<newWsId>&pin=<pin> and the
        // round-trip handler auto-performs the move.
        if (!editTxnId) return;
        var fromWsId = activeWsId || 0;
        var params = new URLSearchParams();
        params.set('returnTo', 'move');
        params.set('txnId',    String(editTxnId));
        if (fromWsId)            params.set('fromWsId', String(fromWsId));
        if (_movePinValidated)   params.set('pin',      String(_movePinValidated));
        window.location.href = '/WeightSheets/LoadType?' + params.toString();
    }

    async function moveLoadConfirm() {
        $('#gdMoveError').prop('hidden', true).text('');
        if (!editTxnId) return;
        if (!_moveLoadSelectedUid) {
            $('#gdMoveError').text('Select a destination weight sheet.').prop('hidden', false);
            return;
        }
        // PIN was captured + privilege-checked by the upfront prompt before
        // this modal opened. If we somehow lost it, kick the user back to
        // the prompt rather than silently failing.
        var pin = _movePinValidated;
        if (!pin || pin <= 0) {
            _moveLoadModalInst.hide();
            openMovePinPrompt();
            return;
        }

        var btn = $('#gdMoveConfirmBtn');
        btn.prop('disabled', true).text('Moving\u2026');
        try {
            var resp = await fetch('/api/GrowerDelivery/' + editTxnId + '/move', {
                method:  'POST',
                headers: { 'Content-Type': 'application/json' },
                body:    JSON.stringify({ TargetWeightSheetUid: _moveLoadSelectedUid, Pin: pin }),
            });
            if (!resp.ok) {
                var detail = await tryParseError(resp);
                $('#gdMoveError').text(detail).prop('hidden', false);
                return;
            }
            var result = await resp.json();
            _moveLoadModalInst.hide();
            // Bounce the user to the destination weight sheet's loads page so
            // they see the move took effect.
            if (result && result.toWeightSheetId) {
                window.location.href = '/GrowerDelivery/WeightSheetDeliveryLoads?wsId=' + result.toWeightSheetId;
            } else {
                window.location.href = '/WeightSheets';
            }
        } catch (ex) {
            $('#gdMoveError').text('Network error: ' + ex.message).prop('hidden', false);
        } finally {
            btn.prop('disabled', false).text('Move Load');
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
        // Populate In/Out/BOL thumbnails for this load — fires immediately and
        // re-fires on the SignalR ImageCaptured event so a fresh capture from
        // the field appears here without a page refresh.
        if (window.gmLoadImages) {
            window.gmLoadImages.attach({ prefix: 'gd', loadNumber: txnId });
        }

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
                        $(SEL.grossTime).text(window.gmFormatServerTime(d.StartedAt, 'time'));
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
                        $(SEL.tareTime).text(window.gmFormatServerTime(d.CompletedAt, 'time'));
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

            // End Dump checkbox prefill (only matters when the location
            // has REQUIRE_DUMP_TYPE turned on — wireEndDumpUI keeps that
            // flag in _requireDumpType).
            prefillEndDumpForEdit(txnId);

            // Update submit button text + reveal the Move Load button (edit mode only).
            // Skip the reveals entirely on a closed WS — applyClosedLockdown
            // hid them and we don't want to undo that.
            if (!_wsClosed) {
                $(SEL.submit).text('Update Load').prop('hidden', false);
                $('#gdMoveLoadBtn').prop('hidden', false);

                // Reveal Delete only for loads that haven't been weighed out yet
                // — same precondition the server enforces. Once an EndQty,
                // DirectQty, or CompletedAt exists the load is final, so we
                // hide the destructive button entirely.
                var weighedOut = (d.EndQty != null) || (d.DirectQty != null) || !!d.CompletedAt;
                $('#gdDeleteLoadBtn').prop('hidden', weighedOut);
            }

            // Move-load round-trip: handle return from the New WS / LoadType
            // flow that was launched from the Move Load modal.
            //   ?moveTo=<id>&pin=<pin>   → operator finished creating a new
            //       WS upstream; auto-perform the move directly. No picker,
            //       no second PIN entry. The carried PIN was already validated
            //       (PIN+Privilege 2) at the start of the move flow.
            //   ?resumeMove=1&pin=<pin>  → operator cancelled out of the new
            //       WS flow; reopen the Move Load picker with the cached PIN
            //       already validated, so they don't have to re-enter it.
            var qp        = new URLSearchParams(window.location.search);
            var moveToId  = parseInt(qp.get('moveTo'),  10) || 0;
            var resumeMove= qp.get('resumeMove') === '1';
            var carriedPin= parseInt(qp.get('pin'),     10) || 0;

            if (moveToId > 0 || resumeMove) {
                // Strip the round-trip params from the URL so a refresh
                // doesn't re-fire the auto-move or auto-reopen.
                var clean = new URLSearchParams(window.location.search);
                clean.delete('moveTo');
                clean.delete('resumeMove');
                clean.delete('pin');
                var qs = clean.toString();
                history.replaceState(null, '', window.location.pathname + (qs ? '?' + qs : ''));

                if (carriedPin) _movePinValidated = carriedPin;

                if (moveToId > 0) {
                    autoPerformMove(moveToId);
                } else {
                    // resumeMove=1 — reopen the picker; skip the PIN prompt
                    // since we already have a validated PIN cached.
                    if (_movePinValidated) {
                        openMoveLoadModal();
                    } else {
                        openMovePinPrompt();
                    }
                }
            }
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

            // End Dump answer — null when the location doesn't require
            // it; the server skips the IS_END_DUMP attribute write in
            // that case. The submit handler overwrites this from a
            // confirmation modal in create mode, or from the inline
            // checkbox in edit mode, before doSubmit fires.
            IsEndDump:     _requireDumpType ? $('#gdIsEndDump').is(':checked') : null,
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
        // Render in the server timezone so all weight-cell capture-time
        // displays read consistently regardless of the operator's browser.
        return window.gmFormatServerTime(date, 'time');
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

        // ── Open the New WS panel — gate on shared GM.requestPin ─────────────
        $(SEL.newWsBtn).on('click', function () {
            _nwsPin = null;
            GM.requestPin({ prompt: 'Enter your PIN to create a new weight sheet.' })
                .then(function (result) {
                    _nwsPin = result.pin;
                    // Now show the New WS panel
                    $(SEL.wsPanel).prop('hidden', true);
                    $(SEL.newWsPanel).prop('hidden', false);
                    setNwsLot(null, null);
                    resetNwsCreateForm();
                    var locationId = $(SEL.locationId).val();
                    if (locationId) refreshNwsLots(locationId);
                })
                .catch(function () { /* cancelled */ });
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

                // The user just validated their PIN to create this WS. Carry it
                // forward to the FIRST load they save on this WS so they don't
                // have to re-enter it for the manual-capture / direct-amount
                // modals. Cleared after that first load saves.
                _firstLoadOnNewWsPin = _nwsPin;

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

    // PIN validated upfront via the priv-10 gate (Modify Lots) before the
    // lot picker opens. saveLotChange() reuses the cached pin.
    var _lotChangePinValidated = null;

    $(document).on('click', '#gdChangeLotBtn', function () {
        GM.requestPin({
            prompt: 'Enter your PIN to change the lot on this weight sheet.',
            requiredPrivilegeId: 10
        })
        .then(function (result) {
            _lotChangePinValidated = result.pin;
            if (!lotChangeModalInstance) {
                lotChangeModalInstance = new bootstrap.Modal(document.getElementById('gdLotChangeModal'));
                initLotChangeGrid();
            }
            selectedChangeLotId = null;
            $('#gdLotChangeSaveBtn').prop('disabled', true);
            $('#gdLotChangeError').prop('hidden', true);
            loadLotChangeGrid();
            lotChangeModalInstance.show();
        })
        .catch(function () { /* cancelled or insufficient privilege */ });
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

    // Save lot change — PIN was validated upfront, reuse the cached value.
    $('#gdLotChangeSaveBtn').on('click', function () {
        if (!activeWsId || !selectedChangeLotId) return;
        if (!_lotChangePinValidated) {
            $('#gdLotChangeError').text('PIN validation lost. Please re-open the lot picker.').prop('hidden', false);
            return;
        }

        $.ajax({
            url: '/api/GrowerDelivery/WeightSheet/' + activeWsId,
            method: 'PATCH',
            contentType: 'application/json',
            data: JSON.stringify({ LotId: selectedChangeLotId, Pin: _lotChangePinValidated })
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
    // SCAN BOL — open the camera picker modal for the current load.
    // Hidden until /api/cameras/bol/available reports at least one BOL camera
    // for this location. The actual modal logic lives in scan.bol.js (window.scanBol).
    // ══════════════════════════════════════════════════════════════════════════
    function wireBolScanButton(locationId) {
        var $btn = $('#gdBolScanBtn');
        if (!$btn.length || !window.scanBol) return;

        // Hide until we know a BOL camera is configured. Re-checks on every
        // edit-mode load in case admins added one while the form was open.
        $btn.prop('hidden', true);
        window.scanBol.checkAvailable(locationId).then(function (available) {
            $btn.prop('hidden', !available);
        });

        $btn.off('click.gmBol').on('click.gmBol', function () {
            if (!editTxnId) {
                // The image is named {loadNumber}_bol.jpg — we need a saved
                // load id first. Tell the operator to save the inbound side.
                if (window.DevExpress && DevExpress.ui && DevExpress.ui.notify) {
                    DevExpress.ui.notify('Save the inbound weight first, then scan the BOL.', 'info', 3500);
                } else {
                    alert('Save the inbound weight first, then scan the BOL.');
                }
                return;
            }
            window.scanBol.open(editTxnId, locationId);
        });
    }

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
