<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ConfirmTicketDelete.aspx.cs" Inherits="ConfirmTicketDelete" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    
    <div class="row">
        <div class=" col-12">
            <h4 runat="server" id="lblhdr" class="text-danger text-center"></h4>
        </div>
          
    </div>
    <br />
    <div class="row ">
        <div class=" d-none d-md-block col"></div>
        <div class="col-12 col-lg-4">
            <asp:FormView runat="server" ID="fvTicket" HorizontalAlign="Center" CssClass=" table font-weight-bold  " DataKeyNames="UID" DataSourceID="sqlTicket">


   
                <ItemTemplate>
                   
                    Date:
                    <asp:Label Text='<%# Bind("Date") %>' runat="server" ID="DateLabel" /><br />
                    Load Type:
                    <asp:Label Text='<%# Bind("[Load Type]") %>' runat="server" ID="Load_TypeLabel" /><br />
                    Grower:
                    <asp:Label Text='<%# Bind("Grower") %>' runat="server" ID="GrowerLabel" /><br />
                    BOL:
                    <asp:Label Text='<%# Bind("BOL") %>' runat="server" ID="BOLLabel" /><br />
                    PO:
                    <asp:Label Text='<%# Bind("PO") %>' runat="server" ID="POLabel" /><br />
                    Comments:
                    <asp:Label Text='<%# Bind("Comments") %>' runat="server" ID="CommentsLabel" /><br />

                </ItemTemplate>
            </asp:FormView>
            <asp:SqlDataSource runat="server" ID="sqlTicket" ConnectionString='<%$ ConnectionStrings:Seed_DataConnectionString %>' SelectCommand="SELECT UID, Ticket, Ticket_Date AS Date, Ticket_Type AS [Load Type], Grower_Name + N' - ' + LTRIM(STR(Grower_ID)) AS Grower, BOL, PO, Comments FROM TicketLocation where UID=@UID">
                <SelectParameters>
                    <asp:QueryStringParameter QueryStringField="UID" Name="UID"></asp:QueryStringParameter>
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
        <div class=" d-none d-md-block col"></div>
    </div>

    <br />
<div class="row">

    <div class="d-none d-md-block col"></div>
    <div class="col-6  col-md-3 col-lg-2">
        <asp:LinkButton runat="server" ID="btnOk" CssClass="btn btn-sm btn-success " Text="OK" Width="100%" OnClick="btnOk_Click" ></asp:LinkButton>
    </div>
    <div class="col-6  col-md-3 col-lg-2">
        <asp:LinkButton runat="server" ID="btnCancel" CssClass="btn btn-sm btn-danger " Text="Cancel" Width="100%"  PostBackUrl="~/Default.aspx" ></asp:LinkButton>
    </div>
    <div class="d-none d-md-block col"></div>

</div>    


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentFull" Runat="Server">
</asp:Content>

