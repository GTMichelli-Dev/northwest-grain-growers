<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ConfirmWeighTypeChange.aspx.cs" Inherits="PreLoad_ConfirmWeighTypeChange" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    
    <div class="row pt-4">
        <div class=" col-12">
            <h3 runat="server" id="lblhdr" class="text-danger text-center font-weight-bold ">Change Ticket Type From</h3>
        </div>
        <div class=" col-12">
            <h4 runat="server" id="lblFrom" class="text-center font-weight-bold "></h4>
        </div>
        <div class=" col-12">
            <h3 runat="server" id="H2" class="text-danger text-center  font-weight-bold ">To</h3>
        </div>
        <div class=" col-12">
            <h4 runat="server" id="lblTo" class=" text-center font-weight-bold "></h4>
        </div>
     </div>
    <hr />
    <div class="row pt-2">
        <div class=" col-12">
            <h4 class="text-danger font-weight-bold font-italic  text-center  ">This Will Clear All Weights For Ticket</h4>

        </div>
    </div>

    <br />

<div class="row">

    <div class="d-none d-md-block col"></div>
    <div class="col-6  col-md-3 col-lg-2">
        <asp:LinkButton runat="server" ID="btnOk" CssClass="btn btn-sm btn-success " Text="OK" Width="100%" OnClick="btnOk_Click" ></asp:LinkButton>
    </div>
    <div class="col-6  col-md-3 col-lg-2">
        <asp:LinkButton runat="server" ID="btnCancel" CssClass="btn btn-sm btn-danger " Text="Cancel" Width="100%"   PostBackUrl="~/PreLoad/TicketCreator.aspx"  ></asp:LinkButton>
    </div>
    <div class="d-none d-md-block col"></div>

</div>    


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentFull" Runat="Server">
</asp:Content>

