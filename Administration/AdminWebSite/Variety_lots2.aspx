<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Variety_lots2.aspx.cs" Inherits="Variety_lots" %>
<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h2>Variety Lots</h2>
            <div class="row">

                <div class="col-md-12 text-left">
                    <table>
                        <tr>
                            <td>
                                <label for="ddLocation" class="text-left ">Locations</label>
                                <asp:DropDownList ID="ddLocation" CssClass="form-control dropdown  " runat="server" DataSourceID="sqlLocations" DataTextField="Location" DataValueField="Id" AutoPostBack="True" OnSelectedIndexChanged="ddLocation_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td style="vertical-align: bottom">
                                <asp:Button ID="btnNew" runat="server" CssClass="btn btn-primary text-left  " OnClick="btnNew_Click" Text="NewLot" />

                            </td>
                        </tr>
                    </table>

                </div>
                <%--  <div class="col-md-4">
                    
                </div>--%>
            </div>

            <hr />
            <br />

            <div class="row col-md-12">

                <asp:GridView ID="GridView1" CssClass="table table-hover table-responsive  " HorizontalAlign="Center" runat="server" DataSourceID="sqlVarietyLots" AutoGenerateColumns="False" DataKeyNames="UID" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" AllowSorting="True">
                    <Columns>
                        <asp:TemplateField HeaderText="Lot" SortExpression="Lot">

                            <ItemTemplate>
                                <asp:TextBox ID="txtLot" AutoPostBack="true" OnTextChanged="txtLot_TextChanged"  runat="server" Text='<%# Bind("Lot") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Variety_Description" SortExpression="Variety_Description">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Variety_Description") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Variety_Description") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Class" SortExpression="Class">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("Class") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("Class") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Crop" SortExpression="Crop">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("Crop") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("Crop") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Active" SortExpression="Active">
                            <EditItemTemplate>
                              
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="ckActive" runat="server" OnCheckedChanged="ckActive_CheckedChanged" AutoPostBack="true" Checked='<%# Bind("Active") %>' Enabled="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date Tested" SortExpression="Date_Tested">
                            <EditItemTemplate>
                                
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="txtDateTested" AutoPostBack="true"   OnTextChanged="txtDateTested_TextChanged" runat="server" Text='<%# Bind("Date_Tested", "{0:d}") %>'></asp:TextBox>
                             <%--   <div id="calendarContainerOverride">
                                    <div style="padding-top: 10px">
                                        <ajaxToolkit:CalendarExtender runat="server" BehaviorID="txtDateTested_CalendarExtender" TargetControlID="txtDateTested" ID="txtDateTested_CalendarExtender"></ajaxToolkit:CalendarExtender>
                                    </div>
                                </div>--%>

                                
                                
                  
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False" CssClass="text-danger " CommandName="Delete" Text="Delete"></asp:LinkButton>
                                <ajaxToolkit:ConfirmButtonExtender runat="server" ConfirmText='<%#"Delete "+ Eval("Lot")+"?" %>' BehaviorID="btnDelete_ConfirmButtonExtender" TargetControlID="btnDelete" ID="btnDelete_ConfirmButtonExtender"></ajaxToolkit:ConfirmButtonExtender>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate >
                       
                        <h5 class="text-center">No Lots Defined For Location</h5>
                       
                       
                        
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:SqlDataSource runat="server" ID="sqlVarietyLots" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT UID, Lot, Variety_Id, Variety_Description, Class, Crop, Active, Date_Tested FROM vwVariety_Lots WHERE (Location_Id = @Location_Id) ORDER BY Crop, Variety_Description, Class" DeleteCommand="DELETE FROM Seed_Class_Lot WHERE (UID = @UID)">
                    <DeleteParameters>
                        <asp:ControlParameter ControlID="GridView1" Name="UID" PropertyName="SelectedValue" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddLocation" PropertyName="SelectedValue" Name="Location_Id"></asp:ControlParameter>
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>





            <asp:Panel ID="pnlNewItem" CssClass="modal-dialog text-center " Style="display: none" runat="server" DefaultButton="btnNewItemSave">

                <div class="modal-content">

                    <div class="modal-header">
                        <div class="col-md-12 text-right ">
                            <asp:LinkButton ID="btnCancelNew" CssClass=" h1 " runat="server">&times;</asp:LinkButton>
                        </div>
                        <asp:Panel runat="server" CssClass="text-center " Visible="false" ID="pnlError">
                            <asp:Label ID="lblNewItemError" runat="server" CssClass=" h3  text-danger  " Text=""></asp:Label>
                        </asp:Panel>

                        <asp:Label ID="lblmodalItemPrompt" CssClass="h4 modal-title " runat="server" Text="Enter New Lot"></asp:Label>

                    </div>
                    <div class="modal-body  ">
                        <div class="row">
                            <div class=" col-md-3"></div>

                            <div class=" col-md-6 ">
                                <div class="row  form-group   ">
                                    <label for="lblCurLocation">Location</label>
                                    <asp:Label runat="server" ID="lblCurLocation" CssClass=" text-left  form-control  "></asp:Label>
                                </div>
                                <div class="row  form-group   ">
                                    <label for="ddSelectCrop">Crop</label>
                                    <asp:DropDownList ID="ddSelectCrop" CssClass="form-control dropdown " runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddSelectCrop_SelectedIndexChanged"  DataSourceID="sqlCrop" DataTextField="text" DataValueField="value"></asp:DropDownList>
                                </div>
                                <div class="row  form-group   ">
                                    <label for="ddSelectVariety">Variety</label>
                                    <asp:DropDownList ID="ddSelectVariety" CssClass="form-control dropdown "  runat="server" DataSourceID="sqlVariety" DataTextField="text" DataValueField="value"></asp:DropDownList>
                                </div>
                                <div class="row  form-group   ">
                                    <label for="ddSelectClass">Class</label>
                                    <asp:DropDownList ID="ddSelectClass" CssClass="form-control dropdown " runat="server" DataSourceID="sqlClass" DataTextField="text" DataValueField="value"></asp:DropDownList>

                                </div>
                                <div class="row  form-group   ">
                                    <label for="txtNewLot">Lot</label>
                                    <asp:TextBox ID="txtNewLot" placeholder="Enter Lot Number" CssClass="form-control" onkeydown="return (event.keyCode!=13);" autocomplete="off" runat="server"></asp:TextBox>
                                </div>
                                <asp:SqlDataSource runat="server" ID="sqlClass" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT '' AS text, NULL AS value UNION SELECT Description AS text, UID AS value FROM Variety_Class ORDER BY text"></asp:SqlDataSource>
                                <asp:SqlDataSource runat="server" ID="sqlCrop" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT '' AS text, NULL AS value UNION SELECT Description AS Text, UID AS value FROM Crops WHERE (Use_At_Seed_Mill = 1) ORDER BY text"></asp:SqlDataSource>
                                <asp:SqlDataSource runat="server" ID="sqlVariety" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT '' AS text, NULL AS value UNION SELECT Variety.Description AS Text, Variety.UID AS value FROM Crops INNER JOIN Variety ON Crops.Description = Variety.Crop WHERE (Crops.Use_At_Seed_Mill = 1) AND (Crops.UID = @CropUID) ORDER BY Text">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddSelectCrop" PropertyName="SelectedValue" Name="CropUID" DbType="Guid"></asp:ControlParameter>
                                    </SelectParameters>
                                </asp:SqlDataSource>

                            </div>


                        </div>
                        <div class=" col-md-3"></div>
                    </div>
                        <div class="row ">
                            <div class=" col-md-2"></div>
                            <div class=" col-md-3">
                                <asp:LinkButton ID="btnNewItemSave" CssClass="btn btn-block btn-success " OnClick="btnNewItemSave_Click" runat="server">Save</asp:LinkButton>
                            </div>
                            <div class=" col-md-2"></div>
                            <div class=" col-md-3">
                                <asp:LinkButton ID="lnkNewItemCancel" CssClass="btn btn-block btn-danger " runat="server">Cancel</asp:LinkButton>
                            </div>
                            <div class=" col-md-2"></div>
                        </div>
                    <br />
                    <br />
                    </div>

                </div>
            </asp:Panel>


            <asp:HiddenField ID="hfNewItem" runat="server" />

            <ajaxToolkit:ModalPopupExtender ID="pnlNewItemPopupExtender1" runat="server" PopupControlID="pnlNewItem" TargetControlID="hfNewItem"
                BackgroundCssClass="modalBackground fade" CancelControlID="lnkNewItemCancel">
            </ajaxToolkit:ModalPopupExtender>





            
            <asp:Panel ID="pnlErrMsg" CssClass="modal-dialog text-center " Style="display: none" runat="server" DefaultButton="lbCancelErrMsg">

                <div class="modal-content">

                    <div class="modal-header">
                        <div class="col-md-12 text-right ">
                            <asp:LinkButton ID="LinkButton1" CssClass=" h1 " runat="server">&times;</asp:LinkButton>
                        </div>
         
                    </div>
                    <div class="modal-body  ">
                          <asp:Label ID="lblErrMsg" CssClass="h4 modal-title " runat="server" Text="Lot Already Exists For Location"></asp:Label>
                    </div>
                        <div class="row ">
                            <div class=" col-md-2"></div>
                         
                            <div class=" col-md-2"></div>
                            <div class=" col-md-3">
                                <asp:LinkButton ID="lbCancelErrMsg" CssClass="btn btn-block btn-danger " runat="server">Cancel</asp:LinkButton>
                            </div>
                            <div class=" col-md-2"></div>
                        </div>
                    <br />
                    <br />
                    </div>

                </div>
            </asp:Panel>


            <asp:HiddenField ID="hfErrMsg" runat="server" />

            <ajaxToolkit:ModalPopupExtender ID="ModalPopupErrMsg" runat="server" PopupControlID="pnlErrMsg" TargetControlID="hfErrMsg"
                BackgroundCssClass="modalBackground fade" CancelControlID="lbCancelErrMsg">
            </ajaxToolkit:ModalPopupExtender>



















            <asp:SqlDataSource ID="sqlLocations" runat="server" ConnectionString="<%$ ConnectionStrings:NW_DataConnectionString %>" SelectCommand="SELECT Locations.Description + N' - ' + LTRIM(STR(Site_Setup.Location_Id)) AS Location, Locations.Id, Locations.Description FROM Locations INNER JOIN Site_Setup ON Locations.Id = Site_Setup.Location_Id ORDER BY Locations.Description"></asp:SqlDataSource>

        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>

