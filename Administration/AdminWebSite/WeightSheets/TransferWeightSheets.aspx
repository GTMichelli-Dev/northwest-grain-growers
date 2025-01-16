<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="TransferWeightSheets.aspx.cs" Inherits="TransferWeightSheets" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
 <%--    <asp:UpdateProgress runat="server" ID="UpdateProgress2">
        <ProgressTemplate>
            <div class="row">
                <div class="col-12">

                    <h2 style="width: 100%" class="text-info text-center  ">Hang On... Getting Data</h2>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>--%>
    <div class="row">
        <div class="col-12">
            <h3>
                <asp:Label ID="lblHeader" runat="server" Text="Transfer Weightsheets"></asp:Label></h3>
           
        </div>



        <div class="col-12">
            <!-- Add align-items-center and justify-content-center to center the buttons both vertically and horizontally -->
            <div class="form-row d-flex align-items-center justify-content-center">
                <asp:Button ID="btnExcel" runat="server" CssClass="btn btn-sm btn-success mx-2" Text="Excel" Width="100px" ClientIDMode="Static" OnClick="btnExcel_Click" />
                <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-sm btn-primary mx-2" Text="Print" Width="100px" ClientIDMode="Static" OnClick="btnPrint_Click" />
            
            </div>
        </div>



    </div>



    <asp:UpdatePanel runat="server" ID="UP1">
        <ContentTemplate>




            <table style="width: 100%" class="text-center ">
                <tr>
                    <td></td>
                    <td></td>
                    <td style="width: 150px" class="text-center ">Find By Weight Sheet
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td style="width: 150px" class="text-center ">
                        <asp:TextBox runat="server" CssClass="form-control " ID="txtWSID" AutoPostBack="True"></asp:TextBox>
                        <ajaxToolkit:FilteredTextBoxExtender runat="server" BehaviorID="txtWSID_FilteredTextBoxExtender" TargetControlID="txtWSID" ID="txtWSID_FilteredTextBoxExtender" ValidChars="0123456789"></ajaxToolkit:FilteredTextBoxExtender>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
            </table>


            <asp:Table runat="server" HorizontalAlign="Center">
                <asp:TableRow>
                    <asp:TableCell>
                           Open/Closed
                    </asp:TableCell>
                    <asp:TableCell>
                           Location
                    </asp:TableCell>
                    <asp:TableCell>
                           Source
                    </asp:TableCell>
                    <asp:TableCell>
                           Crop
                    </asp:TableCell>
                    <asp:TableCell>
                           Hauler
                    </asp:TableCell>

                    <asp:TableCell>
                           From
                    </asp:TableCell>
                    <asp:TableCell>
                           To
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:DropDownList ID="ddOpenClosed" runat="server" AutoPostBack="True" CssClass="form-control " DataSourceID="sqlOpenClosed" DataTextField="Text" DataValueField="value" OnTextChanged="ddOpenClosed_TextChanged">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="sqlOpenClosed" runat="server" ConnectionString="<%$ ConnectionStrings:NW_DataConnectionString %>" SelectCommand="SELECT 'Open/Closed' AS Text, NULL AS value, 1 AS idx UNION SELECT 'Open' AS Text, CONVERT (bit, 0) AS value, 2 AS idx UNION SELECT 'Closed' AS Text, CONVERT (bit, 1) AS value, 0 AS idx ORDER BY idx"></asp:SqlDataSource>

                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList runat="server" ID="cboLocation" CssClass="form-control " DataSourceID="sqlLocation" DataTextField="Text" DataValueField="Id" AutoPostBack="True" OnTextChanged="cboLocation_TextChanged"></asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="sqlLocation" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT 'All Locations' AS Text, NULL AS Id, 0 AS idx UNION SELECT Description + ' - ' + LTRIM(STR(Id)) AS Text, Description AS id, 1 AS idx FROM Locations ORDER BY idx, Text"></asp:SqlDataSource>

                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList runat="server" ID="ddSource" CssClass="form-control " DataTextField="Text" DataValueField="Id" AutoPostBack="True" DataSourceID="sqlLocation" OnTextChanged="ddSource_TextChanged"></asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT 'All Locations' AS Text, NULL AS Id, 0 AS idx UNION SELECT Description + ' - ' + LTRIM(STR(Id)) AS Text, Description AS id, 1 AS idx FROM Locations ORDER BY idx, Text"></asp:SqlDataSource>

                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="ddCrop" runat="server" AutoPostBack="True" CssClass="form-control " DataSourceID="sqlCrop" DataTextField="Text" DataValueField="Id" OnTextChanged="ddCrop_TextChanged">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="sqlCrop" runat="server" CancelSelectOnNullParameter="False" ConnectionString="<%$ ConnectionStrings:NW_DataConnectionString %>" SelectCommand="SELECT 'All Crops' AS Text, NULL AS Id, 0 AS idx UNION SELECT Description + ' - ' + LTRIM(STR(Id)) AS Text, Id, 1 AS idx FROM Crops WHERE (Description NOT LIKE '%Undefined%') AND (Description NOT LIKE '%test%') ORDER BY idx, Text"></asp:SqlDataSource>

                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList runat="server" ID="ddHauler" CssClass="form-control " DataTextField="Text" AutoPostBack="True" DataSourceID="sqlHauler" DataValueField="Value" OnTextChanged="ddHauler_TextChanged"></asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="sqlHauler" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT 'All Haulers' AS text , NULL AS value ,0 AS idx UNION SELECT DISTINCT CASE WHEN Carriers.UID IS NULL THEN '' ELSE Carriers.Description END AS Text, CASE WHEN Carriers.UID IS NULL THEN NULL ELSE Carriers.Description END AS Value, 1 AS idx FROM Weight_Sheets INNER JOIN Carriers ON Weight_Sheets.Carrier_Id = Carriers.Id WHERE (DATEDIFF(day, @StartDate, Weight_Sheets.Creation_Date) &gt;= 0) AND (DATEDIFF(day, Weight_Sheets.Creation_Date, @EndDate) &gt;= 0) ORDER BY idx, text">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="txtStartDate" PropertyName="Text" Name="StartDate"></asp:ControlParameter>
                                <asp:ControlParameter ControlID="txtEndDate" PropertyName="Text" Name="EndDate"></asp:ControlParameter>
                            </SelectParameters>
                        </asp:SqlDataSource>

                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox runat="server" CssClass="form-control " ID="txtStartDate" AutoPostBack="True" OnTextChanged="txtStartDate_TextChanged"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender runat="server" BehaviorID="txtStartDate_CalendarExtender" TargetControlID="txtStartDate" ID="txtStartDate_CalendarExtender"></ajaxToolkit:CalendarExtender>

                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox runat="server" CssClass="form-control " ID="txtEndDate" AutoPostBack="True" OnTextChanged="txtEndDate_TextChanged"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender runat="server" BehaviorID="txtEndDate_CalendarExtender" TargetControlID="txtEndDate" ID="txtEndDate_CalendarExtender"></ajaxToolkit:CalendarExtender>

                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>




            <asp:GridView ID="GridView1" CssClass="table table-bordered table-hover table-sm " Width="100%" HorizontalAlign="Center" runat="server" DataSourceID="sqlWeightSheets" AutoGenerateColumns="False" AllowSorting="True" DataKeyNames="UID" AllowPaging="True" PageSize="50">
                <Columns>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="" SortExpression="btnDetails">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDetails" CssClass="btn btn-outline-dark " runat="server" OnClick="lnkDetails_Click" Text='<%# Eval("btnDetails") %>'></asp:LinkButton>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle CssClass=" text-left " />
                    </asp:TemplateField>
                    <asp:BoundField DataField="WS_Id" SortExpression="WS_Id" HeaderText="Weight Sheet" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Net" ItemStyle-CssClass=" text-right " HeaderText="Net" SortExpression="Net" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:N0}">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>

                    <asp:BoundField DataField="Location" HeaderText="Location" SortExpression="Location" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>


                    <asp:BoundField DataField="Source" ItemStyle-CssClass=" text-left " HeaderText="Source" SortExpression="Source" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle CssClass=" text-right "></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Hauler" ItemStyle-CssClass=" text-left " HeaderText="Hauler" SortExpression="Hauler" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle CssClass=" text-right "></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Creation_Date" HeaderText="Created" SortExpression="Creation_Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:d}">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" CssClass=" text-left " />
                    </asp:BoundField>
                    <asp:BoundField DataField="Crop" HeaderText="Crop" SortExpression="Crop">
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Variety" HeaderText="Variety" SortExpression="Variety"></asp:BoundField>
                    <asp:CheckBoxField DataField="Closed" HeaderText="Closed" SortExpression="Closed" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:CheckBoxField>
                    <asp:CheckBoxField DataField="Original_Printed" HeaderText="Original Printed" SortExpression="Original_Printed" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:CheckBoxField>
                </Columns>
                <EmptyDataTemplate>
                    .
                         <h5 class="text-center">No Weightsheets Matching Filter</h5>

                </EmptyDataTemplate>
            </asp:GridView>
            <asp:SqlDataSource runat="server" ID="sqlWeightSheets" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT 'Details' AS btnDetails, Weight_Sheets.UID, Weight_Sheets.WS_Id, Weight_Sheets.Creation_Date, Weight_Sheets.Closed, Weight_Sheets.Location_Id, Weight_Sheets.Server_Name, Weight_Sheets.Sequence_ID, Locations.Description + N' - ' + LTRIM(STR(Locations.Id)) AS Location, Source.Description + N' - ' + LTRIM(STR(Source.Id)) AS Source, Source.Id AS SourceId, Crops.Description AS Crop, VarietyList.Variety, Weight_Sheet_Transfer_Loads.Crop_Id, Weight_Sheets.Original_Printed, WeightSheet_Net.Net, Weight_Sheets.Carrier_Id, ISNULL(Carriers.Description, N'') AS Hauler FROM Locations AS Source INNER JOIN Weight_Sheets INNER JOIN Weight_Sheet_Transfer_Loads ON Weight_Sheets.UID = Weight_Sheet_Transfer_Loads.Weight_Sheet_UID INNER JOIN Crops ON Weight_Sheet_Transfer_Loads.Crop_Id = Crops.Id INNER JOIN Locations ON Weight_Sheets.Location_Id = Locations.Id ON Source.Id = Weight_Sheet_Transfer_Loads.Source_Id LEFT OUTER JOIN VarietyList ON Weight_Sheet_Transfer_Loads.Crop_Id = VarietyList.Crop_Id AND Weight_Sheet_Transfer_Loads.Variety_Id = VarietyList.Item_Id LEFT OUTER JOIN Carriers ON Weight_Sheets.Carrier_Id = Carriers.Id LEFT OUTER JOIN (SELECT Weight_Sheets_1.UID AS WeightSheetUID, SUM(ABS(Loads.Weight_In - Loads.Weight_Out)) AS Net FROM Weight_Sheets AS Weight_Sheets_1 INNER JOIN Weight_Sheet_Transfer_Loads AS Weight_Sheet_Transfer_Loads_1 ON Weight_Sheets_1.UID = Weight_Sheet_Transfer_Loads_1.Weight_Sheet_UID INNER JOIN Transfer_Loads ON Weight_Sheet_Transfer_Loads_1.UID = Transfer_Loads.Transfer_Load_UID INNER JOIN Loads ON Transfer_Loads.Load_UID = Loads.UID GROUP BY Weight_Sheets_1.UID) AS WeightSheet_Net ON Weight_Sheets.UID = WeightSheet_Net.WeightSheetUID WHERE (DATEDIFF(day, @StartDate, Weight_Sheets.Creation_Date) &gt;= 0) AND (@WS_Id IS NULL) AND (DATEDIFF(day, Weight_Sheets.Creation_Date, @EndDate) &gt;= 0) AND (@WS_Id IS NULL) AND (Weight_Sheets.Closed = ISNULL(@Closed, Weight_Sheets.Closed)) AND (Weight_Sheet_Transfer_Loads.Crop_Id = ISNULL(@Crop, Weight_Sheet_Transfer_Loads.Crop_Id)) AND (Source.Description = ISNULL(@Source_Id, Source.Description)) AND (Locations.Description = ISNULL(@Location_Id, Locations.Description)) AND (ISNULL(Carriers.Description, N'') = ISNULL(@HaulerName, ISNULL(Carriers.Description, N''))) OR (Weight_Sheets.WS_Id = @WS_Id) ORDER BY Weight_Sheets.Creation_Date, Location" CancelSelectOnNullParameter="False">
                <SelectParameters>
                    <asp:ControlParameter ControlID="hfStart" Name="StartDate" PropertyName="Value" />
                    <asp:ControlParameter ControlID="txtWSID" PropertyName="Text" Name="WS_Id"></asp:ControlParameter>

                    <asp:ControlParameter ControlID="hfEnd" Name="EndDate" PropertyName="Value" />
                    <asp:ControlParameter ControlID="ddOpenClosed" Name="Closed" PropertyName="SelectedValue" />
                    <asp:ControlParameter ControlID="ddCrop" PropertyName="SelectedValue" Name="Crop"></asp:ControlParameter>

                    <asp:ControlParameter ControlID="ddSource" Name="Source_Id" PropertyName="SelectedValue" />
                    <asp:ControlParameter ControlID="cboLocation" Name="Location_Id" PropertyName="SelectedValue" />
                    <asp:ControlParameter ControlID="ddHauler" Name="HaulerName" PropertyName="SelectedValue" />
                </SelectParameters>
            </asp:SqlDataSource>

            <asp:HiddenField ID="hfStart" runat="server" />
            <asp:HiddenField ID="hfEnd" runat="server" />

        </ContentTemplate>

    </asp:UpdatePanel>

</asp:Content>

