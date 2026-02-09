(function () {
    "use strict";
    console.log("✅ warehouse.kiosk.js loaded", new Date().toISOString());


    async function printTicket(btnEl, ticket) {
        const deviceId = btnEl?.dataset?.deviceId;

        if (!deviceId) {
            alert("Print device not set (data-device-id missing).");
            return;
        }

        if (!ticket) {
            alert("No ticket number available to print.");
            return;
        }

        const url = `/api/printing/device/${encodeURIComponent(deviceId)}/print-ticket/${encodeURIComponent(ticket)}`;

        btnEl.disabled = true;
            
        try {
            const resp = await fetch(url, { method: "POST" });

            if (!resp.ok) {
                const text = await resp.text();
                throw new Error(text || resp.statusText);
            }

            console.log("Print request sent:", { deviceId, ticket });
            alert(`Print sent for ticket ${ticket}`);
        }
        catch (err) {
            console.error("Print failed", err);
            alert(`Failed to send print request.\n${err?.message || err}`);
        }
        finally {
            btnEl.disabled = false;
        }
    }

    document.addEventListener("click", function (e) {
        const btn = e.target.closest("#btnPrintTicket");
        if (!btn) return;

        const ticket = btn.dataset.ticket;
        printTicket(btn, ticket);
    });



    window.gmWarehouseModeInit = window.gmWarehouseModeInit || {};

    window.gmWarehouseModeInit.kiosk = (function () {
        let initialized = false;
        let pc = null;

        function dispose() {
            try { pc && pc.close(); } catch { }
            pc = null;
            initialized = false;
        }

        async function startCam1() {
            const video = document.getElementById("cam1");
            const status = document.getElementById("status");
            const btn = document.getElementById("fullscreenBtn");

            // If partial isn't loaded yet, bail safely
            if (!video || !status || !btn) return false;

            const setStatus = (m) => { status.textContent = m; };

            // Prevent double-wiring the button if refresh() runs again
            if (!btn.dataset.wired) {
                btn.dataset.wired = "1";
                btn.addEventListener("click", () => {
                    if (!document.fullscreenElement) {
                        if (video.requestFullscreen) video.requestFullscreen();
                        else if (video.webkitRequestFullscreen) video.webkitRequestFullscreen(); // Safari
                        else if (video.msRequestFullscreen) video.msRequestFullscreen(); // Old Edge
                    } else {
                        if (document.exitFullscreen) document.exitFullscreen();
                    }
                });
            }

            try {
                setStatus("Connecting…");

                // If we already had a connection, reset it
                if (pc) {
                    try { pc.close(); } catch { }
                    pc = null;
                }

                pc = new RTCPeerConnection({ iceServers: [] });

                pc.ontrack = (e) => {
                    video.srcObject = e.streams[0];
                    setStatus("Live");
                };

                pc.onconnectionstatechange = () => {
                    setStatus("WebRTC: " + pc.connectionState);
                };

                // Video only first
                pc.addTransceiver("video", { direction: "recvonly" });

                const offer = await pc.createOffer();
                await pc.setLocalDescription(offer);

                // nginx proxy endpoint (same-origin over HTTPS)
                const apiPath = "/rtc/v1/play/";

                // IMPORTANT: match SRS demo expectations
                const srsApi = "http://144.202.88.133:1985/rtc/v1/play/";
                const streamUrl = "webrtc://144.202.88.133/live/cam1";

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
                    return false;
                }

                await pc.setRemoteDescription({ type: "answer", sdp: answer.sdp });
                setStatus("Connected");
                return true;

            } catch (err) {
                console.error(err);
                setStatus("Error: " + err);
                return false;
            }
        }

        function ensureInitialized() {
            if (initialized) return true;

            // Only set initialized true if elements exist and start succeeds
            initialized = true;
            return true;
        }

        async function refresh() {
            // Called after kiosk partial is injected by warehouse.index.js
            ensureInitialized();

            // Start (or restart) cam1 playback
            await startCam1();
        }

        return {
            ensureInitialized,
            refresh,
            dispose
        };
    })();

})();
