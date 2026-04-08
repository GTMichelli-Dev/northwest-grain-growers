(function () {
    "use strict";

    const connStatus = document.getElementById("connStatus");
    const body = document.getElementById("scalesBody");
    const STALE_MS = 5000; // 5 seconds

    // Track rows by scale Id
    const rowById = new Map();
    // Track last update time per scale
    const lastUpdateById = new Map();

    function setConnStatus(text) {
        if (connStatus) connStatus.textContent = text;
    }

    function fmtBool(v) {
        return v ? "Yes" : "No";
    }

    function fmtDate(val) {
        if (!val) return "";
        var d = new Date(val);
        if (isNaN(d.getTime())) return val;
        var hh = String(d.getHours()).padStart(2, "0");
        var mm = String(d.getMinutes()).padStart(2, "0");
        var ss = String(d.getSeconds()).padStart(2, "0");
        var MM = String(d.getMonth() + 1).padStart(2, "0");
        var DD = String(d.getDate()).padStart(2, "0");
        var YY = String(d.getFullYear()).slice(-2);
        return hh + ":" + mm + ":" + ss + " " + MM + "-" + DD + "-" + YY;
    }

    function upsertRow(dto) {
        if (!dto || !dto.Id) return;

        // Record when we received this update
        lastUpdateById.set(dto.Id, Date.now());

        renderRow(dto.Id, dto);
    }

    function renderRow(id, dto) {
        let tr = rowById.get(id);
        if (!tr) {
            tr = document.createElement("tr");
            tr.dataset.id = id;
            tr.innerHTML = `
                <td class="c-id"></td>
                <td class="c-location"></td>
                <td class="c-desc"></td>
                <td class="c-weight"></td>
                <td class="c-ok"></td>
                <td class="c-motion"></td>
                <td class="c-status"></td>
                <td class="c-last"></td>
            `;
            rowById.set(id, tr);
            body.appendChild(tr);
        }

        // Store the latest dto on the row for staleness checks
        tr._dto = dto;

        var isStale = (Date.now() - (lastUpdateById.get(id) || 0)) > STALE_MS;
        var ok = isStale ? false : dto.Ok;
        var motion = isStale ? false : dto.Motion;
        var statusText = isStale ? "Not Connected" : (ok === false ? "Error" : (motion ? "Motion" : "Ok"));
        var weightText = (ok === false || isStale) ? "0" : (dto.Weight ?? 0).toString();

        tr.querySelector(".c-id").textContent = id;
        tr.querySelector(".c-location").textContent = dto.LocationDescription ?? "";
        tr.querySelector(".c-desc").textContent = dto.Description ?? "";
        tr.querySelector(".c-weight").textContent = weightText;
        tr.querySelector(".c-ok").textContent = isStale ? "No" : fmtBool(dto.Ok);
        tr.querySelector(".c-motion").textContent = isStale ? "No" : fmtBool(dto.Motion);
        tr.querySelector(".c-status").textContent = statusText;
        tr.querySelector(".c-last").textContent = fmtDate(dto.LastUpdate);

        // Row highlighting: pink=error/stale, yellow=motion, light green=ok
        tr.classList.toggle("scale-error", ok === false || isStale);
        tr.classList.toggle("scale-motion", ok === true && motion === true);
        tr.classList.toggle("scale-ok", ok === true && motion !== true);
    }

    function sortRows() {
        const rows = Array.from(rowById.values());
        rows.sort((a, b) => Number(a.dataset.id) - Number(b.dataset.id));
        body.innerHTML = "";
        for (const r of rows) body.appendChild(r);
    }

    // Check for stale scales every second
    function checkStaleness() {
        rowById.forEach(function (tr, id) {
            if (tr._dto) renderRow(id, tr._dto);
        });
    }

    async function loadInitial() {
        body.innerHTML = "";
        rowById.clear();
        lastUpdateById.clear();

        try {
            const resp = await fetch("/api/Scale/CachedScales", { cache: "no-store" });
            if (!resp.ok) {
                body.innerHTML = `<tr><td colspan="8">Failed to load scales.</td></tr>`;
                return;
            }

            const list = await resp.json();
            (list || []).forEach(upsertRow);
            sortRows();

            if (!list || list.length === 0) {
                body.innerHTML = `<tr><td colspan="8">No scales found.</td></tr>`;
            }
        } catch (err) {
            console.error(err);
            body.innerHTML = `<tr><td colspan="8">Error loading scales.</td></tr>`;
        }
    }

    async function startSignalR() {
        if (typeof signalR === "undefined") {
            setConnStatus("SignalR not loaded.");
            return;
        }

        const connection = new signalR.HubConnectionBuilder()
            .withUrl("/hubs/scale")
            .withAutomaticReconnect()
            .build();

        connection.on("ScaleUpdated", (dto) => {
            upsertRow(dto);
            sortRows();
        });

        connection.onreconnecting(() => setConnStatus("Reconnecting…"));
        connection.onreconnected(() => setConnStatus("Connected"));
        connection.onclose(() => setConnStatus("Disconnected"));

        setConnStatus("Connecting…");
        await connection.start();
        setConnStatus("Connected");
    }

    // Boot
    (async () => {
        await loadInitial();
        await startSignalR();

        // Check staleness every second
        setInterval(checkStaleness, 1000);

        // Safety refresh every 30 seconds
        setInterval(loadInitial, 30000);
    })().catch(err => {
        console.error(err);
        setConnStatus("Startup error.");
    });

})();
