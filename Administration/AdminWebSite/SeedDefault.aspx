<%@ Page Title="" Language="C#" MasterPageFile="~/Seed.master" AutoEventWireup="true" CodeFile="SeedDefault.aspx.cs" Inherits="SeedDefault" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    


    <script src="Scripts/jquery.signalR-2.4.2.min.js"></script>

    <script src="../signalr/hubs"></script>
    <link rel="stylesheet" href="https://unpkg.com/bootstrap-table@1.18.3/dist/bootstrap-table.min.css">
    <script src="https://unpkg.com/bootstrap-table@1.18.3/dist/bootstrap-table.min.js"></script>
    <script src="https://kit.fontawesome.com/a6c24834b3.js" crossorigin="anonymous"></script>
    <script src="Scripts/siteJS/dateFilter.js"></script>
    <script src="Scripts/siteJS/seedTotals.js"></script>
  <%--  <div class="fixed-table-loading table table-bordered table-hover open" style="top: 31.2812px;">
      <span class="loading-wrap">
      <span class="loading-text" style="font-size: 22.12px;">Loading, please wait</span>
      <span id="loadingData" class="animation-wrap"><span class=" animation-dot"></span></span>
      </span>
    
    </div>--%>
    <div id="allData">
            <div class="row text-center ">



        <label class="col-2 offset-5 btn btn-sm fa fa-refresh fa-2 x" onclick="updateData()"></label>


    </div>
    

    <div class="row text-center ">
        <div id="lastUpdate" class="col-12"></div>
    </div>
    <div class="row text-center">
        <div class="col-12 d-flex flex-row justify-content-center">
            <div class="d-flex align-items-center pr-2">
                Export To Excel By
            </div>
            <div class="d-flex align-items-center">
                <select id="ddType" class="form-control pr-4" style="max-width: 200px">
                    <option value="commodity">Commodity</option>
                    <option value="commodityDetails">Commodity Details</option>
                    <option value="variety">Variety</option>
                    <option value="everything">Everything</option>
                </select>
            </div>
            <div class="d-flex align-items-center pl-2 pr-2">
                And Group By
            </div>
            <div class="d-flex align-items-center">
                <select id="ddGroup" class="form-control pr-4" style="max-width: 200px">
                    <option value="day">Day</option>
                     <option  class="all-locations" value="day_location">Day-Location</option>
                    <option value="total">Total</option>
                    <option  class="all-locations" value="total_location">Total-Location</option>
                    <option value="load">Load</option>
                </select>
            </div>
            <div class="d-flex align-items-center pl-1">
                <button id="loading" class="btn btn-primary" style="display:none; min-width:120px" type="button" disabled>
                    <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
                    Loading...
                </button>
                <button id="export" class="btn  btn-outline-success" style="min-width:120px" type="button" onclick="exportData()">Export</button>
            </div>
        </div>
    </div>
    


    
     <div class="row text-center">
         <div id="totalBushels" class=" col-12 h6"></div>
         <div id="totalLbs" class=" col-12 h6"></div>
     </div>


    <div class="row pb-0 pt-2">
        <div class="col-4 pb-0">
        </div>
        <div class="col-4 pb-0">
            <select id="ddLocations" style="width: 100%" onchange="updateData()" class=" text-center form-control rounded border dropdown mt-0 mb-2 pt-0 pb-1">
                <option value="">All Locations</option>
                <option value="24">Walla Walla - 24 </option>
                <option value="25">Dayton - 25 </option>
                <option value="26">Lancaster - 26 </option>
                <option value="27">Garfield - 27</option>


            </select>
        </div>
        <div class="col-4 pb-0">
        </div>
    </div>

    <div class="row  pb-0 pt-0 ">
        <div class="col-3 pb-0">
        </div>


        <div class="filter datepicker dateRange  col-3 pb-0">
            <div class="input-group ">
                <label class="h3" for="startDate"><span class="h6">From</span></label>
                <input type="text" id="startDate" onchange="updateData()" class=" rounded form-control date start  " />
                <span id="startCalendarIcon" class=" input-append fa fa-calendar fa-2x   "></span>
            </div>
        </div>

        <div class="filter datepicker dateRange col-3 pb-0">
            <div class="input-group  ">
                <label class="h3 " for="endDate"><span class="h6">To</span></label>
                <input type="text" onchange="updateData()" id="endDate" class=" rounded form-control date end  " />
                <span id="endCalendarIcon" class=" input-append fa fa-calendar fa-2x   "></span>

            </div>
        </div>
        <div class="col-3 pb-0">
        </div>
    </div>

    <div class="row">
        <div class="col-6">


            <div class="row pb-0 pt-2">
                <div class="col-4 pb-0">
                </div>
                <div class="col-4 pb-0">
                    <h3 class="text-center" style="width: 100%">Varieties</h3>
                </div>
                <div class="col-4 pb-0">
                </div>
            </div>
            <div class="row ">
                <div class="col-12 pt-0 pb-0 ">


                    <table style="font-size: 0.8em" class="pt-0 table  tableSelection table-bordered table-hover table-sm"
                        id="tblVarieties"
                        data-toggle="tblVarieties"
                        data-filter-control="true"
                        data-show-search-clear-button="false"
                        data-show-button-text="false"
                        data-show-columns="false"
                        data-show-pagination-switch="false"
                        data-pagination="false"
                        data-id-field="Description"
                        data-show-footer="true"
                        data-page-list="[10, 25, 50, 100, all]"
                        data-mobile-responsive="true"
                        data-row-style="rowStyle"
                        data-check-on-init="true">
                        <thead>
                            <tr>
                                <%-- <th data-sortable="true" class="text-left  locationCol" data-field="Location_ID">Location</th>--%>
                                <th data-sortable="true" class="text-left" data-field="Description">Variety</th>
                                <th data-sortable="true" class="text-right" data-formatter="treatedformatter" data-field="Treated">Treated lbs.</th>
                                <th data-sortable="true" class="text-right" data-formatter="cleanformatter" data-field="Clean">Clean lbs.</th>
                                <th data-sortable="true" class="text-right" data-formatter="totalLbsformatter" data-field="Total">Total lbs.</th>
                            </tr>
                        </thead>
                    </table>

                </div>
            </div>
        </div>


        <div class="col-6">

            <div class="row pb-0 pt-2">
                <div class="col-4 pb-0">
                </div>
                <div class="col-4 pb-0">
                    <h3 class="text-center" style="width: 100%">Treatments</h3>
                </div>
                <div class="col-4 pb-0">
                </div>
            </div>
            <div class="row ">
                <div class="col-12 pt-0 pb-0 ">


                    <table style="font-size: 0.8em" class="pt-0 table   tableSelection table-bordered table-hover table-sm"
                        id="tblTreatments"
                        data-toggle="tblTreatments"
                        data-filter-control="true"
                        data-show-search-clear-button="false"
                        data-show-button-text="false"
                        data-show-columns="false"
                        data-show-pagination-switch="false"
                        data-pagination="false"
                        data-show-footer="true"
                        data-page-list="[10, 25, 50, 100, all]"
                        data-mobile-responsive="true"
                        data-row-style="rowStyle"
                        data-check-on-init="true">
                        <thead>
                            <tr>

                                <%--   <th data-sortable="true" class="text-left locationCol"  data-field="Location_ID">Location</th>--%>
                                <th data-sortable="true" class="text-left" data-field="Description">Treatment</th>
                                <th data-sortable="true" class="text-right" data-formatter="totalOzformatter" data-field="TotalOz">Total Ounces</th>
                                <th data-sortable="true" class="text-right" data-formatter="totalGalsformatter" data-field="TotalGals">Total Gallons</th>

                            </tr>
                        </thead>
                    </table>

                </div>
            </div>
        </div>
    </div>
</div>

</asp:Content>

