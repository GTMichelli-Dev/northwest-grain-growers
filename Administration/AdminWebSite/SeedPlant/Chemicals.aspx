<%@ Page Title="Varieties" Language="C#" MasterPageFile="~/Seed.master" AutoEventWireup="true" CodeFile="Chemicals.aspx.cs" Inherits="SeedPlant_Chemicals" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdateProgress runat="server" ID="UPr">
        <ProgressTemplate >
            <h2 class="text-info text-center  ">Hang On... Getting Data</h2>
         </ProgressTemplate>
    </asp:UpdateProgress>
    <h3><asp:Label ID="lblHeader" runat="server" Text="Chemicals"></asp:Label></h3>
    <asp:UpdatePanel runat="server" ID="UP1">
        <ContentTemplate>



            
            
          
            <table>

                <tr>
                    <td>Search</td>
                    <td>Location</td>
                </tr>

                <tr>

                    <td>
                        <asp:TextBox runat="server" ID="txtFilter" CssClass="form-control"  AutoPostBack="true"></asp:TextBox>
                    </td>

                    <td>
                        <asp:DropDownList runat="server" ID="cboLocation" CssClass="form-control" OnTextChanged="cboLocation_TextChanged"  DataSourceID="sqlLocation" DataTextField="Text" DataValueField="Id" AutoPostBack="True"></asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="sqlLocation" ConnectionString='<%$ ConnectionStrings:Seed_DataConnectionString %>' SelectCommand="SELECT 'All Locations' AS Text, NULL AS Id, 0 AS idx UNION SELECT DISTINCT Locations.Description + ' - ' + LTRIM(STR(Locations.ID)) AS Text, Locations.ID, 1 AS idx FROM Locations INNER JOIN Item_Location ON Locations.ID = Item_Location.Location_ID ORDER BY idx, Text"></asp:SqlDataSource>
                    </td>


                </tr>

            </table>




            <asp:GridView ID="GridView1" CssClass=" table table-bordered table-hover " HorizontalAlign="Center" runat="server" DataSourceID="SqlSeedChemicals" AutoGenerateColumns="False" AllowSorting="True" DataKeyNames="UID,Item_Location_UID" >
                <Columns>

                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                    <asp:BoundField DataField="Description" ItemStyle-CssClass=" text-left " HeaderText="Description" SortExpression="Description" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                       <%--   <asp:TemplateField >
                          
                            <ItemTemplate>
                                <asp:Button runat="server" ID="btnLots" CssClass="btn btn-outline-dark   " PostBackUrl='<%#"SeedPlantLots.aspx?UID="+Eval("UID").ToString() %>' Text="Lots" />
                                
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Status" SortExpression="InUse">
                          
                            <ItemTemplate>
                                <asp:Button runat="server" ID="btnInUse"   OnClick="btnInUse_Click" Width="100%"  text='<%# Bind("InUse") %>' CssClass='<%# ((bool)Eval("NotInUse")==false)?"btn btn-outline-success":"btn btn-outline-danger" %>'    />
                                
                            </ItemTemplate>
                        </asp:TemplateField>
                    <asp:TemplateField HeaderText="Comment" SortExpression="Comment">
                      
                        <ItemTemplate>
                            <asp:TextBox runat="server" Text='<%# Bind("Comment") %>' ID="txtComment" CssClass="form-control " AutoPostBack="true" OnTextChanged="txtComment_TextChanged"  ></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>


                </Columns>
                <EmptyDataTemplate>
                    <asp:Panel runat="server" ID="pe" HorizontalAlign="Center" Width="100%">
                        <h5 class=" text-center">No Chemicals Matching Filter</h5>

                    </asp:Panel>



                </EmptyDataTemplate>

            </asp:GridView>
            <asp:SqlDataSource runat="server" ID="SqlSeedChemicals" ConnectionString='<%$ ConnectionStrings:Seed_DataConnectionString %>' SelectCommand="SELECT DISTINCT Items.UID, Items.ID, Items.Description, Items.ItemType, Items.FLC, Items.UOMCode, Item_Location.Inactive, Items.Comment, CASE WHEN Item_Location.NotInUse = 0 THEN 'Active' ELSE 'Inactive' END AS InUse, Item_Location.NotInUse, Item_Location.UID AS Item_Location_UID, ISNULL(@Location, - 1) AS SelectedLocation FROM Items LEFT OUTER JOIN Item_Location ON Items.ID = Item_Location.Id WHERE (Items.ItemType = N'Chemical') AND (Items.Description LIKE N'%' + ISNULL(@Filter, Items.Description) + N'%') AND (ISNULL(Item_Location.Location_ID, '') = ISNULL(@Location, ISNULL(Item_Location.Location_ID, ''))) AND (Item_Location.Inactive = 0) OR (Items.ItemType = N'Chemical') AND (ISNULL(Item_Location.Location_ID, '') = ISNULL(@Location, ISNULL(Item_Location.Location_ID, ''))) AND (LTRIM(STR(Items.ID)) LIKE N'%' + ISNULL(@Filter, LTRIM(STR(Items.ID))) + N'%') AND (Item_Location.Inactive = 0) ORDER BY Items.Description" CancelSelectOnNullParameter="False">
                <SelectParameters>
                    <asp:ControlParameter ControlID="cboLocation" PropertyName="SelectedValue" Name="Location"></asp:ControlParameter>
                    <asp:ControlParameter ControlID="txtFilter" PropertyName="Text" Name="Filter"></asp:ControlParameter>

                </SelectParameters>
            </asp:SqlDataSource>
         

         

        </ContentTemplate>

    </asp:UpdatePanel>
   </asp:Content>


