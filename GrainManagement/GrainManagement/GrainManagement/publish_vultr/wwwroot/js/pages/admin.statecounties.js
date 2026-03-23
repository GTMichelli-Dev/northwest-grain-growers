(function () {
    'use strict';

    var _statesGrid   = null;
    var _countiesGrid = null;
    var _selectedStateId   = null;
    var _selectedStateName = '';

    $(function () {
        initStatesGrid();
        initCountiesGrid();
        loadStates();
    });

    // ── States grid ─────────────────────────────────────────────────────────

    function initStatesGrid() {
        _statesGrid = $('#scStatesGrid').dxDataGrid({
            dataSource:            [],
            keyExpr:               'Id',
            showBorders:           true,
            columnAutoWidth:       true,
            rowAlternationEnabled: true,
            hoverStateEnabled:     true,
            noDataText:            'No states found.',
            selection:             { mode: 'single' },
            editing: {
                mode:             'cell',
                allowUpdating:    true,
                selectTextOnEditStart: true,
            },
            sorting: { mode: 'single' },
            columns: [
                { dataField: 'StateName', caption: 'State', allowEditing: false, sortOrder: 'asc', sortIndex: 0 },
                { dataField: 'StateAbv',  caption: 'Abbreviation', width: 160 },
            ],
            onSelectionChanged: function (e) {
                var row = e.selectedRowsData[0];
                if (row) {
                    _selectedStateId   = row.Id;
                    _selectedStateName = row.StateName;
                    $('#scStateName').text(row.StateName);
                    $('#scCountiesCard').prop('hidden', false);
                    loadCounties(row.Id);
                }
            },
            onRowUpdating: function (e) {
                e.cancel = saveStateAbv(e.oldData.Id, e.newData.StateAbv);
            },
        }).dxDataGrid('instance');
    }

    async function loadStates() {
        try {
            var data = await $.getJSON('/api/Lookups/States');
            _statesGrid.option('dataSource', data);
        } catch (ex) {
            showAlert('Failed to load states.', 'danger');
        }
    }

    function saveStateAbv(id, newAbv) {
        if (!newAbv || !newAbv.trim()) {
            showAlert('Abbreviation cannot be empty.', 'danger');
            return true; // cancel
        }

        var promise = fetch('/api/Lookups/States/' + id, {
            method:  'PATCH',
            headers: { 'Content-Type': 'application/json' },
            body:    JSON.stringify({ Abv: newAbv.trim() }),
        }).then(function (resp) {
            if (!resp.ok) {
                return resp.json().then(function (j) {
                    showAlert('Error: ' + (j.message || resp.statusText), 'danger');
                });
            }
            showAlert('State abbreviation updated.', 'success');
        }).catch(function (ex) {
            showAlert('Network error: ' + ex.message, 'danger');
        });

        return promise;
    }

    // ── Counties grid ───────────────────────────────────────────────────────

    function initCountiesGrid() {
        _countiesGrid = $('#scCountiesGrid').dxDataGrid({
            dataSource:            [],
            keyExpr:               'Id',
            showBorders:           true,
            columnAutoWidth:       true,
            rowAlternationEnabled: true,
            hoverStateEnabled:     true,
            noDataText:            'No counties found.',
            searchPanel: {
                visible:     true,
                width:       240,
                placeholder: 'Search counties\u2026',
            },
            editing: {
                mode:             'cell',
                allowUpdating:    true,
                selectTextOnEditStart: true,
            },
            sorting: { mode: 'single' },
            columns: [
                { dataField: 'CountyName', caption: 'County', allowEditing: false, sortOrder: 'asc', sortIndex: 0 },
                { dataField: 'CountyAbv',  caption: 'Abbreviation', width: 160 },
            ],
            onRowUpdating: function (e) {
                e.cancel = saveCountyAbv(e.oldData.Id, e.newData.CountyAbv);
            },
            onContentReady: function (e) {
                var n = e.component.totalCount();
                $('#scCountyCount').text(n ? '(' + n + ')' : '');
            },
        }).dxDataGrid('instance');
    }

    async function loadCounties(stateId) {
        try {
            var data = await $.getJSON('/api/Lookups/Counties?stateId=' + stateId);
            _countiesGrid.option('dataSource', data);
        } catch (ex) {
            showAlert('Failed to load counties.', 'danger');
        }
    }

    function saveCountyAbv(id, newAbv) {
        if (!newAbv || !newAbv.trim()) {
            showAlert('Abbreviation cannot be empty.', 'danger');
            return true; // cancel
        }

        var promise = fetch('/api/Lookups/Counties/' + id, {
            method:  'PATCH',
            headers: { 'Content-Type': 'application/json' },
            body:    JSON.stringify({ Abv: newAbv.trim() }),
        }).then(function (resp) {
            if (!resp.ok) {
                return resp.json().then(function (j) {
                    showAlert('Error: ' + (j.message || resp.statusText), 'danger');
                });
            }
            showAlert('County abbreviation updated.', 'success');
        }).catch(function (ex) {
            showAlert('Network error: ' + ex.message, 'danger');
        });

        return promise;
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    function showAlert(msg, type) {
        var el = $('#scAlert');
        el.removeClass('alert-success alert-danger')
          .addClass('alert alert-' + type)
          .text(msg)
          .prop('hidden', false);
        setTimeout(function () { el.prop('hidden', true); }, 4000);
    }

})();
