// Load Dump Type — one row per load that carries the IS_END_DUMP
// transaction attribute. Date-range filter only; no district/location
// scoping (the report is intentionally a global slice across the
// whole network for the chosen window).
(function () {
    'use strict';

    var $grid = null;

    function todayIso() {
        var d = new Date();
        return d.getFullYear() + '-'
            + String(d.getMonth() + 1).padStart(2, '0') + '-'
            + String(d.getDate()).padStart(2, '0');
    }

    function fmtTime(v) {
        if (!v) return '';
        var d = new Date(v);
        if (isNaN(d)) return '';
        return d.toLocaleString();
    }

    function buildGrid() {
        $grid = $('#rb-grid').dxDataGrid({
            dataSource: [],
            keyExpr: 'LoadId',
            showBorders: true,
            showRowLines: true,
            rowAlternationEnabled: true,
            columnAutoWidth: true,
            hoverStateEnabled: true,
            paging: { enabled: true, pageSize: 100 },
            pager: { visible: true, showInfo: true, showPageSizeSelector: true, allowedPageSizes: [50, 100, 250, 'all'] },
            sorting: { mode: 'multiple' },
            searchPanel: { visible: false },
            noDataText: 'Pick a date range and click Apply.',
            columns: [
                { dataField: 'LoadId', caption: 'Load #', alignment: 'right', sortOrder: 'asc' },
                { dataField: 'LocationId', caption: 'Location Id', alignment: 'right', width: 100 },
                { dataField: 'LocationName', caption: 'Location' },
                { dataField: 'TimeIn', caption: 'Load Time In', alignment: 'right',
                  customizeText: function (info) { return fmtTime(info.value); } },
                { dataField: 'DumpType', caption: 'Type',
                  cellTemplate: function (cellElement, cellInfo) {
                      var v = cellInfo.value || '';
                      var $span = $('<span>').text(v);
                      if (cellInfo.data.IsEndDump === true) $span.css({ color: '#1565c0', fontWeight: 600 });
                      else if (cellInfo.data.IsEndDump === false) $span.css({ color: '#2e7d32', fontWeight: 600 });
                      cellElement.append($span);
                  } },
            ],
            summary: {
                totalItems: [
                    { column: 'LoadId', summaryType: 'count', displayFormat: '{0} loads' },
                ],
            },
        }).dxDataGrid('instance');
    }

    function fetchData() {
        var fromVal = document.getElementById('rb-from').value || todayIso();
        var toVal   = document.getElementById('rb-to').value   || todayIso();
        $grid.beginCustomLoading('Loading…');
        fetch('/api/ReportBuilder/LoadDumpTypes', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ From: fromVal, To: toVal })
        }).then(function (r) {
            if (!r.ok) throw new Error('HTTP ' + r.status);
            return r.json();
        }).then(function (rows) {
            $grid.option('dataSource', rows || []);
        }).catch(function (err) {
            $grid.option('dataSource', []);
            DevExpress.ui.notify({
                message: 'Failed to load: ' + (err.message || err),
                type: 'error', displayTime: 4000,
            }, { position: 'top center' });
        }).finally(function () {
            $grid.endCustomLoading();
        });
    }

    function exportToExcel() {
        if (!$grid) return;
        var workbook = new ExcelJS.Workbook();
        var worksheet = workbook.addWorksheet('Load Dump Type');
        DevExpress.excelExporter.exportDataGrid({
            component: $grid, worksheet: worksheet,
        }).then(function () {
            return workbook.xlsx.writeBuffer();
        }).then(function (buffer) {
            var fromVal = document.getElementById('rb-from').value || todayIso();
            var toVal   = document.getElementById('rb-to').value   || todayIso();
            saveAs(new Blob([buffer], { type: 'application/octet-stream' }),
                'LoadDumpType_' + fromVal + '_to_' + toVal + '.xlsx');
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        var t = todayIso();
        document.getElementById('rb-from').value = t;
        document.getElementById('rb-to').value   = t;

        buildGrid();

        document.getElementById('rb-apply').addEventListener('click', fetchData);
        document.getElementById('rb-export').addEventListener('click', exportToExcel);
        document.getElementById('rb-search').addEventListener('input', function () {
            if ($grid) $grid.searchByText(this.value || '');
        });
    });
})();
