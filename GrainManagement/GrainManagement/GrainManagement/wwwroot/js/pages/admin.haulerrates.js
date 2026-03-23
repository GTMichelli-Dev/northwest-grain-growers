(function () {
    'use strict';

    var _grid;
    var _allData = [];
    var _selectedRateType = null;

    var rateTypeMap = {
        'A': 'Along Side the Field',
        'F': 'Farm Storage'
    };

    function rateTypeDisplay(code) {
        return rateTypeMap[code] || code;
    }

    function showAlert(msg, type) {
        var $el = $('#hrAlert');
        $el.removeClass('alert-success alert-danger alert-warning alert-info')
           .addClass('alert alert-' + (type || 'info'))
           .text(msg)
           .prop('hidden', false);
        setTimeout(function () { $el.prop('hidden', true); }, 4000);
    }

    function filterGrid() {
        if (!_grid) return;
        if (_selectedRateType) {
            var filtered = _allData.filter(function (r) { return r.RateType === _selectedRateType; });
            _grid.option('dataSource', filtered);
        } else {
            _grid.option('dataSource', _allData);
        }
    }

    $(function () {
        // Rate Type filter
        $('#hrRateTypeFilter').dxSelectBox({
            dataSource: [
                { value: null, text: 'All Rate Types' },
                { value: 'A', text: 'Along Side the Field' },
                { value: 'F', text: 'Farm Storage' }
            ],
            valueExpr: 'value',
            displayExpr: 'text',
            value: null,
            onValueChanged: function (e) {
                _selectedRateType = e.value;
                filterGrid();
            }
        });

        $.getJSON('/api/Hauling/HaulerRates')
            .done(function (data) {
                _allData = data;

                _grid = $('#haulerRatesGrid').dxDataGrid({
                    dataSource: data,
                    keyExpr: 'Id',
                    showBorders: true,
                    columnAutoWidth: true,
                    rowAlternationEnabled: true,
                    hoverStateEnabled: true,
                    noDataText: 'No hauler rates found.',
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
                        {
                            dataField: 'RateType',
                            caption: 'Rate Type',
                            allowEditing: false,
                            sortOrder: 'asc',
                            sortIndex: 0,
                            calculateCellValue: function (rowData) {
                                return rateTypeDisplay(rowData.RateType);
                            }
                        },
                        { dataField: 'MaxDistance', caption: 'Max Distance', allowEditing: false, width: 130 },
                        {
                            dataField: 'Rate',
                            caption: 'Rate',
                            width: 150,
                            dataType: 'number',
                            format: { type: 'fixedPoint', precision: 4 },
                            allowEditing: true,
                            validationRules: [
                                { type: 'range', min: 0, max: 10, message: 'Rate must be between 0 and 10.' }
                            ]
                        }
                    ],
                    onRowUpdating: function (e) {
                        e.cancel = (async function () {
                            try {
                                var resp = await fetch('/api/Hauling/HaulerRates/' + e.key, {
                                    method: 'PATCH',
                                    headers: { 'Content-Type': 'application/json' },
                                    body: JSON.stringify({ Rate: e.newData.Rate })
                                });
                                if (!resp.ok) {
                                    var err = await resp.json().catch(function () { return {}; });
                                    throw new Error(err.message || 'HTTP ' + resp.status);
                                }
                                showAlert('Rate updated.', 'success');
                            } catch (ex) {
                                showAlert(ex.message || 'Failed to save rate.', 'danger');
                                throw ex;
                            }
                        })();
                    }
                }).dxDataGrid('instance');
            })
            .fail(function () {
                showAlert('Failed to load hauler rates.', 'danger');
            });
    });
})();
