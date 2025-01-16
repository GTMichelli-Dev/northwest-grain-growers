<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="BinHistory.aspx.cs" Inherits="Bins_BinHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <script src="https://cdn.jsdelivr.net/npm/tableexport.jquery.plugin@1.29.0/tableExport.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap-table@1.18.3/dist/bootstrap-table.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap-table@1.18.3/dist/extensions/filter-control/bootstrap-table-filter-control.min.js"></script>

    <h4 runat="server" id="header">Bin History</h4>
    <div id="toolbar" style="display: flex; justify-content: space-between; width: 100%;">
        <div>
            <a href="Bins.aspx" class="btn btn-outline-dark">Back</a>
        </div>
        <div>
            <button id="btnExport" class="btn btn-success" onclick="ExportData()">Send To Excel</button>
        </div>
    </div>

    <table id="binsTable"
        data-toggle="binsTable"
        data-pagination="true"
        data-page-list="[50, 100, all]"
        data-page-size="150"
        class="table table-bordered"
        data-toolbar="#toolbar"
        data-minimum-count-columns="2"
        data-filter-control="true"
        data-id-field="BinUID">

        <thead>
            <tr>
                <th data-sortable="true" data-field="AdjustedDate" data-formatter="dateFormatter">Last Measured</th>
                <th data-sortable="true" data-field="Bushels">Bushels</th>
                <th data-sortable="true" data-formatter="proteinFormatter" data-field="Protein">Protein</th>
            </tr>
        </thead>
    </table>
    <asp:HiddenField ID="hfBins" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentFull" runat="Server">
    <script type="text/javascript">
        var binsData = [];
        $(function () {
            console.log('Getting Bins');
            binsData = JSON.parse($('#<%= hfBins.ClientID %>').val());
            $('#binsTable').bootstrapTable({
                data: binsData
            });
        });

        function ExportData() {
            $('#binsTable').tableExport({
                type: 'excel',
                escape: 'false',
                fileName: 'BinsData',
            });
        }

        function proteinFormatter(value, row, index) {
            if (!isNaN(value)) {
                return parseFloat(value).toFixed(3);
            } else {
                return "0.000";
            }
        }

        function dateFormatter(value, row, index) {
            var date = new Date(parseInt(value.substr(6)));
            console.log('date', date);
            var options = {
                year: 'numeric',
                month: '2-digit',
                day: '2-digit',
                hour: '2-digit',
                minute: '2-digit',
                hour12: true
            };
            return date.toLocaleString('en-US', options).replace(',', '');
        }
    </script>
</asp:Content>
