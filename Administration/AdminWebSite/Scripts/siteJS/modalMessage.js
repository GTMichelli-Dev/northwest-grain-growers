





function showMessage(header, message) {

    $('.modal-message-header').removeClass('header-danger');
    $('.modal-message-header').removeClass('header-warning');
    $('.modal-message-header').removeClass('header-info');

    $('.messageHeader').html(header);
    $('.messageToDisplay').html(message);
    $('#modalMessage').modal('show');
   
    $('#mmBtnAck').off('click');
}



function showDangerMessage(header, message) {
   
    $('#btnMessageCancel').off();
    $('.modal-message-header').removeClass('header-warning');
    $('.modal-message-header').removeClass('header-info');
    $('.modal-message-header').addClass('header-danger');

    $('.messageHeader').html(header);
    $('.messageToDisplay').html(message);
    $('#modalMessage').modal('show');
    $('#mmBtnAck').off('click');
    
}



function showWarningMessage(header, message) {
 
    $('.modal-message-header').removeClass('header-danger');
    $('.modal-message-header').removeClass('header-info');
    $('.modal-message-header').addClass('header-warning');
    $('.messageHeader').html(header);
    $('.messageToDisplay').html(message);
    $('#modalMessage').modal('show');
    $('#mmBtnAck').off('click');

    

}




function showInfoMessage(header, message) {

    $('.modal-message-header').removeClass('header-danger');
    $('.modal-message-header').removeClass('header-warning');

    $('.modal-message-header').addClass('header-info');
    $('.messageHeader').html(header);
    $('.messageToDisplay').html(message);
    $('#modalMessage').modal('show');
    $('#mmBtnAck').off('click');

}





function showConfirmMessage(header, message) {

    $('#btnConfirmOK').hide();
    $('#btnConfirmOK').off('click');
    $('#btnCancelConfirm').off('click');
    $('#confirmInput').val('');
    $('#confirmHeader').html(header);
    $('#confirmMessage').html(message);
    $('#modalConfirm').modal('show');
 

}


function checkConfirm(e) {
    ($(e).val() == 'CONFIRM') ? $('#btnConfirmOK').show() : $('#btnConfirmOK').hide()
    
}



function showYesNoMessage(header, message) {
  


    $('.yesno-header').removeClass('header-danger');
    $('.yesno-header').removeClass('header-warning');

    $('.yesno-header').addClass('header-info');


    $('.messageHeader').html(header);
    $('.messageToDisplay').html(message);
    $('#modalYesNoMessage').modal('show');
    $('#mmNo').off('click');
    $('#mmYes').off('click');


}






function showAddLotMessage(header, message) {



    $('.modal-message-header').removeClass('header-danger');
    $('.modal-message-header').removeClass('header-warning');

    $('.modal-message-header').addClass('header-info');
    $('#addLotHeader').html(header);
    $('.messageToDisplay').html(message);
    $('#modalAddLot').modal('show');
    $('#mALAddOK').off('click');
   
    $('#mAlCancel').off('click');

}

//function modalMessage() {

//    var html = [];
//    html.push('<div class="modal  fade  " id="modalMessage" data-backdrop="static" tabindex="-1" role="dialog">');
//    html.push('     <div class="modal-dialog  " style="min-width: 550px">');
//    html.push('     <!-- Modal content-->');
//    html.push('         <div class="modal-content">');
//    html.push('             <div class="modal-header modal-message-header flash-bw  header-danger  ">');
//    html.push('                     <h4 class="modal-title messageHeader  ">message from server</h4>');
//    html.push('                     <button type="button" class="close" data-dismiss="modal">&times;</button>');
//    html.push('             </div>');
//    html.push('             <div class=" modal-body text-center ">');
//    html.push('                 <p class="h2 messageToDisplay font-weight-bold text-center ">Some text in the modal.</p>');
//    html.push('             </div>');
//    html.push('             <div class="modal-footer">');
//    html.push('                 <button type="button" id="btnMessageCancel" style="width: 120px" class="btn btn-danger " data-dismiss="modal">Cancel</button>');
//    html.push('             </div>');
//    html.push('         </div>');
//    html.push('     </div>');
//    html.push('</div>');
//    return [
//        html.join('')

//    ]

//}
