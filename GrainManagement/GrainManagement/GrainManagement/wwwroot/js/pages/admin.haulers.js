(function () {
    'use strict';

    var _grid;

    function showAlert(msg, type) {
        var $el = $('#hAlert');
        $el.removeClass('alert-success alert-danger alert-warning alert-info')
           .addClass('alert alert-' + (type || 'info'))
           .text(msg)
           .prop('hidden', false);
        setTimeout(function () { $el.prop('hidden', true); }, 4000);
    }

    $(function () {
        $.getJSON('/api/Hauling/Haulers')
            .done(function (data) {
                _grid = $('#haulersGrid').dxDataGrid({
                    dataSource: data,
                    keyExpr: 'Id',
                    showBorders: true,
                    columnAutoWidth: true,
                    rowAlternationEnabled: true,
                    hoverStateEnabled: true,
                    noDataText: 'No haulers found.',
                    searchPanel: { visible: true, width: 240, placeholder: 'Search\u2026' },
                    sorting: { mode: 'single' },
                    paging: { pageSize: 25 },
                    pager: { showPageSizeSelector: true, allowedPageSizes: [10, 25, 50, 100] },
                    columns: [
                        { dataField: 'Id', caption: 'ID', width: 80, sortOrder: 'asc', sortIndex: 0 },
                        { dataField: 'Description', caption: 'Hauler' },
                        {
                            dataField: 'IsActive',
                            caption: 'Active',
                            width: 90,
                            dataType: 'boolean'
                        }
                    ]
                }).dxDataGrid('instance');
            })
            .fail(function () {
                showAlert('Failed to load haulers.', 'danger');
            });
    });
})();
