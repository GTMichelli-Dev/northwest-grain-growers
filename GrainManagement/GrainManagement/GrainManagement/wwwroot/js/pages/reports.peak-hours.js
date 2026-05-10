// Peak Hours — line chart of load count by hour-of-day, one series per
// selected location plus an "All" aggregate. Multi-select location
// picker (DevExtreme TagBox); chart uses dxChart; underlying data table
// is exported to Excel.
(function () {
    'use strict';

    var $chart = null;
    var $tagBox = null;
    var $grid = null;
    var lastRows = [];

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

    function loadLocationOptions(districtId) {
        var id = parseInt(districtId, 10) || 0;
        return fetch('/api/ReportBuilder/Locations?districtId=' + id)
            .then(function (r) { return r.ok ? r.json() : []; })
            .then(function (list) {
                var items = (list || []).map(function (l) {
                    return { id: l.LocationId, name: l.Name + ' (' + l.LocationId + ')' };
                });
                if ($tagBox) {
                    $tagBox.option('dataSource', items);
                    $tagBox.option('value', []);
                }
                return items;
            });
    }

    function buildTagBox() {
        $tagBox = $('#rb-location-tags').dxTagBox({
            dataSource: [],
            displayExpr: 'name',
            valueExpr: 'id',
            placeholder: 'Pick one or more locations…',
            searchEnabled: true,
            showSelectionControls: true,
            applyValueMode: 'useButtons',
            stylingMode: 'outlined',
        }).dxTagBox('instance');
    }

    function buildChart() {
        $chart = $('#rb-chart').dxChart({
            commonSeriesSettings: {
                argumentField: 'HourLabel',
                valueField: 'LoadCount',
                type: 'line',
                point: { visible: true, size: 6 },
            },
            dataSource: [],
            series: [],
            legend: { visible: true, verticalAlignment: 'bottom', horizontalAlignment: 'center' },
            argumentAxis: { title: { text: 'Hour of Day (Pacific)' } },
            valueAxis:    { title: { text: 'Load Count' }, valueType: 'numeric' },
            tooltip: { enabled: true, customizeTooltip: function (args) {
                return { text: args.seriesName + ' · ' + args.argumentText + ' → ' + args.valueText + ' loads' };
            } },
            export: { enabled: false },
            title: { text: 'Loads by Hour of Day' },
        }).dxChart('instance');
    }

    function buildGrid() {
        $grid = $('#rb-grid').dxDataGrid({
            dataSource: [],
            keyExpr: ['LocationId', 'Hour'],
            showBorders: true,
            rowAlternationEnabled: true,
            columnAutoWidth: true,
            paging: { enabled: false },
            grouping: { autoExpandAll: true },
            groupPanel: { visible: false },
            noDataText: 'Pick filters and click Apply.',
            columns: [
                { dataField: 'LocationName', caption: 'Location', groupIndex: 0 },
                { dataField: 'HourLabel', caption: 'Hour' },
                { dataField: 'LoadCount', caption: 'Loads', alignment: 'right' },
            ],
            summary: {
                groupItems: [
                    { column: 'LoadCount', summaryType: 'sum', displayFormat: 'Total: {0}', alignByColumn: true },
                ],
            },
        }).dxDataGrid('instance');
    }

    function fetchData() {
        var ids = ($tagBox && $tagBox.option('value')) || [];
        if (!ids.length) {
            DevExpress.ui.notify({
                message: 'Pick at least one location.',
                type: 'warning', displayTime: 3000,
            }, { position: 'top center' });
            return;
        }
        var fromVal = document.getElementById('rb-from').value || todayIso();
        var toVal   = document.getElementById('rb-to').value   || todayIso();

        fetch('/api/ReportBuilder/PeakHours', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ From: fromVal, To: toVal, LocationIds: ids })
        }).then(function (r) {
            if (!r.ok) throw new Error('HTTP ' + r.status);
            return r.json();
        }).then(function (rows) {
            lastRows = rows || [];
            $grid.option('dataSource', lastRows);
            // Build one chart series per distinct LocationName so the
            // chart shows one line per location plus the All aggregate.
            var byName = {};
            var nameOrder = [];
            lastRows.forEach(function (r) {
                if (!byName[r.LocationName]) {
                    byName[r.LocationName] = [];
                    nameOrder.push(r.LocationName);
                }
                byName[r.LocationName].push(r);
            });
            // Pivot to wide format for the chart: each row keyed by
            // HourLabel with one column per location.
            var hours = [];
            for (var h = 0; h < 24; h++) hours.push(h.toString().padStart(2, '0') + ':00');
            var dataSource = hours.map(function (lbl) {
                var row = { HourLabel: lbl };
                nameOrder.forEach(function (name) {
                    var rec = (byName[name] || []).find(function (r) { return r.HourLabel === lbl; });
                    row[name] = rec ? rec.LoadCount : 0;
                });
                return row;
            });
            var series = nameOrder.map(function (name) {
                return { name: name, valueField: name, type: name === 'All' ? 'spline' : 'line' };
            });
            $chart.option({
                dataSource: dataSource,
                series: series,
                commonSeriesSettings: {
                    argumentField: 'HourLabel',
                    point: { visible: true, size: 6 },
                },
            });
        }).catch(function (err) {
            DevExpress.ui.notify({
                message: 'Failed to load: ' + (err.message || err),
                type: 'error', displayTime: 4000,
            }, { position: 'top center' });
        });
    }

    function exportToExcel() {
        if (!$grid) return;
        var workbook = new ExcelJS.Workbook();
        var worksheet = workbook.addWorksheet('Peak Hours');
        DevExpress.excelExporter.exportDataGrid({
            component: $grid, worksheet: worksheet,
        }).then(function () {
            return workbook.xlsx.writeBuffer();
        }).then(function (buffer) {
            var fromVal = document.getElementById('rb-from').value || todayIso();
            var toVal   = document.getElementById('rb-to').value   || todayIso();
            saveAs(new Blob([buffer], { type: 'application/octet-stream' }),
                'PeakHours_' + fromVal + '_to_' + toVal + '.xlsx');
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        var t = todayIso();
        document.getElementById('rb-from').value = t;
        document.getElementById('rb-to').value   = t;

        buildTagBox();
        buildChart();
        buildGrid();

        loadDistricts().then(function () { return loadLocationOptions('0'); });

        document.getElementById('rb-district').addEventListener('change', function () {
            loadLocationOptions(this.value);
        });
        document.getElementById('rb-apply').addEventListener('click', fetchData);
        document.getElementById('rb-export').addEventListener('click', exportToExcel);
    });
})();
