(function () {
    "use strict";

    var connectedServices = {};
    var savedInboundPrinterId = "";
    var savedOutboundPrinterId = "";

    function escHtml(s) { var d = document.createElement("div"); d.textContent = s; return d.innerHTML; }
    function escAttr(s) { return s.replace(/'/g, "\\'").replace(/"/g, "&quot;"); }

    // ===== Load saved assignments =====
    function loadAssignments() {
        fetch("/api/printers/assignments")
            .then(function (r) { return r.ok ? r.json() : {}; })
            .then(function (data) {
                if (data.Inbound) {
                    savedInboundPrinterId = data.Inbound.ServiceId + ":" + data.Inbound.PrinterId;
                }
                if (data.Outbound) {
                    savedOutboundPrinterId = data.Outbound.ServiceId + ":" + data.Outbound.PrinterId;
                }
                updateAssignmentDropdowns();
            })
            .catch(function () { });
    }

    // ===== SignalR =====
    var connection = new signalR.HubConnectionBuilder()
        .withUrl("/hubs/print")
        .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
        .build();

    connection.onreconnecting(function () {
        updateStatusBar();
    });

    connection.onreconnected(function () {
        connectedServices = {};
        connection.invoke("RequestPrinterList").catch(function () { });
    });

    connection.onclose(function () {
        connectedServices = {};
        updateStatusBar();
        renderServices();
        setTimeout(startConnection, 5000);
    });

    function startConnection() {
        connection.start().then(function () {
            connection.invoke("RequestPrinterList").catch(function () { });
        }).catch(function () {
            setTimeout(startConnection, 5000);
        });
    }

    connection.on("PrintServiceReady", function (data) {
        connectedServices[data.serviceId] = data;
        renderServices();
        updateStatusBar();
    });

    connection.on("PrinterListReceived", function (data) {
        connectedServices[data.serviceId] = data;
        renderServices();
        updateStatusBar();
    });

    connection.on("PrintServiceStatusChanged", function (serviceIds) {
        Object.keys(connectedServices).forEach(function (sid) {
            if (serviceIds.indexOf(sid) < 0) delete connectedServices[sid];
        });
        updateStatusBar();
        renderServices();
    });

    connection.on("TestPrintResult", function (data) {
        var spinner = document.getElementById("testPrintSpinner");
        var result = document.getElementById("testPrintResult");
        if (spinner) spinner.style.display = "none";
        if (result) {
            result.style.display = "block";
            if (data.success) {
                result.innerHTML = '<div class="alert alert-success mb-0"><strong>Success!</strong> ' + escHtml(data.message) + "</div>";
            } else {
                result.innerHTML = '<div class="alert alert-danger mb-0"><strong>Failed:</strong> ' + escHtml(data.message) + "</div>";
            }
        }
    });

    connection.on("PrintResult", function (data) {
        var type = data.success ? "success" : "danger";
        var msg = data.success ? "Print job sent." : ("Print failed: " + (data.message || "unknown error"));
        showToast(type, msg);
    });

    loadAssignments();
    startConnection();

    // ===== Status Bar =====
    function updateStatusBar() {
        var bar = document.getElementById("serviceStatusBar");
        var noMsg = document.getElementById("noServicesMessage");
        var count = Object.keys(connectedServices).length;
        if (count > 0) {
            bar.className = "alert alert-success py-2 mb-3 d-flex align-items-center";
            bar.innerHTML = "<strong>" + count + " Print Service(s) Connected</strong>";
            if (noMsg) noMsg.style.display = "none";
        } else {
            bar.className = "alert alert-warning py-2 mb-3 d-flex align-items-center";
            bar.innerHTML = "<strong>No Print Services Connected</strong> &mdash; Start a Web Print Service and point it to this server's SignalR hub.";
            if (noMsg) noMsg.style.display = "block";
        }
    }

    // ===== Render Services =====
    function renderServices() {
        var container = document.getElementById("servicesContainer");
        var html = "";

        Object.keys(connectedServices).forEach(function (serviceId) {
            var svc = connectedServices[serviceId];
            var printers = svc.printers || [];

            html += '<div class="card mb-3">';
            html += '<div class="card-header py-2 d-flex justify-content-between align-items-center" style="background:#f0fdf4;">';
            html += '<span><span class="badge bg-success me-2">Connected</span> <strong>' + escHtml(serviceId) + "</strong> (" + printers.length + " printer" + (printers.length !== 1 ? "s" : "") + ")</span>";
            html += "</div>";

            html += '<div class="card-body p-0"><table class="table table-sm table-hover mb-0 printer-table">';
            html += "<thead><tr><th>Printer ID</th><th>Display Name</th><th>Status</th><th>Enabled</th><th>Default</th><th style=\"width:100px;\">Actions</th></tr></thead><tbody>";

            printers.forEach(function (p) {
                html += "<tr>";
                html += "<td>" + escHtml(p.printerId) + "</td>";
                html += "<td>" + escHtml(p.displayName || p.printerId) + "</td>";
                html += "<td>";
                if (p.status === "idle") html += '<span class="text-success">Idle</span>';
                else if (p.status === "printing") html += '<span class="text-primary">Printing</span>';
                else if (p.status === "disabled") html += '<span class="text-danger">Disabled</span>';
                else html += '<span class="text-muted">' + escHtml(p.status) + "</span>";
                html += "</td>";
                html += "<td>" + (p.enabled ? '<span class="text-success fw-bold">&#10003;</span>' : '<span class="text-muted">&#10007;</span>') + "</td>";
                html += "<td>" + (p.isDefault ? '<span class="badge bg-primary">Default</span>' : "") + "</td>";
                html += '<td><button class="btn btn-outline-info btn-sm" data-service="' + escAttr(serviceId) + '" data-printer="' + escAttr(p.printerId) + '">Test</button></td>';
                html += "</tr>";
            });

            if (printers.length === 0) {
                html += '<tr><td colspan="6" class="text-center text-muted py-3">No printers found on this service.</td></tr>';
            }

            html += "</tbody></table></div></div>";
        });

        container.innerHTML = html;
        updateAssignmentDropdowns();
    }

    // ===== Assignment Dropdowns =====
    function updateAssignmentDropdowns() {
        var inSel = document.getElementById("inboundPrinterSelect");
        var outSel = document.getElementById("outboundPrinterSelect");
        var inVal = inSel.value || savedInboundPrinterId;
        var outVal = outSel.value || savedOutboundPrinterId;

        var options = '<option value="">-- None --</option>';
        Object.keys(connectedServices).forEach(function (sid) {
            var printers = connectedServices[sid].printers || [];
            printers.forEach(function (p) {
                if (!p.enabled) return;
                var val = sid + ":" + p.printerId;
                var label = p.displayName || p.printerId;
                if (Object.keys(connectedServices).length > 1) label += " (" + sid + ")";
                options += '<option value="' + escAttr(val) + '">' + escHtml(label) + "</option>";
            });
        });

        inSel.innerHTML = options;
        outSel.innerHTML = options;
        inSel.value = inVal;
        outSel.value = outVal;
    }

    function saveAssignments() {
        var inVal = document.getElementById("inboundPrinterSelect").value;
        var outVal = document.getElementById("outboundPrinterSelect").value;
        var status = document.getElementById("assignmentStatus");
        status.innerHTML = '<i class="dx-icon dx-icon-refresh dx-icon-spin"></i>';

        fetch("/api/printers/assignments", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                inboundPrinterId: inVal,
                outboundPrinterId: outVal
            })
        }).then(function (r) {
            if (r.ok) {
                savedInboundPrinterId = inVal;
                savedOutboundPrinterId = outVal;
                status.innerHTML = '<span class="text-success">&#10003; Saved</span>';
                setTimeout(function () { status.innerHTML = ""; }, 2000);
            } else {
                status.innerHTML = '<span class="text-danger">Failed</span>';
            }
        }).catch(function () {
            status.innerHTML = '<span class="text-danger">Failed</span>';
        });
    }

    // ===== Test Print =====
    function testPrint(serviceId, printerId) {
        var printerLabel = document.getElementById("testPrintPrinter");
        var spinner = document.getElementById("testPrintSpinner");
        var result = document.getElementById("testPrintResult");

        if (printerLabel) printerLabel.textContent = printerId;
        if (spinner) spinner.style.display = "block";
        if (result) result.style.display = "none";

        new bootstrap.Modal("#testPrintModal").show();

        connection.invoke("TestPrint", serviceId, printerId).catch(function (err) {
            if (spinner) spinner.style.display = "none";
            if (result) {
                result.style.display = "block";
                result.innerHTML = '<div class="alert alert-danger mb-0">Failed to send test: ' + escHtml(err.toString()) + "</div>";
            }
        });
    }

    function showToast(type, msg) {
        var container = document.getElementById("toastContainer");
        if (!container) return;
        container.innerHTML = '<div class="alert alert-' + type + ' alert-dismissible fade show py-2" role="alert">' +
            escHtml(msg) + '<button type="button" class="btn-close btn-close-sm" data-bs-dismiss="alert"></button></div>';
        setTimeout(function () { container.innerHTML = ""; }, 5000);
    }

    // ===== Event Delegation =====
    document.addEventListener("click", function (e) {
        var testBtn = e.target.closest("[data-service][data-printer]");
        if (testBtn) {
            testPrint(testBtn.dataset.service, testBtn.dataset.printer);
            return;
        }
    });

    document.addEventListener("change", function (e) {
        if (e.target.id === "inboundPrinterSelect" || e.target.id === "outboundPrinterSelect") {
            saveAssignments();
        }
    });

})();
