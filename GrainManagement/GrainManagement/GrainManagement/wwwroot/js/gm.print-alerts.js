/**
 * Global print alert listener.
 * Connects to the PrintHub via SignalR and shows a navbar alert bar
 * when a print job fails. Loaded on every page via _Layout.cshtml.
 */
(function () {
    "use strict";

    var alertEl = document.getElementById("gm-print-alert");
    var msgEl = document.getElementById("gm-print-alert-msg");
    if (!alertEl || !msgEl) return;

    // Don't initialize if SignalR isn't loaded
    if (typeof signalR === "undefined") return;

    var hideTimer = null;

    function show(type, msg) {
        if (hideTimer) { clearTimeout(hideTimer); hideTimer = null; }

        alertEl.className = "alert alert-" + type + " alert-dismissible mb-0 rounded-0 py-2 text-center";
        msgEl.textContent = msg;
        alertEl.style.display = "block";

        // Auto-hide success after 4 seconds; errors stay until dismissed
        if (type === "success") {
            hideTimer = setTimeout(function () { alertEl.style.display = "none"; }, 4000);
        }
    }

    function hide() {
        alertEl.style.display = "none";
    }

    // Dismiss button
    alertEl.addEventListener("click", function (e) {
        if (e.target.classList.contains("btn-close")) {
            hide();
        }
    });

    // SignalR connection
    var connection = new signalR.HubConnectionBuilder()
        .withUrl("/hubs/print")
        .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
        .build();

    connection.on("PrintResult", function (data) {
        if (!data) return;
        if (data.success === false) {
            show("danger", "Print failed: " + (data.message || "Unknown error"));
        } else if (data.success === true) {
            show("success", "Print job sent successfully.");
        }
    });

    connection.on("TestPrintResult", function (data) {
        if (!data) return;
        if (data.success === false) {
            show("danger", "Test print failed: " + (data.message || "Unknown error"));
        }
    });

    function start() {
        connection.start().catch(function () {
            setTimeout(start, 5000);
        });
    }

    connection.onclose(function () {
        setTimeout(start, 5000);
    });

    start();
})();
