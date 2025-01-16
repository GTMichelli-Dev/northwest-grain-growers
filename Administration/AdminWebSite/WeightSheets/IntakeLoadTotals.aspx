<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="IntakeLoadTotals.aspx.cs" Inherits="WeightSheets_IntakeLoadTotals" %>

<asp:Content ID="Content0" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdateProgress runat="server" ID="UPr">
        <ProgressTemplate >
        

            <h2 style="width:100%" class="text-info text-center  ">Hang On... Getting Data</h2>
         </ProgressTemplate>
    </asp:UpdateProgress>


                       
   
    <asp:UpdatePanel runat="server" ID="UP1">
        <ContentTemplate>
            <h3><asp:Label ID="lblHeader" runat="server" Text="Intake Totals"></asp:Label></h3>



               <%-- <div class=" form-row d-flex align-content-center">
                    <div class="col-2 offset-4">
                           <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-sm btn-secondary mr-0 ml-auto" Text="Download" Width="100px" ClientIDMode="Static" OnClick="btnPrint_Click" />
                    </div>
                    <div class="col-2">
                    </div>

                </div>--%>

            
     

        
      <table class=" table" >
                <tr>
                    <td></td>
                     <td></td>
                    <td style="width:150px" class="text-center ">
                        From
                    </td>
                    <td style="width:150px" class="text-center ">
                        To
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
                     <td style="width:150px" class="text-center ">
                       <asp:TextBox runat="server" CssClass="input-sm " ID="txtEndDate" AutoPostBack="True" OnTextChanged="txtEndDate_TextChanged"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender runat="server" BehaviorID="txtEndDate_CalendarExtender" TargetControlID="txtEndDate" ID="txtEndDate_CalendarExtender"></ajaxToolkit:CalendarExtender>
    
                     </td>
                     <td></td>
                     <td></td>
                 </tr>
                 </table>

            <table>

                
                <tr>
                    <td>Location</td>
                    <td>Producer</td>
                    <td>Crop</td>
                    <td>Variety</td>
                    
                  
                 
                </tr>

                <tr>
 
                   <td>
                        <asp:DropDownList ID="cboLocation" runat="server" AutoPostBack="True" CssClass="input-sm " DataSourceID="sqlLocation" DataTextField="Text" DataValueField="Id" OnTextChanged="cboLocation_TextChanged">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="sqlLocation" runat="server" ConnectionString="<%$ ConnectionStrings:NW_DataConnectionString %>" SelectCommand="SELECT 'All Locations' AS Text, NULL AS Id, 0 AS idx UNION SELECT Description + ' - ' + LTRIM(STR(Id)) AS Text, Description AS id, 1 AS idx FROM Locations ORDER BY idx, Text"></asp:SqlDataSource>
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
                    <td  class="text-center "> 
                        <asp:DropDownList ID="ddCrop" CssClass="input-sm " AutoPostBack="true"  runat="server" OnTextChanged="ddCrop_TextChanged" DataSourceID="sqlCrop" DataTextField="Text" DataValueField="Id"></asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="sqlCrop" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT 'All Crops' AS Text, NULL AS Id, 0 AS idx UNION SELECT Description + ' - ' + LTRIM(STR(Id)) AS Text, Id, 1 AS idx FROM Crops ORDER BY idx" CancelSelectOnNullParameter="False"></asp:SqlDataSource>
                    </td>
                                        <td  class="text-center "> 
                        <asp:DropDownList ID="ddVariety" CssClass="input-sm " AutoPostBack="true"  runat="server" OnTextChanged="ddVariety_TextChanged" DataSourceID="SqlVariety" DataTextField="Text" DataValueField="Id"></asp:DropDownList>
                                            <asp:SqlDataSource runat="server" ID="SqlVariety" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT 'All Varieties' AS Text, NULL AS Id, 0 AS idx, 1 AS idx2 UNION SELECT Variety + ' - ' + LTRIM(STR(Item_Id)) AS Text, Item_Id, 1 AS idx, 0 AS idx2 FROM VarietyList WHERE (Crop_Id = @CropID) ORDER BY idx, idx2, Text" CancelSelectOnNullParameter="False">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="ddCrop" PropertyName="SelectedValue" Name="CropID"></asp:ControlParameter>
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                    </td>
             
                   
                </tr>
            
            </table>

      
            <hr />
                <div class="d-flex justify-content-center" style="font-size:1.5em">
                <asp:DetailsView ID="DetailsView1" runat="server" Height="50px" Width="300px" AutoGenerateRows="False" DataSourceID="SqlDataSource1">
                    <Fields>
                        <asp:TemplateField HeaderText="Net Lbs" HeaderStyle-Width="150px" SortExpression="NetLbs">
                          
                            <ItemTemplate>
                                <asp:Label runat="server"  CssClass="font-weight-bold"  Text='<%# Bind("NetLbs", "{0:N0}") %>' ID="Label1"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Net Bushels"  HeaderStyle-Width="150px"  SortExpression="NetBushels">
                           
                            <ItemTemplate>
                                <asp:Label runat="server"  CssClass="font-weight-bold"  Text='<%# Bind("NetBushels", "{0:N0}") %>' ID="Label2"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Fields>
                </asp:DetailsView>

                </div>



  
            <br />
            <div class=" row ">
                
            
    

            <asp:GridView ID="GridView1" CssClass="table table-hover col-12"    runat="server" DataSourceID="sqlWeightSheets" AutoGenerateColumns="False" AllowSorting="True" PageSize="50" HorizontalAlign="Center" >
                <Columns>

                    <asp:BoundField DataField="Location_Name" HeaderText="Location" SortExpression="Location_Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Producer" HeaderText="Producer" SortExpression="Producer" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Crop" HeaderText="Crop" ReadOnly="True" SortExpression="Crop">
                    <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Variety" HeaderText="Variety" ReadOnly="True" SortExpression="Variety">
                    <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>

                    <asp:BoundField DataField="Net" HeaderText="Net" SortExpression="Net" ReadOnly="True" DataFormatString="{0:N0}" >

                    <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Bushels" DataFormatString="{0:N0}" HeaderText="Bu" SortExpression="Bushels">
                    <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>

                </Columns>
                <EmptyDataTemplate>
                    
                    
                         <h5  class="text-center">No Weightsheets Matching Filter</h5>
                    
                </EmptyDataTemplate>
            </asp:GridView>
                <asp:SqlDataSource runat="server" ID="sqlWeightSheets" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT Location_Id, Location_Name, Producer, Crop, SUM(Net) AS Net, SUM(Net / 60) AS Bushels, Variety FROM (SELECT Weight_Sheets.Location_Id, Locations.Description AS Location_Name, Locations.Description + N' - ' + LTRIM(STR(Locations.Id)) AS Location, Lots.Producer_Id, Lots.Producer_Description + N' - ' + LTRIM(STR(Lots.Producer_Id)) AS Producer, Crops.Id AS Crop_Id, Crops.Description + N' - ' + LTRIM(STR(Crops.Id)) AS Crop, vwWeightSheet_Net.Net, ISNULL(Crop_Varieties.Description, N'') AS Variety, Crop_Varieties.Item_Id FROM Crop_Varieties RIGHT OUTER JOIN Lots INNER JOIN Weight_Inbound_Loads ON Lots.UID = Weight_Inbound_Loads.Lot_UID INNER JOIN Crops ON Lots.Crop_Id = Crops.Id ON Crop_Varieties.Item_Id = Lots.Variety_Id RIGHT OUTER JOIN vwWeightSheet_Net RIGHT OUTER JOIN Weight_Sheets ON vwWeightSheet_Net.WeightSheetUID = Weight_Sheets.UID LEFT OUTER JOIN Locations ON Weight_Sheets.Location_Id = Locations.Id ON Weight_Inbound_Loads.Weight_Sheet_UID = Weight_Sheets.UID WHERE (DATEDIFF(day, @StartDate, Weight_Sheets.Creation_Date) &gt;= 0) AND (DATEDIFF(day, Weight_Sheets.Creation_Date, @EndDate) &gt;= 0) AND (Lots.Producer_Id = ISNULL(@Producer_Id, Lots.Producer_Id)) AND (Crops.Id = ISNULL(@Crop, Crops.Id)) AND (Crop_Varieties.Item_Id = ISNULL(@VarietyID, Crop_Varieties.Item_Id)) AND (Weight_Sheets.Location_Id = ISNULL(@Location_Id, Locations.Id)) AND (vwWeightSheet_Net.Net IS NOT NULL)) AS derivedtbl_1 GROUP BY Location_Id, Location_Name, Producer, Crop, Variety ORDER BY Location_Name, Crop, Producer, Variety" CancelSelectOnNullParameter="False">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hfStart" Name="StartDate" PropertyName="Value" />
                        <asp:ControlParameter ControlID="hfEnd" PropertyName="Value" Name="EndDate"></asp:ControlParameter>
                        <asp:ControlParameter ControlID="ddProducer" PropertyName="SelectedValue" Name="Producer_Id"></asp:ControlParameter>
                        <asp:ControlParameter ControlID="ddCrop" PropertyName="SelectedValue" Name="Crop"></asp:ControlParameter>

                      




                        <asp:ControlParameter ControlID="ddVariety" PropertyName="SelectedValue" Name="VarietyID"></asp:ControlParameter>
                        <asp:ControlParameter ControlID="cboLocation" Name="Location_Id" PropertyName="SelectedValue" />




                </SelectParameters>
            </asp:SqlDataSource>
                   <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="
