/**
 * LocationCounties – popup CRUD for the system.LocationCounties join table.
 * Exposes: LocationCounties.open(locationId, locationName)
 */
var LocationCounties = (function () {
    'use strict';

    var _popup       = null;
    var _grid        = null;
    var _stateBox    = null;
    var _countyBox   = null;
    var _locationId  = null;
    var _statesData  = null;   // cached from API

    /* ── public entry-point ──────────────────────────────────────────── */

    function open(locationId, locationName) {
        _locationId = locationId;

        if (!_popup) {
            _popup = $('#lcPopup').dxPopup({
                title:       '',
                showTitle:   true,
                width:       680,
                height:      'auto',
                maxHeight:   '85vh',
                dragEnabled: true,
                showCloseButton: true,
                contentTemplate: buildContent,
                onShown: function () { loadGrid(); },
            }).dxPopup('instance');
        }

        _popup.option('title', 'Counties — ' + locationName);
        _popup.show();
    }

    /* ── popup content ───────────────────────────────────────────────── */

    function buildContent(container) {
        var $wrap = $('<div class="lc-popup-body">');

        // alert area
        $wrap.append('<div id="lcAlert" class="alert" role="alert" hidden></div>');

        // assigned-counties grid
        $wrap.append('<div class="lc-section-title">Assigned Counties</div>');
        $wrap.append('<div id="lcGrid"></div>');

        // add-county section
        $wrap.append('<div class="lc-section-title lc-section-title--add">Add a County</div>');
        var $addRow = $('<div class="lc-add-row">');
        $addRow.append('<div id="lcState" class="lc-add-field"></div>');
        $addRow.append('<div id="lcCounty" class="lc-add-field"></div>');
        $addRow.append('<div id="lcAddBtn"></div>');
        $wrap.append($addRow);

        container.append($wrap);

        // initialise sub-widgets (grid, pickers, button) after DOM append
        initGrid();
        initStatePicker();
        initCountyPicker();
        initAddButton();
    }

    /* ── grid ─────────────────────────────────────────────────────────── */

    function initGrid() {
        _grid = $('#lcGrid').dxDataGrid({
            dataSource:            [],
            keyExpr:               'Id',
            showBorders:           true,
            columnAutoWidth:       true,
            rowAlternationEnabled: true,
            hoverStateEnabled:     true,
            noDataText:            'No counties assigned.',
            paging:  { enabled: false },
            sorting: { mode: 'none' },
            columns: [
                { dataField: 'StateName',  caption: 'State',  width: 160 },
                { dataField: 'CountyName', caption: 'County' },
                {
                    caption: '',
                    width: 70,
                    alignment: 'center',
                    cellTemplate: function (cell, info) {
                        $('<div>').dxButton({
                            icon:        'trash',
                            type:        'danger',
                            stylingMode: 'text',
                            hint:        'Remove',
                            onClick: function () { removeCounty(info.data.Id); }
                        }).appendTo(cell);
                    }
                }
            ],
        }).dxDataGrid('instance');
    }

    function loadGrid() {
        if (!_locationId) return;
        $.getJSON('/api/locations/' + _locationId + '/Counties')
            .done(function (data) { _grid.option('dataSource', data); })
            .fail(function ()     { showAlert('Failed to load counties.', 'danger'); });
    }

    /* ── state picker ─────────────────────────────────────────────────── */

    function initStatePicker() {
        _stateBox = $('#lcState').dxSelectBox({
            placeholder:   'Select state…',
            displayExpr:   'StateName',
            valueExpr:     'Id',
            searchEnabled: true,
            dataSource:    [],
            onValueChanged: function (e) {
                if (e.value) loadCountiesForState(e.value);
                else         resetCountyPicker();
            }
        }).dxSelectBox('instance');

        // load states (cached)
        ensureStates().then(function (data) {
            _stateBox.option('dataSource', data);
        });
    }

    function ensureStates() {
        if (_statesData) return $.Deferred().resolve(_statesData).promise();
        return $.getJSON('/api/Lookups/States').then(function (data) {
            _statesData = data;
            return data;
        });
    }

    /* ── county picker ────────────────────────────────────────────────── */

    function initCountyPicker() {
        _countyBox = $('#lcCounty').dxSelectBox({
            placeholder:   'Select county…',
            displayExpr:   'CountyName',
            valueExpr:     'Id',
            searchEnabled: true,
            dataSource:    [],
            disabled:      true,
        }).dxSelectBox('instance');
    }

    function loadCountiesForState(stateId) {
        $.getJSON('/api/Lookups/Counties?stateId=' + stateId)
            .done(function (data) {
                _countyBox.option('dataSource', data);
                _countyBox.option('disabled', false);
                _countyBox.option('value', null);
            })
            .fail(function () { showAlert('Failed to load counties.', 'danger'); });
    }

    function resetCountyPicker() {
        _countyBox.option('dataSource', []);
        _countyBox.option('value', null);
        _countyBox.option('disabled', true);
    }

    /* ── add button ───────────────────────────────────────────────────── */

    function initAddButton() {
        $('#lcAddBtn').dxButton({
            text:        'Add',
            icon:        'plus',
            type:        'success',
            stylingMode: 'contained',
            onClick:     addCounty,
        });
    }

    function addCounty() {
        var countyId = _countyBox.option('value');
        if (!countyId) {
            showAlert('Please select a state and county first.', 'danger');
            return;
        }

        fetch('/api/locations/' + _locationId + '/Counties', {
            method:  'POST',
            headers: { 'Content-Type': 'application/json' },
            body:    JSON.stringify({ CountyId: countyId }),
        })
        .then(function (resp) {
            if (resp.status === 409)
                return resp.json().then(function (j) { showAlert(j.message || 'Already assigned.', 'danger'); });
            if (!resp.ok)
                return resp.json().then(function (j) { showAlert('Error: ' + (j.message || resp.statusText), 'danger'); });

            showAlert('County added.', 'success');
            _stateBox.option('value', null);
            resetCountyPicker();
            loadGrid();
        })
        .catch(function (ex) { showAlert('Network error: ' + ex.message, 'danger'); });
    }

    /* ── remove ───────────────────────────────────────────────────────── */

    function removeCounty(id) {
        fetch('/api/locations/' + _locationId + '/Counties/' + id, { method: 'DELETE' })
            .then(function (resp) {
                if (!resp.ok)
                    return resp.json().then(function (j) { showAlert('Error: ' + (j.message || resp.statusText), 'danger'); });
                loadGrid();
            })
            .catch(function (ex) { showAlert('Network error: ' + ex.message, 'danger'); });
    }

    /* ── helpers ──────────────────────────────────────────────────────── */

    function showAlert(msg, type) {
        var el = $('#lcAlert');
        el.removeClass('alert-success alert-danger')
          .addClass('alert alert-' + type)
          .text(msg)
          .prop('hidden', false);
        setTimeout(function () { el.prop('hidden', true); }, 4000);
    }

    /* ── public API ───────────────────────────────────────────────────── */
    return { open: open };

})();
