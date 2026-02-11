(function () {
    "use strict";

    const el = (id) => document.getElementById(id);

    // UI elements (must exist in your Index.cshtml)
    const weightEl = el("weight");
    const statusEl = el("status");
    const promptEl = el("prompt");           // not used for scale status
    const scaleNameEl = el("scaleName");
    const printerNameEl = el("printerName"); // optional

    const scaleId = Number(window.GM?.scaleId || 0);
    function getParamInsensitive(name) {
        const params = new URLSearchParams(location.search);
        for (const [k, v] of params.entries()) {
            if (k.toLowerCase() === name.toLowerCase()) return v;
        }
        return null;
    }

    const printerId = Number(
        window.GM?.printerId ??
        window.GM?.PrinterId ??
        getParamInsensitive("printerId") ??
        0
    );


    // SignalR health watchdog
    let signalRConnected = false;
    let lastMessageUtc = null;
    const SIGNALR_TIMEOUT_MS = 5000; // if no ScaleUpdated in 5s => red
    const WATCHDOG_TICK_MS = 1000;

    // Remember last scale state so watchdog doesn't "reset" colors
    let lastOk = null;
    let lastMotion = null;

    let printingTimer = null;

    function showPrinting(ticket) {
        const container = document.querySelector(".main-container");
        if (!container) return;

        // Set prompt + overlay class
        setText(promptEl, `Printing ticket ${ticket}...`);
        container.classList.add("printing");

        // Clear old timer and revert after 3 seconds
        if (printingTimer) clearTimeout(printingTimer);
        printingTimer = setTimeout(() => {
            container.classList.remove("printing");
            setText(promptEl, "");
            printingTimer = null;

            // Re-apply your normal visuals right after printing ends
            applyVisualState();
        }, 3000);
    }

    function setText(node, value) {
        if (!node) return;
        node.textContent = value ?? "";
    }

    function setContainerState({ signalrFail, ok, motion }) {
        const container = document.querySelector(".main-container");
        if (!container) return;

        // Overlay: SignalR fail => red/white (highest priority)
        container.classList.toggle("signalr-error", !!signalrFail);

        // IMPORTANT:
        // Do NOT clear scale state when SignalR fails.
        // signalr-error visually overrides, but we keep the last known scale classes
        // so we don't flicker back to white/black.
        if (signalrFail) return;

        // Clear scale states and re-apply based on ok/motion
        container.classList.remove("scale-error", "scale-motion");

        // Rule 1: Scale NOT OK => red/white
        if (ok === false) {
            container.classList.add("scale-error");
            return;
        }

        // Rule 2: OK + Motion => yellow/black
        if (motion === true) {
            container.classList.add("scale-motion");
            return;
        }

        // Rule 3: OK + no motion => default white/black
    }

    function applyVisualState() {
        // Always apply based on last-known scale state + current signalr health
        setContainerState({
            signalrFail: !signalRConnected || isStalled(),
            ok: lastOk,
            motion: lastMotion
        });
    }

    function isStalled() {
        if (!lastMessageUtc) return true; // no data yet => treat as fail visually
        return (Date.now() - lastMessageUtc) > SIGNALR_TIMEOUT_MS;
    }

    function render(dto) {
        if (!dto) return;

        // Remember last-known scale state FIRST
        lastOk = dto.Ok;
        lastMotion = dto.Motion;

        // Weight
        setText(weightEl, (dto.Weight ?? 0).toString());

        // Status/errors text
        let status = dto.Status ?? "";
        if (dto.Ok === false && !status) status = "ERROR";
        setText(statusEl, status);

        // promptEl should NOT be used for status
        setText(promptEl, "");

        // Header info
        if (dto.Description) setText(scaleNameEl, dto.Description);

        // Optional printer name
        if (dto.PrinterName) setText(printerNameEl, dto.PrinterName);

        // Apply visuals (uses lastOk/lastMotion)
        applyVisualState();
    }

    async function loadInitial() {
        if (!scaleId || scaleId < 1) return;

        try {
            const resp = await fetch("/api/Scale/CachedScales", { cache: "no-store" });
            if (!resp.ok) return;

            const list = await resp.json();
            const dto = (list || []).find(s => Number(s.Id) === scaleId);
            if (dto) {
                // Consider initial data as "fresh" so we don't immediately show SignalR red
                // (comment out next 2 lines if you WANT red until SignalR connects)
                lastMessageUtc = Date.now();
                // Note: signalRConnected still false until SignalR starts
                render(dto);
            }
        } catch {
            // ignore
        }
    }

    function startWatchdog() {
        setInterval(() => {
            // Only update the overlay/state — do not wipe scale classes
            const stalled = isStalled();

            // If stalled, we show red overlay; we keep lastOk/lastMotion underneath
            if (stalled) {
                // Optional text message (comment out if you don't want it)
                // setText(statusEl, "No data");
            }

            applyVisualState();
        }, WATCHDOG_TICK_MS);
    }

    async function startSignalR() {
        if (paramsNotSet()) return;

        if (typeof signalR === "undefined") {
            setText(statusEl, "SignalR script not loaded.");
            setText(promptEl, "");
            signalRConnected = false;
            applyVisualState();
            return;
        }

        // Initial UI defaults
        setText(weightEl, "--");
        setText(statusEl, "Connecting...");
        setText(promptEl, "");
        signalRConnected = false;
        applyVisualState();

        // Show cached value immediately (optional but recommended)
        await loadInitial();

        const connection = new signalR.HubConnectionBuilder()
            .withUrl("/hubs/scale")
            .withAutomaticReconnect()
            .build();

        // Receive updates from server
        connection.on("ScaleUpdated", (dto) => {
            if (dto && Number(dto.Id) === scaleId) {
                signalRConnected = true;
                lastMessageUtc = Date.now();
                render(dto);
            }
        });

        // Connection lifecycle
        connection.onreconnecting(() => {
            signalRConnected = false;
            setText(statusEl, "Reconnecting...");
            applyVisualState();
        });

        connection.onreconnected(async () => {
            signalRConnected = true;
            lastMessageUtc = Date.now();

            try {
                await connection.invoke("JoinScale", scaleId);
                setText(statusEl, "Connected");
            } catch {
                setText(statusEl, "Connected (join failed)");
            }

            applyVisualState();
        });

        connection.onclose(() => {
            signalRConnected = false;
            setText(statusEl, "Disconnected");
            applyVisualState();
        });

        // Start + join group
        await connection.start();
        signalRConnected = true;
        lastMessageUtc = Date.now();

        await connection.invoke("JoinScale", scaleId);

        setText(statusEl, "Connected");
        applyVisualState();

        // Watchdog detects “connected but dead”
        startWatchdog();
    }

    // Kick off
    startSignalR().catch(err => {
        console.error(err);
        signalRConnected = false;
        setText(statusEl, "SignalR connection failed");
        setText(promptEl, "");
        applyVisualState();
    });

    function paramsNotSet() {
        if (!scaleId || scaleId < 1)   {
            setText(statusEl, "Missing scaleId. Use ?scaleId=1&printerId=1");
            setText(promptEl, "");
            signalRConnected = false;
            applyVisualState();
            return true;
        }
        if (!printerId || printerId < 1) {
            setText(statusEl, "Missing printerId. Use ?scaleId=1&printerId=1");
            setText(promptEl, "");
            signalRConnected = false;
            applyVisualState();
            return true;
        }
        return false;

    }

    async function startPrintSignalR() {
       

        if (paramsNotSet()) return;

    

        if (typeof signalR === "undefined") {
            setText(statusEl, "SignalR script not loaded.");
            setText(promptEl, "");
            signalRConnected = false;
            applyVisualState();
            return;
        }

        const printConn = new signalR.HubConnectionBuilder()
            .withUrl("/hubs/print")
            .withAutomaticReconnect()
            .build();

        // Receive print command
        printConn.on("PrintLoadTicket", (msg) => {
            if (!msg) return;

            // Safety: only react if printerId matches THIS kiosk
            if ((msg.printerId || "").toString() !== printerId) return;

            showPrinting(msg.ticket);
        });

        await printConn.start();

        // You need a hub method that binds connection <-> printerId
        // (example name; implement on server)
        await printConn.invoke("RegisterPrinter", printerId);
    }


    startPrintSignalR().catch(err => console.error("Print hub failed", err));

  

})();