SELECT        SUM(Net) AS NetLbs, SUM(Net) / 60 AS NetBushels
FROM            (SELECT        Weight_Sheets.Location_Id, Locations.Description AS Location_Name, Locations.Description + N' - ' + LTRIM(STR(Locations.Id)) AS Location, Lots.Producer_Id, 
                         Lots.Producer_Description + N' - ' + LTRIM(STR(Lots.Producer_Id)) AS Producer, Crops.Id AS Crop_Id, Crops.Description + N' - ' + LTRIM(STR(Crops.Id)) AS Crop, vwWeightSheet_Net.Net, 
                         ISNULL(Crop_Varieties.Description, N'') AS Variety, Crop_Varieties.Item_Id
FROM            Crop_Varieties RIGHT OUTER JOIN
                         Lots INNER JOIN
                         Weight_Inbound_Loads ON Lots.UID = Weight_Inbound_Loads.Lot_UID INNER JOIN
                         Crops ON Lots.Crop_Id = Crops.Id ON Crop_Varieties.Item_Id = Lots.Variety_Id RIGHT OUTER JOIN
                         vwWeightSheet_Net RIGHT OUTER JOIN
                         Weight_Sheets ON vwWeightSheet_Net.WeightSheetUID = Weight_Sheets.UID LEFT OUTER JOIN
                         Locations ON Weight_Sheets.Location_Id = Locations.Id ON Weight_Inbound_Loads.Weight_Sheet_UID = Weight_Sheets.UID
WHERE        (DATEDIFF(day, @StartDate, Weight_Sheets.Creation_Date) &gt;= 0) AND (DATEDIFF(day, Weight_Sheets.Creation_Date, @EndDate) &gt;= 0) AND (Lots.Producer_Id = ISNULL(@Producer_Id, Lots.Producer_Id)) AND 
                         (Crops.Id = ISNULL(@Crop, Crops.Id)) AND (Crop_Varieties.Item_Id = ISNULL(@VarietyID, Crop_Varieties.Item_Id)) AND (Weight_Sheets.Location_Id = ISNULL(@Location_Id, Locations.Id)) AND 
                         (vwWeightSheet_Net.Net IS NOT NULL)) AS derivedtbl_1" CancelSelectOnNullParameter="False">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hfStart" Name="StartDate" PropertyName="Value" />
                        <asp:ControlParameter ControlID="hfEnd" PropertyName="Value" Name="EndDate"></asp:ControlParameter>
                        <asp:ControlParameter ControlID="ddProducer" PropertyName="SelectedValue" Name="Producer_Id"></asp:ControlParameter>
                        <asp:ControlParameter ControlID="ddCrop" PropertyName="SelectedValue" Name="Crop"></asp:ControlParameter>

                        




                        <asp:ControlParameter ControlID="ddVariety" PropertyName="SelectedValue" Name="VarietyID"></asp:ControlParameter>
                        <asp:ControlParameter ControlID="cboLocation" Name="Location_Id" PropertyName="SelectedValue" />




                </SelectParameters>
            </asp:SqlDataSource>
               
            </div>

            <asp:HiddenField ID="hfStart" runat="server" />
            <asp:HiddenField ID="hfEnd" runat="server" />

        </ContentTemplate>

    </asp:UpdatePanel>

</asp:Content>



<asp:Content ID="Content1" ContentPlaceHolderID="MainContentFull" runat="Server">
</asp:Content>