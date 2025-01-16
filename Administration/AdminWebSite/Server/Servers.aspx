<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Servers.aspx.cs" Inherits="Server_Servers" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="text-center container  ">
              

                <h3>Servers</h3>
              
                <div class="container">
                    <div class="row">
                       <div class="col-md-3">
                        </div>
                        <div class="col-md-6">
                            <asp:GridView ID="GridView1"  runat="server" CssClass="table table-bordered table-condensed table-hover " AutoGenerateColumns="False" DataKeyNames="UID" DataSourceID="SqlServers" AllowSorting="True" HorizontalAlign="Center" BorderStyle="None" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                                <Columns>
                                    <asp:TemplateField ShowHeader="False" Visible="False">
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="lnkbtnOK" runat="server" CausesValidation="True" CommandName="Update" CssClass="text-success" Text="Update"></asp:LinkButton>
                                            &nbsp;<asp:LinkButton ID="lnkbtnCancel" runat="server" CausesValidation="False" CommandName="Cancel" CssClass="text-danger" Text="Cancel"></asp:LinkButton>

                                        </EditItemTemplate>
                                        <HeaderTemplate>
                                            <div class="text-right " style="width:100%; ">
                                            <asp:LinkButton ID="lnkAdd" OnClick="lnkAdd_Click"   CssClass="text-primary text-right  " runat="server" CommandName="insert">Add</asp:LinkButton>
                                                </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnUpdate" runat="server" CausesValidation="False" CommandName="Edit" CssClass="text-primary " Text="Edit"></asp:LinkButton>
                                            &nbsp;<asp:LinkButton ID="lnkbtnDanger" runat="server" CausesValidation="False" CommandName="Delete" CssClass="text-danger " Text="Delete"></asp:LinkButton>
                                            <ajaxToolkit:ConfirmButtonExtender ID="lnkbtnDanger_ConfirmButtonExtender" runat="server" BehaviorID="lnkbtnDanger_ConfirmButtonExtender" ConfirmText='<%#"Are You Sure You Want To Delete "+ Eval("Server_Name")+"?" %>' TargetControlID="lnkbtnDanger" />

                                        </ItemTemplate>
                                        <HeaderStyle Font-Bold="False" HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right " CssClass="col-md-4" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Server Name" SortExpression="Server_Name">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtServerName"  Enabled="false"  runat="server" CssClass="form-control text-left  "  Text='<%# Bind("Server_Name") %>' BorderStyle="Solid"></asp:TextBox>
                                        </EditItemTemplate>
<%--                                        <HeaderTemplate>
                                            <asp:TextBox ID="txtNewServer" runat="server" OnTextChanged="txtNewServer_TextChanged" AutoPostBack="true"  CssClass=" form-control "></asp:TextBox>
                                        </HeaderTemplate>--%>
                                        <ItemTemplate>
                                            <asp:Label ID="lblServerName" CssClass="text-left " runat="server" Text='<%# Bind("Server_Name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left"  CssClass="col-md-8" />
                                    </asp:TemplateField>
                                   
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="col-md-3">
                        </div>
                    </div>

                    <asp:SqlDataSource runat="server" ID="SqlServers" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' DeleteCommand="DELETE FROM [Servers] WHERE [UID] = @UID" InsertCommand="INSERT INTO Servers(Server_Name) VALUES (@Server_Name)" SelectCommand="SELECT UID, Server_Name FROM Servers ORDER BY Server_Name" UpdateCommand="UPDATE [Servers] SET [Server_Name] = @Server_Name WHERE [UID] = @UID" OnDeleted="SqlServers_Deleted" OnInserted="SqlServers_Inserted" OnUpdated="SqlServers_Updated">
                        <DeleteParameters>
                            <asp:Parameter Name="UID" Type="Object"></asp:Parameter>
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:Parameter Name="Server_Name" />
                        </InsertParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="Server_Name" Type="String"></asp:Parameter>
                            <asp:Parameter Name="UID" Type="Object"></asp:Parameter>
                        </UpdateParameters>
                    </asp:SqlDataSource>




                    <asp:HiddenField ID="hfError" runat="server" />


                    <ajaxToolkit:ModalPopupExtender ID="hfError_ModalPopupExtender" runat="server" BehaviorID="hfError_ModalPopupExtender"  TargetControlID="hfError"
                      BackgroundCssClass=" modal-backdrop fade " CancelControlID="lnkbtnErrorCancel"   PopupControlID="pnlError">
                    </ajaxToolkit:ModalPopupExtender>


                    <asp:Panel ID="pnlError" CssClass =" modal-dialog text-center  " runat="server" Style="display: none">
                       
                            
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h3 class="modal-title">Error</h3>
                                    
                                    </div>
                                    <div class="modal-body">
                                          <asp:Label ID="lblError" runat="server" CssClass="h4 text-danger "  Visible="false" Text=""></asp:Label>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:LinkButton ID="lnkbtnErrorCancel" CssClass="btn btn-danger " runat="server">OK</asp:LinkButton>
                                    </div>
                                
                         
                        </div>
                    </asp:Panel>


                </div>





















            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

