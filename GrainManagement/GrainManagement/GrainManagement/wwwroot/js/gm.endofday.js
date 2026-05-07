// End-Of-Day audit driver — shared across the app. Walks every active
// warehouse location, calling /api/GrowerDelivery/EndOfDayCheck per-location
// and showing each result in the #wdEndOfDayModal partial. The Confirm &
// Continue button advances; on the last location it becomes "Done" and
// dismisses the modal.
//
// The modal instance is created once and reused for every step — never
// disposed/reshown — so Bootstrap can't leave a stranded .modal-backdrop.
//
// Public API: window.GM.openEndOfDayAudit()
(function () {
    "use strict";

    var _modal = null;
    var _locations = []; // [{ LocationId, Name, District }, ...]
    var _index = 0;
    var _wired = false;

    function endOfDayLink(row) {
        var basePath = (row.WeightSheetType || "").toLowerCase() === "transfer"
            ? "/GrowerDelivery/WeightSheetTransferLoads"
            : "/GrowerDelivery/WeightSheetDeliveryLoads";
        return basePath + "?wsId=" + row.WeightSheetId;
    }

    function ensureWiring() {
        if (_wired) return;
        var modalEl = document.getElementById("wdEndOfDayModal");
        if (!modalEl) return; // partial not on this page (module disabled)
        _wired = true;
        _modal = new bootstrap.Modal(modalEl);
        $("#wdEndOfDayContinue").on("click", function () {
            _index++;
            if (_index >= _locations.length) {
                _modal.hide();
                return;
            }
            loadStep();
        });
    }

    function renderGrid(rows) {
        var $grid = $("#wdEndOfDayGrid");
        var $msg  = $("#wdEndOfDayMsg");

        if (!rows.length) {
            $msg.removeClass("alert-warning alert-danger")
                .addClass("alert-success")
                .text("All open weight sheets are complete and have moisture entered. Ready for End Of Day.")
                .prop("hidden", false);
        } else {
            $msg.removeClass("alert-success alert-danger")
                .addClass("alert-warning")
                .text("The following weight sheets need attention before End Of Day can be completed.")
                .prop("hidden", false);
        }

        var existing;
        try { existing = $grid.dxDataGrid("instance"); } catch (e) { existing = null; }

        var options = {
            dataSource: rows,
            keyExpr: "WeightSheetId",
            showBorders: true,
            columnAutoWidth: true,
            paging: { enabled: false },
            columns: [
                { dataField: "WeightSheetIdDisplay", caption: "WS #" },
                { dataField: "Status",               caption: "Status",      width: 220 },
                { dataField: "WeightSheetType",      caption: "Type",        width: 100 },
                { dataField: "LotIdDisplay",         caption: "Lot #" },
                { dataField: "LotDescription",       caption: "Lot" },
                { dataField: "TotalLoads",           caption: "Loads",       width: 80,  alignment: "right" },
                { dataField: "IncompleteLoadCount",  caption: "Incomplete",  width: 110, alignment: "right" },
                { dataField: "MissingMoistureCount", caption: "No Moisture", width: 120, alignment: "right" },
            ],
            onRowClick: function (e) {
                if (e.rowType !== "data" || !e.data) return;
                window.location.href = endOfDayLink(e.data);
            },
            onRowPrepared: function (e) {
                if (e.rowType !== "data" || !e.data) return;
                var status = e.data.Status || "";
                if (status.indexOf("Not Complete") >= 0 && status.indexOf("No Moisture") >= 0) {
                    $(e.rowElement).css("background-color", "rgba(220, 53, 69, 0.18)");
                } else if (status === "Not Complete") {
                    $(e.rowElement).css("background-color", "rgba(255, 193, 7, 0.22)");
                } else if (status === "No Moisture Set") {
                    $(e.rowElement).css("background-color", "rgba(13, 110, 253, 0.14)");
                }
                $(e.rowElement).css("cursor", "pointer");
            }
        };
        if (existing) existing.option(options);
        else          $grid.dxDataGrid(options);
    }

    function loadStep() {
        var loc = _locations[_index];
        var total = _locations.length;
        var isLast = _index >= total - 1;

        $("#wdEndOfDayLocation").text(
            loc.Name + (loc.District ? " (" + loc.District + ")" : "")
        );
        $("#wdEndOfDayProgress").text("Location " + (_index + 1) + " of " + total);
        $("#wdEndOfDayMsg").prop("hidden", true);
        $("#wdEndOfDayContinue").prop("disabled", true).text(isLast ? "Done" : "Confirm & Continue");
        renderGrid([]);

        $.getJSON("/api/GrowerDelivery/EndOfDayCheck?locationId=" + encodeURIComponent(loc.LocationId))
            .done(function (data) {
                renderGrid(data || []);
            })
            .fail(function () {
                renderGrid([]);
                $("#wdEndOfDayMsg")
                    .removeClass("alert-warning alert-success")
                    .addClass("alert-danger")
                    .text("Failed to load End Of Day audit for this location.")
                    .prop("hidden", false);
            })
            .always(function () {
                $("#wdEndOfDayContinue").prop("disabled", false);
            });
    }

    function open() {
        ensureWiring();
        if (!_modal) return; // modal partial not present on this deployment

        _index = 0;
        _locations = [];
        $("#wdEndOfDayLocation").text("Loading locations…");
        $("#wdEndOfDayProgress").text("");
        $("#wdEndOfDayMsg").prop("hidden", true);
        $("#wdEndOfDayContinue").prop("disabled", true).text("Confirm & Continue");
        renderGrid([]);
        _modal.show();

        $.getJSON("/api/locations/WarehouseLocationsList")
            .done(function (locs) {
                _locations = (locs || []).map(function (l) {
                    return {
                        LocationId: l.LocationId || l.locationId,
                        Name: l.Name || l.name,
                        District: l.District || l.district || "",
                    };
                }).filter(function (l) { return l.LocationId > 0; });

                if (!_locations.length) {
                    $("#wdEndOfDayLocation").text("No warehouse locations configured.");
                    $("#wdEndOfDayContinue").text("Done").prop("disabled", false);
                    return;
                }
                loadStep();
            })
            .fail(function () {
                $("#wdEndOfDayLocation").text("");
                $("#wdEndOfDayMsg")
                    .removeClass("alert-warning alert-success")
                    .addClass("alert-danger")
                    .text("Failed to load location list. Please try again.")
                    .prop("hidden", false);
                $("#wdEndOfDayContinue").text("Close").prop("disabled", false);
            });
    }

    window.GM = window.GM || {};
    window.GM.openEndOfDayAudit = open;
})();
