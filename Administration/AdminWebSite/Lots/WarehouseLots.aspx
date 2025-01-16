<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="WarehouseLots.aspx.cs" Inherits="Lots_WarehouseLots" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <h3>Getting Data.</h3>
        </ProgressTemplate>
    </asp:UpdateProgress>
       <asp:UpdateProgress runat="server" ID="UPr">
        <ProgressTemplate >
            <%--<h2 class="text-info text-center  ">Getting Data</h2>--%>
         </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel runat="server" ID="UP1">
        <ContentTemplate>



            <h3>
                <asp:Label ID="lblHeader" runat="server" Text="Lots"></asp:Label></h3>

            <table style="width: 100%" class="text-center ">
                <tr>
                    <td colspan="6" class="text-center ">Find By Lot Number
                    </td>

                </tr>
                <tr>
                    <asp:PlaceHolder ID="NewLotPlaceholder" runat="server" Visible="False">
                    <td colspan="6" class="text-center ">
                        <asp:LinkButton ID="lnkCreateNewLot" runat="server">Create New Lot</asp:LinkButton>
                    </td>
                    </asp:PlaceHolder>
                </tr>
                <tr>
                    <td colspan="6" class="text-center ">
                        <asp:TextBox runat="server" CssClass="input-sm " ID="txtWSID" AutoPostBack="True"></asp:TextBox>
                        <ajaxToolkit:FilteredTextBoxExtender runat="server" BehaviorID="txtWSID_FilteredTextBoxExtender" TargetControlID="txtWSID" ID="txtWSID_FilteredTextBoxExtender" ValidChars="0123456789"></ajaxToolkit:FilteredTextBoxExtender>
                    </td>

                </tr>
                <tr>
                    <td></td>
                    <td>Crop
                    </td>
                    <td>Variety
                    </td>
                    <td>Location
                    </td>
                  
                    <td>From
                    </td>
                    <td>To
                    </td>

                </tr>
                <tr>
                    <td>

                        <asp:DropDownList runat="server" ID="ddOpenClosed" CssClass="input-sm " DataTextField="Text" AutoPostBack="True" DataSourceID="sqlOpenClosed" DataValueField="value"></asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="sqlOpenClosed" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT 'Open/Closed' AS Text, NULL AS value, 0 AS idx UNION SELECT 'Open' AS Text, CONVERT (bit, 0) AS value, 1 AS idx UNION SELECT 'Closed' AS Text, CONVERT (bit, 1) AS value, 2 AS idx ORDER BY idx"></asp:SqlDataSource>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddCrop" CssClass="input-sm " DataTextField="Text" AutoPostBack="True" DataSourceID="sqlCrop" DataValueField="value"></asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="sqlCrop" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT 'All Crops' AS Text, NULL AS value, 0 AS idx UNION SELECT Description + ' - ' + LTRIM(STR(Id)) AS text, Id AS value, 1 AS idx FROM Crops ORDER BY Idx, Text"></asp:SqlDataSource>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddVariety" CssClass="input-sm " DataTextField="Text" AutoPostBack="True" DataSourceID="sqlVariety" DataValueField="value" ></asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="sqlVariety" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT 'All Varieties' AS Text, NULL AS value, 0 AS idx UNION SELECT DISTINCT Variety + ' - ' + LTRIM(STR(Item_Id)) AS text, Item_Id AS value, 1 AS idx FROM VarietyList WHERE (Crop_Id = @Crop) ORDER BY Idx, Text" CancelSelectOnNullParameter="False">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ddCrop" PropertyName="SelectedValue" Name="Crop"></asp:ControlParameter>
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="cboLocation" CssClass="input-sm " DataSourceID="sqlLocation" DataTextField="Text" DataValueField="Id" AutoPostBack="True" OnTextChanged="cboLocation_TextChanged"></asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="sqlLocation" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT 'All Locations' AS Text, null AS Id, 0 AS idx UNION SELECT Description + ' - ' + LTRIM(STR(Id)) AS Text, Id, 1 AS idx FROM Locations ORDER BY idx, Text"></asp:SqlDataSource>
                    </td>


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


            <hr />

            <br />



            <asp:GridView ID="GridView1" CssClass="table table-hover table-responsive  " HorizontalAlign="Center" runat="server" DataSourceID="sqlLots" AutoGenerateColumns="False" AllowSorting="True" DataKeyNames="UID" AllowPaging="True" PageSize="50">
                <Columns>
                    <asp:TemplateField >

                        <ItemTemplate>
                            <asp:LinkButton ID="lnkSelect"  CssClass="btn btn-primary "  OnClick="lnkSelect_Click"  runat="server">Select</asp:LinkButton>

                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="btnDetails">

                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDetails"  CssClass="btn btn-primary "  OnClick="lnkDetails_Click" runat="server">Details</asp:LinkButton>

                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Lot" SortExpression="Lot" HeaderText="Lot" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>

                    <asp:BoundField DataField="Start_Date" HeaderText="Created" SortExpression="Start_Date" DataFormatString="{0:d}"></asp:BoundField>
                    <asp:BoundField DataField="Crop_Id" HeaderText="Crop Id" SortExpression="Crop_Id"></asp:BoundField>
                    <asp:BoundField DataField="Crop" HeaderText="Crop" SortExpression="Crop"></asp:BoundField>
                    <asp:BoundField DataField="Variety" HeaderText="Variety" SortExpression="Variety"></asp:BoundField>


                    <asp:BoundField DataField="Producer_Description" HeaderText="Producer" SortExpression="Producer_Description" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Location" HeaderText="Location" ReadOnly="True" SortExpression="Location"></asp:BoundField>

                    <asp:BoundField DataField="Close_Date" HeaderText="Closed" SortExpression="Close_Date" DataFormatString="{0:d}"></asp:BoundField>
                    <asp:CheckBoxField DataField="Lot_Sampled" HeaderText="Sampled" SortExpression="Lot_Sampled"></asp:CheckBoxField>
                </Columns>
                <EmptyDataTemplate>
                    .
                         <h5 class="text-center">No Lots Matching Filter</h5>

                </EmptyDataTemplate>
            </asp:GridView>
            <asp:SqlDataSource runat="server" ID="sqlLots" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT 'Details' AS btnDetails, Lots.UID, Lots.Start_Date, Crops.Description AS Crop, Lots.Producer_Id, Lots.Producer_Description, Lots.Location_Id, Locations.Description AS Location_Name, CASE WHEN Close_Date IS NULL THEN CONVERT (bit , 0) ELSE CONVERT (bit , 1) END AS Closed, Lots.Server_Name, Lots.Sequence_ID, Lots.Lot_Number AS Lot, Locations.Description + N' - ' + LTRIM(STR(Locations.Id)) AS Location, Lots.Landlord, Lots.Close_Date, Lots.Lot_Sampled, tblVariety.Variety_Id, tblVariety.Variety, tblVariety.Full_Description, Lots.Crop_Id FROM Locations INNER JOIN Lots ON Locations.Id = Lots.Location_Id LEFT OUTER JOIN (SELECT Item_Id AS Variety_Id, Variety, Variety + N' - ' + LTRIM(STR(Item_Id)) AS Full_Description, Crop_Id FROM VarietyList) AS tblVariety ON Lots.Crop_Id = tblVariety.Crop_Id AND Lots.Variety_Id = tblVariety.Variety_Id LEFT OUTER JOIN Crops ON Lots.Crop_Id = Crops.Id WHERE (@Lot_Id IS NULL) AND (DATEDIFF(day, @StartDate, Lots.Start_Date) &gt;= 0) AND (DATEDIFF(day, Lots.Start_Date, @EndDate) &gt;= 0) AND (Crops.Id = ISNULL(@crop_Id, Crops.Id)) AND (CASE WHEN Close_Date IS NULL THEN CONVERT (bit , 0) ELSE CONVERT (bit , 1) END = ISNULL(@Closed, CASE WHEN Close_Date IS NULL THEN CONVERT (bit , 0) ELSE CONVERT (bit , 1) END)) AND (tblVariety.Variety_Id = ISNULL(@Variety_Id, tblVariety.Variety_Id)) OR (Lots.Lot_Number = @Lot_Id) ORDER BY Lots.Start_Date, Location" CancelSelectOnNullParameter="False">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtWSID" Name="Lot_Id" PropertyName="Text" />
                    <asp:ControlParameter ControlID="hfStart" PropertyName="Value" Name="StartDate"></asp:ControlParameter>

                    <asp:ControlParameter ControlID="hfEnd" Name="EndDate" PropertyName="Value" />
                    <asp:ControlParameter ControlID="ddCrop" Name="crop_Id" PropertyName="SelectedValue" />
                    <asp:ControlParameter ControlID="ddOpenClosed" PropertyName="SelectedValue" Name="Closed"></asp:ControlParameter>
                    <asp:ControlParameter ControlID="ddVariety" Name="Variety_Id" PropertyName="SelectedValue" />
                </SelectParameters>
            </asp:SqlDataSource>

            <asp:HiddenField ID="hfStart" runat="server" />
            <asp:HiddenField ID="hfEnd" runat="server" />

        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentFull" Runat="Server">
</asp:Content>

