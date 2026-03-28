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
        createLotPanel: '#nwsCreateLotPanel',
        cancelCreateLotBtn: '#nwsCancelCreateLotBtn',
        item:           '#nwsItem',
        sgShortcut:     '#nwsSgShortcut',
        sgError:        '#nwsSgError',
        account:        '#nwsAccount',
        splitGroup:     '#nwsSplitGroup',
        splitGroupWarn: '#nwsSplitGroupWarning',
        state:          '#nwsState',
        county:         '#nwsCounty',
        lotDesc:        '#nwsLotDesc',
        landlord:       '#nwsLandlord',
        notes:          '#nwsNotes',
        createLotError: '#nwsCreateLotError',
        saveLotBtn:     '#nwsSaveLotBtn',

        // Step 2 — BOL & Hauler
        selectedLotInfo:    '#nwsSelectedLotInfo',
        backToLots:         '#nwsBackToLots',
        bolType:            '#nwsBolType',
        bolTypeHint:        '#nwsBolTypeHint',
        haulerMilesDetails: '#nwsHaulerMilesDetails',
        hauler:             '#nwsHauler',
        miles:              '#nwsMiles',
        calcRateGroup:      '#nwsCalcRateGroup',
        calcRate:           '#nwsCalcRate',
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

    // Lot creation caches (mirrors grower.wslots.js)
    var _accountsCache      = null;
    var _allItemsCache      = null;
    var _stateCountiesCache = null;
    var _locationCountiesData       = null;
    var _locationCountiesLocationId = null;
    var _activeStateCounties        = null;
    var _locationItemFiltersCache      = null;
    var _locationItemFiltersLocationId = null;
    var _lotsCache = [];
    var _populating = false;

    // Wizard state
    var _selectedLot = null;         // { LotId, LotDescription, SplitGroupDescription, SplitGroupId, ... }
    var _createdWsId = null;
    var _milesEntered = false;       // tracks whether user explicitly entered miles

    // ── Init ─────────────────────────────────────────────────────────────────

    $(function () {
        initLocation();
        initLotsGrid();
        initCreateLotForm();
        initHaulerStep();
        wireNavigation();
    });

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
        $(SEL.createLotPanel).prop('hidden', true);
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
        }

        // Reset BOL & hauler state
        $(SEL.haulerError).prop('hidden', true).text('');
        resetBolAndHaulerFields();
    }

    function showStep3(wsId) {
        // Auto-redirect to new load entry after weight sheet creation
        window.location.href = '/GrowerDelivery/Index';
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
                    dataField: 'SplitGroupDescription',
                    caption: 'Split Group',
                },
                {
                    dataField: 'AccountName',
                    caption: 'Account',
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
        });
    }

    async function refreshLots() {
        if (!currentLocationId) return;

        var grid;
        try { grid = $(SEL.lotsGrid).dxDataGrid('instance'); } catch (e) { return; }
        if (!grid) return;

        grid.beginCustomLoading('Loading\u2026');

        try {
            _lotsCache = await $.getJSON('/api/GrowerDelivery/OpenLots?locationId=' + currentLocationId);
        } catch (ex) {
            _lotsCache = [];
        }

        grid.option('dataSource', _lotsCache);
        grid.endCustomLoading();

        applySearch();
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

    async function initCreateLotForm() {
        // Load caches
        if (!_accountsCache) {
            try { _accountsCache = await $.getJSON('/api/Lookups/ProducerAccounts'); }
            catch (ex) { _accountsCache = []; }
        }
        if (!_allItemsCache) {
            try { _allItemsCache = await $.getJSON('/api/Lookups/WarehouseItems'); }
            catch (ex) { _allItemsCache = []; }
        }

        initItemPicker();
        initAccountPicker();
        initSplitGroupPicker();
        await initStateCountyPickers();
        wireCreateLotButtons();
        wireSearch();
    }

    function wireSearch() {
        var debounceTimer = null;
        $(SEL.search).on('input', function () {
            clearTimeout(debounceTimer);
            debounceTimer = setTimeout(applySearch, 300);
        });
    }

    function initItemPicker() {
        $(SEL.item).dxSelectBox({
            dataSource:          _allItemsCache,
            valueExpr:           'ItemId',
            displayExpr:         'Name',
            searchEnabled:       true,
            searchExpr:          'Name',
            searchMode:          'contains',
            showDataBeforeSearch: true,
            minSearchLength:     0,
            placeholder:         'Select item\u2026',
            showClearButton:     true,
            onValueChanged: async function (e) {
                if (_populating) return;
                var acctInst = dxInstance(SEL.account);
                var sgInst   = dxInstance(SEL.splitGroup);
                if (e.value) {
                    var accounts = _accountsCache || [];
                    if (acctInst) {
                        acctInst.reset();
                        if (currentLocationId) {
                            try {
                                accounts = await $.getJSON('/api/Lookups/ProducerAccountsForItem?itemId=' + e.value + '&locationId=' + currentLocationId);
                            } catch (ex) { /* fallback to all accounts */ }
                        }
                        acctInst.option('dataSource', accounts);
                        acctInst.option('disabled', false);
                        if (accounts.length === 1) {
                            acctInst.option('value', accounts[0].AccountId);
                        }
                    }
                    $(SEL.sgShortcut).prop('disabled', false);
                } else {
                    if (acctInst) { acctInst.reset(); acctInst.option('dataSource', _accountsCache || []); acctInst.option('disabled', true); }
                    if (sgInst)   { sgInst.reset(); sgInst.option('dataSource', []); sgInst.option('disabled', true); }
                    $(SEL.sgShortcut).val('').prop('disabled', true);
                    $(SEL.sgError).prop('hidden', true).text('');
                    $(SEL.splitGroupWarn).prop('hidden', true);
                }
                updateLotDescription();
            },
        });
    }

    function initAccountPicker() {
        $(SEL.account).dxSelectBox({
            dataSource:          _accountsCache,
            valueExpr:           'AccountId',
            displayExpr:         'Name',
            searchEnabled:       true,
            searchExpr:          'Name',
            searchMode:          'contains',
            showDataBeforeSearch: true,
            minSearchLength:     0,
            placeholder:         'Select account\u2026',
            showClearButton:     true,
            disabled:            true,
            onValueChanged: function (e) {
                if (_populating) return;
                var sgInst = dxInstance(SEL.splitGroup);
                if (sgInst) { sgInst.reset(); sgInst.option('dataSource', []); sgInst.option('disabled', true); }
                $(SEL.lotDesc).val('');
                $(SEL.splitGroupWarn).prop('hidden', true);
                if (e.value) { loadSplitGroups(e.value); }
            },
            onFocusOut: function (e) {
                if (!e.component.option('value')) {
                    e.component.reset();
                    var sgInst = dxInstance(SEL.splitGroup);
                    if (sgInst) { sgInst.reset(); sgInst.option('dataSource', []); sgInst.option('disabled', true); }
                    $(SEL.lotDesc).val('');
                    $(SEL.splitGroupWarn).prop('hidden', true);
                }
            }
        });
    }

    async function loadSplitGroups(accountId) {
        var groups = [];
        try { groups = await $.getJSON('/api/GrowerDelivery/SplitGroupsByAccount?accountId=' + accountId); }
        catch (ex) { /* ignore */ }
        var sgInst = dxInstance(SEL.splitGroup);
        if (sgInst) {
            sgInst.option('dataSource', groups);
            sgInst.option('disabled', groups.length === 0);
            if (groups.length === 1) { sgInst.option('value', groups[0].SplitGroupId); }
        }
        $(SEL.splitGroupWarn).prop('hidden', groups.length > 0);
    }

    function initSplitGroupPicker() {
        $(SEL.splitGroup).dxSelectBox({
            dataSource:          [],
            valueExpr:           'SplitGroupId',
            displayExpr:         function (item) {
                return item ? item.SplitGroupId + ' - ' + item.SplitGroupDescription : '';
            },
            searchEnabled:       true,
            searchExpr:          ['SplitGroupId', 'SplitGroupDescription'],
            searchMode:          'contains',
            showDataBeforeSearch: true,
            minSearchLength:     0,
            placeholder:         'Select split group\u2026',
            showClearButton:     true,
            disabled:            true,
            onValueChanged: function (e) {
                if (!_populating) {
                    $(SEL.sgShortcut).val(e.value || '');
                    updateLotDescription();
                }
            },
            onFocusOut: function (e) {
                if (!e.component.option('value')) {
                    e.component.reset();
                    $(SEL.sgShortcut).val('');
                    updateLotDescription();
                }
            }
        });
    }

    function updateLotDescription() {
        var itemInst = dxInstance(SEL.item);
        var sgInst   = dxInstance(SEL.splitGroup);
        var itemText = itemInst ? (itemInst.option('text') || '') : '';
        var sgText   = sgInst   ? (sgInst.option('text') || '')   : '';
        var parts = [];
        if (itemText) parts.push(itemText);
        if (sgText)   parts.push(sgText);
        $(SEL.lotDesc).val(parts.join(' - '));
    }

    // ── State / County pickers ──────────────────────────────────────────────

    async function loadStateCountiesData() {
        if (!_stateCountiesCache) {
            try { _stateCountiesCache = await $.getJSON('/api/Lookups/StatesWithCounties'); }
            catch (ex) { _stateCountiesCache = []; }
        }
        return _stateCountiesCache;
    }

    async function initStateCountyPickers() {
        var data = await loadStateCountiesData();
        _activeStateCounties = data;

        var states = data.map(function (s) { return { State: s.State, StateName: s.StateName }; });

        $(SEL.state).dxSelectBox({
            dataSource:          states,
            valueExpr:           'State',
            displayExpr:         'StateName',
            searchEnabled:       true,
            searchExpr:          'StateName',
            searchMode:          'contains',
            showDataBeforeSearch: true,
            minSearchLength:     0,
            placeholder:         'Select state\u2026',
            showClearButton:     true,
            onValueChanged: function (e) {
                var countyInst = $(SEL.county).dxSelectBox('instance');
                if (countyInst) {
                    if (!_populating) countyInst.reset();
                    countyInst.option('dataSource', getCountiesForState(e.value));
                    countyInst.option('disabled', !e.value);
                }
            },
        });

        $(SEL.county).dxSelectBox({
            dataSource:          [],
            valueExpr:           'this',
            displayExpr:         'this',
            searchEnabled:       true,
            searchMode:          'contains',
            showDataBeforeSearch: true,
            minSearchLength:     0,
            placeholder:         'Select county\u2026',
            showClearButton:     true,
            disabled:            true,
        });
    }

    function getCountiesForState(stateCode) {
        var source = _activeStateCounties || _stateCountiesCache;
        if (!stateCode || !source) return [];
        for (var i = 0; i < source.length; i++) {
            if (source[i].State === stateCode) return source[i].Counties;
        }
        return [];
    }

    async function loadLocationCounties() {
        if (!currentLocationId) { _locationCountiesData = []; return; }
        if (_locationCountiesLocationId === currentLocationId && _locationCountiesData !== null) return;
        try {
            _locationCountiesData = await $.getJSON('/api/locations/' + currentLocationId + '/Counties');
        } catch (ex) {
            _locationCountiesData = [];
        }
        _locationCountiesLocationId = currentLocationId;
    }

    async function loadLocationItemFilters() {
        if (!currentLocationId) { _locationItemFiltersCache = []; return; }
        if (_locationItemFiltersLocationId === currentLocationId && _locationItemFiltersCache !== null) return;
        try {
            _locationItemFiltersCache = await $.getJSON('/api/locations/' + currentLocationId + '/Items');
        } catch (ex) {
            _locationItemFiltersCache = [];
        }
        _locationItemFiltersLocationId = currentLocationId;
    }

    function getFilteredItems() {
        var allItems = _allItemsCache || [];
        if (!_locationItemFiltersCache || _locationItemFiltersCache.length === 0) return allItems;
        var allowedIds = {};
        _locationItemFiltersCache.forEach(function (f) { allowedIds[f.ItemId] = true; });
        return allItems.filter(function (item) { return allowedIds[item.ItemId]; });
    }

    function applyStateCountyConstraints() {
        var stateInst  = dxInstance(SEL.state);
        var countyInst = $(SEL.county).dxSelectBox('instance');
        if (!stateInst || !countyInst) return;

        if (_locationCountiesData && _locationCountiesData.length > 0) {
            var stateMap = {};
            _locationCountiesData.forEach(function (lc) {
                if (!stateMap[lc.StateAbv]) {
                    stateMap[lc.StateAbv] = { State: lc.StateAbv, StateName: lc.StateName, Counties: [] };
                }
                stateMap[lc.StateAbv].Counties.push(lc.CountyName);
            });
            _activeStateCounties = Object.values(stateMap);
        } else {
            _activeStateCounties = _stateCountiesCache || [];
        }

        var states = _activeStateCounties.map(function (s) { return { State: s.State, StateName: s.StateName }; });
        stateInst.option('dataSource', states);
        stateInst.reset();
        countyInst.reset();
        countyInst.option('dataSource', []);
        countyInst.option('disabled', true);

        if (_locationCountiesData && _locationCountiesData.length === 1) {
            var lc = _locationCountiesData[0];
            stateInst.option('value', lc.StateAbv);
            setTimeout(function () { countyInst.option('value', lc.CountyName); }, 0);
        }
    }

    // ── Wire create-lot buttons ──────────────────────────────────────────────

    function wireCreateLotButtons() {
        // New Lot button
        $(SEL.newLotBtn).dxButton({
            text: 'New Lot',
            icon: 'add',
            stylingMode: 'outlined',
            type: 'default',
            onClick: async function () {
                $(SEL.lotListPanel).prop('hidden', true);
                $(SEL.createLotPanel).prop('hidden', false);
                await resetCreateLotForm();
            }
        });

        // Cancel create lot
        $(SEL.cancelCreateLotBtn).on('click', function () {
            $(SEL.createLotPanel).prop('hidden', true);
            $(SEL.lotListPanel).prop('hidden', false);
        });

        // Save lot
        $(SEL.saveLotBtn).on('click', async function () {
            await saveCreateLot();
        });

        // SG# shortcut
        $(SEL.sgShortcut).on('keydown', function (e) {
            if (e.key === 'Enter') { e.preventDefault(); applySgShortcut(); }
        }).on('blur', function () {
            if ($(SEL.sgShortcut).val().trim()) applySgShortcut();
        });
    }

    async function applySgShortcut() {
        var raw = $(SEL.sgShortcut).val().trim();
        $(SEL.sgError).prop('hidden', true).text('');
        if (!raw) return;

        var sgId = parseInt(raw, 10);
        if (!sgId || sgId <= 0) {
            $(SEL.sgError).text('Invalid split group number.').prop('hidden', false);
            return;
        }

        try {
            var sg = await $.getJSON('/api/GrowerDelivery/SplitGroupLookup?splitGroupId=' + sgId);
            _populating = true;
            try {
                var acctInst = dxInstance(SEL.account);
                if (acctInst) {
                    acctInst.option('disabled', false);
                    acctInst.option('value', sg.PrimaryAccountId);
                }
                var groups = [];
                try { groups = await $.getJSON('/api/GrowerDelivery/SplitGroupsByAccount?accountId=' + sg.PrimaryAccountId); }
                catch (ex) { /* ignore */ }
                var sgInst = dxInstance(SEL.splitGroup);
                if (sgInst) {
                    sgInst.option('dataSource', groups);
                    sgInst.option('disabled', false);
                    sgInst.option('value', sg.SplitGroupId);
                }
                $(SEL.splitGroupWarn).prop('hidden', groups.length > 0);
                updateLotDescription();
            } finally {
                _populating = false;
            }
        } catch (ex) {
            if (ex.status === 404) {
                $(SEL.sgError).text('Split group ' + sgId + ' not found.').prop('hidden', false);
            } else {
                $(SEL.sgError).text('Lookup failed.').prop('hidden', false);
            }
        }
    }

    async function resetCreateLotForm() {
        var itemInst = dxInstance(SEL.item);
        var acctInst = dxInstance(SEL.account);
        var sgInst   = dxInstance(SEL.splitGroup);

        await loadLocationItemFilters();
        var itemSource = getFilteredItems();
        if (itemInst) {
            itemInst.option('dataSource', itemSource);
            itemInst.option('disabled', false);
            itemInst.reset();
        }
        if (acctInst) { acctInst.reset(); acctInst.option('disabled', true); }
        if (sgInst)   { sgInst.reset(); sgInst.option('dataSource', []); sgInst.option('disabled', true); }

        $(SEL.lotDesc).val('');
        $(SEL.landlord).val('');
        $(SEL.notes).val('');
        $(SEL.sgShortcut).val('').prop('disabled', true);
        $(SEL.sgError).prop('hidden', true).text('');
        $(SEL.splitGroupWarn).prop('hidden', true);
        $(SEL.createLotError).prop('hidden', true).text('');

        await loadLocationCounties();
        applyStateCountyConstraints();

        // Auto-cascade if only one item
        if (itemSource.length === 1 && itemInst) {
            _populating = true;
            try {
                itemInst.option('value', itemSource[0].ItemId);
                $(SEL.sgShortcut).prop('disabled', false);
                updateLotDescription();

                var accounts = _accountsCache || [];
                if (currentLocationId) {
                    try {
                        accounts = await $.getJSON('/api/Lookups/ProducerAccountsForItem?itemId=' + itemSource[0].ItemId + '&locationId=' + currentLocationId);
                    } catch (ex) { /* ignore */ }
                }
                if (acctInst) {
                    acctInst.option('dataSource', accounts);
                    acctInst.option('disabled', false);
                }
                if (accounts.length === 1 && acctInst) {
                    acctInst.option('value', accounts[0].AccountId);
                    await loadSplitGroups(accounts[0].AccountId);
                }
            } finally {
                _populating = false;
            }
        }
    }

    async function saveCreateLot() {
        var splitGroupId = parseInt($(SEL.splitGroup).dxSelectBox('option', 'value'), 10) || null;

        if (!currentLocationId) {
            $(SEL.createLotError).text('No location selected.').prop('hidden', false);
            return;
        }
        if (!splitGroupId) {
            $(SEL.createLotError).text('Please select a split group.').prop('hidden', false);
            return;
        }
        if (!$(SEL.state).dxSelectBox('option', 'value')) {
            $(SEL.createLotError).text('Please select a state.').prop('hidden', false);
            return;
        }
        if (!$(SEL.county).dxSelectBox('option', 'value')) {
            $(SEL.createLotError).text('Please select a county.').prop('hidden', false);
            return;
        }

        var btn = $(SEL.saveLotBtn);
        btn.prop('disabled', true).text('Creating\u2026');
        $(SEL.createLotError).prop('hidden', true);

        try {
            var resp = await fetch('/api/GrowerDelivery/WeightSheetLots', {
                method:  'POST',
                headers: { 'Content-Type': 'application/json' },
                body:    JSON.stringify({
                    LocationId:   currentLocationId,
                    SplitGroupId: splitGroupId,
                    ItemId:       $(SEL.item).dxSelectBox('option', 'value') || null,
                    Notes:        $(SEL.notes).val().trim() || null,
                    State:        $(SEL.state).dxSelectBox('option', 'value') || null,
                    County:       $(SEL.county).dxSelectBox('option', 'value') || null,
                    Landlord:     $(SEL.landlord).val().trim() || null,
                }),
            });

            if (!resp.ok) {
                var detail = await tryParseError(resp);
                $(SEL.createLotError).text('Error: ' + detail).prop('hidden', false);
                return;
            }

            // Switch back to lot list and refresh
            $(SEL.createLotPanel).prop('hidden', true);
            $(SEL.lotListPanel).prop('hidden', false);
            refreshLots();
            showAlert('Weight sheet lot created.', 'success');

        } catch (ex) {
            $(SEL.createLotError).text('Network error: ' + ex.message).prop('hidden', false);
        } finally {
            btn.prop('disabled', false).text('Create Lot');
        }
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
            placeholder: 'Enter miles\u2026',
            onValueChanged: async function (e) {
                _milesEntered = (e.value !== null && e.value !== undefined);
                if (!_milesEntered) { $(SEL.calcRate).val(''); $(SEL.calcRateGroup).prop('hidden', true); return; }
                var bolTypeInst = dxInstance(SEL.bolType);
                var rt = bolTypeInst ? bolTypeInst.option('value') : 'A';
                try {
                    var data = await $.getJSON('/api/Lookups/HaulerRateForMiles?rateType=' + rt + '&miles=' + e.value);
                    $(SEL.calcRate).val('$' + data.Rate.toFixed(2) + ' (up to ' + data.MaxDistance + ' mi)');
                    $(SEL.calcRateGroup).prop('hidden', false);
                } catch (ex) {
                    $(SEL.calcRate).val('No rate found for this mileage');
                    $(SEL.calcRateGroup).prop('hidden', false);
                }
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
            placeholder: 'Rate amount\u2026',
        });

        // Create Weight Sheet button
        $(SEL.createWsBtn).on('click', function () {
            createWeightSheet();
        });
    }

    async function onBolTypeChanged(val) {
        // Hide everything first
        $(SEL.haulerMilesDetails).prop('hidden', true);
        $(SEL.customDetails).prop('hidden', true);
        $(SEL.calcRateGroup).prop('hidden', true);
        $(SEL.haulerError).prop('hidden', true).text('');
        $(SEL.bolTypeHint).prop('hidden', true);
        $(SEL.createWsBtn).prop('disabled', true);

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
        $(SEL.calcRate).val('');
        $(SEL.calcRateGroup).prop('hidden', true);
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
            showStep3(result.WeightSheetId);

        } catch (ex) {
            $(SEL.haulerError).text('Network error: ' + ex.message).prop('hidden', false);
        } finally {
            btn.prop('disabled', false);
        }
    }

})();
