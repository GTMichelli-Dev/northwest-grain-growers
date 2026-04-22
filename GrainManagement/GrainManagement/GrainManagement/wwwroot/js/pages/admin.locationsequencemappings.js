(function () {
    'use strict';

    var _servers = [];
    var _locations = [];

    function showAlert(msg, type) {
        var $el = $('#lsmAlert');
        $el.removeClass('alert-success alert-danger alert-warning alert-info')
           .addClass('alert alert-' + (type || 'info'))
           .text(msg)
           .prop('hidden', false);
        setTimeout(function () { $el.prop('hidden', true); }, 4000);
    }

    $(function () {
        // Load lookup data and grid data in parallel
        $.when(
            $.getJSON('/api/LocationSequenceMappings/Servers'),
            $.getJSON('/api/LocationSequenceMappings/Locations'),
            $.getJSON('/api/LocationSequenceMappings')
        ).done(function (serversRes, locationsRes, dataRes) {
            _servers = serversRes[0];
            _locations = locationsRes[0];
            initGrid(dataRes[0]);
        }).fail(function () {
            showAlert('Failed to load data.', 'danger');
        });
    });

    function initGrid(data) {
        var serverLookup = _servers.map(function (s) {
            return { value: s.ServerId, text: s.Name + ' - ' + s.ServerId };
        });
        var locationLookup = _locations.map(function (l) {
            return { value: l.LocationId, text: l.Name + ' - ' + l.LocationId };
        });

        $('#lsmGrid').dxDataGrid({
            dataSource: new DevExpress.data.CustomStore({
                key: 'Id',
                load: function () {
                    return $.getJSON('/api/LocationSequenceMappings');
                },
                insert: function (values) {
                    return $.ajax({
                        url: '/api/LocationSequenceMappings',
                        method: 'POST',
                        contentType: 'application/json',
                        data: JSON.stringify({
                            serverId: values.ServerId,
                            locationId: values.LocationId,
                            sequenceId: values.SequenceId,
                            lotSeed: values.LotSeed || 0,
                            weightSheetSeed: values.WeightSheetSeed || 0,
                        }),
                    });
                },
                update: function (key, values) {
                    // We need to merge with existing row data for a full PUT
                    var grid = $('#lsmGrid').dxDataGrid('instance');
                    var rowIndex = grid.getRowIndexByKey(key);
                    var rowData = grid.getVisibleRows()[rowIndex].data;
                    var merged = $.extend({}, rowData, values);
                    return $.ajax({
                        url: '/api/LocationSequenceMappings/' + key,
                        method: 'PUT',
                        contentType: 'application/json',
                        data: JSON.stringify({
                            serverId: merged.ServerId,
                            locationId: merged.LocationId,
                            sequenceId: merged.SequenceId,
                            lotSeed: merged.LotSeed || 0,
                            weightSheetSeed: merged.WeightSheetSeed || 0,
                        }),
                    });
                },
                remove: function (key) {
                    return $.ajax({
                        url: '/api/LocationSequenceMappings/' + key,
                        method: 'DELETE',
                    });
                },
            }),
            showBorders: true,
            showRowLines: true,
            rowAlternationEnabled: true,
            columnAutoWidth: true,
            hoverStateEnabled: true,
            noDataText: 'No location sequence mappings found.',
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
                allowDeleting: true,
                confirmDelete: true,
                useIcons: true,
            },
            onRowValidating: function (e) {
                var merged = $.extend({}, e.oldData, e.newData);
                var lot = Number(merged.LotSeed);
                var ws = Number(merged.WeightSheetSeed);
                var messages = [];
                if (!isFinite(lot) || lot < 0 || lot > 99999) {
                    messages.push('Lot Seed must be between 0 and 99999 (got ' + merged.LotSeed + ').');
                }
                if (!isFinite(ws) || ws < 0 || ws > 99999) {
                    messages.push('Weight Sheet Seed must be between 0 and 99999 (got ' + merged.WeightSheetSeed + ').');
                }
                if (messages.length) {
                    e.isValid = false;
                    e.errorText = messages.join(' ');
                    showAlert(messages.join(' '), 'danger');
                }
            },
            export: {
                enabled: true,
                allowExportSelectedData: false,
            },
            onExporting: function (e) {
                var workbook = new ExcelJS.Workbook();
                var worksheet = workbook.addWorksheet('LocationSequenceMappings');
                DevExpress.excelExporter.exportDataGrid({
                    component: e.component,
                    worksheet: worksheet,
                }).then(function () {
                    return workbook.xlsx.writeBuffer();
                }).then(function (buffer) {
                    saveAs(
                        new Blob([buffer], { type: 'application/octet-stream' }),
                        'LocationSequenceMappings.xlsx'
                    );
                });
                e.cancel = true;
            },
            columns: [
                {
                    dataField: 'LocationId',
                    caption: 'Location',
                    sortOrder: 'asc',
                    sortIndex: 0,
                    lookup: {
                        dataSource: locationLookup,
                        valueExpr: 'value',
                        displayExpr: 'text',
                    },
                    validationRules: [{ type: 'required' }],
                },
                {
                    dataField: 'SequenceId',
                    caption: 'Sequence ID',
                    width: 'auto',
                    dataType: 'number',
                    sortOrder: 'asc',
                    sortIndex: 1,
                    validationRules: [
                        { type: 'required' },
                        { type: 'range', min: 1, max: 5, message: 'Sequence ID must be between 1 and 5' },
                    ],
                },
                {
                    dataField: 'ServerId',
                    caption: 'Hosted By (Database Server)',
                    sortOrder: 'asc',
                    sortIndex: 2,
                    lookup: {
                        dataSource: serverLookup,
                        valueExpr: 'value',
                        displayExpr: 'text',
                    },
                    validationRules: [{ type: 'required' }],
                },
                {
                    dataField: 'LotSeed',
                    caption: 'Lot Seed (Last Id Used)',
                    dataType: 'number',
                    width: 'auto',
                    headerCellTemplate: function (container) {
                        $(container)
                            .append($('<span/>').text('Lot Seed '))
                            .append($('<span/>').text('(Last Id Used)').css({ 'font-style': 'italic', 'font-size': '50%' }));
                    },
                    validationRules: [
                        { type: 'range', min: 0, max: 99999, message: 'Lot Seed must be between 0 and 99999' },
                    ],
                },
                {
                    dataField: 'LastLotBaseId',
                    caption: 'Last Lot Id',
                    dataType: 'number',
                    width: 'auto',
                    allowEditing: false,
                    allowFiltering: false,
                    allowSearch: false,
                    customizeText: function (cellInfo) {
                        return cellInfo.value == null ? '---' : cellInfo.valueText;
                    },
                },
                {
                    dataField: 'WeightSheetSeed',
                    caption: 'Weight Sheet (Last Id Used)',
                    dataType: 'number',
                    width: 'auto',
                    headerCellTemplate: function (container) {
                        $(container)
                            .append($('<span/>').text('Weight Sheet '))
                            .append($('<span/>').text('(Last Id Used)').css({ 'font-style': 'italic', 'font-size': '50%' }));
                    },
                    validationRules: [
                        { type: 'range', min: 0, max: 99999, message: 'Weight Sheet Seed must be between 0 and 99999' },
                    ],
                },
                {
                    dataField: 'LastWeightSheetBaseId',
                    caption: 'Last Weight Sheet Id',
                    dataType: 'number',
                    width: 'auto',
                    allowEditing: false,
                    allowFiltering: false,
                    allowSearch: false,
                    customizeText: function (cellInfo) {
                        return cellInfo.value == null ? '---' : cellInfo.valueText;
                    },
                },
            ],
        });
    }
})();
