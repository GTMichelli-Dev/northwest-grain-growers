<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="History.aspx.cs" Inherits="PreLoad_History" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
 
    
    
 <%-- <div class="d-none d-xl-block">
     XL
 </div>
  
       
 <div class="d-none d-lg-block d-xl-none ">
     Lg
 </div>


 <div class="d-none d-md-block d-lg-none ">
    medium
 </div>

 <div class="d-none d-sm-block d-md-none ">
      small  
 </div>


 <div class= "d-block d-sm-none">
    xs
 </div>--%>



    <div class="row">
        <div class="col-12">
            <h5>
                <asp:Label runat="server" ID="lblName"></asp:Label>
            </h5>

        </div>
        <div class="col-6 col-md-2 col-xl-1  ">
            <asp:LinkButton runat="server" Width="100%" PostBackUrl="~/PreLoad/SelectGrower.aspx" Text="Cancel" ID="btnBack" CssClass=" btn btn-danger btn-sm "></asp:LinkButton>
        </div>
    </div>
                   <div class="row ">
                <div class="col-12">

                    <asp:GridView runat="server" ID="grdHistory"  RowStyle-CssClass="border"  HeaderStyle-Font-Size="Small" RowStyle-Font-Size="Small"  Width="100%" CssClass="table table-hover table-sm  table-borderless table-responsive-sm " Font-Size="Larger" ShowHeader="False" AutoGenerateColumns="False" DataKeyNames="UID" DataSourceID="sqlGrowerHistory" AllowPaging="True" AllowSorting="True">

                        <Columns>
                            <asp:TemplateField HeaderText="Ticket" SortExpression="Ticket" HeaderStyle-Width="135px" ItemStyle-Width ="135px" >
                             
                                <ItemTemplate>
                                   <div class="form-row">
                                       <div class=" col-12">
                                           <asp:Label runat="server" Text='<%#"Ticket: "+ Eval("Ticket") %>' ID="Label1"></asp:Label>
                                       </div>
                                       <div class="col-12">
                                           <asp:Label runat="server"  Text='<%# Bind("Ticket_Date", "{0:g}") %>' ID="Label2"></asp:Label>
                                       </div>
                                       <div class="col-12">
                                           <asp:LinkButton runat="server" CssClass="btn btn-outline-dark " Width="100%" OnClick="lnkSelect_Click"  ID="lnkSelect" Text="Duplicate"></asp:LinkButton>
                                       </div>


                                    </div>

                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField  >
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hfVarietyTicketUID" Value='<%# Eval("UID") %>' />
                                    <asp:ListBox runat="server"  CssClass="table table-sm table-responsive " Width="100%" ID="lstVarieties" DataSourceID="SqlHistoryVarieties" DataTextField="Variety_Description" DataValueField="Variety_ID"></asp:ListBox>
                                    <asp:SqlDataSource runat="server" ID="SqlHistoryVarieties" ConnectionString='<%$ ConnectionStrings:Seed_DataConnectionString %>' SelectCommand="SELECT Variety_ID, Variety_Description FROM TicketVarieties WHERE (Seed_Ticket_UID = @UID) ORDER BY Variety_ID">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="hfVarietyTicketUID" PropertyName="Value" Name="UID"></asp:ControlParameter>
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </ItemTemplate>


                            </asp:TemplateField >
                            <asp:TemplateField  >
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hfTreatTicketUID" Value='<%# Eval("UID") %>' />
                                    <asp:ListBox runat="server" Width="100%" CssClass="table table-sm table-responsive " ID="lstTreatments" DataSourceID="SqlHistoryTreatments" DataTextField="Custom_Name" DataValueField="Treatment_ID"></asp:ListBox>
                                    <asp:SqlDataSource runat="server" ID="SqlHistoryTreatments" ConnectionString='<%$ ConnectionStrings:Seed_DataConnectionString %>' SelectCommand="SELECT Treatment_ID, Custom_Name FROM Seed_Ticket_Treatments WHERE (Seed_Ticket_UID = @UID) ORDER BY Custom_Name">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="hfTreatTicketUID" PropertyName="Value" Name="UID"></asp:ControlParameter>
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </ItemTemplate>


                            </asp:TemplateField>
                          

                        </Columns>

                    </asp:GridView>

                </div>
            </div>






    <asp:SqlDataSource runat="server" ID="sqlGrowerHistory" ConnectionString='<%$ ConnectionStrings:Seed_DataConnectionString %>' SelectCommand="SELECT TOP (10) Ticket, Location_ID, Ticket_Date, Grower_ID, UID FROM Seed_Tickets WHERE (Grower_ID = @Grower_ID) AND (Location_ID = @Location_ID) AND (DATEDIFF(day, Ticket_Date, GETDATE()) < 30) ORDER BY Ticket_Date DESC">
                <SelectParameters>
                    <asp:QueryStringParameter QueryStringField="GrowerId" Name="Grower_ID" Type="Int32"></asp:QueryStringParameter>
                    <asp:QueryStringParameter QueryStringField="locationId" Name="Location_ID" Type="Int32"></asp:QueryStringParameter>

                </SelectParameters>
            </asp:SqlDataSource>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentFull" Runat="Server">
</asp:Content>

