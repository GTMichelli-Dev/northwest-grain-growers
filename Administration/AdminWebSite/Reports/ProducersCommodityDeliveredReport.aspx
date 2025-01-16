<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ProducersCommodityDeliveredReport.aspx.cs" Inherits="Reports_ProducersCommodityDeliveredReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdateProgress runat="server" ID="UPr">
        <ProgressTemplate>
            <h2 class="text-info text-center  ">Hang On ..... Getting Data</h2>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <h3>
        <asp:Label ID="lblHeader" runat="server" Text="Producer Commodity Delivery Report By District"></asp:Label>
        &nbsp&nbsp
        <asp:LinkButton runat="server" ID="btnDownload" CssClass=" btn btn-success " OnClick="btnDownload_Click">Send To Excel</asp:LinkButton></h3>

    <asp:UpdatePanel runat="server" ID="UP1">
        <ContentTemplate>

            <table style="width: 100%" class="text-center">
                <thead>
                    <tr>
                        <td style="height: 22px; width: 120px">From</td>
                        <td style="height: 22px; width: 120px">To</td>
                        <td style="height: 22px; width: 150px">District</td>
                        <td style="height: 22px; width: 200px">Location</td>
                        <td style="height: 22px;">Commodity</td>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td style="width: 100px">
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtStartDate" AutoPostBack="True" OnTextChanged="txtStartDate_TextChanged"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender runat="server" BehaviorID="txtStartDate_CalendarExtender" TargetControlID="txtStartDate" ID="CalendarExtender1"></ajaxToolkit:CalendarExtender>
                        </td>
                        <td style="width: 100px">
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtEndDate" AutoPostBack="True" OnTextChanged="txtEndDate_TextChanged"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender runat="server" BehaviorID="txtEndDate_CalendarExtender" TargetControlID="txtEndDate" ID="CalendarExtender2"></ajaxToolkit:CalendarExtender>
                        </td>
                        <td style="width: 150px">
                            <asp:DropDownList runat="server" ID="ddDistricts" CssClass="form-control" DataSourceID="sqlDistricts" DataTextField="Text" DataValueField="Id" AutoPostBack="True" OnTextChanged="ddDistricts_TextChanged"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="sqlDistricts" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT 'All Districts' AS Text, null AS Id, 0 AS idx UNION SELECT District  AS Text, District as Id, 1 AS idx FROM Location_Districts ORDER BY idx, Text"></asp:SqlDataSource>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddLocations" CssClass="form-control" DataSourceID="sqlLocations" DataTextField="Text" DataValueField="Id" AutoPostBack="True" OnTextChanged="ddDistricts_TextChanged"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="sqlLocations" CancelSelectOnNullParameter="false" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT 'All Locations' AS Text, NULL AS Id, 0 AS idx UNION SELECT Locations.Description AS Text, Locations.Id, 1 AS idx FROM Location_Districts INNER JOIN Locations ON Location_Districts.District = Locations.District WHERE (Location_Districts.District = ISNULL(@District, Location_Districts.District)) ORDER BY idx, Text">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddDistricts" PropertyName="SelectedValue" Name="District"></asp:ControlParameter>
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddCommodity" CssClass="form-control" DataSourceID="sqlCommodity" DataTextField="Text" DataValueField="Id" AutoPostBack="true" OnTextChanged="ddCommodity_TextChanged"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="SqlCommodity" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT 'All Commodities' AS Text, NULL AS Id, 0 AS idx UNION SELECT Description AS Text,Description as Id, 1 AS idx FROM Crops WHERE (Active = 1) ORDER BY idx, Text"></asp:SqlDataSource>
                        </td>

                    </tr>
                </tbody>
            </table>

            <table style="width: 100%" class="text-center">
                <thead>
                    <tr>
                        <td style="height: 22px; width: 33%">Variety</td>
                        <td style="height: 22px; width: 33%">Producer</td>
                        <td style="height: 22px; width: 33%">Landlord</td>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                            <asp:DropDownList runat="server" ID="ddVariety" CssClass="form-control" DataSourceID="SqlVariety" DataTextField="Text" DataValueField="Id" AutoPostBack="true" OnTextChanged="ddCommodity_TextChanged"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="SqlVariety" CancelSelectOnNullParameter="false" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT 'All Varieties' AS Text, NULL AS Id, 0 AS idx UNION SELECT VarietyList.Variety AS Text, VarietyList.Item_Id AS Id, 1 AS idx FROM Crops INNER JOIN VarietyList ON Crops.Description = VarietyList.Description WHERE (Crops.Description = @CropDescription)">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddCommodity" PropertyName="SelectedValue" Name="CropDescription"></asp:ControlParameter>
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtProducer" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtProducer_TextChanged"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtLandlord" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtProducer_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                </tbody>
            </table>



            <hr />

            <br />



            <asp:GridView ID="GridView1" CssClass="table table-hover table-responsive  " HorizontalAlign="Center" runat="server" AutoGenerateColumns="False" AllowSorting="True" DataSourceID="ObjProducer">

                <Columns>
                    <asp:BoundField DataField="District" HeaderText="District" SortExpression="District"></asp:BoundField>

                    <asp:BoundField DataField="Location" HeaderText="Location" SortExpression="Location"></asp:BoundField>

                    <asp:BoundField DataField="Producer" HeaderText="Producer" ItemStyle-CssClass="text-left" SortExpression="Producer"></asp:BoundField>
                    <asp:BoundField DataField="Landlord" HeaderText="Landlord" ItemStyle-CssClass="text-left" SortExpression="Landlord"></asp:BoundField>

                    <asp:BoundField DataField="Commodity" HeaderText="Commodity" SortExpression="Commodity"></asp:BoundField>
                    <asp:BoundField DataField="Net" HeaderText="Net" ReadOnly="True" DataFormatString="{0:N0}" SortExpression="Net"></asp:BoundField>
                    <asp:BoundField DataField="NetUnits" HeaderText="Net Units" ReadOnly="True" DataFormatString="{0:N0}" SortExpression="NetUnits"></asp:BoundField>
                    <asp:BoundField DataField="Units" HeaderText="UOM" ReadOnly="True" SortExpression="Units"></asp:BoundField>

                    <asp:BoundField DataField="Variety_Id" HeaderText="Variety Id" SortExpression="Variety_Id"></asp:BoundField>
                    <asp:BoundField DataField="Variety" HeaderText="Variety" SortExpression="Variety"></asp:BoundField>
                    <asp:BoundField DataField="LoadCount" HeaderText="LoadCount" ReadOnly="True" SortExpression="LoadCount"></asp:BoundField>
                </Columns>
                <Columns>
                </Columns>
                <EmptyDataTemplate>
                    .
                         <h5 class="text-center">No Producers Matching Filter</h5>

                </EmptyDataTemplate>
            </asp:GridView>


            <asp:ObjectDataSource runat="server" ID="ObjProducer" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="ReportDataSetTableAdapters.ProducersDeliveredCommodityTableAdapter">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtStartDate" PropertyName="Text" Name="StartDate" Type="DateTime"></asp:ControlParameter>
                    <asp:ControlParameter ControlID="txtEndDate" PropertyName="Text" Name="EndDate" Type="DateTime"></asp:ControlParameter>
                    <asp:ControlParameter ControlID="ddDistricts" PropertyName="SelectedValue" Name="District" Type="String"></asp:ControlParameter>
                    <asp:ControlParameter ControlID="ddCommodity" PropertyName="SelectedValue" Name="Commodity" Type="String"></asp:ControlParameter>
                    <asp:ControlParameter ControlID="txtProducer" PropertyName="Text" Name="Producer" Type="String"></asp:ControlParameter>
                    <asp:ControlParameter ControlID="txtLandlord" PropertyName="Text" Name="Landlord" Type="String"></asp:ControlParameter>
                    <asp:ControlParameter ControlID="ddLocations" PropertyName="SelectedValue" Name="Location_Id" Type="Int32"></asp:ControlParameter>
                    <asp:ControlParameter ControlID="ddVariety" PropertyName="SelectedValue" Name="Variety_Id" Type="Int32"></asp:ControlParameter>
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:HiddenField ID="hfStart" runat="server" />
            <asp:HiddenField ID="hfEnd" runat="server" />


        </ContentTemplate>

    </asp:UpdatePanel>

</asp:Content>

