<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AddEditMisc.aspx.cs" Inherits="PreLoad_AddEditMisc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

      <h3 class="col-12 text-center ">Misc Items</h3>
    <asp:UpdatePanel runat="server" ID="up1">
        <ContentTemplate>
             <asp:HiddenField runat="server" ID="hfLocation" />
            <div class="form-row pt-4">
                 <div class="d-none d-md-block col"></div>
                <div class="col-6 col-md-3 col-lg-2">
                    <label for="ddMisc">Item</label>
                    <asp:DropDownList runat="server" ID="ddMisc" AutoPostBack="true" OnTextChanged="ddMisc_TextChanged"  CssClass=" font-weight-bold  form-control " DataSourceID="SqlMisc" DataTextField="Description" DataValueField="ID"></asp:DropDownList>
                </div>
                <div class="col-4 col-md-3 col-lg-2 text-left ">
                    <label for="txtQuantity">Quantity</label>
                    <asp:TextBox runat="server" ID="txtQuantity" CssClass="form-control font-weight-bold  "  type="number" OnTextChanged="txtQuantity_TextChanged"  Text="1" ></asp:TextBox>
                </div>
                 <div class="d-none d-md-block col"></div>
            </div>
           
            
            <div class="row pt-4">

                <div class="d-none d-md-block col"></div>
 
                <div class="col-6  col-md-3 col-lg-2">
                    <asp:LinkButton runat="server" ID="btnCancel" CssClass="btn btn-sm btn-danger " Text="Cancel" Width="100%" PostBackUrl="~/PreLoad/TicketCreator.aspx"></asp:LinkButton>
                </div>
                <div class="col-6  col-md-3 col-lg-2">
                    <asp:LinkButton runat="server" ID="btnOk" CssClass="btn btn-sm btn-success " Text="OK" Width="100%" OnClick="btnOk_Click"></asp:LinkButton>
                </div>
                <div class="d-none d-md-block col"></div>

            </div>    




            <asp:SqlDataSource runat="server" ID="SqlMisc" ConnectionString='<%$ ConnectionStrings:Seed_DataConnectionString %>' SelectCommand="SELECT NULL AS ID, 'Select Item' AS Description, 0 AS idx UNION SELECT Items.ID, Items.Description, 1 AS idx FROM Item_Location INNER JOIN Items ON Item_Location.Id = Items.ID WHERE (Item_Location.Location_ID = @Location_Id) AND (Items.ItemType = N'Other') ORDER BY idx, Description">
                <SelectParameters>
                    <asp:ControlParameter ControlID="hfLocation" PropertyName="Value" Name="Location_Id"></asp:ControlParameter>

                </SelectParameters>
            </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentFull" Runat="Server">
</asp:Content>

