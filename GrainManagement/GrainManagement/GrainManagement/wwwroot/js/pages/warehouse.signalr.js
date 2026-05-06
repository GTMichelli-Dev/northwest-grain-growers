(function () {
    "use strict";

    function getLocationId() {
        return (window.GM && typeof GM.getLocationId === "function")
            ? GM.getLocationId()
            : 0;
    }

    let connection = null;
    let starting = null;
    let joinedLocationId = 0;
    const wsUpdateListeners = new Set();

    async function ensureConnected() {
        if (connection && connection.state === "Connected") return connection;
        if (starting) return starting;

        if (!window.signalR) {
            throw new Error("SignalR client library not loaded");
        }

        connection = new signalR.HubConnectionBuilder()
            .withUrl("/hubs/warehouse")
            .withAutomaticReconnect()
            .build();

        // Intake snapshot pushed from server
        connection.on("intakeSnapshot", (snapshot) => {
            try {
                const mod = window.gmWarehouseModeInit?.intake;
                if (mod && typeof mod.applySnapshot === "function") {
                    mod.applySnapshot(snapshot);
                }
            } catch (e) {
                console.error("Failed applying intake snapshot", e);
            }
        });

        // Weight-sheet mutation broadcast — fans out to every page listener.
        // Payload: { LocationId, WeightSheetId, ChangeKind, At }
        connection.on("weightSheetUpdated", (payload) => {
            wsUpdateListeners.forEach((fn) => {
                try { fn(payload); }
                catch (e) { console.error("weightSheetUpdated listener failed", e); }
            });
        });

        // Re-join the location group after any reconnect so we keep getting events.
        connection.onreconnected(() => {
            if (joinedLocationId > 0) {
                connection.invoke("JoinLocation", joinedLocationId).catch(() => {});
            }
        });

        starting = connection.start()
            .catch(err => {
                console.error("SignalR connect failed", err);
                throw err;
            })
            .finally(() => { starting = null; });

        await starting;
        return connection;
    }

    async function requestIntakeRefresh() {
        const loc = getLocationId();
        if (!loc) return;

        const conn = await ensureConnected();
        await conn.invoke("RequestIntakeRefresh", loc);
    }

    /**
     * Subscribe this client to weight-sheet mutation broadcasts for the
     * current (or supplied) location. Returns an unsubscribe function.
     *
     *   const off = gmWarehouseRealtime.onWeightSheetUpdated((p) => { ... });
     *   off();   // unsubscribe
     */
    async function onWeightSheetUpdated(callback, locationId) {
        if (typeof callback !== "function") return function () {};
        wsUpdateListeners.add(callback);

        const loc = locationId || getLocationId();
        try {
            const conn = await ensureConnected();
            if (loc > 0 && joinedLocationId !== loc) {
                await conn.invoke("JoinLocation", loc);
                joinedLocationId = loc;
            }
        } catch (e) {
            // Connection failed — listener stays registered; if reconnect
            // succeeds later we'll re-join the group automatically.
        }
        return function unsubscribe() { wsUpdateListeners.delete(callback); };
    }

    // Expose global helper
    window.gmWarehouseRealtime = {
        ensureConnected,
        requestIntakeRefresh,
        onWeightSheetUpdated,
    };
})();
