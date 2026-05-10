// Intake Location Report — same shape as Producer Delivery, with
// NetUnits + Primary UoM added (resolved from each load's category
// DefaultUom and accumulated per-load so multi-crop rows still round-
// trip correctly).
(function () {
    'use strict';

    var $grid = null;
    var $locTags = null;
    var $cropTags = null;

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
                if ($locTags) {
                    $locTags.option('dataSource', items);
                    $locTags.option('value', items.map(function (i) { return i.id; }));
                }
                return items;
            });
    }

    function loadCropOptions() {
        var locIds = ($locTags && $locTags.option('value')) || [];
        if (!locIds.length) {
            if ($cropTags) {
                $cropTags.option('dataSource', []);
                $cropTags.option('value', []);
            }
            return Promise.resolve([]);
        }
        var fromVal = document.getElementById('rb-from').value || todayIso();
        var toVal   = document.getElementById('rb-to').value   || todayIso();
        return fetch('/api/ReportBuilder/IntakeCrops', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ From: fromVal, To: toVal, LocationIds: locIds })
        })
        .then(function (r) { return r.ok ? r.json() : []; })
        .then(function (list) {
            var items = (list || []).map(function (c) {
                return { id: c.CropId, name: c.Description };
            });
            if ($cropTags) {
                $cropTags.option('dataSource', items);
                $cropTags.option('value', items.map(function (i) { return i.id; }));
            }
            return items;
        });
    }

    function pickIds(tagBox) {
        var v = tagBox && tagBox.option('value');
        return Array.isArray(v) ? v.slice() : [];
    }
    function isGroupBySplit() {
        return document.getElementById('rb-groupby-split').checked;
    }

    function fmtNum(v, dp) {
        if (v === null || v === undefined || v === '') return '';
        var n = Number(v);
        if (!isFinite(n)) return '';
        return n.toLocaleString(undefined, { minimumFractionDigits: dp || 0, maximumFractionDigits: dp || 0 });
    }
    function fmtInt(v) { var n = Number(v); return isFinite(n) ? Math.round(n).toLocaleString() : ''; }

    function buildTagBoxes() {
        $locTags = $('#rb-location-tags').dxTagBox({
            dataSource: [], displayExpr: 'name', valueExpr: 'id',
            placeholder: 'Pick locations…',
            searchEnabled: true, showSelectionControls: true,
            applyValueMode: 'useButtons', stylingMode: 'outlined',
            onValueChanged: function () { loadCropOptions(); },
        }).dxTagBox('instance');

        $cropTags = $('#rb-crop-tags').dxTagBox({
            dataSource: [], displayExpr: 'name', valueExpr: 'id',
            placeholder: 'All crops',
            searchEnabled: true, showSelectionControls: true,
            applyValueMode: 'useButtons', stylingMode: 'outlined',
        }).dxTagBox('instance');
    }

    function buildGrid() {
        $grid = $('#rb-grid').dxDataGrid({
            dataSource: [],
            keyExpr: ['LocationId', 'PrimaryAccountId', 'SplitGroupId'],
            showBorders: true,
            showRowLines: true,
            rowAlternationEnabled: true,
            columnAutoWidth: true,
            hoverStateEnabled: true,
            paging: { enabled: true, pageSize: 100 },
            pager: { visible: true, showInfo: true, showPageSizeSelector: true, allowedPageSizes: [50, 100, 250, 'all'] },
            grouping: { autoExpandAll: true },
            groupPanel: { visible: false },
            sorting: { mode: 'multiple' },
            searchPanel: { visible: false },
            noDataText: 'Pick filters and click Apply.',
            columns: [
                { dataField: 'DistrictName', caption: 'District', groupIndex: 0 },
                { dataField: 'LocationName', caption: 'Location', groupIndex: 1 },
                { dataField: 'PrimaryAccountName', caption: 'Primary Account' },
                { dataField: 'PrimaryAccountId', caption: 'Account #', alignment: 'right', width: 100 },
                { dataField: 'SplitGroupId', caption: 'Split #', alignment: 'right', width: 100, visible: false },
                { dataField: 'SplitGroupDescription', caption: 'Split Description', visible: false },
                { dataField: 'LoadCount', caption: '# Loads', alignment: 'right',
                  customizeText: function (info) { return fmtInt(info.value); } },
                { dataField: 'NetLbs', caption: 'Net (lbs)', alignment: 'right',
                  customizeText: function (info) { return fmtNum(info.value, 0); } },
                { dataField: 'NetUnits', caption: 'Net Units', alignment: 'right',
                  customizeText: function (info) { return fmtNum(info.value, 2); } },
                { dataField: 'PrimaryUom', caption: 'Primary UoM', width: 110, alignment: 'center' },
            ],
            summary: {
                groupItems: [
                    { column: 'LoadCount', summaryType: 'sum',
                      valueFormat: function (v) { return fmtInt(v); }, displayFormat: 'Loads: {0}', alignByColumn: true },
                    { column: 'NetLbs', summaryType: 'sum',
                      valueFormat: function (v) { return fmtNum(v, 0); }, displayFormat: 'Net Lbs: {0}', alignByColumn: true },
                    { column: 'NetUnits', summaryType: 'sum',
                      valueFormat: function (v) { return fmtNum(v, 2); }, displayFormat: 'Net Units: {0}', alignByColumn: true },
                ],
                totalItems: [
                    { column: 'LoadCount', summaryType: 'sum',
                      valueFormat: function (v) { return fmtInt(v); }, displayFormat: '{0} loads' },
                    { column: 'NetLbs', summaryType: 'sum',
                      valueFormat: function (v) { return fmtNum(v, 0); }, displayFormat: 'Total Lbs: {0}' },
                    { column: 'NetUnits', summaryType: 'sum',
                      valueFormat: function (v) { return fmtNum(v, 2); }, displayFormat: 'Total Units: {0}' },
                ],
            },
        }).dxDataGrid('instance');
    }

    function applyGroupByVisibility() {
        if (!$grid) return;
        var split = isGroupBySplit();
        $grid.columnOption('SplitGroupId', 'visible', split);
        $grid.columnOption('SplitGroupDescription', 'visible', split);
    }

    function fetchData() {
        var locIds = pickIds($locTags);
        if (!locIds.length) {
            DevExpress.ui.notify({
                message: 'Pick at least one location.',
                type: 'warning', displayTime: 3000,
            }, { position: 'top center' });
            return;
        }
        var cropIds = pickIds($cropTags);
        var fromVal = document.getElementById('rb-from').value || todayIso();
        var toVal   = document.getElementById('rb-to').value   || todayIso();
        $grid.beginCustomLoading('Loading…');

        fetch('/api/ReportBuilder/IntakeLocation', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                From: fromVal, To: toVal,
                LocationIds: locIds,
                CropIds: cropIds,
                GroupBySplit: isGroupBySplit(),
            })
        }).then(function (r) {
            if (!r.ok) throw new Error('HTTP ' + r.status);
            return r.json();
        }).then(function (rows) {
            applyGroupByVisibility();
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
        var worksheet = workbook.addWorksheet('Intake Location');
        DevExpress.excelExporter.exportDataGrid({
            component: $grid, worksheet: worksheet,
        }).then(function () {
            return workbook.xlsx.writeBuffer();
        }).then(function (buffer) {
            var fromVal = document.getElementById('rb-from').value || todayIso();
            var toVal   = document.getElementById('rb-to').value   || todayIso();
            saveAs(new Blob([buffer], { type: 'application/octet-stream' }),
                'IntakeLocation_' + fromVal + '_to_' + toVal + '.xlsx');
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        var t = todayIso();
        document.getElementById('rb-from').value = t;
        document.getElementById('rb-to').value   = t;

        buildTagBoxes();
        buildGrid();

        loadDistricts()
            .then(function () { return loadLocationOptions('0'); })
            .then(function () { return loadCropOptions(); });

        document.getElementById('rb-district').addEventListener('change', function () {
            loadLocationOptions(this.value).then(function () { return loadCropOptions(); });
        });
        document.getElementById('rb-from').addEventListener('change', loadCropOptions);
        document.getElementById('rb-to').addEventListener('change', loadCropOptions);

        document.getElementById('rb-groupby-primary').addEventListener('change', applyGroupByVisibility);
        document.getElementById('rb-groupby-split').addEventListener('change', applyGroupByVisibility);

        document.getElementById('rb-apply').addEventListener('click', fetchData);
        document.getElementById('rb-export').addEventListener('click', exportToExcel);
        document.getElementById('rb-search').addEventListener('input', function () {
            if ($grid) $grid.searchByText(this.value || '');
        });
    });
})();
