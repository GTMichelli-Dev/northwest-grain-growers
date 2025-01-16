<%@ Page Title="" Language="C#" MasterPageFile="~/Seed.master" AutoEventWireup="true" CodeFile="SeedQuerys.aspx.cs" Inherits="SeedQuerys" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">


       <script src="Scripts/siteJS/dateFilter.js"></script>

    <script src="Scripts/siteJS/seedQuerys.js"></script>

        
    
   
  
    <div class="row pb-0 pt-2">
        <div class="col-4 pb-0">
        </div>
        <div class="col-4 pb-0">
            <select id="ddLocations" style="width: 100%"  class=" text-center form-control rounded border dropdown mt-0 mb-2 pt-0 pb-1">
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
                <input type="text" id="startDate"  class=" rounded form-control date start  " />
                <span id="startCalendarIcon" class=" input-append fa fa-calendar fa-2x   "></span>
            </div>
        </div>

        <div class="filter datepicker dateRange col-3 pb-0">
            <div class="input-group  ">
                <label class="h3 " for="endDate"><span class="h6">To</span></label>
                <input type="text"  id="endDate" class=" rounded form-control date end  " />
                <span id="endCalendarIcon" class=" input-append fa fa-calendar fa-2x   "></span>

            </div>
        </div>
        <div class="col-3 pb-0">
        </div>
    </div>

         <div  class="row text-center ">
           
            
        
        <label  class="col-2 offset-5 btn btn-sm mb-0 mt-1 btn-outline-success" onclick="downloadTotalsByVariety()">Download Totals By Variety</label>
            <label  class="col-2 offset-5 btn btn-sm mb-0 mt-1 btn-outline-success" onclick="downloadTotalsByDay()">Download Totals By Day</label>


          
    </div>


</asp:Content>

