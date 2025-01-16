<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="IntakeWeightSheets.aspx.cs" Inherits="IntakeWeightSheets" %>

<asp:Content ID="Content0" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdateProgress runat="server" ID="UPr">
        <ProgressTemplate>


            <h2 style="width: 100%" class="text-info text-center  ">Hang On... Getting Data</h2>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>




<asp:Content ID="Content1" ContentPlaceHolderID="MainContentFull" runat="Server">
                           <style>
       .spinner {
           display: none;
           position: fixed;
           z-index: 999;
           top: 20%;
           left: 50%;
           width: 50px;
           height: 50px;
           margin: -25px 0 0 -25px;
           border: 8px solid #f3f3f3;
           border-radius: 50%;
           border-top: 8px solid #3498db;
           animation: spin 2s linear infinite;
       }

       @keyframes spin {
           0% {
               transform: rotate(0deg);
           }

           100% {
               transform: rotate(360deg);
           }
       }
   </style>
 <asp:UpdateProgress runat="server" ID="UpdateProgress1">
    <ProgressTemplate>
       
    
    </ProgressTemplate>
</asp:UpdateProgress>
    <div class="row">
        <div class="col-12">
            <h3><asp:Label ID="lblHeader" runat="server" Text="Intake Weightsheets"></asp:Label></h3>
             <div class="spinner" id="spinner"></div>
        </div>
        <div class="col-12">
            <!-- Add align-items-center and justify-content-center to center the buttons both vertically and horizontally -->
            <div class="form-row d-flex align-items-center justify-content-center">
                <asp:Button ID="btnExcel" runat="server" CssClass="btn btn-sm btn-success mx-2" Text="Excel" Width="100px" ClientIDMode="Static" OnClick="btnExcel_Click" />
                <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-sm btn-primary mx-2" Text="Print" Width="100px" ClientIDMode="Static" OnClick="btnPrint_Click" />
                <asp:Button ID="btnEmail" runat="server" ClientIDMode="Static" CssClass="btn btn-sm btn-secondary mx-2" Text="Re-Email" Width="100px" OnClick="btnEmail_Click" />
            </div>
        </div>

        <div class="col-12">


            <asp:UpdatePanel runat="server" ID="UP1">
                <ContentTemplate>

   
                    <table style="width: 100%" class="text-center ">

                        <tr>
                            <td></td>
                            <td></td>
                            <td style="width: 150px" class="text-center ">Find By Weight Sheet
                            </td>
                            <td style="width: 150px" class="text-center ">Find By Lot
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td style="width: 150px" class="text-center ">
                                <asp:TextBox runat="server" CssClass="input-sm " ID="txtWSID" AutoPostBack="True"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender runat="server" BehaviorID="txtWSID_FilteredTextBoxExtender" TargetControlID="txtWSID" ID="txtWSID_FilteredTextBoxExtender" ValidChars="0123456789"></ajaxToolkit:FilteredTextBoxExtender>
                            </td>
                            <td style="width: 150px" class="text-center ">
                                <asp:TextBox runat="server" CssClass="input-sm " ID="txtLot" OnTextChanged="txtLot_TextChanged" AutoPostBack="True"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender runat="server" BehaviorID="txtLot_FilteredTextBoxExtender" TargetControlID="txtLot" ID="txtLot_FilteredTextBoxExtender" ValidChars="0123456789"></ajaxToolkit:FilteredTextBoxExtender>

                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                    <table>


                        <table style="width: 100%" class="text-center ">
                            <tr>
                                <td></td>
                                <td></td>
                                <td style="width: 150px" class="text-center ">From
                                </td>
                                <td style="width: 150px" class="text-center ">To
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td style="width: 150px" class="text-center ">
                                    <asp:TextBox runat="server" CssClass="input-sm " ID="txtStartDate" AutoPostBack="True" OnTextChanged="txtStartDate_TextChanged"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender runat="server" BehaviorID="txtStartDate_CalendarExtender" TargetControlID="txtStartDate" ID="txtStartDate_CalendarExtender"></ajaxToolkit:CalendarExtender>
                                </td>
                                <td style="width: 150px" class="text-center ">
                                    <asp:TextBox runat="server" CssClass="input-sm " ID="txtEndDate" AutoPostBack="True" OnTextChanged="txtEndDate_TextChanged"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender runat="server" BehaviorID="txtEndDate_CalendarExtender" TargetControlID="txtEndDate" ID="txtEndDate_CalendarExtender"></ajaxToolkit:CalendarExtender>

                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                        </table>

                        <table>


                            <tr>
                                <td>Open/Closed</td>
                                <td>Producer</td>
                                <td>Crop</td>
                                <td>Location</td>
                                <td>Hauler</td>

                            </tr>

                            <tr>

                                <td>
                                    <asp:DropDownList runat="server" ID="ddOpenClosed" CssClass="input-sm " DataTextField="Text" AutoPostBack="True" DataSourceID="sqlOpenClosed" DataValueField="value" OnTextChanged="ddOpenClosed_TextChanged"></asp:DropDownList>
                                    <asp:SqlDataSource runat="server" ID="sqlOpenClosed" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT 'Open/Closed' AS Text, NULL AS value, 1 AS idx UNION SELECT 'Open' AS Text, CONVERT (bit, 0) AS value, 2 AS idx UNION SELECT 'Closed' AS Text, CONVERT (bit, 1) AS value, 0 AS idx ORDER BY idx"></asp:SqlDataSource>
                                </td>

                                <td>

                                    <asp:DropDownList runat="server" ID="ddProducer" CssClass="input-sm " DataTextField="Text" AutoPostBack="True" DataSourceID="sqlProducer" DataValueField="value" OnTextChanged="ddProducer_TextChanged"></asp:DropDownList>
                                    <asp:SqlDataSource runat="server" ID="sqlProducer" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT        'All Producers' AS Text, NULL AS value, 0 AS idx UNION SELECT DISTINCT Lots.Producer_Description + N' - ' + LTRIM(STR(Lots.Producer_Id)) AS Text, Lots.Producer_Id AS Value, 1 AS idx FROM Lots INNER JOIN Weight_Inbound_Loads ON Lots.UID = Weight_Inbound_Loads.Lot_UID INNER JOIN Weight_Sheets ON Weight_Inbound_Loads.Weight_Sheet_UID = Weight_Sheets.UID WHERE (DATEDIFF(day, @StartDate, Weight_Sheets.Creation_Date) >= 0) AND (DATEDIFF(day, Weight_Sheets.Creation_Date, @EndDate) >= 0) ORDER BY idx, Text">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="txtStartDate" PropertyName="Text" Name="StartDate"></asp:ControlParameter>
                                            <asp:ControlParameter ControlID="txtEndDate" PropertyName="Text" Name="EndDate"></asp:ControlParameter>
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </td>
                                <td class="text-center ">
                                    <asp:DropDownList ID="ddCrop" CssClass="input-sm " AutoPostBack="true" runat="server" OnTextChanged="ddCrop_TextChanged" DataSourceID="sqlCrop" DataTextField="Text" DataValueField="Id"></asp:DropDownList>
                                    <asp:SqlDataSource runat="server" ID="sqlCrop" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT 'All Crops' AS Text, NULL AS Id, 0 AS idx UNION SELECT Description + ' - ' + LTRIM(STR(Id)) AS Text, Id, 1 AS idx FROM Crops ORDER BY idx" CancelSelectOnNullParameter="False"></asp:SqlDataSource>
                                </td>

                                <td>
                                    <asp:DropDownList runat="server" ID="cboLocation" CssClass="input-sm " DataSourceID="sqlLocation" DataTextField="Text" DataValueField="Id" AutoPostBack="True" OnTextChanged="cboLocation_TextChanged"></asp:DropDownList>
                                    <asp:SqlDataSource runat="server" ID="sqlLocation" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT 'All Locations' AS Text, NULL AS Id, 0 AS idx UNION SELECT Description + ' - ' + LTRIM(STR(Id)) AS Text, Description AS id, 1 AS idx FROM Locations ORDER BY idx, Text"></asp:SqlDataSource>

                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddHauler" CssClass="input-sm " Width="250px" DataTextField="Text" AutoPostBack="True" DataSourceID="sqlHauler" DataValueField="Value" OnTextChanged="ddHauler_TextChanged"></asp:DropDownList>
                                    <asp:SqlDataSource runat="server" ID="sqlHauler" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT '' AS text , NULL AS value ,0 AS idx UNION SELECT DISTINCT CASE WHEN Carriers.UID IS NULL THEN '' ELSE Carriers.Description END AS Text, CASE WHEN Carriers.UID IS NULL THEN NULL ELSE Carriers.Description END AS Value, 1 AS idx FROM Weight_Sheets INNER JOIN Carriers ON Weight_Sheets.Carrier_Id = Carriers.Id WHERE (DATEDIFF(day, @StartDate, Weight_Sheets.Creation_Date) &gt;= 0) AND (DATEDIFF(day, Weight_Sheets.Creation_Date, @EndDate) &gt;= 0) ORDER BY idx, text">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="txtStartDate" PropertyName="Text" Name="StartDate"></asp:ControlParameter>
                                            <asp:ControlParameter ControlID="txtEndDate" PropertyName="Text" Name="EndDate"></asp:ControlParameter>
                                        </SelectParameters>
                                    </asp:SqlDataSource>

                                </td>
                            </tr>

                        </table>


                        <hr />

                        <br />



                        <asp:GridView ID="GridView1" CssClass="table table-hover table-responsive  " Width="100%" HorizontalAlign="Center" runat="server" DataSourceID="sqlWeightSheets" AutoGenerateColumns="False" AllowSorting="True" DataKeyNames="UID" PageSize="50">
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="" SortExpression="btnDetails">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDetails" CssClass="btn btn-outline-dark  " runat="server" OnClick="lnkDetails_Click" Text='<%# Eval("btnDetails") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle CssClass=" text-left " />
                                </asp:TemplateField>

                                <asp:BoundField DataField="WS_Id" HeaderText="Weight Sheet" SortExpression="WS_Id" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Creation_Date" ItemStyle-CssClass=" text-left " HeaderText="Date" SortExpression="Creation_Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:d}">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle CssClass=" text-left " HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:CheckBoxField DataField="Closed" HeaderText="Closed" SortExpression="Closed" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:CheckBoxField>
                                <asp:CheckBoxField DataField="Original_Printed" HeaderText="Original Printed" SortExpression="Original_Printed" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:CheckBoxField>

                                <asp:TemplateField HeaderText="Lot" SortExpression="Lot">

                                    <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" Text='<%# Bind("Lot") %>' NavigateUrl='<%#"~/Lots/LotDetails.aspx?UID=" + Eval("LotUID") %>' Target="_blank" runat="server"></asp:HyperLink>

                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Location" HeaderText="Location" SortExpression="Location" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ReadOnly="True">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Crop" HeaderText="Crop" ReadOnly="True" SortExpression="Crop">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Variety" HeaderText="Variety" ReadOnly="True" SortExpression="Variety">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Producer" HeaderText="Producer" ReadOnly="True" SortExpression="Producer">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>

                                <asp:BoundField DataField="Hauler" HeaderText="Hauler" SortExpression="Hauler" />

                                <asp:BoundField DataField="Net" HeaderText="Net" SortExpression="Net">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Miles" HeaderText="Miles" SortExpression="Miles">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Rate" HeaderText="Rate" SortExpression="Rate">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Custom_Rate" HeaderText="Custom Rate" SortExpression="Custom_Rate">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                            </Columns>
                            <EmptyDataTemplate>


                                <h5 class="text-center">No Weightsheets Matching Filter</h5>

                            </EmptyDataTemplate>
                        </asp:GridView>
                        <asp:SqlDataSource runat="server" ID="sqlWeightSheets" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT 'Details' AS btnDetails, Weight_Sheets.UID, Weight_Sheets.WS_Id, Weight_Sheets.Creation_Date, Weight_Sheets.Closed, Weight_Sheets.Location_Id, Locations.Description AS Location_Name, Weight_Sheets.Sequence_ID, Lots.Lot_Number AS Lot, Locations.Description + N' - ' + LTRIM(STR(Locations.Id)) AS Location, Lots.Producer_Id, Lots.Producer_Description + N' - ' + LTRIM(STR(Lots.Producer_Id)) AS Producer, Crops.Id AS Crop_Id, Crops.Description + N' - ' + LTRIM(STR(Crops.Id)) AS Crop, Lots.UID AS LotUID, Weight_Sheets.Original_Printed, vwWeightSheet_Net.Net, ISNULL(Carriers.Description, N'') AS Hauler, Weight_Sheets.Carrier_Id AS Hauler_Id, ISNULL(Crop_Varieties.Description, N'') AS Variety, ISNULL(Weight_Sheets.Miles, 0) AS Miles, ISNULL(Weight_Sheets.Rate, 0) AS Rate, ISNULL(Weight_Sheets.Custom_Hauler_Rate, 0) AS Custom_Rate FROM Crop_Varieties RIGHT OUTER JOIN Lots INNER JOIN Weight_Inbound_Loads ON Lots.UID = Weight_Inbound_Loads.Lot_UID INNER JOIN Crops ON Lots.Crop_Id = Crops.Id ON Crop_Varieties.Item_Id = Lots.Variety_Id RIGHT OUTER JOIN vwWeightSheet_Net RIGHT OUTER JOIN Weight_Sheets LEFT OUTER JOIN Carriers ON Weight_Sheets.Carrier_Id = Carriers.Id ON vwWeightSheet_Net.WeightSheetUID = Weight_Sheets.UID LEFT OUTER JOIN Locations ON Weight_Sheets.Location_Id = Locations.Id ON Weight_Inbound_Loads.Weight_Sheet_UID = Weight_Sheets.UID WHERE (DATEDIFF(day, @StartDate, Weight_Sheets.Creation_Date) &gt;= 0) AND (@WS_Id IS NULL) AND (DATEDIFF(day, Weight_Sheets.Creation_Date, @EndDate) &gt;= 0) AND (Weight_Sheets.Closed = ISNULL(@Closed, Weight_Sheets.Closed)) AND (Lots.Producer_Id = ISNULL(@Producer_Id, Lots.Producer_Id)) AND (Crops.Id = ISNULL(@Crop, Crops.Id)) AND (@LotNumber IS NULL) AND (Locations.Description = ISNULL(@Location_Id, Locations.Description)) AND (ISNULL(Carriers.Description, N'') = ISNULL(@HaulerName, ISNULL(Carriers.Description, N''))) OR (Weight_Sheets.WS_Id = @WS_Id) OR (Lots.Lot_Number = @LotNumber) ORDER BY Weight_Sheets.Creation_Date, Location, Weight_Sheets.WS_Id" CancelSelectOnNullParameter="False">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hfStart" Name="StartDate" PropertyName="Value" />
                                <asp:ControlParameter ControlID="txtWSID" PropertyName="Text" Name="WS_Id"></asp:ControlParameter>
                                <asp:ControlParameter ControlID="hfEnd" PropertyName="Value" Name="EndDate"></asp:ControlParameter>
                                <asp:ControlParameter ControlID="ddOpenClosed" Name="Closed" PropertyName="SelectedValue" />
                                <asp:ControlParameter ControlID="ddProducer" PropertyName="SelectedValue" Name="Producer_Id"></asp:ControlParameter>
                                <asp:ControlParameter ControlID="ddCrop" PropertyName="SelectedValue" Name="Crop"></asp:ControlParameter>

                                <asp:ControlParameter ControlID="txtLot" PropertyName="Text" Name="LotNumber"></asp:ControlParameter>
                                <asp:ControlParameter ControlID="cboLocation" PropertyName="SelectedValue" Name="Location_Id"></asp:ControlParameter>
                                <asp:ControlParameter ControlID="ddHauler" Name="HaulerName" PropertyName="SelectedValue" />




                            </SelectParameters>
                        </asp:SqlDataSource>

                        <asp:HiddenField ID="hfStart" runat="server" />
                        <asp:HiddenField ID="hfEnd" runat="server" />
                </ContentTemplate>

            </asp:UpdatePanel>
        </div>
    </div>


</asp:Content>

