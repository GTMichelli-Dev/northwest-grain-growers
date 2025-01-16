
$(document).ready(function () {
   

   


    $('#tbl').bootstrapTable({
        onClickCell: function (field, value, row) {
            console.log(row);
            console.log(row.UID)
        
            if (field == 'delete') {
                deleteCropProducer(row.UID);
            }
        },
        onClickRow: function (row, $element, field) {
        },

    });

    $('#tbl').bootstrapTable('showLoading');



    
    $.ajax({
        type: "POST",
        url: "/WebService.asmx/GetCrops",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (results) {
            if (results.d.Success) {
                let crops = JSON.parse(results.d.Data);
                $('#ddCrop').find('option').remove();
                var Option = "<option  value='' >Select Crop</option>";
                $("#ddCrop").append(Option);
                for (var i = 0; i < crops.length; i++) {
                    var Option = "<option  value='" + crops[i].UID + "' > " + crops[i].Description + "</option>";
                    $("#ddCrop").append(Option);
                }
                $("#ddCrop").val('');
    
            }
            else {
                showDangerMessage('Error', results.d.Message);
                
            }
        }
        ,
        error: function (xhr, status, error) {
       
            var errorMessage = xhr.status + ': ' + xhr.statusText
            showDangerMessage('Error', errorMessage);
            
        }
    });

    getList()
});


function addItem() {
    $("#txtCustomer").val('');
    showNew();
   /* $('#modalAddItem').modal('show');*/
}

function saveNewItem() {
   
    var message = '';
    var customer = $("#txtCustomer").val();
    var commodity = $("#ddCrop").val();
    if (customer == '') message += "Customer Cannot Be Blank <br/>";
    if (commodity == '') message += "Commodity Cannot Be Blank <br/>";
    if (customer != '' && commodity != '') {
        $.ajax({
            type: "POST",
            url: "/WebService.asmx/AddCropProducer",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                'CropUID': commodity,
                'NameID': customer,
            }),
            dataType: "json",
            success: function (results) {
                getList();
            }
            ,
            error: function (xhr, status, error) {

                var errorMessage = xhr.status + ': ' + xhr.statusText
                showDangerMessage('Error', errorMessage);
                $("#txtCustomer").val('');

            }
        });
    }
    else {
        showDangerMessage('Error Saving Filter', message);
    }
}

function getProducers() {
    let customerFilter = $('#txtCustomer').val();
    if (customerFilter.length >1) {


        $.ajax({
            type: "POST",
            url: "/WebService.asmx/GetProducers",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                'filter': customerFilter
            }),
            dataType: "json",
            success: function (results) {
                if (results.d.Success) {

                    let producers = JSON.parse(results.d.Data);
                    console.log(producers);
                    $("#txtCustomer").autocomplete({
                        source: producers
                    });
                    $('#txtCustomer').css('z-index', 9999);
                    //$("#txtCustomer").autocomplete({

                    //    select: function (event, ui) {

                    //        $("#txtCustomer").val(ui.item.value)


                    //        validateName();
                    //    }
                    //});


                }
                else {
                    showDangerMessage('Error', results.d.Message);

                }
            }
            ,
            error: function (xhr, status, error) {

                var errorMessage = xhr.status + ': ' + xhr.statusText
                showDangerMessage('Error', errorMessage);

            }
        });
    }
}

function hideNew() {
    $('#table').slideDown();
    $('#addFilter').slideUp();
}

function showNew() {
    $('#table').slideUp();
    $('#addFilter').slideDown();
}




function validateName() {

    $.ajax({
        type: "POST",
        url: "/WebService.asmx/CheckProducer",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            'Filter': $("#txtCustomer").val()
        }),
        dataType: "json",
        success: function (results) {
            if (results.d.Success) {
                $("#txtCustomer").val(results.d.Data);
            }
            else {
                $("#txtCustomer").val('');
                
            }

        }
        ,
        error: function (xhr, status, error) {
         
            var errorMessage = xhr.status + ': ' + xhr.statusText
            showDangerMessage('Error', errorMessage);
            $("#txtCustomer").val('');

        }
    });


    }


function deleteCropProducer(uid) {
    $.ajax({
        type: "POST",
        url: "/WebService.asmx/DeleteCropProducer",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            'UID':uid
        }),
        dataType: "json",
        success: function (results) {
            if (results.d.Success) {
                getList();
            }
            else {
                showDangerMessage('Error', results.d.Message);
               
            }
        }
        ,
        error: function (xhr, status, error) {
         
            var errorMessage = xhr.status + ': ' + xhr.statusText
            showDangerMessage('Error', errorMessage);
            
        }
    })

}

function getList() {
    hideNew();
    $('#tbl').bootstrapTable('showLoading');
    $.ajax({
        type: "POST",
        url: "/WebService.asmx/GetCropProducers",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (results) {
            if (results.d.Success) {
                let data = JSON.parse(results.d.Data);
                console.log(data);
                $('#tbl').bootstrapTable('refreshOptions', {
                    data: data
                });
                $('#tbl').bootstrapTable('hideLoading');

            }
            else {
                showDangerMessage('Error', results.d.Message);
                $('#tbl').bootstrapTable('hideLoading');
            }
        }
        ,
        error: function (xhr, status, error) {
        
            var errorMessage = xhr.status + ': ' + xhr.statusText
            showDangerMessage('Error', errorMessage);
            $('#tbl').bootstrapTable('hideLoading');
        }
    })
}


function deleteformatter(value, row, index) {
    return "<label class='btn-bol btn btn-sm btn-danger  m-0 pt-0 pb-0'>Delete</label>"
}








