(function () {
    'use strict';

    function showAlert(msg, type) {
        var $el = $('#srvAlert');
        $el.removeClass('alert-success alert-danger alert-warning alert-info')
           .addClass('alert alert-' + (type || 'info'))
           .text(msg)
           .prop('hidden', false);
        setTimeout(function () { $el.prop('hidden', true); }, 4000);
    }

    $(function () {
        $('#srvGrid').dxDataGrid({
            dataSource: new DevExpress.data.CustomStore({
                key: 'ServerId',
                load: function () {
                    return $.getJSON('/api/Servers');
                },
                insert: function (values) {
                    return $.ajax({
                        url: '/api/Servers',
                        method: 'POST',
                        contentType: 'application/json',
                        data: JSON.stringify(values),
                    });
                },
                update: function (key, values) {
                    return $.ajax({
                        url: '/api/Servers/' + key,
                        method: 'PUT',
                        contentType: 'application/json',
                        data: JSON.stringify(values),
                    });
                },
                remove: function (key) {
                    return $.ajax({
                        url: '/api/Servers/' + key,
                        method: 'DELETE',
                    });
                },
            }),
            showBorders: true,
            showRowLines: true,
            rowAlternationEnabled: true,
            columnAutoWidth: true,
            hoverStateEnabled: true,
            noDataText: 'No servers found.',
            searchPanel: { visible: true, placeholder: 'Search\u2026', width: 280 },
            headerFilter: { visible: true },
            filterRow: { visible: true },
            paging: { enabled: true, pageSize: 25 },
            pager: {
                visible: true,
                showPageSizeSelector: true,
                allowedPageSizes: [10, 25, 50, 100, 'all'],
                showInfo: true,
                showNavigationButtons: true,
            },
            sorting: { mode: 'multiple' },
            editing: {
                mode: 'row',
                allowAdding: true,
                allowUpdating: true,
                allowDeleting: false,
                useIcons: true,
            },
            export: {
                enabled: true,
                allowExportSelectedData: false,
            },
            onExporting: function (e) {
                var workbook = new ExcelJS.Workbook();
                var worksheet = workbook.addWorksheet('Servers');
                DevExpress.excelExporter.exportDataGrid({
                    component: e.component,
                    worksheet: worksheet,
                }).then(function () {
                    return workbook.xlsx.writeBuffer();
                }).then(function (buffer) {
                    saveAs(
                        new Blob([buffer], { type: 'application/octet-stream' }),
                        'Servers.xlsx'
                    );
                });
                e.cancel = true;
            },
            columns: [
                {
                    dataField: 'ServerId',
                    caption: 'Server ID',
                    width: 100,
                    sortOrder: 'asc',
                    validationRules: [{ type: 'required' }],
                },
                {
                    dataField: 'ServerName',
                    caption: 'Server Name',
                    validationRules: [{ type: 'required' }],
                },
                {
                    dataField: 'FriendlyName',
                    caption: 'Friendly Name',
                },
                {
                    dataField: 'Url',
                    caption: 'URL',
                    cellTemplate: function (container, options) {
                        if (options.value) {
                            $('<a>')
                                .attr('href', options.value)
                                .attr('target', '_blank')
                                .attr('rel', 'noopener')
                                .text(options.value)
                                .appendTo(container);
                        }
                    },
                },
                {
                    dataField: 'IsActive',
                    caption: 'Active',
                    width: 90,
                    dataType: 'boolean',
                },
            ],
        });
    });
})();
