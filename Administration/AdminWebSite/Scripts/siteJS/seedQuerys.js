
$(document).ready(function () {

    //newDate();
    $('#startDate').val((sessionStorage.getItem('startDate') || null) || getFormattedDate(new Date()));
    $('#endDate').val((sessionStorage.getItem('endDate') || null) || getFormattedDate(new Date()));
    $('#ddLocations').val((sessionStorage.getItem('location') || null) || "");
  
});


function downloadTotalsByVariety() {

    var startDate = $('#startDate').val();
    var endDate = $('#endDate').val();
    var locationId = $('#ddLocations').val();


    var serviceUrl = "ReportService.asmx/GetSeedTotalsByDateVarietyExcelFile";
    var data = {
        locationId: locationId,
        sd: startDate,
        ed: endDate
    };

    $.ajax({
        url: serviceUrl,
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {

            var location = locationId || '';
            if (location != '') location = "Location_" + location + "_";
            var filename = location + "Totals_By_Date_From_" + formatDateToMMDDYY(startDate) + "_To_" + formatDateToMMDDYY(endDate) + ".xlsx"


            var base64Data = response.d; // .d is the property for the result in ASMX services
            var blob = base64ToBlob(base64Data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            var downloadLink = document.createElement("a");
            downloadLink.href = window.URL.createObjectURL(blob);
            downloadLink.download = filename;

            document.body.appendChild(downloadLink);
            downloadLink.click();
            document.body.removeChild(downloadLink);
        }
    });
}



function downloadTotalsByDay() {

    var startDate = $('#startDate').val();
    var endDate = $('#endDate').val();
    var locationId = $('#ddLocations').val();


    var serviceUrl = "ReportService.asmx/GetSeedTotalsByDateExcelFile";
    var data = {
        locationId: locationId,
        sd: startDate,
        ed: endDate
    };

    $.ajax({
        url: serviceUrl,
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {

            var location = locationId || '';
            if (location != '') location = "Location_" + location + "_";
            var filename = location + "Totals_By_Date_From_" + formatDateToMMDDYY(startDate) + "_To_" + formatDateToMMDDYY(endDate) + ".xlsx"


            var base64Data = response.d; // .d is the property for the result in ASMX services
            var blob = base64ToBlob(base64Data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            var downloadLink = document.createElement("a");
            downloadLink.href = window.URL.createObjectURL(blob);
            downloadLink.download = filename;

            document.body.appendChild(downloadLink);
            downloadLink.click();
            document.body.removeChild(downloadLink);
        }
    });
}






function formatDateToMMDDYY(dt) {
    var date = new Date(dt)
    var dd = String(date.getDate()).padStart(2, '0');
    var mm = String(date.getMonth() + 1).padStart(2, '0'); // January is 0!
    var yy = String(date.getFullYear()).substr(2, 2);

    return mm + '-' + dd + '-' + yy;
}

function base64ToBlob(base64, mimeType) {
    mimeType = mimeType || '';
    var sliceSize = 1024;
    var byteChars = window.atob(base64);
    var byteArrays = [];

    for (var offset = 0; offset < byteChars.length; offset += sliceSize) {
        var slice = byteChars.slice(offset, offset + sliceSize);
        var byteNumbers = new Array(slice.length);

        for (var i = 0; i < slice.length; i++) {
            byteNumbers[i] = slice.charCodeAt(i);
        }

        var byteArray = new Uint8Array(byteNumbers);
        byteArrays.push(byteArray);
    }

    return new Blob(byteArrays, { type: mimeType });
}