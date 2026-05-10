// load-images.js — populates the In / Out / BOL thumbnail placeholders
// (.gm-gd-thumb) for a given load number. Single source of truth for
// "this load has these images"; used by GrowerDelivery, Transfer, and any
// other view that drops three .gm-gd-thumb tiles with role-suffixed ids.
//
// Usage:
//   gmLoadImages.attach({ prefix: 'gd', loadNumber: 12345 })
//   gmLoadImages.attach({ prefix: 'tl', loadNumber: 12345 })
//
// The helper looks for #{prefix}ThumbIn, #{prefix}ThumbOut, #{prefix}ThumbBol
// and replaces the inner .gm-gd-thumb__placeholder with an <img> when an
// image exists for that direction. A click on the thumb opens a lightbox.
//
// When a fresh capture lands while the user is on the page, the SignalR
// /hubs/camera ImageCaptured event refreshes the matching thumb in place.
(function () {
    'use strict';

    const PLACEHOLDER_HTML =
        '<div class="gm-gd-thumb__placeholder">' +
            '<i class="dx-icon dx-icon-photo" style="font-size:24px;opacity:.4;"></i>' +
        '</div>';

    // serviceId-suffix used in image filenames on disk
    const DIRECTIONS = { In: 'in', Out: 'out', Bol: 'bol' };

    // One module-level SignalR connection; multiple .attach() calls share it.
    let _conn = null;
    const _attached = []; // [{ prefix, loadNumber }]

    function imageUrl(loadNumber, direction) {
        // Append a cachebuster so a recapture (overwriting the same file)
        // doesn't show the stale browser-cached frame.
        return '/api/ticket/' + encodeURIComponent(loadNumber) +
               '/image?direction=' + encodeURIComponent(direction) +
               '&t=' + Date.now();
    }

    function setThumb(elId, has, loadNumber, direction, label) {
        const el = document.getElementById(elId);
        if (!el) return;
        // Remove the labelless placeholder/img child(ren) but keep the
        // <div class="gm-gd-thumb__label"> for visual continuity.
        Array.from(el.children).forEach(function (c) {
            if (c.classList.contains('gm-gd-thumb__placeholder') ||
                c.classList.contains('gm-gd-thumb__img-wrap')) {
                el.removeChild(c);
            }
        });

        const slot = document.createElement('div');
        if (has) {
            slot.className = 'gm-gd-thumb__img-wrap';
            slot.style.cursor = 'pointer';
            const img = document.createElement('img');
            img.src = imageUrl(loadNumber, direction);
            img.alt = label + ' photo';
            img.style.maxWidth  = '64px';
            img.style.maxHeight = '48px';
            img.style.objectFit = 'cover';
            img.style.borderRadius = '4px';
            slot.appendChild(img);
            slot.addEventListener('click', function () {
                openLightbox(loadNumber, direction, label);
            });
        } else {
            slot.innerHTML = PLACEHOLDER_HTML.replace(/^<div class="gm-gd-thumb__placeholder">|<\/div>$/g, '');
            slot.className = 'gm-gd-thumb__placeholder';
        }
        // Insert before the label so the label stays at the bottom
        const labelEl = el.querySelector('.gm-gd-thumb__label');
        if (labelEl) el.insertBefore(slot, labelEl); else el.appendChild(slot);
    }

    function openLightbox(loadNumber, direction, label) {
        let modal = document.getElementById('gmImageLightbox');
        if (!modal) {
            modal = document.createElement('div');
            modal.id = 'gmImageLightbox';
            modal.className = 'modal fade';
            modal.tabIndex = -1;
            modal.innerHTML =
                '<div class="modal-dialog modal-dialog-centered modal-lg">' +
                  '<div class="modal-content">' +
                    '<div class="modal-header">' +
                      '<h5 class="modal-title" id="gmImageLightboxTitle"></h5>' +
                      '<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>' +
                    '</div>' +
                    '<div class="modal-body text-center bg-dark p-1">' +
                      '<img id="gmImageLightboxImg" style="max-width:100%; max-height:75vh;" />' +
                    '</div>' +
                  '</div>' +
                '</div>';
            document.body.appendChild(modal);
        }
        document.getElementById('gmImageLightboxTitle').textContent =
            'Load ' + loadNumber + ' — ' + label;
        document.getElementById('gmImageLightboxImg').src = imageUrl(loadNumber, direction);
        if (window.bootstrap) new bootstrap.Modal(modal).show();
    }

    function refresh(prefix, loadNumber) {
        if (!loadNumber) return;
        fetch('/api/ticket/' + encodeURIComponent(loadNumber) + '/image-status',
              { cache: 'no-store' })
            .then(r => r.ok ? r.json() : { inImage: false, outImage: false, bolImage: false })
            .then(s => {
                setThumb(prefix + 'ThumbIn',  !!s.inImage,  loadNumber, DIRECTIONS.In,  'Inbound');
                setThumb(prefix + 'ThumbOut', !!s.outImage, loadNumber, DIRECTIONS.Out, 'Outbound');
                setThumb(prefix + 'ThumbBol', !!s.bolImage, loadNumber, DIRECTIONS.Bol, 'BOL');
            })
            .catch(() => { /* leave placeholders */ });
    }

    function ensureSignalR() {
        if (_conn || !window.signalR) return;
        _conn = new signalR.HubConnectionBuilder()
            .withUrl('/hubs/camera')
            .withAutomaticReconnect()
            .build();
        _conn.on('ImageCaptured', function (ticket, direction) {
            // Refresh every attached prefix that's watching this load.
            _attached.forEach(function (a) {
                if (String(a.loadNumber) === String(ticket)) {
                    refresh(a.prefix, a.loadNumber);
                }
            });
        });
        _conn.start().catch(function () { /* periodic refresh below still works */ });
    }

    window.gmLoadImages = {
        /** Hook the three thumbnails up to a load. Safe to call multiple times. */
        attach: function (opts) {
            if (!opts || !opts.prefix || !opts.loadNumber) return;
            // De-duplicate by (prefix, loadNumber)
            const ix = _attached.findIndex(a => a.prefix === opts.prefix);
            if (ix >= 0) _attached[ix].loadNumber = opts.loadNumber;
            else _attached.push({ prefix: opts.prefix, loadNumber: opts.loadNumber });

            ensureSignalR();
            refresh(opts.prefix, opts.loadNumber);
        },

        /** Reset the thumbnails to empty placeholders — useful when the form switches to a new (unsaved) load. */
        clear: function (prefix) {
            ['ThumbIn', 'ThumbOut', 'ThumbBol'].forEach(function (suffix) {
                const el = document.getElementById(prefix + suffix);
                if (!el) return;
                Array.from(el.children).forEach(function (c) {
                    if (c.classList.contains('gm-gd-thumb__placeholder') ||
                        c.classList.contains('gm-gd-thumb__img-wrap')) {
                        el.removeChild(c);
                    }
                });
                const slot = document.createElement('div');
                slot.className = 'gm-gd-thumb__placeholder';
                slot.innerHTML = '<i class="dx-icon dx-icon-photo" style="font-size:24px;opacity:.4;"></i>';
                const labelEl = el.querySelector('.gm-gd-thumb__label');
                if (labelEl) el.insertBefore(slot, labelEl); else el.appendChild(slot);
            });
            const ix = _attached.findIndex(a => a.prefix === prefix);
            if (ix >= 0) _attached.splice(ix, 1);
        },

        /** Force a refresh of one attached prefix — useful after the user just saved. */
        refresh: function (prefix) {
            const a = _attached.find(x => x.prefix === prefix);
            if (a) refresh(a.prefix, a.loadNumber);
        }
    };
})();
