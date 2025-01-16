<%@ Page Title="" Language="C#" MasterPageFile="~/Seed.master" AutoEventWireup="true" CodeFile="Traits.aspx.cs" Inherits="SeedPlant_Traits" %>

  
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="Server">
    <script>
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if ((charCode > 31 && (charCode < 48 || charCode > 57)) && (charCode != 46))
                return false;
            return true;
        }

    </script>
    <asp:UpdateProgress runat="server" ID="UPr">
        <ProgressTemplate >
            <h2 class="text-info text-center  ">Hang On... Getting Data</h2>
         </ProgressTemplate>
    </asp:UpdateProgress>
    <h3><asp:Label ID="lblHeader" runat="server" Text="Traits"></asp:Label></h3>

    <asp:UpdatePanel runat="server" ID="UP1">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hfUsed" />

            <asp:HiddenField runat="server" ID="hfUID" />
             <asp:HiddenField runat="server" ID="hfID" />
           <div class="row">
               <div class="col-2">
                   <asp:LinkButton runat="server" ID="btnAddNew" Text="New Trait" CssClass="btn btn-primary " Visible="false" OnClick="btnAddNew_Click" ></asp:LinkButton>
               </div>
           </div>
            <div class="row">
                <div class="col">
            <asp:GridView ID="GridView1" CssClass=" table table-bordered table-hover " HorizontalAlign="Center" runat="server" DataSourceID="SqlSeedPlantItems" AutoGenerateColumns="False" AllowSorting="True" DataKeyNames="UID,ID,Description" >
                <Columns>

                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Description" ItemStyle-CssClass=" text-left " HeaderText="Description" SortExpression="Description" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle CssClass=" text-left " HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Color">
                        <ItemTemplate>
                            <div class="form-inline ">
                                <asp:Button runat="server" CssClass="btn btn-outline-dark mr-3  " Text="Set" ID="btnSetColor" OnClick="btnSetColor_Click" />
                                <asp:Label runat="server" Width="25px" CssClass="mr-1 " Text='<%# Eval("Color_Index").ToString() %>' BackColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("Color").ToString()) %>' ID="Label1"></asp:Label>
                                <asp:Label runat="server" CssClass="mr-3 " Text='<%# Bind("Duration") %>' ID="Label2"></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <asp:Panel runat="server" ID="pe" HorizontalAlign="Center" Width="100%">
                        <h5 class=" text-center">No Commodities Matching Filter</h5>

                    </asp:Panel>



                </EmptyDataTemplate>

            </asp:GridView>
                </div>
            </div>
                    

            <asp:SqlDataSource runat="server" ID="SqlSeedPlantItems" ConnectionString='<%$ ConnectionStrings:Seed_DataConnectionString %>' SelectCommand="SELECT DISTINCT Trait.UID, Trait.ID, Trait.Description, Trait.Color_Index, ISNULL(Colors.Color, N'#ffffff') AS Color, Trait.Duration FROM Trait INNER JOIN Colors ON Trait.Color_Index = Colors.ID ORDER BY Trait.Description" CancelSelectOnNullParameter="False" OnDataBinding="SqlSeedPlantItems_DataBinding">
            </asp:SqlDataSource>
         
    
         

        </ContentTemplate>

    </asp:UpdatePanel>








     <%-- Message --%>
    <div id="ColorPopup" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:UpdatePanel runat="server" ID="UPColorPopUpHeader" UpdateMode="Conditional">

                        <ContentTemplate>
                            <h5 class="modal-title "><asp:Label runat="server" ID="lblColorPopupHeader"></asp:Label></h5>

                        </ContentTemplate>

                    </asp:UpdatePanel>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UPColor" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                                <asp:Panel runat="server" ID="pnlNew" Width="100%" HorizontalAlign="Center" >
                                    <div class="row ">
                                        <div class=" col-12">
                                            <asp:Label runat="server" ID="lblNewError" CssClass="h4 text-danger "></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-2 form-group ">
                                            <label for="txtNewTraitID">Trait ID</label>
                                            <asp:TextBox runat="server" ID="txtNewTraitID"  CssClass="form-control  text-right " onkeypress="return isNumberKey(event)"></asp:TextBox>
                                        </div>
                                        
                                        
                                    </div>
                                    <div class="row">
                                        <div class="col form-group ">
                                            <label for="txtNewTraitDescription">Description</label>
                                            <asp:TextBox runat="server" ID="txtNewTraitDescription"  CssClass="form-control "></asp:TextBox>
                                        </div>
                                        
                                        
                                    </div>


                                </asp:Panel>
                                <div class="form-row">
                                    Color
                                </div>
                                <div class="form-row">


                                    <asp:Table runat="server" ID="tblPrimaryColors" CssClass="table table-bordered table-sm  " HorizontalAlign="Center">

                                        <asp:TableRow>
                                            <asp:TableHeaderCell>Index</asp:TableHeaderCell>
                                            <asp:TableCell>0</asp:TableCell>
                                            <asp:TableCell>1</asp:TableCell>
                                            <asp:TableCell>2</asp:TableCell>
                                            <asp:TableCell>3</asp:TableCell>
                                            <asp:TableCell>4</asp:TableCell>
                                            <asp:TableCell>5</asp:TableCell>
                                            <asp:TableCell>6</asp:TableCell>
                                            <asp:TableCell>7</asp:TableCell>
                                            <asp:TableCell>8</asp:TableCell>
                                            <asp:TableCell>9</asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableHeaderCell>Color</asp:TableHeaderCell>
                                            <asp:TableCell>
                                                <asp:Button runat="server" Width="20px" CssClass="btn font-weight-bold  " Text="0" OnClick="ColorButton_Click" ID="Button0" BackColor="#ffffff" />
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:Button runat="server" Width="20px" CssClass="btn font-weight-bold  " Text="1"  OnClick="ColorButton_Click" ID="Button1" BackColor="#ffffff" />
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:Button runat="server" Width="20px" CssClass="btn font-weight-bold  " Text="2"  OnClick="ColorButton_Click" ID="Button2" BackColor="#ffffff" />
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:Button runat="server" Width="20px" CssClass="btn font-weight-bold  " Text="3"  OnClick="ColorButton_Click" ID="Button3" BackColor="#ffffff" />
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:Button runat="server" Width="20px" CssClass="btn font-weight-bold  " Text="4"  OnClick="ColorButton_Click" ID="Button4" BackColor="#ffffff" />
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:Button runat="server" Width="20px" CssClass="btn font-weight-bold  " Text="5"  OnClick="ColorButton_Click" ID="Button5" BackColor="#ffffff" />
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:Button runat="server" Width="20px" CssClass="btn font-weight-bold  " Text="6"  OnClick="ColorButton_Click" ID="Button6" BackColor="#ffffff" />
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:Button runat="server" Width="20px" CssClass="btn font-weight-bold  "  Text="7" OnClick="ColorButton_Click" ID="Button7" BackColor="#ffffff" />
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:Button runat="server" Width="20px" CssClass="btn font-weight-bold  " Text="8"  OnClick="ColorButton_Click" ID="Button8" BackColor="#ffffff" />
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:Button runat="server" Width="20px" CssClass="btn font-weight-bold  "  Text="9" OnClick="ColorButton_Click" ID="Button9" BackColor="#ffffff" />
                                            </asp:TableCell>

                                        </asp:TableRow>
                                    </asp:Table>
                                </div>
                            <div class="form-row">


                                <div class="form-inline ">
                                    <asp:Panel runat="server" Width="100%" ID="pnlPrimaryValues">
                                        <div class="form-inline ">

                                            <asp:Label runat="server" CssClass="mr-2" Width="25px" ID="lblPrimaryPopupColor"></asp:Label>
                                            <asp:Panel runat="server" ID="pnlPrimaryDuration">
                                                <asp:Label runat="server" CssClass="ml-3 mr-2">Duration (Seconds)</asp:Label>
                                                <asp:TextBox runat="server" ID="txtPrimaryDuration" Width="80px" AutoPostBack="true" AutoCompleteType="Disabled" OnTextChanged="txtPrimaryDuration_TextChanged" MaxLength="6" CssClass="form-control  text-right " onkeypress="return isNumberKey(event)"></asp:TextBox>
                                            </asp:Panel>
                                        
                                        </div>
                                    </asp:Panel>
                                </div>

                            </div>
                            

                        </ContentTemplate>

                    </asp:UpdatePanel>

                </div>
                <div class="modal-footer">

                    <asp:UpdatePanel runat="server" ID="UPFooter" UpdateMode="Conditional" >
                        <ContentTemplate>
                            <asp:Button runat="server" ID="btnOK" CssClass="btn btn-primary  " Text="Save" OnClick="btnOK_Click"  />
                             <button type="button" runat="server" id="btnCancel" class="btn btn-secondary" data-dismiss="modal">Done</button>        
                        </ContentTemplate>
                    </asp:UpdatePanel>
                  
                    
                </div>
            </div>
        </div>

    </div>
   </asp:Content>


