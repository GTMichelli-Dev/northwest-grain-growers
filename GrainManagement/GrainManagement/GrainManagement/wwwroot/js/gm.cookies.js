/**
 * gm.cookies.js — Global cookie helpers for GrainManagement.
 * Include once in _Layout before any page-specific scripts.
 */
var GM = window.GM || {};

(function (ns) {
    "use strict";

    // ── Generic cookie helpers ──────────────────────────────────────────────

    ns.setCookie = function (name, value, days) {
        var d = new Date();
        d.setTime(d.getTime() + (days || 365) * 86400000);
        document.cookie = name + "=" + encodeURIComponent(value) +
            "; expires=" + d.toUTCString() + "; path=/; SameSite=Lax";
    };

    ns.getCookie = function (name) {
        var escaped = name.replace(/[.*+?^${}()|[\]\\]/g, "\\$&");
        var m = document.cookie.match(new RegExp("(^|;\\s*)" + escaped + "=([^;]+)"));
        return m ? decodeURIComponent(m[2]) : null;
    };

    ns.clearCookie = function (name) {
        document.cookie = name + "=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/; SameSite=Lax";
    };

    // ── Location shorthand ──────────────────────────────────────────────────

    var LOC_KEY = "GM.SelectedWarehouseLocationId";
    var SERVER_LOC_KEY = "GrainMgmt_LocationId"; // server-side LocationContext cookie

    ns.getLocationId = function () {
        var v = ns.getCookie(LOC_KEY);
        if (v) return parseInt(v, 10) || 0;

        // Fallback: sync from server-side location cookie
        var sv = ns.getCookie(SERVER_LOC_KEY);
        if (sv) {
            var id = parseInt(sv, 10) || 0;
            if (id) ns.setCookie(LOC_KEY, id, 365); // persist so future reads are instant
            return id;
        }
        return 0;
    };

    ns.setLocationId = function (val) {
        if (val) {
            ns.setCookie(LOC_KEY, val, 365);
        } else {
            ns.clearCookie(LOC_KEY);
        }
    };

})(GM);
