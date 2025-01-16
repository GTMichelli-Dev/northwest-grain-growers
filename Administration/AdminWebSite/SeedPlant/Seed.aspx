<%@ Page Title="Varieties" Language="C#" MasterPageFile="~/Seed.master" AutoEventWireup="true" CodeFile="Seed.aspx.cs" Inherits="SeedPlant_Lots" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
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
    <h3><asp:Label ID="lblHeader" runat="server" Text="Varieties"></asp:Label></h3>
   
    <asp:UpdatePanel runat="server" ID="UP1">
        <ContentTemplate>


            <asp:HiddenField runat="server" ID="hfUID" />
             <asp:HiddenField runat="server" ID="hfID" />
            
            
          
            <table>

                <tr>
                    <td>Search</td>
                    <td>Location</td>
                    <td>Commodity</td>
                </tr>

                <tr>

                    <td>
                        <asp:TextBox runat="server" ID="txtFilter" CssClass="form-control"  AutoPostBack="true"></asp:TextBox>
                    </td>

                    <td>
                        <asp:DropDownList runat="server" ID="cboLocation" CssClass="form-control" OnTextChanged="cboLocation_TextChanged"  DataSourceID="sqlLocation" DataTextField="Text" DataValueField="Id" AutoPostBack="True"></asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="sqlLocation" ConnectionString='<%$ ConnectionStrings:Seed_DataConnectionString %>' SelectCommand="SELECT 'All Locations' AS Text, NULL AS Id, 0 AS idx UNION SELECT DISTINCT Locations.Description + ' - ' + LTRIM(STR(Locations.ID)) AS Text, Locations.ID, 1 AS idx FROM Locations INNER JOIN Item_Location ON Locations.ID = Item_Location.Location_ID ORDER BY idx, Text"></asp:SqlDataSource>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddCommodity" CssClass="form-control" OnTextChanged="cboLocation_TextChanged" DataSourceID="sqlCommodities" DataTextField="Text" DataValueField="Id" AutoPostBack="True"></asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="sqlCommodities" ConnectionString='<%$ ConnectionStrings:Seed_DataConnectionString %>' SelectCommand="SELECT 'All Commodities' AS Text, NULL AS Id, 0 AS idx UNION SELECT DISTINCT Description + N' - ' + LTRIM(STR(Id)) AS Text, Id, 1 AS idx FROM Seed_Departments WHERE (Not_Used = 0) ORDER BY idx, Text"></asp:SqlDataSource>
                    </td>


                </tr>

            </table>




            <asp:GridView ID="GridView1" CssClass=" table table-bordered table-hover " HorizontalAlign="Center" runat="server" DataSourceID="SqlSeedPlantItems" AutoGenerateColumns="False" AllowSorting="True" DataKeyNames="UID,ID,Description,ItemLocation_UID" >
                <Columns>

                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Description" ItemStyle-CssClass=" text-left " HeaderText="Description" SortExpression="Description" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle CssClass=" text-left " HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:TemplateField>

                        <ItemTemplate>
                            <asp:Button runat="server" ID="btnLots" CssClass="btn btn-outline-dark   " PostBackUrl='<%#"SeedPlantLots.aspx?UID="+Eval("UID").ToString() %>' Text="Lots" />

                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Status" SortExpression="InUse">

                        <ItemTemplate>
                            <asp:Button runat="server" ID="btnInUse" OnClick="btnInUse_Click" Width="100%" Text='<%# Bind("InUse") %>' CssClass='<%# ((bool)Eval("NotInUse")==false)?"btn btn-outline-success":"btn btn-outline-danger" %>' />

                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Comment" SortExpression="Comment">

                        <ItemTemplate>
                            <asp:TextBox runat="server" Text='<%# Bind("Comment") %>' ID="txtComment" CssClass="form-control " AutoPostBack="true" OnTextChanged="txtComment_TextChanged"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>



                </Columns>
                <EmptyDataTemplate>
                    <asp:Panel runat="server" ID="pe" HorizontalAlign="Center" Width="100%">
                        <h5 class=" text-center">No Seed Matching Filter</h5>

                    </asp:Panel>



                </EmptyDataTemplate>

            </asp:GridView>
            <asp:SqlDataSource runat="server" ID="SqlSeedPlantItems" ConnectionString='<%$ ConnectionStrings:Seed_DataConnectionString %>' SelectCommand="SELECT DISTINCT Items.UID, Item_Location.UID AS ItemLocation_UID, Items.ID, Items.Description, Items.Comment, CASE WHEN Item_Location.NotInUse = 0 THEN 'Active' ELSE 'Inactive' END AS InUse, Item_Location.NotInUse, ISNULL(@Location, - 1) AS SelectedLocation, Items.Dept, Seed_Departments.Description AS Commodity, Item_Location.Inactive FROM Seed_Departments INNER JOIN Items ON Seed_Departments.Id = Items.Dept LEFT OUTER JOIN Item_Location ON Items.ID = Item_Location.Id WHERE (Items.ItemType = N'Seed') AND (Items.Description LIKE N'%' + ISNULL(@Filter, Items.Description) + N'%') AND (Items.Inactive = 0) AND (ISNULL(Item_Location.Location_ID, '') = ISNULL(@Location, ISNULL(Item_Location.Location_ID, ''))) AND (Seed_Departments.Not_Used = 0) AND (Items.Dept = ISNULL(@Dept, Items.Dept)) AND (NOT (Item_Location.UID IS NULL)) AND (Items.NotInUse = 0) AND (Item_Location.Inactive = 0) OR (Items.ItemType = N'Seed') AND (Items.Inactive = 0) AND (ISNULL(Item_Location.Location_ID, '') = ISNULL(@Location, ISNULL(Item_Location.Location_ID, ''))) AND (Seed_Departments.Not_Used = 0) AND (Items.Dept = ISNULL(@Dept, Items.Dept)) AND (NOT (Item_Location.UID IS NULL)) AND (LTRIM(STR(Items.ID)) LIKE N'%' + ISNULL(@Filter, LTRIM(STR(Items.ID))) + N'%') AND (Items.NotInUse = 0) AND (Item_Location.Inactive = 0) ORDER BY Items.Description" CancelSelectOnNullParameter="False" OnDataBinding="SqlSeedPlantItems_DataBinding">
                <SelectParameters>
                    <asp:ControlParameter ControlID="cboLocation" PropertyName="SelectedValue" Name="Location"></asp:ControlParameter>
                    <asp:ControlParameter ControlID="txtFilter" PropertyName="Text" Name="Filter"></asp:ControlParameter>
                    <asp:ControlParameter ControlID="ddCommodity" PropertyName="SelectedValue" Name="Dept"></asp:ControlParameter>

                </SelectParameters>
            </asp:SqlDataSource>






            <asp:GridView ID="GridView2" CssClass=" table table-bordered table-hover " HorizontalAlign="Center" runat="server" DataSourceID="SqlSeedPlantItemsAll" AutoGenerateColumns="False" AllowSorting="True" DataKeyNames="UID,ID,Description" >
                <Columns>

                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Description" ItemStyle-CssClass=" text-left " HeaderText="Description" SortExpression="Description" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle CssClass=" text-left " HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:TemplateField>

                        <ItemTemplate>
                            <asp:Button runat="server" ID="btnLots" CssClass="btn btn-outline-dark   " PostBackUrl='<%#"SeedPlantLots.aspx?UID="+Eval("UID").ToString() %>' Text="Lots" />

                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Comment" SortExpression="Comment">

                        <ItemTemplate>
                            <asp:TextBox runat="server" Text='<%# Bind("Comment") %>' ID="txtComment" CssClass="form-control " AutoPostBack="true" OnTextChanged="txtComment_TextChanged"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>



                </Columns>
                <EmptyDataTemplate>
                    <asp:Panel runat="server" ID="pe" HorizontalAlign="Center" Width="100%">
                        <h5 class=" text-center">No Seed Matching Filter</h5>

                    </asp:Panel>



                </EmptyDataTemplate>

            </asp:GridView>
         

            <asp:SqlDataSource runat="server" ID="SqlSeedPlantItemsAll" ConnectionString='<%$ ConnectionStrings:Seed_DataConnectionString %>' SelectCommand="SELECT DISTINCT Items.UID, Items.ID, Items.Description, Items.Comment, Items.Dept, Seed_Departments.Description AS Commodity FROM Seed_Departments INNER JOIN Items ON Seed_Departments.Id = Items.Dept WHERE (Items.ItemType = N'Seed') AND (Items.Description LIKE N'%' + ISNULL(@Filter, Items.Description) + N'%') AND (Items.Inactive = 0) AND (Seed_Departments.Not_Used = 0) AND (Items.Dept = ISNULL(@Dept, Items.Dept)) AND (Items.NotInUse = 0) OR (Items.ItemType = N'Seed') AND (Items.Inactive = 0) AND (Seed_Departments.Not_Used = 0) AND (Items.Dept = ISNULL(@Dept, Items.Dept)) AND (Items.NotInUse = 0) AND (LTRIM(STR(Items.ID)) LIKE N'%' + ISNULL(@Filter, LTRIM(STR(Items.ID))) + N'%') ORDER BY Items.Description" CancelSelectOnNullParameter="False" OnDataBinding="SqlSeedPlantItems_DataBinding">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtFilter" PropertyName="Text" Name="Filter"></asp:ControlParameter>
                    <asp:ControlParameter ControlID="ddCommodity" PropertyName="SelectedValue" Name="Dept"></asp:ControlParameter>

                </SelectParameters>
            </asp:SqlDataSource>
         
    
         

        </ContentTemplate>

    </asp:UpdatePanel>

    

   
   </asp:Content>


