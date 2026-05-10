(function () {
    'use strict';

    var SERVICE_ID = 'as400sync';
    var DISTRICT_COOKIE = 'GrainMgmt_AwtDistrictId';

    // ── DOM refs ────────────────────────────────────────────────────────
    var elFrom        = document.getElementById('awt-from');
    var elTo          = document.getElementById('awt-to');
    var elDistrict    = document.getElementById('awt-district');
    var elRefresh     = document.getElementById('awt-refresh');
    var elLocSection  = document.getElementById('awt-locations-section');
    var elLocs        = document.getElementById('awt-locations');
    var elLocAll      = document.getElementById('awt-loc-all');
    var elLocNone     = document.getElementById('awt-loc-none');
    var elLocCollapse = document.getElementById('awt-locations-collapse');
    var elLocToggleIc = document.getElementById('awt-loc-toggle-icon');
    var elLocSummary  = document.getElementById('awt-loc-summary');
    var elSheetGrid   = document.getElementById('awt-sheet-grid');
    var elSheetSum    = document.getElementById('awt-sheet-summary');
    var elUpload      = document.getElementById('awt-upload');
    var elClear       = document.getElementById('awt-clear');
    var elPreview     = document.getElementById('awt-preview');
    var elPdfModal    = document.getElementById('awt-pdf-modal');
    var elPdfIframe   = document.getElementById('awt-pdf-iframe');
    var elPdfSpinner  = document.getElementById('awt-pdf-spinner');
    var elPdfDownload = document.getElementById('awt-pdf-download');
    var elPdfClose    = document.getElementById('awt-pdf-close');
    var elPdfCloseX   = document.getElementById('awt-pdf-close-x');
    var elConn        = document.getElementById('awt-conn');
    var elVersion     = document.getElementById('awt-version');
    var elJob         = document.getElementById('awt-status-job');
    var elStage       = document.getElementById('awt-status-stage');
    var elMessage     = document.getElementById('awt-status-message');
    var elBar         = document.getElementById('awt-progress-bar');
    var elCounter     = document.getElementById('awt-status-counter');
    var elTime        = document.getElementById('awt-status-time');
    var elErrors      = document.getElementById('awt-errors');
    var elErrorsList  = document.getElementById('awt-errors-list');
    var elLog         = document.getElementById('awt-log');

    // ── State ───────────────────────────────────────────────────────────
    var sheetGrid = null;
    var serviceConnected = false;
    var jobRunning = false;

    // ── Helpers ─────────────────────────────────────────────────────────
    function setCookie(name, value, days) {
        var d = new Date(); d.setTime(d.getTime() + (days * 86400000));
        document.cookie = name + '=' + encodeURIComponent(value) + '; expires=' + d.toUTCString() + '; path=/';
    }
    function getCookie(name) {
        var m = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
        return m ? decodeURIComponent(m[2]) : null;
    }
    function pacificTodayMinusOne() {
        var nowUtc = new Date();
        // Pacific offset is -7 (DST) or -8. Easiest portable approach: render
        // through toLocaleString in en-CA which gives "yyyy-mm-dd hh:mm:ss"
        // for the Pacific zone, then subtract one day.
        var ptStr = nowUtc.toLocaleString('en-CA', { timeZone: 'America/Los_Angeles' });
        var ymd = (ptStr.split(',')[0] || ptStr.split(' ')[0]).trim();
        var parts = ymd.split('-');
        if (parts.length !== 3) return new Date(Date.now() - 86400000).toISOString().slice(0, 10);
        var y = parseInt(parts[0], 10), m = parseInt(parts[1], 10), d = parseInt(parts[2], 10);
        var dt = new Date(Date.UTC(y, m - 1, d));
        dt.setUTCDate(dt.getUTCDate() - 1);
        return dt.toISOString().slice(0, 10);
    }
    function fmtNum(n) { return (n === null || n === undefined) ? '' : Number(n).toLocaleString(); }
    function fmtTons(lbs) {
        if (lbs === null || lbs === undefined) return '';
        return (Number(lbs) / 2000).toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    }
    function appendLog(line) {
        var ts = new Date().toLocaleTimeString();
        elLog.textContent = '[' + ts + '] ' + line + '\n' + elLog.textContent;
    }

    // ── Districts / locations / sheets ─────────────────────────────────
    function loadDistricts() {
        return $.getJSON('/api/AgvantageWarehouseTransfer/Districts').then(function (rows) {
            elDistrict.innerHTML = '<option value="">-- Select district --</option>';
            (rows || []).forEach(function (d) {
                var opt = document.createElement('option');
                opt.value = String(d.DistrictId);
                opt.textContent = d.Name;
                elDistrict.appendChild(opt);
            });
            var saved = getCookie(DISTRICT_COOKIE);
            if (saved && elDistrict.querySelector('option[value="' + saved + '"]')) {
                elDistrict.value = saved;
                onDistrictChanged();
            }
        }).fail(function (jq) { appendLog('Failed to load districts: ' + jq.status); });
    }

    function onDistrictChanged() {
        var id = elDistrict.value;
        elLocs.innerHTML = '';
        elLocSection.hidden = true;
        clearSheetGrid();
        if (!id) return;

        setCookie(DISTRICT_COOKIE, id, 365);

        $.getJSON('/api/AgvantageWarehouseTransfer/Locations?districtId=' + encodeURIComponent(id))
            .then(function (rows) {
                if (!rows || rows.length === 0) {
                    elLocs.innerHTML = '<span class="text-muted small">No warehouse locations in this district.</span>';
                    elLocSection.hidden = false;
                    return;
                }
                rows.forEach(function (loc) {
                    var lab = document.createElement('label');
                    lab.className = 'form-check form-check-inline mb-0';
                    var cb = document.createElement('input');
                    cb.type = 'checkbox';
                    cb.className = 'form-check-input awt-loc-cb';
                    cb.value = String(loc.LocationId);
                    cb.checked = true;
                    cb.addEventListener('change', function () { updateLocationSummary(); refreshSheets(); });
                    lab.appendChild(cb);
                    var span = document.createElement('span');
                    span.className = 'form-check-label small';
                    span.textContent = loc.Name + (loc.Code ? ' (' + loc.Code + ')' : '');
                    lab.appendChild(span);
                    elLocs.appendChild(lab);
                });
                elLocSection.hidden = false;
                updateLocationSummary();
                refreshSheets();
            })
            .fail(function (jq) { appendLog('Failed to load locations: ' + jq.status); });
    }

    function selectedLocationIds() {
        return Array.from(elLocs.querySelectorAll('.awt-loc-cb:checked')).map(function (cb) { return parseInt(cb.value, 10); });
    }

    function updateLocationSummary() {
        if (!elLocSummary) return;
        var total = elLocs.querySelectorAll('.awt-loc-cb').length;
        var sel = selectedLocationIds().length;
        elLocSummary.textContent = total === 0 ? '' : '(' + sel + ' of ' + total + ' selected)';
    }

    function setLocationsCollapsed(collapsed) {
        if (!elLocCollapse || !window.bootstrap || !window.bootstrap.Collapse) return;
        var inst = window.bootstrap.Collapse.getOrCreateInstance(elLocCollapse, { toggle: false });
        if (collapsed) inst.hide(); else inst.show();
    }

    function refreshSheets() {
        var locs = selectedLocationIds();
        elRefresh.disabled = locs.length === 0;
        if (locs.length === 0) { clearSheetGrid(); return; }

        var body = {
            From: elFrom.value,
            To: elTo.value,
            LocationIds: locs,
        };

        $.ajax({
            url: '/api/AgvantageWarehouseTransfer/Sheets',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(body),
        }).then(function (rows) {
            renderSheetGrid(rows || []);
        }).fail(function (jq) {
            appendLog('Failed to load sheets: ' + jq.status);
            renderSheetGrid([]);
        });
    }

    function clearSheetGrid() {
        if (sheetGrid) { sheetGrid.option('dataSource', []); }
        elSheetSum.textContent = '';
        elUpload.disabled = true;
    }

    function renderSheetGrid(rows) {
        rows.forEach(function (r) { r.Selected = true; });
        if (!sheetGrid) {
            sheetGrid = $(elSheetGrid).dxDataGrid({
                dataSource: rows,
                keyExpr: 'WeightSheetId',
                showBorders: true,
                rowAlternationEnabled: true,
                columnAutoWidth: true,
                paging: { enabled: true, pageSize: 50 },
                pager: { visible: true, showInfo: true, showPageSizeSelector: true, allowedPageSizes: [25, 50, 100, 'all'] },
                sorting: { mode: 'multiple' },
                selection: { mode: 'multiple', showCheckBoxesMode: 'always', allowSelectAll: true },
                onSelectionChanged: function (e) {
                    elUpload.disabled = jobRunning || (e.selectedRowKeys || []).length === 0;
                    updateSummary(e.component.getDataSource().items().length, (e.selectedRowKeys || []).length);
                },
                onContentReady: function (e) {
                    if (e.component.getSelectedRowKeys().length === 0) {
                        e.component.selectAll();
                    }
                },
                noDataText: 'No closed weight sheets in this filter.',
                columns: [
                    { dataField: 'As400Id',         caption: 'AS400 ID',     width: 110, alignment: 'right' },
                    { dataField: 'WeightSheetType', caption: 'Type',         width: 90 },
                    { dataField: 'CreationDate',    caption: 'Created',      dataType: 'date', format: 'yyyy-MM-dd', width: 120 },
                    { dataField: 'LocationName',    caption: 'Location' },
                    {
                        caption: 'From → To',
                        calculateCellValue: function (r) {
                            if (r.WeightSheetType === 'Transfer') {
                                return (r.SourceLocationName || '?') + ' → ' + (r.DestinationLocationName || '?');
                            }
                            return '';
                        },
                    },
                    { dataField: 'Commodity',       caption: 'Commodity' },
                    { dataField: 'LotAs400Id',      caption: 'Lot',          width: 110, alignment: 'right' },
                    {
                        dataField: 'NetLbs', caption: 'Net (tons)', width: 110, alignment: 'right',
                        customizeText: function (info) { return fmtTons(info.value); },
                    },
                ],
            }).dxDataGrid('instance');
        } else {
            sheetGrid.option('dataSource', rows);
        }
        updateSummary(rows.length, rows.length);
    }

    function updateSummary(total, selected) {
        elSheetSum.textContent = total === 0 ? 'No matching weight sheets.'
            : (selected + ' of ' + total + ' selected');
        elUpload.disabled = jobRunning || selected === 0;
    }

    // ── Progress panel ──────────────────────────────────────────────────
    function setBar(pct, indeterminate) {
        var clamped = Math.max(0, Math.min(100, pct));
        elBar.style.width = clamped + '%';
        if (indeterminate) {
            elBar.classList.add('progress-bar-striped', 'progress-bar-animated');
        } else {
            elBar.classList.remove('progress-bar-striped', 'progress-bar-animated');
        }
    }

    function setConnBadge(state) {
        elConn.classList.remove('bg-secondary', 'bg-success', 'bg-danger', 'bg-warning');
        if (state === 'connected')         { elConn.textContent = 'Service connected'; elConn.classList.add('bg-success'); }
        else if (state === 'disconnected') { elConn.textContent = 'Service offline';   elConn.classList.add('bg-danger'); }
        else if (state === 'hub-disc')     { elConn.textContent = 'Hub disconnected';  elConn.classList.add('bg-warning'); }
        else                                { elConn.textContent = 'Connecting...';    elConn.classList.add('bg-secondary'); }
        refreshUploadButton();
    }

    function refreshUploadButton() {
        var hasSelection = sheetGrid ? (sheetGrid.getSelectedRowKeys() || []).length > 0 : false;
        elUpload.disabled = !serviceConnected || jobRunning || !hasSelection;
        if (elPreview) elPreview.disabled = jobRunning || !hasSelection;
        if (elClear)   elClear.disabled   = !serviceConnected || jobRunning;
    }

    function applyStatus(s) {
        if (!s) return;
        var stage = s.Stage || s.stage || '';
        var job   = s.Job   || s.job   || '';
        var msg   = s.Message || s.message || '';
        var cur   = s.Current != null ? s.Current : s.current;
        var tot   = s.Total   != null ? s.Total   : s.total;
        var ver   = s.Version || s.version;
        if (ver) elVersion.textContent = 'v' + ver;

        elJob.textContent = job ? '(' + job + ')' : '';
        elStage.textContent = stage || 'Idle.';
        elMessage.textContent = msg || '';

        var hasTotal = (tot != null && Number(tot) > 0);
        var curN = (cur != null) ? Number(cur) : 0;
        var totN = hasTotal ? Number(tot) : 0;

        if (stage === 'Done') {
            elCounter.textContent = hasTotal
                ? fmtNum(totN) + ' of ' + fmtNum(totN) + ' weight sheets uploaded'
                : fmtNum(curN) + ' weight sheets uploaded';
            setBar(100, false);
        } else if (stage === 'Error') {
            elCounter.textContent = hasTotal ? fmtNum(curN) + ' of ' + fmtNum(totN) + ' uploaded' : '';
            setBar(0, false);
        } else if (hasTotal) {
            elCounter.textContent = fmtNum(curN) + ' of ' + fmtNum(totN) + ' weight sheets uploaded';
            setBar((curN / totN) * 100, false);
        } else if (curN > 0) {
            elCounter.textContent = fmtNum(curN) + ' weight sheets uploaded';
            setBar(100, true);
        } else {
            elCounter.textContent = '';
            setBar(0, stage === 'Counting' || stage === 'Connecting');
        }
        elTime.textContent = new Date().toLocaleTimeString();

        jobRunning = !!stage && stage !== 'Idle' && stage !== 'Done' && stage !== 'Error';
        refreshUploadButton();
    }

    function pushError(item) {
        var li = document.createElement('li');
        var ws = item && (item.WeightSheetId || item.weightSheetId);
        var msg = item && (item.Message || item.message) || 'Unknown error';
        li.textContent = (ws ? 'WS ' + ws + ': ' : '') + msg;
        elErrorsList.appendChild(li);
        elErrors.hidden = false;
    }

    // ── SignalR ─────────────────────────────────────────────────────────
    var connection = null;
    function connectHub() {
        if (typeof signalR === 'undefined') {
            elStage.textContent = 'SignalR client not loaded.';
            return;
        }
        connection = new signalR.HubConnectionBuilder()
            .withUrl('/hubs/as400sync')
            .withAutomaticReconnect()
            .build();

        connection.on('ServiceConnected', function (sid) {
            if (sid === SERVICE_ID) {
                serviceConnected = true; setConnBadge('connected');
                appendLog('Service connected: ' + sid);
                connection.invoke('RequestSnapshot', SERVICE_ID).catch(function () {});
            }
        });
        connection.on('ServiceDisconnected', function (sid) {
            if (sid === SERVICE_ID) {
                serviceConnected = false; setConnBadge('disconnected');
                appendLog('Service disconnected: ' + sid);
            }
        });
        connection.on('Status', function (s) {
            applyStatus(s);
            if (s) appendLog(((s.Job || s.job) || '') + ' ' + ((s.Stage || s.stage) || '') + (s.Message ? ' — ' + s.Message : ''));
        });
        connection.on('Completed', function (r) {
            jobRunning = false; refreshUploadButton();
            var msg = (r && (r.Message || r.message)) || 'Upload complete.';
            elStage.textContent = 'Upload complete.';
            elMessage.textContent = msg;
            setBar(100, false);
            appendLog('DONE: ' + msg);
            setLocationsCollapsed(false);
        });
        connection.on('Error', function (e) {
            jobRunning = false; refreshUploadButton();
            var msg = (e && (e.Message || e.message)) || 'Upload failed.';
            elStage.textContent = 'Error';
            elMessage.textContent = msg;
            setBar(0, false);
            pushError(e);
            appendLog('ERROR: ' + msg);
            setLocationsCollapsed(false);
        });

        connection.onreconnecting(function () { setConnBadge('hub-disc'); });
        connection.onreconnected(function () { joinAndProbe(); });
        connection.onclose(function () { setConnBadge('hub-disc'); });

        connection.start().then(joinAndProbe).catch(function (err) {
            console.error('AWT hub connect failed', err);
            setConnBadge('hub-disc');
        });
    }

    function joinAndProbe() {
        connection.invoke('JoinWatcher')
            .then(function () { return connection.invoke('GetConnectedServices'); })
            .then(function (services) {
                services = services || [];
                if (services.indexOf(SERVICE_ID) >= 0) {
                    serviceConnected = true; setConnBadge('connected');
                    connection.invoke('RequestSnapshot', SERVICE_ID).catch(function () {});
                } else {
                    serviceConnected = false; setConnBadge('disconnected');
                }
            })
            .catch(function () { setConnBadge('hub-disc'); });
    }

    // ── Clear U5SILOAD ─────────────────────────────────────────────────
    function startClear() {
        if (!serviceConnected || jobRunning) return;

        var html =
            '<div class="text-start">' +
              '<div class="alert alert-danger d-flex align-items-start mb-2" role="alert" style="margin-bottom:0;">' +
                '<i class="dx-icon dx-icon-warning fs-3 me-2" aria-hidden="true"></i>' +
                '<div>' +
                  '<div class="fw-bold mb-1">Clear Last Uploads On Agvantage?</div>' +
                  '<div class="small">' +
                    'This wipes every row from <code>U5SILOAD</code> on Agvantage. ' +
                    'It cannot be undone.' +
                  '</div>' +
                '</div>' +
              '</div>' +
            '</div>';

        // DevExtreme is already loaded by _Layout (dx.all.js). Use the
        // native confirm dialog so the prompt matches the rest of the
        // project's modal style. Falls back to window.confirm if DX
        // isn't on the page for some reason (e.g. asset cache miss).
        var ask;
        if (window.DevExpress && DevExpress.ui && DevExpress.ui.dialog) {
            ask = DevExpress.ui.dialog.custom({
                title: 'Clear Last Uploads On Agvantage',
                messageHtml: html,
                buttons: [
                    { text: 'Clear', type: 'danger', stylingMode: 'contained', onClick: function () { return true; } },
                    { text: 'Cancel', stylingMode: 'outlined',                  onClick: function () { return false; } },
                ],
            }).show();
        } else {
            ask = $.Deferred().resolve(window.confirm(
                'Clear Last Uploads On Agvantage? This wipes every row from U5SILOAD and cannot be undone.'
            )).promise();
        }

        $.when(ask).then(function (ok) {
            if (!ok) return;

            jobRunning = true; refreshUploadButton();
            elErrorsList.innerHTML = ''; elErrors.hidden = true;
            elStage.textContent = 'Clearing last uploads on Agvantage...';
            elMessage.textContent = 'Sending DELETE to AS400.';
            setBar(0, true);
            setLocationsCollapsed(true);

            connection.invoke('RunClearU5Siload', SERVICE_ID)
                .catch(function (err) {
                    jobRunning = false; refreshUploadButton();
                    elStage.textContent = 'Error';
                    elMessage.textContent = (err && err.message) || 'Failed to send command.';
                    appendLog('ERROR sending RunClearU5Siload: ' + (err && err.message));
                    setLocationsCollapsed(false);
                });
        });
    }

    // ── PDF preview ────────────────────────────────────────────────────
    var pdfBlobUrl = null;
    function freePdfBlob() {
        if (pdfBlobUrl) {
            try { URL.revokeObjectURL(pdfBlobUrl); } catch (e) { /* ignore */ }
            pdfBlobUrl = null;
        }
    }

    function openPreview() {
        var ids = sheetGrid ? (sheetGrid.getSelectedRowKeys() || []) : [];
        if (ids.length === 0) return;

        // Show modal with spinner; iframe will appear once the blob loads.
        if (window.bootstrap && window.bootstrap.Modal) {
            var inst = window.bootstrap.Modal.getOrCreateInstance(elPdfModal);
            inst.show();
        } else {
            elPdfModal.style.display = 'block';
        }
        elPdfIframe.style.display = 'none';
        elPdfSpinner.style.display = '';
        if (elPdfDownload) { elPdfDownload.hidden = true; elPdfDownload.removeAttribute('href'); }

        fetch('/api/printjobs/weight-sheets/combined-pdf', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ WeightSheetIds: ids }),
        }).then(function (resp) {
            if (!resp.ok) throw new Error('PDF endpoint returned ' + resp.status);
            return resp.blob();
        }).then(function (blob) {
            freePdfBlob();
            pdfBlobUrl = URL.createObjectURL(blob);
            elPdfIframe.src = pdfBlobUrl;
            elPdfIframe.style.display = '';
            elPdfSpinner.style.display = 'none';
            if (elPdfDownload) {
                elPdfDownload.hidden = false;
                elPdfDownload.href = pdfBlobUrl;
                elPdfDownload.download = 'WeightSheets-' + new Date().toISOString().slice(0, 10) + '.pdf';
            }
        }).catch(function (err) {
            elPdfSpinner.innerHTML = '<div class="text-danger">Failed to build PDF.<br><span class="small">' + (err.message || err) + '</span></div>';
        });
    }

    function closePreview() {
        if (window.bootstrap && window.bootstrap.Modal) {
            var inst = window.bootstrap.Modal.getOrCreateInstance(elPdfModal);
            inst.hide();
        } else {
            elPdfModal.style.display = 'none';
        }
        // Free the blob after the modal animation completes.
        setTimeout(function () {
            elPdfIframe.src = 'about:blank';
            freePdfBlob();
        }, 300);
    }

    // ── Upload trigger ─────────────────────────────────────────────────
    function startUpload() {
        if (!serviceConnected || jobRunning) return;
        var ids = sheetGrid ? (sheetGrid.getSelectedRowKeys() || []) : [];
        if (ids.length === 0) return;

        elErrorsList.innerHTML = ''; elErrors.hidden = true;
        jobRunning = true; refreshUploadButton();
        elStage.textContent = 'Requesting...';
        elMessage.textContent = 'Sending ' + ids.length + ' weight sheets to the AS400 sync service.';
        setBar(0, true);

        // Hide the locations picker so the status / progress card has
        // room without forcing the user to scroll.
        setLocationsCollapsed(true);

        connection.invoke('RunWarehouseTransferUpload', SERVICE_ID, ids)
            .catch(function (err) {
                jobRunning = false; refreshUploadButton();
                elStage.textContent = 'Error';
                elMessage.textContent = (err && err.message) || 'Failed to send command.';
                appendLog('ERROR sending RunWarehouseTransferUpload: ' + (err && err.message));
                setLocationsCollapsed(false);
            });
    }

    // ── Wire up ─────────────────────────────────────────────────────────
    $(function () {
        var defaultDate = pacificTodayMinusOne();
        elFrom.value = defaultDate;
        elTo.value   = defaultDate;

        elDistrict.addEventListener('change', onDistrictChanged);
        elFrom.addEventListener('change', refreshSheets);
        elTo.addEventListener('change', refreshSheets);
        elRefresh.addEventListener('click', refreshSheets);

        elLocAll.addEventListener('click', function () {
            elLocs.querySelectorAll('.awt-loc-cb').forEach(function (cb) { cb.checked = true; });
            updateLocationSummary();
            refreshSheets();
        });
        elLocNone.addEventListener('click', function () {
            elLocs.querySelectorAll('.awt-loc-cb').forEach(function (cb) { cb.checked = false; });
            updateLocationSummary();
            refreshSheets();
        });
        elUpload.addEventListener('click', startUpload);
        if (elClear)   elClear.addEventListener('click', startClear);
        if (elPreview) elPreview.addEventListener('click', openPreview);
        if (elPdfClose)  elPdfClose.addEventListener('click', closePreview);
        if (elPdfCloseX) elPdfCloseX.addEventListener('click', closePreview);

        // Swap the chevron icon when the locations panel collapses /
        // expands so the header reads as a regular accordion control.
        if (elLocCollapse) {
            elLocCollapse.addEventListener('hide.bs.collapse', function () {
                if (elLocToggleIc) elLocToggleIc.className = 'dx-icon dx-icon-chevronright';
                elLocToggleIc.style.marginRight = '4px';
            });
            elLocCollapse.addEventListener('show.bs.collapse', function () {
                if (elLocToggleIc) elLocToggleIc.className = 'dx-icon dx-icon-chevrondown';
                elLocToggleIc.style.marginRight = '4px';
            });
        }

        loadDistricts();
        connectHub();
    });
})();
