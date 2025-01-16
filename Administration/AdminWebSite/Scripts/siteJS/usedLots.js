

var reportHub = $.connection.reportHub;

$(document).ready(function () {
    $('#tbl').bootstrapTable();
  


    $.connection.hub.disconnected(function () {
        setTimeout(function () {
            $.connection.hub.start();
        }, 5000); // Restart connection after 5 seconds.
    });

    $.connection.hub.start().done(function () {
     $('#ddLocations').val((sessionStorage.getItem('location') || null) || "");
    



        updateData();
  
    });
  
});


function formattedDate(date) {

    var dd = String(date.getDate()).padStart(2, '0');
    var mm = String(date.getMonth() + 1).padStart(2, '0'); // January is 0!
    var yy = String(date.getFullYear()).substring(2, 4);
    var hours = date.getHours();
    var minutes = String(date.getMinutes()).padStart(2, '0');
    var ampm = hours >= 12 ? 'pm' : 'am';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    var formattedDate = mm + '/' + dd + '/' + yy + ' ' + hours + ':' + minutes + ' ' + ampm;
    return formattedDate;
}



reportHub.client.updateUsedLots = function(data) {
    $('#spinner').hide();
    console.log(data);

    $('#lastUpdate').text(formattedDate(new Date( data.UpdateTime)));
    $('#tbl').bootstrapTable('refreshOptions', {
        data: data.UsedLots
  });


}


function updateData() {
    $('#spinner').show();

    var locationId = $('#ddLocations').val();
    reportHub.server.getUsedLots((locationId == '') ? null : locationId);
}





function lotLinkFormatter(value, row, index) {
    return `<a href="javascript:void(0);" onclick="showLotTickets('${row.Lot}')">${value}</a>`;
}

function showLotTickets(lot) {
    $('#spinner').show();
    reportHub.server.getTicketsForLot(lot);
}


reportHub.client.updateTicketsForLot = function (data) {
    $('#spinner').hide();
    var tableHtml = `
        <table id="lotTicketsTable"  style="font-size: 0.8em"  class="table table-striped
           data-toggle="lotTicketsTable"
           data-filter-control="true"
           data-show-search-clear-button="false"
           data-show-button-text="false"
           data-show-columns="false"
           data-show-pagination-switch="false"
           data-pagination="false"
           data-id-field="Description"
           data-show-footer="true"
           data-page-list="[10, 25, 50, 100, all]"
           data-mobile-responsive="true"
           data-row-style="rowStyle"
           data-check-on-init="true">
            <thead>
                <tr>
                 <th data-sortable="true" class="text-center"  data-field="Location">Location</th>

                   <th data-sortable="true" class="text-left" data-formatter="ticketLinkFormatter" data-field="Ticket">Ticket</th>
                    <th data-sortable="true" class="text-left" data-formatter="dateFormatter" data-field="TicketDate">Date</th>
                    <th data-sortable="true" class="text-center"  data-field="Variety">Variety</th>
                    <th data-sortable="true" class="text-center"  data-field="Grower">Grower</th>
                     <th data-sortable="true" class="text-center"  data-field="Treated">Treated</th>
                    <th data-sortable="true" class="text-right" data-formatter="netformatter" data-field="net">Net Bu</th>
            
                </tr>
            </thead>
           
        </table>
    `;

    Swal.fire({
        title: 'Lot Tickets ',
        html: tableHtml,
        width: '80%',
        showCloseButton: true,
        focusConfirm: false,
        didOpen: () => {
            $('#lotTicketsTable').bootstrapTable({
                data: data
            });
        }
    });
}


function netformatter(value, row, index) {


    return (row.Net).toFixed(2);
}


function dateFormatter(value, row, index) {
    var date = new Date(row.TicketDate);

    return formatDateToMMDDYY(date);
}
function formatDateToMMDDYY(dt) {
    var date = new Date(dt)
    var dd = String(date.getDate()).padStart(2, '0');
    var mm = String(date.getMonth() + 1).padStart(2, '0'); // January is 0!
    var yy = String(date.getFullYear()).substring(2, 4);

    return mm + '/' + dd + '/' + yy;
}

function ticketLinkFormatter(value, row, index) {
    return `<a href="javascript:void(0);" onclick="downloadTicket('${row.UID}',${value})">${value}</a>`;
}
async function downloadTicket(uid, ticket) {
    try {
        const response = await $.ajax({
            type: "POST",
            url: "../../WebService.asmx/DownloadTicket",
            data: JSON.stringify({ UID: uid }),
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        });

        const byteArray = new Uint8Array(response.d);
        const blob = new Blob([byteArray], { type: "application/pdf" });
        const link = document.createElement('a');
        link.href = window.URL.createObjectURL(blob);
        link.download = "Seed_Ticket_" + ticket + ".pdf";
        link.click();
    } catch (error) {
        console.error("Error downloading ticket: ", error);
    }
}






