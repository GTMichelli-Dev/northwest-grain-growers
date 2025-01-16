<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Bins.aspx.cs" Inherits="Bins_Bins" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
      <script src="https://cdn.jsdelivr.net/npm/tableexport.jquery.plugin@1.29.0/tableExport.min.js"></script>
 <script src="https://cdn.jsdelivr.net/npm/bootstrap-table@1.18.3/dist/bootstrap-table.min.js"></script>
 <script src="https://cdn.jsdelivr.net/npm/bootstrap-table@1.18.3/dist/extensions/filter-control/bootstrap-table-filter-control.min.js"></script>

    <h4>Bins</h4>
    <div id="toolbar">
       <button id="btnExport" class="btn btn-success" onclick="ExportData()">Send To Excell</button>
       <a href="AddBin.aspx" class="btn btn-outline-dark">Add Bin</a> 

        <%--<button id="clearFilters" class="btn btn-danger">Clear Filters</button>--%>
    </div>
    <table id="binsTable"
        data-cookie="true"
        data-cookie-id-table="binsTable"
        data-toggle="binsTable"
        data-pagination="true"
       
        data-page-list="[50, 100, all]"
        data-page-size="150"
        class="table table-bordered"
        data-toolbar="#toolbar"
       
       
        data-click-to-select="true"
        
        data-minimum-count-columns="2"
      
        data-filter-control="true"
        data-id-field="BinUID">

        <thead>
            <tr>
                <th data-field="District" data-sortable="true" data-filter-control="select">District</th>
               
                <th data-field="Location" data-sortable="true" data-filter-control="select">Location</th>
                <th data-field="Bin" data-sortable="true">Description</th>



                <th  data-sortable="true" data-field="Bushels">Bushels</th>
                <th  data-sortable="true"  data-formatter="proteinFormatter"  data-field="Protein">Protein</th>

                <th data-sortable="true" data-field="AdjustedDate" data-formatter="dateFormatter">Last Measured</th>
                <th data-formatter ="updateBushelsFormatter"></th>
                <th data-formatter ="showHistoryFormatter"></th>
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
            // Function to format the date


            binsData = JSON.parse($('#<%= hfBins.ClientID %>').val()); // Corrected line
            
           $('#binsTable').bootstrapTable({
               data: binsData,
               cookieStorage: 'localStorage'
           });

            //$('#binsTable').on('column-search.bs.table', function (e, field, text) {
            //    let filters = JSON.parse(localStorage.getItem('tableFilters') || '{}');
            //    filters[field] = text;
            //    localStorage.setItem('tableFilters', JSON.stringify(filters));
            //});


            //let filters = JSON.parse(localStorage.getItem('tableFilters') || '{}');

            //// Reapply filters
            //for (let field in filters) {
            //    $('#binsTable').bootstrapTable('filterBy', { [field]: filters[field] });
            //    // Update the UI filter controls
            //    $(`#binsTable th[data-field="${field}"] input, #binsTable th[data-field="${field}"] select`).val(filters[field]);
            //}
        });

        //$('#clearFilters').click(function () {
        //    localStorage.removeItem('tableFilters');
        //    $('#binsTable').bootstrapTable('filterBy', {});
        //});


        function ExportData() {
            $('#binsTable').tableExport({
                type: 'excel',
                escape: 'false',
                fileName: 'BinsData',
                ignoreRow: [0]
            });
        }



        function showHistoryFormatter(value, row, index) {
            return '<a class="btn btn-outline-info" href="BinHistory.aspx?BinUID=' + row.BinUID + '">History</a>';
        }


        function updateBushelsFormatter(value, row, index) {
            return '<a class="btn btn-outline-dark" href="UpdateBushels.aspx?BinUID=' + row.BinUID + '">Update</a>';
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


