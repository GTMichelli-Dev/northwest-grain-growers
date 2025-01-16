<%@ Page Title="" Language="C#" MasterPageFile="~/Seed.master" AutoEventWireup="true" CodeFile="UsedSeedLots.aspx.cs" Inherits="UsedSeedLots" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">




    <script src="../../Scripts/jquery.signalR-2.4.2.min.js"></script>
    <script src="../../signalr/hubs"></script>
    <link rel="stylesheet" href="https://unpkg.com/bootstrap-table@1.18.3/dist/bootstrap-table.min.css">
    <script src="https://unpkg.com/bootstrap-table@1.18.3/dist/bootstrap-table.min.js"></script>
    <script src="https://kit.fontawesome.com/a6c24834b3.js" crossorigin="anonymous"></script>
    <script src="../Scripts/siteJS/usedLots.js"></script>
   
    <!-- Include SweetAlert CSS and JS -->
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css">
<script src="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.js"></script>

<!-- Include Bootstrap Table CSS and JS -->
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-table/1.18.3/bootstrap-table.min.css">
<script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-table/1.18.3/bootstrap-table.min.js"></script>


    <h5 class="text-center">Seed Ticket Totals</h5>

    <div id="allData">

<div id="spinner" class="spinner" style="display:none;"></div>


        <div class="row text-center ">
            <div id="lastUpdate" class="col-12"></div>
        </div>
   


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

        <div class="row">
            <div class="col-12">
                <h3 class="text-center" style="width: 100%">Loads</h3>


                <div class="row ">
                    <div class="col-12 pt-0 pb-0 ">


                        <table style="font-size: 0.8em" class="pt-0 table  tableSelection table-bordered table-hover table-sm"
                            id="tbl"
                            data-toggle="tbl"
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
                                   
                                    <th data-sortable="true" class="text-left" data-formatter="lotLinkFormatter" data-field="Lot">Lot</th>

                                    <th data-sortable="true" class="text-left" data-field="Description">Variety</th>
                                    <th data-sortable="true" class="text-left" data-field="Variety_Id">ID</th>
                                    

                                </tr>
                            </thead>
                        </table>

                    </div>
                </div>
            </div>


        </div>
    </div>

</asp:Content>

