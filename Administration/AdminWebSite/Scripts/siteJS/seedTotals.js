

var reportHub = $.connection.reportHub;

$(document).ready(function () {
    $('#tblVarieties').bootstrapTable();
    $('#tblTreatments').bootstrapTable();
  


    $.connection.hub.disconnected(function () {
        setTimeout(function () {
            $.connection.hub.start();
        }, 5000); // Restart connection after 5 seconds.
    });

    $.connection.hub.start().done(function () {
        //newDate();
        $('#startDate').val((sessionStorage.getItem('startDate') || null) || getFormattedDate(new Date()));
        $('#endDate').val((sessionStorage.getItem('endDate') || null) || getFormattedDate(new Date()));
        $('#ddLocations').val((sessionStorage.getItem('location') || null) || "");
    



        updateData();
        var startDate = $('#startDate').val();
        var endDate = $('#endDate').val();
        var location = $('#ddLocations').val();
        $('#hfStartDate').val(startDate);
        $('#hfEndDate').val(endDate);
        $('#hfLocationID').val("");

    });
  
});


function checkSelectedGroup() {

    ($('#ddLocations').val() == '') ? $('.all-locations').show() : $('.all-locations').hide();
    if ($('#ddLocations').val() != '') {
        var selectedOption = $('#ddGroup').find('option:selected');
        if (selectedOption.hasClass('all-locations')) {
            $('#ddGroup').val('day');
        }
    }
}

function getStartDate() {
   
   
}


reportHub.client.updateTables = function (jsonVarieties, jsonTreatments, clean, treated, total,lastUpdate) {
    var varietyData = JSON.parse(jsonVarieties);
    var treatmentData = JSON.parse(jsonTreatments);
    console.log(varietyData);
    console.log(treatmentData)
    $('#tblVarieties').bootstrapTable('refreshOptions', {
        data: varietyData
    });

    $('#tblTreatments').bootstrapTable('refreshOptions', {
        data: treatmentData

    });

  

    $('#lastUpdate').text('Updated ' + lastUpdate);
    var totalbu = "Clean:" + commaSeparateNumber((clean / 60).toFixed(0)) + " bu.&nbsp&nbsp&nbspTreated:" + commaSeparateNumber((treated / 60).toFixed(0))+ " bu.&nbsp&nbsp&nbspTotal:" + commaSeparateNumber((total / 60).toFixed(0)) + " bu.";
    var totallbs = "Clean:" + commaSeparateNumber(clean) + " lbs.&nbsp&nbsp&nbspTreated:"  + commaSeparateNumber(treated) + " lbs.&nbsp&nbsp&nbspTotal:" + commaSeparateNumber(total) + " lbs.";
    $('#totalBushels').html(totalbu);
    $('#totalLbs').html(totallbs);
    setTimeout(function () { updateData(); }, 300000);
}

function updateData() {
    
    checkSelectedGroup();
    var startDate = $('#startDate').val();
    var endDate = $('#endDate').val();
    sessionStorage.setItem('startDate', startDate);
    sessionStorage.setItem('endDate', endDate);
    var location = $('#ddLocations').val();
    sessionStorage.setItem('location', location);
    $('#hfStartDate').val(startDate);
    $('#hfEndDate').val(endDate);
    $('#hfLocationID').val(location);


    $('#tblVarieties').bootstrapTable('showLoading');
    $('#tblTreatments').bootstrapTable('showLoading');
    reportHub.server.getData((location == '') ? null : location, startDate, endDate);
}



function exportData() {
    //$('#allData').slideUp(200, function () { $('#wait').slideDown() });
    $('#export').hide();
    $('#loading').show();
    var startDate = $('#startDate').val();
    var endDate = $('#endDate').val();
    var locationId = $('#ddLocations').val();
    var groupBy = $('#ddGroup').val();
    var groupType = $('#ddType').val();
    var formattedStartDate = formatDateToMMDDYY(startDate);
    var formattedEndDate = formatDateToMMDDYY(endDate);

    var serviceUrl = "ReportService.asmx/GetSeedTotalsByDateExcelFile";
    var data = {
        locationId: locationId,
        sd: startDate,
        ed: endDate,
        group: groupBy,
        type: groupType
    };

    $.ajax({
        url: serviceUrl,
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $('#export').show();
            $('#loading').hide();

            var location = locationId || '';
            if (location != '') location = "Location_" + location + "_";
            var filename = location + groupType + "_Totals_By_" + groupBy + "_From_" + formattedStartDate + "_To_" + formattedEndDate + ".xlsx"
            filename = filename.toLowerCase();

            var base64Data = response.d; // .d is the property for the result in ASMX services
            var blob = base64ToBlob(base64Data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            var downloadLink = document.createElement("a");
            downloadLink.href = window.URL.createObjectURL(blob);
            downloadLink.download = filename;

            document.body.appendChild(downloadLink);
            downloadLink.click();
            //$('#wait').slideUp(200, function () { $('#allData').slideDown() });
            document.body.removeChild(downloadLink);
        },
        error: function () {
            $('#export').show();
            $('#loading').hide();

        }
    });
}





function commaSeparateNumber(val) {
    while (/(\d+)(\d{3})/.test(val.toString())) {
        val = val.toString().replace(/(\d+)(\d{3})/, '$1' + ',' + '$2');
    }
    return val;
}


function totalLbsformatter(value, row, index) {


    return commaSeparateNumber(row.Net.toFixed(0));
}



function treatedformatter(value, row, index) {
    

    return commaSeparateNumber(row.Treated.toFixed(0));
}


function cleanformatter(value, row, index) {


    return commaSeparateNumber(row.Clean.toFixed(0));
}


function totalOzformatter(value, row, index) {


    return commaSeparateNumber(row.TotalOz.toFixed(1));
}


function totalGalsformatter(value, row, index) {


    return commaSeparateNumber(row.TotalGals.toFixed(2));
}





function formatDateToMMDDYY(dt) {
    var date = new Date(dt)
    var dd = String(date.getDate()).padStart(2, '0');
    var mm = String(date.getMonth() + 1).padStart(2, '0'); // January is 0!
    var yy = String(date.getFullYear()).substring(2, 2);
substring
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