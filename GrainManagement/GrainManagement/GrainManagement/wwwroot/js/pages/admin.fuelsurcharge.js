(function () {
    'use strict';

    var _grid;
    var _allData = [];
    var _selectedDistrictId = null;

    function showAlert(msg, type) {
        var $el = $('#fsAlert');
        $el.removeClass('alert-success alert-danger alert-warning alert-info')
           .addClass('alert alert-' + (type || 'info'))
           .text(msg)
           .prop('hidden', false);
        setTimeout(function () { $el.prop('hidden', true); }, 4000);
    }

    function filterGrid() {
        if (!_grid) return;
        if (_selectedDistrictId) {
            var filtered = _allData.filter(function (r) { return r.DistrictId === _selectedDistrictId; });
            _grid.option('dataSource', filtered);
        } else {
            _grid.option('dataSource', _allData);
        }
    }

    async function loadData() {
        try {
            _allData = await $.getJSON('/api/Hauling/FuelSurcharges');
            filterGrid();
        } catch (ex) {
            showAlert('Failed to load fuel surcharges.', 'danger');
        }
    }

    $(function () {
        // District filter
        $.getJSON('/api/Hauling/Districts').done(function (districts) {
            var items = [{ DistrictId: null, Name: 'All Districts' }].concat(districts);
            $('#fsDistrictFilter').dxSelectBox({
                dataSource: items,
                valueExpr: 'DistrictId',
                displayExpr: 'Name',
                value: null,
                searchEnabled: true,
                searchExpr: 'Name',
                searchMode: 'contains',
                showDataBeforeSearch: true,
                minSearchLength: 0,
                onValueChanged: function (e) {
                    _selectedDistrictId = e.value;
                    filterGrid();
                }
            });
        });

        // Bulk value input
        $('#fsBulkValue').dxNumberBox({
            value: null,
            min: 0,
            format: { type: 'fixedPoint', precision: 2 },
            placeholder: '0.00'
        });

        // Bulk apply button
        $('#fsBulkApply').dxButton({
            text: 'Apply',
            type: 'default',
            onClick: async function () {
                var val = $('#fsBulkValue').dxNumberBox('option', 'value');
                if (val === null || val === undefined) {
                    showAlert('Enter a surcharge value first.', 'warning');
                    return;
                }

                var url;
                var label;
                if (_selectedDistrictId) {
                    url = '/api/Hauling/FuelSurchargeByDistrict/' + _selectedDistrictId;
                    label = 'district';
                } else {
                    url = '/api/Hauling/FuelSurchargeAll';
                    label = 'all locations';
                }

                try {
                    var resp = await fetch(url, {
                        method: 'PATCH',
                        headers: { 'Content-Type': 'application/json' },
                        body: JSON.stringify({ FuelSurcharge: val })
                    });
                    if (!resp.ok) throw new Error('HTTP ' + resp.status);
                    var result = await resp.json();
                    showAlert('Updated ' + (result.Updated || 0) + ' location(s) for ' + label + '.', 'success');
                    await loadData();
                } catch (ex) {
                    showAlert('Failed to apply bulk surcharge.', 'danger');
                }
            }
        });

        // Grid
        $.getJSON('/api/Hauling/FuelSurcharges').done(function (data) {
            _allData = data;

            _grid = $('#fuelSurchargeGrid').dxDataGrid({
                dataSource: data,
                keyExpr: 'LocationId',
                showBorders: true,
                columnAutoWidth: true,
                rowAlternationEnabled: true,
                hoverStateEnabled: true,
                noDataText: 'No locations found.',
                searchPanel: { visible: true, width: 240, placeholder: 'Search\u2026' },
                sorting: { mode: 'single' },
                paging: { pageSize: 25 },
                pager: { showPageSizeSelector: true, allowedPageSizes: [10, 25, 50, 100] },
                editing: {
                    mode: 'cell',
                    allowUpdating: true,
                    selectTextOnEditStart: true
                },
                columns: [
                    { dataField: 'Name', caption: 'Location', allowEditing: false, sortOrder: 'asc', sortIndex: 0 },
                    { dataField: 'DistrictName', caption: 'District', allowEditing: false },
                    {
                        dataField: 'FuelSurcharge',
                        caption: 'Fuel Surcharge',
                        width: 150,
                        dataType: 'number',
                        format: { type: 'fixedPoint', precision: 2 },
                        allowEditing: true
                    }
                ],
                onRowUpdating: function (e) {
                    e.cancel = (async function () {
                        try {
                            var resp = await fetch('/api/Hauling/FuelSurcharge/' + e.key, {
                                method: 'PATCH',
                                headers: { 'Content-Type': 'application/json' },
                                body: JSON.stringify({ FuelSurcharge: e.newData.FuelSurcharge })
                            });
                            if (!resp.ok) throw new Error('HTTP ' + resp.status);
                            // Update local cache
                            var item = _allData.find(function (r) { return r.LocationId === e.key; });
                            if (item) item.FuelSurcharge = e.newData.FuelSurcharge;
                            showAlert('Surcharge updated.', 'success');
                        } catch (ex) {
                            showAlert('Failed to save surcharge.', 'danger');
                            throw ex;
                        }
                    })();
                }
            }).dxDataGrid('instance');
        }).fail(function () {
            showAlert('Failed to load fuel surcharges.', 'danger');
        });
    });
})();
