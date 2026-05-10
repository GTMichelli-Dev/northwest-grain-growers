// Bin Protein Weighted Average — dxDataGrid grouped by Location with
// one row per Bin showing weighted-average protein and total bushels.
// Filter row + search + Excel export inline above the grid.
(function () {
    'use strict';

    var $grid = null;

    function todayIso() {
        var d = new Date();
        return d.getFullYear() + '-'
            + String(d.getMonth() + 1).padStart(2, '0') + '-'
            + String(d.getDate()).padStart(2, '0');
    }

    function loadDistricts() {
        return fetch('/api/ReportBuilder/Districts')
            .then(function (r) { return r.ok ? r.json() : []; })
            .then(function (list) {
                var sel = document.getElementById('rb-district');
                sel.innerHTML = '<option value="0">All</option>';
                (list || []).forEach(function (d) {
                    var o = document.createElement('option');
                    o.value = d.DistrictId; o.textContent = d.Name;
                    sel.appendChild(o);
                });
                sel.value = '0';
            });
    }

    function loadLocations(districtId) {
        var sel = document.getElementById('rb-location');
        sel.innerHTML = '<option value="">— All in selection —</option>';
        var id = parseInt(districtId, 10) || 0;
        return fetch('/api/ReportBuilder/Locations?districtId=' + id)
            .then(function (r) { return r.ok ? r.json() : []; })
            .then(function (list) {
                (list || []).forEach(function (l) {
                    var o = document.createElement('option');
                    o.value = l.LocationId;
                    o.textContent = l.Name + ' (' + l.LocationId + ')';
                    sel.appendChild(o);
                });
                return list;
            });
    }

    function pickLocationIds() {
        var sel = document.getElementById('rb-location');
        var single = parseInt(sel.value, 10);
        if (single) return [single];
        var ids = [];
        for (var i = 0; i < sel.options.length; i++) {
            var v = parseInt(sel.options[i].value, 10);
            if (v) ids.push(v);
        }
        return ids;
    }

    function fmtNum(v) {
        if (v === null || v === undefined || v === '') return '';
        var n = Number(v);
        if (!isFinite(n)) return '';
        return n.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    }
    function fmtProtein(v) {
        if (v === null || v === undefined || v === '') return '';
        var n = Number(v);
        if (!isFinite(n) || n === 0) return '';
        return n.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 3 }) + ' %';
    }
    function fmtInt(v) {
        var n = Number(v);
        return isFinite(n) ? Math.round(n).toLocaleString() : '';
    }

    function buildGrid() {
        $grid = $('#rb-grid').dxDataGrid({
            dataSource: [],
            keyExpr: ['LocationId', 'Bin'],
            showBorders: true,
            showRowLines: true,
            rowAlternationEnabled: true,
            columnAutoWidth: true,
            hoverStateEnabled: true,
            grouping: { autoExpandAll: true },
            groupPanel: { visible: false },
            paging: { enabled: false },
            scrolling: { mode: 'standard' },
            searchPanel: { visible: false },
            noDataText: 'Pick filters and click Apply.',
            columns: [
                { dataField: 'LocationName', caption: 'Location', groupIndex: 0 },
                { dataField: 'Bin', caption: 'Bin', minWidth: 160 },
                { dataField: 'AvgProtein', caption: 'Avg Protein', alignment: 'right',
                  customizeText: function (info) { return fmtProtein(info.value); } },
                { dataField: 'TotalBushels', caption: 'Total Bushels', alignment: 'right',
                  customizeText: function (info) { return fmtNum(info.value); } },
                { dataField: 'SampledBushels', caption: 'Sampled Bushels', alignment: 'right',
                  customizeText: function (info) { return fmtNum(info.value); } },
                { dataField: 'LoadCount', caption: 'Loads', alignment: 'right',
                  customizeText: function (info) { return fmtInt(info.value); } },
                { dataField: 'SamplesUsed', caption: 'Samples Used', alignment: 'right',
                  customizeText: function (info) { return fmtInt(info.value); } },
            ],
            summary: {
                groupItems: [
                    { column: 'TotalBushels', summaryType: 'sum',
                      valueFormat: function (v) { return fmtNum(v); }, displayFormat: 'Bushels: {0}', alignByColumn: true },
                    { column: 'LoadCount', summaryType: 'sum',
                      valueFormat: function (v) { return fmtInt(v); }, displayFormat: 'Loads: {0}', alignByColumn: true },
                ],
            },
        }).dxDataGrid('instance');
    }

    function fetchData() {
        var ids = pickLocationIds();
        if (!ids.length) {
            $grid.option('dataSource', []);
            return;
        }
        var fromVal = document.getElementById('rb-from').value || todayIso();
        var toVal   = document.getElementById('rb-to').value   || todayIso();
        $grid.beginCustomLoading('Loading…');
        fetch('/api/ReportBuilder/BinProtein', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ From: fromVal, To: toVal, LocationIds: ids })
        }).then(function (r) {
            if (!r.ok) throw new Error('HTTP ' + r.status);
            return r.json();
        }).then(function (rows) {
            $grid.option('dataSource', rows || []);
        }).catch(function (err) {
            $grid.option('dataSource', []);
            DevExpress.ui.notify({
                message: 'Failed to load: ' + (err.message || err),
                type: 'error', displayTime: 4000
            }, { position: 'top center' });
        }).finally(function () {
            $grid.endCustomLoading();
        });
    }

    function exportToExcel() {
        if (!$grid) return;
        var workbook = new ExcelJS.Workbook();
        var worksheet = workbook.addWorksheet('Bin Protein');
        DevExpress.excelExporter.exportDataGrid({
            component: $grid, worksheet: worksheet,
        }).then(function () {
            return workbook.xlsx.writeBuffer();
        }).then(function (buffer) {
            var fromVal = document.getElementById('rb-from').value || todayIso();
            var toVal   = document.getElementById('rb-to').value   || todayIso();
            saveAs(new Blob([buffer], { type: 'application/octet-stream' }),
                'BinProtein_' + fromVal + '_to_' + toVal + '.xlsx');
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        var t = todayIso();
        document.getElementById('rb-from').value = t;
        document.getElementById('rb-to').value   = t;

        buildGrid();
        loadDistricts().then(function () { return loadLocations('0'); });

        document.getElementById('rb-district').addEventListener('change', function () {
            loadLocations(this.value);
        });
        document.getElementById('rb-apply').addEventListener('click', fetchData);
        document.getElementById('rb-export').addEventListener('click', exportToExcel);
        document.getElementById('rb-search').addEventListener('input', function () {
            if ($grid) $grid.searchByText(this.value || '');
        });
    });
})();
