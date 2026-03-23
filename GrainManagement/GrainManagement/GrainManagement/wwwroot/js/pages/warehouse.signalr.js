(function () {
    "use strict";

    function getLocationId() {
        return GM.getLocationId();
    }

    let connection = null;
    let starting = null;

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

    // Expose global helper
    window.gmWarehouseRealtime = {
        ensureConnected,
        requestIntakeRefresh
    };
})();
