/**
 * LocationItems – popup CRUD for the system.LocationItemFilter join table.
 * Exposes: LocationItems.open(locationId, locationName)
 */
var LocationItems = (function () {
    'use strict';

    var _popup      = null;
    var _grid       = null;
    var _itemBox    = null;
    var _locationId = null;
    var _itemsData  = null;   // cached from API

    /* ── public entry-point ──────────────────────────────────────────── */

    function open(locationId, locationName) {
        _locationId = locationId;

        if (!_popup) {
            _popup = $('#liPopup').dxPopup({
                title:       '',
                showTitle:   true,
                width:       580,
                height:      'auto',
                maxHeight:   '85vh',
                dragEnabled: true,
                showCloseButton: true,
                contentTemplate: buildContent,
                onShown: function () { loadGrid(); },
                onHidden: function () {
                    // Refresh the Filtered Items column in the main grid
                    $.getJSON('/api/locations/AllItemFilters').done(function (data) {
                        _itemFiltersMap = data || {};
                        var grid = $('#locationsGrid').dxDataGrid('instance');
                        if (grid) grid.refresh();
                    });
                },
            }).dxPopup('instance');
        }

        _popup.option('title', 'Items — ' + locationName);
        _popup.show();
    }

    /* ── popup content ───────────────────────────────────────────────── */

    function buildContent(container) {
        var $wrap = $('<div class="li-popup-body">');

        // note
        $wrap.append('<div style="margin-bottom:12px; font-size:0.88rem; color:#666; font-style:italic;">This only applies to warehouse loads, not seed.</div>');

        // alert area
        $wrap.append('<div id="liAlert" class="alert" role="alert" hidden></div>');

        // assigned-items grid
        $wrap.append('<div class="li-section-title">Assigned Items</div>');
        $wrap.append('<div id="liGrid"></div>');

        // add-item section
        $wrap.append('<div class="li-section-title li-section-title--add">Add an Item</div>');
        var $addRow = $('<div class="li-add-row">');
        $addRow.append('<div id="liItem" class="li-add-field"></div>');
        $addRow.append('<div id="liAddBtn"></div>');
        $wrap.append($addRow);

        container.append($wrap);

        // initialise sub-widgets after DOM append
        initGrid();
        initItemPicker();
        initAddButton();
    }

    /* ── grid ─────────────────────────────────────────────────────────── */

    function initGrid() {
        _grid = $('#liGrid').dxDataGrid({
            dataSource:            [],
            keyExpr:               'Id',
            showBorders:           true,
            columnAutoWidth:       true,
            rowAlternationEnabled: true,
            hoverStateEnabled:     true,
            noDataText:            'No items assigned.',
            paging:  { enabled: false },
            sorting: { mode: 'none' },
            columns: [
                { dataField: 'ItemName', caption: 'Item' },
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
                            onClick: function () { removeItem(info.data.Id); }
                        }).appendTo(cell);
                    }
                }
            ],
        }).dxDataGrid('instance');
    }

    function loadGrid() {
        if (!_locationId) return;
        $.getJSON('/api/locations/' + _locationId + '/Items')
            .done(function (data) { _grid.option('dataSource', data); })
            .fail(function ()     { showAlert('Failed to load items.', 'danger'); });
    }

    /* ── item picker ─────────────────────────────────────────────────── */

    function initItemPicker() {
        _itemBox = $('#liItem').dxSelectBox({
            placeholder:          'Select item…',
            displayExpr:          'Name',
            valueExpr:            'ItemId',
            searchEnabled:        true,
            searchExpr:           'Name',
            searchMode:           'contains',
            showDataBeforeSearch: true,
            minSearchLength:      0,
            showClearButton:      true,
            dataSource:           [],
        }).dxSelectBox('instance');

        // load items (cached)
        ensureItems().then(function (data) {
            _itemBox.option('dataSource', data);
        });
    }

    function ensureItems() {
        if (_itemsData) return $.Deferred().resolve(_itemsData).promise();
        return $.getJSON('/api/Lookups/WarehouseItems').then(function (data) {
            _itemsData = data;
            return data;
        });
    }

    /* ── add button ───────────────────────────────────────────────────── */

    function initAddButton() {
        $('#liAddBtn').dxButton({
            text:        'Add',
            icon:        'plus',
            type:        'success',
            stylingMode: 'contained',
            onClick:     addItem,
        });
    }

    function addItem() {
        var itemId = _itemBox.option('value');
        if (!itemId) {
            showAlert('Please select an item first.', 'danger');
            return;
        }

        fetch('/api/locations/' + _locationId + '/Items', {
            method:  'POST',
            headers: { 'Content-Type': 'application/json' },
            body:    JSON.stringify({ ItemId: itemId }),
        })
        .then(function (resp) {
            if (resp.status === 409)
                return resp.json().then(function (j) { showAlert(j.message || 'Already assigned.', 'danger'); });
            if (!resp.ok)
                return resp.json().then(function (j) { showAlert('Error: ' + (j.message || resp.statusText), 'danger'); });

            showAlert('Item added.', 'success');
            _itemBox.option('value', null);
            loadGrid();
        })
        .catch(function (ex) { showAlert('Network error: ' + ex.message, 'danger'); });
    }

    /* ── remove ───────────────────────────────────────────────────────── */

    function removeItem(id) {
        fetch('/api/locations/' + _locationId + '/Items/' + id, { method: 'DELETE' })
            .then(function (resp) {
                if (!resp.ok)
                    return resp.json().then(function (j) { showAlert('Error: ' + (j.message || resp.statusText), 'danger'); });
                loadGrid();
            })
            .catch(function (ex) { showAlert('Network error: ' + ex.message, 'danger'); });
    }

    /* ── helpers ──────────────────────────────────────────────────────── */

    function showAlert(msg, type) {
        var el = $('#liAlert');
        el.removeClass('alert-success alert-danger')
          .addClass('alert alert-' + type)
          .text(msg)
          .prop('hidden', false);
        setTimeout(function () { el.prop('hidden', true); }, 4000);
    }

    /* ── public API ───────────────────────────────────────────────────── */
    return { open: open };

})();
