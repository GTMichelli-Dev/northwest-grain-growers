(function () {
    "use strict";

    const video = document.getElementById("cam1");
    const status = document.getElementById("status");
    const fullscreenBtn = document.getElementById("fullscreenBtn");

    if (!video || !status) return;

    const setStatus = (m) => { status.textContent = m; };

    // --- Fullscreen ---
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

    // --- Inactivity -> return to /Camera after 2 minutes ---
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

    // --- WebRTC playback (same pattern as warehouse.kiosk.js, but parameterized) ---
    let pc = null;

    async function start() {
        try {
            setStatus("Connecting…");

            if (pc) { try { pc.close(); } catch { } pc = null; }

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

            // Same nginx-proxied endpoint you’re already using
            const apiPath = "/rtc/v1/play/";

            // Use current host for stream + SRS API
            const host = window.location.hostname;

            const streamKey = (window.gmCamera && window.gmCamera.streamKey) ? window.gmCamera.streamKey : "cam1";
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

    start();
})();
