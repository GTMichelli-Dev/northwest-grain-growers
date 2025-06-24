<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AddLocation.aspx.cs" Inherits="Server_AddLocation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="text-center container  ">
              

                <h3>Add Location</h3>
              
                <div class="container">
                </div>

            </div>
 
<div class="row">
    <div class="col-8 offset-2">
        <div class="form-group">
            <label for="fromLocation">From Location</label>
            <select id="fromLocation" class="form-control"></select>
        </div>
        <div class="form-group">
            <label for="toLocation">To Location</label>
            <select id="toLocation" class="form-control"></select>
        </div>
        <button id="saveBtn" class="btn btn-primary" style="display:none;">Save</button>
        <button id="cancelBtn" class="btn btn-secondary">Cancel</button>
    </div>
</div>
    </div>

<script>
    let data = [];
    $(function () {
        $.ajax({
            url: '/WebService.asmx/GetLocations',
            method: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            success: function (response) {
                // If response is wrapped in { d: ... }, unwrap it
                let rawdata = response.d ? response.d : response;
                data = JSON.parse(rawdata);
                console.log(data);

                // Populate dropdowns
                let fromSelect = $('#fromLocation');
                let toSelect = $('#toLocation');
                fromSelect.empty().append('<option value="">Select From Location</option>');
                toSelect.empty().append('<option value="">Select To Location</option>');
                data.forEach(function (loc) {
                    fromSelect.append(`<option value="${loc.Id}">${loc.Description}</option>`);
                    toSelect.append(`<option value="${loc.Id}">${loc.Description}</option>`);
                });
            },
            error: function () {
                alert("Failed to get Locations.");
            }
        });

        // Show save button only when both selects are filled
        $('#fromLocation, #toLocation').on('change', function () {
            let fromVal = $('#fromLocation').val();
            let toVal = $('#toLocation').val();
            if (fromVal && toVal) {
                if (fromVal === toVal) {
                    alert("From Location and To Location cannot be the same.");
                    $('#fromLocation').val('');
                    $('#toLocation').val('');
                    $('#saveBtn').hide();
                } else {
                    $('#saveBtn').show();
                }

             
            } else {
                $('#saveBtn').hide();
            }
        });

        // Cancel button handler
        $('#cancelBtn').on('click', function () {
            window.location.href = '/Server/LocationsFilter.aspx';
        });


        // Save button handler
        $('#saveBtn').on('click', function () {
            let fromVal = $('#fromLocation').val();
            let toVal = $('#toLocation').val();
            let fromText = $('#fromLocation option:selected').text();
            let toText = $('#toLocation option:selected').text();

            $.ajax({
                url: '/WebService.asmx/AddNewLocationFilter',
                method: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify({
                    FromLocationId: fromVal,
                    FromDescription: fromText,
                    ToLocationId: toVal,
                    ToDescription: toText
                }),
                success: function (response) {
                    let result = response.d ? response.d : response;
                    if (typeof result === "string") {
                        result = JSON.parse(result);
                    }
                    if (result.Success) {
                        window.location.href = '/Server/LocationsFilter.aspx';
                    } else {
                        alert(result.Message);
                    }
                },
                error: function (xhr) {
                    alert("Failed to save location filter.");
                }
            });
        });
    });
</script>


        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

