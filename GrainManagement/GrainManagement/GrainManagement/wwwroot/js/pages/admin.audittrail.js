(function () {
    'use strict';

    var _grid;

    $(function () {
        loadData();
    });

    // ── Data ──────────────────────────────────────────────────────────────────

    function loadData() {
        $.getJSON('/api/AuditTrail')
            .done(function (data) {
                initGrid(data);
            })
            .fail(function (xhr) {
                console.error('[AuditTrail] load failed', xhr.status, xhr.responseText);
            });
    }

    // ── JSON diff helpers ─────────────────────────────────────────────────────

    function tryParse(json) {
        if (!json) return null;
        try { return JSON.parse(json); } catch (e) { return null; }
    }

    function getJsonDiffs(oldJson, newJson) {
        var oldObj = tryParse(oldJson);
        var newObj = tryParse(newJson);
        var diffs = [];

        if (!oldObj && !newObj) return diffs;

        if (!oldObj) {
            // INSERT — everything in newObj is new
            if (newObj) {
                Object.keys(newObj).forEach(function (k) {
                    diffs.push({ key: k, oldVal: null, newVal: newObj[k] });
                });
            }
            return diffs;
        }

        if (!newObj) {
            // DELETE — everything in oldObj was removed
            Object.keys(oldObj).forEach(function (k) {
                diffs.push({ key: k, oldVal: oldObj[k], newVal: null });
            });
            return diffs;
        }

        // UPDATE — compare keys
        var allKeys = {};
        Object.keys(oldObj).forEach(function (k) { allKeys[k] = true; });
        Object.keys(newObj).forEach(function (k) { allKeys[k] = true; });

        Object.keys(allKeys).forEach(function (k) {
            var ov = oldObj[k] !== undefined ? oldObj[k] : null;
            var nv = newObj[k] !== undefined ? newObj[k] : null;
            if (String(ov) !== String(nv)) {
                diffs.push({ key: k, oldVal: ov, newVal: nv });
            }
        });

        return diffs;
    }

    function formatVal(v) {
        if (v === null || v === undefined) return '(empty)';
        return String(v);
    }

    // ── MOVE_LOAD action — custom rendering ─────────────────────────────────
    // Audit JSON shape (set by GrowerDeliveryApiController.MoveLoad):
    //   old: { As400Id, LoadId }
    //   new: { As400Id, LoadId }
    // The user who moved the load is captured in AuditTrail.UserName (the
    // "User" column in the grid), so it isn't repeated in the payload.

    function renderMoveLoadChanges(container, row) {
        var oldObj = tryParse(row.OldJson) || {};
        var newObj = tryParse(row.NewJson) || {};
        var $wrap = $('<div class="gm-at-diffs-wrap"></div>');

        $('<span class="gm-at-diff"></span>')
            .text('LoadId: ' + formatVal(newObj.LoadId !== undefined ? newObj.LoadId : oldObj.LoadId))
            .appendTo($wrap);

        $('<span class="gm-at-diff"></span>')
            .text('As400Id: ' + formatVal(oldObj.As400Id)
                  + ' \u2192 ' + formatVal(newObj.As400Id))
            .appendTo($wrap);

        $wrap.appendTo(container);
    }

    function moveLoadChangesText(row) {
        var oldObj = tryParse(row.OldJson) || {};
        var newObj = tryParse(row.NewJson) || {};
        var loadId = newObj.LoadId !== undefined ? newObj.LoadId : oldObj.LoadId;
        return 'LoadId: ' + formatVal(loadId)
            + '; As400Id: ' + formatVal(oldObj.As400Id)
            + ' -> ' + formatVal(newObj.As400Id);
    }

    // Preferred order for MOVE_LOAD detail rows; falls back to alphabetical
    // for any keys not in this list.
    var MOVE_LOAD_KEY_ORDER = ['As400Id', 'LoadId'];

    // ── Grid ──────────────────────────────────────────────────────────────────

    function initGrid(data) {
        _grid = $('#atGrid').dxDataGrid({
            dataSource: data,
            keyExpr: 'AuditId',
            showBorders: true,
            columnAutoWidth: true,
            rowAlternationEnabled: true,
            hoverStateEnabled: true,
            wordWrapEnabled: true,
            filterRow: { visible: true },
            headerFilter: { visible: true },
            searchPanel: { visible: true, width: 240, placeholder: 'Search audit log\u2026' },
            sorting: { mode: 'multiple' },
            paging: { pageSize: 25 },
            pager: {
                showPageSizeSelector: true,
                allowedPageSizes: [10, 25, 50, 100],
                showInfo: true,
            },
            export: {
                enabled: true,
                allowExportSelectedData: false,
            },
            toolbar: {
                items: [
                    { location: 'after', name: 'searchPanel' },
                    { location: 'after', name: 'exportButton' },
                ],
            },
            masterDetail: {
                enabled: true,
                template: masterDetailTemplate,
            },
            columns: [
                {
                    dataField: 'AuditId',
                    caption: 'ID',
                    width: 70,
                    dataType: 'number',
                },
                {
                    dataField: 'UserName',
                    caption: 'User',
                    width: 140,
                },
                {
                    // CreatedAt is stored UTC in system.AuditTrail; the API
                    // tags it Kind=Utc so the JSON carries a 'Z' suffix and
                    // dxDataGrid converts to the browser's local timezone.
                    // Default sort newest-first.
                    dataField: 'CreatedAt',
                    caption: 'Date (Local)',
                    dataType: 'datetime',
                    format: 'MM/dd/yyyy HH:mm:ss',
                    width: 170,
                    sortOrder: 'desc',
                    sortIndex: 0,
                },
                {
                    dataField: 'LocationName',
                    caption: 'Location',
                    calculateCellValue: function (row) {
                        if (!row.LocationName) return '(' + row.LocationId + ')';
                        return row.LocationName + ' (' + row.LocationId + ')';
                    },
                },
                {
                    dataField: 'ServerName',
                    caption: 'Server',
                    calculateCellValue: function (row) {
                        if (!row.ServerName) return '(' + row.ServerId + ')';
                        return row.ServerName + ' (' + row.ServerId + ')';
                    },
                    width: 150,
                },
                {
                    dataField: 'Action',
                    caption: 'Action',
                    width: 90,
                },
                {
                    dataField: 'KeyJson',
                    caption: 'Primary Key',
                    width: 160,
                },
                {
                    caption: 'Changes',
                    cellTemplate: function (container, options) {
                        var row = options.data;
                        if (row.Action === 'MOVE_LOAD') {
                            renderMoveLoadChanges(container, row);
                            return;
                        }
                        var diffs = getJsonDiffs(row.OldJson, row.NewJson);
                        if (diffs.length === 0) {
                            $('<span class="text-muted">No changes</span>').appendTo(container);
                            return;
                        }
                        var $wrap = $('<div class="gm-at-diffs-wrap"></div>');
                        diffs.forEach(function (d) {
                            var text = d.key + ': ' + formatVal(d.oldVal) + ' \u2192 ' + formatVal(d.newVal);
                            $('<span class="gm-at-diff"></span>').text(text).appendTo($wrap);
                        });
                        $wrap.appendTo(container);
                    },
                    allowFiltering: false,
                    allowSorting: false,
                    allowExporting: true,
                    calculateCellValue: function (row) {
                        // Plain-text version for export and search
                        if (row.Action === 'MOVE_LOAD') {
                            return moveLoadChangesText(row);
                        }
                        var diffs = getJsonDiffs(row.OldJson, row.NewJson);
                        return diffs.map(function (d) {
                            return d.key + ': ' + formatVal(d.oldVal) + ' -> ' + formatVal(d.newVal);
                        }).join('; ');
                    },
                },
            ],
            onExporting: function (e) {
                var workbook = new ExcelJS.Workbook();
                var worksheet = workbook.addWorksheet('AuditTrail');
                DevExpress.excelExporter.exportDataGrid({
                    component: e.component,
                    worksheet: worksheet,
                }).then(function () {
                    return workbook.xlsx.writeBuffer();
                }).then(function (buffer) {
                    saveAs(
                        new Blob([buffer], { type: 'application/octet-stream' }),
                        'AuditTrail.xlsx'
                    );
                });
                e.cancel = true;
            },
        }).dxDataGrid('instance');
    }

    // ── Master-detail template ────────────────────────────────────────────────

    function masterDetailTemplate(container, options) {
        var row = options.data;
        var oldObj = tryParse(row.OldJson) || {};
        var newObj = tryParse(row.NewJson) || {};
        var diffs = getJsonDiffs(row.OldJson, row.NewJson);
        var diffKeys = {};
        diffs.forEach(function (d) { diffKeys[d.key] = true; });

        // Collect all keys from both objects
        var allKeys = {};
        Object.keys(oldObj).forEach(function (k) { allKeys[k] = true; });
        Object.keys(newObj).forEach(function (k) { allKeys[k] = true; });
        var keys;
        if (row.Action === 'MOVE_LOAD') {
            // Render move-load fields in the operator-friendly order:
            //   ID, WeightSheetId, LoadId, MovedBy. Anything not on the list
            //   appended alphabetically after.
            var listed = MOVE_LOAD_KEY_ORDER.filter(function (k) { return allKeys[k]; });
            var extras = Object.keys(allKeys)
                .filter(function (k) { return MOVE_LOAD_KEY_ORDER.indexOf(k) === -1; })
                .sort();
            keys = listed.concat(extras);
        } else {
            keys = Object.keys(allKeys).sort();
        }

        var $detail = $('<div class="gm-at-detail"></div>');

        // Old values column
        var $oldCol = $('<div class="gm-at-detail__col"></div>');
        $oldCol.append('<div class="gm-at-detail__heading">Original Record</div>');
        if (Object.keys(oldObj).length === 0) {
            $oldCol.append('<div class="text-muted">(no data)</div>');
        } else {
            keys.forEach(function (k) {
                if (!(k in oldObj)) return;
                var cls = diffKeys[k] ? 'gm-at-kv gm-at-changed' : 'gm-at-kv';
                $oldCol.append(
                    '<div class="' + cls + '">' +
                    '<span class="gm-at-kv__key">' + escHtml(k) + '</span>' +
                    '<span class="gm-at-kv__val">' + escHtml(formatVal(oldObj[k])) + '</span>' +
                    '</div>'
                );
            });
        }

        // New values column
        var $newCol = $('<div class="gm-at-detail__col"></div>');
        $newCol.append('<div class="gm-at-detail__heading">Updated Record</div>');
        if (Object.keys(newObj).length === 0) {
            $newCol.append('<div class="text-muted">(no data)</div>');
        } else {
            keys.forEach(function (k) {
                if (!(k in newObj)) return;
                var cls = diffKeys[k] ? 'gm-at-kv gm-at-changed' : 'gm-at-kv';
                $newCol.append(
                    '<div class="' + cls + '">' +
                    '<span class="gm-at-kv__key">' + escHtml(k) + '</span>' +
                    '<span class="gm-at-kv__val">' + escHtml(formatVal(newObj[k])) + '</span>' +
                    '</div>'
                );
            });
        }

        $detail.append($oldCol).append($newCol);
        $(container).append($detail);
    }

    function escHtml(s) {
        var div = document.createElement('div');
        div.textContent = s;
        return div.innerHTML;
    }

})();
