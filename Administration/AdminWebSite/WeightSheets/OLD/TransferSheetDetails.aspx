<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="TransferSheetDetails.aspx.cs" Inherits="WeightSheets_WeightSheetDetails_WeightSheetDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:UpdatePanel runat="server" ID="UP1">
        <ContentTemplate>
            <br />
            <br />
            <h3>
                <asp:Label ID="lblWeightSheet" runat="server" Text="Label"></asp:Label>
            </h3>
                      <div class="container">
                   
                 
                      
                    <h3>Transfer Load</h3>

          


                          <asp:DetailsView ID="IntakeDetails" ClientIDMode="Static" HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Left" EditRowStyle-HorizontalAlign="Left" CssClass="table table-hover table-condensed  " runat="server" Height="20px" Width="50%" AutoGenerateRows="False" DataSourceID="sqlIntakeWS" DefaultMode="Edit">

                              <Fields>

                                  <asp:TemplateField HeaderText="Weigh Sheet" SortExpression="WS_Id">
                                      <EditItemTemplate>
                                          
                                         <asp:Label runat="server" ID="lblWS" Text='<%# Bind("WS_ID") %>'></asp:Label>
                                      </EditItemTemplate>
                                  </asp:TemplateField>


                                  
                                  
                                  <asp:TemplateField HeaderText="Created" SortExpression="Creation_Date">
                                      <EditItemTemplate>
                                          <asp:Label runat="server" ID="lblDate" ClientIDMode="Static" Text='<%#Eval("Creation_Date", "{0:dd/MM/yyyy}") %>' ></asp:Label>
                                          
                                          
                                          <asp:TextBox runat="server" ID="txtDate" OnTextChanged="txtDate_TextChanged" ClientIDMode="Static"  AutoPostBack="true" Text='<%#Eval("Creation_Date", "{0:dd/MM/yyyy}") %>' ></asp:TextBox>
                                          <ajaxToolkit:CalendarExtender runat="server" ID="txtDateCalendarExtender" BehaviorID="txtDateCalendarExtender" TargetControlID="txtDate" />
                                          

                             
                                          
                                      </EditItemTemplate>
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Carrier" SortExpression="Carrier_Description">
                                      <EditItemTemplate>
                                          <asp:label runat="server" Text='<%# Bind("Carrier_Description") %>' ID="TextBox2"></asp:label>
                                      </EditItemTemplate>
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Crop">
                                      <EditItemTemplate>
                                          <asp:label runat="server" Text='<%# Bind("Crop_Description") %>' ID="TextBox3"></asp:label>
                                      </EditItemTemplate>
                                  </asp:TemplateField>
                           
                                  <asp:TemplateField HeaderText="Crop ID" SortExpression="Commodity_Code">
                                      <EditItemTemplate>
                                          <asp:Label runat="server" Text='<%# Bind("Crop_ID") %>' ID="TextBox5"></asp:Label>
                                      </EditItemTemplate>
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Comment" SortExpression="Comment">
                                      <EditItemTemplate>
                                          <asp:TextBox runat="server" CssClass="form-control " Text='<%# Bind("Comment") %>' OnTextChanged="txtComment_TextChanged"  ID="txtComment"></asp:TextBox>
                                      </EditItemTemplate>
                                  
                                  </asp:TemplateField>

                              </Fields>

                          </asp:DetailsView>
                              <asp:SqlDataSource runat="server" ID="sqlIntakeWS" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT Weight_Sheet_UID AS UID, WS_Id, Carrier_Description, Carrier_Id, Weighmaster, Comment, BOL_Type, Rate, Location_Id, Server_Name, Sequence_ID, Creation_Date, Crop_Id, Crop_Description, Location_Description, Total_Billed, Original_Printed, Is_Loadout FROM vwTransfer_Weight_Sheet_Information WHERE (Weight_Sheet_UID = @UID)">
                                  <SelectParameters>
                                      <asp:QueryStringParameter QueryStringField="UID" Name="UID"></asp:QueryStringParameter>
                                  </SelectParameters>
                              </asp:SqlDataSource>




                              <asp:GridView ID="GridIntake" runat="server" CssClass="table table-hover " HorizontalAlign="Center" AutoGenerateColumns="False" DataKeyNames="WS_UID,Load_UID" DataSourceID="sqlIntakeLoads" AllowSorting="True" PageSize="100">
                              <Columns>
                                  <asp:BoundField DataField="Load_Id" HeaderText="Load #" ItemStyle-HorizontalAlign="Left" SortExpression="Load_Id">
                                  <ItemStyle HorizontalAlign="Right" />
                                  </asp:BoundField>
                                  <asp:TemplateField HeaderText="Protien" SortExpression="Protien">
                                      <EditItemTemplate>
                                          <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("Protein") %>'></asp:TextBox>
                                      </EditItemTemplate>
                                      <ItemTemplate>
                                          <asp:Label ID="Label3" runat="server" Text='<%# Bind("Protein") %>'></asp:Label>
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
                          <asp:SqlDataSource ID="sqlIntakeLoads" runat="server" ConnectionString="<%$ ConnectionStrings:NW_DataConnectionString %>" SelectCommand="SELECT Loads.Load_Id, Weight_Sheets.UID AS WS_UID, Transfer_Loads.UID AS TransferLoad_UID, Transfer_Loads.Protein, Loads.UID AS Load_UID, Loads.Time_In, Loads.Weight_In, Loads.Manual_Weight_In, Loads.Time_Out, Loads.Weight_Out, Loads.Manual_Weight_Out, Loads.Bol, Loads.Bin, Loads.Truck_Id, Loads.Comment FROM Weight_Sheet_Transfer_Loads INNER JOIN Weight_Sheets ON Weight_Sheet_Transfer_Loads.Weight_Sheet_UID = Weight_Sheets.UID INNER JOIN Loads INNER JOIN Transfer_Loads ON Loads.UID = Transfer_Loads.Load_UID ON Weight_Sheet_Transfer_Loads.UID = Transfer_Loads.Transfer_Load_UID WHERE (Weight_Sheets.UID = @UID) ORDER BY Loads.Load_Id" CancelSelectOnNullParameter="False">
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

