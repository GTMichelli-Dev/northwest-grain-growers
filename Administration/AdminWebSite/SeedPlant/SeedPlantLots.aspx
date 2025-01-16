<%@ Page Title="" Language="C#" MasterPageFile="~/Seed.master" AutoEventWireup="true" CodeFile="SeedPlantLots.aspx.cs" Inherits="SeedPlant_SeedPlantLots" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:UpdatePanel runat="server" ID="UP1">
        <ContentTemplate>
            <h4 class="text-center ">
                <asp:Label runat="server" ID="lblHeader"></asp:Label>
            </h4>
            <asp:GridView runat="server" ID="Grd1" HorizontalAlign="Center" AllowSorting="true" CssClass="table table-bordered table-hover table-sm " DataSourceID="sqlspItemLocations" AutoGenerateColumns="False" DataKeyNames="UID">
                <Columns>
                    <asp:BoundField DataField="Location_Id" HeaderText="Id" SortExpression="Location_Id"></asp:BoundField>
                    <asp:BoundField DataField="Location" ItemStyle-CssClass="text-left " HeaderText="Location" SortExpression="Location"></asp:BoundField>
                    <%--        <asp:BoundField DataField="Id" HeaderText="Id" ItemStyle-CssClass="text-left " SortExpression="Id"></asp:BoundField>
                    <asp:BoundField DataField="ItemDescription" ItemStyle-CssClass="text-left " HeaderText="Description" SortExpression="ItemDescription"></asp:BoundField>--%>
                    <asp:TemplateField HeaderText="Lot" SortExpression="Lot" ItemStyle-Width="150px">

                        <ItemTemplate>
                            <asp:TextBox runat="server" Text='<%# Bind("Lot") %>' AutoPostBack="true" CssClass="form-control " OnTextChanged="txtLot_TextChanged" ID="txtLot"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="100px" SortExpression="InUse">

                        <ItemTemplate>
                            <asp:Button runat="server" ID="btnInUse" OnClick="btnInUse_Click" Text='<%# Bind("InUse") %>' CssClass='<%# ((bool)Eval("NotInUse")==false)?"btn btn-outline-success":"btn btn-outline-danger" %>' />

                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Comment" SortExpression="Comment">

                        <ItemTemplate>
                            <asp:TextBox runat="server" Text='<%# Bind("Comment") %>' AutoPostBack="true" CssClass="form-control " OnTextChanged="txtComment_TextChanged" ID="txtComment"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Pure_Seed" SortExpression="Pure_Seed">

                        <ItemTemplate>
                            <asp:TextBox runat="server" Text='<%# Bind("Pure_Seed") %>' Enabled='<%# Eval("enabled") %>' AutoPostBack="true" OnTextChanged="txtPure_Seed_TextChanged"  CssClass="form-control " ID="txtPure_Seed"></asp:TextBox>

                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Other_Crop" SortExpression="Other_Crop">

                        <ItemTemplate>
                            <asp:TextBox runat="server" Text='<%# Bind("Other_Crop") %>'  Enabled='<%# Eval("enabled") %>'  OnTextChanged="txtOther_Crop_TextChanged" AutoPostBack="true" CssClass="form-control " ID="txtOther_Crop"></asp:TextBox>

                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Inert_Matter" SortExpression="Inert_Matter">

                        <ItemTemplate>
                            <asp:TextBox runat="server" Text='<%# Bind("Inert_Matter") %>'  Enabled='<%# Eval("enabled") %>'  OnTextChanged="txtInert_Matter_TextChanged"  AutoPostBack="true" CssClass="form-control " ID="txtInert_Matter"></asp:TextBox>

                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Germination" SortExpression="Germination">

                        <ItemTemplate>
                            <asp:TextBox runat="server" Text='<%# Bind("Germination") %>'  Enabled='<%# Eval("enabled") %>'  OnTextChanged="txtGermination_TextChanged" AutoPostBack="true" CssClass="form-control " ID="txtGermination"></asp:TextBox>

                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Weed_Seed" SortExpression="Weed_Seed">

                        <ItemTemplate>
                            <asp:TextBox runat="server" Text='<%# Bind("Weed_Seed") %>'  Enabled='<%# Eval("enabled") %>'  OnTextChanged="txtWeed_Seed_TextChanged" AutoPostBack="true" CssClass="form-control " ID="txtWeed_Seed"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Date_Tested" SortExpression="Date_Tested">
                        <ItemTemplate>
                            <asp:TextBox runat="server" Text='<%# Bind("Date_Tested") %>'  Enabled='<%# Eval("enabled") %>'  OnTextChanged="txtDate_Tested_TextChanged" AutoPostBack="true" CssClass="form-control " ID="txtDate_Tested"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <h5 class="text-center">No Locations Assigned In Agvantage To This Variety. Add A Location In Agvantage First</h5>
                </EmptyDataTemplate>

            </asp:GridView>
            <asp:SqlDataSource runat="server" ID="sqlspItemLocations" ConnectionString='<%$ ConnectionStrings:Seed_DataConnectionString %>' SelectCommand="SELECT        Item_Location.Location_ID, Locations.Description AS Location, Items.ID, Items.Description AS ItemDescription, Item_Location.Lot, Item_Location.Comment, Items.UID AS ItemUID, Item_Location.UID, 
                         Item_Location.NotInUse, CASE WHEN Item_Location.NotInUse = 0 THEN 'Active' ELSE 'Inactive' END AS InUse, Lot_Analysis.Pure_Seed, Lot_Analysis.Other_Crop, Lot_Analysis.Inert_Matter, 
                         Lot_Analysis.Germination, Lot_Analysis.Weed_Seed, Lot_Analysis.Date_Tested, CASE WHEN Lot_Analysis.Lot IS NULL THEN CONVERT(bit, 0) ELSE CONVERT(bit, 1) END AS enabled
FROM            Items INNER JOIN
                         Item_Location ON Items.ID = Item_Location.Id INNER JOIN
                         Locations ON Item_Location.Location_ID = Locations.ID LEFT OUTER JOIN
                         Lot_Analysis ON Item_Location.Lot = Lot_Analysis.Lot AND Items.ID = Lot_Analysis.Id AND Locations.ID = Lot_Analysis.Location_ID
 WHERE (Items.UID = @UID)">
                <SelectParameters>
                    <asp:QueryStringParameter QueryStringField="UID" Name="UID"></asp:QueryStringParameter>
                </SelectParameters>
            </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

