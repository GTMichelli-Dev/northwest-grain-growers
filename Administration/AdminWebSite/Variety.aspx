<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Variety.aspx.cs" Inherits="Variety" %>
<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>


            <h3>Varieties</h3>
            <p>
                <asp:Button ID="btnNew" runat="server" CssClass="btn btn-primary " OnClick="btnNew_Click"  Text="NewVariety" />
            </p>
            <p>
                <asp:GridView ID="GridView1" runat="server" HorizontalAlign="Center" CssClass="table table-condensed table-hover " AutoGenerateColumns="False" DataSourceID="sqlVarieties" AllowSorting="True" DataKeyNames="UID">
                    <Columns>
                        <asp:BoundField DataField="Crop" HeaderText="Crop" SortExpression="Crop"></asp:BoundField>
                        <asp:TemplateField HeaderText="Description" SortExpression="Description">

                            <ItemTemplate>
                                <asp:TextBox ID="txtDescription" AutoPostBack="true" OnTextChanged="txtDescription_TextChanged" runat="server" Text='<%# Bind("Description") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Id" SortExpression="Id">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Id") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Breeder" HeaderText="Breeder" SortExpression="Breeder"></asp:BoundField>
                        <asp:BoundField DataField="Trait" HeaderText="Trait" SortExpression="Trait"></asp:BoundField>
                        <asp:TemplateField HeaderText="Use For Seed" SortExpression="Use_For_Seed">
                            <EditItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Use_For_Seed") %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Use_For_Seed") %>' Enabled="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField />
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="btndelete" OnClick="btndelete_Click" runat="server" CssClass=" text-danger "  CausesValidation="False" Text="Delete"></asp:LinkButton>
                                <ajaxToolkit:ConfirmButtonExtender runat="server" ConfirmText='<%#"Delete "+ Eval("Description")+"?" %>' BehaviorID="btndelete_ConfirmButtonExtender" TargetControlID="btndelete" ID="btndelete_ConfirmButtonExtender"></ajaxToolkit:ConfirmButtonExtender>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource runat="server" ID="sqlVarieties" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT Crop, Description, Id, Breeder, Trait, Use_For_Seed, UID FROM Variety WHERE (Use_For_Seed = 1) ORDER BY Crop, Description"></asp:SqlDataSource>
            </p>

            <asp:Panel ID="pnlNewItem" CssClass="modal-dialog text-center " Style="display: none" runat="server" DefaultButton="btnNewItemSave">
                <div class="modal-content">
                    <div class="modal-body  ">
                        <div class="col-md-12 text-right ">
                            <asp:LinkButton ID="btnCancelNew" CssClass=" h1 " runat="server">&times;</asp:LinkButton>
                        </div>
                        <p>
                            <asp:Label ID="lblNewItemError" runat="server" CssClass=" h3  text-danger  " Text=""></asp:Label>
                        </p>

                        <asp:Label ID="lblmodalItemPrompt" CssClass="h4 modal-title " runat="server" Text="Enter New Variety"></asp:Label>

                        <div class="row">
                            <div class=" col-md-3"></div>

                            <div class=" col-md-6 ">

                                <div class="row  form-group   ">
                                    <label for="ddSelectCrop">Crop</label>
                                    <asp:DropDownList ID="ddSelectCrop" CssClass="form-control dropdown " runat="server"  DataSourceID="sqlCrop" DataTextField="text" DataValueField="value"></asp:DropDownList>
                                </div>
                                <div class="row  form-group   ">
                                    <label for="txtNewVariety">Variety</label>
                                    <asp:TextBox ID="txtNewVariety"  CssClass="form-control" onkeydown="return (event.keyCode!=13);" autocomplete="off" runat="server"></asp:TextBox>
                                </div>
                                <div class="row  form-group   ">
                                    <label for="txtNewID">ID</label>
                                    <asp:TextBox ID="txtNewID"  CssClass="form-control" MaxLength="3" onkeydown="return (event.keyCode!=13);" autocomplete="off" runat="server"></asp:TextBox>
                                    <ajaxToolkit:FilteredTextBoxExtender runat="server" BehaviorID="txtNewID_FilteredTextBoxExtender" TargetControlID="txtNewID" ID="txtNewID_FilteredTextBoxExtender" ValidChars="0123456789"></ajaxToolkit:FilteredTextBoxExtender>
                                </div>
                                
                                <div class="row  form-group   ">
                                    <label for="txtNewBreeder">Breeder</label>
                                    <asp:TextBox ID="txtNewBreeder" CssClass="form-control" onkeydown="return (event.keyCode!=13);" autocomplete="off" runat="server"></asp:TextBox>
                                </div>
                                <div class="row  form-group   ">
                                    <label for="txtNewTrait">Trait</label>
                                    <asp:TextBox ID="txtNewTrait" CssClass="form-control" onkeydown="return (event.keyCode!=13);" autocomplete="off" runat="server"></asp:TextBox>
                                </div>
                                <asp:SqlDataSource runat="server" ID="sqlCrop" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT '' AS text, NULL AS value UNION SELECT Description AS Text, UID AS value FROM Crops WHERE (Use_At_Seed_Mill = 1) ORDER BY text"></asp:SqlDataSource>


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
                          <asp:Label ID="lblErrMsg" CssClass="h4 modal-title " runat="server" Text="Variety Already Exists"></asp:Label>
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










        </ContentTemplate>

    </asp:UpdatePanel>



</asp:Content>

