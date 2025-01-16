<%@ Page Title="" Language="C#" MasterPageFile="~/Seed.master" AutoEventWireup="true" CodeFile="SeedTicketTotals.aspx.cs" Inherits="SeedDefault" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">




    <script src="../../Scripts/jquery.signalR-2.4.2.min.js"></script>
    <script src="../../signalr/hubs"></script>
    <link rel="stylesheet" href="https://unpkg.com/bootstrap-table@1.18.3/dist/bootstrap-table.min.css">
    <script src="https://unpkg.com/bootstrap-table@1.18.3/dist/bootstrap-table.min.js"></script>
    <script src="https://kit.fontawesome.com/a6c24834b3.js" crossorigin="anonymous"></script>
    <script src="../../Scripts/siteJS/dateFilter.js"></script>
    <script src="../../Scripts/siteJS/seedTicketTotals.js"></script>
    <%--  <div class="fixed-table-loading table table-bordered table-hover open" style="top: 31.2812px;">
      <span class="loading-wrap">
      <span class="loading-text" style="font-size: 22.12px;">Loading, please wait</span>
      <span id="loadingData" class="animation-wrap"><span class=" animation-dot"></span></span>
      </span>
    
    </div>--%>


    <h5 class="text-center">Seed Ticket Totals</h5>

    <div id="allData">
<%--        <div class="row text-center ">



            <label class="col-2 offset-5 btn btn-sm fa fa-refresh fa-2 x" onclick="updateData()"></label>


        </div>--%>

        <!-- Add this spinner element to your HTML -->
<div id="spinner" class="spinner" style="display:none;"></div>

        <div class="row justify-content-center">
            <div class="col-6">
                <table class="table table-bordered text-center">
                    <thead>
                        <tr>
                            <th>Clean Bushels</th>
                            <th>Treated Bushels</th>
                            <th>Total Bushels</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td class="cleanBushels text-right">0</td>
                            <td class="treatedBushels text-right">0</td>
                            <td class="totalBushels text-right">0</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>

        <div class="row text-center ">
            <div id="lastUpdate" class="col-12"></div>
        </div>
     <%--   <div class="row text-center">
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
                        <option class="all-locations" value="day_location">Day-Location</option>
                        <option value="total">Total</option>
                        <option class="all-locations" value="total_location">Total-Location</option>
                        <option value="load">Load</option>
                    </select>
                </div>
                <div class="d-flex align-items-center pl-1">
                    <button id="loading" class="btn btn-primary" style="display: none; min-width: 120px" type="button" disabled>
                        <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
                        Loading...
                    </button>
                    <button id="export" class="btn  btn-outline-success" style="min-width: 120px" type="button" onclick="exportData()">Export</button>
                </div>
            </div>
        </div>--%>




        <div class="row text-center">
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
            <div class="col-12">
                <h3 class="text-center" style="width: 100%">Loads</h3>


                <div class="row ">
                    <div class="col-12 pt-0 pb-0 ">


                        <table style="font-size: 0.8em" class="pt-0 table  tableSelection table-bordered table-hover table-sm"
                            id="tblTotal"
                            data-toggle="tblTotal"
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
                                     <th data-sortable="true" class="text-center"  data-field="Location">Location</th>
                                   
                                    <th data-sortable="true" class="text-left" data-formatter="ticketLinkFormatter" data-field="Ticket">Ticket</th>

                                    <th data-sortable="true" class="text-center" data-formatter="completeformatter" data-field="Complete">Complete</th>
                                    <th data-sortable="true" class="text-left" data-field="Condition">Condition</th>
                                    <th data-sortable="true" class="text-left" data-formatter="dateFormatter" data-field="TicketDate">Date</th>
                                    <th data-sortable="true" class="text-right" data-formatter="netformatter" data-field="net">Net lbs</th>
                                    <th data-sortable="true" class="text-right" data-formatter="netBuformatter" data-field="net">Net Bu</th>
                                    <th data-sortable="true" class="text-left" data-field="CommodityDetails">Commodity</th>
                                    <th data-sortable="true" class="text-left" data-field="Variety">Variety</th>
                                    <th data-sortable="true" class="text-left" data-field="Grower">Grower</th>
                                    

                                </tr>
                            </thead>
                        </table>

                    </div>
                </div>
            </div>


        </div>
    </div>

</asp:Content>

