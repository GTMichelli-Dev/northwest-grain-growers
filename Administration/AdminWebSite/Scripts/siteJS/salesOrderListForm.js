
var Sites = [];
var SalesOrderInfoList = [];


$(document).ready(function () {
    showLoading();

    $('#startDate').val(dateFilterStart());
    $('#endDate').val(dateFilterEnd());



    $('#soStatus').val(salesOrderStatusIndex());


   

    //$('body').on('click',function (e) {
    //    console.log(e.clientX);
    //    console.log(e.clientY);
    //});

 

    //$('body').on('mousedown', function (event) {
    //    switch (event.which) {
    //        case 1:
    //            console.log('Left mouse button is pressed');
    //            break;
    //        case 2:
    //            console.log('Middle mouse button is pressed');
    //            break;
    //        case 3:
    //            console.log('Right mouse button is pressed');
    //            break;
    //        default:
    //            console.log('Nothing');
    //    }
    //});


    

    
    $('#Site').on('change', function () {
        var siteID = $('#Site').val();

        setCurrentSiteID(siteID);
        getSalesOrderList();
    })



    $.ajax({
        type: "POST",
        url: "SystemWebService.asmx/GetSites",
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (results) {
            if (results.d.Success) {
                Sites = JSON.parse(results.d.Data);
                $('.siteSelection').find('option').remove();
                for (var i = 0; i < Sites.length; i++) {
                    var Option = "<option  value='" + Sites[i].ID + "' > " + Sites[i].Description + "</option>";
                    $(".siteSelection").append(Option);
                }
                var siteID = currentSiteID();

                $('#Site').val(siteID);

                
            }
            else {
                showDangerMessage('Error', results.d.Message);;
            }
        }
        ,
        error: function (xhr, status, error) {
            var errorMessage = xhr.status + ': ' + xhr.statusText
            showDangerMessage('Error', errorMessage);
        }
    })

    if (getUrlParameter('customerName') != null) {
        $('.btn-bol').hide();

    }


    $('#tblSalesOrders').bootstrapTable({
        onClickCell: function (field, value, row) {

        },
        onClickRow: function (row, $element, field) {

            console.log(row);
            console.log(row.UID)
            //var mousePos = {};
            //var rect = e.target.getBoundingClientRect();
            //mousePos.x = e.clientX - rect.left; // get the mouse position relative to the element
            //mousePos.y = e.clientY - rect.top;
            if (field == 'bol') {
                $(location).attr('href', 'SalesOrderBols.aspx?UID=' + row.UID);
            }
            else {
                if (getUrlParameter('customerName') != null) {
                    window.location.href = "SalesOrderForm.aspx?UID=" + row.UID + "&NewSalesOrder=true";
                }
                else {
                    window.location.href = "SalesOrderForm.aspx?UID=" + row.UID;
                }

            }
            
        },

    });
    customerName = getUrlParameter('customerName');
    if (customerName != null) {
        $('#CustomerInput').val(customerName);
        $('#header').html('Select Sales Order To Copy');
        $('#soStatus').val(3);
        past180Days();
        $('#btnCancel').show();
        $('#btnNewSalesOrder').hide();
    }
    else {
        $('#btnCancel').hide();
        $('#btnNewSalesOrder').show();
        $('#btnCancel').attr('href', 'SalesOrderForm.aspx?' + $.param({ customerName: customerName })); 
    }

    getSalesOrderList()
});


function updateDateFilter() {
    //var oldStartDate = dateFilterStart();
    //var oldEndDate = dateFilterEnd();
    dateFilterStart($('#startDate').val());
    dateFilterEnd($('#endDate').val());
    $('#startDate').val(dateFilterStart());
    $('#endDate').val(dateFilterEnd());
    //if ((oldStartDate != dateFilterStart()) || (oldEndDate != dateFilterEnd())) {
    //    getSalesOrderList();
    //}

    
}


function getSalesOrderList() {
    let createSalesInvoice = (getUrlParameter('CreateSalesInvoice') || 'false').toLowerCase() == 'true';

    updateDateFilter();
    salesOrderStatusIndex($('#soStatus').val() * 1)
    let inactiveIndex = ($('#soActive').val() * 1);
    let statusIndex = salesOrderStatusIndex();
    if (createSalesInvoice) {
        statusIndex=0
        inactiveIndex = 0
        $('.notInvoice').hide();
    }
    if (statusIndex == 0){
        $('.datepicker').hide();
        $('.soActive').show();
    }
    else {
        $('.datepicker').show();
        $('.soActive').hide();
    }

  

  
    $.ajax({
        type: "POST",
        url: "SalesOrderApi.asmx/GetSalesOrderLists",
        data: JSON.stringify( {
            'SiteId': currentSiteID(),
            'CustomerFilter': $('#CustomerInput').val(), 
            'SeedFilter': $('#SeedInput').val(),
            'TreatmentFilter': $('#TreatmentInput').val(),
            'StartDate': dateFilterStart(),
            'EndDate': dateFilterEnd(),
            'StatusIndex': statusIndex,
            'InactiveIndex': inactiveIndex
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (results) {
            hideLoading();
            if (results.d.Success) {
                SalesOrderInfoList = JSON.parse(results.d.Data);
                console.log(SalesOrderInfoList);
                $('#tblSalesOrders').bootstrapTable('refreshOptions', {
                    data: SalesOrderInfoList
                });
                

            }
            else {
                showDangerMessage('Error', results.d.Message);;
            }
        }
        ,
        error: function (xhr, status, error) {
            hideLoading();
            var errorMessage = xhr.status + ': ' + xhr.statusText
            showDangerMessage('Error', errorMessage);
        }
    })

}





function dateCreatedFormatter(value, row, index) {

    return new Date(row.CreationDate).toLocaleDateString()
 
}




function priorityFormatter(value, row, index) {

    return row.PriorityIdx

}




function iDFormatter(value, row, index) {

    return row.ID

}


function closedFormatter(value, row, index) {

    return (row.SalesOrder.Closed)?'Closed':'Open'

}



function notesFormatter(value, row, index) {
    return row.Notes
}

function accountFormatter(value, row, index) {
    
  
    return row.AccountName


   
}



function bolFormatter(value, row, index) {
    return "<label class='btn-bol btn btn-sm btn-secondary  m-0 pt-0 pb-0'>Bol's</label>"
}
