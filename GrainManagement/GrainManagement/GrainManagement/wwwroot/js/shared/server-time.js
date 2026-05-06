/*
 * server-time.js
 *
 * Helpers for rendering UTC-stored timestamps in the server-configured
 * timezone (window.gmServerTimeZone, set in _Layout.cshtml from the
 * "TimeZone" appsettings key).
 *
 * Rationale: we keep storage in UTC for correctness (DST gaps, multi-region
 * safety). Every display site goes through these helpers so the UI reads in
 * the operator-facing timezone rather than the browser's locale.
 *
 * Usage:
 *   gmFormatServerTime(input)              → "04/29/2026 03:15:42 PM"
 *   gmFormatServerTime(input, 'datetime')  → same
 *   gmFormatServerTime(input, 'date')      → "04/29/2026"
 *   gmFormatServerTime(input, 'time')      → "03:15:42 PM"
 *   gmFormatServerTime(input, { ...Intl.DateTimeFormat options })
 */
(function () {
    'use strict';

    var TZ = window.gmServerTimeZone || null;

    function toDate(input) {
        if (input == null || input === '') return null;
        if (input instanceof Date) return isNaN(input.getTime()) ? null : input;
        var d = new Date(input);
        return isNaN(d.getTime()) ? null : d;
    }

    var PRESETS = {
        datetime: { year: 'numeric', month: '2-digit', day: '2-digit',
                    hour: '2-digit', minute: '2-digit', second: '2-digit', hour12: true },
        datetimeShort: { year: 'numeric', month: '2-digit', day: '2-digit',
                         hour: 'numeric', minute: '2-digit', hour12: true },
        date:     { year: 'numeric', month: '2-digit', day: '2-digit' },
        time:     { hour: '2-digit', minute: '2-digit', second: '2-digit', hour12: true },
        timeShort:{ hour: 'numeric', minute: '2-digit', hour12: true },
    };

    function buildOptions(opts) {
        if (typeof opts === 'string') return PRESETS[opts] || PRESETS.datetime;
        return opts || PRESETS.datetime;
    }

    function formatIn(input, opts) {
        var d = toDate(input);
        if (!d) return '';
        var options = Object.assign({}, buildOptions(opts));
        if (TZ) options.timeZone = TZ;
        try {
            return new Intl.DateTimeFormat(undefined, options).format(d);
        } catch (_) {
            // Fallback: bad TZ id or environment without Intl. Use locale defaults.
            return d.toLocaleString();
        }
    }

    /** Format any UTC datetime (ISO string, Date, or epoch) in the server zone. */
    window.gmFormatServerTime = function (input, opts) {
        return formatIn(input, opts || 'datetime');
    };

    /** Convenience: date-only render in the server zone. */
    window.gmFormatServerDate = function (input) {
        return formatIn(input, 'date');
    };

    /** Convenience: time-only render in the server zone. */
    window.gmFormatServerTimeOnly = function (input) {
        return formatIn(input, 'time');
    };

    /**
     * DevExtreme grids that use `dataType: 'date'` / `'datetime'` format the
     * value with the browser locale by default. Wire this as a column's
     * `customizeText` to force server-zone rendering instead. Pair with one
     * of the preset names above (or a full options object).
     *
     *   { dataField:'StartedAt', dataType:'datetime',
     *     customizeText: gmDxServerTime('datetime') }
     */
    window.gmDxServerTime = function (preset) {
        return function (cellInfo) {
            var v = cellInfo && (cellInfo.value != null ? cellInfo.value : cellInfo.valueText);
            return v == null || v === '' ? '' : formatIn(v, preset || 'datetime');
        };
    };
})();
