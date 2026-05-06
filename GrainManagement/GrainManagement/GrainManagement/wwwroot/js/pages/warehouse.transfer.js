(function () {
    'use strict';

    // ── State ────────────────────────────────────────────────────────────────
    var _params       = new URLSearchParams(window.location.search);
    var _lotType      = (_params.get('lotType') || 'warehouse').toLowerCase(); // 'seed' | 'warehouse'
    var _pin          = parseInt(_params.get('pin'), 10) || 0;
    var _wsId         = parseInt(_params.get('wsId'), 10) || 0;
    var _currentLocId = 0;
    var _currentLocName = '';
    var _direction    = 'Received';
    var _wsRowUid     = null;
    var _wsItemId     = 0;
    var _wsItemName   = '';
    var _itemsCache   = [];
    var _locationsCache = [];
    var _calculatedRate = null;   // Numeric rate fetched for current type+miles.

    // ── Boot ─────────────────────────────────────────────────────────────────
    $(function () {
        if (!_pin && !_wsId) {
            window.location.href = '/WeightSheets/LoadType';
            return;
        }
        initLocation()
            .then(initPickers)
            .then(wireEvents)
            .then(function () {
                if (_wsId > 0) {
                    loadExistingWeightSheet(_wsId);
                }
            });
    });

    async function initLocation() {
        try {
            var current = await $.getJSON('/api/LocationContextApi/current');
            if (current && current.HasLocation && current.LocationId) {
                _currentLocId   = current.LocationId;
                _currentLocName = current.LocationName || current.Name || '';
                $('#trCurrentLocationName').text(_currentLocName || ('#' + _currentLocId));
            }
        } catch (ex) {
            showAlert('Could not read current location: ' + ex.message, 'danger');
        }
    }

    async function initPickers() {
        // Variety — same lookup the warehouse / seed flows use.
        var itemsUrl = (_lotType === 'seed')
            ? '/api/Lookups/SeedItems'
            : '/api/Lookups/WarehouseItems';
        try { _itemsCache = await $.getJSON(itemsUrl) || []; }
        catch (ex) { _itemsCache = []; }

        if (_lotType === 'seed') {
            // Seed mode uses the same popup-grid picker as the seed lot editor:
            // search box + scrollable grid, no filter dropdowns.
            $('#trItem').prop('hidden', true);
            $('#trItemSeedWrap').prop('hidden', false);
            initSeedVarietyPopup();
        } else {
            $('#trItem').dxSelectBox({
                dataSource: _itemsCache,
                displayExpr: 'Name',
                valueExpr:   'ItemId',
                placeholder: 'Select variety…',
                searchEnabled: true,
            });
        }

        // Counterpart location — opposite-list, exclude current location.
        var locUrl = (_lotType === 'seed')
            ? '/api/locations/SeedLocationsList'
            : '/api/locations/WarehouseLocationsList';
        try { _locationsCache = await $.getJSON(locUrl) || []; }
        catch (ex) { _locationsCache = []; }

        var counterpartLocations = _locationsCache.filter(function (l) {
            return l.LocationId !== _currentLocId;
        });

        $('#trCounterpartLocation').dxSelectBox({
            dataSource: counterpartLocations,
            displayExpr: 'Name',
            valueExpr:   'LocationId',
            placeholder: 'Select location…',
            searchEnabled: true,
        });

        // Hauler
        try {
            var haulers = await $.getJSON('/api/Lookups/Haulers') || [];
            $('#trHauler').dxSelectBox({
                dataSource: haulers,
                displayExpr: 'Description',
                valueExpr:   'Id',
                placeholder: 'Select hauler…',
                searchEnabled: true,
            });
        } catch (ex) { /* hauler optional */ }

        // Rate type — A/F/U/C
        $('#trRateType').dxSelectBox({
            items: [
                { Code: 'A', Name: 'Along Side Field' },
                { Code: 'F', Name: 'Farm Storage' },
                { Code: 'U', Name: 'Universal' },
                { Code: 'C', Name: 'Custom' },
            ],
            displayExpr: 'Name',
            valueExpr:   'Code',
            placeholder: 'Select rate type…',
            value: 'A',
            onValueChanged: function (e) {
                var isCustom = e.value === 'C';
                $('#trCustomRateRow').prop('hidden', !isCustom);
                $('#trMilesWrap').prop('hidden',     isCustom);
                $('#trCalcRateWrap').prop('hidden',  isCustom);
                refreshCalculatedRate();
            },
        });
    }

    function wireEvents() {
        $('input[name="trDirection"]').on('change', function () {
            _direction = $(this).val();
            var rcv = (_direction === 'Received');
            $('#trDirHelpRcv').prop('hidden', !rcv);
            $('#trDirHelpShp').prop('hidden',  rcv);
            $('#trCounterpartLabelRcv').prop('hidden', !rcv);
            $('#trCounterpartLabelShp').prop('hidden',  rcv);
        });

        $('#trCreateWsBtn').on('click', createWeightSheet);
        $('#trClearLoadBtn').on('click', clearLoadForm);
        $('#trSaveLoadBtn').on('click', saveLoad);

        // Calculated rate refresh as miles is typed (debounced).
        var milesTimer = null;
        $('#trMiles').on('input', function () {
            if (milesTimer) clearTimeout(milesTimer);
            milesTimer = setTimeout(refreshCalculatedRate, 250);
        });

        // In House toggle: hides hauler/rate-type/miles/rate fields. The
        // server stamps RateType='I', CustomRateDescription='In House',
        // Miles=0, Rate=0, HaulerId=null when this is sent.
        $('#trInHouse').on('change', function () {
            var on = this.checked;
            $('#trHaulerWrap').prop('hidden', on);
            $('#trRateTypeWrap').prop('hidden', on);
            $('#trMilesWrap').prop('hidden', on);
            $('#trCalcRateWrap').prop('hidden', on);
            $('#trCustomRateRow').prop('hidden', on || dxValue('#trRateType') !== 'C');
            if (on) {
                try { $('#trHauler').dxSelectBox('instance').reset(); } catch (e) {}
                $('#trMiles').val('');
                $('#trCustomDesc').val('');
                $('#trCustomRate').val('');
                _calculatedRate = null;
                $('#trCalcRate').val('');
            }
        });

        $('input[name="trMode"]').on('change', function () {
            var mode = $(this).val();
            $('#trTruckFields').prop('hidden',  mode !== 'truck');
            $('#trDirectFields').prop('hidden', mode !== 'direct');
        });
    }

    // ── Calculated hauler rate ──────────────────────────────────────────────
    // Looks up the rate tier for the current rate type + miles. Mirrors the
    // intake flow's BOL modal logic. Only A/F/U have a calculated rate; for
    // Custom the field is hidden and the operator types the rate directly.
    async function refreshCalculatedRate() {
        var $cell = $('#trCalcRate');
        var rateType = dxValue('#trRateType');
        var miles    = parseFloat($('#trMiles').val());
        _calculatedRate = null;
        if (!rateType || rateType === 'C' || !miles || miles <= 0) {
            $cell.val('');
            return;
        }
        try {
            var data = await $.getJSON('/api/Lookups/HaulerRateForMiles?rateType=' + rateType + '&miles=' + miles);
            if (data && typeof data.Rate === 'number') {
                _calculatedRate = data.Rate;
                $cell.val('$' + data.Rate.toFixed(2));
            } else {
                $cell.val('');
            }
        } catch (ex) {
            $cell.val('—');
        }
    }

    // ── Seed variety popup picker (mirrors grower.editlot.js) ────────────────
    var _seedPopupInst = null;
    var _seedSearchText = '';

    function initSeedVarietyPopup() {
        $('#trItemSeedPopup').dxPopup({
            title: 'Select Variety',
            visible: false,
            showCloseButton: true,
            width: '90%',
            height: '80%',
            contentTemplate: function (contentEl) {
                var $wrap = $('<div>').css({ display: 'flex', flexDirection: 'column', gap: '8px', height: '100%' });

                var $search = $('<input type="text" class="form-control form-control-sm" placeholder="Search varieties…" />').appendTo($wrap);
                $search.on('input', function () {
                    _seedSearchText = ($(this).val() || '').toLowerCase().trim();
                    refreshSeedGrid();
                });

                var $grid = $('<div>').css({ flex: '1 1 auto', minHeight: '0' }).attr('id', 'trItemSeedGrid').appendTo($wrap);
                $grid.dxDataGrid({
                    dataSource: _itemsCache,
                    keyExpr:    'ItemId',
                    showBorders: true,
                    showRowLines: true,
                    rowAlternationEnabled: true,
                    columnAutoWidth: true,
                    height: '100%',
                    paging: { enabled: true, pageSize: 50 },
                    pager: { visible: true, allowedPageSizes: [25, 50, 100], showPageSizeSelector: true, showInfo: true },
                    sorting: { mode: 'multiple' },
                    noDataText: 'No varieties match the current search.',
                    columns: [
                        { dataField: 'ItemId',      caption: 'Item #',  width: 100, alignment: 'right' },
                        { dataField: 'Name',        caption: 'Variety', sortOrder: 'asc' },
                        { dataField: 'ProductName', caption: 'Product' },
                        { caption: 'Cert Class',   calculateCellValue: function (r) { return traitNameFor(r, 1); }, width: 120 },
                        { caption: 'Herb. System', calculateCellValue: function (r) { return traitNameFor(r, 2); }, width: 140 },
                        { caption: 'Condition',    calculateCellValue: function (r) { return traitNameFor(r, 6); }, width: 120 },
                        {
                            caption: '', width: 90, fixed: true, fixedPosition: 'right',
                            cellTemplate: function (container, options) {
                                $('<button>')
                                    .addClass('btn btn-sm btn-primary')
                                    .text('Select')
                                    .on('click', function () { selectSeedVariety(options.data); })
                                    .appendTo(container);
                            },
                        },
                    ],
                    onRowDblClick: function (e) { selectSeedVariety(e.data); },
                });

                contentEl.append($wrap);
            },
        });
        _seedPopupInst = $('#trItemSeedPopup').dxPopup('instance');

        $('#trItemSeedBtn,#trItemSeedDisplay').on('click', function () {
            if (_seedPopupInst) {
                _seedPopupInst.show();
                setTimeout(refreshSeedGrid, 0);
            }
        });
    }

    function traitNameFor(item, traitTypeId) {
        var traits = (item && item.Traits) || [];
        for (var i = 0; i < traits.length; i++) {
            if (traits[i].TraitTypeId === traitTypeId) return traits[i].TraitName || traits[i].TraitCode || '';
        }
        return '';
    }

    function refreshSeedGrid() {
        var grid;
        try { grid = $('#trItemSeedGrid').dxDataGrid('instance'); } catch (e) { return; }
        if (!grid) return;
        var filtered = (_itemsCache || []).filter(function (item) {
            if (!_seedSearchText) return true;
            var hay = (String(item.ItemId || '') + ' ' + (item.Name || '') + ' ' + (item.ProductName || '')).toLowerCase();
            return hay.indexOf(_seedSearchText) !== -1;
        });
        grid.option('dataSource', filtered);
    }

    function selectSeedVariety(item) {
        if (!item) return;
        $('#trItemSeedId').val(item.ItemId);
        $('#trItemSeedDisplay').val(item.Name || '');
        if (_seedPopupInst) _seedPopupInst.hide();
    }

    // ── WS create ────────────────────────────────────────────────────────────
    async function createWeightSheet() {
        var itemId = (_lotType === 'seed')
            ? (parseInt($('#trItemSeedId').val(), 10) || null)
            : dxValue('#trItem');
        var counterpartLocId = dxValue('#trCounterpartLocation');
        var haulerId = dxValue('#trHauler');
        var rateType = dxValue('#trRateType');
        var miles = parseFloat($('#trMiles').val()) || null;
        var customDesc = $.trim($('#trCustomDesc').val());
        var customRate = parseFloat($('#trCustomRate').val()) || null;

        var isInHouse = !!$('#trInHouse').is(':checked');

        if (!_currentLocId) { showAlert('No current location.', 'danger'); return; }
        if (!itemId)        { showAlert('Variety is required.', 'danger'); return; }
        if (!counterpartLocId) { showAlert('Counterpart location is required.', 'danger'); return; }

        if (!isInHouse) {
            if (!rateType) { showAlert('Rate type is required.', 'danger'); return; }
            if (!haulerId) { showAlert('Hauler is required.', 'danger'); return; }
            if (rateType === 'C' && (!customDesc || !customRate)) {
                showAlert('Custom rate description and rate are required for Custom.', 'danger');
                return;
            }
        }

        // Resolved rate sent to the server: 0 for In House, typed value for
        // Custom, otherwise the calculated rate from the Miles lookup.
        var resolvedRate;
        if (isInHouse)              resolvedRate = 0;
        else if (rateType === 'C')  resolvedRate = customRate;
        else                        resolvedRate = _calculatedRate;
        if (!isInHouse && (!resolvedRate || resolvedRate <= 0)) {
            showAlert(rateType === 'C'
                ? 'Rate is required.'
                : 'Enter Miles so a rate can be calculated.', 'danger');
            return;
        }

        var payload = {
            LocationId:            _currentLocId,
            Direction:             _direction,
            ItemId:                itemId,
            SourceLocationId:      _direction === 'Received' ? counterpartLocId : _currentLocId,
            DestinationLocationId: _direction === 'Received' ? _currentLocId    : counterpartLocId,
            RateType:              isInHouse ? 'I' : rateType,
            HaulerId:              isInHouse ? null : haulerId,
            Miles:                 isInHouse ? 0 : (rateType === 'C' ? null : miles),
            CustomRateDescription: isInHouse ? 'In House' : (rateType === 'C' ? customDesc : null),
            Rate:                  resolvedRate,
            Pin:                   _pin,
        };

        var btn = $('#trCreateWsBtn');
        btn.prop('disabled', true).text('Saving…');
        try {
            const resp = await fetch('/api/Transfer/WeightSheets', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload),
            });
            if (!resp.ok) {
                const err = await tryParseError(resp);
                showAlert('Save failed: ' + err, 'danger');
                return;
            }
            const result = await resp.json();

            // After the WS is created, jump straight to the transfer load
            // entry page so the operator can capture their first load without
            // a manual handoff. PIN rides along so manual-entry confirmations
            // can resolve the user.
            var qs = new URLSearchParams();
            qs.set('wsId', String(result.WeightSheetId));
            if (_pin) qs.set('pin', String(_pin));
            window.location.href = '/GrowerDelivery/WeightSheetTransferLoad?' + qs.toString();
            return;
        } catch (ex) {
            showAlert('Network error: ' + ex.message, 'danger');
        } finally {
            btn.prop('disabled', false).text('Create Weight Sheet');
        }
    }

    // ── WS open / load list ─────────────────────────────────────────────────
    async function loadExistingWeightSheet(wsId) {
        try {
            var resp = await fetch('/api/Transfer/Loads?wsId=' + wsId);
            if (!resp.ok) {
                const err = await tryParseError(resp);
                showAlert('Could not load weight sheet: ' + err, 'danger');
                return;
            }
            var data = await resp.json();
            renderHeader(data.WeightSheet);
            renderLoads(data.Loads || []);
            $('#trPicker').prop('hidden', true);
            $('#trHeader').prop('hidden', false);
            $('#trLoadEntry').prop('hidden', false);
            $('#trLoadsList').prop('hidden', false);
            // Populate the bin pickers now that we know the WS LocationId.
            initBinPickers();
        } catch (ex) {
            showAlert('Network error: ' + ex.message, 'danger');
        }
    }

    function renderHeader(ws) {
        if (!ws) return;
        $('#trHeaderWsId').text(ws.As400Id ? String(ws.As400Id) : ('#' + ws.WeightSheetId));
        $('#trHeaderDirection').text(ws.Direction || '—');
        $('#trHeaderVariety').text((ws.Item && ws.Item.Description) || '—');
        $('#trHeaderSource').text((ws.SourceLocation && ws.SourceLocation.Name) || '—');
        $('#trHeaderDest').text((ws.DestinationLocation && ws.DestinationLocation.Name) || '—');
        var rt = ws.RateType;
        // The DB only allows A/F/U/C — "In House" rides on RateType='C' with
        // CustomRateDescription set. Detect by inspecting that description.
        var isInHouse = (ws.CustomRateDescription || '').toLowerCase() === 'in house';
        $('#trHeaderHauler').text(isInHouse ? 'In House' : (ws.HaulerName || '—'));
        var rtLabel = isInHouse
            ? 'In House'
            : (({ A: 'Along Side Field', F: 'Farm Storage', U: 'Universal', C: 'Custom' })[rt] || (rt || '—'));
        $('#trHeaderRateType').text(rtLabel);
        _wsItemId   = (ws.Item && ws.Item.ItemId) || _wsItemId;
        _wsItemName = (ws.Item && ws.Item.Description) || _wsItemName;
        _direction  = ws.Direction || _direction;
        _wsRowUid   = ws.RowUid;
    }

    function renderLoads(loads) {
        $('#trLoadCount').text(loads.length);
        var dsRows = loads.map(function (l, i) {
            return {
                Row:          i + 1,
                LoadNumber:   l.TransactionId,
                In:           l.InWeight,
                Out:          l.OutWeight,
                Direct:       l.DirectQty,
                Net:          l.Net != null ? l.Net : (l.DirectQty != null ? l.DirectQty : null),
                TruckId:      l.TruckId,
                BOL:          l.BOL,
                Driver:       l.Driver,
                Bin:          l.ContainerDescription,
                StartedAt:    l.StartedAt,
                CompletedAt:  l.CompletedAt,
                Notes:        l.Notes,
                IsVoided:     l.IsVoided,
            };
        });

        var $grid = $('#trLoadsGrid');
        var existing;
        try { existing = $grid.dxDataGrid('instance'); } catch (e) { existing = null; }

        var options = {
            dataSource: dsRows,
            columns: [
                { dataField: 'Row',         caption: '#',          width: 50, alignment: 'center' },
                { dataField: 'LoadNumber',  caption: 'Load #',     width: 110 },
                { dataField: 'TruckId',     caption: 'Truck' },
                { dataField: 'BOL',         caption: 'BOL' },
                { dataField: 'Driver',      caption: 'Driver' },
                { dataField: 'Bin',         caption: 'Bin' },
                { dataField: 'In',          caption: 'In',         dataType: 'number', format: '#,##0' },
                { dataField: 'Out',         caption: 'Out',        dataType: 'number', format: '#,##0' },
                { dataField: 'Direct',      caption: 'Direct',     dataType: 'number', format: '#,##0' },
                { dataField: 'Net',         caption: 'Net',        dataType: 'number', format: '#,##0' },
                { dataField: 'StartedAt',   caption: 'Time In',    dataType: 'datetime',
                  customizeText: window.gmDxServerTime('datetime') },
                { dataField: 'CompletedAt', caption: 'Time Out',   dataType: 'datetime',
                  customizeText: window.gmDxServerTime('datetime') },
                { dataField: 'Notes',       caption: 'Notes' },
            ],
            showBorders: true,
            columnAutoWidth: true,
            wordWrapEnabled: true,
            paging: { enabled: false },
            onRowPrepared: function (e) {
                if (e.rowType === 'data' && e.data && e.data.IsVoided) {
                    $(e.rowElement).css({ 'text-decoration': 'line-through', opacity: 0.55 });
                }
            },
        };

        if (existing) {
            existing.option(options);
        } else {
            $grid.dxDataGrid(options);
        }
    }

    // ── Bin picker ──────────────────────────────────────────────────────────
    async function initBinPickers() {
        try {
            var bins = await $.getJSON('/api/Lookups/ContainerBins?locationId=' + _currentLocId) || [];
            ['#trBin', '#trDirectBin'].forEach(function (selector) {
                var existing;
                try { existing = $(selector).dxSelectBox('instance'); } catch (e) { existing = null; }
                var opts = {
                    dataSource: bins,
                    displayExpr: 'ContainerDescription',
                    valueExpr:   'ContainerId',
                    placeholder: 'Select bin…',
                    searchEnabled: true,
                    showClearButton: true,
                };
                if (existing) existing.option(opts);
                else          $(selector).dxSelectBox(opts);
            });
        } catch (ex) { /* bins optional for outbound */ }
    }

    // ── Save load ───────────────────────────────────────────────────────────
    async function saveLoad() {
        if (!_wsRowUid) { showAlert('Save the weight sheet header first.', 'danger'); return; }

        var mode = $('input[name="trMode"]:checked').val();
        var isTruck = mode === 'truck';
        var startQty   = isTruck ? (parseFloat($('#trStartQty').val()) || null) : null;
        var endQty     = isTruck ? (parseFloat($('#trEndQty').val()) || null) : null;
        var directQty  = !isTruck ? (parseFloat($('#trDirectQty').val()) || null) : null;
        var binId      = isTruck ? dxValue('#trBin') : dxValue('#trDirectBin');
        var truckId    = isTruck ? $.trim($('#trTruckId').val()) : $.trim($('#trDirectTruckId').val());
        var bol        = $.trim($('#trBol').val());
        var driver     = $.trim($('#trDriver').val());
        var notes      = $.trim($('#trNotes').val());

        if (isTruck && !startQty) { showAlert('In Weight is required for truck mode.', 'danger'); return; }
        if (!isTruck && !directQty) { showAlert('Direct Quantity is required for direct mode.', 'danger'); return; }

        var manualSource = await resolveManualSource();
        if (!manualSource) {
            showAlert('Manual quantity source type/method is not configured.', 'danger');
            return;
        }

        // Resolve user behind the saved PIN — required when any source is MANUAL.
        var createdByUserId = await resolvePinUser();
        if (!createdByUserId) {
            showAlert('Could not resolve PIN user — re-enter your PIN and retry.', 'danger');
            return;
        }

        var payload = {
            WeightSheetUid: _wsRowUid,
            LocationId:     _currentLocId,
            StartQty:       startQty,
            EndQty:         endQty,
            DirectQty:      directQty,
            StartQtyMethodId:           isTruck ? manualSource.MethodId : null,
            StartQtySourceTypeId:       isTruck ? manualSource.SourceTypeId : null,
            StartQtyLocation:           isTruck ? _currentLocName : null,
            StartQtySourceDescription:  isTruck ? 'Manual entry' : null,
            EndQtyMethodId:             (isTruck && endQty) ? manualSource.MethodId : null,
            EndQtySourceTypeId:         (isTruck && endQty) ? manualSource.SourceTypeId : null,
            EndQtyLocation:             (isTruck && endQty) ? _currentLocName : null,
            EndQtySourceDescription:    (isTruck && endQty) ? 'Manual entry' : null,
            DirectQtyMethodId:          !isTruck ? manualSource.MethodId : null,
            DirectQtySourceTypeId:      !isTruck ? manualSource.SourceTypeId : null,
            DirectQtyLocation:          !isTruck ? _currentLocName : null,
            DirectQtySourceDescription: !isTruck ? 'Manual entry' : null,
            ToContainers:               binId ? [{ ContainerId: binId, Percent: 100 }] : null,
            BOL:        bol,
            TruckId:    truckId,
            Driver:     driver,
            Notes:      notes,
            CreatedByUserId: createdByUserId,
            RefType:    'WeightSheet',
            RefId:      _wsRowUid,
            CompletedAt: (isTruck && !endQty) ? null : new Date().toISOString(),
            StartedAt:   new Date().toISOString(),
        };

        var btn = $('#trSaveLoadBtn');
        btn.prop('disabled', true).text('Saving…');
        try {
            const resp = await fetch('/api/Transfer', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload),
            });
            if (!resp.ok) {
                const err = await tryParseError(resp);
                showAlert('Save failed: ' + err, 'danger');
                return;
            }
            clearLoadForm();
            await loadExistingWeightSheet(_wsId);
        } catch (ex) {
            showAlert('Network error: ' + ex.message, 'danger');
        } finally {
            btn.prop('disabled', false).text('Save Load');
        }
    }

    function clearLoadForm() {
        $('#trStartQty,#trEndQty,#trDirectQty,#trTruckId,#trDirectTruckId,#trBol,#trDriver,#trNotes').val('');
        ['#trBin', '#trDirectBin'].forEach(function (s) {
            try { $(s).dxSelectBox('instance').reset(); } catch (e) { }
        });
    }

    // ── Tiny helpers ────────────────────────────────────────────────────────
    var _manualSourceCache = null;
    async function resolveManualSource() {
        if (_manualSourceCache) return _manualSourceCache;
        try {
            var [methods, sources] = await Promise.all([
                $.getJSON('/api/Lookups/QuantityMethods?locationId=' + _currentLocId),
                $.getJSON('/api/Lookups/QuantitySourceTypes'),
            ]);
            var manualMethod = (methods || []).find(function (m) { return m.Code === 'MANUAL'; });
            var manualSrc    = (sources || []).find(function (s) { return s.Code === 'MANUAL'; });
            if (!manualMethod || !manualSrc) return null;
            _manualSourceCache = {
                MethodId:     manualMethod.QuantityMethodId,
                SourceTypeId: manualSrc.QuantitySourceTypeId,
            };
            return _manualSourceCache;
        } catch (ex) { return null; }
    }

    var _pinUserCache = null;
    async function resolvePinUser() {
        if (_pinUserCache) return _pinUserCache;
        if (!_pin) return null;
        try {
            var data = await $.getJSON('/api/GrowerDelivery/ValidatePin?pin=' + _pin);
            if (data && data.UserId) {
                _pinUserCache = data.UserId;
                return _pinUserCache;
            }
        } catch (ex) { /* fall through */ }
        return null;
    }

    function dxValue(selector) {
        try { return $(selector).dxSelectBox('instance').option('value'); }
        catch (e) { return null; }
    }

    function showAlert(msg, level) {
        var $el = $('#trAlert');
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
        } catch (e) {
            return resp.status + ' ' + resp.statusText;
        }
    }

})();
