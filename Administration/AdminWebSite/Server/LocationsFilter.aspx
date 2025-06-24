<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="LocationsFilter.aspx.cs" Inherits="Server_LocationFilter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="text-center container  ">


                <h3>Location Filters</h3>

                <div class="container">
                </div>

            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
                <a href="AddLocation.aspx">New Location</a>
            <div class="row">
                <div class="col-8 offset-2">
                    <table id="locationTable"
                        data-toggle="table"
                        data-click-to-select="true"
                        class="table table-bordered">
                        <thead>
                            <tr>
                                <th data-field="SourceDescription" data-sortable="true" style="text-align: left;">Source</th>
                                <th data-field="DestinationDescription" data-sortable="true" style="text-align: left;">Destination</th>
                                <th data-field="actions" data-formatter="actionFormatter" data-events="actionEvents" style="text-align: center;">Actions</th>

                            </tr>
                        </thead>
                    </table>
                </div>
            </div>

            <script>
                let data = [];

                function loadLocationFilters() {
                    $('#locationTable').bootstrapTable('showLoading'); // Show loading indicator

                    $.ajax({
                        url: '/WebService.asmx/GetLocationFilters',
                        method: 'POST',
                        contentType: 'application/json',
                        dataType: 'json',
                        success: function (response) {
                            let rawdata = response.d ? response.d : response;
                            data = JSON.parse(rawdata);
                            console.log(data);
                            $('#locationTable').bootstrapTable('removeAll');
                            $('#locationTable').bootstrapTable('load', data);
                            $('#locationTable').bootstrapTable('hideLoading'); // Hide loading indicator

                            // Refresh the table after loading data

                        },
                        error: function () {
                            $('#locationTable').bootstrapTable('hideLoading'); // Hide loading indicator

                            alert("Failed to get Location filters.");
                        }
                    });
                }


                // Formatter for the delete button
                function actionFormatter(value, row, index) {
                    return `<button class="btn btn-danger btn-sm delete-row" data-uid="${row.Uid}">Delete</button>`;
                }

                // Event handler for delete button
                window.actionEvents = {
                    'click .delete-row': function (e, value, row, index) {
                        if (confirm('Are you sure you want to delete this filter?')) {
                            $.ajax({
                                url: '/WebService.asmx/DeleteLocationFilter',
                                method: 'POST',
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'json',
                                data: JSON.stringify({ UID: row.Uid }),
                                success: function (response) {
                                    let result = response.d ? response.d : response;
                                    if (typeof result === "string") {
                                        result = JSON.parse(result);
                                    }
                                    if (result.Success) {
                                        $('#locationTable').bootstrapTable('removeAll');
                                        loadLocationFilters();
                                    } else {
                                        alert(result.Message);
                                    }
                                },
                                error: function () {
                                    alert("Failed to delete filter.");
                                }
                            });
                        }
                    }
                };

                $(function () {
                    $('#locationTable').bootstrapTable();
                    loadLocationFilters();
                });
            </script>
</asp:Content>

