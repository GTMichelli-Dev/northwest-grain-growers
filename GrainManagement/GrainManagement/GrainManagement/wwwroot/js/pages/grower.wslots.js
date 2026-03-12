(function () {
    'use strict';

    // ── Selectors ────────────────────────────────────────────────────────────

    const SEL = {
        alert:           '#wslAlert',
        listPanel:       '#wslListPanel',
        list:            '#wslList',
        newBtn:          '#wslNewBtn',
        createPanel:     '#wslCreatePanel',
        cancelCreateBtn: '#wslCancelCreateBtn',
        account:         '#wslAccount',
        splitGroup:      '#wslSplitGroup',
        item:            '#wslItem',
        state:           '#wslState',
        county:          '#wslCounty',
        lotDesc:         '#wslLotDesc',
        notes:           '#wslNotes',
        createError:     '#wslCreateError',
        saveBtn:         '#wslSaveBtn',
        filterDesc:      '#wslFilterDesc',
        filterDate:      '#wslFilterDate',
        filterStatus:    '#wslFilterStatus',
        splitGroupWarn:  '#wslSplitGroupWarning',
    };

    const LOCATION_STORAGE_KEY = 'gm_location_id';

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

    // ── Init ─────────────────────────────────────────────────────────────────

    $(function () {
        initLocation();
        initItemPicker();
        initAccountPicker();
        initSplitGroupPicker();
        initStateCountyPickers();
        wireButtons();
        wireFilters();
    });

    // ── Location picker ───────────────────────────────────────────────────────

    async function initLocation() {
        const savedId = parseInt(localStorage.getItem(LOCATION_STORAGE_KEY) || '0', 10) || null;

        let locations = [];
        try {
            locations = await $.getJSON('/api/locations/WarehouseLocationsList');
        } catch (ex) {
            console.warn('[WeightSheetLots] Location prefetch failed', ex);
        }

        $('#wslLocation').dxSelectBox({
            dataSource:   locations,
            valueExpr:    'LocationId',
            displayExpr:  function (item) { return item ? item.Name + ' \u2013 ' + item.LocationId : ''; },
            searchEnabled: true,
            placeholder:  'Select location\u2026',
            width:        'auto',
            value:        savedId,
            onValueChanged: function (e) {
                currentLocationId = e.value || null;
                _locationCountiesData = null;       // invalidate so next create re-fetches
                _locationCountiesLocationId = null;
                if (currentLocationId) {
                    localStorage.setItem(LOCATION_STORAGE_KEY, String(currentLocationId));
                    showListPanel();
                    refreshLots();
                } else {
                    localStorage.removeItem(LOCATION_STORAGE_KEY);
                    hidePanels();
                }
            }
        });

        if (savedId) {
            currentLocationId = savedId;
            showListPanel();
            refreshLots();
        }
    }

    // ── Filters ──────────────────────────────────────────────────────────────

    function wireFilters() {
        $(SEL.filterDesc).on('input', applyFilters);
        $(SEL.filterDate).on('change', applyFilters);
        $(SEL.filterStatus).on('change', applyFilters);
    }

    function applyFilters() {
        var descTerm   = ($(SEL.filterDesc).val() || '').toLowerCase().trim();
        var dateTerm   = $(SEL.filterDate).val() || '';   // yyyy-mm-dd or empty
        var statusTerm = $(SEL.filterStatus).val() || 'open';

        var filtered = _lotsCache.filter(function (lot) {
            // Status filter
            if (statusTerm === 'open'   && !lot.IsOpen) return false;
            if (statusTerm === 'closed' &&  lot.IsOpen) return false;

            // Description filter (case-insensitive substring)
            if (descTerm && (lot.LotDescription || '').toLowerCase().indexOf(descTerm) === -1) return false;

            // Date filter (compare MM/dd/yyyy from API to yyyy-mm-dd input)
            if (dateTerm && lot.CreatedAt) {
                var parts = lot.CreatedAt.split('/'); // MM/dd/yyyy
                if (parts.length === 3) {
                    var lotDate = parts[2] + '-' + parts[0].padStart(2, '0') + '-' + parts[1].padStart(2, '0');
                    if (lotDate !== dateTerm) return false;
                }
            }

            return true;
        });

        renderLots(filtered);
    }

    // ── Lot list ──────────────────────────────────────────────────────────────

    async function refreshLots() {
        if (!currentLocationId) return;
        $(SEL.list).html('<span class="text-muted small fst-italic">Loading\u2026</span>');

        try {
            _lotsCache = await $.getJSON('/api/GrowerDelivery/WeightSheetLots?locationId=' + currentLocationId);
        } catch (ex) {
            $(SEL.list).html('<span class="text-danger small">Failed to load lots.</span>');
            _lotsCache = [];
            return;
        }

        applyFilters();
    }

    function renderLots(lots) {
        const list = $(SEL.list);

        if (!lots.length) {
            list.html(
                '<div class="gm-gd-ws-empty">' +
                    '<span class="text-muted small">No lots match the current filters.</span>' +
                '</div>'
            );
            return;
        }

        const rows = lots.map(function (lot) {
            var desc     = escapeHtml(lot.LotDescription || '\u2014');
            var sg       = escapeHtml(lot.SplitGroupDescription || '\u2014');
            var itemName = escapeHtml(lot.ItemDescription || '');
            var notes    = escapeHtml(lot.Notes || '');
            var date     = escapeHtml(lot.CreatedAt || '');
            var state    = escapeHtml(lot.State || '');
            var county   = escapeHtml(lot.County || '');
            var isOpen   = lot.IsOpen;
            var hasClosedWs = lot.HasClosedWeightSheet;

            // Edit rules: full edit if open AND no closed WS; otherwise notes-only
            var canFullEdit = isOpen && !hasClosedWs;
            var editLabel   = canFullEdit ? 'Edit' : 'Edit Notes';

            return (
                '<div class="gm-wsl-card' + (isOpen ? '' : ' gm-wsl-row--closed') + '" data-id="' + lot.Id + '">' +
                    '<div class="gm-wsl-card__body">' +
                        '<div class="gm-wsl-card__id">' + lot.Id + '</div>' +
                        '<div class="gm-wsl-card__info">' +
                            '<div class="gm-wsl-card__lot">' + desc + '</div>' +
                            (itemName ? '<div class="gm-wsl-card__item">' + itemName + '</div>' : '') +
                            '<div class="gm-wsl-card__sg">' + (lot.SplitGroupId ? lot.SplitGroupId + ' - ' : '') + sg + '</div>' +
                            (state || county ? '<div class="gm-wsl-card__location">' + (state ? state : '') + (state && county ? ' \u2013 ' : '') + (county ? county : '') + '</div>' : '') +
                            (notes ? '<div class="gm-wsl-card__notes">' + notes + '</div>' : '') +
                        '</div>' +
                        '<div class="gm-wsl-card__right">' +
                            '<div class="gm-wsl-card__date">' + date + '</div>' +
                            '<span class="gm-wsl-card__status' + (isOpen ? ' gm-wsl-card__status--open' : ' gm-wsl-card__status--closed') + '">' +
                                (isOpen ? 'Open' : 'Closed') +
                            '</span>' +
                        '</div>' +
                    '</div>' +
                    '<div class="gm-wsl-card__actions">' +
                        '<button type="button" class="btn btn-sm btn-outline-primary gm-wsl-edit-btn" ' +
                            'data-id="' + lot.Id + '" data-full="' + (canFullEdit ? '1' : '0') + '">' +
                            editLabel +
                        '</button>' +
                        '<button type="button" class="btn btn-sm ' +
                            (isOpen ? 'btn-outline-danger gm-wsl-close-btn' : 'btn-outline-success gm-wsl-open-btn') +
                            '" data-id="' + lot.Id + '">' +
                            (isOpen ? 'Close' : 'Re-open') +
                        '</button>' +
                    '</div>' +
                '</div>'
            );
        }).join('');

        list.html(rows);

        // Wire close/open buttons
        list.find('.gm-wsl-close-btn').on('click', async function () {
            await toggleLot($(this).data('id'), 'close');
        });
        list.find('.gm-wsl-open-btn').on('click', async function () {
            await toggleLot($(this).data('id'), 'open');
        });

        // Wire edit buttons
        list.find('.gm-wsl-edit-btn').on('click', function () {
            var lotId    = $(this).data('id');
            var fullEdit = $(this).data('full') === 1 || $(this).data('full') === '1';
            var lotData  = findLotById(lotId);
            if (lotData) enterEditMode(lotData, fullEdit);
        });
    }

    function findLotById(id) {
        for (var i = 0; i < _lotsCache.length; i++) {
            if (_lotsCache[i].Id === id) return _lotsCache[i];
        }
        return null;
    }

    // ── Inline editing ───────────────────────────────────────────────────────

    async function enterEditMode(lot, fullEdit) {
        // Pre-fetch all items if needed for full edit
        if (fullEdit && !_allItemsCache) {
            try { _allItemsCache = await $.getJSON('/api/Lookups/WarehouseItems'); }
            catch (ex) {
                console.warn('[WeightSheetLots] Items load failed', ex);
                _allItemsCache = [];
            }
        }

        // Pre-fetch state/counties data
        await loadStateCountiesData();

        // Resolve account-filtered item list for the Item picker
        var editItems = _allItemsCache || [];
        if (fullEdit && lot.AccountId) {
            try {
                var filters = await $.getJSON('/api/AccountItemFilters?accountId=' + lot.AccountId);
                if (filters.length > 0) {
                    var allowedIds = {};
                    for (var i = 0; i < filters.length; i++) { allowedIds[filters[i].ItemId] = true; }
                    editItems = (_allItemsCache || []).filter(function (it) { return allowedIds[it.ItemId]; });
                }
            } catch (ex) { console.warn('[WeightSheetLots] AccountItemFilters load failed', ex); }
        }

        var card = $(SEL.list).find('.gm-wsl-card[data-id="' + lot.Id + '"]');
        if (!card.length) return;

        card.addClass('gm-wsl-card--editing');

        // Account and Split Group are always read-only context
        var acctName = escapeHtml(lot.AccountName && lot.AccountName.trim() ? lot.AccountName : (lot.SplitGroupDescription || '\u2014'));
        var sgName   = escapeHtml(lot.SplitGroupDescription || '\u2014');

        // Build edit form HTML
        var html =
            '<div class="gm-wsl-card__body">' +
                '<div class="gm-wsl-card__id">#' + lot.Id + '</div>' +
                '<div class="gm-wsl-card__info gm-wsl-edit-fields">' +
                    '<div class="gm-wsl-edit-readonly">' +
                        '<span class="gm-wsl-edit-label">Account</span> ' + acctName +
                    '</div>' +
                    '<div class="gm-wsl-edit-readonly">' +
                        '<span class="gm-wsl-edit-label">Split Group</span> ' + sgName +
                    '</div>';

        if (fullEdit) {
            html +=
                '<div class="gm-wsl-edit-field">' +
                    '<label class="gm-wsl-edit-label">Description</label>' +
                    '<input type="text" class="form-control form-control-sm" id="wslEditDesc_' + lot.Id + '" ' +
                        'value="' + escapeAttr(lot.LotDescription || '') + '" />' +
                '</div>' +
                '<div class="gm-wsl-edit-field">' +
                    '<label class="gm-wsl-edit-label">Item</label>' +
                    '<div id="wslEditItem_' + lot.Id + '"></div>' +
                '</div>' +
                '<div class="gm-wsl-edit-field">' +
                    '<label class="gm-wsl-edit-label">State</label>' +
                    '<div id="wslEditState_' + lot.Id + '"></div>' +
                '</div>' +
                '<div class="gm-wsl-edit-field">' +
                    '<label class="gm-wsl-edit-label">County</label>' +
                    '<div id="wslEditCounty_' + lot.Id + '"></div>' +
                '</div>';
        } else {
            if (lot.ItemDescription) {
                html += '<div class="gm-wsl-edit-readonly"><span class="gm-wsl-edit-label">Item</span> ' + escapeHtml(lot.ItemDescription) + '</div>';
            }
            if (lot.State) {
                html += '<div class="gm-wsl-edit-readonly"><span class="gm-wsl-edit-label">State</span> ' + escapeHtml(lot.State) + '</div>';
            }
            if (lot.County) {
                html += '<div class="gm-wsl-edit-readonly"><span class="gm-wsl-edit-label">County</span> ' + escapeHtml(lot.County) + '</div>';
            }
        }

        html +=
                '<div class="gm-wsl-edit-field">' +
                    '<label class="gm-wsl-edit-label">Notes</label>' +
                    '<textarea class="form-control form-control-sm" id="wslEditNotes_' + lot.Id + '" rows="2">' +
                        escapeHtml(lot.Notes || '') +
                    '</textarea>' +
                '</div>' +
            '</div>' +
            '<div class="gm-wsl-card__right">' +
                '<div class="gm-wsl-card__date">' + escapeHtml(lot.CreatedAt || '') + '</div>' +
            '</div>' +
        '</div>' +
        '<div class="gm-wsl-card__actions gm-wsl-card__actions--edit">' +
            '<button type="button" class="btn btn-sm btn-primary gm-wsl-save-edit-btn" data-id="' + lot.Id + '" data-full="' + (fullEdit ? '1' : '0') + '">Save</button>' +
            '<button type="button" class="btn btn-sm btn-outline-secondary gm-wsl-cancel-edit-btn" data-id="' + lot.Id + '">Cancel</button>' +
        '</div>';

        card.html(html);

        // Init item dxSelectBox if full edit — filtered by account
        if (fullEdit) {
            $('#wslEditItem_' + lot.Id).dxSelectBox({
                dataSource:          editItems,
                valueExpr:           'ItemId',
                displayExpr:         'Name',
                searchEnabled:       true,
                searchExpr:          'Name',
                searchMode:          'contains',
                showDataBeforeSearch: true,
                minSearchLength:     0,
                placeholder:         'Select item\u2026',
                showClearButton:     true,
                value:               lot.ItemId || null,
            });

            // Init State/County pickers for edit (always use full list)
            var editStates = (_stateCountiesCache || []).map(function (s) { return { State: s.State, StateName: s.StateName }; });

            $('#wslEditState_' + lot.Id).dxSelectBox({
                dataSource:          editStates,
                valueExpr:           'State',
                displayExpr:         'StateName',
                searchEnabled:       true,
                searchExpr:          'StateName',
                searchMode:          'contains',
                showDataBeforeSearch: true,
                minSearchLength:     0,
                placeholder:         'Select state\u2026',
                showClearButton:     true,
                value:               lot.State || null,
                onValueChanged: function (e) {
                    var cInst = $('#wslEditCounty_' + lot.Id).dxSelectBox('instance');
                    if (cInst) {
                        cInst.reset();
                        cInst.option('dataSource', getCountiesForState(e.value, _stateCountiesCache));
                        cInst.option('disabled', !e.value);
                    }
                },
            });

            $('#wslEditCounty_' + lot.Id).dxSelectBox({
                dataSource:          getCountiesForState(lot.State, _stateCountiesCache),
                valueExpr:           'this',
                displayExpr:         'this',
                searchEnabled:       true,
                searchMode:          'contains',
                showDataBeforeSearch: true,
                minSearchLength:     0,
                placeholder:         'Select county\u2026',
                showClearButton:     true,
                disabled:            !lot.State,
                value:               lot.County || null,
            });
        }

        // Wire save/cancel
        card.find('.gm-wsl-save-edit-btn').on('click', async function () {
            var lotId = $(this).data('id');
            var isFull = $(this).data('full') === 1 || $(this).data('full') === '1';
            await saveEdit(lotId, isFull);
        });

        card.find('.gm-wsl-cancel-edit-btn').on('click', function () {
            applyFilters(); // re-render from cache
        });
    }

    async function saveEdit(lotId, fullEdit) {
        var body = {
            Notes: ($('#wslEditNotes_' + lotId).val() || '').trim() || null,
        };

        if (fullEdit) {
            body.LotDescription = ($('#wslEditDesc_' + lotId).val() || '').trim() || null;
            var itemInst = $('#wslEditItem_' + lotId).dxSelectBox('instance');
            body.ItemId = itemInst ? itemInst.option('value') || null : null;
            var stateInst = $('#wslEditState_' + lotId).dxSelectBox('instance');
            body.State = stateInst ? stateInst.option('value') || null : null;
            var countyInst = $('#wslEditCounty_' + lotId).dxSelectBox('instance');
            body.County = countyInst ? countyInst.option('value') || null : null;
        }

        var saveBtn = $(SEL.list).find('.gm-wsl-save-edit-btn[data-id="' + lotId + '"]');
        saveBtn.prop('disabled', true).text('Saving\u2026');

        try {
            var resp = await fetch('/api/GrowerDelivery/WeightSheetLots/' + lotId, {
                method:  'PATCH',
                headers: { 'Content-Type': 'application/json' },
                body:    JSON.stringify(body),
            });

            if (!resp.ok) {
                var detail = await tryParseError(resp);
                showAlert('Error: ' + detail, 'danger');
                return;
            }

            showAlert('Lot updated.', 'success');
            await refreshLots();
        } catch (ex) {
            showAlert('Network error: ' + ex.message, 'danger');
        } finally {
            saveBtn.prop('disabled', false).text('Save');
        }
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
                updateLotDescription();
            },
            onFocusOut: function (e) {
                if (!e.component.option('value')) {
                    e.component.reset();
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
            onValueChanged: function (e) {
                var acctInst = $(SEL.account).dxSelectBox('instance');
                var sgInst   = $(SEL.splitGroup).dxSelectBox('instance');
                if (e.value) {
                    // Enable account picker
                    if (acctInst) acctInst.option('disabled', false);
                } else {
                    // Clear and disable account + split group
                    if (acctInst) { acctInst.reset(); acctInst.option('disabled', true); }
                    if (sgInst)   { sgInst.reset(); sgInst.option('dataSource', []); sgInst.option('disabled', true); }
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
                    countyInst.reset();
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

        $(SEL.newBtn).on('click', async function () {
            $(SEL.listPanel).prop('hidden', true);
            $(SEL.createPanel).prop('hidden', false);
            await resetCreateForm();
        });

        $(SEL.cancelCreateBtn).on('click', function () {
            $(SEL.createPanel).prop('hidden', true);
            $(SEL.listPanel).prop('hidden', false);
        });

        $(SEL.saveBtn).on('click', async function () {
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
                    }),
                });

                if (!resp.ok) {
                    const detail = await tryParseError(resp);
                    $(SEL.createError).text('Error: ' + detail).prop('hidden', false);
                    return;
                }

                $(SEL.createPanel).prop('hidden', true);
                $(SEL.listPanel).prop('hidden', false);
                refreshLots();
                showAlert('Weight sheet lot created.', 'success');

            } catch (ex) {
                $(SEL.createError).text('Network error: ' + ex.message).prop('hidden', false);
            } finally {
                btn.prop('disabled', false).text('Create Lot');
            }
        });
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    function showListPanel() {
        $(SEL.listPanel).prop('hidden', false);
        $(SEL.createPanel).prop('hidden', true);
    }

    function hidePanels() {
        $(SEL.listPanel).prop('hidden', true);
        $(SEL.createPanel).prop('hidden', true);
    }

    async function resetCreateForm() {
        var itemInst   = dxInstance(SEL.item);
        var acctInst   = dxInstance(SEL.account);
        var sgInst     = dxInstance(SEL.splitGroup);

        // Item resets but stays enabled with full list
        if (itemInst) itemInst.reset();

        // Account and Split Group reset and go disabled
        if (acctInst) { acctInst.reset(); acctInst.option('disabled', true); }
        if (sgInst)   { sgInst.reset(); sgInst.option('dataSource', []); sgInst.option('disabled', true); }

        $(SEL.lotDesc).val('');
        $(SEL.notes).val('');
        $(SEL.splitGroupWarn).prop('hidden', true);
        $(SEL.createError).prop('hidden', true).text('');

        // Load location counties and apply state/county constraints
        await loadLocationCounties();
        applyStateCountyConstraints();
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
