(function () {
  "use strict";

  window.gmWarehouseModeInit = window.gmWarehouseModeInit || {};

  window.gmWarehouseModeInit.newtruck = (function () {

    const config = {
      listScalesUrl: "/api/Scale/CachedScales",
      hubUrl: "/hubs/scale", // update if your Program.cs maps ScaleHub differently

      // WebRTC (same pattern as camera.view.js)
      playApiPath: "/rtc/v1/play/",

      // Default stream keys (override per-site/per-scale via window.gmNewTruckCameras)
      defaultStreams: { kiosk: "1", bol: "3", scale: "2" }
    };

    let selected = {
      mode: "none",     // none | manual | scale
      scaleId: null,
      scaleName: null
    };

    let hubConn = null;

    // WebRTC players
    const players = { kiosk: null, bol: null, scale: null };

    const els = {};

    function cacheEls() {
      els.btnPickScale = document.getElementById("btnPickScale");
      els.pickScaleModal = document.getElementById("pickScaleModal");
      els.manualWeightModal = document.getElementById("manualWeightModal");
      els.scaleList = document.getElementById("gmScaleList");
      els.btnManualScale = document.getElementById("btnManualScale");
      els.manualInput = document.getElementById("manualWeightInput");
      els.manualErr = document.getElementById("manualWeightError");
      els.btnManualOk = document.getElementById("btnManualWeightOk");
      els.btnSave = document.getElementById("btnSaveNewTruck");
      els.btnPrint = document.getElementById("btnPrintTicket");

      els.scaleBox = document.getElementById("gmScaleBox");
      els.scaleName = document.getElementById("gmScaleName");
      els.scaleStatus = document.getElementById("gmScaleStatus");
      els.scaleWeight = document.getElementById("gmScaleWeight");

      // Cameras
      els.camPanel = document.getElementById("gmNewTruckCams");
      els.camKiosk = document.getElementById("gmCamKiosk");
      els.camBol = document.getElementById("gmCamBol");
      els.camScale = document.getElementById("gmCamScale");
      els.camStatusKiosk = document.getElementById("gmCamStatusKiosk");
      els.camStatusBol = document.getElementById("gmCamStatusBol");
      els.camStatusScale = document.getElementById("gmCamStatusScale");

      // NOTE: this modal id must exist in the parent view (not in this partial)
      els.newTruckModal = document.getElementById("newIntakeTruckModal");
    }

    function safeText(el, text) {
      if (el) el.textContent = text ?? "";
    }

    function setScaleUI({ name, weight, ok, motion, statusText, mode }) {
      if (!els.scaleBox) return;

      safeText(els.scaleName, name || "No scale selected");
      safeText(els.scaleStatus, statusText || "—");

      // If not ok -> force 0 (per your spec)
      const w = (ok === false) ? 0 : (Number(weight) || 0);
      safeText(els.scaleWeight, String(w));

      // clear all states
      els.scaleBox.classList.remove(
        "gm-scaleBox--ok",
        "gm-scaleBox--motion",
        "gm-scaleBox--bad",
        "gm-scaleBox--manual"
      );

      // Priority:
      // manual > bad > motion > ok
      if (mode === "manual" || statusText === "Manual" || name === "Manual") {
        els.scaleBox.classList.add("gm-scaleBox--manual");
        return;
      }

      if (ok === false) {
        els.scaleBox.classList.add("gm-scaleBox--bad");
      } else if (motion === true) {
        els.scaleBox.classList.add("gm-scaleBox--motion");
      } else {
        els.scaleBox.classList.add("gm-scaleBox--ok");
      }
    }


    // ===========================
    // Cameras (WebRTC)
    // ===========================

    function getStreamKeys(scaleId) {
      // Optional overrides:
      // window.gmNewTruckCameras = {
      //   default: { kiosk:"1", bol:"2", scale:"3" },
      //   byScale: { 5: { kiosk:"kiosk5", bol:"bol5", scale:"scale5" } }
      // }
      const m = window.gmNewTruckCameras || {};
      const byScale = m.byScale || m.ByScale || {};
      const dflt = m.default || m.Default || config.defaultStreams;

      const per = byScale[String(scaleId)] || byScale[Number(scaleId)] || null;
      return {
        kiosk: (per && (per.kiosk || per.Kiosk)) || dflt.kiosk,
        bol: (per && (per.bol || per.Bol)) || dflt.bol,
        scale: (per && (per.scale || per.Scale)) || dflt.scale
      };
    }

    function requestFullscreen(el) {
      if (!el) return;
      try {
        if (el.requestFullscreen) el.requestFullscreen();
        else if (el.webkitRequestFullscreen) el.webkitRequestFullscreen();
        else if (el.msRequestFullscreen) el.msRequestFullscreen();
      } catch { }
    }

    function wireVideoFullscreen(videoEl) {
      if (!videoEl || videoEl.dataset.gmWiredFullscreen) return;
      videoEl.dataset.gmWiredFullscreen = "1";
      videoEl.addEventListener("click", () => requestFullscreen(videoEl));
    }

    function stopAllPlayers() {
      for (const k of Object.keys(players)) {
        try { players[k]?.stop?.(); } catch { }
        players[k] = null;
      }
    }

    function setCamPanelVisible(visible) {
      if (!els.camPanel) return;
      els.camPanel.classList.toggle("d-none", !visible);
    }

    function makePlayer(videoEl, statusEl) {
      let pc = null;

      const setStatus = (m) => safeText(statusEl, m);

      async function start(streamKey) {
        if (!videoEl) return;

        // Always re-wire click-to-fullscreen
        wireVideoFullscreen(videoEl);

        try {
          setStatus("Connecting…");

          if (pc) { try { pc.close(); } catch { } pc = null; }

          pc = new RTCPeerConnection({ iceServers: [] });

          pc.ontrack = (e) => {
            videoEl.srcObject = e.streams[0];
            setStatus("Live");
          };

          pc.onconnectionstatechange = () => {
            setStatus("WebRTC: " + pc.connectionState);
          };

          pc.addTransceiver("video", { direction: "recvonly" });

          const offer = await pc.createOffer();
          await pc.setLocalDescription(offer);

          const host = window.location.hostname;
          const srsApi = `http://${host}:1985/rtc/v1/play/`;
          const streamUrl = `webrtc://${host}/live/${streamKey}`;

          const resp = await fetch(config.playApiPath, {
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

      function stop() {
        try { if (pc) pc.close(); } catch { }
        pc = null;
        if (videoEl) {
          try { videoEl.srcObject = null; } catch { }
        }
        setStatus("—");
      }

      return { start, stop };
    }

    async function updateCameras() {
      const show = (selected.mode === "scale" && Number(selected.scaleId) > 0);

      if (!show) {
        setCamPanelVisible(false);
        stopAllPlayers();
        return;
      }

      setCamPanelVisible(true);

      const keys = getStreamKeys(selected.scaleId);

      players.kiosk = players.kiosk || makePlayer(els.camKiosk, els.camStatusKiosk);
      players.bol = players.bol || makePlayer(els.camBol, els.camStatusBol);
      players.scale = players.scale || makePlayer(els.camScale, els.camStatusScale);

      await players.kiosk.start(keys.kiosk);
      await players.bol.start(keys.bol);
      await players.scale.start(keys.scale);
    }


    // ===========================
    // Print (same pattern as warehouse.kiosk.js)
    // ===========================

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

      const url = `/api/printing/printer/${encodeURIComponent(deviceId)}/print-ticket/${encodeURIComponent(ticket)}`;

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

    function updatePrintButtonDefaults() {
      if (!els.btnPrint) return;

      // Optional global override:
      // window.gmNewTruckPrint = { deviceId:"1", ticket:"123456" }
      const p = window.gmNewTruckPrint || {};

      const deviceId = (p.deviceId || p.DeviceId || "") || (selected.mode === "scale" ? String(selected.scaleId || "") : "");
      const ticket = (p.ticket || p.Ticket || "") || els.btnPrint.dataset.ticket || "";

      els.btnPrint.dataset.deviceId = deviceId;
      els.btnPrint.dataset.ticket = ticket;

      // Enable if it has a device id; ticket may get set later
      els.btnPrint.disabled = !deviceId;
    }


    // ===========================
    // SignalR (Scale)
    // ===========================

    async function ensureHubConnected() {
      if (!window.signalR) {
        console.warn("SignalR client not loaded (signalR is undefined).");
        return;
      }

      if (hubConn && hubConn.state === "Connected") return;

      hubConn = new signalR.HubConnectionBuilder()
        .withUrl(config.hubUrl)
        .withAutomaticReconnect()
        .build();

      // Server pushes IScaleClient.ScaleUpdated(ScaleDto dto)
      hubConn.on("ScaleUpdated", (dto) => {
        if (selected.mode !== "scale") return;

        const id = dto?.id ?? dto?.Id;
        if (String(id) !== String(selected.scaleId)) return;

        const name = dto?.description ?? dto?.Description ?? selected.scaleName ?? `Scale ${id}`;
        const ok = (dto?.ok ?? dto?.Ok) !== false;
        const motion = (dto?.motion ?? dto?.Motion) === true;
        const weight = ok ? (dto?.weight ?? dto?.Weight ?? 0) : 0;

        const statusText =
          (dto?.status ?? dto?.Status) ??
          (ok ? (motion ? "Motion" : "OK") : "No connection");

        setScaleUI({ name, weight, ok, motion, statusText, mode: "scale" });
      });

      try {
        await hubConn.start();
      } catch (e) {
        console.warn("ScaleHub start failed:", e);
      }
    }

    async function joinScale(scaleId) {
      if (!hubConn || hubConn.state !== "Connected") return;
      if (!scaleId || scaleId < 1) return;

      try {
        await hubConn.invoke("JoinScale", Number(scaleId));
      } catch (e) {
        console.warn("JoinScale failed:", e);
      }
    }

    async function leaveScale(scaleId) {
      if (!hubConn || hubConn.state !== "Connected") return;
      if (!scaleId || scaleId < 1) return;

      try {
        await hubConn.invoke("LeaveScale", Number(scaleId));
      } catch (e) {
        console.warn("LeaveScale failed:", e);
      }
    }


    // ===========================
    // Load list of scales
    // ===========================

    async function loadScales() {
      if (!els.scaleList) return;

      els.scaleList.innerHTML = `<div class="text-muted">Loading…</div>`;

      const resp = await fetch(config.listScalesUrl, { headers: { "Accept": "application/json" } });
      if (!resp.ok) {
        els.scaleList.innerHTML = `<div class="text-danger">Failed to load scales</div>`;
        return;
      }

      const scales = await resp.json();
      els.scaleList.innerHTML = "";

      if (!Array.isArray(scales) || scales.length === 0) {
        els.scaleList.innerHTML = `<div class="text-muted">No scales found</div>`;
        return;
      }

      for (const s of scales) {
        const id = s.id ?? s.Id;
        const name = s.description ?? s.Description ?? s.name ?? s.Name ?? ("Scale " + id);

        const btn = document.createElement("button");
        btn.type = "button";
        btn.className = "btn btn-outline-primary w-100 text-start mb-2";
        btn.textContent = name;

        btn.addEventListener("click", async () => {
          const prev = selected.scaleId;

          selected.mode = "scale";
          selected.scaleId = id;
          selected.scaleName = name;

          // immediately reflect current cached state
          const ok = (s.ok ?? s.Ok) !== false;
          const motion = (s.motion ?? s.Motion) === true;
          const weight = ok ? (s.weight ?? s.Weight ?? 0) : 0;
          const statusText = (s.status ?? s.Status) ?? (ok ? (motion ? "Motion" : "OK") : "No connection");

          setScaleUI({ name, weight, ok, motion, statusText, mode: "scale" });

          // join hub group for this scale (and leave previous)
          await ensureHubConnected();
          await leaveScale(prev);
          await joinScale(id);

          // Cameras + print button update
          updatePrintButtonDefaults();
          await updateCameras();

          bootstrap.Modal.getInstance(els.pickScaleModal)?.hide();
        });

        els.scaleList.appendChild(btn);
      }
    }

    function openPickModal() {
      loadScales().catch(console.error);

      if (!els.pickScaleModal) {
        console.error("pickScaleModal element not found");
        return;
      }
      if (!window.bootstrap?.Modal) {
        alert("Bootstrap modal JS not loaded. (bootstrap.Modal missing)");
        return;
      }

      if (!els.pickScaleModal._gmModal) {
        els.pickScaleModal._gmModal = new bootstrap.Modal(els.pickScaleModal);
      }
      els.pickScaleModal._gmModal.show();
    }


    function openManualModal() {
      if (!els.manualInput || !els.manualErr) return;

      els.manualErr.classList.add("d-none");
      els.manualInput.value = "";
      els.manualInput.removeAttribute("readonly");
      els.manualInput.removeAttribute("disabled");

      if (!els.manualWeightModal._gmModal) {
        els.manualWeightModal._gmModal = new bootstrap.Modal(els.manualWeightModal);
      }
      els.manualWeightModal._gmModal.show();

      els.manualWeightModal.addEventListener("shown.bs.modal", () => {
        els.manualInput.focus();
      }, { once: true });
    }

    async function applyManualWeight() {
      if (!els.manualInput || !els.manualErr) return;

      const v = Number(els.manualInput.value);
      const valid = Number.isFinite(v) && v >= 1 && v <= 1000000;

      if (!valid) {
        els.manualErr.classList.remove("d-none");
        return;
      }

      // leave hub group if we were on a real scale
      const prev = selected.scaleId;
      selected.mode = "manual";
      selected.scaleId = null;
      selected.scaleName = "Manual";
      leaveScale(prev).catch(console.error);

      setScaleUI({
        name: "Manual",
        weight: Math.trunc(v),
        ok: true,
        motion: false,
        statusText: "Manual",
        mode: "manual"
      });

      // Hide cameras when manual
      updatePrintButtonDefaults();
      await updateCameras();

      els.manualWeightModal?._gmModal?.hide();
      els.pickScaleModal?._gmModal?.hide();
    }

    async function saveNewTruck() {
      const payload = {
        mode: selected.mode,
        scaleId: selected.mode === "scale" ? Number(selected.scaleId) : null,
        scaleName: selected.scaleName,
        weight: Number(els.scaleWeight?.textContent) || 0
      };

      const resp = await fetch("/api/warehouse/intake/new-truck", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload)
      });

      if (!resp.ok) {
        console.error("Save failed:", resp.status);
        return;
      }

      // return to intake
      if (window.gmWarehouse?.initMode) {
        await window.gmWarehouse.initMode("intake");
        window.gmWarehouseModeInit?.intake?.refresh?.();
      } else {
        console.warn("gmWarehouse.initMode not available. Expose it in warehouse.index.js.");
      }
    }


    function wireEvents() {
      // Global guard: prevents multiple document click handlers across reloads
      if (window.__gmNewTruckClickWired) return;
      window.__gmNewTruckClickWired = true;

      document.addEventListener("click", (e) => {
        if (e.target.closest("#btnPickScale")) {
          openPickModal();
          return;
        }
        if (e.target.closest("#btnManualScale")) {
          openManualModal();
          return;
        }
        if (e.target.closest("#btnManualWeightOk")) {
          applyManualWeight().catch(console.error);
          return;
        }
        if (e.target.closest("#btnSaveNewTruck")) {
          saveNewTruck().catch(console.error);
          return;
        }
        if (e.target.closest("#btnPrintTicket")) {
          const btn = e.target.closest("#btnPrintTicket");
          const ticket = btn?.dataset?.ticket;
          printTicket(btn, ticket);
          return;
        }
      });
    }


    async function backToIntake() {
      GM.setCookie("GM.WarehouseMode", "intake", 30);

      if (window.gmWarehouse?.initMode) {
        await window.gmWarehouse.initMode("intake");
        window.gmWarehouseModeInit?.intake?.refresh?.();
      }
    }

    function wireCloseBackToIntake() {
      if (!els.newTruckModal) return;
      if (els.newTruckModal.dataset.gmWiredClose) return;
      els.newTruckModal.dataset.gmWiredClose = "true";

      els.newTruckModal.addEventListener("hidden.bs.modal", () => {
        // Clean up camera peer connections when leaving
        stopAllPlayers();
        setCamPanelVisible(false);
        backToIntake().catch(console.error);
      });
    }


    async function ensureInitialized() {
      cacheEls();
      wireEvents();

      setScaleUI({
        name: "No scale selected",
        weight: 0,
        ok: true,
        motion: false,
        statusText: "—",
        mode: "none"
      });

      // Start with cameras hidden
      setCamPanelVisible(false);

      // Print button default state
      updatePrintButtonDefaults();

      wireCloseBackToIntake();

      // non-fatal: connect hub so scale updates are live once a scale is chosen
      await ensureHubConnected();

      // Wire fullscreen for videos if present
      wireVideoFullscreen(els.camKiosk);
      wireVideoFullscreen(els.camBol);
      wireVideoFullscreen(els.camScale);
    }

    return { ensureInitialized };

  })();
})();
