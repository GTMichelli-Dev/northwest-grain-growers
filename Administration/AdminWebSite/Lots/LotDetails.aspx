<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="LotDetails.aspx.cs" Inherits="Lots_LotDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel runat="server" ID="UP1">
        <ContentTemplate>
            <br />
            <br />

            <asp:DetailsView ID="DetailsView2" runat="server" CssClass="table table-hover  "  HorizontalAlign="Center" AutoGenerateRows="False" DataSourceID="SqlDataSource1" Width="50%" OnPageIndexChanging="DetailsView2_PageIndexChanging">
                <Fields>

                    <asp:TemplateField ShowHeader="false">

                        <ItemTemplate>
                            <asp:Label ID="lblLotNumber" runat="server" CssClass="h2" Text='<%#"Lot:"+ Eval("Lot_Number") %>'></asp:Label>
                        </ItemTemplate>


                    </asp:TemplateField>

                    <asp:TemplateField ShowHeader="false">
                        <ItemTemplate>
                            <asp:HiddenField ID="hfLot" runat="server" Value='<%# Bind("Lot_Number") %>' />
                            <asp:LinkButton CssClass="btn btn-primary " runat="server" OnClick="lnkWeightSheets_Click" ID="lnkWeightSheets">Weight Sheets For Lot</asp:LinkButton>
                        </ItemTemplate>

                    </asp:TemplateField>
                    
                    <asp:TemplateField ShowHeader="false">
                        <ItemTemplate>
                            <asp:HiddenField ID="hflotUID" Value='<%# Bind("lotUID") %>'  runat="server" />
                            <asp:LinkButton CssClass="btn btn-warning" runat="server" OnClick="lnkReopen_Click" ID="lnkReopen">Open Lot</asp:LinkButton>
                            <ajaxToolkit:ConfirmButtonExtender runat="server" ConfirmText='<%#"Lot:"+ Eval("Question") %>' BehaviorID="lnkOpenLot_ConfirmButtonExtender" TargetControlID="lnkReopen" ID="lnkOpenLot_ConfirmButtonExtender"></ajaxToolkit:ConfirmButtonExtender>

                        </ItemTemplate>

                    </asp:TemplateField>


                </Fields>
            </asp:DetailsView>



            <asp:DetailsView ID="DetailsView1" runat="server" CssClass="table table-hover  " HorizontalAlign="Center" AutoGenerateRows="False" DataSourceID="SqlDataSource1" Width="50%">
                <Fields>

                    <asp:BoundField DataField="Location" HeaderText="Location" ReadOnly="True" SortExpression="Location">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Start_Date" HeaderText="Date Created"  DataFormatString="{0:MM/dd/yyyy}" ReadOnly="True" SortExpression="Start_Date">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Close_Date" HeaderText="Date Closed"  DataFormatString="{0:MM/dd/yyyy}" ReadOnly="True" SortExpression="Close_Date">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Grower" SortExpression="Producer">
                        <EditItemTemplate>
                            <asp:LinkButton ID="LinkButton1" CssClass="form-control " runat="server" PostBackUrl='<%# "~/Producers/Producers.aspx?Producer="+ Eval("Producer")+"&Producer_Id="+ Eval("Producer_Id")+"&Lot_Number="+ Eval("Lot_Number")+"&LotUID="+Eval("lotUID")+"&Lot="+Eval("Lot_Number") %>' Text='<%# Eval("Producer") %>'></asp:LinkButton>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("Producer") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label5" runat="server" Text='<%# Bind("Producer") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Crop" SortExpression="Crop">
                        <EditItemTemplate>
                            <asp:DropDownList ID="cboCrops" CssClass="in form-control " runat="server" AutoPostBack="True" OnTextChanged="cboCrops_TextChanged"   DataSourceID="sqlCrops" DataTextField="Description" DataValueField="Id" SelectedValue='<%# Bind("Crop_Id") %>' OnPreRender="cboCrops_PreRender"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="sqlCrops" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT Id, Description+' - '+ltrim(str(Id)) as Description FROM Crops"></asp:SqlDataSource>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Crop") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Crop") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Variety" SortExpression="Variety">
                        <EditItemTemplate>
                            <asp:LinkButton ID="lnkVariety" CssClass="form-control " runat="server" PostBackUrl='<%# "~/Lots/LotVariety.aspx?Lot_Number="+ Eval("Lot_Number")+"&LotUID="+Eval("lotUID")+"&Crop_Id="+Eval("Crop_Id") %>' Text='<%# Eval("Variety") %>'></asp:LinkButton>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Variety") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("Variety") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                   
                    <asp:TemplateField HeaderText="Landlord" SortExpression="Landlord">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtLandlord" CssClass="form-control " runat="server" AutoPostBack="true" OnTextChanged="txtLandlord_TextChanged"  Text='<%# Bind("Landlord") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("Landlord") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("Landlord") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Lot Sampled" SortExpression="Lot_Sampled">
                        <EditItemTemplate>
                            <asp:CheckBox ID="ckLotSampled" runat="server" AutoPostBack="true"  OnCheckedChanged="ckLotSampled_CheckedChanged"  Checked='<%# Bind("Lot_Sampled") %>' />
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Lot_Sampled") %>' />
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Lot_Sampled") %>' Enabled="false" />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                 <%--   <asp:TemplateField HeaderText="State" SortExpression="State_Abv">
                        <EditItemTemplate>
                            <asp:DropDownList runat="server" ID="ddState" CssClass="form-control"  Width="120px"  AutoPostBack="true" OnTextChanged="ddState_TextChanged" SelectedValue='<%# Bind("State_Abv") %>'  DataSourceID="SqlState" DataTextField="text" DataValueField="value">
                            </asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="SqlState" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT 'Not Set' AS text, NULL AS value, 0 AS idx UNION SELECT 'WA' AS text, 'WA' AS value, 1 AS idx UNION SELECT 'OR' AS text, 'OR' AS value, 1 AS idx UNION SELECT 'ID' AS text, 'ID' AS value, 1 AS idx ORDER BY idx, text"></asp:SqlDataSource>
                            
                        </EditItemTemplate>

                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Bind("State_Abv") %>' CssClass="form-control " ID="Label7"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Farm" SortExpression="FSA_Number">
                        <EditItemTemplate>
                            
                            <asp:TextBox ID="txtFarm" runat="server" CssClass="form-control " OnTextChanged="txtFarm_TextChanged" AutoPostBack="true"  Text='<%# Bind("FSA_Number") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:TextBox ID="TextBox3" runat="server"  CssClass="form-control " Text='<%# Bind("FSA_Number") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server"  CssClass="form-control " Text='<%# Bind("FSA_Number") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Comment" SortExpression="Comment">
                        <EditItemTemplate>
                            <table>
                                <tr>
                                    <td style="width: 80% ">
                                        <asp:TextBox ID="txtComment" Rows="3" Cssclass="form-control" TextMode="MultiLine"  runat="server" OnTextChanged="txtComment_TextChanged"  Text='<%# Bind("Comment") %>'></asp:TextBox>
                                    </td>
                                    <td style="width: 20% ">
                                        <asp:LinkButton runat="server" ID="lnkSaveComment" CssClass="btn btn-primary  " OnClick="lnkSaveComment_Click" Text="Save </br> Comment"></asp:LinkButton>
                                    </td>
                                </tr>

                            </table>
                            
                        </EditItemTemplate>
                        <InsertItemTemplate>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label6" runat="server"  CssClass="form-control " Text='<%# Bind("Comment") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>

                </Fields>
            </asp:DetailsView>

            </ContentTemplate>
    </asp:UpdatePanel>

    <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT        vw_Lots.Lot_Number, vw_Lots.Crop, Locations.Description + N' - ' + LTRIM(STR(Locations.Id)) AS Location, vw_Lots.Producer_Description + N'  - ' + LTRIM(STR(vw_Lots.Producer_Id)) AS Producer, 
                         vw_Lots.Landlord, vw_Lots.Start_Date, vw_Lots.Close_Date, vw_Lots.Lot_Sampled, vw_Lots.Void, vw_Lots.Server_Name, vw_Lots.Sequence_ID, CASE WHEN vw_Lots.Close_Date IS NULL THEN CONVERT(bit, 
                         0) ELSE CONVERT(bit, 1) END AS Closed, vw_Lots.UID AS lotUID, vw_Lots.Producer_Id, vw_Lots.Crop_Id, vw_Lots.Variety_Id, VarietyList.Variety, CASE WHEN vw_Lots.close_date IS NULL 
                         THEN 'Are You Sure You Want To Close Lot' ELSE 'Are You Sure You Want To Open Lot' END AS Question, vw_Lots.Comment, vw_Lots.Farm AS FSA_Number
FROM            vw_Lots INNER JOIN
                         Locations ON vw_Lots.Location_Id = Locations.Id LEFT OUTER JOIN
                         VarietyList ON vw_Lots.Variety_Id = VarietyList.Item_Id
WHERE        (vw_Lots.UID = @UID)">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="UID" Name="UID"></asp:QueryStringParameter>
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

