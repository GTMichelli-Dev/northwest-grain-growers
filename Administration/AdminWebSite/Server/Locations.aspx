<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Locations.aspx.cs" Inherits="Server_Locations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="text-center container  ">
              

                <h3>Location Filters</h3>
              
                <div class="container">
                </div>

            </div>
 
<div class="row"><div class="col-8 offset-2">                
<table id="locationTable"
       data-toggle="table"
       data-click-to-select="true"
       class="table table-bordered">
    <thead>
        <tr>
            <th data-field="Description" data-sortable="true" style="text-align: left;">Description</th>
            <th data-field="Id" data-sortable="true">Location ID</th>
          
            <th data-field="FilterDescription" data-sortable="true" >Filters</th>
        </tr>
    </thead>
</table>
</div>
    </div>

<script>
    let data = [];
    $(function () {
        $.ajax({
            url: '/WebService.asmx/GetLocationFilters',
            method: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            success: function (response) {
                // If response is wrapped in { d: ... }, unwrap it
                let rawdata = response.d ? response.d : response;
                data = JSON.parse(rawdata);
                console.log(data);
                $('#locationTable').bootstrapTable('load', data);

            },
            error: function () {
                alert("Failed to get license status.");
            }
        });
    });


</script>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

