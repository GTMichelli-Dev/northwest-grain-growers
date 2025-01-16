<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="WeightSheetDetails.aspx.cs" Inherits="WeightSheets_WeightSheetDetails_WeightSheetDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:UpdatePanel runat="server" ID="UP1">
        <ContentTemplate>
            <br />
            <br />
            <h3>
                <asp:Label ID="lblWeightSheet" runat="server" Text="Label"></asp:Label>
            </h3>
                      <div class="container">
                   
                 
                      
                    <h3>Intake Load</h3>

          


                          <asp:DetailsView ID="IntakeDetails" ClientIDMode="Static" HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Left" EditRowStyle-HorizontalAlign="Left" CssClass="table table-hover table-condensed  " runat="server" Height="20px" Width="50%" AutoGenerateRows="False" DataSourceID="sqlIntakeWS" DefaultMode="Edit">

                              <Fields>

                                  <asp:TemplateField HeaderText="Lot Number" SortExpression="Lot_Number">
                                      <EditItemTemplate>
                                          <asp:LinkButton ID="lnkLot" Text='<%# Bind("Lot_Number") %>' PostBackUrl=    '<%#"~/Lots/LotDetails?UID="+Eval("Lot_UID") %>'  runat="server">LinkButton</asp:LinkButton>
                                         
                                      </EditItemTemplate>
                                  </asp:TemplateField>


                                  
                                  
                                  <asp:TemplateField HeaderText="Created" SortExpression="Creation_Date">
                                      <EditItemTemplate>
                                          <asp:Label runat="server" ID="lblDate" ClientIDMode="Static" Text='<%#Eval("Creation_Date", "{0:dd/MM/yyyy}") %>' ></asp:Label>
                                          
                                          
                                          <asp:TextBox runat="server" ID="txtDate" OnTextChanged="txtDate_TextChanged" ClientIDMode="Static"  AutoPostBack="true" Text='<%#Eval("Creation_Date", "{0:dd/MM/yyyy}") %>' ></asp:TextBox>
                                          <ajaxToolkit:CalendarExtender runat="server" ID="txtDateCalendarExtender" BehaviorID="txtDateCalendarExtender" TargetControlID="txtDate" />
                                          

                             
                                          
                                      </EditItemTemplate>
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Producer" SortExpression="Producer_Description">
                                      <EditItemTemplate>
                                          <asp:label runat="server" Text='<%# Bind("Producer_Description") %>' ID="TextBox2"></asp:label>
                                      </EditItemTemplate>
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Producer">
                                      <EditItemTemplate>
                                          <asp:label runat="server" Text='<%# Bind("Producer_Id") %>' ID="TextBox3"></asp:label>
                                      </EditItemTemplate>
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Landlord" SortExpression="Landlord">
                                      <EditItemTemplate>
                                          <asp:Label runat="server" Text='<%# Bind("Landlord") %>' ID="TextBox4"></asp:Label>
                                      </EditItemTemplate>
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Commodity" SortExpression="Commodity_Code">
                                      <EditItemTemplate>
                                          <asp:Label runat="server" Text='<%# Bind("Commodity_Code") %>' ID="TextBox5"></asp:Label>
                                      </EditItemTemplate>
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Variety" SortExpression="Variety">
                                      <EditItemTemplate>
                                          <asp:Label runat="server" Text='<%# Bind("Variety") %>' ID="TextBox6"></asp:Label>
                                      </EditItemTemplate>
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Farm" SortExpression="FSA_Number">
                                      <EditItemTemplate>
                                          <asp:label runat="server" Text='<%# Bind("FSA_Number") %>' ID="TextBox7"></asp:label>
                                      </EditItemTemplate>
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Split Number" SortExpression="Split_Number">
                                      <EditItemTemplate>
                                          <asp:Label runat="server" Text='<%# Bind("Split_Number") %>' ID="TextBox8"></asp:Label>
                                      </EditItemTemplate>
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Hauler" SortExpression="Carrier_Description">
                                      <EditItemTemplate>
                                          
                                          <asp:Label  runat="server" Text='<%# Bind("Carrier_Description") %>' ID="TextBox9"></asp:Label>
                                      </EditItemTemplate>
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Rate" SortExpression="Rate">
                                      <EditItemTemplate>
                                          <asp:Label runat="server" Text='<%# Bind("Rate") %>' ID="TextBox10"></asp:Label>
                                      </EditItemTemplate>
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Total Billed" SortExpression="Total_Billed">
                                      <EditItemTemplate>
                                          <asp:TextBox runat="server"  Text='<%# Bind("Total_Billed") %>' AutoPostBack="true" OnTextChanged="TotalBilled_TextChanged"  ID="TotalBilled"> </asp:TextBox>
                                      </EditItemTemplate>
                                     
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Comment" SortExpression="Comment">
                                      <EditItemTemplate>
                                          <asp:TextBox runat="server" CssClass="form-control " Text='<%# Bind("Comment") %>' OnTextChanged="txtComment_TextChanged"  ID="txtComment"></asp:TextBox>
                                      </EditItemTemplate>
                                  
                                  </asp:TemplateField>

                              </Fields>

                          </asp:DetailsView>
                          <asp:SqlDataSource runat="server" ID="sqlIntakeWS" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT UID, WS_Id, Carrier_Description, Carrier_Id, Weighmaster, Comment, Miles, BOL_Type, Rate, Location_Id, Server_Name, Sequence_ID, Creation_Date, Lot_Number, Producer_Id, Producer_Description, Crop_Id, Crop_Description, FSA_Number, Landlord, Producer_ID_Description, Split_Number, Location_Description, Lot_UID, Total_Loads, Not_Completed, Completed, Total_Billed, Close_Date, Start_Date, Commodity_Code, Variety, Crop_Variety, Original_Printed, Weight_Sheet_Closed, Lot_Sampled, Lot_Closed, Is_Loadout, Is_New_Lot, Is_End_Lot, Date_Created, InDirt, Crop_Variety AS Expr1 FROM vwWeight_Sheet_Information WHERE (UID = @UID)">
                                  <SelectParameters>
                                      <asp:QueryStringParameter QueryStringField="UID" Name="UID"></asp:QueryStringParameter>
                                  </SelectParameters>
                              </asp:SqlDataSource>




                              <asp:GridView ID="GridIntake" runat="server" CssClass="table table-hover " HorizontalAlign="Center" AutoGenerateColumns="False" DataKeyNames="WS_UID,InboundLoad_UID,Load_UID" DataSourceID="sqlIntakeLoads" AllowSorting="True" PageSize="100">
                              <Columns>
                                  <asp:BoundField DataField="Load_Id" HeaderText="Load #" ItemStyle-HorizontalAlign="Left" SortExpression="Load_Id">
                                  <ItemStyle HorizontalAlign="Right" />
                                  </asp:BoundField>
                                  <asp:TemplateField HeaderText="Protien" SortExpression="Protien">
                                      <EditItemTemplate>
                                          <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("Protien") %>'></asp:TextBox>
                                      </EditItemTemplate>
                                      <ItemTemplate>
                                          <asp:Label ID="Label3" runat="server" Text='<%# Bind("Protien") %>'></asp:Label>
                                      </ItemTemplate>
                                      <ItemStyle HorizontalAlign="Right" />
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Weight In" SortExpression="Weight_In">
                                      <EditItemTemplate>
                                          <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Weight_In") %>'></asp:TextBox>
                                      </EditItemTemplate>
                                      <ItemTemplate>
                                          <asp:Label ID="Label1" runat="server" Text='<%# Bind("Weight_In") %>'></asp:Label>
                                      </ItemTemplate>
                                      <ItemStyle HorizontalAlign="Right" />
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Weight Out" SortExpression="Weight_Out">
                                      <EditItemTemplate>
                                          <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Weight_Out") %>'></asp:TextBox>
                                      </EditItemTemplate>
                                      <ItemTemplate>
                                          <asp:Label ID="Label2" runat="server" Text='<%# Bind("Weight_Out") %>'></asp:Label>
                                      </ItemTemplate>
                                      <ItemStyle HorizontalAlign="Right" />
                                  </asp:TemplateField>
                                  <asp:BoundField DataField="Bol" HeaderText="Bol" SortExpression="Bol"></asp:BoundField>
                                  <asp:BoundField DataField="Bin" HeaderText="Bin" SortExpression="Bin"></asp:BoundField>
                                  <asp:BoundField DataField="Truck_Id" HeaderText="Truck Id" SortExpression="Truck_Id"></asp:BoundField>
                                  <asp:BoundField DataField="Comment" HeaderText="Comment" SortExpression="Comment"></asp:BoundField>
                              </Columns>
                    </asp:GridView>
                          <asp:SqlDataSource ID="sqlIntakeLoads" runat="server" ConnectionString="<%$ ConnectionStrings:NW_DataConnectionString %>" SelectCommand="SELECT Loads.Load_Id, Weight_Sheets.UID AS WS_UID, Inbound_Loads.UID AS InboundLoad_UID, Inbound_Loads.Protien, Loads.UID AS Load_UID, Loads.Time_In, Loads.Weight_In, Loads.Manual_Weight_In, Loads.Time_Out, Loads.Weight_Out, Loads.Manual_Weight_Out, Loads.Bol, Loads.Bin, Loads.Truck_Id, Loads.Comment FROM Loads INNER JOIN Inbound_Loads ON Loads.UID = Inbound_Loads.Load_UID INNER JOIN Weight_Sheets ON Inbound_Loads.WS_UID = Weight_Sheets.UID WHERE (Weight_Sheets.UID = @UID) ORDER BY Loads.Load_Id" CancelSelectOnNullParameter="False">
                              <SelectParameters>
                                  <asp:QueryStringParameter QueryStringField="UID" Name="UID"></asp:QueryStringParameter>

                              </SelectParameters>
                    </asp:SqlDataSource>
                              <asp:PlaceHolder ID="PlaceHolderIntake" runat="server">
                         </asp:PlaceHolder>  
                                                 <asp:PlaceHolder ID="PlaceHolderTransfer" runat="server">
             <%--       <h3>Transfer Load</h3>
                          <asp:GridView ID="GrdTransfer" runat="server" CssClass="table table-hover " HorizontalAlign="Center" AutoGenerateColumns="False" DataKeyNames="Load_UID" DataSourceID="SqlTransferLoads" AllowSorting="True" PageSize="100">
                              <Columns>
                                  <asp:BoundField DataField="Load_Id" HeaderText="Load #" ItemStyle-HorizontalAlign="Left" SortExpression="Load_Id">
                                  <ItemStyle HorizontalAlign="Right" />
                                  </asp:BoundField>
                                  <asp:TemplateField HeaderText="Protien" SortExpression="Protien">
                                      <EditItemTemplate>
                                          <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("Protien") %>'></asp:TextBox>
                                      </EditItemTemplate>
                                      <ItemTemplate>
                                          <asp:Label ID="Label3" runat="server" Text='<%# Bind("Protien") %>'></asp:Label>
                                      </ItemTemplate>
                                      <ItemStyle HorizontalAlign="Right" />
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Weight In" SortExpression="Weight_In">
                                      <EditItemTemplate>
                                          <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Weight_In") %>'></asp:TextBox>
                                      </EditItemTemplate>
                                      <ItemTemplate>
                                          <asp:Label ID="Label1" runat="server" Text='<%# Bind("Weight_In") %>'></asp:Label>
                                      </ItemTemplate>
                                      <ItemStyle HorizontalAlign="Right" />
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Weight Out" SortExpression="Weight_Out">
                                      <EditItemTemplate>
                                          <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Weight_Out") %>'></asp:TextBox>
                                      </EditItemTemplate>
                                      <ItemTemplate>
                                          <asp:Label ID="Label2" runat="server" Text='<%# Bind("Weight_Out") %>'></asp:Label>
                                      </ItemTemplate>
                                      <ItemStyle HorizontalAlign="Right" />
                                  </asp:TemplateField>
                                  <asp:BoundField DataField="Bol" HeaderText="Bol" SortExpression="Bol"></asp:BoundField>
                                  <asp:BoundField DataField="Bin" HeaderText="Bin" SortExpression="Bin"></asp:BoundField>
                                  <asp:BoundField DataField="Truck_Id" HeaderText="Truck Id" SortExpression="Truck_Id"></asp:BoundField>
                                  <asp:BoundField DataField="Comment" HeaderText="Comment" SortExpression="Comment"></asp:BoundField>
                              </Columns>
                    </asp:GridView>
                                                     <asp:SqlDataSource ID="SqlTransferLoads" runat="server" ConnectionString="<%$ ConnectionStrings:NW_DataConnectionString %>" SelectCommand="SELECT Loads.Load_Id, Weight_Sheets.UID AS WS_UID, Loads.UID AS Load_UID, Loads.Time_In, Loads.Weight_In, Loads.Manual_Weight_In, Loads.Time_Out, Loads.Weight_Out, Loads.Manual_Weight_Out, Loads.Bol, Loads.Bin, Loads.Truck_Id, Loads.Comment, Transfer_Loads.UID AS TransferLoadUID, Weight_Sheet_Transfer_Loads.UID AS WeightSheetTransferLoadUID, Transfer_Loads.Protein AS Protien FROM Loads INNER JOIN Transfer_Loads ON Loads.UID = Transfer_Loads.Load_UID INNER JOIN Weight_Sheet_Transfer_Loads ON Transfer_Loads.Transfer_Load_UID = Weight_Sheet_Transfer_Loads.UID INNER JOIN Weight_Sheets ON Weight_Sheet_Transfer_Loads.Weight_Sheet_UID = Weight_Sheets.UID WHERE (Weight_Sheets.UID = @UID) ORDER BY Loads.Load_Id" CancelSelectOnNullParameter="False">
                              <SelectParameters>
                                  <asp:QueryStringParameter QueryStringField="UID" Name="UID"></asp:QueryStringParameter>

                              </SelectParameters>
                    </asp:SqlDataSource>--%>

                         </asp:PlaceHolder>  
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>

