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
        alert:           '#wslAlert',
        createPanel:     '#wslCreatePanel',
        panelTitle:      '#wslPanelTitle',
        account:         '#wslAccount',
        splitGroup:      '#wslSplitGroup',
        item:            '#wslItem',
        state:           '#wslState',
        county:          '#wslCounty',
        lotDesc:         '#wslLotDesc',
        landlord:        '#wslLandlord',
        farmNumber:      '#wslFarmNumber',
        notes:           '#wslNotes',
        createError:     '#wslCreateError',
        saveBtn:         '#wslSaveBtn',
        cancelBtn:       '#wslCancelBtn',
        splitGroupWarn:  '#wslSplitGroupWarning',
        sgShortcut:      '#wslSgShortcut',
        sgError:         '#wslSgError',
        sgSearchBtn:     '#wslSgSearchBtn',
        sgPickerPopup:   '#wslSgPickerPopup',
    };

    // ── State ────────────────────────────────────────────────────────────────

    let currentLocationId   = null;
    let _accountsCache      = null;
    let _allItemsCache      = null;
    let _stateCountiesCache = null;

    let _locationCountiesData       = null;
    let _locationCountiesLocationId = null;
    let _activeStateCounties        = null;

    let _locationItemFiltersCache       = null;
    let _locationItemFiltersLocationId  = null;

    let _editingLotId    = null;
    let _editingFullEdit = false;
    let _editingOriginal = null;

    let _populating = false;

    let _sgOverrideActive     = false;
    let _sgOverrideSplitGroup = null;

    let _lastSgLookup = '';

    // Deep-link params
    let _returnTo = null;
    let _wsId     = null;
    let _lotType  = null; // 'seed' or 'warehouse' — for new lot mode
    let _prePin   = null; // PIN passed via URL (already collected)

    // ── Init ─────────────────────────────────────────────────────────────────

    $(function () {
        var urlParams = new URLSearchParams(window.location.search);
        var lotId     = parseInt(urlParams.get('lotId'), 10) || null;
        _returnTo     = urlParams.get('returnTo') || null;
        _wsId         = parseInt(urlParams.get('wsId'), 10) || null;
        _lotType      = (urlParams.get('lotType') || '').toLowerCase() || null;
        _prePin       = parseInt(urlParams.get('pin'), 10) || null;

        // Update back bar based on returnTo
        if (_returnTo === 'deliveryLoads' && _wsId) {
            var wsUrl = '/GrowerDelivery/WeightSheetDeliveryLoads?wsId=' + _wsId;
            var $bar = $('#editLotBackBar');
            $bar.attr('href', wsUrl).attr('title', 'Back to Weight Sheet');
            $bar.find('.gm-module-bar__label').text('BACK TO WEIGHT SHEET');
        } else if (_returnTo === 'lots') {
            var $bar = $('#editLotBackBar');
            $bar.attr('href', '/GrowerDelivery/WeightSheetLots').attr('title', 'Back to Lots');
            $bar.find('.gm-module-bar__label').text('LOTS');
        }

        initItemPicker();
        initAccountPicker();
        initSplitGroupPicker();
        initStateCountyPickers();
        wireButtons();

        initLocationThenLoad(lotId);
    });

    async function initLocationThenLoad(lotId) {
        try {
            var resp = await fetch('/api/LocationContextApi/current');
            var current = await resp.json();
            if (current.HasLocation && current.LocationId) {
                currentLocationId = current.LocationId;
            }
        } catch (ex) {
            console.warn('[EditLot] Could not read current location', ex);
        }

        if (lotId) {
            await loadAndEditLot(lotId);
        } else {
            // New lot mode
            var typeLabel = _lotType === 'warehouse' ? 'Warehouse' : _lotType === 'seed' ? 'Seed' : '';
            $(SEL.panelTitle).text('New ' + typeLabel + ' Lot');
            $(SEL.saveBtn).text('Create Lot');
            await resetCreateForm();
        }
    }

    async function loadAndEditLot(lotId) {
        if (!currentLocationId) {
            showAlert('No location selected.', 'danger');
            return;
        }

        try {
            var lots = await $.getJSON('/api/GrowerDelivery/WeightSheetLots?locationId=' + currentLocationId + '&status=all');
            var lot = null;
            for (var i = 0; i < lots.length; i++) {
                if (lots[i].LotId === lotId) { lot = lots[i]; break; }
            }
            if (!lot) {
                showAlert('Lot not found.', 'danger');
                return;
            }

            var isOpen = lot.IsOpen;
            var hasClosedWs = lot.HasClosedWeightSheet;
            var createdToday = false;
            if (lot.CreatedAt) {
                var parts = lot.CreatedAt.split('/');
                if (parts.length === 3) {
                    var now = new Date();
                    var todayStr = String(now.getMonth() + 1).padStart(2, '0') + '/' +
                                   String(now.getDate()).padStart(2, '0') + '/' + now.getFullYear();
                    createdToday = (lot.CreatedAt === todayStr);
                }
            }
            var canFullEdit = (isOpen && !hasClosedWs) || createdToday;

            await enterEditMode(lot, canFullEdit);
        } catch (ex) {
            showAlert('Error loading lot: ' + ex.message, 'danger');
        }
    }

    // ── Edit mode ────────────────────────────────────────────────────────────

    async function enterEditMode(lot, fullEdit) {
        _editingLotId    = lot.LotId;
        _editingFullEdit = fullEdit;
        _editingOriginal = {
            Notes:          lot.Notes || null,
            LotDescription: lot.LotDescription || null,
            ItemId:         lot.ItemId || null,
            State:          lot.State || null,
            County:         lot.County || null,
            Landlord:       lot.Landlord || null,
            FarmNumber:     lot.FarmNumber || null,
        };

        $(SEL.panelTitle).text('Edit Lot ' + formatLotId(lot.LotId));
        $(SEL.saveBtn).text('Save');

        $(SEL.createError).prop('hidden', true).text('');
        $(SEL.splitGroupWarn).prop('hidden', true);
        $(SEL.sgError).prop('hidden', true).text('');

        _populating = true;
        try {
            await populateEditForm(lot, fullEdit);
        } finally {
            _populating = false;
        }
    }

    // ── Account cache ────────────────────────────────────────────────────────

    async function ensureAccountsCacheLoaded() {
        if (_accountsCache) return;
        try { _accountsCache = await $.getJSON('/api/Lookups/ProducerAccounts'); }
        catch (ex) {
            console.warn('[EditLot] Account cache load failed', ex);
            _accountsCache = [];
        }
    }

    function isAccountKnown(accountId) {
        if (!accountId || !_accountsCache) return false;
        for (var i = 0; i < _accountsCache.length; i++) {
            if (_accountsCache[i].AccountId === accountId) return true;
        }
        return false;
    }

    function clearAccountAndSplitGroup() {
        _sgOverrideActive     = false;
        _sgOverrideSplitGroup = null;
        _populating = true;
        try {
            var acctInst = dxInstance(SEL.account);
            if (acctInst) {
                acctInst.option('dataSource', _accountsCache || []);
                acctInst.option('value', null);
            }
            var sgInst = dxInstance(SEL.splitGroup);
            if (sgInst) {
                sgInst.option('dataSource', []);
                sgInst.option('value', null);
                sgInst.option('disabled', true);
            }
            $(SEL.lotDesc).val('');
            $(SEL.splitGroupWarn).prop('hidden', true);
            $(SEL.sgError).prop('hidden', true).text('');
        } finally {
            _populating = false;
        }
    }

    function isSplitGroupInList(groups, splitGroupId) {
        if (!splitGroupId || !groups) return false;
        for (var i = 0; i < groups.length; i++) {
            if (groups[i].SplitGroupId === splitGroupId) return true;
        }
        return false;
    }

    // ── Populate edit form ───────────────────────────────────────────────────

    async function populateEditForm(lot, fullEdit) {
        await ensureAccountsCacheLoaded();

        $(SEL.sgShortcut).val(lot.SplitGroupId || '').prop('disabled', !fullEdit);
        _lastSgLookup = lot.SplitGroupId ? String(lot.SplitGroupId) : '';

        if (fullEdit) {
            await loadLocationItemFilters();
            var editItems = getFilteredItems();
            if (lot.AccountId) {
                try {
                    var filters = await $.getJSON('/api/AccountItemFilters?accountId=' + lot.AccountId);
                    if (filters.length > 0) {
                        var allowedIds = {};
                        for (var i = 0; i < filters.length; i++) { allowedIds[filters[i].ItemId] = true; }
                        editItems = editItems.filter(function (it) { return allowedIds[it.ItemId]; });
                    }
                } catch (ex) { console.warn('[EditLot] AccountItemFilters load failed', ex); }
            }

            var itemInst = dxInstance(SEL.item);
            if (itemInst) {
                itemInst.option('dataSource', editItems);
                itemInst.option('disabled', false);
                itemInst.option('value', lot.ItemId || null);
            }

            var acctInst = dxInstance(SEL.account);
            var sgInst   = dxInstance(SEL.splitGroup);

            var sgLookup = null;
            if (lot.SplitGroupId) {
                try { sgLookup = await $.getJSON('/api/GrowerDelivery/SplitGroupLookup?splitGroupId=' + lot.SplitGroupId); }
                catch (ex) { }
            }
            var acctKnown = isAccountKnown(lot.AccountId);

            if (sgLookup && sgLookup.PrimaryAccountId) {
                _sgOverrideActive     = false;
                _sgOverrideSplitGroup = null;
                if (acctInst) {
                    acctInst.option('dataSource', _accountsCache || []);
                    acctInst.option('disabled', false);
                    acctInst.option('value', acctKnown ? lot.AccountId : null);
                }
                var groups = [];
                try { groups = await $.getJSON('/api/GrowerDelivery/SplitGroupsByAccount?accountId=' + sgLookup.PrimaryAccountId); }
                catch (ex) { console.warn('[EditLot] SplitGroup load failed', ex); }
                var sgKnown = isSplitGroupInList(groups, lot.SplitGroupId);
                if (sgInst) {
                    sgInst.option('dataSource', groups);
                    sgInst.option('disabled', false);
                    sgInst.option('value', sgKnown ? lot.SplitGroupId : null);
                }
                if (!acctKnown || !sgKnown) {
                    $(SEL.splitGroupWarn)
                        .text('Original account or split group is no longer available. Please re-select.')
                        .prop('hidden', false);
                }
            } else if (sgLookup) {
                _sgOverrideActive     = true;
                _sgOverrideSplitGroup = {
                    SplitGroupId:          sgLookup.SplitGroupId,
                    SplitGroupDescription: sgLookup.SplitGroupDescription,
                };
                if (sgInst) {
                    sgInst.option('dataSource', [_sgOverrideSplitGroup]);
                    sgInst.option('value', sgLookup.SplitGroupId);
                    sgInst.option('disabled', true);
                }
                if (acctInst) {
                    acctInst.option('dataSource', _accountsCache || []);
                    acctInst.option('disabled', false);
                    acctInst.option('value', acctKnown ? lot.AccountId : null);
                }
                if (!acctKnown) {
                    $(SEL.splitGroupWarn)
                        .text('This split group has no primary account. Please select any active producer account.')
                        .prop('hidden', false);
                }
            } else {
                _sgOverrideActive     = false;
                _sgOverrideSplitGroup = null;
                if (acctInst) {
                    acctInst.option('dataSource', _accountsCache || []);
                    acctInst.option('disabled', false);
                    acctInst.option('value', acctKnown ? lot.AccountId : null);
                }
                if (sgInst) {
                    sgInst.option('dataSource', []);
                    sgInst.option('value', null);
                    sgInst.option('disabled', false);
                }
                $(SEL.splitGroupWarn)
                    .text('Original account or split group is no longer available. Please re-select.')
                    .prop('hidden', false);
            }

            await loadLocationCounties();

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

            var stateInst  = dxInstance(SEL.state);
            var countyInst = dxInstance(SEL.county);
            var states = _activeStateCounties.map(function (s) { return { State: s.State, StateName: s.StateName }; });

            if (stateInst) {
                stateInst.option('dataSource', states);
                stateInst.option('disabled', false);
                stateInst.option('value', lot.State || null);
            }
            if (countyInst) {
                countyInst.option('dataSource', getCountiesForState(lot.State));
                countyInst.option('disabled', !lot.State);
                countyInst.option('value', lot.County || null);
            }

            $(SEL.lotDesc).val(lot.LotDescription || '');
            $(SEL.landlord).val(lot.Landlord || '').prop('disabled', false);
            $(SEL.farmNumber).val(lot.FarmNumber || '').prop('disabled', false);
            $(SEL.notes).val(lot.Notes || '');

        } else {
            // Notes-only edit
            var itemInst2 = dxInstance(SEL.item);
            if (itemInst2) {
                itemInst2.option('value', lot.ItemId || null);
                itemInst2.option('disabled', true);
            }

            var acctInst2  = dxInstance(SEL.account);
            var sgInst2    = dxInstance(SEL.splitGroup);
            var acctKnown2 = isAccountKnown(lot.AccountId);

            var sgLookup2 = null;
            if (lot.SplitGroupId) {
                try { sgLookup2 = await $.getJSON('/api/GrowerDelivery/SplitGroupLookup?splitGroupId=' + lot.SplitGroupId); }
                catch (ex) { }
            }

            if (sgLookup2 && sgLookup2.PrimaryAccountId) {
                var groups2 = [];
                try { groups2 = await $.getJSON('/api/GrowerDelivery/SplitGroupsByAccount?accountId=' + sgLookup2.PrimaryAccountId); }
                catch (ex) { }
                var sgKnown2 = isSplitGroupInList(groups2, lot.SplitGroupId);
                if (acctInst2) {
                    acctInst2.option('value', acctKnown2 ? lot.AccountId : null);
                    acctInst2.option('disabled', true);
                }
                if (sgInst2) {
                    sgInst2.option('dataSource', groups2);
                    sgInst2.option('value', sgKnown2 ? lot.SplitGroupId : null);
                    sgInst2.option('disabled', true);
                }
                if (!acctKnown2 || !sgKnown2) {
                    $(SEL.splitGroupWarn)
                        .text('Original account or split group is no longer available.')
                        .prop('hidden', false);
                }
            } else if (sgLookup2) {
                if (sgInst2) {
                    sgInst2.option('dataSource', [{
                        SplitGroupId:          sgLookup2.SplitGroupId,
                        SplitGroupDescription: sgLookup2.SplitGroupDescription,
                    }]);
                    sgInst2.option('value', sgLookup2.SplitGroupId);
                    sgInst2.option('disabled', true);
                }
                if (acctInst2) {
                    acctInst2.option('value', acctKnown2 ? lot.AccountId : null);
                    acctInst2.option('disabled', true);
                }
            } else {
                if (acctInst2) {
                    acctInst2.option('value', acctKnown2 ? lot.AccountId : null);
                    acctInst2.option('disabled', true);
                }
                if (sgInst2) {
                    sgInst2.option('dataSource', []);
                    sgInst2.option('value', null);
                    sgInst2.option('disabled', true);
                }
                $(SEL.splitGroupWarn)
                    .text('Original account or split group is no longer available.')
                    .prop('hidden', false);
            }

            await loadStateCountiesData();
            var stateInst2  = dxInstance(SEL.state);
            var countyInst2 = dxInstance(SEL.county);
            if (stateInst2) {
                var states2 = (_stateCountiesCache || []).map(function (s) { return { State: s.State, StateName: s.StateName }; });
                stateInst2.option('dataSource', states2);
                stateInst2.option('value', lot.State || null);
                stateInst2.option('disabled', true);
            }
            if (countyInst2) {
                countyInst2.option('dataSource', getCountiesForState(lot.State));
                countyInst2.option('value', lot.County || null);
                countyInst2.option('disabled', true);
            }

            $(SEL.lotDesc).val(lot.LotDescription || '');
            $(SEL.landlord).val(lot.Landlord || '').prop('disabled', true);
            $(SEL.farmNumber).val(lot.FarmNumber || '').prop('disabled', true);
            $(SEL.notes).val(lot.Notes || '');
        }
    }

    // ── Pickers ──────────────────────────────────────────────────────────────

    async function initAccountPicker() {
        if (!_accountsCache) {
            try { _accountsCache = await $.getJSON('/api/Lookups/ProducerAccounts'); }
            catch (ex) {
                console.warn('[EditLot] Account load failed', ex);
                _accountsCache = [];
            }
            (_accountsCache || []).sort(function (a, b) {
                return (a.Name || '').localeCompare(b.Name || '', undefined, { sensitivity: 'base' });
            });
        }

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
                if (_sgOverrideActive) {
                    if (e.value) $(SEL.splitGroupWarn).prop('hidden', true);
                    updateLotDescription();
                    return;
                }
                const sgInst = $(SEL.splitGroup).dxSelectBox('instance');
                if (sgInst) { sgInst.reset(); sgInst.option('dataSource', []); sgInst.option('disabled', true); }
                $(SEL.lotDesc).val('');
                $(SEL.splitGroupWarn).prop('hidden', true);
                if (e.value) loadSplitGroups(e.value);
            },
            onFocusOut: function (e) {
                if (!e.component.option('value')) {
                    e.component.reset();
                    if (_sgOverrideActive) return;
                    const sgInst = $(SEL.splitGroup).dxSelectBox('instance');
                    if (sgInst) { sgInst.reset(); sgInst.option('dataSource', []); sgInst.option('disabled', true); }
                    $(SEL.lotDesc).val('');
                    $(SEL.splitGroupWarn).prop('hidden', true);
                }
            }
        });
    }

    async function loadSplitGroups(accountId) {
        setPickerLoading(SEL.splitGroup, true);
        let groups = [];
        try { groups = await $.getJSON('/api/GrowerDelivery/SplitGroupsByAccount?accountId=' + accountId); }
        catch (ex) { console.warn('[EditLot] SplitGroup load failed', ex); }
        const sgInst = dxInstance(SEL.splitGroup);
        if (sgInst) {
            sgInst.option('dataSource', groups);
            sgInst.option('disabled', groups.length === 0);
            sgInst.option('placeholder', 'Select split group\u2026');
            if (groups.length === 1) sgInst.option('value', groups[0].SplitGroupId);
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
        var itemInst = $(SEL.item).dxSelectBox('instance');
        var sgInst   = $(SEL.splitGroup).dxSelectBox('instance');
        var itemText = itemInst ? (itemInst.option('text') || '') : '';
        var sgText   = sgInst   ? (sgInst.option('text') || '')   : '';
        var parts = [];
        if (itemText) parts.push(itemText);
        if (sgText)   parts.push(sgText);
        $(SEL.lotDesc).val(parts.join(' - '));
    }

    function setPickerLoading(sel, loading) {
        var inst = dxInstance(sel);
        if (!inst) return;
        if (loading) {
            inst.option('disabled', true);
            inst.option('placeholder', 'Loading\u2026');
        } else {
            inst.option('disabled', false);
            inst.option('placeholder', sel === SEL.account ? 'Select account\u2026'
                : sel === SEL.splitGroup ? 'Select split group\u2026' : 'Select\u2026');
        }
    }

    async function initItemPicker() {
        if (!_allItemsCache) {
            var itemsUrl = _lotType === 'seed' ? '/api/Lookups/SeedItems' : '/api/Lookups/WarehouseItems';
            try { _allItemsCache = await $.getJSON(itemsUrl); }
            catch (ex) {
                console.warn('[EditLot] Items load failed', ex);
                _allItemsCache = [];
            }
            (_allItemsCache || []).sort(function (a, b) {
                return (a.Name || '').localeCompare(b.Name || '', undefined, { sensitivity: 'base' });
            });
        }

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
                var acctInst = $(SEL.account).dxSelectBox('instance');
                var sgInst   = $(SEL.splitGroup).dxSelectBox('instance');
                if (e.value) {
                    var accounts = _accountsCache || [];
                    if (acctInst) {
                        acctInst.reset();
                        setPickerLoading(SEL.account, true);
                        if (currentLocationId) {
                            try {
                                accounts = await $.getJSON('/api/Lookups/ProducerAccountsForItem?itemId=' + e.value + '&locationId=' + currentLocationId);
                            } catch (ex) {
                                console.warn('[EditLot] Filtered accounts load failed', ex);
                            }
                        }
                        acctInst.option('dataSource', accounts);
                        setPickerLoading(SEL.account, false);
                        if (accounts.length === 1) acctInst.option('value', accounts[0].AccountId);
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

    // ── State / County pickers ──────────────────────────────────────────────

    async function loadStateCountiesData() {
        if (!_stateCountiesCache) {
            try { _stateCountiesCache = await $.getJSON('/api/Lookups/StatesWithCounties'); }
            catch (ex) {
                console.warn('[EditLot] State/Counties load failed', ex);
                _stateCountiesCache = [];
            }
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

    function getCountiesForState(stateCode, source) {
        source = source || _activeStateCounties || _stateCountiesCache;
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
            console.warn('[EditLot] LocationCounties load failed', ex);
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
            console.warn('[EditLot] LocationItemFilters load failed', ex);
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
        var countyInst = dxInstance(SEL.county);
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

    // ── Buttons ──────────────────────────────────────────────────────────────

    function wireButtons() {
        $(SEL.cancelBtn).on('click', function () {
            navigateBack();
        });

        $(SEL.saveBtn).on('click', async function () {
            if (_editingLotId) {
                await saveEdit();
            } else {
                await saveCreate();
            }
        });

        function maybeApplySgShortcut() {
            var cur = ($(SEL.sgShortcut).val() || '').trim();
            if (!cur || cur === _lastSgLookup) return;
            _lastSgLookup = cur;
            applySgShortcut();
        }
        $(SEL.sgShortcut).on('keydown', function (e) {
            if (e.key === 'Enter') { e.preventDefault(); maybeApplySgShortcut(); }
        }).on('blur', function () {
            maybeApplySgShortcut();
        }).on('input', function () {
            _lastSgLookup = '';
            if (!($(SEL.sgShortcut).val() || '').trim()) {
                clearAccountAndSplitGroup();
            }
        });

        $(SEL.sgSearchBtn).on('click', function () {
            openSplitGroupPicker();
        });
    }

    function navigateBack() {
        if (_returnTo === 'deliveryLoads' && _wsId) {
            window.location.href = '/GrowerDelivery/WeightSheetDeliveryLoads?wsId=' + _wsId;
        } else {
            window.location.href = '/GrowerDelivery/WeightSheetLots';
        }
    }

    // Use pre-collected PIN if available, otherwise prompt
    async function getPin() {
        if (_prePin) {
            var pin = _prePin;
            _prePin = null; // only use once
            return pin;
        }
        return await promptForPin();
    }

    // ── Split group search popup ─────────────────────────────────────────────

    let _allSplitGroupsCache = null;
    let _sgDetailCache       = {};
    let _sgPopupInitialized  = false;

    async function openSplitGroupPicker() {
        if (!_sgPopupInitialized) {
            initSplitGroupPickerPopup();
            _sgPopupInitialized = true;
        }
        if (!_allSplitGroupsCache) {
            try { _allSplitGroupsCache = await $.getJSON('/api/SplitGroups'); }
            catch (ex) {
                console.warn('[EditLot] AllSplitGroups load failed', ex);
                _allSplitGroupsCache = [];
            }
            var grid = $(SEL.sgPickerPopup + ' .gm-wsl-sg-grid').dxDataGrid('instance');
            if (grid) grid.option('dataSource', _allSplitGroupsCache);
        }
        $(SEL.sgPickerPopup).dxPopup('instance').show();
    }

    function initSplitGroupPickerPopup() {
        var $popupContent = $(
            '<div class="gm-wsl-sg-picker">' +
              '<div class="gm-wsl-sg-grid"></div>' +
            '</div>'
        );

        $(SEL.sgPickerPopup).dxPopup({
            title:         'Find Split Group',
            visible:       false,
            showCloseButton: true,
            width:         '80%',
            height:        '80%',
            dragEnabled:   true,
            contentTemplate: function (contentElement) {
                contentElement.append($popupContent);
                $popupContent.find('.gm-wsl-sg-grid').dxDataGrid({
                    dataSource:      _allSplitGroupsCache || [],
                    keyExpr:         'SplitGroupId',
                    showBorders:     true,
                    showRowLines:    true,
                    rowAlternationEnabled: true,
                    columnAutoWidth: true,
                    height:          '100%',
                    searchPanel:     { visible: true, placeholder: 'Search any field\u2026', width: 300 },
                    headerFilter:    { visible: true },
                    filterRow:       { visible: true },
                    paging:          { pageSize: 25 },
                    pager:           { visible: true, showPageSizeSelector: true, allowedPageSizes: [10, 25, 50, 100], showInfo: true },
                    hoverStateEnabled: true,
                    noDataText:      'No split groups available.',
                    onRowPrepared: function (e) {
                        if (e.rowType === 'data' && e.data && e.data.PrimaryAccountId == null) {
                            $(e.rowElement).addClass('gm-sg-unassigned');
                        }
                    },
                    columns: [
                        { dataField: 'SplitGroupId', caption: 'SG #', width: 90, alignment: 'left' },
                        { dataField: 'SplitGroupDescription', caption: 'Description' },
                        { dataField: 'PrimaryAccountAs400Id', caption: 'Primary Acct #', width: 130, alignment: 'left' },
                        {
                            dataField: 'PrimaryAccountName',
                            caption: 'Primary Account',
                            calculateCellValue: function (rowData) {
                                return (rowData && rowData.PrimaryAccountName) || 'Unassigned';
                            },
                        },
                    ],
                    masterDetail: {
                        enabled: true,
                        template: function (detailElement, detailInfo) {
                            var splitGroupId = detailInfo.data.SplitGroupId;
                            var $detail = $('<div class="gm-wsl-sg-detail"></div>');
                            detailElement.append($detail);
                            $detail.dxDataGrid({
                                dataSource:      [],
                                showBorders:     true,
                                columnAutoWidth: true,
                                noDataText:      'No grower splits configured.',
                                columns: [
                                    { dataField: 'As400AccountId', caption: 'Acct #',  width: 110, alignment: 'left' },
                                    { dataField: 'AccountName',    caption: 'Grower' },
                                    {
                                        dataField: 'SplitPercent',
                                        caption:   'Split %',
                                        width:     110,
                                        alignment: 'right',
                                        format:    { type: 'fixedPoint', precision: 4 },
                                    },
                                ],
                                summary: {
                                    totalItems: [{
                                        column: 'SplitPercent',
                                        summaryType: 'sum',
                                        displayFormat: 'Total: {0}',
                                        valueFormat: { type: 'fixedPoint', precision: 4 },
                                    }],
                                },
                            });
                            var detailInst = $detail.dxDataGrid('instance');
                            if (_sgDetailCache[splitGroupId]) {
                                detailInst.option('dataSource', _sgDetailCache[splitGroupId]);
                            } else {
                                $.getJSON('/api/SplitGroups/' + splitGroupId + '/Percents')
                                    .done(function (rows) {
                                        _sgDetailCache[splitGroupId] = rows || [];
                                        detailInst.option('dataSource', _sgDetailCache[splitGroupId]);
                                    })
                                    .fail(function (ex) {
                                        console.warn('[EditLot] SplitGroupPercents load failed', ex);
                                    });
                            }
                        },
                    },
                    onRowDblClick: function (e) {
                        if (e.rowType === 'data' && e.data) applySplitGroupPickerSelection(e.data);
                    },
                    selection: { mode: 'single' },
                    onToolbarPreparing: function (e) {
                        e.toolbarOptions.items.unshift({
                            location: 'after',
                            widget:   'dxButton',
                            options: {
                                text: 'Select', icon: 'check', stylingMode: 'contained', type: 'default',
                                onClick: function () {
                                    var selected = e.component.getSelectedRowsData();
                                    if (selected && selected.length) applySplitGroupPickerSelection(selected[0]);
                                },
                            },
                        });
                    },
                });
            },
        });
    }

    function applySplitGroupPickerSelection(row) {
        if (!row || !row.SplitGroupId) return;
        $(SEL.sgShortcut).val(row.SplitGroupId);
        applySgShortcut();
        var popup = $(SEL.sgPickerPopup).dxPopup('instance');
        if (popup) popup.hide();
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

        _lastSgLookup = String(sgId);

        try {
            var sg = await $.getJSON('/api/GrowerDelivery/SplitGroupLookup?splitGroupId=' + sgId);

            _populating = true;
            try {
                var acctInst = dxInstance(SEL.account);
                var sgInst   = dxInstance(SEL.splitGroup);

                if (sg.PrimaryAccountId) {
                    _sgOverrideActive     = false;
                    _sgOverrideSplitGroup = null;
                    $(SEL.splitGroupWarn).text('No split group set up for this account.');
                    if (acctInst) {
                        acctInst.option('disabled', false);
                        acctInst.option('value', sg.PrimaryAccountId);
                    }
                    var groups = [];
                    try { groups = await $.getJSON('/api/GrowerDelivery/SplitGroupsByAccount?accountId=' + sg.PrimaryAccountId); }
                    catch (ex) { console.warn('[EditLot] SplitGroup load failed', ex); }
                    if (sgInst) {
                        sgInst.option('dataSource', groups);
                        sgInst.option('disabled', false);
                        sgInst.option('value', sg.SplitGroupId);
                    }
                    $(SEL.splitGroupWarn).prop('hidden', groups.length > 0);
                } else {
                    _sgOverrideActive     = true;
                    _sgOverrideSplitGroup = {
                        SplitGroupId:          sg.SplitGroupId,
                        SplitGroupDescription: sg.SplitGroupDescription,
                    };
                    if (sgInst) {
                        sgInst.option('dataSource', [_sgOverrideSplitGroup]);
                        sgInst.option('value', sg.SplitGroupId);
                        sgInst.option('disabled', true);
                    }
                    if (acctInst) {
                        acctInst.option('dataSource', _accountsCache || []);
                        acctInst.option('disabled', false);
                        acctInst.option('value', null);
                    }
                    $(SEL.splitGroupWarn)
                        .text('This split group has no primary account. Please select any active producer account.')
                        .prop('hidden', false);
                }
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
            _sgOverrideActive     = false;
            _sgOverrideSplitGroup = null;
            _populating = true;
            try {
                var acctInstErr = dxInstance(SEL.account);
                if (acctInstErr) {
                    acctInstErr.option('dataSource', _accountsCache || []);
                    acctInstErr.option('value', null);
                }
                var sgInstErr = dxInstance(SEL.splitGroup);
                if (sgInstErr) {
                    sgInstErr.option('dataSource', []);
                    sgInstErr.option('value', null);
                    sgInstErr.option('disabled', true);
                }
                $(SEL.lotDesc).val('');
                $(SEL.splitGroupWarn).prop('hidden', true);
            } finally {
                _populating = false;
            }
        }
    }

    // ── Save ─────────────────────────────────────────────────────────────────

    async function saveCreate() {
        const splitGroupId = parseInt(
            $(SEL.splitGroup).dxSelectBox('option', 'value'), 10) || null;

        if (!currentLocationId) {
            $(SEL.createError).text('No location selected.').prop('hidden', false);
            return;
        }
        if (!splitGroupId) {
            $(SEL.createError).text('Please select a split group.').prop('hidden', false);
            return;
        }

        var overrideAccountId = null;
        if (_sgOverrideActive) {
            overrideAccountId = parseInt(
                $(SEL.account).dxSelectBox('option', 'value'), 10) || null;
            if (!overrideAccountId) {
                $(SEL.createError).text('Please select an account for this split group.').prop('hidden', false);
                return;
            }
        }

        if (!$(SEL.state).dxSelectBox('option', 'value')) {
            $(SEL.createError).text('Please select a state.').prop('hidden', false);
            return;
        }
        if (!$(SEL.county).dxSelectBox('option', 'value')) {
            $(SEL.createError).text('Please select a county.').prop('hidden', false);
            return;
        }

        var createPin = await getPin();
        if (createPin === null) return;

        const btn = $(SEL.saveBtn);
        btn.prop('disabled', true).text('Creating\u2026');
        $(SEL.createError).prop('hidden', true);

        try {
            const resp = await fetch('/api/GrowerDelivery/WeightSheetLots', {
                method:  'POST',
                headers: { 'Content-Type': 'application/json' },
                body:    JSON.stringify({
                    LocationId:        currentLocationId,
                    SplitGroupId:      splitGroupId,
                    ItemId:            $(SEL.item).dxSelectBox('option', 'value') || null,
                    Notes:             $(SEL.notes).val().trim() || null,
                    State:             $(SEL.state).dxSelectBox('option', 'value') || null,
                    County:            $(SEL.county).dxSelectBox('option', 'value') || null,
                    Landlord:          $(SEL.landlord).val().trim() || null,
                    FarmNumber:        $(SEL.farmNumber).val().trim() || null,
                    Pin:               createPin,
                    OverrideAccountId: overrideAccountId,
                    LotType:           _lotType === 'warehouse' ? 1 : 0,
                }),
            });

            if (!resp.ok) {
                const detail = await tryParseError(resp);
                $(SEL.createError).text('Error: ' + detail).prop('hidden', false);
                return;
            }

            var result = await resp.json();

            if (_returnTo === 'deliveryLoads' && _wsId && result.LotId) {
                var pin = await getPin();
                if (pin === null) {
                    window.location.href = '/GrowerDelivery/WeightSheetDeliveryLoads?wsId=' + _wsId;
                    return;
                }
                btn.text('Assigning\u2026');
                try {
                    var patchResp = await fetch('/api/GrowerDelivery/WeightSheet/' + _wsId, {
                        method:  'PATCH',
                        headers: { 'Content-Type': 'application/json' },
                        body:    JSON.stringify({ LotId: result.LotId, Pin: pin }),
                    });
                    if (!patchResp.ok) {
                        var patchDetail = await tryParseError(patchResp);
                        $(SEL.createError).text('Lot created but reassign failed: ' + patchDetail).prop('hidden', false);
                        return;
                    }
                } catch (patchEx) {
                    $(SEL.createError).text('Lot created but reassign failed: ' + patchEx.message).prop('hidden', false);
                    return;
                }
                window.location.href = '/GrowerDelivery/WeightSheetDeliveryLoads?wsId=' + _wsId;
                return;
            }

            navigateBack();

        } catch (ex) {
            $(SEL.createError).text('Network error: ' + ex.message).prop('hidden', false);
        } finally {
            btn.prop('disabled', false).text('Create Lot');
        }
    }

    async function saveEdit() {
        var body = {
            Notes: ($(SEL.notes).val() || '').trim() || null,
        };

        if (_editingFullEdit) {
            body.LotDescription = ($(SEL.lotDesc).val() || '').trim() || null;
            var itemInst = dxInstance(SEL.item);
            body.ItemId = itemInst ? itemInst.option('value') || null : null;
            var stateInst = dxInstance(SEL.state);
            body.State = stateInst ? stateInst.option('value') || null : null;
            var countyInst = dxInstance(SEL.county);
            body.County = countyInst ? countyInst.option('value') || null : null;
            body.Landlord = ($(SEL.landlord).val() || '').trim() || null;
            body.FarmNumber = ($(SEL.farmNumber).val() || '').trim() || null;
        }

        if (_editingOriginal) {
            var orig = _editingOriginal;
            var dirty = (body.Notes || null) !== (orig.Notes || null);
            if (_editingFullEdit && !dirty) {
                dirty = (body.LotDescription || null) !== (orig.LotDescription || null)
                     || (body.ItemId || null) !== (orig.ItemId || null)
                     || (body.State || null) !== (orig.State || null)
                     || (body.County || null) !== (orig.County || null)
                     || (body.Landlord || null) !== (orig.Landlord || null)
                     || (body.FarmNumber || null) !== (orig.FarmNumber || null);
            }
            if (!dirty) {
                navigateBack();
                return;
            }
        }

        var pin = await getPin();
        if (pin === null) {
            if (_returnTo === 'deliveryLoads' && _wsId) {
                window.location.href = '/GrowerDelivery/WeightSheetDeliveryLoads?wsId=' + _wsId;
            }
            return;
        }

        body.Pin = pin;

        var btn = $(SEL.saveBtn);
        btn.prop('disabled', true).text('Saving\u2026');
        $(SEL.createError).prop('hidden', true);

        try {
            var resp = await fetch('/api/GrowerDelivery/WeightSheetLots/' + _editingLotId, {
                method:  'PATCH',
                headers: { 'Content-Type': 'application/json' },
                body:    JSON.stringify(body),
            });

            if (!resp.ok) {
                var detail = await tryParseError(resp);
                $(SEL.createError).text('Error: ' + detail).prop('hidden', false);
                return;
            }

            if (_returnTo === 'deliveryLoads' && _wsId) {
                window.location.href = '/GrowerDelivery/WeightSheetDeliveryLoads?wsId=' + _wsId;
                return;
            }
            navigateBack();
        } catch (ex) {
            $(SEL.createError).text('Network error: ' + ex.message).prop('hidden', false);
        } finally {
            btn.prop('disabled', false).text('Save');
        }
    }

    // ── Reset form for new lot ──────────────────────────────────────────────

    async function resetCreateForm() {
        _sgOverrideActive     = false;
        _sgOverrideSplitGroup = null;

        var itemInst   = dxInstance(SEL.item);
        var acctInst   = dxInstance(SEL.account);
        var sgInst     = dxInstance(SEL.splitGroup);

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
        _lastSgLookup = '';
        $(SEL.splitGroupWarn)
            .text('No split group set up for this account.')
            .prop('hidden', true);
        $(SEL.createError).prop('hidden', true).text('');

        await loadLocationCounties();
        applyStateCountyConstraints();

        if (itemSource.length === 1 && itemInst) {
            _populating = true;
            try {
                var selectedItemId = itemSource[0].ItemId;
                itemInst.option('value', selectedItemId);
                $(SEL.sgShortcut).prop('disabled', false);
                updateLotDescription();

                var accounts = _accountsCache || [];
                if (currentLocationId) {
                    try {
                        accounts = await $.getJSON('/api/Lookups/ProducerAccountsForItem?itemId=' + selectedItemId + '&locationId=' + currentLocationId);
                    } catch (ex) {
                        console.warn('[EditLot] Filtered accounts load failed in auto-cascade', ex);
                    }
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

    // ── PIN prompt ──────────────────────────────────────────────────────────

    function promptForPin() {
        return new Promise(function (resolve) {
            var $overlay = $('<div class="gm-pin-overlay"></div>');
            var $dialog  = $(
                '<div class="gm-pin-dialog">' +
                    '<h5>Enter Your PIN</h5>' +
                    '<p class="text-muted small mb-2">A valid user PIN is required to save changes.</p>' +
                    '<input type="text" class="form-control gm-pin-input" placeholder="PIN" inputmode="numeric" autocomplete="off" autofocus style="-webkit-text-security:disc" />' +
                    '<div class="gm-pin-error text-danger small mt-1" hidden></div>' +
                    '<div class="d-flex gap-2 mt-3">' +
                        '<button type="button" class="btn btn-primary gm-pin-confirm flex-fill">Confirm</button>' +
                        '<button type="button" class="btn btn-outline-secondary gm-pin-cancel flex-fill">Cancel</button>' +
                    '</div>' +
                '</div>'
            );

            $overlay.append($dialog);
            $('body').append($overlay);

            var $input = $dialog.find('.gm-pin-input');
            var $error = $dialog.find('.gm-pin-error');

            function close(val) {
                $overlay.remove();
                resolve(val);
            }

            $dialog.find('.gm-pin-cancel').on('click', function () { close(null); });
            $dialog.find('.gm-pin-confirm').on('click', function () {
                var val = parseInt($input.val(), 10);
                if (!val || val <= 0) {
                    $error.text('Please enter a valid numeric PIN.').prop('hidden', false);
                    $input.focus();
                    return;
                }
                close(val);
            });

            $input.on('keydown', function (e) {
                if (e.key === 'Enter') $dialog.find('.gm-pin-confirm').click();
                if (e.key === 'Escape') close(null);
            });

            setTimeout(function () { $input.focus(); }, 100);
        });
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    function showAlert(msg, type) {
        const el = $(SEL.alert);
        el.removeClass('alert-success alert-danger alert-warning alert-info')
          .addClass('alert alert-' + type)
          .text(msg)
          .prop('hidden', false);
        setTimeout(() => el.prop('hidden', true), 4000);
    }

    async function tryParseError(resp) {
        try {
            const j = await resp.json();
            return j.message || j.title || resp.statusText;
        } catch { return resp.statusText; }
    }

    function dxInstance(sel) {
        try { return $(sel).dxSelectBox('instance'); } catch (e) { return null; }
    }

})();
