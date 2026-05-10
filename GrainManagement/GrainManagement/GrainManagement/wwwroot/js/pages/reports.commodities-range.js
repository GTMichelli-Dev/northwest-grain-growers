// Commodities By Date Range — DevExtreme dxDataGrid grouped by Location,
// with one row per Crop showing intake / transfer-from / transfer-to net
// totals (in the crop's primary unit of measure) plus the net change.
// Filters + search + Excel export sit inline above the grid.
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
                    o.value = d.DistrictId;
                    o.textContent = d.Name;
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
        // If a single location is chosen, use it; otherwise enumerate
        // every option in the location <select> (the district's full
        // location list).
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
    function fmtInt(v) {
        if (v === null || v === undefined || v === '') return '';
        var n = Number(v);
        if (!isFinite(n)) return '';
        return Math.round(n).toLocaleString();
    }

    function buildGrid() {
        $grid = $('#rb-grid').dxDataGrid({
            dataSource: [],
            keyExpr: ['LocationId', 'Crop'],
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
            summary: {
                groupItems: [
                    { column: 'IntakeNet', summaryType: 'sum',
                      valueFormat: function (v) { return fmtNum(v); }, displayFormat: 'Intake: {0}' },
                    { column: 'TransferFromNet', summaryType: 'sum',
                      valueFormat: function (v) { return fmtNum(v); }, displayFormat: 'From: {0}' },
                    { column: 'TransferToNet', summaryType: 'sum',
                      valueFormat: function (v) { return fmtNum(v); }, displayFormat: 'To: {0}' },
                    { column: 'NetChange', summaryType: 'sum',
                      valueFormat: function (v) { return fmtNum(v); }, displayFormat: 'Net: {0}', showInGroupFooter: false, alignByColumn: true },
                ],
            },
            columns: [
                { dataField: 'LocationName', caption: 'Location', groupIndex: 0 },
                { dataField: 'Crop', caption: 'Crop', minWidth: 160 },
                { dataField: 'PrimaryUom', caption: 'Primary UoM', width: 110, alignment: 'center' },
                { dataField: 'IntakeNet', caption: 'Intake Net', alignment: 'right',
                  customizeText: function (info) { return fmtNum(info.value); } },
                { dataField: 'IntakeLoadCount', caption: 'Intake Loads', alignment: 'right',
                  customizeText: function (info) { return fmtInt(info.value); } },
                { dataField: 'TransferFromNet', caption: 'Transfer From Net', alignment: 'right',
                  customizeText: function (info) { return fmtNum(info.value); } },
                { dataField: 'TransferFromLoadCount', caption: 'From Loads', alignment: 'right',
                  customizeText: function (info) { return fmtInt(info.value); } },
                { dataField: 'TransferToNet', caption: 'Transfer To Net', alignment: 'right',
                  customizeText: function (info) { return fmtNum(info.value); } },
                { dataField: 'TransferToLoadCount', caption: 'To Loads', alignment: 'right',
                  customizeText: function (info) { return fmtInt(info.value); } },
                { dataField: 'NetChange', caption: 'Net Change', alignment: 'right',
                  customizeText: function (info) { return fmtNum(info.value); } },
            ],
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

        fetch('/api/ReportBuilder/Commodities', {
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
        var worksheet = workbook.addWorksheet('Commodities');
        DevExpress.excelExporter.exportDataGrid({
            component: $grid, worksheet: worksheet,
        }).then(function () {
            return workbook.xlsx.writeBuffer();
        }).then(function (buffer) {
            var fromVal = document.getElementById('rb-from').value || todayIso();
            var toVal   = document.getElementById('rb-to').value   || todayIso();
            saveAs(new Blob([buffer], { type: 'application/octet-stream' }),
                'CommoditiesByDateRange_' + fromVal + '_to_' + toVal + '.xlsx');
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
