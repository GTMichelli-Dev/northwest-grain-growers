<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="WeightSheets.aspx.cs" Inherits="WeightSheets" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdateProgress runat="server" ID="UPr">
        <ProgressTemplate >
            <%--<h2 class="text-info text-center  ">Getting Data</h2>--%>
         </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel runat="server" ID="UP1">
        <ContentTemplate>



            <h3>
                <asp:Label ID="lblHeader" runat="server" Text="Weightsheets"></asp:Label></h3>

            <table style="width: 100%"  class="text-center ">
                <tr>
                    <td colspan="2" class="text-center ">Find By Weight Sheet
                    </td>

                </tr>
                <tr>
                    <td colspan="2" class="text-center ">
                        <asp:TextBox runat="server" CssClass="input-sm " ID="txtWSID" AutoPostBack="True"></asp:TextBox>
                        <ajaxToolkit:FilteredTextBoxExtender runat="server" BehaviorID="txtWSID_FilteredTextBoxExtender" TargetControlID="txtWSID" ID="txtWSID_FilteredTextBoxExtender" ValidChars="0123456789"></ajaxToolkit:FilteredTextBoxExtender>
                    </td>

                </tr>
                <tr>
                    
                    <td>From</td>
                    
                    <td>To</td>
                    
                </tr>
                <tr>
                    
                    <td>
                        <asp:TextBox runat="server" CssClass="input-sm " ID="txtStartDate" AutoPostBack="True" OnTextChanged="txtStartDate_TextChanged"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender runat="server" BehaviorID="txtStartDate_CalendarExtender" TargetControlID="txtStartDate" ID="txtStartDate_CalendarExtender"></ajaxToolkit:CalendarExtender>
                    </td>
                    
                    <td>
                        <asp:TextBox runat="server" CssClass="input-sm " ID="txtEndDate" AutoPostBack="True" OnTextChanged="txtEndDate_TextChanged"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender runat="server" BehaviorID="txtEndDate_CalendarExtender" TargetControlID="txtEndDate" ID="txtEndDate_CalendarExtender"></ajaxToolkit:CalendarExtender>
                    </td>
                    
                </tr>
            </table>




            <table style="width: 100%" class="text-center ">
                <tr>
                    <td></td>
                    <td>Type
                    </td>
                    <td>Location
                    </td>
                    <td>Commodity
                    </td>
                    <td>Grower
                    </td>


                </tr>



                <tr>
                    <td>

                        <asp:DropDownList runat="server" ID="ddOpenClosed" CssClass="input-sm " DataTextField="Text" AutoPostBack="True" DataSourceID="sqlOpenClosed" DataValueField="value" OnTextChanged="ddOpenClosed_TextChanged"></asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="sqlOpenClosed" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT 'Open/Closed' AS Text, NULL AS value, 1 AS idx UNION SELECT 'Open' AS Text, CONVERT (bit, 0) AS value, 2 AS idx UNION SELECT 'Closed' AS Text, CONVERT (bit, 1) AS value, 0 AS idx ORDER BY idx"></asp:SqlDataSource>
                    </td>

                    <td>

                        <asp:DropDownList runat="server" ID="ddType" CssClass="input-sm " DataTextField="Text" AutoPostBack="True" DataSourceID="sqlType" DataValueField="value" OnTextChanged="ddType_TextChanged"></asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="sqlType" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT 'Intake/Transfer' AS text, NULL AS value, 0 AS idx UNION SELECT 'Intake' AS text, 'Intake' AS value, 1 AS idx UNION SELECT 'Transfer' AS text, 'Transfer' AS value, 2 AS idx ORDER BY idx"></asp:SqlDataSource>
                    </td>
                    <td>

                        <asp:DropDownList runat="server" ID="cboLocation" CssClass="input-sm " DataSourceID="sqlLocation" DataTextField="Text" DataValueField="Id" AutoPostBack="True" OnTextChanged="cboLocation_TextChanged"></asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="sqlLocation" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT 'All Locations' AS Text, null AS Id, 0 AS idx UNION SELECT Description + ' - ' + LTRIM(STR(Id)) AS Text, Id, 1 AS idx FROM Locations ORDER BY idx, Text"></asp:SqlDataSource>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddCommodity" CssClass="input-sm" DataSourceID="sqlCommodity" DataTextField="Text" DataValueField="Id" AutoPostBack="true" OnTextChanged="ddCommodity_TextChanged"></asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="SqlCommodity" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT 'All Commodities' AS Text, NULL AS Id, 0 AS idx UNION SELECT Description AS Text, Description, 1 AS idx FROM Crops WHERE (Active = 1) ORDER BY idx, Text"></asp:SqlDataSource>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtGrower" CssClass="form-control " AutoPostBack="true" OnTextChanged="txtGrower_TextChanged"></asp:TextBox>
                    </td>
                </tr>
            </table>


            <hr />

            <br />



            <asp:GridView ID="GridView1" CssClass="table table-hover table-responsive  " HorizontalAlign="Center" runat="server" DataSourceID="sqlWeightSheets" AutoGenerateColumns="False" AllowSorting="True" DataKeyNames="UID" AllowPaging="True" PageSize="50">
                <Columns>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="" SortExpression="btnDetails">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDetails" CssClass="btn btn-primary " runat="server" OnClick="lnkDetails_Click" Text='<%# Eval("btnDetails") %>'></asp:LinkButton>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle CssClass=" text-left " />
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="lnkLoads" CssClass="btn btn-default " Text="Loads" PostBackUrl='<%#"~/WeightSheets/WeightSheetLoads.aspx?WSUID="+ Eval("UID")+"&WS_ID="+ Eval("WS_ID") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="WSType" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass=" text-right " HeaderText="Type" ReadOnly="True" SortExpression="WSType">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle CssClass=" text-left "></ItemStyle>
                    </asp:BoundField>

                    <asp:BoundField DataField="Location" HeaderText="Location" SortExpression="Location" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Grower" SortExpression="Grower" HeaderText="Grower" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Commodity" SortExpression="Commodity" HeaderText="Commodity" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>

                    <asp:BoundField DataField="WS_Id" SortExpression="WS_Id" HeaderText="Weight Sheet" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Creation_Date" ItemStyle-CssClass=" text-left " HeaderText="Created" SortExpression="Creation_Date" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle CssClass=" text-left " />
                    </asp:BoundField>
                    <asp:CheckBoxField DataField="Closed" HeaderText="Closed" SortExpression="Closed" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:CheckBoxField>
                    <asp:CheckBoxField DataField="Original_Printed" HeaderText="Original Printed" SortExpression="Original_Printed" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:CheckBoxField>
                    <asp:BoundField DataField="Lot" HeaderText="Lot" SortExpression="Lot" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                </Columns>
                <EmptyDataTemplate>
                    .
                         <h5 class="text-center">No Weightsheets Matching Filter</h5>

                </EmptyDataTemplate>
            </asp:GridView>
            <asp:SqlDataSource runat="server" ID="sqlWeightSheets" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT 'Details' AS btnDetails, Weight_Sheets.UID, Weight_Sheets.WS_Id, Weight_Sheets.Creation_Date, Weight_Sheets.Closed, Weight_Sheets.Location_Id, Locations.Description AS Location_Name, Weight_Sheets.Server_Name, Weight_Sheets.Sequence_ID, Lots.Lot_Number AS Lot, CASE WHEN Weight_Inbound_Loads.uid IS NULL THEN 'Transfer' ELSE 'Intake' END AS WSType, Locations.Description + N' - ' + LTRIM(STR(Locations.Id)) AS Location, Weight_Sheets.Original_Printed, ISNULL(Producers.Description + N' - ' + LTRIM(STR(Producers.Id)), N'') AS Grower, Crops.Description AS Commodity FROM Producers INNER JOIN Lots INNER JOIN Weight_Inbound_Loads ON Lots.UID = Weight_Inbound_Loads.Lot_UID ON Producers.Id = Lots.Producer_Id INNER JOIN Crops ON Lots.Crop_Id = Crops.Id RIGHT OUTER JOIN Weight_Sheets LEFT OUTER JOIN Locations ON Weight_Sheets.Location_Id = Locations.Id ON Weight_Inbound_Loads.Weight_Sheet_UID = Weight_Sheets.UID WHERE (DATEDIFF(day, @StartDate, Weight_Sheets.Creation_Date) >= 0) AND (@WS_Id IS NULL) AND (DATEDIFF(day, Weight_Sheets.Creation_Date, @EndDate) >= 0) AND (@WS_Id IS NULL) AND (Weight_Sheets.Location_Id = ISNULL(@Location_Id, Weight_Sheets.Location_Id)) AND (CASE WHEN Weight_Inbound_Loads.uid IS NULL THEN 'Transfer' ELSE 'Intake' END = ISNULL(@Type, CASE WHEN Weight_Inbound_Loads.uid IS NULL THEN 'Transfer' ELSE 'Intake' END)) AND (Weight_Sheets.Closed = ISNULL(@Closed, Weight_Sheets.Closed)) AND (ISNULL(Producers.Description + N' - ' + LTRIM(STR(Producers.Id)), N'') LIKE N'%' + ISNULL(@Grower, ISNULL(Producers.Description + N' - ' + LTRIM(STR(Producers.Id)), N'')) + N'%') AND (Crops.Description = ISNULL(@Commodity, Crops.Description)) OR (Weight_Sheets.WS_Id = @WS_Id) ORDER BY Weight_Sheets.Creation_Date, Location" CancelSelectOnNullParameter="False">
                <SelectParameters>
                    <asp:ControlParameter ControlID="hfStart" Name="StartDate" PropertyName="Value" />
                    <asp:ControlParameter ControlID="txtWSID" PropertyName="Text" Name="WS_Id"></asp:ControlParameter>

                    <asp:ControlParameter ControlID="hfEnd" Name="EndDate" PropertyName="Value" />
                    <asp:ControlParameter ControlID="cboLocation" Name="Location_Id" PropertyName="SelectedValue" />
                    <asp:ControlParameter ControlID="ddType" PropertyName="SelectedValue" Name="Type"></asp:ControlParameter>

                    <asp:ControlParameter ControlID="ddOpenClosed" PropertyName="SelectedValue" Name="Closed"></asp:ControlParameter>
                    <asp:ControlParameter ControlID="txtGrower" Name="Grower" PropertyName="Text" />
                    <asp:ControlParameter ControlID="ddCommodity" PropertyName="SelectedValue" Name="Commodity"></asp:ControlParameter>
                </SelectParameters>
            </asp:SqlDataSource>

            <asp:HiddenField ID="hfStart" runat="server" />
            <asp:HiddenField ID="hfEnd" runat="server" />

        </ContentTemplate>

    </asp:UpdatePanel>

</asp:Content>

