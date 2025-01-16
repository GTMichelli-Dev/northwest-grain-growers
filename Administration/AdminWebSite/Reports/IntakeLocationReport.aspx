<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="IntakeLocationReport.aspx.cs" Inherits="Reports_IntakeLocationReport" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
               <div class="row">
            <div class=" offset-3  col-md-2 text-center ">

                <asp:TextBox runat="server" ID="txtSelectedDate" CssClass="  text-center form-control  " Font-Size="Large"  AutoPostBack="true" OnTextChanged="txtSelectedDate_TextChanged" ></asp:TextBox>
                  <ajaxToolkit:CalendarExtender runat="server" BehaviorID="txtSelectedDate_CalendarExtender" TargetControlID="txtSelectedDate" ID="txtSelectedDate_CalendarExtender"></ajaxToolkit:CalendarExtender>
            </div>
            <div class="col-md-2 text-center ">

                <asp:TextBox runat="server" ID="txtSelectedEndDate" CssClass="  text-center form-control  " Font-Size="Large"  AutoPostBack="true"  OnTextChanged="txtSelectedEndDate_TextChanged"  ></asp:TextBox>
                  <ajaxToolkit:CalendarExtender runat="server" BehaviorID="txtSelectedEndDate_CalendarExtender" TargetControlID="txtSelectedEndDate" ID="txtSelectedEndDate_CalendarExtender"></ajaxToolkit:CalendarExtender>
            </div>


            <div class="col-md-2"><asp:LinkButton runat="server" ID="Download" OnClick="Download_Click"  Text="Send To Excel" CssClass="btn btn-success  "></asp:LinkButton> </div>
                </div>

   
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
                         <asp:HiddenField ID="hfSelectedDate" runat="server" />
                     <asp:HiddenField ID="hfSelectedEndDate" runat="server" />
            <asp:GridView runat="server" ID="grd1" HorizontalAlign="Center" CssClass="table table-hover " AutoGenerateColumns="False" DataSourceID="sqlSummary">

                <Columns>
                    <asp:BoundField DataField="Location_Id" HeaderText="Location Id" SortExpression="Location_Id"></asp:BoundField>
                    <asp:BoundField DataField="Location" HeaderText="Location" SortExpression="Location"></asp:BoundField>
                    <asp:BoundField DataField="Commodity_Id" HeaderText="Commodity Id" SortExpression="Commodity_Id"></asp:BoundField>
                    <asp:BoundField DataField="Commodity" HeaderText="Commodity" SortExpression="Commodity"></asp:BoundField>
                    <asp:BoundField DataField="UOM" HeaderText="UOM" SortExpression="UOM"></asp:BoundField>
                    <asp:BoundField DataField="Net" HeaderText="Net" SortExpression="Net" DataFormatString="{0:N0}">
                    <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Net UOM" HeaderText="Net UOM" SortExpression="Net UOM" DataFormatString="{0:N2}">
                    <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>

            <asp:SqlDataSource runat="server" ID="sqlSummary" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT Location_Id, Location, Commodity_Id, Commodity, UOM, Net, Net_UOM AS [Net UOM] FROM dbo.LoadsByCommodityLocation(@StartDate, @EndDate) AS LoadsByCommodityLocation_1 ORDER BY Location_Id, Commodity_Id">
                <SelectParameters>
                    <asp:ControlParameter ControlID="hfSelectedDate" PropertyName="Value" Name="StartDate"></asp:ControlParameter>
                    <asp:ControlParameter ControlID="hfSelectedEndDate" PropertyName="Value" Name="EndDate"></asp:ControlParameter>
                </SelectParameters>
            </asp:SqlDataSource>
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>

