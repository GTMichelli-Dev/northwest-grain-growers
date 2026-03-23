(function () {
    'use strict';

    // ── Helpers ─────────────────────────────────────────────────────────────

    // Formats a numeric LotId like 604063000001 as "604-063-000001"
    function formatLotId(id) {
        var s = String(id);
        if (s.length < 7) return s;
        return s.substring(0, 3) + '-' + s.substring(3, 6) + '-' + s.substring(6);
    }

    // ── Selectors ────────────────────────────────────────────────────────────

    const SEL = {
        alert:           '#wslAlert',
        listPanel:       '#wslListPanel',
        lotsGrid:        '#wslLotsGrid',
        newBtn:          '#wslNewBtn',
        createPanel:     '#wslCreatePanel',
        panelTitle:      '#wslPanelTitle',
        cancelCreateBtn: '#wslCancelCreateBtn',
        account:         '#wslAccount',
        splitGroup:      '#wslSplitGroup',
        item:            '#wslItem',
        state:           '#wslState',
        county:          '#wslCounty',
        lotDesc:         '#wslLotDesc',
        landlord:        '#wslLandlord',
        notes:           '#wslNotes',
        createError:     '#wslCreateError',
        saveBtn:         '#wslSaveBtn',
        search:          '#wslSearch',
        filterDate:      '#wslFilterDate',
        filterStatus:    '#wslFilterStatus',
        splitGroupWarn:  '#wslSplitGroupWarning',
        sgShortcut:      '#wslSgShortcut',
        sgError:         '#wslSgError',
    };

    // ── State ────────────────────────────────────────────────────────────────

    let currentLocationId   = null;
    let _accountsCache      = null;
    let _allItemsCache      = null;
    let _lotsCache          = [];
    let _stateCountiesCache = null;

    // LocationCounties constraint data
    let _locationCountiesData       = null;   // raw from API
    let _locationCountiesLocationId = null;   // which location was fetched
    let _activeStateCounties        = null;   // same shape as _stateCountiesCache, may be filtered

    // LocationItemFilter constraint data
    let _locationItemFiltersCache       = null;   // array from API or null
    let _locationItemFiltersLocationId  = null;   // which location was fetched

    // Edit mode — when non-null, the form is in edit mode for this lot
    let _editingLotId   = null;
    let _editingFullEdit = false;
    let _editingOriginal = null; // snapshot of original values for dirty-check

    // Suppress cascading resets while populating edit form
    let _populating = false;

    // Deep-link params (from WeightSheetDeliveryLoads "Edit Lot Properties" / "New Lot" links)
    let _deepLinkEditLotId = null;
    let _deepLinkReturnTo  = null;
    let _deepLinkWsId      = null;
    let _deepLinkCreateNew = false;

    // ── Init ─────────────────────────────────────────────────────────────────

    $(function () {
        // Parse deep-link query params
        var urlParams = new URLSearchParams(window.location.search);
        _deepLinkEditLotId = parseInt(urlParams.get('editLotId'), 10) || null;
        _deepLinkReturnTo  = urlParams.get('returnTo') || null;
        _deepLinkWsId      = parseInt(urlParams.get('wsId'), 10) || null;
        _deepLinkCreateNew = urlParams.get('createNew') === 'true';

        // Redirect banner back to weight sheet if coming from delivery loads
        if (_deepLinkReturnTo === 'deliveryLoads' && _deepLinkWsId) {
            var wsUrl = '/GrowerDelivery/WeightSheetDeliveryLoads?wsId=' + _deepLinkWsId;
            var $bar = $('.gm-module-bar');
            $bar.attr('href', wsUrl).attr('title', 'Back to Weight Sheet');
            $bar.find('.gm-module-bar__label').text('BACK TO WEIGHT SHEET');
        }

        initLocation();
        initLotsGrid();
        initItemPicker();
        initAccountPicker();
        initSplitGroupPicker();
        initStateCountyPickers();
        wireButtons();
        wireFilters();
    });

    // ── Location (reads from navbar selector) ──────────────────────────────────

    function setLocationId(id) {
        if (id !== currentLocationId) {
            currentLocationId = id;
            _locationCountiesData = null;
            _locationCountiesLocationId = null;
            _locationItemFiltersCache = null;
            _locationItemFiltersLocationId = null;
        }
    }

    async function initLocation() {
        var dropdown = document.getElementById('gm-location-select');

        // Listen for navbar location changes (the page reloads on change,
        // but handle in-page changes just in case).
        if (dropdown) {
            dropdown.addEventListener('change', function () {
                var id = parseInt(dropdown.value, 10) || null;
                setLocationId(id);
                if (id) {
                    showListPanel();
                    refreshLots();
                } else {
                    hidePanels();
                }
            });
        }

        // Fetch the current location from the session API (the navbar
        // dropdown is populated asynchronously so its value isn't ready yet).
        try {
            var resp = await fetch('/api/LocationContextApi/current');
            var current = await resp.json();
            if (current.HasLocation && current.LocationId) {
                setLocationId(current.LocationId);
                showListPanel();
                refreshLots();
            }
        } catch (ex) {
            console.warn('[WeightSheetLots] Could not read current location', ex);
        }
    }

    // ── Paging state ──────────────────────────────────────────────────────────
    let _pagingEnabled = true;

    // ── Filters & grid ─────────────────────────────────────────────────────────

    function wireFilters() {
        var debounceTimer = null;
        $(SEL.search).on('input', function () {
            clearTimeout(debounceTimer);
            debounceTimer = setTimeout(applyGridFilters, 300);
        });
        $(SEL.filterDate).on('change', applyGridFilters);
        $(SEL.filterStatus).on('change', applyGridFilters);

        $('#wslPagingSwitch').dxSwitch({
            value: _pagingEnabled,
            hint: 'Turn paging on/off',
            onValueChanged: function (e) {
                _pagingEnabled = e.value;
                var grid = $(SEL.lotsGrid).dxDataGrid('instance');
                if (grid) {
                    grid.option('paging.enabled', e.value);
                    grid.option('pager.visible', e.value);
                }
            }
        });
    }

    function applyGridFilters() {
        var grid;
        try { grid = $(SEL.lotsGrid).dxDataGrid('instance'); } catch (e) { return; }
        if (!grid) return;

        var searchTerm = ($(SEL.search).val() || '').trim();
        var dateTerm   = $(SEL.filterDate).val() || '';
        var statusTerm = $(SEL.filterStatus).val() || 'open';

        var combinedFilter = [];

        // Status filter
        if (statusTerm === 'open') {
            combinedFilter.push(['IsOpen', '=', true]);
        } else if (statusTerm === 'closed') {
            combinedFilter.push(['IsOpen', '=', false]);
        }

        // Date filter
        if (dateTerm) {
            // Convert yyyy-mm-dd to MM/dd/yyyy to match API format
            var dp = dateTerm.split('-');
            if (dp.length === 3) {
                var dateStr = dp[1] + '/' + dp[2] + '/' + dp[0];
                combinedFilter.push(['CreatedAt', '=', dateStr]);
            }
        }

        // Search across all visible text fields
        if (searchTerm) {
            var searchFilter = [
                ['LotDescription', 'contains', searchTerm],
                'or',
                ['SplitGroupDescription', 'contains', searchTerm],
                'or',
                ['ItemDescription', 'contains', searchTerm],
                'or',
                ['State', 'contains', searchTerm],
                'or',
                ['County', 'contains', searchTerm],
                'or',
                ['Landlord', 'contains', searchTerm],
                'or',
                ['Notes', 'contains', searchTerm],
                'or',
                ['AccountName', 'contains', searchTerm],
                'or',
                ['CreatedAt', 'contains', searchTerm],
            ];
            combinedFilter.push(searchFilter);
        }

        if (combinedFilter.length === 0) {
            grid.clearFilter();
        } else if (combinedFilter.length === 1) {
            grid.filter(combinedFilter[0]);
        } else {
            // Join with 'and'
            var andFilter = [combinedFilter[0]];
            for (var i = 1; i < combinedFilter.length; i++) {
                andFilter.push('and');
                andFilter.push(combinedFilter[i]);
            }
            grid.filter(andFilter);
        }
    }

    // ── Lot grid ──────────────────────────────────────────────────────────────

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
            columnFixing: { enabled: true },
            noDataText: 'No lots match the current filters.',
            paging: {
                enabled: _pagingEnabled,
                pageSize: 20,
            },
            pager: {
                visible: _pagingEnabled,
                showPageSizeSelector: true,
                allowedPageSizes: [10, 20, 50],
                showInfo: true,
            },
            sorting: { mode: 'multiple' },
            columns: [
                {
                    dataField: 'IsOpen',
                    caption: 'Status',
                    width: 70,
                    fixed: true,
                    fixedPosition: 'left',
                    cellTemplate: function (container, options) {
                        var isOpen = options.data.IsOpen;
                        $('<span>')
                            .addClass('gm-wsl-card__status ' + (isOpen ? 'gm-wsl-card__status--open' : 'gm-wsl-card__status--closed'))
                            .text(isOpen ? 'Open' : 'Closed')
                            .appendTo(container);
                    },
                },
                {
                    dataField: 'LotId',
                    caption: 'Lot ID',
                    sortOrder: 'desc',
                    sortIndex: 0,
                    customizeText: function (cellInfo) { return formatLotId(cellInfo.value); },
                },
                {
                    dataField: 'As400Id',
                    caption: 'Agvantage Id',
                    width: 110,
                },
                {
                    dataField: 'SplitGroupId',
                    caption: 'SG ID',
                    width: 80,
                },
                {
                    dataField: 'SplitGroupDescription',
                    caption: 'Split Group',
                },
                {
                    dataField: 'ItemDescription',
                    caption: 'Item',
                    calculateCellValue: function (rowData) {
                        if (!rowData.ItemDescription) return '';
                        return rowData.ItemDescription + (rowData.ItemId ? ' (' + rowData.ItemId + ')' : '');
                    },
                },
                {
                    dataField: 'AccountName',
                    caption: 'Primary',
                    calculateCellValue: function (rowData) {
                        if (!rowData.AccountName) return '';
                        return rowData.AccountName + (rowData.AccountAs400Id ? ' (' + rowData.AccountAs400Id + ')' : '');
                    },
                },
                {
                    dataField: 'Landlord',
                    caption: 'Landlord',
                },
                {
                    dataField: 'Notes',
                    caption: 'Notes',
                    cssClass: 'gm-wsl-grid-notes',
                },
                {
                    dataField: 'CreatedAt',
                    caption: 'Created',
                    width: 100,
                },
                {
                    caption: '',
                    width: 160,
                    fixed: true,
                    fixedPosition: 'right',
                    allowSorting: false,
                    cellTemplate: function (container, options) {
                        var lot = options.data;
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

                        var $wrap = $('<div>').addClass('gm-wsl-grid-actions');

                        $('<button>')
                            .addClass('btn btn-sm btn-outline-primary')
                            .text(canFullEdit ? 'Edit' : 'Edit Notes')
                            .on('click', function () {
                                enterEditMode(lot, canFullEdit);
                            })
                            .appendTo($wrap);

                        $('<button>')
                            .addClass('btn btn-sm ' + (isOpen ? 'btn-outline-danger' : 'btn-outline-success'))
                            .text(isOpen ? 'Close' : 'Re-open')
                            .on('click', function () {
                                toggleLot(lot.LotId, isOpen ? 'close' : 'open');
                            })
                            .appendTo($wrap);

                        $wrap.appendTo(container);
                    },
                },
            ],
            onRowPrepared: function (e) {
                if (e.rowType === 'data' && !e.data.IsOpen) {
                    $(e.rowElement).addClass('gm-wsl-row--closed');
                }
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
            _lotsCache = await $.getJSON('/api/GrowerDelivery/WeightSheetLots?locationId=' + currentLocationId);
        } catch (ex) {
            _lotsCache = [];
        }

        grid.option('dataSource', _lotsCache);
        grid.endCustomLoading();

        applyGridFilters();

        // Deep-link: auto-open lot in edit mode if editLotId was in the URL
        if (_deepLinkEditLotId) {
            var targetLot = findLotById(_deepLinkEditLotId);
            if (targetLot) {
                var isOpen     = targetLot.IsOpen;
                var hasClosedWs = targetLot.HasClosedWeightSheets;
                var createdToday = targetLot.CreatedAt && targetLot.CreatedAt.substring(0, 10) === new Date().toISOString().substring(0, 10);
                var canFullEdit = (isOpen && !hasClosedWs) || createdToday;
                enterEditMode(targetLot, canFullEdit);
            }
            _deepLinkEditLotId = null; // only auto-open once
        }

        // Deep-link: auto-open create form if createNew=true was in the URL
        if (_deepLinkCreateNew) {
            _editingLotId    = null;
            _editingFullEdit = false;
            $(SEL.panelTitle).text('New Weight Sheet Lot');
            $(SEL.saveBtn).text('Create Lot');
            $(SEL.landlord).prop('disabled', false);
            $(SEL.listPanel).prop('hidden', true);
            $(SEL.createPanel).prop('hidden', false);
            $('.gm-gd').removeClass('gm-gd--wide');
            await resetCreateForm();
            _deepLinkCreateNew = false; // only auto-open once
        }
    }

    function findLotById(id) {
        for (var i = 0; i < _lotsCache.length; i++) {
            if (_lotsCache[i].LotId === id) return _lotsCache[i];
        }
        return null;
    }

    // ── Edit mode (reuses the create panel) ──────────────────────────────────

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
        };

        // Show the create panel, hide the list
        $(SEL.listPanel).prop('hidden', true);
        $(SEL.createPanel).prop('hidden', false);
        $('.gm-gd').removeClass('gm-gd--wide');

        // Update title and button text
        $(SEL.panelTitle).text('Edit Lot ' + formatLotId(lot.LotId));
        $(SEL.saveBtn).text('Save');

        // Clear error
        $(SEL.createError).prop('hidden', true).text('');
        $(SEL.splitGroupWarn).prop('hidden', true);
        $(SEL.sgError).prop('hidden', true).text('');

        // Suppress cascading resets while we populate all pickers
        _populating = true;
        try {
            await populateEditForm(lot, fullEdit);
        } finally {
            _populating = false;
        }
    }

    async function populateEditForm(lot, fullEdit) {
        // SG# shortcut — show current value, enable/disable based on edit mode
        $(SEL.sgShortcut).val(lot.SplitGroupId || '').prop('disabled', !fullEdit);

        if (fullEdit) {
            // ── Full edit: all pickers editable, pre-populated ──────────────

            // Resolve item list — start with location-filtered items, then narrow by account filters
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
                } catch (ex) { console.warn('[WeightSheetLots] AccountItemFilters load failed', ex); }
            }

            // Item picker — set data source and value
            var itemInst = dxInstance(SEL.item);
            if (itemInst) {
                itemInst.option('dataSource', editItems);
                itemInst.option('disabled', false);
                itemInst.option('value', lot.ItemId || null);
            }

            // Account picker — load accounts, set value, enable
            var acctInst = dxInstance(SEL.account);
            if (acctInst) {
                acctInst.option('disabled', false);
                acctInst.option('value', lot.AccountId || null);
            }

            // Split group picker — load groups for the account, set value
            if (lot.AccountId) {
                var groups = [];
                try { groups = await $.getJSON('/api/GrowerDelivery/SplitGroupsByAccount?accountId=' + lot.AccountId); }
                catch (ex) { console.warn('[WeightSheetLots] SplitGroup load failed', ex); }
                var sgInst = dxInstance(SEL.splitGroup);
                if (sgInst) {
                    sgInst.option('dataSource', groups);
                    sgInst.option('disabled', false);
                    sgInst.option('value', lot.SplitGroupId || null);
                }
            }

            // State/County pickers — load constraints, set data sources, then set values
            await loadLocationCounties();

            // Build the constrained state list (same logic as applyStateCountyConstraints but without resetting)
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

            // Lot description
            $(SEL.lotDesc).val(lot.LotDescription || '');

            // Landlord
            $(SEL.landlord).val(lot.Landlord || '').prop('disabled', false);

            // Notes
            $(SEL.notes).val(lot.Notes || '');

        } else {
            // ── Notes-only edit: disable all pickers, show current values ────

            // Item — show current value but disabled
            var itemInst2 = dxInstance(SEL.item);
            if (itemInst2) {
                itemInst2.option('value', lot.ItemId || null);
                itemInst2.option('disabled', true);
            }

            // Account — disabled
            var acctInst2 = dxInstance(SEL.account);
            if (acctInst2) {
                acctInst2.option('value', lot.AccountId || null);
                acctInst2.option('disabled', true);
            }

            // Split group — load and disable
            if (lot.AccountId) {
                var groups2 = [];
                try { groups2 = await $.getJSON('/api/GrowerDelivery/SplitGroupsByAccount?accountId=' + lot.AccountId); }
                catch (ex) { /* ignore */ }
                var sgInst2 = dxInstance(SEL.splitGroup);
                if (sgInst2) {
                    sgInst2.option('dataSource', groups2);
                    sgInst2.option('value', lot.SplitGroupId || null);
                    sgInst2.option('disabled', true);
                }
            } else {
                var sgInst3 = dxInstance(SEL.splitGroup);
                if (sgInst3) { sgInst3.option('disabled', true); }
            }

            // State/County — set data sources, values, then disable
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

            // Lot description — read-only
            $(SEL.lotDesc).val(lot.LotDescription || '');

            // Landlord — disabled
            $(SEL.landlord).val(lot.Landlord || '').prop('disabled', true);

            // Notes — editable
            $(SEL.notes).val(lot.Notes || '');
        }
    }

    function exitEditMode() {
        _editingLotId    = null;
        _editingFullEdit = false;
        _editingOriginal = null;

        // Restore panel title and button text
        $(SEL.panelTitle).text('New Weight Sheet Lot');
        $(SEL.saveBtn).text('Create Lot');

        // Re-enable inputs in case they were disabled
        $(SEL.landlord).prop('disabled', false);
        $(SEL.sgShortcut).val('').prop('disabled', false);
        $(SEL.sgError).prop('hidden', true).text('');

        // Hide create panel, show list
        $(SEL.createPanel).prop('hidden', true);
        $(SEL.listPanel).prop('hidden', false);
        $('.gm-gd').addClass('gm-gd--wide');
    }

    async function toggleLot(id, action) {
        try {
            const resp = await fetch('/api/GrowerDelivery/WeightSheetLots/' + id + '/' + action, {
                method: 'POST',
            });
            if (!resp.ok) {
                const detail = await tryParseError(resp);
                showAlert('Error: ' + detail, 'danger');
                return;
            }
            refreshLots();
        } catch (ex) {
            showAlert('Network error: ' + ex.message, 'danger');
        }
    }

    // ── Create form ───────────────────────────────────────────────────────────

    async function initAccountPicker() {
        if (!_accountsCache) {
            try { _accountsCache = await $.getJSON('/api/Lookups/ProducerAccounts'); }
            catch (ex) {
                console.warn('[WeightSheetLots] Account load failed', ex);
                _accountsCache = [];
            }
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
                const sgInst = $(SEL.splitGroup).dxSelectBox('instance');
                if (sgInst) { sgInst.reset(); sgInst.option('dataSource', []); sgInst.option('disabled', true); }
                $(SEL.lotDesc).val('');
                $(SEL.splitGroupWarn).prop('hidden', true);
                if (e.value) {
                    loadSplitGroups(e.value);
                }
            },
            onFocusOut: function (e) {
                if (!e.component.option('value')) {
                    e.component.reset();
                    const sgInst = $(SEL.splitGroup).dxSelectBox('instance');
                    if (sgInst) { sgInst.reset(); sgInst.option('dataSource', []); sgInst.option('disabled', true); }
                    $(SEL.lotDesc).val('');
                    $(SEL.splitGroupWarn).prop('hidden', true);
                }
            }
        });
    }

    async function loadSplitGroups(accountId) {
        let groups = [];
        try { groups = await $.getJSON('/api/GrowerDelivery/SplitGroupsByAccount?accountId=' + accountId); }
        catch (ex) { console.warn('[WeightSheetLots] SplitGroup load failed', ex); }
        const sgInst = dxInstance(SEL.splitGroup);
        if (sgInst) {
            sgInst.option('dataSource', groups);
            sgInst.option('disabled', groups.length === 0);
            // Auto-populate if exactly one split group
            if (groups.length === 1) {
                sgInst.option('value', groups[0].SplitGroupId);
            }
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
                    // Sync SG# shortcut input with the dropdown selection
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

    // ── Item picker ────────────────────────────────────────────────────────

    async function initItemPicker() {
        if (!_allItemsCache) {
            try { _allItemsCache = await $.getJSON('/api/Lookups/WarehouseItems'); }
            catch (ex) {
                console.warn('[WeightSheetLots] Items load failed', ex);
                _allItemsCache = [];
            }
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
                    // Filter accounts by item + location, then enable picker
                    var accounts = _accountsCache || [];
                    if (acctInst) {
                        acctInst.reset();
                        if (currentLocationId) {
                            try {
                                accounts = await $.getJSON('/api/Lookups/ProducerAccountsForItem?itemId=' + e.value + '&locationId=' + currentLocationId);
                            } catch (ex) {
                                console.warn('[WeightSheetLots] Filtered accounts load failed', ex);
                            }
                        }
                        acctInst.option('dataSource', accounts);
                        acctInst.option('disabled', false);
                        // Auto-select if only one account, then auto-cascade to split groups
                        if (accounts.length === 1) {
                            acctInst.option('value', accounts[0].AccountId);
                        }
                    }
                    $(SEL.sgShortcut).prop('disabled', false);
                } else {
                    // Clear and disable account + split group + SG#
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
                console.warn('[WeightSheetLots] State/Counties load failed', ex);
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


    // ── LocationCounties constraints ──────────────────────────────────────

    async function loadLocationCounties() {
        if (!currentLocationId) { _locationCountiesData = []; return; }
        if (_locationCountiesLocationId === currentLocationId && _locationCountiesData !== null) return;
        try {
            _locationCountiesData = await $.getJSON('/api/locations/' + currentLocationId + '/Counties');
        } catch (ex) {
            console.warn('[WeightSheetLots] LocationCounties load failed', ex);
            _locationCountiesData = [];
        }
        _locationCountiesLocationId = currentLocationId;
    }

    // ── LocationItemFilter constraints ───────────────────────────────────

    async function loadLocationItemFilters() {
        if (!currentLocationId) { _locationItemFiltersCache = []; return; }
        if (_locationItemFiltersLocationId === currentLocationId && _locationItemFiltersCache !== null) return;
        try {
            _locationItemFiltersCache = await $.getJSON('/api/locations/' + currentLocationId + '/Items');
        } catch (ex) {
            console.warn('[WeightSheetLots] LocationItemFilters load failed', ex);
            _locationItemFiltersCache = [];
        }
        _locationItemFiltersLocationId = currentLocationId;
    }

    function getFilteredItems() {
        var allItems = _allItemsCache || [];
        if (!_locationItemFiltersCache || _locationItemFiltersCache.length === 0) {
            return allItems;  // no filters — allow all active items
        }
        // Build set of allowed ItemIds from the location filters
        var allowedIds = {};
        _locationItemFiltersCache.forEach(function (f) { allowedIds[f.ItemId] = true; });
        return allItems.filter(function (item) { return allowedIds[item.ItemId]; });
    }

    function applyStateCountyConstraints() {
        var stateInst  = dxInstance(SEL.state);
        var countyInst = dxInstance(SEL.county);
        if (!stateInst || !countyInst) return;

        if (_locationCountiesData && _locationCountiesData.length > 0) {
            // Build filtered _activeStateCounties from LocationCounties
            var stateMap = {};
            _locationCountiesData.forEach(function (lc) {
                if (!stateMap[lc.StateAbv]) {
                    stateMap[lc.StateAbv] = { State: lc.StateAbv, StateName: lc.StateName, Counties: [] };
                }
                stateMap[lc.StateAbv].Counties.push(lc.CountyName);
            });
            _activeStateCounties = Object.values(stateMap);
        } else {
            // No location counties — use full list
            _activeStateCounties = _stateCountiesCache || [];
        }

        var states = _activeStateCounties.map(function (s) { return { State: s.State, StateName: s.StateName }; });
        stateInst.option('dataSource', states);
        stateInst.reset();
        countyInst.reset();
        countyInst.option('dataSource', []);
        countyInst.option('disabled', true);

        // Auto-populate if exactly one location county
        if (_locationCountiesData && _locationCountiesData.length === 1) {
            var lc = _locationCountiesData[0];
            stateInst.option('value', lc.StateAbv);
            // County data source is updated by state's onValueChanged;
            // set the county value after that propagates
            setTimeout(function () {
                countyInst.option('value', lc.CountyName);
            }, 0);
        }
    }

    function wireButtons() {

        $(SEL.newBtn).dxButton({
            text: "New Lot",
            icon: "add",
            stylingMode: "outlined",
            type: "default",
            onClick: async function () {
                _editingLotId    = null;
                _editingFullEdit = false;
                $(SEL.panelTitle).text('New Weight Sheet Lot');
                $(SEL.saveBtn).text('Create Lot');
                $(SEL.landlord).prop('disabled', false);
                $(SEL.listPanel).prop('hidden', true);
                $(SEL.createPanel).prop('hidden', false);
                $('.gm-gd').removeClass('gm-gd--wide');
                await resetCreateForm();
            }
        });

        $(SEL.cancelCreateBtn).on('click', function () {
            // If deep-linked from delivery loads, redirect back to weight sheet
            if (_deepLinkReturnTo === 'deliveryLoads' && _deepLinkWsId) {
                window.location.href = '/GrowerDelivery/WeightSheetDeliveryLoads?wsId=' + _deepLinkWsId;
                return;
            }
            exitEditMode();
        });

        $(SEL.saveBtn).on('click', async function () {
            if (_editingLotId) {
                await saveEdit();
            } else {
                await saveCreate();
            }
        });

        // SG# shortcut — on Enter or blur, look up the split group
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
                // Set account picker value (enable it first)
                var acctInst = dxInstance(SEL.account);
                if (acctInst) {
                    acctInst.option('disabled', false);
                    acctInst.option('value', sg.PrimaryAccountId);
                }

                // Load split groups for this account and set the value
                var groups = [];
                try { groups = await $.getJSON('/api/GrowerDelivery/SplitGroupsByAccount?accountId=' + sg.PrimaryAccountId); }
                catch (ex) { console.warn('[WeightSheetLots] SplitGroup load failed', ex); }

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
        if (!$(SEL.state).dxSelectBox('option', 'value')) {
            $(SEL.createError).text('Please select a state.').prop('hidden', false);
            return;
        }
        if (!$(SEL.county).dxSelectBox('option', 'value')) {
            $(SEL.createError).text('Please select a county.').prop('hidden', false);
            return;
        }

        const btn = $(SEL.saveBtn);
        btn.prop('disabled', true).text('Creating\u2026');
        $(SEL.createError).prop('hidden', true);

        try {
            const resp = await fetch('/api/GrowerDelivery/WeightSheetLots', {
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
                const detail = await tryParseError(resp);
                $(SEL.createError).text('Error: ' + detail).prop('hidden', false);
                return;
            }

            var result = await resp.json();
            console.log('[wslots] Lot created:', result, 'returnTo:', _deepLinkReturnTo, 'wsId:', _deepLinkWsId);

            // If creating from delivery loads, prompt for PIN and reassign the lot to the weight sheet
            if (_deepLinkReturnTo === 'deliveryLoads' && _deepLinkWsId && result.LotId) {
                console.log('[wslots] Prompting for PIN to assign lot', result.LotId, 'to WS', _deepLinkWsId);
                var pin = await promptForPin();
                console.log('[wslots] PIN result:', pin);
                if (pin === null) {
                    // User cancelled PIN — lot was created but not assigned, go back to weight sheet
                    window.location.href = '/GrowerDelivery/WeightSheetDeliveryLoads?wsId=' + _deepLinkWsId;
                    return;
                }
                // Reassign lot to weight sheet
                btn.text('Assigning\u2026');
                try {
                    console.log('[wslots] PATCHing WS', _deepLinkWsId, 'with LotId:', result.LotId, 'Pin:', pin);
                    var patchBody = JSON.stringify({ LotId: result.LotId, Pin: pin });
                    console.log('[wslots] PATCH body:', patchBody);
                    var patchResp = await fetch('/api/GrowerDelivery/WeightSheet/' + _deepLinkWsId, {
                        method:  'PATCH',
                        headers: { 'Content-Type': 'application/json' },
                        body:    patchBody,
                    });
                    console.log('[wslots] PATCH response status:', patchResp.status);
                    var patchRespBody = await patchResp.clone().text();
                    console.log('[wslots] PATCH response body:', patchRespBody);
                    if (!patchResp.ok) {
                        var patchDetail = await tryParseError(patchResp);
                        console.error('[wslots] PATCH failed:', patchDetail);
                        $(SEL.createError).text('Lot created but reassign failed: ' + patchDetail).prop('hidden', false);
                        return;
                    }
                } catch (patchEx) {
                    console.error('[wslots] PATCH exception:', patchEx);
                    $(SEL.createError).text('Lot created but reassign failed: ' + patchEx.message).prop('hidden', false);
                    return;
                }
                console.log('[wslots] PATCH succeeded, redirecting to delivery loads');
                window.location.href = '/GrowerDelivery/WeightSheetDeliveryLoads?wsId=' + _deepLinkWsId;
                return;
            }

            exitEditMode();
            refreshLots();
            showAlert('Weight sheet lot created.', 'success');

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
        }

        // Check if anything actually changed — if not, just return to the list
        if (_editingOriginal) {
            var orig = _editingOriginal;
            var dirty = (body.Notes || null) !== (orig.Notes || null);
            if (_editingFullEdit && !dirty) {
                dirty = (body.LotDescription || null) !== (orig.LotDescription || null)
                     || (body.ItemId || null) !== (orig.ItemId || null)
                     || (body.State || null) !== (orig.State || null)
                     || (body.County || null) !== (orig.County || null)
                     || (body.Landlord || null) !== (orig.Landlord || null);
            }
            if (!dirty) {
                exitEditMode();
                showAlert('No changes detected.', 'info');
                return;
            }
        }

        // Show PIN popup and wait for user input
        var pin = await promptForPin();
        if (pin === null) {
            // If deep-linked from delivery loads, redirect back to weight sheet
            if (_deepLinkReturnTo === 'deliveryLoads' && _deepLinkWsId) {
                window.location.href = '/GrowerDelivery/WeightSheetDeliveryLoads?wsId=' + _deepLinkWsId;
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

            // If deep-linked from delivery loads, redirect back to weight sheet
            if (_deepLinkReturnTo === 'deliveryLoads' && _deepLinkWsId) {
                window.location.href = '/GrowerDelivery/WeightSheetDeliveryLoads?wsId=' + _deepLinkWsId;
                return;
            }
            exitEditMode();
            showAlert('Lot updated.', 'success');
            await refreshLots();
        } catch (ex) {
            $(SEL.createError).text('Network error: ' + ex.message).prop('hidden', false);
        } finally {
            btn.prop('disabled', false).text('Save');
        }
    }

    // ── PIN prompt popup ────────────────────────────────────────────────────

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

            var $input   = $dialog.find('.gm-pin-input');
            var $error   = $dialog.find('.gm-pin-error');

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

    // ── Helpers ───────────────────────────────────────────────────────────────

    function showListPanel() {
        $(SEL.listPanel).prop('hidden', false);
        $(SEL.createPanel).prop('hidden', true);
        $('.gm-gd').addClass('gm-gd--wide');
    }

    function hidePanels() {
        $(SEL.listPanel).prop('hidden', true);
        $(SEL.createPanel).prop('hidden', true);
        $('.gm-gd').removeClass('gm-gd--wide');
    }

    async function resetCreateForm() {
        var itemInst   = dxInstance(SEL.item);
        var acctInst   = dxInstance(SEL.account);
        var sgInst     = dxInstance(SEL.splitGroup);

        // Load location item filters, then set item dropdown to filtered or full list
        await loadLocationItemFilters();
        var itemSource = getFilteredItems();
        if (itemInst) {
            itemInst.option('dataSource', itemSource);
            itemInst.option('disabled', false);
            itemInst.reset();
        }

        // Account and Split Group reset and go disabled
        if (acctInst) { acctInst.reset(); acctInst.option('disabled', true); }
        if (sgInst)   { sgInst.reset(); sgInst.option('dataSource', []); sgInst.option('disabled', true); }

        $(SEL.lotDesc).val('');
        $(SEL.landlord).val('');
        $(SEL.notes).val('');
        $(SEL.sgShortcut).val('').prop('disabled', true);
        $(SEL.sgError).prop('hidden', true).text('');
        $(SEL.splitGroupWarn).prop('hidden', true);
        $(SEL.createError).prop('hidden', true).text('');

        // Load location counties and apply state/county constraints
        await loadLocationCounties();
        applyStateCountyConstraints();

        // Auto-cascade: if only one item, select it → filter accounts → if only one account, select it → load split groups
        if (itemSource.length === 1 && itemInst) {
            _populating = true;
            try {
                var selectedItemId = itemSource[0].ItemId;
                itemInst.option('value', selectedItemId);
                $(SEL.sgShortcut).prop('disabled', false);
                updateLotDescription();

                // Filter accounts by item + location
                var accounts = _accountsCache || [];
                if (currentLocationId) {
                    try {
                        accounts = await $.getJSON('/api/Lookups/ProducerAccountsForItem?itemId=' + selectedItemId + '&locationId=' + currentLocationId);
                    } catch (ex) {
                        console.warn('[WeightSheetLots] Filtered accounts load failed in auto-cascade', ex);
                    }
                }
                if (acctInst) {
                    acctInst.option('dataSource', accounts);
                    acctInst.option('disabled', false);
                }

                // Check if only one account — auto-select it
                if (accounts.length === 1 && acctInst) {
                    acctInst.option('value', accounts[0].AccountId);
                    await loadSplitGroups(accounts[0].AccountId);
                }
            } finally {
                _populating = false;
            }
        }
    }

    function showAlert(msg, type) {
        const el = $(SEL.alert);
        el.removeClass('alert-success alert-danger')
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

    /** Safely get a dxSelectBox instance (returns null if not yet initialized). */
    function dxInstance(sel) {
        try { return $(sel).dxSelectBox('instance'); } catch (e) { return null; }
    }

    function escapeHtml(str) {
        return String(str)
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;');
    }

    function escapeAttr(str) {
        return String(str)
            .replace(/&/g, '&amp;')
            .replace(/"/g, '&quot;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;');
    }

})();
