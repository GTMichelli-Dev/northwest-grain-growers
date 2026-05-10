// /Remote landing page. Lists active locations + their mapped server
// (LocationSequenceMappings → Server), filtered by district, with a
// Connect button per row that opens the server URL in a new tab. The
// district choice is persisted in a long-lived cookie so the operator
// returns to their preferred slice every visit.
(function () {
    'use strict';

    var FILTER_COOKIE = 'gm.remote.districtId';
    // ~50 years — practical "never expires". Browsers cap cookie
    // lifetime at ~400 days now, so this gets clamped down on read but
    // we still get a long persistence window with one Set.
    var FILTER_COOKIE_DAYS = 50 * 365;

    function readCookie(name) {
        var prefix = name + '=';
        var parts = (document.cookie || '').split(';');
        for (var i = 0; i < parts.length; i++) {
            var c = parts[i].replace(/^\s+/, '');
            if (c.indexOf(prefix) === 0) return decodeURIComponent(c.substring(prefix.length));
        }
        return null;
    }
    function writeCookie(name, value) {
        var d = new Date();
        d.setDate(d.getDate() + FILTER_COOKIE_DAYS);
        document.cookie = name + '=' + encodeURIComponent(value)
            + '; expires=' + d.toUTCString() + '; path=/; SameSite=Lax';
    }

    function escapeHtml(s) {
        return String(s == null ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }

    function loadDistricts() {
        return fetch('/api/Remote/Districts')
            .then(function (r) { return r.ok ? r.json() : []; })
            .then(function (list) {
                var sel = document.getElementById('rb-district');
                sel.innerHTML = '<option value="0">All Locations</option>';
                (list || []).forEach(function (d) {
                    var o = document.createElement('option');
                    o.value = d.DistrictId; o.textContent = d.Name;
                    sel.appendChild(o);
                });
                // Apply persisted choice if it's still in the list.
                var saved = readCookie(FILTER_COOKIE);
                if (saved !== null) {
                    var match = false;
                    for (var i = 0; i < sel.options.length; i++) {
                        if (sel.options[i].value === saved) { match = true; break; }
                    }
                    sel.value = match ? saved : '0';
                } else {
                    sel.value = '0';
                }
            });
    }

    function renderRows(rows) {
        var container = document.getElementById('rb-list');
        if (!rows || !rows.length) {
            container.innerHTML = '<div class="alert alert-info mb-0">No remote-accessible locations in this filter.</div>';
            return;
        }
        var bodyRows = rows.map(function (r) {
            var serverLabel = r.ServerFriendlyName && r.ServerFriendlyName.length
                ? (r.ServerFriendlyName + ' (' + (r.ServerName || '') + ')')
                : (r.ServerName || '');
            var url = r.Url || '';
            var canConnect = url && url.length > 0;
            var connectBtn = canConnect
                ? '<a class="btn btn-sm btn-primary" href="' + escapeHtml(url) + '" target="_blank" rel="noopener">' +
                    '<i class="fa fa-arrow-up-right-from-square"></i> Connect</a>'
                : '<button type="button" class="btn btn-sm btn-outline-secondary" disabled>No URL</button>';
            return '<tr>' +
                '<td>' + escapeHtml(r.LocationName) +
                  ' <span class="text-muted small">(' + escapeHtml(r.LocationId) + ')</span></td>' +
                '<td>' + escapeHtml(r.DistrictName) + '</td>' +
                '<td>' + escapeHtml(serverLabel) + '</td>' +
                '<td><span class="text-muted small">' + escapeHtml(url) + '</span></td>' +
                '<td class="text-end">' + connectBtn + '</td>' +
            '</tr>';
        }).join('');

        container.innerHTML =
            '<table class="table table-sm table-hover align-middle">' +
              '<thead class="table-light">' +
                '<tr><th>Location</th><th>District</th><th>Server</th><th>URL</th><th class="text-end">Connect</th></tr>' +
              '</thead>' +
              '<tbody>' + bodyRows + '</tbody>' +
            '</table>';
    }

    function fetchLocations() {
        var sel = document.getElementById('rb-district');
        var id = parseInt(sel.value, 10) || 0;
        document.getElementById('rb-list').innerHTML =
            '<div class="text-muted small">Loading…</div>';
        fetch('/api/Remote/Locations?districtId=' + id)
            .then(function (r) { return r.ok ? r.json() : []; })
            .then(renderRows)
            .catch(function () {
                document.getElementById('rb-list').innerHTML =
                    '<div class="alert alert-danger mb-0">Failed to load remote locations.</div>';
            });
    }

    document.addEventListener('DOMContentLoaded', function () {
        loadDistricts().then(fetchLocations);

        document.getElementById('rb-district').addEventListener('change', function () {
            writeCookie(FILTER_COOKIE, this.value || '0');
            fetchLocations();
        });
    });
})();
