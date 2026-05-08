(function () {
    'use strict';

    // ── Helpers ─────────────────────────────────────────────────────────────

    function formatLotId(id) {
        var s = String(id);
        if (s.length < 7) return s;
        return s.substring(0, 3) + '-' + s.substring(3, 6) + '-' + s.substring(6);
    }

    function dxInstance(sel) {
        try { return $(sel).dxSelectBox('instance'); } catch (e) { return null; }
    }

    function dxNumberInstance(sel) {
        try { return $(sel).dxNumberBox('instance'); } catch (e) { return null; }
    }

    async function tryParseError(resp) {
        try {
            var j = await resp.json();
            return j.message || j.title || resp.statusText;
        } catch (e) { return resp.statusText; }
    }

    function showAlert(msg, type) {
        var el = $('#nwsAlert');
        el.removeClass('alert-success alert-danger alert-info')
          .addClass('alert alert-' + type)
          .text(msg)
          .prop('hidden', false);
        setTimeout(function () { el.prop('hidden', true); }, 4000);
    }

    // ── Selectors ────────────────────────────────────────────────────────────

    var SEL = {
        // Step panels
        step1:          '#nwsStep1',
        step2:          '#nwsStep2',
        step3:          '#nwsStep3',

        // Step 1 — Lots
        lotListPanel:   '#nwsLotListPanel',
        lotsGrid:       '#nwsLotsGrid',
        newLotBtn:      '#nwsNewLotBtn',
        search:         '#nwsSearch',

        // Step 2 — BOL & Hauler
        selectedLotInfo:    '#nwsSelectedLotInfo',
        backToLots:         '#nwsBackToLots',
        bolType:            '#nwsBolType',
        bolTypeHint:        '#nwsBolTypeHint',
        haulerMilesDetails: '#nwsHaulerMilesDetails',
        hauler:             '#nwsHauler',
        miles:              '#nwsMiles',
        customDetails:      '#nwsCustomDetails',
        customHauler:       '#nwsCustomHaulerSelect',
        customRateDesc:     '#nwsCustomRateDesc',
        customRate:         '#nwsCustomRate',
        haulerError:        '#nwsHaulerError',
        createWsBtn:        '#nwsCreateWsBtn',

        // Step 3 — Success
        successInfo:     '#nwsSuccessInfo',
        addLoadBtn:      '#nwsAddLoadBtn',
    };

    // ── State ────────────────────────────────────────────────────────────────

    var currentLocationId = null;

    var _lotsCache = [];

    // Lot type from URL (seed or warehouse)
    var _lotType = (new URLSearchParams(window.location.search).get('lotType') || 'seed').toLowerCase();

    // Wizard state
    var _selectedLot = null;         // { LotId, LotDescription, SplitGroupDescription, SplitGroupId, ... }
    var _wsPin = null;               // PIN captured from modal before WS creation
    var _createdWsId = null;
    var _milesEntered = false;       // tracks whether user explicitly entered miles

    // Move-load round-trip: when this page is reached from the Move Load
    // modal, we cache the source load's variety so we can warn the operator
    // if the lot they're about to use on the new WS has a different variety.
    var _sourceVarietyForMove = '';

    // ── Init ─────────────────────────────────────────────────────────────────

    $(function () {
        initLocation();
        initLotsGrid();
        initCreateLotForm();
        initHaulerStep();
        wireNavigation();
        wirePinModal();
        wireMoveFlowBack();
    });

    // When this page was reached via the Move Load round-trip, the back bar
    // and the trailing "Back to Weight Sheets" success-screen link should
    // bounce the operator back to /GrowerDelivery/Index?resumeMove=1 with
    // the cached PIN, instead of leaving them in /WeightSheets. Also fetches
    // the source load's variety so we can warn on cross-variety choices.
    function wireMoveFlowBack() {
        var qp = new URLSearchParams(window.location.search);
        if (qp.get('returnTo') !== 'move') return;

        var pin      = parseInt(qp.get('pin'),      10) || 0;
        var fromWsId = parseInt(qp.get('fromWsId'), 10) || 0;
        var txnId    = parseInt(qp.get('txnId'),    10) || 0;
        if (!fromWsId || !txnId) return;

        var resume = new URLSearchParams();
        resume.set('wsId',       String(fromWsId));
        resume.set('txnId',      String(txnId));
        resume.set('resumeMove', '1');
        if (pin) resume.set('pin', String(pin));
        var resumeUrl = '/GrowerDelivery/Index?' + resume.toString();

        $('#nwsBackBar').attr('href', resumeUrl);
        $('a.btn[href="/WeightSheets"]').attr('href', resumeUrl);

        // Cache the source load's variety so showStep2 / new-lot navigation
        // can warn the operator if the chosen lot's variety doesn't match.
        $.getJSON('/api/GrowerDelivery/' + txnId + '/move-candidates')
            .done(function (resp) {
                _sourceVarietyForMove = (resp && resp.SourceVariety) || '';
            })
            .fail(function () { /* silent — warning just won't fire */ });
    }

    // ── PIN modal ────────────────────────────────────────────────────────────

    function wirePinModal() {
        // Read PIN from URL query param (passed from LoadType page)
        var urlPin = parseInt(new URLSearchParams(window.location.search).get('pin'), 10) || 0;
        if (urlPin > 0) {
            _wsPin = urlPin;
            return; // PIN already validated on LoadType page
        }

        // No PIN in URL — gate via the shared GM.requestPin. If the operator
        // cancels, send them back to the WS list (this page is unusable
        // without a validated PIN).
        GM.requestPin({ prompt: 'Enter your PIN to create a new weight sheet.' })
            .then(function (result) { _wsPin = result.pin; })
            .catch(function () { window.location.href = '/WeightSheets'; });
    }

    // ── Location ─────────────────────────────────────────────────────────────

    async function initLocation() {
        try {
            var resp = await fetch('/api/LocationContextApi/current');
            var current = await resp.json();
            if (current.HasLocation && current.LocationId) {
                currentLocationId = current.LocationId;
                showStep1();
                refreshLots();
            }
        } catch (ex) {
            console.warn('[NewWeightSheet] Could not read current location', ex);
        }
    }

    // ── Step navigation ──────────────────────────────────────────────────────

    function showStep1() {
        $(SEL.step1).prop('hidden', false);
        $(SEL.step2).prop('hidden', true);
        $(SEL.step3).prop('hidden', true);
        $(SEL.lotListPanel).prop('hidden', false);
    }

    function showStep2() {
        $(SEL.step1).prop('hidden', true);
        $(SEL.step2).prop('hidden', false);
        $(SEL.step3).prop('hidden', true);

        // Populate lot confirmation details
        if (_selectedLot) {
            var lot = _selectedLot;
            $('#nwsConfirmLotId').text(formatLotId(lot.LotId));
            $('#nwsConfirmLotDesc').text(lot.LotDescription || '\u2014');
            $('#nwsConfirmItem').text(lot.ItemDescription || '\u2014');
            $('#nwsConfirmAccount').text(lot.AccountName || '\u2014');
            $('#nwsConfirmSplitGroup').text(
                lot.SplitGroupId
                    ? lot.SplitGroupId + (lot.SplitGroupDescription ? ' - ' + lot.SplitGroupDescription : '')
                    : '\u2014'
            );
            $('#nwsConfirmState').text(lot.State || '\u2014');
            $('#nwsConfirmCounty').text(lot.County || '\u2014');
            $('#nwsConfirmLandlord').text(lot.Landlord || '\u2014');
            $('#nwsConfirmFarmNumber').text(lot.FarmNumber || '\u2014');
        }

        // Cross-variety warning (only meaningful in the move-load round-trip):
        // if the chosen lot's variety differs from the source load's variety,
        // surface a danger banner so the operator knows before continuing.
        var $vw = $('#nwsMoveVarietyWarn');
        if (_sourceVarietyForMove
            && _selectedLot
            && _selectedLot.ItemDescription
            && _selectedLot.ItemDescription !== _sourceVarietyForMove) {
            $('#nwsMoveVarietyWarnText').text(
                'You are moving the load to a new variety — from "'
                + _sourceVarietyForMove + '" to "'
                + _selectedLot.ItemDescription + '".'
            );
            $vw.prop('hidden', false);
        } else {
            $vw.prop('hidden', true);
        }

        // Reset BOL & hauler state
        $(SEL.haulerError).prop('hidden', true).text('');
        resetBolAndHaulerFields();
    }

    function showStep3(wsId) {
        // Store the weight sheet ID in a cookie so the delivery page auto-selects it.
        document.cookie = "GrainMgmt_WsId=" + wsId + ";path=/;max-age=86400;SameSite=Lax";

        // Carry the PIN forward in the URL so the FIRST load on this freshly-
        // created WS doesn't re-prompt — the operator just validated. The
        // delivery page reads ?newWs=1&pin= once on init, sets its in-page
        // first-load PIN, then strips both from the URL bar so a refresh
        // can't reuse a stale pin. Subsequent loads always re-prompt.
        var params = new URLSearchParams();
        params.set('wsId', String(wsId));
        params.set('newWs', '1');
        if (_wsPin) params.set('pin', String(_wsPin));
        window.location.href = '/GrowerDelivery/Index?' + params.toString();
    }

    // ── Wire navigation buttons ──────────────────────────────────────────────

    function wireNavigation() {
        $(SEL.backToLots).on('click', function () {
            _selectedLot = null;
            showStep1();
        });
    }

    // ══════════════════════════════════════════════════════════════════════════
    // STEP 1: LOT SELECTION + CREATION
    // ══════════════════════════════════════════════════════════════════════════

    function initLotsGrid() {
        $(SEL.lotsGrid).dxDataGrid({
            dataSource: [],
            keyExpr: 'LotId',
            showBorders: true,
            showRowLines: true,
            rowAlternationEnabled: true,
            columnAutoWidth: true,
            wordWrapEnabled: false,
            scrolling: { columnRenderingMode: 'virtual' },
            noDataText: 'No open lots at this location.',
            paging: { enabled: true, pageSize: 20 },
            pager: { visible: true, showPageSizeSelector: true, allowedPageSizes: [10, 20, 50], showInfo: true },
            sorting: { mode: 'multiple' },
            // Whole-row click acts as Select. The dedicated Select button
            // stays for discoverability, but operators on touch screens
            // (kiosk weighmasters) hit the row faster than the small btn.
            // The masterDetail expand icon has its own click region — DX
            // suppresses our row-click when that icon is the actual target.
            onRowClick: function (e) {
                if (e.rowType !== 'data' || !e.data) return;
                _selectedLot = e.data;
                showStep2();
            },
            onRowPrepared: function (e) {
                if (e.rowType === 'data') e.rowElement.css('cursor', 'pointer');
            },
            columns: [
                {
                    dataField: 'LotId',
                    caption: 'Lot ID',
                    sortOrder: 'desc',
                    sortIndex: 0,
                    customizeText: function (cellInfo) { return formatLotId(cellInfo.value); },
                },
                {
                    dataField: 'LotDescription',
                    caption: 'Lot Description',
                },
                {
                    dataField: 'ItemDescription',
                    caption: 'Item',
                },
                {
                    dataField: 'AccountName',
                    caption: 'Primary Account',
                },
                {
                    caption: '',
                    width: 100,
                    fixed: true,
                    fixedPosition: 'right',
                    allowSorting: false,
                    cellTemplate: function (container, options) {
                        var lot = options.data;
                        $('<button>')
                            .addClass('btn btn-sm btn-primary')
                            .text('Select')
                            .on('click', function () {
                                _selectedLot = lot;
                                showStep2();
                            })
                            .appendTo(container);
                    },
                },
            ],
            masterDetail: {
                enabled: true,
                template: function (container, options) {
                    var lotId = options.data.LotId;
                    var $detail = $('<div>').appendTo(container);
                    $('<div>').addClass('fw-semibold mb-1').css('font-size', '0.85rem')
                        .text('Split Group — ' + (options.data.SplitGroupDescription || 'N/A')).appendTo($detail);
                    var $grid = $('<div>').appendTo($detail);
                    $.getJSON('/api/GrowerDelivery/LotSplitGroup/' + lotId).done(function (data) {
                        $grid.dxDataGrid({
                            dataSource: data,
                            showBorders: true,
                            showRowLines: true,
                            columnAutoWidth: true,
                            paging: { enabled: false },
                            columns: [
                                { dataField: 'AccountName', caption: 'Account' },
                                { dataField: 'SplitPercent', caption: '%', width: 80,
                                    format: { type: 'percent', precision: 2 },
                                    alignment: 'right' },
                                { dataField: 'PrimaryAccount', caption: 'Primary', width: 80,
                                    dataType: 'boolean' },
                            ],
                        });
                    });
                },
            },
        });
    }

    async function refreshLots() {
        if (!currentLocationId) return;

        var grid;
        try { grid = $(SEL.lotsGrid).dxDataGrid('instance'); } catch (e) { return; }
        if (!grid) return;

        grid.beginCustomLoading('Loading\u2026');

        try {
            var allLots = await $.getJSON('/api/GrowerDelivery/OpenLots?locationId=' + currentLocationId);
            // Filter to only lots matching the selected lot type (0=Seed, 1=Warehouse)
            var lotTypeInt = _lotType === 'warehouse' ? 1 : 0;
            _lotsCache = allLots.filter(function (l) { return l.LotType === lotTypeInt; });
        } catch (ex) {
            _lotsCache = [];
        }

        grid.option('dataSource', _lotsCache);
        grid.endCustomLoading();

        applySearch();

        // If EditWeightSheetLot just sent us back with ?selectLotId=<id>,
        // auto-advance to Step 2 for that lot.
        var selectLotId = parseInt(new URLSearchParams(window.location.search).get('selectLotId'), 10) || null;
        if (selectLotId) {
            var match = _lotsCache.find(function (l) { return l.LotId === selectLotId; });
            if (match) {
                _selectedLot = match;
                // Strip selectLotId from the URL so a page refresh doesn't reselect.
                var cleanParams = new URLSearchParams(window.location.search);
                cleanParams.delete('selectLotId');
                var newQs = cleanParams.toString();
                history.replaceState(null, '', window.location.pathname + (newQs ? '?' + newQs : ''));
                showStep2();
            }
        }
    }

    function applySearch() {
        var grid;
        try { grid = $(SEL.lotsGrid).dxDataGrid('instance'); } catch (e) { return; }
        if (!grid) return;

        var term = ($(SEL.search).val() || '').trim();
        if (!term) {
            grid.clearFilter();
            return;
        }

        grid.filter([
            ['LotDescription', 'contains', term],
            'or',
            ['SplitGroupDescription', 'contains', term],
        ]);
    }

    // ── Create Lot Form (replicates grower.wslots.js patterns) ───────────────

    function initCreateLotForm() {
        // "New Lot" navigates to the dedicated EditWeightSheetLot page.
        // After the lot is created there, EditWeightSheetLot returns to this
        // page with ?selectLotId=<id>, which refreshLots() auto-selects.
        //
        // Lot create is gated on AddLots (priv 9). The operator already
        // entered a PIN to start this WS — if that PIN satisfies,
        // GM.requestPin resolves immediately from the cache and there is no
        // second prompt. Otherwise we prompt for a PIN that does, and
        // forward THAT pin to EditWeightSheetLot (overriding the WS pin)
        // so the lot-create endpoint accepts it.
        $(SEL.newLotBtn).dxButton({
            text: 'New Lot',
            icon: 'add',
            stylingMode: 'outlined',
            type: 'default',
            onClick: function () {
                GM.requestPin({
                    prompt: 'Enter a PIN with lot privileges to create a new lot.',
                    requiredPrivilegeId: 9 // AddLots
                }).then(function (lotPin) {
                    var params = new URLSearchParams();
                    params.set('lotType', _lotType);
                    // Prefer the lot-priv PIN we just validated. It satisfies
                    // both the New Lot gate AND any subsequent WS-create call
                    // since lot-priv users almost always also have basic
                    // create-WS rights. If for some reason it doesn't, the
                    // server will surface a clear error.
                    var pinForUrl = (lotPin && lotPin.pin) || _wsPin;
                    if (pinForUrl) params.set('pin', String(pinForUrl));
                    params.set('returnTo', 'newws');
                    // Carry source variety through so EditWeightSheetLot can warn
                    // when the operator's about to save a cross-variety lot during
                    // a move-load round-trip.
                    if (_sourceVarietyForMove) params.set('sourceVariety', _sourceVarietyForMove);

                    // Forward move-flow context (when present) so EditWeightSheetLot
                    // can echo it back on return — preserves the move flow across the
                    // lot-creation sub-trip, so NewWeightSheet won't lose its
                    // returnTo=move / txnId / fromWsId after the lot is saved.
                    var here = new URLSearchParams(window.location.search);
                    var moveTxnId    = here.get('txnId');
                    var moveFromWsId = here.get('fromWsId');
                    if (moveTxnId)    params.set('moveTxnId',    moveTxnId);
                    if (moveFromWsId) params.set('moveFromWsId', moveFromWsId);

                    window.location.href = '/GrowerDelivery/EditWeightSheetLot?' + params.toString();
                }).catch(function () { /* user cancelled or invalid PIN */ });
            }
        });

        wireSearch();
    }

    function wireSearch() {
        var debounceTimer = null;
        $(SEL.search).on('input', function () {
            clearTimeout(debounceTimer);
            debounceTimer = setTimeout(applySearch, 300);
        });
    }


    // ══════════════════════════════════════════════════════════════════════════
    // STEP 2: BOL TYPE & HAULER SELECTION
    // ══════════════════════════════════════════════════════════════════════════

    function initHaulerStep() {
        // ── BOL Type dropdown (required, prominent) ──
        $(SEL.bolType).dxSelectBox({
            dataSource: [
                { Value: 'U', Text: 'Universal' },
                { Value: 'A', Text: 'Along Side the Field' },
                { Value: 'F', Text: 'Farm Storage' },
                { Value: 'C', Text: 'Custom' },
            ],
            valueExpr:   'Value',
            displayExpr: 'Text',
            placeholder: 'Select BOL Type\u2026',
            showClearButton: true,
            onValueChanged: function (e) {
                onBolTypeChanged(e.value);
            },
        });

        // ── Hauler + Miles (shared for A and F) ──
        $(SEL.hauler).dxSelectBox({
            dataSource:          [],
            valueExpr:           'Id',
            displayExpr:         'Description',
            searchEnabled:       true,
            searchExpr:          'Description',
            searchMode:          'contains',
            showDataBeforeSearch: true,
            minSearchLength:     0,
            placeholder:         'Select hauler\u2026',
            showClearButton:     true,
        });

        $(SEL.miles).dxNumberBox({
            value: undefined,
            min: 0,
            format: '#0.##',
            placeholder: 'Miles\u2026',
            inputAttr: { style: 'text-align:right;font-size:15px;' },
            onValueChanged: function (e) {
                _milesEntered = (e.value !== null && e.value !== undefined);
            },
        });

        // ── Custom: Hauler + Rate Description + Rate ──
        $(SEL.customHauler).dxSelectBox({
            dataSource:          [],
            valueExpr:           'Id',
            displayExpr:         'Description',
            searchEnabled:       true,
            searchExpr:          'Description',
            searchMode:          'contains',
            showDataBeforeSearch: true,
            minSearchLength:     0,
            placeholder:         'Select hauler\u2026',
            showClearButton:     true,
        });

        $(SEL.customRate).dxNumberBox({
            min: 0,
            format: '#0.00',
            placeholder: 'Rate\u2026',
            inputAttr: { style: 'text-align:right' },
        });

        // Create Weight Sheet button
        $(SEL.createWsBtn).on('click', function () {
            createWeightSheet();
        });

        // "None" rate bypass — hides BOL type / hauler / rate fields. The
        // server stamps RateType='N', CustomRateDescription='None', Rate=0,
        // Miles=0, no hauler. Requires CK_WeightSheets_RateType to allow 'N'
        // (see SQL/AddInHouseRateType.sql).
        $('#nwsNoneRate').on('change', function () {
            var on = this.checked;
            $('#nwsBolTypeWrap').prop('hidden', on);
            $(SEL.haulerMilesDetails).prop('hidden', on ? true : $(SEL.haulerMilesDetails).prop('hidden'));
            $(SEL.customDetails).prop('hidden', on ? true : $(SEL.customDetails).prop('hidden'));
            $(SEL.bolTypeHint).prop('hidden', on || !$(SEL.bolTypeHint).text());
            $(SEL.haulerError).prop('hidden', true);
            if (on) {
                var bolTypeInst = dxInstance(SEL.bolType);
                if (bolTypeInst) bolTypeInst.reset();
                resetBolAndHaulerFields();
                $(SEL.createWsBtn).prop('disabled', false);
            } else {
                $(SEL.createWsBtn).prop('disabled', true);
            }
        });
    }

    async function onBolTypeChanged(val) {
        // Hide everything first
        $(SEL.haulerMilesDetails).prop('hidden', true);
        $(SEL.customDetails).prop('hidden', true);
        $(SEL.haulerError).prop('hidden', true).text('');
        $(SEL.bolTypeHint).prop('hidden', true);
        $(SEL.createWsBtn).prop('disabled', true);

        // Reset all BOL-related fields
        var haulerInst = dxInstance(SEL.hauler);
        if (haulerInst) haulerInst.reset();
        var milesInst = dxNumberInstance(SEL.miles);
        if (milesInst) milesInst.reset();
        _milesEntered = false;

        var customHaulerInst = dxInstance(SEL.customHauler);
        if (customHaulerInst) customHaulerInst.reset();
        $('#nwsCustomRateDesc').val('');
        var customRateInst = dxNumberInstance(SEL.customRate);
        if (customRateInst) customRateInst.reset();

        if (!val) return;

        if (val === 'U') {
            // Universal — no hauler or rate needed
            $(SEL.bolTypeHint).text('Universal BOL \u2014 no hauler or rate needed.').prop('hidden', false);
            $(SEL.createWsBtn).prop('disabled', false);
        } else if (val === 'A' || val === 'F') {
            // Along Side the Field / Farm Storage — need hauler + miles
            var label = val === 'A' ? 'Along Side the Field' : 'Farm Storage';
            $(SEL.bolTypeHint).text(label + ' \u2014 select a hauler and enter miles for rate calculation.').prop('hidden', false);
            $(SEL.haulerMilesDetails).prop('hidden', false);
            $(SEL.createWsBtn).prop('disabled', false);

            // Load haulers
            await loadHaulersInto(SEL.hauler);
        } else if (val === 'C') {
            // Custom — hauler + rate description + rate
            $(SEL.bolTypeHint).text('Custom \u2014 select a hauler, describe the rate, and enter an amount.').prop('hidden', false);
            $(SEL.customDetails).prop('hidden', false);
            $(SEL.createWsBtn).prop('disabled', false);

            // Load haulers
            await loadHaulersInto(SEL.customHauler);
        }
    }

    async function loadHaulersInto(selector) {
        try {
            var haulers = await $.getJSON('/api/Lookups/Haulers');
            var inst = dxInstance(selector);
            if (inst) inst.option('dataSource', haulers);
        } catch (ex) {
            showAlert('Failed to load haulers.', 'danger');
        }
    }

    function resetBolAndHaulerFields() {
        // BOL Type
        var bolTypeInst = dxInstance(SEL.bolType);
        if (bolTypeInst) bolTypeInst.reset();

        $(SEL.bolTypeHint).prop('hidden', true);

        // Hauler + Miles fields
        $(SEL.haulerMilesDetails).prop('hidden', true);
        var haulerInst = dxInstance(SEL.hauler);
        if (haulerInst) haulerInst.reset();
        var milesInst = dxNumberInstance(SEL.miles);
        if (milesInst) milesInst.option('value', undefined);
        _milesEntered = false;

        // Custom fields
        $(SEL.customDetails).prop('hidden', true);
        var customHaulerInst = dxInstance(SEL.customHauler);
        if (customHaulerInst) customHaulerInst.reset();
        var crInst = dxNumberInstance(SEL.customRate);
        if (crInst) crInst.reset();
        $(SEL.customRateDesc).val('');

        $(SEL.createWsBtn).prop('disabled', true);
    }

    // ══════════════════════════════════════════════════════════════════════════
    // WEIGHT SHEET CREATION
    // ══════════════════════════════════════════════════════════════════════════

    // Map BOL type codes to display names for CustomRateDescription
    var BOL_TYPE_NAMES = { 'U': 'Universal', 'A': 'Along Side the Field', 'F': 'Farm Storage', 'C': 'Custom' };

    async function createWeightSheet() {
        if (!_selectedLot) {
            showAlert('No lot selected.', 'danger');
            return;
        }

        if (!_wsPin) {
            $(SEL.haulerError).text('PIN is required. Please go back and enter your PIN.').prop('hidden', false);
            return;
        }

        // ── "None" bypass: short-circuits BOL/hauler/rate validation ──
        if ($('#nwsNoneRate').is(':checked')) {
            var nonePayload = {
                LocationId:            currentLocationId,
                LotId:                 _selectedLot.LotId,
                RateType:              'N',
                HaulerId:              null,
                Miles:                 0,
                CustomRateDescription: 'None',
                Rate:                  0,
                Pin:                   _wsPin,
            };
            await postWeightSheet(nonePayload);
            return;
        }

        var bolTypeInst = dxInstance(SEL.bolType);
        var bolType = bolTypeInst ? bolTypeInst.option('value') : null;

        // ── Validate BOL Type ──
        if (!bolType) {
            $(SEL.haulerError).text('Please select a BOL Type.').prop('hidden', false);
            return;
        }

        var payload = {
            LocationId: currentLocationId,
            LotId:      _selectedLot.LotId,
            RateType:   bolType,
            Pin:        _wsPin,
        };

        // ── Universal: Rate = 0, no hauler, store description ──
        if (bolType === 'U') {
            payload.Rate = 0;
            payload.CustomRateDescription = BOL_TYPE_NAMES['U'];
        }

        // ── Along Side the Field / Farm Storage: need hauler + miles ──
        if (bolType === 'A' || bolType === 'F') {
            var haulerInst = dxInstance(SEL.hauler);
            var haulerId = haulerInst ? haulerInst.option('value') : null;
            if (!haulerId) {
                $(SEL.haulerError).text('Please select a hauler.').prop('hidden', false);
                return;
            }

            var milesInst = dxNumberInstance(SEL.miles);
            var miles = milesInst ? milesInst.option('value') : null;
            if (!_milesEntered || miles === null || miles === undefined) {
                $(SEL.haulerError).text('Please enter the miles (even if zero).').prop('hidden', false);
                return;
            }

            payload.HaulerId = haulerId;
            payload.Miles = miles;
            payload.CustomRateDescription = BOL_TYPE_NAMES[bolType];

            try {
                var rateData = await $.getJSON('/api/Lookups/HaulerRateForMiles?rateType=' + bolType + '&miles=' + miles);
                payload.Rate = rateData.Rate;
            } catch (ex) {
                $(SEL.haulerError).text('No rate tier found for ' + miles + ' miles.').prop('hidden', false);
                return;
            }
        }

        // ── Custom: hauler + rate description + rate ──
        if (bolType === 'C') {
            var customHaulerInst = dxInstance(SEL.customHauler);
            var cHaulerId = customHaulerInst ? customHaulerInst.option('value') : null;
            if (!cHaulerId) {
                $(SEL.haulerError).text('Please select a hauler.').prop('hidden', false);
                return;
            }

            var customDesc = $(SEL.customRateDesc).val().trim();
            var crInst = dxNumberInstance(SEL.customRate);
            var customRate = crInst ? crInst.option('value') : null;

            if (!customDesc) {
                $(SEL.haulerError).text('Please enter a rate description.').prop('hidden', false);
                return;
            }
            if (customRate === null || customRate === undefined) {
                $(SEL.haulerError).text('Please enter a rate amount.').prop('hidden', false);
                return;
            }

            payload.HaulerId = cHaulerId;
            payload.CustomRateDescription = customDesc;
            payload.Rate = customRate;
        }

        await postWeightSheet(payload);
    }

    async function postWeightSheet(payload) {
        $(SEL.haulerError).prop('hidden', true);
        var btn = $(SEL.createWsBtn);
        btn.prop('disabled', true);

        try {
            var resp = await fetch('/api/GrowerDelivery/WeightSheets', {
                method:  'POST',
                headers: { 'Content-Type': 'application/json' },
                body:    JSON.stringify(payload),
            });

            if (!resp.ok) {
                var detail = await tryParseError(resp);
                $(SEL.haulerError).text('Error: ' + detail).prop('hidden', false);
                return;
            }

            var result = await resp.json();

            // Move-load round-trip: if the operator launched this WS-create
            // from the Move Load modal, send them straight back to the load
            // edit page with ?moveTo=<newWsId>&pin=<pin> so the receiving
            // page can auto-perform the move without a second PIN prompt.
            var qp = new URLSearchParams(window.location.search);
            if (qp.get('returnTo') === 'move') {
                var fromWsId = parseInt(qp.get('fromWsId'), 10) || 0;
                var txnId    = parseInt(qp.get('txnId'),    10) || 0;
                var carryPin = parseInt(qp.get('pin'),      10) || _wsPin || 0;
                if (fromWsId && txnId && result.WeightSheetId) {
                    var ret = new URLSearchParams();
                    ret.set('wsId',   String(fromWsId));
                    ret.set('txnId',  String(txnId));
                    ret.set('moveTo', String(result.WeightSheetId));
                    if (carryPin) ret.set('pin', String(carryPin));
                    window.location.href = '/GrowerDelivery/Index?' + ret.toString();
                    return;
                }
            }

            showStep3(result.WeightSheetId);

        } catch (ex) {
            $(SEL.haulerError).text('Network error: ' + ex.message).prop('hidden', false);
        } finally {
            btn.prop('disabled', false);
        }
    }

})();
