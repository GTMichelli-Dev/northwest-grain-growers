(function () {
    'use strict';

    // ── Selectors ────────────────────────────────────────────────────────────

    const SEL = {
        form:           '#gmGdForm',
        alert:          '#gmGdAlert',
        submit:         '#gmGdSubmit',

        // Scale partial hidden inputs (owned by _ScaleWeight)
        scaleValue:     '#gwSwValue',

        // Weight sheet panels
        wsPanel:            '#gdWsPanel',
        wsList:             '#gdWsList',
        newWsBtn:           '#gdNewWsBtn',
        newWsPanel:         '#gdNewWsPanel',
        newWsError:         '#gdNewWsError',
        createWsBtn:        '#gdCreateWsBtn',
        cancelNewWsBtn:     '#gdCancelNewWsBtn',
        weightSheetUid:     '#gdWeightSheetUid',

        // New WS lot create form (inside _NewWeightSheet)
        nwsLotList:         '#gdNwsLotList',
        nwsAccount:         '#gdNwsAccount',
        nwsSplitGroup:      '#gdNwsSplitGroup',
        nwsLotDesc:         '#gdNwsLotDesc',
        nwsCreateError:     '#gdNwsCreateError',
        createWsLotBtn:     '#gdCreateWsLotBtn',
        nwsSelectedLotName: '#gdNwsSelectedLotName',

        // Gross
        captureGross:   '#gdCaptureGross',
        grossDisplay:   '#gdGrossDisplay',
        grossTime:      '#gdGrossTime',
        grossRow:       '#gdGrossRow',
        startQty:       '#gdStartQty',
        startedAt:      '#gdStartedAt',

        // Tare
        captureTare:    '#gdCaptureTare',
        tareDisplay:    '#gdTareDisplay',
        tareTime:       '#gdTareTime',
        tareRow:        '#gdTareRow',
        endQty:         '#gdEndQty',
        completedAt:    '#gdCompletedAt',

        // Net display
        netDisplay:     '#gdNetDisplay',

        // Hidden FK inputs
        accountId:      '#gdAccountId',
        productId:      '#gdProductId',
        lotId:          '#gdLotId',
        locationId:     '#gdLocationId',
        splitGroupId:   '#gdSplitGroupId',
        containerId:    '#gdContainerId',

        // New lot modal
        newLotBtn:      '#gdNewLotBtn',
        newLotModal:    '#gdNewLotModal',
        newLotProduct:  '#gdNewLotProductName',
        newLotDesc:     '#gdNewLotDesc',
        newLotError:    '#gdNewLotError',
        newLotSave:     '#gdNewLotSave',
    };

    // ── Constants ────────────────────────────────────────────────────────────

    const LOCATION_STORAGE_KEY = 'gm_location_id';

    // ── State ────────────────────────────────────────────────────────────────

    let grossCaptured         = false;
    let currentProductName    = null;
    let newLotModalInstance   = null;
    let currentWeightSheetUid = null;
    let selectedNwsLotId      = null;   // lot selected in the New WS panel
    let selectedNwsLotDesc    = null;

    // ── Init ─────────────────────────────────────────────────────────────────

    $(function () {
        initLocation();       // async — fetches eagerly, restores from localStorage
        initSelectBoxes();
        initNwsAccountPicker();
        initNwsSplitGroupPicker();
        wireWeightCapture();
        wireNewLotModal();
        wireWeightSheetPanel();
        wireSubmit();
    });

    // ── Location — eager load + localStorage persistence ─────────────────────

    async function initLocation() {
        // Restore previously selected location (survives browser close)
        const savedId = parseInt(localStorage.getItem(LOCATION_STORAGE_KEY) || '0', 10) || null;

        // Pre-fetch the list immediately so the dropdown is populated on first open
        let locations = [];
        try {
            locations = await $.getJSON('/api/locations/WarehouseLocationsList');
        } catch (ex) {
            console.warn('[GrowerDelivery] Location prefetch failed', ex);
        }

        $('#gdLocation').dxSelectBox({
            dataSource:    locations,
            valueExpr:     'LocationId',
            displayExpr:   function (item) { return item ? item.Name + ' \u2013 ' + item.LocationId : ''; },
            searchEnabled: true,
            placeholder:   'Select location…',
            width:         'auto',
            value:         savedId,
            onValueChanged: function (e) {
                const val = e.value ?? '';
                $(SEL.locationId).val(val);
                $('#gdFormBody').prop('hidden', !val);
                $(SEL.wsPanel).prop('hidden', !val);
                $(SEL.newWsPanel).prop('hidden', true);
                if (val) {
                    localStorage.setItem(LOCATION_STORAGE_KEY, String(val));
                    refreshWeightSheets(val);
                } else {
                    localStorage.removeItem(LOCATION_STORAGE_KEY);
                    selectWeightSheet(null);
                }
            }
        });

        // Sync hidden input and reveal panels if a location was restored
        if (savedId) {
            $(SEL.locationId).val(savedId);
            $('#gdFormBody').prop('hidden', false);
            $(SEL.wsPanel).prop('hidden', false);
            refreshWeightSheets(savedId);
        }
    }

    // ── SelectBox initialization ─────────────────────────────────────────────

    function initSelectBoxes() {
        // ── Account (Grower) ──────────────────────────────────────────────────
        $('#gdAccount').dxSelectBox({
            dataSource: new DevExpress.data.DataSource({
                store: new DevExpress.data.CustomStore({
                    key:  'AccountId',
                    load: () => $.getJSON('/api/Lookups/Accounts')
                })
            }),
            valueExpr:    'AccountId',
            displayExpr:  'Name',
            searchEnabled: true,
            placeholder:  'Select grower…',
            onValueChanged: function (e) {
                $(SEL.accountId).val(e.value ?? '');
            }
        });

        // ── Product ───────────────────────────────────────────────────────────
        $('#gdProduct').dxSelectBox({
            dataSource: new DevExpress.data.DataSource({
                store: new DevExpress.data.CustomStore({
                    key:  'ProductId',
                    load: () => $.getJSON('/api/Lookups/WarehouseItems')
                })
            }),
            valueExpr:    'ItemId',
            displayExpr:  'Name',
            searchEnabled: true,
            placeholder:  'Select Item…',
            onValueChanged: function (e) {
                $(SEL.productId).val(e.value ?? '');
                currentProductName = e.component.option('text') || null;
                refreshLots();
                // Enable + New Lot button only after a product is selected
                $(SEL.newLotBtn)
                    .prop('disabled', !e.value)
                    .attr('title', e.value ? 'Create a new lot for this product' : 'Select a product first');
            }
        });

        // ── Lot ───────────────────────────────────────────────────────────────
        $('#gdLot').dxSelectBox({
            dataSource: new DevExpress.data.DataSource({
                store: new DevExpress.data.CustomStore({
                    key:  'Id',
                    load: function () {
                        const productId = $(SEL.productId).val();
                        const qs = productId ? '?productId=' + productId : '';
                        return $.getJSON('/api/Lookups/Lots' + qs);
                    }
                })
            }),
            valueExpr:    'Id',
            displayExpr:  'LotNumber',
            searchEnabled: true,
            placeholder:  'Select lot…',
            onValueChanged: function (e) {
                $(SEL.lotId).val(e.value ?? '');
            }
        });

        // ── Split Group (optional) ────────────────────────────────────────────
        $('#gdSplitGroup').dxSelectBox({
            dataSource: new DevExpress.data.DataSource({
                store: new DevExpress.data.CustomStore({
                    key:  'SplitGroupId',
                    load: () => $.getJSON('/api/Lookups/SplitGroups')
                })
            }),
            valueExpr:    'SplitGroupId',
            displayExpr:  'Name',
            searchEnabled: true,
            showClearButton: true,
            placeholder:  'None',
            onValueChanged: function (e) {
                $(SEL.splitGroupId).val(e.value ?? '');
            }
        });

        // ── Destination Container (optional) ──────────────────────────────────
        $('#gdContainer').dxSelectBox({
            dataSource: new DevExpress.data.DataSource({
                store: new DevExpress.data.CustomStore({
                    key:  'Id',
                    load: () => $.getJSON('/api/Lookups/Containers')
                })
            }),
            valueExpr:    'Id',
            displayExpr:  'Name',
            searchEnabled: true,
            showClearButton: true,
            placeholder:  'None',
            onValueChanged: function (e) {
                $(SEL.containerId).val(e.value ?? '');
            }
        });
    }

    function refreshLots() {
        const instance = $('#gdLot').dxSelectBox('instance');
        if (!instance) return;
        instance.reset();
        $(SEL.lotId).val('');
        instance.getDataSource().reload();
    }

    // ── New Lot Modal ─────────────────────────────────────────────────────────

    function wireNewLotModal() {
        // Bootstrap modal instance (lazily created)
        function getModal() {
            if (!newLotModalInstance) {
                const el = document.querySelector(SEL.newLotModal);
                if (el) newLotModalInstance = new bootstrap.Modal(el);
            }
            return newLotModalInstance;
        }

        // Open modal — pre-fill the product name
        $(SEL.newLotBtn).on('click', function () {
            $(SEL.newLotProduct).text(currentProductName || '—');
            $(SEL.newLotDesc).val('');
            $(SEL.newLotError).prop('hidden', true).text('');
            getModal()?.show();
        });

        // Save new lot
        $(SEL.newLotSave).on('click', async function () {
            const productId = parseInt($(SEL.productId).val(), 10);
            const desc      = $(SEL.newLotDesc).val().trim();
            const splitId   = parseInt($(SEL.splitGroupId).val(), 10) || null;

            if (!desc) {
                $(SEL.newLotError).text('Lot description is required.').prop('hidden', false);
                return;
            }

            const btn = $(SEL.newLotSave);
            btn.prop('disabled', true).text('Creating…');
            $(SEL.newLotError).prop('hidden', true);

            try {
                const resp = await fetch('/api/Lookups/Lots', {
                    method:  'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body:    JSON.stringify({
                        ProductId:      productId,
                        LotDescription: desc,
                        SplitGroupId:   splitId,
                    })
                });

                if (!resp.ok) {
                    const detail = await tryParseError(resp);
                    $(SEL.newLotError).text('Error: ' + detail).prop('hidden', false);
                    return;
                }

                const created = await resp.json();   // { id, lotNumber }

                // Inject the new lot into the SelectBox and select it
                const lotInstance = $('#gdLot').dxSelectBox('instance');
                await lotInstance.getDataSource().reload();
                lotInstance.option('value', created.id);
                $(SEL.lotId).val(created.id);

                getModal()?.hide();

            } catch (ex) {
                $(SEL.newLotError).text('Network error: ' + ex.message).prop('hidden', false);
            } finally {
                btn.prop('disabled', false).text('Create Lot');
            }
        });
    }

    // ── Weight capture ───────────────────────────────────────────────────────

    function wireWeightCapture() {
        $(SEL.captureGross).on('click', function () {
            const w = readScaleWeight();
            if (w === null) return;

            const now = new Date();
            $(SEL.startQty).val(w);
            $(SEL.startedAt).val(now.toISOString());
            $(SEL.grossDisplay).text(fmtWeight(w));
            $(SEL.grossTime).text(fmtTime(now));
            $(SEL.grossRow).addClass('gm-gd-weight__row--captured');

            grossCaptured = true;
            $(SEL.captureTare).prop('disabled', false);

            updateNet();
        });

        $(SEL.captureTare).on('click', function () {
            const w = readScaleWeight();
            if (w === null) return;

            const now = new Date();
            $(SEL.endQty).val(w);
            $(SEL.completedAt).val(now.toISOString());
            $(SEL.tareDisplay).text(fmtWeight(w));
            $(SEL.tareTime).text(fmtTime(now));
            $(SEL.tareRow).addClass('gm-gd-weight__row--captured');

            updateNet();
        });
    }

    function readScaleWeight() {
        const raw = $(SEL.scaleValue).val();
        const w   = parseInt(raw, 10);
        if (!raw || isNaN(w) || w <= 0) {
            showAlert('No stable scale weight available. Select a scale and wait for a stable reading, or use Manual entry.', 'danger');
            return null;
        }
        hideAlert();
        return w;
    }

    function updateNet() {
        const gross = parseInt($(SEL.startQty).val(), 10);
        const tare  = parseInt($(SEL.endQty).val(), 10);

        if (!isNaN(gross) && !isNaN(tare)) {
            $(SEL.netDisplay).text(fmtWeight(Math.abs(gross - tare)));
        } else if (!isNaN(gross)) {
            $(SEL.netDisplay).text('Awaiting tare…');
        }
    }

    // ── Form submission ──────────────────────────────────────────────────────

    function wireSubmit() {
        $(SEL.form).on('submit', async function (e) {
            e.preventDefault();
            hideAlert();

            const payload = buildPayload();
            const err = validate(payload);
            if (err) {
                showAlert(err, 'danger');
                return;
            }

            const btn = $(SEL.submit);
            btn.prop('disabled', true).text('Saving…');

            try {
                const resp = await fetch('/api/GrowerDelivery', {
                    method:  'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body:    JSON.stringify(payload)
                });

                if (resp.ok) {
                    showAlert('Delivery saved successfully.', 'success');
                    // TODO: optionally redirect or reset form for next delivery
                } else {
                    const detail = await tryParseError(resp);
                    showAlert('Save failed: ' + detail, 'danger');
                }
            } catch (ex) {
                showAlert('Network error: ' + ex.message, 'danger');
            } finally {
                btn.prop('disabled', false).text('Save Delivery');
            }
        });
    }

    function buildPayload() {
        function numOrNull(id) {
            const v = parseFloat($('#' + id).val());
            return isNaN(v) ? null : v;
        }
        function strOrNull(id) {
            const v = $('#' + id).val()?.trim();
            return v || null;
        }

        return {
            LotId:          intOrNull(SEL.lotId),
            ProductId:      intOrNull(SEL.productId),
            AccountId:      intOrNull(SEL.accountId),
            LocationId:     intOrNull(SEL.locationId),
            SplitGroupId:   intOrNull(SEL.splitGroupId) || null,
            ToContainerId:  intOrNull(SEL.containerId)  || null,
            WeightSheetUid: $(SEL.weightSheetUid).val() || null,
            StartQty:      numOrNull('gdStartQty')     || null,
            EndQty:        numOrNull('gdEndQty')        || null,
            StartedAt:     strOrNull('gdStartedAt')    || null,
            CompletedAt:   strOrNull('gdCompletedAt')  || null,
            Notes:         strOrNull('gdNotes')        || null,

            // Grain quality
            Moisture:      numOrNull('gdMoisture'),
            Protein:       numOrNull('gdProtein'),
            TestWeight:    numOrNull('gdTestWeight'),
            Dockage:       numOrNull('gdDockage'),
            Grade:         strOrNull('gdGrade'),
            ForeignMatter: numOrNull('gdForeignMatter'),
            Splits:        numOrNull('gdSplits'),
            Damaged:       numOrNull('gdDamaged'),
            Oil:           numOrNull('gdOil'),
            Starch:        numOrNull('gdStarch'),
        };
    }

    function intOrNull(selector) {
        const v = parseInt($(selector).val(), 10);
        return isNaN(v) ? null : v;
    }

    function validate(p) {
        if (!p.AccountId)  return 'Grower Account is required.';
        if (!p.ProductId)  return 'Product is required.';
        if (!p.LotId)      return 'Lot is required.';
        if (!p.LocationId) return 'Location is required.';
        if (!p.StartQty)   return 'Gross weight must be captured before saving.';
        if (!p.EndQty)     return 'Tare weight must be captured before saving.';
        return null;
    }

    async function tryParseError(resp) {
        try {
            const j = await resp.json();
            return j.message || j.title || resp.statusText;
        } catch {
            return resp.statusText;
        }
    }

    // ── Alert helpers ────────────────────────────────────────────────────────

    function showAlert(msg, type) {
        const el = $(SEL.alert);
        el.removeClass('alert-success alert-danger')
          .addClass('alert alert-' + type)
          .text(msg)
          .prop('hidden', false);
        el[0].scrollIntoView({ behavior: 'smooth', block: 'nearest' });
    }

    function hideAlert() {
        $(SEL.alert).prop('hidden', true).text('');
    }

    // ── Formatters ───────────────────────────────────────────────────────────

    function fmtWeight(w) {
        return Number(w).toLocaleString(undefined, { maximumFractionDigits: 0 }) + ' lbs';
    }

    function fmtTime(date) {
        return date.toLocaleTimeString(undefined, { hour: '2-digit', minute: '2-digit', second: '2-digit' });
    }

    // ── Weight sheet panel ───────────────────────────────────────────────────

    async function refreshWeightSheets(locationId) {
        $(SEL.wsList).html('<span class="text-muted small fst-italic">Loading…</span>');
        selectWeightSheet(null);

        let sheets = [];
        try {
            sheets = await $.getJSON('/api/GrowerDelivery/OpenWeightSheets?locationId=' + locationId);
        } catch (ex) {
            $(SEL.wsList).html('<span class="text-danger small">Failed to load weight sheets.</span>');
            return;
        }

        renderWeightSheets(sheets);
    }

    function renderWeightSheets(sheets) {
        const list = $(SEL.wsList);

        if (!sheets.length) {
            list.html(
                '<div class="gm-gd-ws-empty">' +
                    '<span class="text-muted small">No open weight sheets for this location.</span>' +
                    ' <span class="text-muted small">Create one to associate loads.</span>' +
                '</div>'
            );
            return;
        }

        const rows = sheets.map(function (ws) {
            const lot    = ws.LotDescription ? escapeHtml(ws.LotDescription) : '<span class="text-muted">—</span>';
            const status = ws.Status         ? escapeHtml(ws.Status)         : '';
            const isSelected = currentWeightSheetUid === ws.WeightSheetUid;

            return (
                '<div class="gm-gd-ws-row' + (isSelected ? ' gm-gd-ws-row--selected' : '') + '" ' +
                      'data-uid="' + escapeAttr(ws.WeightSheetUid) + '">' +
                    '<div class="gm-gd-ws-row__id">#' + ws.WeightSheetId + '</div>' +
                    '<div class="gm-gd-ws-row__date">' + escapeHtml(ws.CreationDate) + '</div>' +
                    '<div class="gm-gd-ws-row__lot">' + lot + '</div>' +
                    '<div class="gm-gd-ws-row__status">' + status + '</div>' +
                    '<button type="button" class="btn btn-sm ' +
                        (isSelected ? 'btn-primary gm-gd-ws-row__deselect' : 'btn-outline-primary gm-gd-ws-row__select') +
                        '">' +
                        (isSelected ? 'Selected ✓' : 'Select') +
                    '</button>' +
                '</div>'
            );
        }).join('');

        list.html(rows);

        // Wire row select/deselect buttons
        list.find('.gm-gd-ws-row__select').on('click', function () {
            const uid = $(this).closest('.gm-gd-ws-row').data('uid');
            selectWeightSheet(uid);
            // Re-render to reflect new selection
            const locationId = $(SEL.locationId).val();
            if (locationId) refreshWeightSheets(locationId);
        });

        list.find('.gm-gd-ws-row__deselect').on('click', function () {
            selectWeightSheet(null);
            const locationId = $(SEL.locationId).val();
            if (locationId) refreshWeightSheets(locationId);
        });
    }

    function selectWeightSheet(uid) {
        currentWeightSheetUid = uid || null;
        $(SEL.weightSheetUid).val(uid || '');
    }

    function wireWeightSheetPanel() {

        // ── Open the New WS panel ─────────────────────────────────────────────
        $(SEL.newWsBtn).on('click', function () {
            $(SEL.wsPanel).prop('hidden', true);
            $(SEL.newWsPanel).prop('hidden', false);
            setNwsLot(null, null);
            resetNwsCreateForm();
            const locationId = $(SEL.locationId).val();
            if (locationId) refreshNwsLots(locationId);
        });

        // ── Back to weight sheet list ─────────────────────────────────────────
        $(SEL.cancelNewWsBtn).on('click', function () {
            $(SEL.newWsPanel).prop('hidden', true);
            $(SEL.wsPanel).prop('hidden', false);
        });

        // ── Create weight sheet for selected lot ──────────────────────────────
        $(SEL.createWsBtn).on('click', async function () {
            const locationId = parseInt($(SEL.locationId).val(), 10);

            if (!locationId) {
                $(SEL.newWsError).text('No location selected.').prop('hidden', false);
                return;
            }
            if (!selectedNwsLotId) {
                $(SEL.newWsError).text('Please select a weight sheet lot above.').prop('hidden', false);
                return;
            }

            const btn = $(SEL.createWsBtn);
            btn.prop('disabled', true).text('Creating…');
            $(SEL.newWsError).prop('hidden', true);

            try {
                const resp = await fetch('/api/GrowerDelivery/WeightSheets', {
                    method:  'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body:    JSON.stringify({ LocationId: locationId, LotId: selectedNwsLotId }),
                });

                if (!resp.ok) {
                    const detail = await tryParseError(resp);
                    $(SEL.newWsError).text('Error: ' + detail).prop('hidden', false);
                    return;
                }

                const created = await resp.json();
                selectWeightSheet(created.WeightSheetUid);

                $(SEL.newWsPanel).prop('hidden', true);
                $(SEL.wsPanel).prop('hidden', false);
                refreshWeightSheets(locationId);

            } catch (ex) {
                $(SEL.newWsError).text('Network error: ' + ex.message).prop('hidden', false);
            } finally {
                btn.prop('disabled', false).text('Create Weight Sheet');
            }
        });

        // ── Create new weight sheet lot ───────────────────────────────────────
        $(SEL.createWsLotBtn).on('click', async function () {
            const locationId  = parseInt($(SEL.locationId).val(), 10);
            const splitGroupId = parseInt(
                $(SEL.nwsSplitGroup).dxSelectBox('option', 'value'), 10) || null;

            if (!locationId) {
                $(SEL.nwsCreateError).text('No location selected.').prop('hidden', false);
                return;
            }
            if (!splitGroupId) {
                $(SEL.nwsCreateError).text('Please select a split group.').prop('hidden', false);
                return;
            }

            const btn = $(SEL.createWsLotBtn);
            btn.prop('disabled', true).text('Creating…');
            $(SEL.nwsCreateError).prop('hidden', true);

            try {
                const resp = await fetch('/api/GrowerDelivery/WeightSheetLots', {
                    method:  'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body:    JSON.stringify({ LocationId: locationId, SplitGroupId: splitGroupId }),
                });

                if (!resp.ok) {
                    const detail = await tryParseError(resp);
                    $(SEL.nwsCreateError).text('Error: ' + detail).prop('hidden', false);
                    return;
                }

                const created = await resp.json();   // { Id, LotDescription }

                // Auto-select the new lot and refresh the list
                setNwsLot(created.Id, created.LotDescription);
                resetNwsCreateForm();
                refreshNwsLots(locationId);

            } catch (ex) {
                $(SEL.nwsCreateError).text('Network error: ' + ex.message).prop('hidden', false);
            } finally {
                btn.prop('disabled', false).text('+ Create Lot');
            }
        });
    }

    // ── NWS lot list ─────────────────────────────────────────────────────────

    async function refreshNwsLots(locationId) {
        $(SEL.nwsLotList).html('<span class="text-muted small fst-italic">Loading…</span>');

        let lots = [];
        try {
            lots = await $.getJSON('/api/GrowerDelivery/OpenLots?locationId=' + locationId);
        } catch (ex) {
            $(SEL.nwsLotList).html('<span class="text-danger small">Failed to load lots.</span>');
            return;
        }

        renderNwsLots(lots);
    }

    function renderNwsLots(lots) {
        const list = $(SEL.nwsLotList);

        if (!lots.length) {
            list.html(
                '<div class="gm-gd-ws-empty">' +
                    '<span class="text-muted small">No weight sheet lots found at this location.</span>' +
                    ' <span class="text-muted small">Create one below.</span>' +
                '</div>'
            );
            return;
        }

        const rows = lots.map(function (lot) {
            const desc  = lot.LotDescription        ? escapeHtml(lot.LotDescription)        : '—';
            const sg    = lot.SplitGroupDescription ? escapeHtml(lot.SplitGroupDescription) : '';
            const isSel = selectedNwsLotId === lot.Id;

            return (
                '<div class="gm-gd-ws-row' + (isSel ? ' gm-gd-ws-row--selected' : '') + '" ' +
                      'data-lot-id="' + lot.Id + '" data-lot-desc="' + escapeAttr(lot.LotDescription || '') + '">' +
                    '<div class="gm-gd-ws-row__id">#' + lot.Id + '</div>' +
                    '<div class="gm-gd-ws-row__lot">' + desc + '</div>' +
                    '<div class="gm-gd-ws-row__status">' + sg + '</div>' +
                    '<button type="button" class="btn btn-sm ' +
                        (isSel ? 'btn-primary gm-gd-nws__deselect' : 'btn-outline-primary gm-gd-nws__select') +
                        '">' +
                        (isSel ? 'Selected ✓' : 'Use This Lot') +
                    '</button>' +
                '</div>'
            );
        }).join('');

        list.html(rows);

        list.find('.gm-gd-nws__select').on('click', function () {
            const row  = $(this).closest('.gm-gd-ws-row');
            setNwsLot(parseInt(row.data('lot-id'), 10), row.data('lot-desc'));
            const locationId = $(SEL.locationId).val();
            if (locationId) refreshNwsLots(locationId);
        });

        list.find('.gm-gd-nws__deselect').on('click', function () {
            setNwsLot(null, null);
            const locationId = $(SEL.locationId).val();
            if (locationId) refreshNwsLots(locationId);
        });
    }

    function setNwsLot(id, desc) {
        selectedNwsLotId   = id   || null;
        selectedNwsLotDesc = desc || null;
        const hasLot = !!selectedNwsLotId;
        $(SEL.createWsBtn).prop('disabled', !hasLot);
        $(SEL.nwsSelectedLotName).text(hasLot ? 'Lot: ' + selectedNwsLotDesc : '');
    }

    // ── NWS create-lot form helpers ───────────────────────────────────────────

    function initNwsAccountPicker() {
        $(SEL.nwsAccount).dxSelectBox({
            dataSource: new DevExpress.data.DataSource({
                store: new DevExpress.data.CustomStore({
                    key:  'AccountId',
                    load: () => $.getJSON('/api/Lookups/Accounts'),
                })
            }),
            valueExpr:    'AccountId',
            displayExpr:  'Name',
            searchEnabled: true,
            placeholder:  'Select account…',
            showClearButton: true,
            onValueChanged: function (e) {
                // Reset split group when account changes
                const sgInst = $(SEL.nwsSplitGroup).dxSelectBox('instance');
                if (sgInst) {
                    sgInst.reset();
                    sgInst.getDataSource().reload();
                }
                $(SEL.nwsLotDesc).val('');
            }
        });
    }

    function initNwsSplitGroupPicker() {
        $(SEL.nwsSplitGroup).dxSelectBox({
            dataSource: new DevExpress.data.DataSource({
                store: new DevExpress.data.CustomStore({
                    key:  'SplitGroupId',
                    load: function () {
                        const accountId = $(SEL.nwsAccount).dxSelectBox('option', 'value');
                        if (!accountId) return Promise.resolve([]);
                        return $.getJSON('/api/GrowerDelivery/SplitGroupsByAccount?accountId=' + accountId);
                    }
                })
            }),
            valueExpr:    'SplitGroupId',
            displayExpr:  function (item) {
                return item ? item.SplitGroupId + ' - ' + item.SplitGroupDescription : '';
            },
            searchEnabled: true,
            placeholder:  'Select split group…',
            showClearButton: true,
            onValueChanged: function (e) {
                // Auto-fill lot description from the selected split group's description
                const desc = e.component.option('text') || '';
                $(SEL.nwsLotDesc).val(desc);
            }
        });
    }

    function resetNwsCreateForm() {
        const acctInst = $(SEL.nwsAccount).dxSelectBox('instance');
        const sgInst   = $(SEL.nwsSplitGroup).dxSelectBox('instance');
        if (acctInst) acctInst.reset();
        if (sgInst)   sgInst.reset();
        $(SEL.nwsLotDesc).val('');
        $(SEL.nwsCreateError).prop('hidden', true).text('');
    }

    // ── Utility ──────────────────────────────────────────────────────────────

    function escapeHtml(str) {
        return String(str)
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;');
    }

    function escapeAttr(str) {
        return String(str).replace(/"/g, '&quot;');
    }

})();
