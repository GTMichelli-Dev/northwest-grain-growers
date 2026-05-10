// Daily Intake/Transfer report picker. Loads districts → locations,
// queries the API for (location, date, type) candidates that had
// activity in the date range, lets the operator click a row's "Print"
// button to view the corresponding XtraReport in an iframe modal.
(function () {
    'use strict';

    var pdfModal = null;

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
        sel.innerHTML = '<option value="">— Select —</option>';
        // districtId 0 = "All" — the API returns every warehouse-enabled
        // location. Use that for the initial page load too.
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
            });
    }

    function escapeHtml(s) {
        return String(s == null ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }

    function renderResults(rows) {
        var container = document.getElementById('rb-results');
        if (!rows || !rows.length) {
            container.innerHTML = '<div class="alert alert-info mb-0">No matching activity in this date range.</div>';
            return;
        }
        var bodyRows = rows.map(function (r) {
            var typeBadge = r.Type === 'Intake'
                ? '<span class="badge bg-success">Intake</span>'
                : '<span class="badge bg-primary">Transfer</span>';
            return '<tr>' +
                '<td>' + typeBadge + '</td>' +
                '<td>' + escapeHtml(r.DateDisplay) + '</td>' +
                '<td>' + escapeHtml(r.LocationName) + '</td>' +
                '<td class="text-end">' + (r.LoadCount || 0) + '</td>' +
                '<td class="text-end">' +
                    '<button class="btn btn-sm btn-outline-primary rb-print"' +
                    ' data-loc-id="' + escapeHtml(r.LocationId) + '"' +
                    ' data-date="' + escapeHtml(r.Date) + '"' +
                    ' data-date-display="' + escapeHtml(r.DateDisplay) + '"' +
                    ' data-loc-name="' + escapeHtml(r.LocationName) + '"' +
                    ' data-type="' + escapeHtml(r.Type) + '">' +
                    '<i class="fa fa-print"></i> Print</button>' +
                '</td>' +
            '</tr>';
        }).join('');

        container.innerHTML =
            '<table class="table table-sm table-hover align-middle">' +
              '<thead class="table-light">' +
                '<tr><th>Type</th><th>Date</th><th>Location</th><th class="text-end">Loads</th><th class="text-end">Print</th></tr>' +
              '</thead>' +
              '<tbody>' + bodyRows + '</tbody>' +
            '</table>';
    }

    function fetchCandidates() {
        var locId = parseInt(document.getElementById('rb-location').value, 10);
        if (!locId) {
            document.getElementById('rb-results').innerHTML =
                '<div class="alert alert-warning mb-0">Pick a location.</div>';
            return;
        }
        var includeIntake   = document.getElementById('rb-type-intake').checked;
        var includeTransfer = document.getElementById('rb-type-transfer').checked;
        if (!includeIntake && !includeTransfer) {
            document.getElementById('rb-results').innerHTML =
                '<div class="alert alert-warning mb-0">Pick at least one type.</div>';
            return;
        }
        var fromVal = document.getElementById('rb-from').value || todayIso();
        var toVal   = document.getElementById('rb-to').value   || todayIso();

        document.getElementById('rb-results').innerHTML =
            '<div class="text-muted small">Loading…</div>';

        fetch('/api/ReportBuilder/SummaryCandidates', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                From: fromVal, To: toVal,
                LocationIds: [locId],
                IncludeIntake: includeIntake,
                IncludeTransfer: includeTransfer
            })
        }).then(function (r) {
            if (!r.ok) throw new Error('HTTP ' + r.status);
            return r.json();
        }).then(renderResults)
          .catch(function (err) {
            document.getElementById('rb-results').innerHTML =
                '<div class="alert alert-danger mb-0">Failed to load: ' + escapeHtml(err.message || err) + '</div>';
          });
    }

    function openPdf(locId, date, type, locName, dateDisplay) {
        var path = (type === 'Intake' ? 'Pdf/Intake' : 'Pdf/Transfer');
        var url = '/api/ReportBuilder/' + path
            + '?locationId=' + encodeURIComponent(locId)
            + '&day=' + encodeURIComponent(date);
        document.getElementById('rb-pdf-title').textContent =
            'Daily ' + type + ' — ' + locName + ' — ' + dateDisplay;
        document.getElementById('rb-pdf-iframe').src = url;
        if (!pdfModal) pdfModal = new bootstrap.Modal(document.getElementById('rb-pdf-modal'));
        pdfModal.show();
    }

    document.addEventListener('DOMContentLoaded', function () {
        var t = todayIso();
        document.getElementById('rb-from').value = t;
        document.getElementById('rb-to').value   = t;

        // Default district = "All", then immediately load every
        // warehouse-enabled location so the page is usable on first paint.
        loadDistricts().then(function () { return loadLocations('0'); });

        document.getElementById('rb-district').addEventListener('change', function () {
            loadLocations(this.value);
        });
        document.getElementById('rb-apply').addEventListener('click', fetchCandidates);

        document.getElementById('rb-results').addEventListener('click', function (e) {
            var btn = e.target.closest('.rb-print');
            if (!btn) return;
            openPdf(
                btn.getAttribute('data-loc-id'),
                btn.getAttribute('data-date'),
                btn.getAttribute('data-type'),
                btn.getAttribute('data-loc-name'),
                btn.getAttribute('data-date-display')
            );
        });

        // Clear the iframe src on close so closing + reopening can land on
        // the same URL without the browser ignoring "no change" reloads.
        document.getElementById('rb-pdf-modal').addEventListener('hidden.bs.modal', function () {
            document.getElementById('rb-pdf-iframe').src = 'about:blank';
        });
    });
})();
