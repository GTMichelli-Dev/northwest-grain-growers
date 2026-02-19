(function () {
    "use strict";

    const video = document.getElementById("cam1");
    const status = document.getElementById("status");
    const fullscreenBtn = document.getElementById("fullscreenBtn");

    if (!video || !status) return;

    const setStatus = (m) => { status.textContent = m; };

    // -----------------------------
    // Fullscreen
    // -----------------------------
    if (fullscreenBtn && !fullscreenBtn.dataset.wired) {
        fullscreenBtn.dataset.wired = "1";
        fullscreenBtn.addEventListener("click", () => {
            if (!document.fullscreenElement) {
                if (video.requestFullscreen) video.requestFullscreen();
                else if (video.webkitRequestFullscreen) video.webkitRequestFullscreen();
                else if (video.msRequestFullscreen) video.msRequestFullscreen();
            } else {
                if (document.exitFullscreen) document.exitFullscreen();
            }
        });
    }

    // -----------------------------
    // Inactivity -> return to /Camera after 2 minutes
    // -----------------------------
    const INACTIVITY_MS = 2 * 60 * 1000;
    let inactivityTimer = null;

    function resetInactivity() {
        if (inactivityTimer) clearTimeout(inactivityTimer);
        inactivityTimer = setTimeout(() => {
            window.location.href = "/Camera";
        }, INACTIVITY_MS);
    }

    ["mousemove", "mousedown", "keydown", "touchstart", "scroll", "click"].forEach(evt => {
        window.addEventListener(evt, resetInactivity, { passive: true });
    });
    resetInactivity();

    // -----------------------------
    // Config (from View.cshtml)
    // -----------------------------
    const cameraId =
        (window.gmCamera && window.gmCamera.cameraId) ||
        (window.gmCamera && window.gmCamera.streamKey) ||
        "cam1";

    const streamKey =
        (window.gmCamera && window.gmCamera.streamKey) ? window.gmCamera.streamKey : "cam1";

    // -----------------------------
    // SignalR presence
    // -----------------------------
    const HUB_URL = "/hubs/camera";
    let hub = null;
    let hubStarted = false;

    async function ensureHub() {
        if (hubStarted) return true;

        if (!window.signalR) {
            console.warn("signalR client not found. Streaming will always attempt to play (no start/stop control).");
            return false;
        }

        hub = new signalR.HubConnectionBuilder()
            .withUrl(HUB_URL)
            .withAutomaticReconnect()
            .build();

        hub.onreconnecting(() => setStatus("SignalR reconnecting…"));
        hub.onreconnected(() => setStatus("SignalR connected"));
        hub.onclose(() => setStatus("SignalR closed"));

        await hub.start();
        hubStarted = true;
        return true;
    }

    async function joinCamera() {
        const ok = await ensureHub();
        if (!ok) return;

        try {
            await hub.invoke("JoinCamera", cameraId);
        } catch (e) {
            console.warn("JoinCamera failed:", e);
        }
    }

    async function leaveCamera() {
        if (!hubStarted) return;

        try {
            await hub.invoke("LeaveCamera", cameraId);
        } catch (e) {
            console.warn("LeaveCamera failed:", e);
        }
    }

    // -----------------------------
    // WebRTC playback (SRS /rtc/v1/play/)
    // -----------------------------
    let pc = null;

    function cleanupWebRtc() {
        try {
            if (pc) {
                try { pc.ontrack = null; } catch { }
                try { pc.onconnectionstatechange = null; } catch { }
                try { pc.close(); } catch { }
                pc = null;
            }
        } catch { }

        try {
            if (video.srcObject) {
                try { video.srcObject.getTracks().forEach(t => t.stop()); } catch { }
                video.srcObject = null;
            }
        } catch { }
    }

    async function startWebRtc() {
        try {
            setStatus("Connecting…");

            cleanupWebRtc();

            pc = new RTCPeerConnection({ iceServers: [] });

            pc.ontrack = (e) => {
                video.srcObject = e.streams[0];
                setStatus("Live");
            };

            pc.onconnectionstatechange = () => {
                setStatus("WebRTC: " + pc.connectionState);
            };

            pc.addTransceiver("video", { direction: "recvonly" });

            const offer = await pc.createOffer();
            await pc.setLocalDescription(offer);

            const apiPath = "/rtc/v1/play/";
            const host = window.location.hostname;

            const srsApi = `http://${host}:1985/rtc/v1/play/`;
            const streamUrl = `webrtc://${host}/live/${streamKey}`;

            const resp = await fetch(apiPath, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    api: srsApi,
                    streamurl: streamUrl,
                    clientip: null,
                    tid: Math.random().toString(16).slice(2),
                    sdp: offer.sdp
                })
            });

            const answer = await resp.json();

            if (answer.code !== 0) {
                console.error("SRS error:", answer);
                setStatus("SRS error: " + (answer.msg || answer.code));
                return;
            }

            await pc.setRemoteDescription({ type: "answer", sdp: answer.sdp });
            setStatus("Connected");
        } catch (err) {
            console.error(err);
            setStatus("Error: " + (err?.message || err));
        }
    }

    // -----------------------------
    // Watching state = tab visible
    // (You can tighten this later to "visible AND playing".)
    // -----------------------------
    let isVisible = !document.hidden;
    let isWatching = false;

    // Heartbeat keeps server-side presence clean (optional but helpful)
    const HEARTBEAT_MS = 10_000;
    let heartbeatTimer = null;

    function stopHeartbeat() {
        if (heartbeatTimer) clearInterval(heartbeatTimer);
        heartbeatTimer = null;
    }

    function startHeartbeat() {
        stopHeartbeat();
        if (!hubStarted) return;
        heartbeatTimer = setInterval(() => {
            // "touch" join periodically so server can TTL-clean zombies if you implement that later
            try { hub.invoke("JoinCamera", cameraId); } catch { }
        }, HEARTBEAT_MS);
    }

    async function updateWatching() {
        const shouldWatch = isVisible; // your requirement: hidden tab => stop

        if (shouldWatch === isWatching) return;
        isWatching = shouldWatch;

        if (isWatching) {
            // Ask server to start publisher on Pi
            await joinCamera();

            // Give the Pi a moment to start publishing before play (optional).
            // If you don’t want any delay, set to 0.
            await new Promise(r => setTimeout(r, 250));

            // Start WebRTC play
            await startWebRtc();

            // Start heartbeats after hub is up
            startHeartbeat();
        } else {
            stopHeartbeat();

            // Stop WebRTC immediately to save bandwidth
            cleanupWebRtc();

            // Tell server viewer left (server can delay actual stop)
            await leaveCamera();

            setStatus("Paused (tab hidden)");
        }
    }

    // Visibility changes drive watching
    document.addEventListener("visibilitychange", () => {
        isVisible = !document.hidden;
        updateWatching();
    });

    // Best-effort cleanup when navigating away
    window.addEventListener("pagehide", () => {
        isVisible = false;
        updateWatching();
    });

    window.addEventListener("beforeunload", () => {
        // fire-and-forget best effort
        try { leaveCamera(); } catch { }
    });

    // Initial state
    updateWatching();
})();
