(function () {
    'use strict';

    // ── Selectors ────────────────────────────────────────────────────────────

    const SEL = {
        alert:       '#aifAlert',
        item:        '#aifItem',
        account:     '#aifAccount',
        location:    '#aifLocation',
        addBtn:      '#aifAddBtn',
        clearBtn:    '#aifClearBtn',
        addError:    '#aifAddError',
        list:        '#aifList',
        count:       '#aifCount',
    };

    // ── State ────────────────────────────────────────────────────────────────

    let _itemsCache      = null;
    let _accountsCache   = null;
    let _locationsCache  = null;

    let _selectedItemId      = null;
    let _selectedAccountId   = null;
    let _selectedLocationId  = null;

    let _grid = null;

    // ── Init ─────────────────────────────────────────────────────────────────

    $(async function () {
        await Promise.all([
            initItemPicker(),
            initAccountPicker(),
            initLocationPicker(),
        ]);
        wireButtons();
        initGrid();
        refreshList();   // load all records immediately on page open
    });

    // ── Item picker — all warehouse items ─────────────────────────────────────

    async function initItemPicker() {
        if (!_itemsCache) {
            try { _itemsCache = await $.getJSON('/api/Lookups/WarehouseItems'); }
            catch (ex) {
                console.warn('[AccountItemFilters] Items load failed', ex);
                _itemsCache = [];
            }
        }

        $(SEL.item).dxSelectBox({
            dataSource:           _itemsCache,
            valueExpr:            'ItemId',
            displayExpr:          'Name',
            searchEnabled:        true,
            searchExpr:           'Name',
            searchMode:           'contains',
            showDataBeforeSearch: true,
            minSearchLength:      0,
            placeholder:          'Select item…',
            showClearButton:      true,
            onValueChanged: function (e) {
                _selectedItemId = e.value || null;
                refreshAddButton();
            },
            onFocusOut: function (e) {
                if (!e.component.option('value')) { e.component.reset(); }
            }
        });
    }

    // ── Account picker ────────────────────────────────────────────────────────

    async function initAccountPicker() {
        if (!_accountsCache) {
            try { _accountsCache = await $.getJSON('/api/Lookups/ProducerAccounts'); }
            catch (ex) {
                console.warn('[AccountItemFilters] Account load failed', ex);
                _accountsCache = [];
            }
        }

        $(SEL.account).dxSelectBox({
            dataSource:           _accountsCache,
            valueExpr:            'AccountId',
            displayExpr:          'Name',
            searchEnabled:        true,
            searchExpr:           'Name',
            searchMode:           'contains',
            showDataBeforeSearch: true,
            minSearchLength:      0,
            placeholder:          'Filter by account…',
            showClearButton:      true,
            onValueChanged: function (e) {
                _selectedAccountId = e.value || null;
                refreshAddButton();
            },
            onFocusOut: function (e) {
                if (!e.component.option('value')) { e.component.reset(); }
            }
        });
    }

    // ── Location picker ────────────────────────────────────────────────────────

    const ALL_LOCATIONS_ID = 0;

    async function initLocationPicker() {
        if (!_locationsCache) {
            try { _locationsCache = await $.getJSON('/api/locations/WarehouseLocationsList'); }
            catch (ex) {
                console.warn('[AccountItemFilters] Locations load failed', ex);
                _locationsCache = [];
            }
        }

        // Prepend "All Locations" as the first option
        var pickerData = [{ LocationId: ALL_LOCATIONS_ID, Name: '— All Locations —' }].concat(_locationsCache);

        $(SEL.location).dxSelectBox({
            dataSource:           pickerData,
            valueExpr:            'LocationId',
            displayExpr:          'Name',
            searchEnabled:        true,
            searchExpr:           'Name',
            searchMode:           'contains',
            showDataBeforeSearch: true,
            minSearchLength:      0,
            placeholder:          'Select location…',
            showClearButton:      true,
            onValueChanged: function (e) {
                _selectedLocationId = (e.value != null) ? e.value : null;
                refreshAddButton();
            },
            onFocusOut: function (e) {
                if (e.component.option('value') == null) { e.component.reset(); }
            }
        });
    }

    // ── Results grid ──────────────────────────────────────────────────────────

    function initGrid() {
        _grid = $(SEL.list).dxDataGrid({
            dataSource:            [],
            keyExpr:               'Id',
            showBorders:           true,
            columnAutoWidth:       true,
            rowAlternationEnabled: true,
            hoverStateEnabled:     true,
            noDataText:            'No filters found.',
            searchPanel: {
                visible:     true,
                width:       240,
                placeholder: 'Search filters…',
            },
            toolbar: {
                items: [
                    { name: 'searchPanel', location: 'after' },
                    {
                        location: 'after',
                        template: function () {
                            return $('<div>').css({ display: 'flex', alignItems: 'center', gap: '8px' })
                                .append($('<span>').text('Paging').css({ fontWeight: 600 }))
                                .append($('<div>').dxSwitch({
                                    value: true,
                                    hint: 'Turn paging on/off',
                                    onValueChanged: function (e) {
                                        _grid.option('paging.enabled', e.value);
                                        _grid.option('pager.visible', e.value);
                                    }
                                }));
                        }
                    }
                ]
            },
            sorting: { mode: 'single' },
            columns: [
                { dataField: 'AccountName',        caption: 'Account', sortOrder: 'asc', sortIndex: 0 },
                { dataField: 'ItemDescription',    caption: 'Item' },
                { dataField: 'LocationName',       caption: 'Location' },
                {
                    type:  'buttons',
                    width: 90,
                    buttons: [{
                        text:     'Remove',
                        cssClass: 'btn btn-sm btn-outline-danger',
                        onClick:  function (e) { deleteFilter(e.row.data.Id); },
                    }],
                },
            ],
            onContentReady: function (e) {
                const n = e.component.totalCount();
                $(SEL.count).text(n ? '(' + n + ')' : '');
            },
        }).dxDataGrid('instance');
    }

    // ── Add / Clear buttons ───────────────────────────────────────────────────

    function wireButtons() {

        $(SEL.addBtn).on('click', async function () {
            if (!_selectedItemId || !_selectedAccountId || _selectedLocationId == null) return;

            $(SEL.addError).prop('hidden', true).text('');
            const btn = $(SEL.addBtn).prop('disabled', true).text('Adding…');

            try {
                var url, body;

                if (_selectedLocationId === ALL_LOCATIONS_ID) {
                    // Bulk — add filters for ALL warehouse locations
                    url  = '/api/AccountItemFilters/bulk';
                    body = JSON.stringify({ AccountId: _selectedAccountId, ItemId: _selectedItemId });
                } else {
                    // Single location
                    url  = '/api/AccountItemFilters';
                    body = JSON.stringify({ AccountId: _selectedAccountId, ItemId: _selectedItemId, LocationId: _selectedLocationId });
                }

                const resp = await fetch(url, {
                    method:  'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body:    body,
                });

                if (!resp.ok) {
                    const detail = await tryParseError(resp);
                    $(SEL.addError).text('Error: ' + detail).prop('hidden', false);
                    return;
                }

                if (_selectedLocationId === ALL_LOCATIONS_ID) {
                    const result = await resp.json();
                    showAlert('Filters added for ' + result.added + ' location(s).', 'success');
                } else {
                    showAlert('Filter added successfully.', 'success');
                }

                // Reset account picker so the user can add another account to the same item
                const accountInst = $(SEL.account).dxSelectBox('instance');
                if (accountInst) { accountInst.reset(); }
                _selectedAccountId = null;

                refreshList();

            } catch (ex) {
                $(SEL.addError).text('Network error: ' + ex.message).prop('hidden', false);
            } finally {
                btn.prop('disabled', false).text('Add Filter');
                refreshAddButton();
            }
        });

        $(SEL.clearBtn).on('click', function () {
            const itemInst     = $(SEL.item).dxSelectBox('instance');
            const accountInst  = $(SEL.account).dxSelectBox('instance');
            const locationInst = $(SEL.location).dxSelectBox('instance');

            if (itemInst)     itemInst.reset();
            if (accountInst)  accountInst.reset();
            if (locationInst) locationInst.reset();

            _selectedItemId      = null;
            _selectedAccountId   = null;
            _selectedLocationId  = null;

            $(SEL.addError).prop('hidden', true).text('');
            refreshAddButton();
        });
    }

    // ── List / refresh ────────────────────────────────────────────────────────

    async function refreshList() {
        let rows = [];
        try {
            rows = await $.getJSON('/api/AccountItemFilters');
        } catch (ex) {
            showAlert('Failed to load filters.', 'danger');
            return;
        }

        if (_grid) {
            _grid.option('dataSource', rows);
        }
    }

    async function deleteFilter(id) {
        try {
            const resp = await fetch('/api/AccountItemFilters/' + encodeURIComponent(id), {
                method: 'DELETE',
            });
            if (!resp.ok) {
                const detail = await tryParseError(resp);
                showAlert('Error: ' + detail, 'danger');
                return;
            }
            showAlert('Filter removed.', 'success');
            refreshList();
        } catch (ex) {
            showAlert('Network error: ' + ex.message, 'danger');
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    function refreshAddButton() {
        $(SEL.addBtn).prop('disabled', !_selectedItemId || !_selectedAccountId || _selectedLocationId == null);
    }

    function showAlert(msg, type) {
        const el = $(SEL.alert);
        el.removeClass('alert-success alert-danger')
          .addClass('alert alert-' + type)
          .text(msg)
          .prop('hidden', false);
        setTimeout(function () { el.prop('hidden', true); }, 4000);
    }

    async function tryParseError(resp) {
        try {
            const j = await resp.json();
            return j.message || j.title || resp.statusText;
        } catch { return resp.statusText; }
    }

})();
