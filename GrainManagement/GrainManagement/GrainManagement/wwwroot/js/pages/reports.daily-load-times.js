// Daily Load Times — one row per load with TimeIn / TimeOut /
// duration. Filter row + search + Excel export inline above the grid.
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

    function fmtNum(v, dp) {
        if (v === null || v === undefined || v === '') return '';
        var n = Number(v);
        if (!isFinite(n)) return '';
        return n.toLocaleString(undefined, { minimumFractionDigits: dp || 0, maximumFractionDigits: dp || 0 });
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
            searchPanel: { visible: false },
            grouping: { autoExpandAll: true },
            groupPanel: { visible: false },
            sorting: { mode: 'multiple' },
            noDataText: 'Pick filters and click Apply.',
            columns: [
                { dataField: 'Date', caption: 'Date', dataType: 'date', sortOrder: 'asc' },
                { dataField: 'LocationName', caption: 'Location' },
                { dataField: 'WeightSheetId', caption: 'WS #' },
                { dataField: 'TruckId', caption: 'Truck' },
                { dataField: 'Bin', caption: 'Bin' },
                { dataField: 'TimeIn', caption: 'Time In', alignment: 'right',
                  customizeText: function (info) { return fmtTime(info.value); } },
                { dataField: 'TimeOut', caption: 'Time Out', alignment: 'right',
                  customizeText: function (info) { return fmtTime(info.value); } },
                { dataField: 'DurationMinutes', caption: 'Duration (min)', alignment: 'right',
                  customizeText: function (info) { return fmtNum(info.value, 1); } },
                { dataField: 'NetLbs', caption: 'Net Lbs', alignment: 'right',
                  customizeText: function (info) { return fmtNum(info.value, 0); } },
            ],
            summary: {
                totalItems: [
                    { column: 'DurationMinutes', summaryType: 'avg',
                      valueFormat: function (v) { return fmtNum(v, 1); }, displayFormat: 'Avg: {0}' },
                    { column: 'NetLbs', summaryType: 'sum',
                      valueFormat: function (v) { return fmtNum(v, 0); }, displayFormat: 'Total: {0}' },
                    { column: 'LoadId', summaryType: 'count', displayFormat: '{0} loads' },
                ],
            },
        }).dxDataGrid('instance');
    }

    function fetchData() {
        var ids = pickLocationIds();
        if (!ids.length) { $grid.option('dataSource', []); return; }
        var fromVal = document.getElementById('rb-from').value || todayIso();
        var toVal   = document.getElementById('rb-to').value   || todayIso();
        $grid.beginCustomLoading('Loading…');
        fetch('/api/ReportBuilder/DailyLoadTimes', {
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
        var worksheet = workbook.addWorksheet('Daily Load Times');
        DevExpress.excelExporter.exportDataGrid({
            component: $grid, worksheet: worksheet,
        }).then(function () {
            return workbook.xlsx.writeBuffer();
        }).then(function (buffer) {
            var fromVal = document.getElementById('rb-from').value || todayIso();
            var toVal   = document.getElementById('rb-to').value   || todayIso();
            saveAs(new Blob([buffer], { type: 'application/octet-stream' }),
                'DailyLoadTimes_' + fromVal + '_to_' + toVal + '.xlsx');
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
