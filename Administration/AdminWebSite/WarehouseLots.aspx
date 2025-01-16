<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="WarehouseLots.aspx.cs" Inherits="WarehouseLots" %>
<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <h3>Getting Data.</h3>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h2>Warehouse Lots</h2>
            <div class="row">

                <div class="col-md-12 text-left">
                    <table>
                        <tr>
                            <td>
                                <label for="txtStartDate" class="text-left ">Start Date</label>
                                <asp:TextBox runat="server" ID="txtStartDate" CssClass=" form-control " AutoPostBack="true" OnTextChanged="txtStartDate_TextChanged"   ></asp:TextBox>
                                <ajaxToolkit:CalendarExtender runat="server" BehaviorID="txtStartDate_CalendarExtender" TargetControlID="txtStartDate" ID="txtStartDate_CalendarExtender"></ajaxToolkit:CalendarExtender>
                            </td>
                            <td>
                                <label for="txtEndDate" class="text-left ">End Date</label>
                                <asp:TextBox runat="server" ID="txtEndDate" CssClass=" form-control " AutoPostBack="true" OnTextChanged="txtEndDate_TextChanged"   ></asp:TextBox>
                                <ajaxToolkit:CalendarExtender runat="server" BehaviorID="txtEndDate_CalendarExtender" TargetControlID="txtEndDate" ID="txtEndDate_CalendarExtender"></ajaxToolkit:CalendarExtender>
     
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="ddLocation" class="text-left ">Locations</label>
                                <asp:DropDownList ID="ddLocation" CssClass="form-control dropdown  " runat="server" DataSourceID="sqlLocations" DataTextField="Location" DataValueField="Id" AutoPostBack="True" OnSelectedIndexChanged="ddLocation_SelectedIndexChanged" >
                                </asp:DropDownList>
                            </td>
                            <td>
                                <label for="ddClosed" class="text-left ">Open/Closed</label>
                                <asp:DropDownList ID="ddClosed" CssClass="form-control dropdown  " runat="server" AutoPostBack="True" DataSourceID="sqlClosed" DataTextField="text" DataValueField="value">
                                </asp:DropDownList>
                                <asp:SqlDataSource runat="server" ID="sqlClosed" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT 'Open' AS text, convert(bit,0) AS value, 1 AS idx UNION SELECT 'Closed' AS text, convert(bit,1) AS value, 1 AS idx UNION SELECT 'Open/Closed' AS text, NULL AS value, 0 AS idx ORDER BY idx, text"></asp:SqlDataSource>
                            </td>
                            <td>
                                <label for="ddProducer" class="text-left ">Producer</label>
                                <asp:DropDownList ID="ddProducer" CssClass="form-control dropdown  " runat="server" DataSourceID="sqlProducer" DataTextField="text" DataValueField="value" AutoPostBack="True" >
                                </asp:DropDownList>
                                <asp:SqlDataSource runat="server" ID="sqlProducer" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT Producer_Description AS text, Producer_Description AS value, 1 AS idx FROM Lots WHERE (Location_Id = @Location_Id) UNION SELECT 'All Producers' AS text, NULL AS value, 0 AS idx ORDER BY idx, text">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddLocation" Name="Location_Id" PropertyName="SelectedValue" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </td>
                            <td>
                                <label for="ddCrop" class="text-left ">Crop</label>
                                <asp:DropDownList ID="ddCrop" CssClass="form-control dropdown  " runat="server" DataSourceID="sqlCrop" DataTextField="text" DataValueField="value" AutoPostBack="True" OnSelectedIndexChanged="ddLocation_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:SqlDataSource runat="server" ID="sqlCrop" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT Crops.Description AS text, Crops.Description AS value, 1 AS idx FROM Crops INNER JOIN Lots ON Crops.Id = Lots.Crop_Id WHERE (Lots.Location_Id = @Location_Id) UNION SELECT 'All Crops' AS text, NULL AS value, 0 AS idx ORDER BY idx, text">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddLocation" Name="Location_Id" PropertyName="SelectedValue" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </td>
                            <td style="vertical-align: bottom">
                                &nbsp;</td>
                        </tr>
                    </table>

                </div>
                <%--  <div class="col-md-4">
                    
                </div>--%>
            </div>

            <hr />
            <br />

            <div class="row col-md-12">

                <asp:GridView ID="GridView1" CssClass="table table-hover table-responsive  " HorizontalAlign="Center" runat="server" DataSourceID="sqlLots" AutoGenerateColumns="False" AllowSorting="True">
                    <Columns>
                     
                        <asp:TemplateField HeaderText="Lot" SortExpression="Lot">

                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" PostBackUrl='<%#"~/WeightSheets.aspx?Lot="+ Eval("Lot")+"&Location_Id="+Eval("Location_Id") %>' Text='<%# Eval("Lot") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle CssClass="text-left " />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Status"  ItemStyle-CssClass="text-left " HeaderText="Status" ReadOnly="True" SortExpression="Status" >
                        <ItemStyle CssClass="text-left " />
                        </asp:BoundField>
                        <asp:BoundField DataField="Location" ItemStyle-CssClass="text-left " HeaderText="Location" SortExpression="Location" >
                        <ItemStyle CssClass="text-left " />
                        </asp:BoundField>
                        <asp:BoundField DataField="Producer"  ItemStyle-CssClass="text-left " HeaderText="Producer" SortExpression="Producer" >
                        <ItemStyle CssClass="text-left " />
                        </asp:BoundField>
                        <asp:BoundField DataField="Landlord"  ItemStyle-CssClass="text-left " HeaderText="Landlord" SortExpression="Landlord" >
                        <ItemStyle CssClass="text-left " />
                        </asp:BoundField>
                        <asp:BoundField DataField="Start_Date" ItemStyle-CssClass="text-left "  DataFormatString="{0:d}" HeaderText="Start" SortExpression="Start_Date" >
                        <ItemStyle CssClass="text-left " />
                        </asp:BoundField>
                        <asp:BoundField DataField="Close_Date" ItemStyle-CssClass="text-left "  DataFormatString="{0:d}" HeaderText="Closed" SortExpression="Close_Date" >
                        <ItemStyle CssClass="text-left " />
                        </asp:BoundField>
                        <asp:BoundField DataField="Crop"   ItemStyle-CssClass="text-left " HeaderText="Crop" SortExpression="Crop" >
                        <ItemStyle CssClass="text-left " />
                        </asp:BoundField>
                        <asp:BoundField DataField="Variety" ItemStyle-CssClass="text-left "  HeaderText="Variety" SortExpression="Variety" >
                        <ItemStyle CssClass="text-left " />
                        </asp:BoundField>
                        <asp:BoundField DataField="Comment" ItemStyle-CssClass="text-left "  HeaderText="Comment" SortExpression="Comment" >
                        <ItemStyle CssClass="text-left " />
                        </asp:BoundField>
                    </Columns>
                    <EmptyDataTemplate >
                       
                        <h5 class="text-center">No Lots Matching Filter</h5>
                       
                       
                        
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:SqlDataSource runat="server" ID="sqlLots" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT Location_Id, Lot, Status, Location, Producer, Landlord, Start_Date, Close_Date, Crop, Variety, Comment, Closed, Css FROM (SELECT Lots.Location_Id, Lots.Lot_Number AS Lot, CASE WHEN Close_Date IS NULL THEN 'Open' ELSE 'Closed' END AS Status, Locations.Description AS Location, Producers.Description AS Producer, Lots.Landlord, Lots.Start_Date, Lots.Close_Date, Crops.Description AS Crop, VarietyList.Variety, Lots.Comment, CASE WHEN Close_Date IS NULL THEN CONVERT (bit , 0) ELSE CONVERT (bit , 1) END AS Closed, CASE WHEN Close_Date IS NULL THEN 'text-success' ELSE 'text-danger' END AS Css FROM Lots INNER JOIN Producers ON Lots.Producer_Id = Producers.Id INNER JOIN Locations ON Lots.Location_Id = Locations.Id INNER JOIN Crops ON Lots.Crop_Id = Crops.Id LEFT OUTER JOIN VarietyList ON Lots.Variety_Id = VarietyList.Item_Id WHERE (Lots.Void = 0)) AS tblLots WHERE (Location_Id = ISNULL(@Location_Id, Location_Id)) AND (Closed = ISNULL(@Closed, Closed)) AND (Producer = ISNULL(@Producer, Producer)) AND (DATEDIFF(day, Start_Date, @EndDate) &gt;= 0) AND (DATEDIFF(day, @StartDate, Start_Date) &gt;= 0) AND (Crop = ISNULL(@Crop, Crop)) ORDER BY Variety, Lot" DeleteCommand="DELETE FROM Seed_Class_Lot WHERE (UID = @UID)" CancelSelectOnNullParameter="False">
                    <DeleteParameters>
                        <asp:ControlParameter ControlID="GridView1" Name="UID" PropertyName="SelectedValue" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddLocation" PropertyName="SelectedValue" Name="Location_Id"></asp:ControlParameter>
                        <asp:ControlParameter ControlID="ddClosed" Name="Closed" PropertyName="SelectedValue" />
                        <asp:ControlParameter ControlID="ddProducer" Name="Producer" PropertyName="SelectedValue" />
                        <asp:ControlParameter ControlID="txtEndDate" Name="EndDate" PropertyName="Text" />
                        <asp:ControlParameter ControlID="txtStartDate" Name="StartDate" PropertyName="Text" />
                        <asp:ControlParameter ControlID="ddCrop" Name="Crop" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>









            <asp:SqlDataSource ID="sqlLocations" runat="server" ConnectionString="<%$ ConnectionStrings:NW_DataConnectionString %>" SelectCommand="SELECT DISTINCT Locations.Description + N' - ' + LTRIM(STR(Site_Setup.Location_Id)) AS Location, Locations.Id, Locations.Description FROM Locations INNER JOIN Site_Setup ON Locations.Id = Site_Setup.Location_Id ORDER BY Locations.Description" CancelSelectOnNullParameter="False"></asp:SqlDataSource>

        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>

