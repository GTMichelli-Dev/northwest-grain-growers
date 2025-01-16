<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ProducerCropFilter.aspx.cs" Inherits="Crops_ProducerCropFilter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <script src="../Scripts/siteJS/cropProducerFilter.js"></script>
    <div id="table">
    <div class="row col-12 d-flex justify-content-center mt-2">
        <h4>Grower Commodity Filters  <input type="button" onclick="addItem()" class=" btn btn-sm btn-outline-dark" value="Add Filter" /></h4>
    </div>
    <div class="row col-6 offset-3 d-flex justify-content-center">
      


        <div class="col-12 pt-0 pb-0 ">
            <table class="pt-0 table  tableSelection table-bordered table-hover table-sm"
                id="tbl"
                data-toggle="tbl">
                <thead>
                    <tr>

                        <th data-sortable="true" class="text-left" data-field="Crop">Commodity</th>
                        <th data-sortable="true"  class="text-left" data-field="Producer">Grower</th>

                        <th data-sortable="true" class="text-right" data-formatter="deleteformatter" data-field="delete"></th>
                    </tr>
                </thead>
            </table>

        </div>
    </div>
</div>

    <div id="addFilter" style="display: none">
        <div class="form-row d-flex justify-content-center ">

            <div class="col-6 ">
                <div class="col-12">
                    <h4>New Grower Commodity Filter</h4>
                </div>
                <div class="col-12">

                    <div class=" form-group">
                        <label>Commodity</label>
                        <select id="ddCrop" class=" form-control"></select>

                    </div>
                </div>

                <div class="col-12">

                    <div class=" form-group">
                        <label>Customer</label>
                        <input type="text" id="txtCustomer" onkeyup="getProducers();" autocomplete="off" onchange="validateName()" class=" form-control" />

                    </div>
                </div>

                <div class=" col-12">

                    <button type="button" style="width: 120px" class="btn btn-danger " onclick="hideNew();" data-dismiss="modal">Cancel</button>
                    <button type="button" id="mmBtnAddItem" style="width: 120px" class="btn btn-success " onclick="saveNewItem();" data-dismiss="modal">Add</button>

                </div>

            </div>
        </div>

    </div>
    </div>

</asp:Content>

