(function () {
    "use strict";

    const connStatus = document.getElementById("connStatus");
    const body = document.getElementById("scalesBody");

    // Track rows by scale Id
    const rowById = new Map();

    function setConnStatus(text) {
        if (connStatus) connStatus.textContent = text;
    }

    function fmtBool(v) {
        return v ? "Yes" : "No";
    }

    function upsertRow(dto) {
        if (!dto || !dto.Id) return;

        let tr = rowById.get(dto.Id);
        if (!tr) {
            tr = document.createElement("tr");
            tr.dataset.id = dto.Id;
            tr.innerHTML = `
                <td class="c-id"></td>
                <td class="c-desc"></td>
                <td class="c-weight"></td>
                <td class="c-ok"></td>
                <td class="c-motion"></td>
                <td class="c-status"></td>
                <td class="c-last"></td>
            `;
            rowById.set(dto.Id, tr);
            body.appendChild(tr);
        }

        tr.querySelector(".c-id").textContent = dto.Id;
        tr.querySelector(".c-desc").textContent = dto.Description ?? "";
        tr.querySelector(".c-weight").textContent = (dto.Weight ?? 0).toString();
        tr.querySelector(".c-ok").textContent = fmtBool(dto.Ok);
        tr.querySelector(".c-motion").textContent = fmtBool(dto.Motion);
        tr.querySelector(".c-status").textContent = dto.Status ?? "";
        tr.querySelector(".c-last").textContent = dto.LastUpdate ?? "";


        // Highlight row pink when scale is NOT OK
        tr.classList.toggle("scale-error", dto.Ok === false);
    }

    function sortRows() {
        const rows = Array.from(rowById.values());
        rows.sort((a, b) => Number(a.dataset.id) - Number(b.dataset.id));
        body.innerHTML = "";
        for (const r of rows) body.appendChild(r);
    }

    async function loadInitial() {
        body.innerHTML = "";
        rowById.clear();

        try {
            const resp = await fetch("/api/Scale/CachedScales", { cache: "no-store" });
            if (!resp.ok) {
                body.innerHTML = `<tr><td colspan="7">Failed to load scales.</td></tr>`;
                return;
            }

            const list = await resp.json();
            (list || []).forEach(upsertRow);
            sortRows();

            if (!list || list.length === 0) {
                body.innerHTML = `<tr><td colspan="7">No scales found.</td></tr>`;
            }
        } catch (err) {
            console.error(err);
            body.innerHTML = `<tr><td colspan="7">Error loading scales.</td></tr>`;
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

        // Receive updates for ALL scales
        connection.on("ScaleUpdated", (dto) => {
            // DTO fields are PascalCase (matches your JSON config)
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

        // Optional safety refresh in case a SignalR message is missed
        setInterval(loadInitial, 30000);
    })().catch(err => {
        console.error(err);
        setConnStatus("Startup error.");
    });

})();
