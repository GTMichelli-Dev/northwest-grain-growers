(function () {
    "use strict";

    const statusBar = document.getElementById("serviceStatusBar");
    const container = document.getElementById("servicesContainer");
    const noServicesMsg = document.getElementById("noServicesMessage");
    const toastContainer = document.getElementById("toastContainer");

    const services = {};
    let connection = null;
    let locations = []; // [{ LocationId, Name }]
    let liveWeightScaleId = null; // currently open test scale

    // ── Locations ─────────────────────────────────────────────────────────

    async function loadLocations() {
        try {
            var resp = await fetch("/api/Scale/Locations", { cache: "no-store" });
            if (resp.ok) locations = await resp.json();
        } catch (err) {
            console.error("Failed to load locations", err);
        }
        populateLocationDropdown();
    }



    function populateLocationDropdown() {
        var sel = document.getElementById("fldLocation");
        sel.innerHTML = '<option value="">-- Select Location --</option>';
        locations.forEach(function (loc) {
            var opt = document.createElement("option");
            opt.value = loc.LocationId || loc.locationId;
            opt.textContent = loc.Name || loc.name;
            opt.dataset.name = loc.Name || loc.name;
            sel.appendChild(opt);
        });
    }

    function getLocationName(locationId) {
        if (!locationId) return "";
        var loc = locations.find(function (l) { return (l.LocationId || l.locationId) == locationId; });
        return loc ? (loc.Name || loc.name) : "";
    }

    // ── Toast ─────────────────────────────────────────────────────────────

    function showToast(message, type) {
        var cls = type === "error" ? "bg-danger text-white"
                : type === "success" ? "bg-success text-white"
                : "bg-info text-white";
        var html = '<div class="toast show ' + cls + '" role="alert">' +
            '<div class="toast-body d-flex justify-content-between align-items-center">' +
            '<span>' + message + '</span>' +
            '<button type="button" class="btn-close btn-close-white ms-2" data-bs-dismiss="toast"></button>' +
            '</div></div>';
        toastContainer.insertAdjacentHTML("beforeend", html);
        var el = toastContainer.lastElementChild;
        setTimeout(function () { if (el.parentNode) el.remove(); }, 4000);
    }

    // ── Render ─────────────────────────────────────────────────────────────

    function g(s, field) { return s[field] !== undefined ? s[field] : s[field.charAt(0).toLowerCase() + field.slice(1)] || ""; }

    function render() {
        var ids = Object.keys(services);
        if (ids.length === 0) {
            container.innerHTML = "";
            noServicesMsg.style.display = "";
            statusBar.className = "alert alert-warning py-2 mb-3 d-flex align-items-center";
            statusBar.innerHTML = '<i class="dx-icon dx-icon-warning me-2"></i> No scale services connected.';
            return;
        }

        noServicesMsg.style.display = "none";
        statusBar.className = "alert alert-success py-2 mb-3 d-flex align-items-center";
        statusBar.innerHTML = '<i class="dx-icon dx-icon-check me-2"></i> ' + ids.length + ' service(s) connected.';

        container.innerHTML = "";
        ids.forEach(function (sid) {
            var svc = services[sid];
            var card = document.createElement("div");
            card.className = "card service-card";

            var header = '<div class="card-header">' +
                '<div><strong>' + sid + '</strong></div>' +
                '<div>' +
                '<button class="btn btn-sm btn-outline-primary me-1" onclick="scaleConfig.addScale(\'' + sid + '\')"><i class="dx-icon dx-icon-add"></i> Add Scale</button>' +
                '<button class="btn btn-sm btn-outline-secondary" onclick="scaleConfig.refreshService(\'' + sid + '\')"><i class="dx-icon dx-icon-refresh"></i></button>' +
                '</div></div>';

            var body = '<div class="card-body p-0">';
            if (!svc.scales || svc.scales.length === 0) {
                body += '<div class="text-center text-muted py-3">No scales configured.</div>';
            } else {
                body += '<table class="table table-sm table-striped scale-table mb-0">' +
                    '<thead><tr>' +
                    '<th>ID</th><th>Description</th><th>Location</th><th>Brand</th><th>IP:Port</th><th>Enabled</th><th>Actions</th>' +
                    '</tr></thead><tbody>';
                svc.scales.forEach(function (s) {
                    var locDesc = g(s, "LocationDescription") || getLocationName(g(s, "LocationId"));
                    body += '<tr>' +
                        '<td>' + g(s, "Id") + '</td>' +
                        '<td>' + g(s, "Description") + '</td>' +
                        '<td>' + locDesc + '</td>' +
                        '<td>' + g(s, "Brand") + '</td>' +
                        '<td>' + g(s, "IpAddress") + ':' + g(s, "Port") + '</td>' +
                        '<td>' + (g(s, "Enabled") ? 'Yes' : 'No') + '</td>' +
                        '<td>' +
                        '<button class="btn btn-xs btn-outline-info me-1" onclick=\'scaleConfig.testScale(' + g(s, "Id") + ',"' + (g(s, "Description") || '').replace(/"/g, '') + '")\'><i class="dx-icon dx-icon-chart"></i> Test</button>' +
                        '<button class="btn btn-xs btn-outline-primary me-1" onclick=\'scaleConfig.editScale("' + sid + '",' + JSON.stringify(s).replace(/'/g, "\\'") + ')\'><i class="dx-icon dx-icon-edit"></i></button>' +
                        '<button class="btn btn-xs btn-outline-danger" onclick=\'scaleConfig.deleteScale("' + sid + '",' + g(s, "Id") + ',"' + (g(s, "Description") || '').replace(/"/g, '') + '")\'><i class="dx-icon dx-icon-trash"></i></button>' +
                        '</td></tr>';
                });
                body += '</tbody></table>';
            }
            body += '</div>';

            card.innerHTML = header + body;
            container.appendChild(card);
        });
    }

    // ── Modal ──────────────────────────────────────────────────────────────

    function openModal(serviceId, mode, scale) {
        document.getElementById("modalServiceId").value = serviceId;
        document.getElementById("modalMode").value = mode;
        document.getElementById("scaleModalTitle").textContent = mode === "add" ? "Add Scale" : "Edit Scale";

        if (scale) {
            document.getElementById("modalOriginalScaleId").value = g(scale, "Id");
            document.getElementById("fldDescription").value = g(scale, "Description");
            document.getElementById("fldLocation").value = g(scale, "LocationId") || "";
            document.getElementById("fldBrand").value = g(scale, "Brand") || "SMA";
            document.getElementById("fldIpAddress").value = g(scale, "IpAddress");
            document.getElementById("fldPort").value = g(scale, "Port") || 3001;
            document.getElementById("fldRequestCommand").value = g(scale, "RequestCommand");
            document.getElementById("fldEncoding").value = g(scale, "Encoding") || "ascii";
            document.getElementById("fldEnabled").checked = g(scale, "Enabled") !== false;
        } else {
            document.getElementById("modalOriginalScaleId").value = "";
            document.getElementById("fldDescription").value = "";
            document.getElementById("fldLocation").value = "";
            document.getElementById("fldBrand").value = "SMA";
            document.getElementById("fldIpAddress").value = "";
            document.getElementById("fldPort").value = 0;
            document.getElementById("fldRequestCommand").value = "";
            document.getElementById("fldEncoding").value = "ascii";
            document.getElementById("fldEnabled").checked = true;
        }

        var modal = new bootstrap.Modal(document.getElementById("scaleModal"));
        modal.show();
    }

    function getModalData() {
        var locSel = document.getElementById("fldLocation");
        var selectedOption = locSel.options[locSel.selectedIndex];
        return {
            Description: document.getElementById("fldDescription").value.trim(),
            LocationId: parseInt(locSel.value) || 0,
            LocationDescription: selectedOption && selectedOption.dataset.name ? selectedOption.dataset.name : "",
            Brand: document.getElementById("fldBrand").value,
            IpAddress: document.getElementById("fldIpAddress").value.trim(),
            Port: parseInt(document.getElementById("fldPort").value) || 3001,
            RequestCommand: document.getElementById("fldRequestCommand").value.trim() || "W\\r\\n",
            Encoding: document.getElementById("fldEncoding").value,
            Enabled: document.getElementById("fldEnabled").checked
        };
    }

    // ── Save handler ──────────────────────────────────────────────────────

    document.getElementById("btnSaveScale").addEventListener("click", function () {
        var serviceId = document.getElementById("modalServiceId").value;
        var mode = document.getElementById("modalMode").value;
        var data = getModalData();

        if (!data.Description) { showToast("Description is required.", "error"); return; }
        if (!data.IpAddress) { showToast("IP Address is required.", "error"); return; }
        if (!data.LocationId) { showToast("Location is required.", "error"); return; }

        if (mode === "add") {
            connection.invoke("AddScaleToService", serviceId, data)
                .catch(function (err) { showToast("Failed: " + err.toString(), "error"); });
        } else {
            var originalId = parseInt(document.getElementById("modalOriginalScaleId").value);
            connection.invoke("UpdateScaleOnService", serviceId, originalId, data)
                .catch(function (err) { showToast("Failed: " + err.toString(), "error"); });
        }

        bootstrap.Modal.getInstance(document.getElementById("scaleModal")).hide();
    });

    // ── Public API ────────────────────────────────────────────────────────

    // ── Live Weight Test ─────────────────────────────────────────────────

    var liveWeightLastReceived = 0;
    var liveWeightStaleTimer = null;

    function showLiveWeight(scaleId, displayName) {
        liveWeightScaleId = scaleId;
        liveWeightLastReceived = 0;
        document.getElementById("liveWeightScaleName").textContent = displayName || ("Scale " + scaleId);
        document.getElementById("liveWeightValue").textContent = "0";
        document.getElementById("liveWeightValue").style.color = "#888";
        document.getElementById("liveWeightUnit").style.color = "#888";
        document.getElementById("liveWeightStatus").innerHTML = '<span style="color:#888;">Waiting for data...</span>';
        document.getElementById("diagRaw").textContent = "--";
        document.getElementById("diagParsed").textContent = "--";
        document.getElementById("diagStatus").textContent = "--";
        document.getElementById("diagOk").textContent = "--";
        document.getElementById("diagMotion").textContent = "--";
        document.getElementById("diagLastUpdate").textContent = "--";

        var modal = new bootstrap.Modal(document.getElementById("liveWeightModal"));
        modal.show();

        // Start staleness checker
        if (liveWeightStaleTimer) clearInterval(liveWeightStaleTimer);
        liveWeightStaleTimer = setInterval(checkLiveWeightStaleness, 1000);

        document.getElementById("liveWeightModal").addEventListener("hidden.bs.modal", function () {
            liveWeightScaleId = null;
            if (liveWeightStaleTimer) { clearInterval(liveWeightStaleTimer); liveWeightStaleTimer = null; }
        }, { once: true });
    }

    function checkLiveWeightStaleness() {
        if (liveWeightScaleId === null) return;
        if (liveWeightLastReceived === 0) return; // haven't received anything yet
        if ((Date.now() - liveWeightLastReceived) > 5000) {
            document.getElementById("liveWeightValue").textContent = "0";
            document.getElementById("liveWeightValue").style.color = "#f00";
            document.getElementById("liveWeightUnit").style.color = "#a00";
            document.getElementById("liveWeightStatus").innerHTML = '<span style="color:#f00;">NOT CONNECTED</span>';
            document.getElementById("diagStatus").textContent = "Not Connected";
        }
    }

    function handleScaleUpdated(dto) {
        if (liveWeightScaleId === null) return;
        var id = dto.Id !== undefined ? dto.Id : dto.id;
        if (id !== liveWeightScaleId) return;

        liveWeightLastReceived = Date.now();

        var weightEl = document.getElementById("liveWeightValue");
        var unitEl = document.getElementById("liveWeightUnit");
        var statusEl = document.getElementById("liveWeightStatus");
        var ok = dto.Ok !== undefined ? dto.Ok : dto.ok;
        var motion = dto.Motion !== undefined ? dto.Motion : dto.motion;
        var weight = dto.Weight !== undefined ? dto.Weight : dto.weight;

        if (!ok) {
            weightEl.textContent = "0";
            weightEl.style.color = "#f00";
            unitEl.style.color = "#a00";
            statusEl.innerHTML = '<span style="color:#f00;">ERROR</span>';
        } else if (motion) {
            weightEl.textContent = Number(weight).toLocaleString();
            weightEl.style.color = "#ffc107";
            unitEl.style.color = "#cc9900";
            statusEl.innerHTML = '<span style="color:#ffc107;">MOTION</span>';
        } else {
            weightEl.textContent = Number(weight).toLocaleString();
            weightEl.style.color = "#0f0";
            unitEl.style.color = "#0a0";
            statusEl.innerHTML = '<span style="color:#0a0;">STABLE</span>';
        }

        // Diagnostic
        var raw = dto.RawResponse !== undefined ? dto.RawResponse : (dto.rawResponse || "");
        document.getElementById("diagRaw").textContent = raw || "--";
        document.getElementById("diagParsed").textContent = "weight=" + weight + " motion=" + motion + " ok=" + ok;
        document.getElementById("diagStatus").textContent = dto.Status || dto.status || "--";
        document.getElementById("diagOk").textContent = String(ok);
        document.getElementById("diagMotion").textContent = String(motion);
        var lu = dto.LastUpdate || dto.lastUpdate;
        document.getElementById("diagLastUpdate").textContent = lu ? new Date(lu).toLocaleTimeString() : "--";
    }

    // ── Public API ────────────────────────────────────────────────────────

    window.scaleConfig = {
        addScale: function (serviceId) { openModal(serviceId, "add", null); },
        editScale: function (serviceId, scale) { openModal(serviceId, "edit", scale); },
        deleteScale: function (serviceId, scaleId, desc) {
            if (!confirm("Delete scale '" + desc + "' (ID: " + scaleId + ")?")) return;
            connection.invoke("DeleteScaleFromService", serviceId, scaleId)
                .catch(function (err) { showToast("Failed: " + err.toString(), "error"); });
        },
        refreshService: function (serviceId) {
            connection.invoke("RequestServiceAnnounce", serviceId)
                .catch(function (err) { showToast("Refresh failed: " + err.toString(), "error"); });
        },
        testScale: function (scaleId, desc) {
            showLiveWeight(scaleId, desc);
        }
    };

    // ── SignalR ────────────────────────────────────────────────────────────

    async function startSignalR() {
        connection = new signalR.HubConnectionBuilder()
            .withUrl("/hubs/scale")
            .withAutomaticReconnect()
            .build();

        connection.on("ServiceConnected", function (serviceId, locationId, locationDescription) {
            if (!services[serviceId]) {
                services[serviceId] = { scales: [] };
            }
            render();
        });

        connection.on("ServiceDisconnected", function (serviceId) {
            delete services[serviceId];
            render();
        });

        connection.on("ScalesAnnounced", function (serviceId, locationId, locationDescription, scales) {
            services[serviceId] = { scales: scales || [] };
            render();
        });

        connection.on("ScaleUpdated", function (dto) {
            handleScaleUpdated(dto);
        });

        connection.on("CrudResult", function (result) {
            if (result && result.success) {
                showToast(result.message || "Operation succeeded.", "success");
            } else {
                showToast(result.message || "Operation failed.", "error");
            }
        });

        connection.onreconnecting(function () {
            statusBar.className = "alert alert-warning py-2 mb-3";
            statusBar.textContent = "Reconnecting...";
        });

        connection.onreconnected(function () {
            statusBar.className = "alert alert-success py-2 mb-3";
            statusBar.textContent = "Reconnected. Checking services...";
            pollForServices();
        });

        connection.onclose(function () {
            statusBar.className = "alert alert-danger py-2 mb-3";
            statusBar.textContent = "Disconnected. Retrying in 5s...";
            // Auto-reconnect from scratch after server restart
            setTimeout(function () { restartConnection(); }, 5000);
        });

        await connection.start();
        await pollForServices();
    }

    async function restartConnection() {
        statusBar.className = "alert alert-warning py-2 mb-3";
        statusBar.textContent = "Reconnecting...";
        // Clear stale service state
        Object.keys(services).forEach(function (k) { delete services[k]; });
        render();
        try {
            await connection.start();
            statusBar.className = "alert alert-success py-2 mb-3";
            statusBar.textContent = "Connected. Checking services...";
            await pollForServices();
        } catch (err) {
            console.warn("Reconnect failed, retrying in 5s...", err);
            statusBar.className = "alert alert-danger py-2 mb-3";
            statusBar.textContent = "Server unavailable. Retrying in 5s...";
            setTimeout(function () { restartConnection(); }, 5000);
        }
    }

    async function pollForServices() {
        try {
            var connectedIds = await connection.invoke("GetConnectedServices");
            if (connectedIds.length === 0) {
                render(); // shows "no services" message
                setTimeout(pollForServices, 5000);
            } else {
                statusBar.className = "alert alert-info py-2 mb-3 d-flex align-items-center";
                statusBar.innerHTML = '<i class="dx-icon dx-icon-refresh me-2"></i> ' + connectedIds.length + ' service(s) found. Loading scales...';
                for (var i = 0; i < connectedIds.length; i++) {
                    await connection.invoke("RequestServiceAnnounce", connectedIds[i]);
                }
            }
        } catch (err) {
            console.warn("pollForServices failed, retrying...", err);
            setTimeout(pollForServices, 5000);
        }
    }

    // ── Boot ──────────────────────────────────────────────────────────────

    (async function () {
        await loadLocations();
        await startSignalR();
    })().catch(function (err) {
        console.error(err);
        statusBar.className = "alert alert-danger py-2 mb-3";
        statusBar.textContent = "Failed to connect to SignalR hub.";
    });

})();
