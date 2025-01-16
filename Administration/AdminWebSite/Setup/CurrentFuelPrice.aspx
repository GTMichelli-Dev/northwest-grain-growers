<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CurrentFuelPrice.aspx.cs" Inherits="Setup_CurrentFuelPrice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel runat="server" >
        <ContentTemplate >
       

            <div >
                <h3>Current Fuel Price</h3>
                <h3>
                    <asp:Label runat="server" ID="lblError" CssClass="text-danger  text-center "></asp:Label>

                </h3>
                <div class="row">
                    <p class="offset-5 col-md-2 text-center ">
                        <asp:TextBox CssClass="form-control text-right " autocomplete="false" AutoCompleteType="None" runat="server" ID="txtCurrentPrice" MaxLength="6"></asp:TextBox>
                        <ajaxToolkit:FilteredTextBoxExtender runat="server" BehaviorID="txtCurrentPrice_FilteredTextBoxExtender" TargetControlID="txtCurrentPrice" ID="txtCurrentPrice_FilteredTextBoxExtender" ValidChars="0123456789."></ajaxToolkit:FilteredTextBoxExtender>

                    </p>
                </div>

                <div class="row">

                    <div class="offset-4 col-md-2 text-center ">

                        <asp:LinkButton ID="btnNewOk" CssClass="btn btn-block btn-sm  btn-outline-success  " OnClick="btnNewOk_Click" runat="server">Save</asp:LinkButton>
                    </div>
                    <div class="  col-md-2 text-center  ">
                        <asp:LinkButton ID="LinkButton1" CssClass="btn btn-block btn-sm   btn-outline-danger " PostBackUrl="~/Default.aspx" runat="server">Cancel</asp:LinkButton>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

