(function () {
    'use strict';

    var REFRESH_MS = 60 * 1000;
    var LBS_PER_TON = 2000;
    // Filter state cookie. Stores from/to/search/activeOnly together with
    // the day they were saved so a stale cookie from yesterday gets
    // ignored — the user wanted the date range to reset on a new day.
    var FILTER_COOKIE = 'gm.cd.filters';
    var FILTER_COOKIE_DAYS = 7;

    function readFilterCookie() {
        var prefix = FILTER_COOKIE + '=';
        var parts = (document.cookie || '').split(';');
        for (var i = 0; i < parts.length; i++) {
            var c = parts[i].replace(/^\s+/, '');
            if (c.indexOf(prefix) === 0) {
                try { return JSON.parse(decodeURIComponent(c.substring(prefix.length))); }
                catch (e) { return null; }
            }
        }
        return null;
    }

    function writeFilterCookie(state) {
        var d = new Date();
        d.setDate(d.getDate() + FILTER_COOKIE_DAYS);
        document.cookie = FILTER_COOKIE + '=' + encodeURIComponent(JSON.stringify(state))
            + '; expires=' + d.toUTCString() + '; path=/; SameSite=Lax';
    }

    function saveCurrentFilterState() {
        writeFilterCookie({
            from: document.getElementById('cd-from').value || '',
            to: document.getElementById('cd-to').value || '',
            search: document.getElementById('cd-search').value || '',
            activeOnly: !!document.getElementById('cd-active-only').checked,
            savedOn: todayLocalIso(),
        });
    }

    var $grid       = null;
    var refreshTimer = null;
    var dataStore   = null;

    function todayLocalIso() {
        var d = new Date();
        var y = d.getFullYear();
        var m = String(d.getMonth() + 1).padStart(2, '0');
        var day = String(d.getDate()).padStart(2, '0');
        return y + '-' + m + '-' + day;
    }

    function formatTons(lbs) {
        if (lbs === null || lbs === undefined) return '';
        var tons = Number(lbs) / LBS_PER_TON;
        return tons.toLocaleString(undefined, {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2,
        });
    }

    function formatBu(bu) {
        if (bu === null || bu === undefined) return '';
        return Math.round(Number(bu)).toLocaleString();
    }

    function fetchData() {
        var fromVal = document.getElementById('cd-from').value || todayLocalIso();
        var toVal   = document.getElementById('cd-to').value   || todayLocalIso();
        var url = '/api/CentralDashboard'
            + '?from=' + encodeURIComponent(fromVal)
            + '&to='   + encodeURIComponent(toVal);

        return $.getJSON(url).then(function (resp) {
            var stamp = new Date();
            document.getElementById('cd-last-updated').textContent =
                'Last updated: ' + stamp.toLocaleTimeString();
            return (resp && resp.Rows) || [];
        }).fail(function () {
            document.getElementById('cd-last-updated').textContent =
                'Failed to refresh at ' + new Date().toLocaleTimeString();
            return [];
        });
    }

    function buildStore() {
        return new DevExpress.data.CustomStore({
            key: 'LocationId',
            load: function () { return fetchData(); },
        });
    }

    function applyActiveFilter(grid) {
        var activeOnly = document.getElementById('cd-active-only').checked;
        if (activeOnly) {
            grid.filter(['HasActivity', '=', true]);
        } else {
            grid.clearFilter();
        }
    }

    function buildGrid() {
        dataStore = buildStore();

        $grid = $('#cd-grid').dxDataGrid({
            dataSource: dataStore,
            showBorders: true,
            showRowLines: true,
            rowAlternationEnabled: true,
            columnAutoWidth: true,
            hoverStateEnabled: true,
            noDataText: 'No activity in this date range.',
            searchPanel: { visible: false },
            paging:  { enabled: true, pageSize: 50 },
            pager:   { visible: true, showInfo: true, showPageSizeSelector: true, allowedPageSizes: [25, 50, 100, 'all'] },
            sorting: { mode: 'multiple' },
            summary: {
                totalItems: [
                    { column: 'IntakeLoads',      summaryType: 'sum', displayFormat: '{0}' },
                    { column: 'IntakeLbs',        summaryType: 'sum', valueFormat: function (v) { return formatTons(v); }, displayFormat: '{0}' },
                    { column: 'IntakeBu',         summaryType: 'sum', valueFormat: function (v) { return formatBu(v); },   displayFormat: '{0}' },
                    { column: 'TransferInLoads',  summaryType: 'sum', displayFormat: '{0}' },
                    { column: 'TransferInLbs',    summaryType: 'sum', valueFormat: function (v) { return formatTons(v); }, displayFormat: '{0}' },
                    { column: 'TransferInBu',     summaryType: 'sum', valueFormat: function (v) { return formatBu(v); },   displayFormat: '{0}' },
                    { column: 'TransferOutLoads', summaryType: 'sum', displayFormat: '{0}' },
                    { column: 'TransferOutLbs',   summaryType: 'sum', valueFormat: function (v) { return '-' + formatTons(v); }, displayFormat: '{0}' },
                    { column: 'TransferOutBu',    summaryType: 'sum', valueFormat: function (v) { return '-' + formatBu(v); },   displayFormat: '{0}' },
                ],
            },
            columns: [
                {
                    dataField: 'LocationId',
                    caption: 'ID',
                    width: 70,
                    alignment: 'right',
                    sortOrder: 'asc',
                },
                {
                    dataField: 'LocationName',
                    caption: 'Location',
                    minWidth: 180,
                },
                {
                    caption: 'Intake',
                    alignment: 'center',
                    columns: [
                        { dataField: 'IntakeLoads', caption: '#', width: 70, alignment: 'right', dataType: 'number' },
                        {
                            dataField: 'IntakeLbs', caption: 'Tons', alignment: 'right',
                            calculateCellValue: function (row) { return Number(row.IntakeLbs || 0); },
                            customizeText: function (info) { return formatTons(info.value); },
                            sortingMethod: function (a, b) { return Number(a) - Number(b); },
                        },
                        {
                            dataField: 'IntakeBu', caption: 'Bushels', alignment: 'right',
                            customizeText: function (info) { return formatBu(info.value); },
                        },
                    ],
                },
                {
                    caption: 'Transfer In',
                    alignment: 'center',
                    columns: [
                        { dataField: 'TransferInLoads', caption: '#', width: 70, alignment: 'right', dataType: 'number' },
                        {
                            dataField: 'TransferInLbs', caption: 'Tons', alignment: 'right',
                            customizeText: function (info) { return formatTons(info.value); },
                        },
                        {
                            dataField: 'TransferInBu', caption: 'Bushels', alignment: 'right',
                            customizeText: function (info) { return formatBu(info.value); },
                        },
                    ],
                },
                {
                    caption: 'Transfer Out',
                    alignment: 'center',
                    columns: [
                        { dataField: 'TransferOutLoads', caption: '#', width: 70, alignment: 'right', dataType: 'number' },
                        {
                            dataField: 'TransferOutLbs', caption: 'Tons', alignment: 'right',
                            customizeText: function (info) {
                                var v = Number(info.value || 0);
                                return v === 0 ? formatTons(0) : '-' + formatTons(v);
                            },
                            cssClass: 'text-danger',
                        },
                        {
                            dataField: 'TransferOutBu', caption: 'Bushels', alignment: 'right',
                            customizeText: function (info) {
                                var v = Number(info.value || 0);
                                return v === 0 ? formatBu(0) : '-' + formatBu(v);
                            },
                            cssClass: 'text-danger',
                        },
                    ],
                },
                {
                    caption: 'Net',
                    alignment: 'center',
                    columns: [
                        {
                            dataField: 'NetLbs', caption: 'Tons', alignment: 'right',
                            customizeText: function (info) { return formatTons(info.value); },
                            cellTemplate: function (container, options) {
                                var v = Number(options.value || 0);
                                $('<span>')
                                    .text(formatTons(v))
                                    .addClass(v < 0 ? 'text-danger' : '')
                                    .appendTo(container);
                            },
                        },
                        {
                            dataField: 'NetBu', caption: 'Bushels', alignment: 'right',
                            cellTemplate: function (container, options) {
                                var v = Number(options.value || 0);
                                $('<span>')
                                    .text(formatBu(v))
                                    .addClass(v < 0 ? 'text-danger' : '')
                                    .appendTo(container);
                            },
                        },
                    ],
                },
                {
                    dataField: 'HasActivity',
                    visible: false,
                    showInColumnChooser: false,
                },
            ],
            onContentReady: function (e) {
                applyActiveFilter(e.component);
            },
        }).dxDataGrid('instance');
    }

    function refresh() {
        if (!$grid) return;
        $grid.refresh();
    }

    function startRefreshTimer() {
        stopRefreshTimer();
        refreshTimer = setInterval(refresh, REFRESH_MS);
    }

    function stopRefreshTimer() {
        if (refreshTimer) {
            clearInterval(refreshTimer);
            refreshTimer = null;
        }
    }

    $(function () {
        if (!document.getElementById('cd-grid')) return;

        var today = todayLocalIso();
        // Restore filters from cookie if it was saved earlier today;
        // otherwise default everything (date range, search, active-only).
        var saved = readFilterCookie();
        var sameDay = saved && saved.savedOn === today;
        document.getElementById('cd-from').value = sameDay && saved.from ? saved.from : today;
        document.getElementById('cd-to').value   = sameDay && saved.to   ? saved.to   : today;
        document.getElementById('cd-search').value = sameDay ? (saved.search || '') : '';
        document.getElementById('cd-active-only').checked =
            sameDay ? !!saved.activeOnly : true;

        buildGrid();
        // Push the restored search text into the grid (the input change
        // handler is the only other place that calls searchByText, and
        // setting input.value on a fresh page doesn't fire input events).
        var initialSearch = document.getElementById('cd-search').value || '';
        if ($grid && initialSearch) $grid.searchByText(initialSearch);
        startRefreshTimer();

        // Date inputs auto-apply on change — no Apply button.
        function onDateChange() {
            saveCurrentFilterState();
            refresh();
        }
        document.getElementById('cd-from').addEventListener('change', onDateChange);
        document.getElementById('cd-to').addEventListener('change', onDateChange);
        document.getElementById('cd-active-only').addEventListener('change', function () {
            if ($grid) applyActiveFilter($grid);
            saveCurrentFilterState();
        });
        document.getElementById('cd-search').addEventListener('input', function () {
            if ($grid) $grid.searchByText(this.value || '');
            saveCurrentFilterState();
        });
        document.getElementById('cd-export').addEventListener('click', function () {
            if (!$grid) return;
            var workbook = new ExcelJS.Workbook();
            var worksheet = workbook.addWorksheet('Activity by Location');
            DevExpress.excelExporter.exportDataGrid({
                component: $grid,
                worksheet: worksheet,
            }).then(function () {
                return workbook.xlsx.writeBuffer();
            }).then(function (buffer) {
                var fromVal = document.getElementById('cd-from').value || todayLocalIso();
                var toVal   = document.getElementById('cd-to').value   || todayLocalIso();
                var fname = 'ActivityByLocation_' + fromVal + '_to_' + toVal + '.xlsx';
                saveAs(new Blob([buffer], { type: 'application/octet-stream' }), fname);
            });
        });

        // Stop polling when the tab is hidden; resume when visible.
        document.addEventListener('visibilitychange', function () {
            if (document.hidden) stopRefreshTimer(); else startRefreshTimer();
        });
    });
})();
