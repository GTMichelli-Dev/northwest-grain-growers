/* Kiosk service worker.
 *
 *  Goal: when the kiosk PC reboots while the website is offline, the
 *  browser still loads the /Kiosk page from cache instead of the
 *  built-in "Hmmm... can't reach this page" error. Once the page is
 *  alive, the in-page SignalR retry loop + comm-error overlay take it
 *  from there until the server comes back.
 *
 *  Strategy:
 *    - On install, precache the kiosk shell (HTML, the SignalR client
 *      bundle, the brand logo).
 *    - For navigation to /Kiosk: network-first with cache fallback so
 *      online reboots still pick up the latest copy.
 *    - For static assets under /lib/ or /assets/: cache-first.
 *    - For /api/ or /hubs/ routes: never cache and never intercept —
 *      the kiosk MUST see honest network failures so its comm-error
 *      logic can do its job.
 */

const CACHE = 'kiosk-shell-v2';
const SHELL = [
    '/Kiosk',
    '/lib/signalr/signalr.min.js',
    // Brand logo paths — best-effort. If a deployment uses a different
    // theme the matching path will populate on first online load via the
    // static-asset cache-first handler below.
    '/assets/branding/nwgg/logoBW.jpg',
    '/assets/branding/default/logo.png',
];

self.addEventListener('install', (event) => {
    event.waitUntil(
        caches.open(CACHE)
            // addAll fails atomically — addAll is the safe choice but we
            // tolerate per-asset failures so a missing optional logo
            // doesn't sink the whole install.
            .then((c) => Promise.all(
                SHELL.map((url) =>
                    c.add(url).catch(() => null)
                )
            ))
            .then(() => self.skipWaiting())
    );
});

self.addEventListener('activate', (event) => {
    event.waitUntil(
        caches.keys()
            .then((keys) => Promise.all(
                keys.filter((k) => k !== CACHE).map((k) => caches.delete(k))
            ))
            .then(() => self.clients.claim())
    );
});

self.addEventListener('fetch', (event) => {
    const req = event.request;
    if (req.method !== 'GET') return;

    let url;
    try { url = new URL(req.url); }
    catch (e) { return; }

    // Same-origin only — never intercept anything else.
    if (url.origin !== self.location.origin) return;

    // Live calls — must fail honestly so the kiosk's comm-error overlay
    // can react. We don't even register a fetch handler for these.
    if (url.pathname.startsWith('/api/')
        || url.pathname.startsWith('/hubs/')) {
        return;
    }

    // Navigation to /Kiosk: network-first, cache fallback.
    const isKioskNav =
        (req.mode === 'navigate'
         || (req.headers.get('Accept') || '').includes('text/html'))
        && /^\/kiosk\/?$/i.test(url.pathname);

    if (isKioskNav) {
        event.respondWith((async () => {
            try {
                const fresh = await fetch(req);
                // Cache the fresh copy for the next reboot.
                const copy = fresh.clone();
                caches.open(CACHE).then((c) => c.put('/Kiosk', copy)).catch(() => {});
                return fresh;
            } catch (err) {
                const cached = await caches.match('/Kiosk');
                if (cached) return cached;
                throw err;
            }
        })());
        return;
    }

    // Static assets: cache-first, populate on first miss.
    if (url.pathname.startsWith('/lib/')
        || url.pathname.startsWith('/assets/')) {
        event.respondWith((async () => {
            const cached = await caches.match(req);
            if (cached) return cached;
            try {
                const fresh = await fetch(req);
                if (fresh.ok) {
                    const copy = fresh.clone();
                    caches.open(CACHE).then((c) => c.put(req, copy)).catch(() => {});
                }
                return fresh;
            } catch (err) {
                // No cached copy and network is down — let it 404.
                return new Response('', { status: 504, statusText: 'Offline' });
            }
        })());
    }

    // Everything else: default browser behaviour, no caching.
});
