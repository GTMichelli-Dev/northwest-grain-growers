<%@ Page Title="" Language="C#" MasterPageFile="~/Seed.master" AutoEventWireup="true" CodeFile="Commodities.aspx.cs" Inherits="SeedPlant_Commodities" %>


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
    <h3><asp:Label ID="lblHeader" runat="server" Text="Commodities"></asp:Label></h3>

    <asp:UpdatePanel runat="server" ID="UP1">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hfUsed" />

            <asp:HiddenField runat="server" ID="hfUID" />
             <asp:HiddenField runat="server" ID="hfID" />
            <div class="row">
                <div class="col-3">
                    <asp:DropDownList runat="server" CssClass="form-control " ID="ddUsed" AutoPostBack="true" OnTextChanged="ddUsed_TextChanged">
                        <asp:ListItem Value="0">Used Commodities</asp:ListItem>
                        <asp:ListItem Value="1">All Commodities</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="col">
            <asp:GridView ID="GridView1" CssClass=" table table-bordered table-hover " HorizontalAlign="Center" runat="server" DataSourceID="SqlSeedPlantItems" AutoGenerateColumns="False" AllowSorting="True" DataKeyNames="UID,ID,Description,Seed_Color_UID" >
                <Columns>

                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Description" ItemStyle-CssClass=" text-left " HeaderText="Description" SortExpression="Description" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle CssClass=" text-left " HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Varieties" SortExpression="Id">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="btnVarieties" PostBackUrl='<%#"~/SeedPlant/Seed.aspx?Commodity_Id="+ Eval("Id") %>' Text="Varieties"></asp:LinkButton> 
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Spring&lt;/br&gt;Wheat" SortExpression="Spring_Wheat">

                        <ItemTemplate>
                            <asp:CheckBox runat="server" Checked='<%# Bind("Spring_Wheat") %>' Enabled="true" ID="ckSprintWheat" AutoPostBack="true" OnCheckedChanged="ckSprintWheat_CheckedChanged"></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="Not&lt;/br&gt;Used" SortExpression="Spring_Wheat">
                        <ItemTemplate>
                            <asp:CheckBox runat="server" Checked='<%# Bind("Not_Used") %>' Enabled="true" ID="ckNotUsed" AutoPostBack="true" OnCheckedChanged="ckNotUsed_CheckedChanged"></asp:CheckBox>








                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Color">
                        <ItemTemplate>

                            <div class="form-inline ">
                                <asp:Button runat="server" CssClass="btn btn-outline-dark mr-3  " Text="Set" ID="btnSetColor" OnClick="btnSetColor_Click" />
                                <asp:Label runat="server" Width="25px" CssClass="mr-1 " Text='<%# Eval("Primary_Color_Index").ToString() %>' BackColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("Primary_Color").ToString()) %>' ID="Label1"></asp:Label>
                                <asp:Label runat="server" CssClass="mr-3 " Text='<%# Bind("Primary_Color_Duration") %>' ID="Label2"></asp:Label>

                                <asp:Label runat="server" CssClass="mr-1 " Width="25px" Text='<%# Eval("Secondary_Color_Index").ToString() %>' BackColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("Secondary_Color").ToString()) %>' ID="Label3"></asp:Label>
                                <asp:Label runat="server" CssClass="mr-3 " Text='<%# Bind("Secondary_Color_Duration") %>' ID="Label4"></asp:Label>


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
                    

            <asp:SqlDataSource runat="server" ID="SqlSeedPlantItems" ConnectionString='<%$ ConnectionStrings:Seed_DataConnectionString %>' SelectCommand="SELECT DISTINCT Seed_Departments.UID, Seed_Departments.Id, Seed_Departments.Description, Seed_Departments.Spring_Wheat, Seed_Colors.UID AS Seed_Color_UID, Seed_Colors.Primary_Color_Index, Seed_Colors.Primary_Color_Duration, Seed_Colors.Secondary_Color_Index, Seed_Colors.Secondary_Color_Duration, CASE WHEN Primary_Color_Index IS NULL THEN CONVERT (bit , 0) ELSE CONVERT (bit , 1) END AS ColorSet, ISNULL(Primary_Color.Color, N'#ffffff') AS Primary_Color, ISNULL(Secondary_Color.Color, N'#ffffff') AS Secondary_Color, Seed_Departments.Not_Used FROM Seed_Departments LEFT OUTER JOIN Seed_Colors ON Seed_Departments.Id = Seed_Colors.Commodity_ID LEFT OUTER JOIN Colors AS Secondary_Color ON Seed_Colors.Secondary_Color_Index = Secondary_Color.ID LEFT OUTER JOIN Colors AS Primary_Color ON Seed_Colors.Primary_Color_Index = Primary_Color.ID WHERE (Seed_Departments.Not_Used = ISNULL(@Used, Seed_Departments.Not_Used)) ORDER BY Seed_Departments.Description" CancelSelectOnNullParameter="False" OnDataBinding="SqlSeedPlantItems_DataBinding">
                <SelectParameters>
                    <asp:ControlParameter ControlID="hfUsed" PropertyName="Value" Name="Used"></asp:ControlParameter>
                </SelectParameters>
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
                            
                                <div class="form-row">
                                    Primary Color
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
                                        <asp:Panel runat="server"  Width="100%"  ID="pnlPrimaryValues">
                                            <div class="form-inline ">

                                                <asp:Label runat="server" CssClass="mr-2" Width="25px" ID="lblPrimaryPopupColor"></asp:Label>
                                                <asp:Panel runat="server" ID="pnlPrimaryDuration">
                                                    <asp:Label runat="server" CssClass="ml-3 mr-2">Duration (Seconds)</asp:Label>
                                                    <asp:TextBox runat="server" ID="txtPrimaryDuration" Width="80px" AutoPostBack="true" AutoCompleteType="Disabled" OnTextChanged="txtPrimaryDuration_TextChanged" MaxLength="6" CssClass="form-control  text-right " onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                </asp:Panel>
                                                <asp:Button runat="server" ID="btnClearPrimaryColor" OnClick="btnClearPrimaryColor_Click" CssClass="btn btn-outline-danger  ml-2 " Text="Clear" />
                                            </div>
                                        </asp:Panel>
                                    </div>

                                </div>
                            

                            <br />
                            <asp:Panel runat="server" ID="pnlSecondary">
                            
                                    <div class="form-row">
                                        Secondary Color
                                    </div>

                                    <div class="form-row">


                                        <asp:Table runat="server" ID="tblSecondaryColor" CssClass="table table-bordered table-sm  " HorizontalAlign="Center">

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
                                                    <asp:Button runat="server" Width="20px" CssClass="btn font-weight-bold  "  Text="0" OnClick="ColorButton_Click" ID="Button10" BackColor="#ffffff" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:Button runat="server" Width="20px" CssClass="btn font-weight-bold  "  Text="1" OnClick="ColorButton2_Click" ID="Button11" BackColor="#ffffff" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:Button runat="server" Width="20px" CssClass="btn font-weight-bold  "  Text="2" OnClick="ColorButton2_Click" ID="Button12" BackColor="#ffffff" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:Button runat="server" Width="20px" CssClass="btn font-weight-bold  "  Text="3" OnClick="ColorButton2_Click" ID="Button13" BackColor="#ffffff" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:Button runat="server" Width="20px" CssClass="btn font-weight-bold  "  Text="4" OnClick="ColorButton2_Click" ID="Button14" BackColor="#ffffff" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:Button runat="server" Width="20px" CssClass="btn font-weight-bold  "  Text="5" OnClick="ColorButton2_Click" ID="Button15" BackColor="#ffffff" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:Button runat="server" Width="20px" CssClass="btn font-weight-bold  "  Text="6" OnClick="ColorButton2_Click" ID="Button16" BackColor="#ffffff" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:Button runat="server" Width="20px" CssClass="btn font-weight-bold  "  Text="7" OnClick="ColorButton2_Click" ID="Button17" BackColor="#ffffff" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:Button runat="server" Width="20px" CssClass="btn font-weight-bold  "  Text="8" OnClick="ColorButton2_Click" ID="Button18" BackColor="#ffffff" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:Button runat="server" Width="20px" CssClass="btn font-weight-bold  "  Text="9" OnClick="ColorButton2_Click" ID="Button19" BackColor="#ffffff" />
                                                </asp:TableCell>

                                            </asp:TableRow>
                                        </asp:Table>
                                    </div>
                                    <div class="form-row">
                                        <div class="form-inline ">
                                            <asp:Panel runat="server" Width="100%"  ID="pnlSecondaryValues">
                                                 <div class="form-inline ">
                                                    <asp:Label runat="server" CssClass="mr-2" Width="25px" ID="lblSecondaryPopupColor"></asp:Label>
                                                    <asp:Panel runat="server" ID="pnlSecondaryDuration">
                                                        <asp:Label runat="server" CssClass="ml-3 mr-2">Duration (Seconds)</asp:Label>
                                                        <asp:TextBox runat="server" ID="txtSecondaryDuration" Width="80px"  AutoPostBack="true" AutoCompleteType="Disabled" OnTextChanged="txtSecondaryDuration_TextChanged" MaxLength="6" CssClass="form-control  text-right " onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                    </asp:Panel>
                                                    <asp:Button runat="server" ID="btnClearSecondaryColor" OnClick="btnClearSecondaryColor_Click" CssClass="btn btn-outline-danger ml-2 " Text="Clear" />
                                               </div>
                                            </asp:Panel>
                                        </div>
                                    </div>
                            
                            </asp:Panel>

                        </ContentTemplate>

                    </asp:UpdatePanel>

                </div>
                <div class="modal-footer">


                  
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Done</button>
                </div>
            </div>
        </div>

    </div>
   </asp:Content>

