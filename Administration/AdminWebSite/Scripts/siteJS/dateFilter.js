$(document).ready(function () {





    $('.date').datepicker({
        'format': 'mm/dd/yyyy',
        'autoclose': true
    });




    $('#startDate').on('click', function () {
        $('#startDate').datepicker('show');
    });

    $('#endDate').on('click', function () {
        $('#endDate').datepicker('show');
    });

    $('#startCalendarIcon').on('click', function () {
        $('#startDate').datepicker('show');
    });

    $('#endCalendarIcon').on('click', function () {
        $('#endDate').datepicker('show');
    });






});

function past180Days() {
    var d = getFormattedDate(addDays(-180));

    var dt = getFormattedDate(new Date());


    $('#startDate').val(d);
    $('#endDate').val(dt);


};



function addDays(days) {
    var result = new Date();
    result.setDate(result.getDate() + days);
    return result;
}

function newDate() {
    var d = getFormattedDate(new Date());

    var dt = getFormattedDate(new Date());


    $('#startDate').val(d);
    $('#endDate').val(dt);


};

function getFormattedDateTime(dt) {
    var dtStr = dt.toLocaleDateString([], { year: '2-digit', month: '2-digit', day: '2-digit' }) + ' ' + dt.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
    return dtStr;
    //var year = dt.getFullYear();
    //var yy = year.toString().trim().substring(2, 4)

    //var month = (1 + dt.getMonth()).toString();
    //month = month.length > 1 ? month : '0' + month;

    //var day = dt.getDate().toString();
    //day = day.length > 1 ? day : '0' + day;

    //var date = month + '/' + day + '/' + yy;


    //return  date + " " + addLeadingZeros(dt.getHours()) + ":" + addLeadingZeros(dt.getMinutes())
}

function addLeadingZeros(n) {
    if (n <= 9) {
        return "0" + n;
    }
    return n
}

function getFormattedDate(date) {
    var year = date.getFullYear();

    var month = (1 + date.getMonth()).toString();
    month = month.length > 1 ? month : '0' + month;

    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;

    return month + '/' + day + '/' + year;
}