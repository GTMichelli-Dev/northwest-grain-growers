(function () {
    'use strict';

    const SESSION_KEY = 'gm_scaleWeight_source';

    const SEL = {
        root:        '#gmScaleWeight',
        sources:     '#gwSwSources',
        live:        '#gwSwLive',
        liveWeight:  '#gwSwLiveWeight',
        liveStatus:  '#gwSwLiveStatus',
        manual:      '#gwSwManual',
        manualInput: '#gwSwManualInput',
        hiddenWeight:'#gwSwValue',
        hiddenSource:'#gwSwSourceType',
        prompt:      '#gwSwPrompt',
    };

    let connection     = null;
    let currentScaleId = null;

    async function init() {
        if (!document.querySelector(SEL.root)) return;

        let scales = [];
        try {
            const resp = await fetch('/api/Scale/CachedScales');
            if (resp.ok) scales = await resp.json();
        } catch { /* scales unavailable – fall through to manual only */ }

        buildSources(scales);
        bindManualInput();
    }

    // ── Source selector ──────────────────────────────────────────────────────

    function buildSources(scales) {
        const container = document.querySelector(SEL.sources);
        if (!container) return;
        container.innerHTML = '';

        scales.forEach(s => container.appendChild(makeBtn(String(s.Id), esc(s.Description))));
        container.appendChild(makeBtn('manual', 'Manual Weight'));

        container.addEventListener('click', e => {
            const btn = e.target.closest('.gm-sw-sourceBtn');
            if (btn) selectSource(btn.dataset.value, container);
        });

        // Restore previous session selection
        const saved = sessionStorage.getItem(SESSION_KEY);
        if (saved) {
            const match = container.querySelector(`.gm-sw-sourceBtn[data-value="${CSS.escape(saved)}"]`);
            if (match) selectSource(saved, container);
        }
        // No auto-select – operator must choose
    }

    function makeBtn(value, label) {
        const btn = document.createElement('button');
        btn.type        = 'button';
        btn.className   = 'gm-sw-sourceBtn';
        btn.dataset.value = value;
        btn.textContent = label;
        return btn;
    }

    function selectSource(value, container) {
        // Toggle active class on buttons
        container.querySelectorAll('.gm-sw-sourceBtn').forEach(b => {
            b.classList.toggle('gm-sw-sourceBtn--active', b.dataset.value === value);
        });

        sessionStorage.setItem(SESSION_KEY, value);
        handleSourceChange(value);
    }

    function handleSourceChange(value) {
        const liveEl   = document.querySelector(SEL.live);
        const manualEl = document.querySelector(SEL.manual);
        const srcInput = document.querySelector(SEL.hiddenSource);
        const promptEl = document.querySelector(SEL.prompt);

        disconnectScale();

        if (promptEl) promptEl.hidden = true;

        if (value === 'manual') {
            liveEl.hidden   = true;
            manualEl.hidden = false;
            srcInput.value  = 'manual';
            syncManualWeight();
        } else {
            liveEl.hidden   = false;
            manualEl.hidden = true;
            srcInput.value  = 'scale';
            connectScale(parseInt(value, 10));
        }
    }

    // ── Live scale ───────────────────────────────────────────────────────────

    async function connectScale(scaleId) {
        currentScaleId = scaleId;

        // Seed immediately from cache
        try {
            const resp = await fetch('/api/Scale/CachedScales');
            if (resp.ok) {
                const scales = await resp.json();
                const match  = scales.find(s => s.Id === scaleId);
                if (match) applyDto(match);
            }
        } catch { }

        connection = new signalR.HubConnectionBuilder()
            .withUrl('/hubs/scale')
            .withAutomaticReconnect()
            .build();

        connection.on('ScaleUpdated', dto => {
            if (dto.Id === currentScaleId) applyDto(dto);
        });

        try {
            await connection.start();
            await connection.invoke('JoinScale', scaleId);
        } catch (err) {
            console.warn('[ScaleWeight] SignalR connect failed', err);
        }
    }

    function applyDto(dto) {
        const weightEl = document.querySelector(SEL.liveWeight);
        const statusEl = document.querySelector(SEL.liveStatus);
        const hiddenW  = document.querySelector(SEL.hiddenWeight);

        if (weightEl) weightEl.textContent = fmtWeight(dto.Weight);

        if (statusEl) {
            if (!dto.Ok) {
                statusEl.textContent    = dto.Status || 'No connection';
                statusEl.dataset.ok     = '0';
                statusEl.dataset.motion = '0';
            } else if (dto.Motion) {
                statusEl.textContent    = 'Motion – wait for stable';
                statusEl.dataset.ok     = '1';
                statusEl.dataset.motion = '1';
            } else {
                statusEl.textContent    = 'Stable';
                statusEl.dataset.ok     = '1';
                statusEl.dataset.motion = '0';
            }
        }

        // Only commit a weight when the scale is OK and not in motion
        hiddenW.value = (dto.Ok && !dto.Motion) ? Math.round(dto.Weight) : '';
    }

    function disconnectScale() {
        if (connection) {
            if (currentScaleId !== null) {
                connection.invoke('LeaveScale', currentScaleId).catch(() => { });
            }
            connection.stop();
            connection = null;
        }
        currentScaleId = null;
    }

    // ── Manual entry ─────────────────────────────────────────────────────────

    function bindManualInput() {
        document.querySelector(SEL.manualInput)
            ?.addEventListener('input', syncManualWeight);
    }

    function syncManualWeight() {
        const input  = document.querySelector(SEL.manualInput);
        const hidden = document.querySelector(SEL.hiddenWeight);
        if (!input || !hidden) return;

        let val = parseInt(input.value, 10);
        if (isNaN(val) || val < 0) {
            val         = 0;
            input.value = 0;
        }
        hidden.value = val;
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    function fmtWeight(w) {
        return Number(w).toLocaleString(undefined, { maximumFractionDigits: 0 }) + ' lbs';
    }

    function esc(str) {
        return String(str).replace(/[&<>"']/g, c =>
            ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c]));
    }

    document.addEventListener('DOMContentLoaded', init);
})();
