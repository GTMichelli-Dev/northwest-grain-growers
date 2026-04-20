(function () {
    'use strict';

    // Cache per-group detail so expanding a row twice doesn't re-fetch.
    var _detailCache = {};

    $(function () {
        loadAndInitGrid();
    });

    async function loadAndInitGrid() {
        var rows = [];
        try {
            rows = await $.getJSON('/api/SplitGroups');
        } catch (ex) {
            console.warn('[AdminSplitGroups] AllSplitGroups load failed', ex);
        }
        initGrid(rows);
        wireUnassignedFilter();
    }

    // "Show only No Primary Account Set" checkbox: applies a grid filter on
    // PrimaryAccountId IS NULL. DevExtreme's filter API doesn't have a direct
    // "is null" operator, so we use an anonymous filter function via
    // dataSource filter on the grid instance.
    function wireUnassignedFilter() {
        var $cb = $('#adminSgOnlyUnassigned');
        $cb.on('change', function () {
            var grid = $('#adminSgGrid').dxDataGrid('instance');
            if (!grid) return;
            if (this.checked) {
                grid.filter(['PrimaryAccountId', '=', null]);
            } else {
                grid.clearFilter();
            }
        });
    }

    function initGrid(rows) {
        $('#adminSgGrid').dxDataGrid({
            dataSource:           rows,
            keyExpr:              'SplitGroupId',
            showBorders:          true,
            showRowLines:         true,
            rowAlternationEnabled: true,
            columnAutoWidth:      true,
            hoverStateEnabled:    true,
            noDataText:           'No split groups available.',
            searchPanel:          { visible: true, placeholder: 'Search any field\u2026', width: 320 },
            headerFilter:         { visible: true },
            filterRow:            { visible: true },
            paging:               { pageSize: 25 },
            pager: {
                visible:              true,
                showPageSizeSelector: true,
                allowedPageSizes:     [10, 25, 50, 100],
                showInfo:             true,
            },
            sorting:              { mode: 'multiple' },
            columnChooser:        { enabled: true },
            export: {
                enabled: true,
                allowExportSelectedData: false,
            },
            onExporting: function (e) {
                // DevExtreme's exporter honors the grid's current filter state
                // (search panel, column filters, header filter, filter row) and
                // exports ALL matching rows across every page — not just the
                // visible page.
                var workbook = new ExcelJS.Workbook();
                var worksheet = workbook.addWorksheet('SplitGroups');
                DevExpress.excelExporter.exportDataGrid({
                    component: e.component,
                    worksheet: worksheet,
                }).then(function () {
                    return workbook.xlsx.writeBuffer();
                }).then(function (buffer) {
                    saveAs(
                        new Blob([buffer], { type: 'application/octet-stream' }),
                        'SplitGroups.xlsx'
                    );
                });
                e.cancel = true;
            },

            // Light yellow highlight when PrimaryAccountId is null.
            onRowPrepared: function (e) {
                if (e.rowType === 'data' && e.data && e.data.PrimaryAccountId == null) {
                    $(e.rowElement).addClass('gm-sg-unassigned');
                }
            },

            columns: [
                {
                    dataField: 'SplitGroupId',
                    caption:   'SG #',
                    width:     100,
                    alignment: 'left',
                    sortOrder: 'asc',
                },
                {
                    dataField: 'SplitGroupDescription',
                    caption:   'Description',
                },
                {
                    dataField: 'PrimaryAccountAs400Id',
                    caption:   'Primary Acct #',
                    width:     140,
                    alignment: 'left',
                },
                {
                    dataField: 'PrimaryAccountName',
                    caption:   'Primary Account',
                    // Show "Not Set" when the primary is unassigned. We use
                    // calculateCellValue so the cell text and search/filter use
                    // the displayed value.
                    calculateCellValue: function (rowData) {
                        return (rowData && rowData.PrimaryAccountName) || 'Not Set';
                    },
                },
            ],

            masterDetail: {
                enabled: true,
                template: function (detailElement, detailInfo) {
                    var splitGroupId = detailInfo.data.SplitGroupId;
                    var $detail = $('<div class="gm-admin-sg-detail"></div>');
                    detailElement.append($detail);

                    var detailGridOptions = {
                        dataSource:      [],
                        showBorders:     true,
                        columnAutoWidth: true,
                        noDataText:      'No grower splits configured.',
                        columns: [
                            { dataField: 'As400AccountId', caption: 'Acct #',  width: 120, alignment: 'left' },
                            { dataField: 'AccountName',    caption: 'Grower' },
                            {
                                dataField: 'SplitPercent',
                                caption:   'Split %',
                                width:     120,
                                alignment: 'right',
                                format:    { type: 'fixedPoint', precision: 4 },
                            },
                        ],
                        summary: {
                            totalItems: [{
                                column:        'SplitPercent',
                                summaryType:   'sum',
                                displayFormat: 'Total: {0}',
                                valueFormat:   { type: 'fixedPoint', precision: 4 },
                            }],
                        },
                    };
                    $detail.dxDataGrid(detailGridOptions);

                    var detailInst = $detail.dxDataGrid('instance');
                    if (_detailCache[splitGroupId]) {
                        detailInst.option('dataSource', _detailCache[splitGroupId]);
                    } else {
                        $.getJSON('/api/SplitGroups/' + splitGroupId + '/Percents')
                            .done(function (rows) {
                                _detailCache[splitGroupId] = rows || [];
                                detailInst.option('dataSource', _detailCache[splitGroupId]);
                            })
                            .fail(function (ex) {
                                console.warn('[AdminSplitGroups] SplitGroupPercents load failed', ex);
                            });
                    }
                },
            },
        });
    }
})();
