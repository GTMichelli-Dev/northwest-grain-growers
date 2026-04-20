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
        farmNumber:      '#wslFarmNumber',
        notes:           '#wslNotes',
        createError:     '#wslCreateError',
        saveBtn:         '#wslSaveBtn',
        search:          '#wslSearch',
        filterDate:      '#wslFilterDate',
        filterStatus:    '#wslFilterStatus',
        filterLotType:   '#wslFilterLotType',
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

    // Override mode — set when user types an SG# whose split group has no PrimaryAccountId.
    // In this mode the account picker shows ALL producer accounts and picking one does NOT
    // reload the split group dropdown; the manually entered SG# stays locked and is sent to
    // the server as the OverrideAccountId in the create payload.
    let _sgOverrideActive     = false;
    let _sgOverrideSplitGroup = null; // { SplitGroupId, SplitGroupDescription }

    // Deep-link params (from WeightSheetDeliveryLoads "Edit Lot Properties" / "New Lot" links)
    let _deepLinkEditLotId = null;
    let _deepLinkReturnTo  = null;
    let _deepLinkWsId      = null;
    let _deepLinkCreateNew = false;

    // Dedup guard for the SG# shortcut input: tracks the last value we looked
    // up so keydown(Enter) + blur don't fire the lookup twice, AND so opening
    // a lot in edit mode (which seeds the input programmatically) doesn't
    // re-run the lookup on blur and wipe the account. Module-scoped so both
    // populateEditForm and applySgShortcut can update it.
    let _lastSgLookup = '';

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

    // Format a Date as yyyy-mm-dd for a <input type="date"> value.
    function toDateInputValue(d) {
        var yyyy = d.getFullYear();
        var mm   = String(d.getMonth() + 1).padStart(2, '0');
        var dd   = String(d.getDate()).padStart(2, '0');
        return yyyy + '-' + mm + '-' + dd;
    }

    // Cookie helpers for persisting the user's "from date" for closed/all views.
    // The cookie is never written or read while status is "open" — open shows all
    // open lots regardless of age and has no date filter.
    var FROM_DATE_COOKIE = 'wslFromDate';

    function readCookie(name) {
        var prefix = name + '=';
        var parts = document.cookie ? document.cookie.split(';') : [];
        for (var i = 0; i < parts.length; i++) {
            var c = parts[i].replace(/^\s+/, '');
            if (c.indexOf(prefix) === 0) {
                return decodeURIComponent(c.substring(prefix.length));
            }
        }
        return '';
    }

    function writeCookie(name, value) {
        // 1 year, site-wide, lax — purely a UI convenience, no sensitive data.
        var expires = new Date();
        expires.setFullYear(expires.getFullYear() + 1);
        document.cookie = name + '=' + encodeURIComponent(value)
            + '; expires=' + expires.toUTCString()
            + '; path=/; SameSite=Lax';
    }

    // When status is "open" the date filter is meaningless (we show all open lots
    // regardless of age), so it is disabled and cleared. When the user switches to
    // "closed" or "all", we restore the previously chosen "from date" from the
    // cookie, or fall back to today - 90 days to mirror the server-side default.
    function syncDateFilterToStatus() {
        var statusTerm = $(SEL.filterStatus).val() || 'open';
        var $date = $(SEL.filterDate);
        if (statusTerm === 'open') {
            // Don't touch the cookie — we want to keep it for when the user
            // flips back to closed/all.
            $date.val('').prop('disabled', true);
        } else {
            var saved = readCookie(FROM_DATE_COOKIE);
            if (saved) {
                $date.val(saved);
            } else if (!$date.val()) {
                var d = new Date();
                d.setDate(d.getDate() - 90);
                var defaultValue = toDateInputValue(d);
                $date.val(defaultValue);
                writeCookie(FROM_DATE_COOKIE, defaultValue);
            }
            $date.prop('disabled', false);
        }
    }

    function wireFilters() {
        var debounceTimer = null;
        $(SEL.search).on('input', function () {
            clearTimeout(debounceTimer);
            debounceTimer = setTimeout(applyGridFilters, 300);
        });
        // Date & status filters drive the server-side query so we can look
        // back further than the 90-day default for closed/all views without
        // loading everything up front.
        $(SEL.filterDate).on('change', function () {
            // Persist the user's chosen "from date" for closed/all views so it
            // sticks across status flips and page reloads.
            var statusTerm = $(SEL.filterStatus).val() || 'open';
            if (statusTerm !== 'open') {
                var v = $(SEL.filterDate).val() || '';
                if (v) writeCookie(FROM_DATE_COOKIE, v);
            }
            refreshLots();
        });
        $(SEL.filterStatus).on('change', function () {
            syncDateFilterToStatus();
            refreshLots();
        });
        $(SEL.filterLotType).on('change', function () {
            applyGridFilters();
        });

        // Initial state: status defaults to "open", so disable the date filter.
        syncDateFilterToStatus();

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

        var combinedFilter = [];

        // Lot type filter (client-side: LotType 0=Seed, 1=Warehouse)
        var lotTypeTerm = ($(SEL.filterLotType).val() || 'all').toLowerCase();
        if (lotTypeTerm === 'seed') {
            combinedFilter.push(['LotType', '=', 0]);
        } else if (lotTypeTerm === 'warehouse') {
            combinedFilter.push(['LotType', '=', 1]);
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
                ['FarmNumber', 'contains', searchTerm],
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
                    cellTemplate: function (container, options) {
                        var isOpen = options.data.IsOpen;
                        $('<span>')
                            .addClass('gm-wsl-card__status ' + (isOpen ? 'gm-wsl-card__status--open' : 'gm-wsl-card__status--closed'))
                            .text(isOpen ? 'Open' : 'Closed')
                            .appendTo(container);
                    },
                },
                {
                    dataField: 'LotType',
                    caption: 'Type',
                    width: 110,
                    cellTemplate: function (container, options) {
                        var lt = options.data.LotType;
                        var label = lt === 1 ? 'Warehouse' : 'Seed';
                        $('<span>')
                            .css({ fontSize: '0.8rem', fontWeight: '600' })
                            .text(label)
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
                    caption: 'Landlord Name',
                },
                {
                    dataField: 'FarmNumber',
                    caption: 'Farm #',
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
                    width: 240,
                    fixed: true,
                    fixedPosition: 'right',
                    allowSorting: false,
                    cssClass: 'gm-wsl-actions-cell',
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
                                window.location.href = '/GrowerDelivery/EditWeightSheetLot?lotId=' + lot.LotId;
                            })
                            .appendTo($wrap);

                        $('<button>')
                            .addClass('btn btn-sm ' + (isOpen ? 'btn-outline-danger' : 'btn-outline-success'))
                            .text(isOpen ? 'Close' : 'Re-open')
                            .on('click', function () {
                                toggleLot(lot.LotId, isOpen ? 'close' : 'open');
                            })
                            .appendTo($wrap);

                        $('<button>')
                            .addClass('btn btn-sm btn-outline-secondary')
                            .html('<i class="dx-icon dx-icon-print"></i> Label')
                            .attr('title', 'Print lot label')
                            .on('click', function () {
                                printLotLabel(lot.LotId);
                            })
                            .appendTo($wrap);

                        $wrap.appendTo(container);
                    },
                },
            ],
            onRowPrepared: function (e) {
                if (e.rowType === 'data') {
                    if (!e.data.IsOpen) {
                        $(e.rowElement).addClass('gm-wsl-row--closed');
                    }
                    var borderColor = e.data.LotType === 1 ? '#d4a017' : '#2e7d32';
                    $(e.rowElement).css('border-left', '4px solid ' + borderColor);
                    var bgColor = e.data.LotType === 1 ? 'rgba(212,160,23,0.12)' : 'rgba(46,125,50,0.12)';
                    $(e.rowElement).find('td').css('background-color', bgColor);
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
            // Server-side filtering:
            //   status   — open (default) / closed / all
            //   fromDate — "show lots created on or after this date". Open lots
            //              have no default window; closed/all default to 90 days
            //              if the user hasn't picked a date.
            var statusTerm = $(SEL.filterStatus).val() || 'open';
            var dateTerm   = $(SEL.filterDate).val() || '';

            var url = '/api/GrowerDelivery/WeightSheetLots?locationId=' + currentLocationId
                    + '&status=' + encodeURIComponent(statusTerm);
            if (dateTerm) {
                url += '&fromDate=' + encodeURIComponent(dateTerm);
            }
            _lotsCache = await $.getJSON(url);
        } catch (ex) {
            _lotsCache = [];
        }

        grid.option('dataSource', _lotsCache);
        grid.endCustomLoading();

        applyGridFilters();

        // Deep-link: redirect to standalone edit page if editLotId was in the URL
        if (_deepLinkEditLotId) {
            var editUrl = '/GrowerDelivery/EditWeightSheetLot?lotId=' + _deepLinkEditLotId;
            if (_deepLinkReturnTo) editUrl += '&returnTo=' + encodeURIComponent(_deepLinkReturnTo);
            if (_deepLinkWsId) editUrl += '&wsId=' + _deepLinkWsId;
            window.location.href = editUrl;
            return;
        }

        // Deep-link: redirect to standalone create page if createNew=true was in the URL
        if (_deepLinkCreateNew) {
            var createUrl = '/GrowerDelivery/EditWeightSheetLot';
            if (_deepLinkReturnTo) createUrl += '?returnTo=' + encodeURIComponent(_deepLinkReturnTo);
            if (_deepLinkWsId) createUrl += (_deepLinkReturnTo ? '&' : '?') + 'wsId=' + _deepLinkWsId;
            window.location.href = createUrl;
            return;
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
            FarmNumber:     lot.FarmNumber || null,
        };

        // Push a history entry so the browser Back button leaves edit mode
        // and returns to the list, rather than navigating off the page.
        try {
            history.pushState({ wslEditingLotId: lot.LotId }, '', window.location.pathname + window.location.search);
        } catch (ex) { /* pushState not available — back button will just navigate away */ }

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

    // Ensure _accountsCache has been loaded so we can check existence before
    // trusting a stored AccountId on a lot. Safe to call multiple times.
    async function ensureAccountsCacheLoaded() {
        if (_accountsCache) return;
        try { _accountsCache = await $.getJSON('/api/Lookups/ProducerAccounts'); }
        catch (ex) {
            console.warn('[WeightSheetLots] Account cache load failed', ex);
            _accountsCache = [];
        }
    }

    // True when the account exists in our cache of active producer accounts.
    function isAccountKnown(accountId) {
        if (!accountId) return false;
        if (!_accountsCache) return false;
        for (var i = 0; i < _accountsCache.length; i++) {
            if (_accountsCache[i].AccountId === accountId) return true;
        }
        return false;
    }

    // Clear the Account + Split Group pickers and associated UI state. Used
    // when the SG# shortcut is cleared so we don't leave stale picks tied to
    // a split group the user just deleted.
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

    // True when the split group is present in the given groups list.
    function isSplitGroupInList(groups, splitGroupId) {
        if (!splitGroupId || !groups) return false;
        for (var i = 0; i < groups.length; i++) {
            if (groups[i].SplitGroupId === splitGroupId) return true;
        }
        return false;
    }

    async function populateEditForm(lot, fullEdit) {
        // Need the accounts cache to reliably detect missing/inactive accounts
        // on stored lots before we try to populate pickers from them.
        await ensureAccountsCacheLoaded();

        // SG# shortcut — show current value, enable/disable based on edit mode.
        // Seed _lastSgLookup so a subsequent blur on this field (with the value
        // unchanged) doesn't re-run applySgShortcut and wipe the account pick.
        $(SEL.sgShortcut).val(lot.SplitGroupId || '').prop('disabled', !fullEdit);
        _lastSgLookup = lot.SplitGroupId ? String(lot.SplitGroupId) : '';

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

            // Split group + Account pickers.
            //
            // Strategy: look up the split group directly first. It may be an
            // "override-mode" group (SplitGroup.PrimaryAccountId is NULL) —
            // override lots are bound to their primary account only through
            // LotSplitGroups.PrimaryAccount = 1, which we already resolved on
            // the server into lot.AccountId. For those lots, /SplitGroupsByAccount
            // legitimately returns nothing, so we must NOT treat that as
            // "split group missing".
            var acctInst = dxInstance(SEL.account);
            var sgInst   = dxInstance(SEL.splitGroup);

            var sgLookup = null;
            if (lot.SplitGroupId) {
                try { sgLookup = await $.getJSON('/api/GrowerDelivery/SplitGroupLookup?splitGroupId=' + lot.SplitGroupId); }
                catch (ex) { /* 404 = split group gone/inactive, handled below */ }
            }
            var acctKnown = isAccountKnown(lot.AccountId);

            if (sgLookup && sgLookup.PrimaryAccountId) {
                // ── Normal mode ────────────────────────────────────────────
                // The split group has a concrete primary account. Load groups
                // for that primary and set the dropdown normally.
                _sgOverrideActive     = false;
                _sgOverrideSplitGroup = null;

                if (acctInst) {
                    acctInst.option('dataSource', _accountsCache || []);
                    acctInst.option('disabled', false);
                    acctInst.option('value', acctKnown ? lot.AccountId : null);
                }

                var groups = [];
                try { groups = await $.getJSON('/api/GrowerDelivery/SplitGroupsByAccount?accountId=' + sgLookup.PrimaryAccountId); }
                catch (ex) { console.warn('[WeightSheetLots] SplitGroup load failed', ex); }
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
                // ── Override mode ──────────────────────────────────────────
                // The split group exists but has no primary account — the
                // lot's producer account came from LotSplitGroups. Lock the
                // split group picker to this single entry (same as the SG#
                // shortcut override path) and let the user pick any producer.
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
                // ── Split group missing / inactive ─────────────────────────
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

            // Landlord + Farm Number
            $(SEL.landlord).val(lot.Landlord || '').prop('disabled', false);
            $(SEL.farmNumber).val(lot.FarmNumber || '').prop('disabled', false);

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

            // Account + Split group — both pickers stay disabled in notes-only
            // mode. Same strategy as the full-edit branch: look up the split
            // group directly so override-mode groups (SplitGroup.PrimaryAccountId
            // is NULL) still display the correct values instead of being
            // falsely flagged as "no longer available".
            var acctInst2  = dxInstance(SEL.account);
            var sgInst2    = dxInstance(SEL.splitGroup);
            var acctKnown2 = isAccountKnown(lot.AccountId);

            var sgLookup2 = null;
            if (lot.SplitGroupId) {
                try { sgLookup2 = await $.getJSON('/api/GrowerDelivery/SplitGroupLookup?splitGroupId=' + lot.SplitGroupId); }
                catch (ex) { /* 404 = missing/inactive */ }
            }

            if (sgLookup2 && sgLookup2.PrimaryAccountId) {
                // Normal mode
                var groups2 = [];
                try { groups2 = await $.getJSON('/api/GrowerDelivery/SplitGroupsByAccount?accountId=' + sgLookup2.PrimaryAccountId); }
                catch (ex) { /* ignore */ }
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
                // Override mode — show this single split group, lot.AccountId
                // resolves through LotSplitGroups already.
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
                // Missing / inactive
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

            // Landlord + Farm Number — disabled
            $(SEL.landlord).val(lot.Landlord || '').prop('disabled', true);
            $(SEL.farmNumber).val(lot.FarmNumber || '').prop('disabled', true);

            // Notes — editable
            $(SEL.notes).val(lot.Notes || '');
        }
    }

    // fromPopState: true when the caller is the popstate handler — the browser
    // has already rewound the history stack for us, so we must NOT call
    // history.back() again.
    function exitEditMode(fromPopState) {
        var wasEditing = _editingLotId != null;

        _editingLotId    = null;
        _editingFullEdit = false;
        _editingOriginal = null;

        // Restore panel title and button text
        $(SEL.panelTitle).text('New Weight Sheet Lot');
        $(SEL.saveBtn).text('Create Lot');

        // Re-enable inputs in case they were disabled
        $(SEL.landlord).prop('disabled', false);
        $(SEL.farmNumber).prop('disabled', false);
        $(SEL.sgShortcut).val('').prop('disabled', false);
        $(SEL.sgError).prop('hidden', true).text('');
        _lastSgLookup = '';

        // Hide create panel, show list
        $(SEL.createPanel).prop('hidden', true);
        $(SEL.listPanel).prop('hidden', false);
        $('.gm-gd').addClass('gm-gd--wide');

        // If the user exited via Cancel / Save (not the browser Back button),
        // collapse the history entry we pushed in enterEditMode so the back
        // button doesn't leave the user stuck on an empty list state.
        if (wasEditing && !fromPopState) {
            if (history.state && history.state.wslEditingLotId) {
                try { history.back(); } catch (ex) { /* ignore */ }
            }
        }
    }

    // Browser Back button while in edit mode: exit edit mode instead of
    // navigating away from the page.
    window.addEventListener('popstate', function () {
        if (_editingLotId != null) {
            exitEditMode(true);
        }
    });

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

                // Override mode: keep the manually entered SG# locked; do NOT reset
                // or reload the split group picker from the chosen account. Once a
                // producer is chosen, the "please select any active producer account"
                // warning is satisfied — hide it.
                if (_sgOverrideActive) {
                    if (e.value) {
                        $(SEL.splitGroupWarn).prop('hidden', true);
                    }
                    updateLotDescription();
                    return;
                }

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
                    // In override mode, don't tear down the SG# the user already locked in.
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
        catch (ex) { console.warn('[WeightSheetLots] SplitGroup load failed', ex); }
        const sgInst = dxInstance(SEL.splitGroup);
        if (sgInst) {
            sgInst.option('dataSource', groups);
            sgInst.option('disabled', groups.length === 0);
            sgInst.option('placeholder', 'Select split group\u2026');
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

    // ── Item picker ────────────────────────────────────────────────────────

    async function initItemPicker() {
        if (!_allItemsCache) {
            try { _allItemsCache = await $.getJSON('/api/Lookups/WarehouseItems'); }
            catch (ex) {
                console.warn('[WeightSheetLots] Items load failed', ex);
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
                    // Filter accounts by item + location, then enable picker
                    var accounts = _accountsCache || [];
                    if (acctInst) {
                        acctInst.reset();
                        setPickerLoading(SEL.account, true);
                        if (currentLocationId) {
                            try {
                                accounts = await $.getJSON('/api/Lookups/ProducerAccountsForItem?itemId=' + e.value + '&locationId=' + currentLocationId);
                            } catch (ex) {
                                console.warn('[WeightSheetLots] Filtered accounts load failed', ex);
                            }
                        }
                        acctInst.option('dataSource', accounts);
                        setPickerLoading(SEL.account, false);
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
            onClick: function () {
                promptNewLot();
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

        // SG# shortcut — on Enter or blur, look up the split group.
        // Enter + the subsequent blur both fire, and edit mode also seeds the
        // input programmatically, so we use the module-scoped _lastSgLookup
        // to guard against re-running the lookup on an unchanged value.
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
            // User is editing again — allow the next lookup to fire.
            _lastSgLookup = '';

            // If they cleared the field entirely, drop the current Account +
            // Split Group selections so the form doesn't keep a stale pick
            // that no longer corresponds to any SG#.
            if (!($(SEL.sgShortcut).val() || '').trim()) {
                clearAccountAndSplitGroup();
            }
        });

        // Search button — opens the split group picker popup.
        $(SEL.sgSearchBtn).on('click', function () {
            openSplitGroupPicker();
        });
    }

    // ── Split group search popup ─────────────────────────────────────────────

    // Cache of all split groups and per-group detail (percent rows). Loaded
    // lazily the first time the search popup is opened.
    let _allSplitGroupsCache = null;
    let _sgDetailCache       = {};
    let _sgPopupInitialized  = false;

    async function openSplitGroupPicker() {
        if (!_sgPopupInitialized) {
            initSplitGroupPickerPopup();
            _sgPopupInitialized = true;
        }

        // Load the cache on first open; after that we just re-show the popup.
        if (!_allSplitGroupsCache) {
            try {
                _allSplitGroupsCache = await $.getJSON('/api/SplitGroups');
            } catch (ex) {
                console.warn('[WeightSheetLots] AllSplitGroups load failed', ex);
                _allSplitGroupsCache = [];
            }
            var grid = $(SEL.sgPickerPopup + ' .gm-wsl-sg-grid').dxDataGrid('instance');
            if (grid) grid.option('dataSource', _allSplitGroupsCache);
        }

        $(SEL.sgPickerPopup).dxPopup('instance').show();
    }

    function initSplitGroupPickerPopup() {
        // Build the popup content: a dxDataGrid with master-detail for percents.
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
                    // Highlight rows where the primary account is unassigned.
                    onRowPrepared: function (e) {
                        if (e.rowType === 'data' && e.data && e.data.PrimaryAccountId == null) {
                            $(e.rowElement).addClass('gm-sg-unassigned');
                        }
                    },
                    columns: [
                        {
                            dataField: 'SplitGroupId',
                            caption:   'SG #',
                            width:     90,
                            alignment: 'left',
                        },
                        {
                            dataField: 'SplitGroupDescription',
                            caption:   'Description',
                        },
                        {
                            dataField: 'PrimaryAccountAs400Id',
                            caption:   'Primary Acct #',
                            width:     130,
                            alignment: 'left',
                        },
                        {
                            dataField: 'PrimaryAccountName',
                            caption:   'Primary Account',
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

                            // Create an empty dxDataGrid, then populate from the
                            // detail endpoint (with caching).
                            var detailGridOptions = {
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
                            };
                            $detail.dxDataGrid(detailGridOptions);

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
                                        console.warn('[WeightSheetLots] SplitGroupPercents load failed', ex);
                                    });
                            }
                        },
                    },
                    // Double-click a row to select it.
                    onRowDblClick: function (e) {
                        if (e.rowType === 'data' && e.data) {
                            applySplitGroupPickerSelection(e.data);
                        }
                    },
                    // Single-click select + Select toolbar button.
                    selection: { mode: 'single' },
                    onToolbarPreparing: function (e) {
                        e.toolbarOptions.items.unshift({
                            location: 'after',
                            widget:   'dxButton',
                            options: {
                                text:         'Select',
                                icon:         'check',
                                stylingMode:  'contained',
                                type:         'default',
                                onClick: function () {
                                    var gridInst = e.component;
                                    var selected = gridInst.getSelectedRowsData();
                                    if (selected && selected.length) {
                                        applySplitGroupPickerSelection(selected[0]);
                                    }
                                },
                            },
                        });
                    },
                });
            },
        });
    }

    // Apply a chosen split group from the popup: auto-populate the SG# input,
    // close the popup, and trigger the same lookup the user would get by typing.
    function applySplitGroupPickerSelection(row) {
        if (!row || !row.SplitGroupId) return;
        $(SEL.sgShortcut).val(row.SplitGroupId);
        // Reset the dedup guard so applySgShortcut actually fires for this id.
        // (It's declared inside wireButtons closure; setting the input value
        // above + calling applySgShortcut directly here is enough.)
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

        // Mark this value as "already processed" so a subsequent blur (or the
        // picker-popup selection path that calls us directly) doesn't re-run.
        _lastSgLookup = String(sgId);

        try {
            var sg = await $.getJSON('/api/GrowerDelivery/SplitGroupLookup?splitGroupId=' + sgId);

            _populating = true;
            try {
                var acctInst = dxInstance(SEL.account);
                var sgInst   = dxInstance(SEL.splitGroup);

                if (sg.PrimaryAccountId) {
                    // ── Normal path: split group has a primary account ─────────────
                    _sgOverrideActive     = false;
                    _sgOverrideSplitGroup = null;
                    $(SEL.splitGroupWarn).text('No split group set up for this account.');

                    if (acctInst) {
                        acctInst.option('disabled', false);
                        acctInst.option('value', sg.PrimaryAccountId);
                    }

                    // Load split groups for this account and set the value
                    var groups = [];
                    try { groups = await $.getJSON('/api/GrowerDelivery/SplitGroupsByAccount?accountId=' + sg.PrimaryAccountId); }
                    catch (ex) { console.warn('[WeightSheetLots] SplitGroup load failed', ex); }

                    if (sgInst) {
                        sgInst.option('dataSource', groups);
                        sgInst.option('disabled', false);
                        sgInst.option('value', sg.SplitGroupId);
                    }

                    $(SEL.splitGroupWarn).prop('hidden', groups.length > 0);
                } else {
                    // ── Override path: no primary account on this split group ──────
                    // Let the user pick any active producer; the selected account will
                    // be sent as OverrideAccountId on create.
                    _sgOverrideActive     = true;
                    _sgOverrideSplitGroup = {
                        SplitGroupId:          sg.SplitGroupId,
                        SplitGroupDescription: sg.SplitGroupDescription,
                    };

                    // Lock the split group picker to this single entry (display only).
                    if (sgInst) {
                        sgInst.option('dataSource', [_sgOverrideSplitGroup]);
                        sgInst.option('value', sg.SplitGroupId);
                        sgInst.option('disabled', true);
                    }

                    // Enable account picker with all producer accounts; clear selection.
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

            // Clear both pickers so the user isn't left with stale selections
            // that don't match the SG# they just typed. Also drop any override
            // state from a previous successful lookup.
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

        // Override mode: the server needs an explicit account to mark as primary
        // (SplitPercent=0) since the split group itself has no PrimaryAccountId.
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

        // Prompt for PIN — required to populate CreatedByUserName
        var createPin = await promptForPin();
        if (createPin === null) return; // user cancelled

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
            body.FarmNumber = ($(SEL.farmNumber).val() || '').trim() || null;
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
                     || (body.Landlord || null) !== (orig.Landlord || null)
                     || (body.FarmNumber || null) !== (orig.FarmNumber || null);
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

    // ── New Lot prompt — lot type → PIN → navigate ────────────────────────

    async function promptNewLot() {
        // Step 1: Ask for lot type
        var lotType = await promptLotType();
        if (!lotType) return; // cancelled

        // Step 2: Ask for PIN
        var pin = await promptForPin();
        if (pin === null) return; // cancelled

        // Navigate to the edit lot page in create mode
        window.location.href = '/GrowerDelivery/EditWeightSheetLot'
            + '?lotType=' + encodeURIComponent(lotType)
            + '&pin=' + encodeURIComponent(pin)
            + '&returnTo=lots';
    }

    function promptLotType() {
        return new Promise(function (resolve) {
            var $overlay = $('<div class="gm-pin-overlay"></div>');
            var $dialog  = $(
                '<div class="gm-pin-dialog">' +
                    '<h5>Select Lot Type</h5>' +
                    '<p class="text-muted small mb-2">What type of lot would you like to create?</p>' +
                    '<div class="d-flex gap-2 mt-3">' +
                        '<button type="button" class="btn btn-success gm-lottype-seed flex-fill" style="font-weight:700;padding:12px;">Seed</button>' +
                        '<button type="button" class="btn btn-warning gm-lottype-warehouse flex-fill" style="font-weight:700;padding:12px;">Warehouse</button>' +
                    '</div>' +
                    '<div class="mt-2 text-center">' +
                        '<button type="button" class="btn btn-outline-secondary btn-sm gm-lottype-cancel">Cancel</button>' +
                    '</div>' +
                '</div>'
            );

            $overlay.append($dialog);
            $('body').append($overlay);

            function close(val) {
                $overlay.remove();
                resolve(val);
            }

            $dialog.find('.gm-lottype-seed').on('click', function () { close('seed'); });
            $dialog.find('.gm-lottype-warehouse').on('click', function () { close('warehouse'); });
            $dialog.find('.gm-lottype-cancel').on('click', function () { close(null); });

            $overlay.on('keydown', function (e) {
                if (e.key === 'Escape') close(null);
            });
        });
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
        // Clear any lingering override-account mode from a previous create attempt.
        _sgOverrideActive     = false;
        _sgOverrideSplitGroup = null;

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
        _lastSgLookup = '';
        $(SEL.splitGroupWarn)
            .text('No split group set up for this account.')
            .prop('hidden', true);
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

    async function printLotLabel(lotId) {
        if (!lotId) return;

        // Step 1: ensure a Lot printer is configured
        try {
            const check = await fetch('/api/printing/has-printer?role=Lot');
            if (!check.ok) {
                showAlert('Failed to check printer configuration.', 'danger');
                return;
            }
            const info = await check.json();
            if (!info.configured) {
                showAlert('No lot printer is configured. Open Printer settings and assign a Lot printer.', 'warning');
                return;
            }
        } catch (ex) {
            showAlert('Network error checking printer: ' + ex.message, 'danger');
            return;
        }

        // Step 2: dispatch the print
        try {
            const resp = await fetch('/api/printing/print-lot-label/' + lotId, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' }
            });
            if (!resp.ok) {
                const detail = await tryParseError(resp);
                showAlert('Print failed: ' + detail, 'danger');
                return;
            }
            showAlert('Lot label sent to printer.', 'success');
        } catch (ex) {
            showAlert('Network error: ' + ex.message, 'danger');
        }
    }

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
