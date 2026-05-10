// scan.bol.js — drives the #scanBolModal partial. Exposes window.scanBol
// with .checkAvailable(locationId?) → Promise<bool> and .open(loadNumber, locationId?).
//
// Image capture is fire-and-forget: clicking "Take BOL Picture" hits
// /api/cameras/{serviceId}/{cameraId}/capture?ticket={load}&direction=bol,
// which broadcasts a SignalR command to that CameraService. The actual
// upload happens server-to-server (CameraService → /api/ticket/{load}/image)
// and the web SignalR ImageCaptured event tells UIs to refresh.
(function () {
    'use strict';

    const SEL = {
        modal:     '#scanBolModal',
        select:    '#bol-camera-select',
        empty:     '#bol-cam-empty',
        img:       '#bol-stream',
        status:    '#bol-stream-status',
        take:      '#bol-take-btn',
        loadLabel: '#bol-load-label'
    };

    let _modalInstance = null;
    let _currentLoad   = null;

    function modal() {
        if (_modalInstance) return _modalInstance;
        const el = document.querySelector(SEL.modal);
        if (!el || !window.bootstrap) return null;
        _modalInstance = new bootstrap.Modal(el);
        document.querySelector(SEL.take).addEventListener('click', onTake);
        document.querySelector(SEL.select).addEventListener('change', onPick);
        return _modalInstance;
    }

    function fetchBolCams(locationId) {
        const url = '/api/cameras/bol' + (locationId ? '?locationId=' + encodeURIComponent(locationId) : '');
        return fetch(url, { cache: 'no-store' }).then(r => r.json());
    }

    function onPick() {
        const sel = document.querySelector(SEL.select);
        const opt = sel.selectedOptions[0];
        const img = document.querySelector(SEL.img);
        const status = document.querySelector(SEL.status);
        const take = document.querySelector(SEL.take);

        if (!opt || !opt.value) {
            img.removeAttribute('src');
            status.textContent = 'No camera selected.';
            take.disabled = true;
            return;
        }

        const streamUrl = opt.getAttribute('data-stream');
        const online    = opt.getAttribute('data-online') === 'true';

        if (online && streamUrl) {
            // Bust cache so reopening doesn't stick a stale frame
            img.src = streamUrl + (streamUrl.indexOf('?') >= 0 ? '&' : '?') + 't=' + Date.now();
            status.textContent = 'Live: ' + streamUrl;
            take.disabled = false;
        } else {
            img.removeAttribute('src');
            status.textContent = online
                ? 'Camera online but no stream URL published.'
                : 'Camera is offline.';
            take.disabled = !online;
        }
    }

    function onTake() {
        const sel = document.querySelector(SEL.select);
        const opt = sel.selectedOptions[0];
        if (!opt || !opt.value || !_currentLoad) return;

        const serviceId = opt.getAttribute('data-service');
        const cameraId  = opt.getAttribute('data-cam');
        const take = document.querySelector(SEL.take);
        take.disabled = true;
        take.textContent = 'Sending…';

        const url = '/api/cameras/' + encodeURIComponent(serviceId) +
                    '/' + encodeURIComponent(cameraId) +
                    '/capture?ticket=' + encodeURIComponent(_currentLoad) +
                    '&direction=bol';
        fetch(url, { method: 'POST' })
            .then(r => r.json())
            .then(() => {
                take.textContent = 'Sent';
                setTimeout(() => {
                    take.textContent = 'Take BOL Picture';
                    take.disabled = false;
                    if (_modalInstance) _modalInstance.hide();
                }, 800);
            })
            .catch(err => {
                console.warn('BOL capture failed', err);
                take.textContent = 'Failed — retry';
                take.disabled = false;
            });
    }

    function populate(cams) {
        const sel   = document.querySelector(SEL.select);
        const empty = document.querySelector(SEL.empty);
        sel.innerHTML = '';

        if (!cams || !cams.length) {
            empty.classList.remove('d-none');
            document.querySelector(SEL.take).disabled = true;
            return;
        }
        empty.classList.add('d-none');

        cams.forEach(function (c) {
            const opt = document.createElement('option');
            opt.value = c.serviceId + '|' + c.cameraId;
            opt.textContent = (c.displayName || c.cameraId) + (c.online ? '' : ' (offline)');
            opt.setAttribute('data-service', c.serviceId);
            opt.setAttribute('data-cam',     c.cameraId);
            opt.setAttribute('data-online',  c.online ? 'true' : 'false');
            opt.setAttribute('data-stream',  c.streamUrl || '');
            sel.appendChild(opt);
        });

        // Auto-select if exactly one online camera (spec)
        const onlineOpts = Array.from(sel.options).filter(o => o.getAttribute('data-online') === 'true');
        if (onlineOpts.length === 1) {
            sel.value = onlineOpts[0].value;
        }
        onPick();
    }

    window.scanBol = {
        /** Returns true if at least one BOL camera exists for the location. */
        checkAvailable: function (locationId) {
            const url = '/api/cameras/bol/available' + (locationId ? '?locationId=' + encodeURIComponent(locationId) : '');
            return fetch(url, { cache: 'no-store' })
                .then(r => r.json())
                .then(j => !!(j && j.available))
                .catch(() => false);
        },

        open: function (loadNumber, locationId) {
            _currentLoad = loadNumber;
            const m = modal();
            if (!m) {
                alert('BOL modal not initialized — make sure _ScanBolModal.cshtml is included.');
                return;
            }
            document.querySelector(SEL.loadLabel).textContent = loadNumber;
            document.querySelector(SEL.take).disabled = true;
            document.querySelector(SEL.img).removeAttribute('src');
            document.querySelector(SEL.status).textContent = 'Loading cameras…';
            fetchBolCams(locationId).then(populate);
            m.show();
        }
    };
})();
