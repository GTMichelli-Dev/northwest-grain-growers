/**
 * LocationQuantityMethods – popup CRUD for the Inventory.LocationQuantityMethods join table.
 * Exposes: LocationQuantityMethods.open(locationId, locationName)
 */
var LocationQuantityMethods = (function () {
    'use strict';

    var _popup      = null;
    var _grid       = null;
    var _methodBox  = null;
    var _locationId = null;
    var _allMethods = null;   // cached from API

    /* ── public entry-point ──────────────────────────────────────────── */

    function open(locationId, locationName) {
        _locationId = locationId;

        if (!_popup) {
            _popup = $('#lqmPopup').dxPopup({
                title:       '',
                showTitle:   true,
                width:       580,
                height:      'auto',
                maxHeight:   '85vh',
                dragEnabled: true,
                showCloseButton: true,
                contentTemplate: buildContent,
                onShown: function () { loadGrid(); },
            }).dxPopup('instance');
        }

        _popup.option('title', 'Weight Methods — ' + locationName);
        _popup.show();
    }

    /* ── popup content ───────────────────────────────────────────────── */

    function buildContent(container) {
        var $wrap = $('<div class="lqm-popup-body">');

        // note
        $wrap.append('<div style="margin-bottom:12px; font-size:0.88rem; color:#666; font-style:italic;">Configure which weight methods (Truck Scale, Bulkloader, Rail) are available at this location. Manual is always included.</div>');

        // alert area
        $wrap.append('<div id="lqmAlert" class="alert" role="alert" hidden></div>');

        // assigned-methods grid
        $wrap.append('<div style="font-weight:600; margin-bottom:6px;">Assigned Weight Methods</div>');
        $wrap.append('<div id="lqmGrid"></div>');

        // add-method section
        $wrap.append('<div style="font-weight:600; margin-top:16px; margin-bottom:6px;">Add a Weight Method</div>');
        var $addRow = $('<div style="display:flex; gap:8px; align-items:center;">');
        $addRow.append('<div id="lqmMethod" style="flex:1;"></div>');
        $addRow.append('<div id="lqmAddBtn"></div>');
        $wrap.append($addRow);

        container.append($wrap);

        // initialise sub-widgets after DOM append
        initGrid();
        initMethodPicker();
        initAddButton();
    }

    /* ── grid ─────────────────────────────────────────────────────────── */

    function initGrid() {
        _grid = $('#lqmGrid').dxDataGrid({
            dataSource:            [],
            keyExpr:               'QuantityMethodId',
            showBorders:           true,
            columnAutoWidth:       true,
            rowAlternationEnabled: true,
            hoverStateEnabled:     true,
            noDataText:            'No weight methods assigned.',
            paging:  { enabled: false },
            sorting: { mode: 'none' },
            columns: [
                { dataField: 'Code', caption: 'Code', width: 140 },
                { dataField: 'Description', caption: 'Description' },
                {
                    caption: '',
                    width: 70,
                    alignment: 'center',
                    cellTemplate: function (cell, info) {
                        if (info.data.Code === 'MANUAL') return; // MANUAL cannot be removed
                        $('<div>').dxButton({
                            icon:        'trash',
                            type:        'danger',
                            stylingMode: 'text',
                            hint:        'Remove',
                            onClick: function () { removeMethod(info.data.QuantityMethodId); }
                        }).appendTo(cell);
                    }
                }
            ],
        }).dxDataGrid('instance');
    }

    function loadGrid() {
        if (!_locationId) return;
        $.getJSON('/api/locations/' + _locationId + '/QuantityMethods')
            .done(function (data) { _grid.option('dataSource', data); })
            .fail(function ()     { showAlert('Failed to load weight methods.', 'danger'); });
    }

    /* ── method picker ───────────────────────────────────────────────── */

    function initMethodPicker() {
        _methodBox = $('#lqmMethod').dxSelectBox({
            placeholder:          'Select weight method…',
            displayExpr:          function (item) { return item ? item.Description + ' (' + item.Code + ')' : ''; },
            valueExpr:            'QuantityMethodId',
            searchEnabled:        true,
            searchExpr:           ['Description', 'Code'],
            searchMode:           'contains',
            showDataBeforeSearch: true,
            minSearchLength:      0,
            showClearButton:      true,
            dataSource:           [],
        }).dxSelectBox('instance');

        // load methods (cached), excluding MANUAL since it's always present
        ensureMethods().then(function (data) {
            var filtered = data.filter(function (m) { return m.Code !== 'MANUAL'; });
            _methodBox.option('dataSource', filtered);
        });
    }

    function ensureMethods() {
        if (_allMethods) return $.Deferred().resolve(_allMethods).promise();
        return $.getJSON('/api/locations/AllQuantityMethods').then(function (data) {
            _allMethods = data;
            return data;
        });
    }

    /* ── add button ───────────────────────────────────────────────────── */

    function initAddButton() {
        $('#lqmAddBtn').dxButton({
            text:        'Add',
            icon:        'plus',
            type:        'success',
            stylingMode: 'contained',
            onClick:     addMethod,
        });
    }

    function addMethod() {
        var methodId = _methodBox.option('value');
        if (!methodId) {
            showAlert('Please select a weight method first.', 'danger');
            return;
        }

        fetch('/api/locations/' + _locationId + '/QuantityMethods', {
            method:  'POST',
            headers: { 'Content-Type': 'application/json' },
            body:    JSON.stringify({ QuantityMethodId: methodId }),
        })
        .then(function (resp) {
            if (resp.status === 409)
                return resp.json().then(function (j) { showAlert(j.message || 'Already assigned.', 'danger'); });
            if (!resp.ok)
                return resp.json().then(function (j) { showAlert('Error: ' + (j.message || resp.statusText), 'danger'); });

            showAlert('Weight method added.', 'success');
            _methodBox.option('value', null);
            loadGrid();
        })
        .catch(function (ex) { showAlert('Network error: ' + ex.message, 'danger'); });
    }

    /* ── remove ───────────────────────────────────────────────────────── */

    function removeMethod(methodId) {
        fetch('/api/locations/' + _locationId + '/QuantityMethods/' + methodId, { method: 'DELETE' })
            .then(function (resp) {
                if (!resp.ok)
                    return resp.json().then(function (j) { showAlert('Error: ' + (j.message || resp.statusText), 'danger'); });
                loadGrid();
            })
            .catch(function (ex) { showAlert('Network error: ' + ex.message, 'danger'); });
    }

    /* ── helpers ──────────────────────────────────────────────────────── */

    function showAlert(msg, type) {
        var el = $('#lqmAlert');
        el.removeClass('alert-success alert-danger')
          .addClass('alert alert-' + type)
          .text(msg)
          .prop('hidden', false);
        setTimeout(function () { el.prop('hidden', true); }, 4000);
    }

    /* ── public API ───────────────────────────────────────────────────── */
    return { open: open };

})();
