<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="SelectCommodityReport.aspx.cs" Inherits="WeightSheets_SelectCommodityReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <style>
        .alert-warning {
            background-color: #fcf8e3; /* Bootstrap alert-warning background color */
            border: 1px solid #ddd; /* Add border to maintain cell borders */
            color: black;
        }

        .alert-success {
            background-color: #d4edda; /* Bootstrap alert-success background color */
            border: 1px solid #c3e6cb; /* Add border to maintain cell borders */
            color: black;
        }

        .grid-container table {
            width: 100%;
            border-collapse: collapse;
        }

        .grid-container th, .grid-container td {
            border: 1px solid #ddd; /* Ensure all cells have borders */
        }

        .spinner {
            display: none;
            position: fixed;
            z-index: 999;
            top: 25%;
            left: 50%;
            width: 100px;
            height: 100px;
            margin: -25px 0 0 -25px;
            border: 8px solid #f3f3f3;
            border-top: 8px solid #3498db;
            border-radius: 50%;
            animation: spin 2s linear infinite;
        }

        @keyframes spin {
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
        }
    </style>

    <div class="text-center">
        <asp:LinkButton ID="lnkSelect" CssClass="btn btn-success" ClientIDMode="Static" OnClick="lnkSelect_Click" runat="server">Send To Excel</asp:LinkButton>
    </div>
    <h3>
        <asp:Label ID="lblHeader" runat="server" Text="Commodities By Date / Location"></asp:Label>
    </h3>

    <asp:UpdatePanel runat="server" ID="UP1">
        <ContentTemplate>
            <asp:HiddenField ID="hfRecordCount" runat="server" ClientIDMode="Static" />

            <table style="width: 100%" class="text-center">
                <div class="d-flex justify-content-center">
                    <div class="form-row d-flex align-items-center">
                        <div class="col-3">
                            <asp:DropDownList runat="server" ID="cboLocation" CssClass="form-control" DataSourceID="sqlLocation" DataTextField="Description" DataValueField="value" AutoPostBack="False" OnTextChanged="cboLocation_TextChanged"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="sqlLocation" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT NULL AS value, 'All Locations' AS Description, 0 AS idx UNION SELECT Id AS Value, Description + N' - ' + LTRIM(STR(Id)) AS Description, 1 AS idx FROM Locations ORDER BY idx, Description"></asp:SqlDataSource>
                        </div>
                        <div class="col-2">
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtStartDate" AutoPostBack="False"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender runat="server" BehaviorID="txtStartDate_CalendarExtender" TargetControlID="txtStartDate" ID="txtStartDate_CalendarExtender"></ajaxToolkit:CalendarExtender>
                        </div>
                        <div class="col-2">
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtEndDate" AutoPostBack="False"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender runat="server" BehaviorID="txtEndDate_CalendarExtender" TargetControlID="txtEndDate" ID="CalendarExtender1"></ajaxToolkit:CalendarExtender>
                        </div>
                        <div class="col-2">
                            <asp:Button runat="server" ID="btnFilter" OnClick="btnFilter_Click" ClientIDMode="Static" CssClass="btn btn-outline-info" Text="Filter" />
                        </div>
                    </div>
                </div>

                <hr />

                <br />

                <div class="grid-container">
                    <asp:GridView ID="GridView1" CssClass="table table-hover" HorizontalAlign="Center" runat="server" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="false" OnRowDataBound="GridView1_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="Location" HeaderText="Location" ItemStyle-HorizontalAlign="Left" SortExpression="Location" ReadOnly="True">
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Crop" HeaderText="Crop" ItemStyle-HorizontalAlign="Left" SortExpression="Crop" ReadOnly="True">
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="UOM" HeaderText="Units" ItemStyle-HorizontalAlign="Left" SortExpression="Units">
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Intake" HeaderText="Intake" ItemStyle-HorizontalAlign="Right" SortExpression="Intake" ReadOnly="True">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="IntakeLoads" HeaderText="Intake Loads" ItemStyle-HorizontalAlign="Left" SortExpression="IntakeLoads" ReadOnly="True">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Transfered" HeaderText="Transfered" ItemStyle-HorizontalAlign="Left" SortExpression="Transfered" ReadOnly="True">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Outbound_Location" HeaderText="Transfered To" ItemStyle-HorizontalAlign="Right" SortExpression="Outbound_Location" ReadOnly="True">
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TransferLoads" HeaderText="Transfer Loads" ItemStyle-HorizontalAlign="Left" SortExpression="TransferLoads" ReadOnly="True">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                        </Columns>
                        <EmptyDataTemplate>
                            <h2>No Data Matching Filter</h2>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>


                <asp:HiddenField ID="hfStart" runat="server" />
                <asp:HiddenField ID="hfEnd" runat="server" />

                <div class="spinner" id="loadingSpinner"></div>

                <script type="text/javascript">
                    function checkRecordCount() {
                        var recordCount = $('#hfRecordCount').val();
                        console.log('Record Count', recordCount);
                        (recordCount > 0) ? $('#lnkSelect').show() : $('#lnkSelect').hide();
                    }

                    function showSpinner() {
                        console.log('show spinner');
                        //var spinner = $('#loadingSpinner');
                        //console.log('Spinner element:', spinner);
                        //spinner.show();
                    }

                    function hideSpinner() {
                        //console.log('hide spinner');
                        //var spinner = $('#loadingSpinner');
                        //console.log('Spinner element:', spinner);
                        //spinner.hide();
                    }

                    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
                        console.log('pageLoaded event');
                        checkRecordCount();
                        hideSpinner();
                    });

                    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                        console.log('endRequest event');
                        checkRecordCount();
                        hideSpinner();
                    });

                    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(function () {
                        console.log('beginRequest event');
                        showSpinner();
                    });

                    //$(document).ready(function () {
                    //    $('#lnkSelect').click(function (event) {
                    //        event.preventDefault(); // Prevent the default action
                    //        __doPostBack('lnkSelect', '');
                    //        setTimeout(function () {
                    //            showSpinner();
                               
                    //        }, 250);
                    //    });

                    //    $('#btnFilter').click(function (event) {
                    //        event.preventDefault(); // Prevent the default action
                    //        __doPostBack('btnFilter', ''); 
                    //        setTimeout(function () {
                    //            showSpinner();
                               
                    //        }, 250);
                    //    });
                    //});
                   
</script>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
