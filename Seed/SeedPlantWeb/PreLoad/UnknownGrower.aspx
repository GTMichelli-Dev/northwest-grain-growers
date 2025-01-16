<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="UnknownGrower.aspx.cs" Inherits="UnknownGrower" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:UpdatePanel runat="server" ID="Up2">
        <ContentTemplate>
            <asp:Timer runat="server" ID="tmrUPdate" Interval="1000"></asp:Timer>
        </ContentTemplate>

    </asp:UpdatePanel>




    <h3 class=" text-danger ">Unknown Grower</h3>
    <h4>Enter A Description (Min 2 Characters)</h4>

    <div class="row">
        <div class="col-sm-12 col-md-8 col-lg-6">
            <div class="input-group input-group-lg">
                <asp:TextBox CssClass="form-control " placeholder="Grower Decription" AutoCompleteType="Disabled" Font-Size="Larger" runat="server" ID="txtgrower" AutoPostBack="true" OnTextChanged="txtgrower_TextChanged"></asp:TextBox>

            </div>
        </div>
    </div>




    <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">

        <ContentTemplate>

            <div class="row pt-5">
                <div class="col-6 col-md-2">
                    <asp:LinkButton runat="server" ID="btnCancel" CssClass="btn btn-danger " Width="100%" Text="Cancel" OnClientClick="window.history.back();" PostBackUrl="~/Default.aspx" />
                </div>


                <div class="col-6 col-md-2">
                    <asp:Button runat="server" ID="btnOk" CssClass="btn btn-success " Width="100%" Text="OK" OnClick="btnOk_Click" />
                </div>


            </div>


        </ContentTemplate>
    </asp:UpdatePanel>




</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentFull" Runat="Server">
</asp:Content>

