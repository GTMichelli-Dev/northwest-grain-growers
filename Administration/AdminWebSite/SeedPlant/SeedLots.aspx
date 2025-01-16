<%@ Page Title="Seed Lots" Language="C#" MasterPageFile="~/Site.master"  AutoEventWireup="true" CodeFile="SeedLots.aspx.cs" Inherits="SeedPlant_SeedLots" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h3>Seed Lots</h3>
    <asp:UpdatePanel runat="server" ID="UP1">
        <ContentTemplate>
            <div >
                <h3>Seed Variety Lots</h3>
                <div class="container">
                    <div class="row  form-group  ">
                        <div class="col-4">
                            <label for="ddLocations">Location</label>
                            <asp:DropDownList runat="server" ID="ddLocations" CssClass="form-control" AutoPostBack="true" DataSourceID="sqlLocations" DataTextField="Text" DataValueField="Value"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="sqlLocations" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT Description + ' - ' + LTRIM(STR(Id)) AS Text, Id AS Value, 1 AS idx FROM Locations WHERE (Id = 24) UNION SELECT Description + ' - ' + LTRIM(STR(Id)) AS Text, Id AS Value, 2 AS idx FROM Locations AS Locations_3 WHERE (Id = 25) OR (Id = 26) UNION SELECT Description + ' - ' + LTRIM(STR(Id)) AS Text, Id AS Value, 3 AS idx FROM Locations AS Locations_3 WHERE (Id <> 24) OR (Id <> 25) OR (Id <> 26) UNION SELECT 'Select Location' AS Text, NULL AS Id, 0 AS idx ORDER BY idx, Text"></asp:SqlDataSource>
                        </div>
                        <div class="col-4">
                            <label for="ddCrop">Crop</label>
                            <asp:DropDownList runat="server" ID="ddCrop" CssClass="form-control" AutoPostBack="true" DataSourceID="sqlCrop" DataTextField="Text" DataValueField="Value"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="sqlCrop" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT DISTINCT Crops.Description + ' - ' + LTRIM(STR(Crops.Id)) AS Text, Crops.UID AS Value, 1 AS idx FROM Crops INNER JOIN Variety_Crop_Groups ON Crops.UID = Variety_Crop_Groups.Crop_UID WHERE (Crops.Use_At_Seed_Mill = 1) UNION SELECT 'Select Crop' AS Text, NULL AS Id, 0 AS idx ORDER BY idx, Text"></asp:SqlDataSource>
                        </div>
                        <div class="col-4">
                            <label for="ddVariety">Variety</label>
                            <asp:DropDownList runat="server" ID="ddVariety" CssClass="form-control" AutoPostBack="true" DataSourceID="sqlVariety" DataTextField="Text" DataValueField="Value"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="sqlVariety" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT Variety.Description + ' - ' + LTRIM(STR(Variety.Id)) AS Text, Variety.UID AS Value, 1 AS idx FROM Variety INNER JOIN Variety_Crop_Groups ON Variety.UID = Variety_Crop_Groups.Variety_UID WHERE (Variety_Crop_Groups.Crop_UID = @CropUID) UNION SELECT '' AS Text, NULL AS Id, 0 AS idx ORDER BY idx, Text">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddCrop" PropertyName="SelectedValue" Name="CropUID"></asp:ControlParameter>
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </div>
                    <div class=" row text-center " >
                        <div class="col-sm-offset-3 col-sm-6">
                            <asp:GridView runat="server" ID="grdLots" CssClass="table table-hover " HorizontalAlign="Center" AutoGenerateColumns="False" DataKeyNames="UID" AllowPaging="True" AllowSorting="True" PageSize="100" DataSourceID="sqlLots">

                                <Columns>

                                    <asp:BoundField DataField="Variety" ItemStyle-CssClass="text-left " HeaderText="Variety" SortExpression="Variety"></asp:BoundField>
                                    <asp:BoundField DataField="Class" HeaderText="Class" ItemStyle-CssClass="text-left " SortExpression="Class"></asp:BoundField>
                                    <asp:TemplateField HeaderText="Lot" ItemStyle-CssClass="text-left " SortExpression="Lot">

                                        <ItemTemplate>
                                            <asp:TextBox runat="server" Text='<%# Bind("Lot") %>' ID="txtLot" Width="250px" OnTextChanged="txtLot_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                </Columns>
                                <EmptyDataTemplate>

                                    <asp:Panel runat="server" HorizontalAlign="Center" Width="100%">
                                        No Varieties Matching Filter
                                    </asp:Panel>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
    

                    <asp:SqlDataSource runat="server" ID="sqlLots" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT DISTINCT Seed_Variety_Lots.UID, Crops.Description AS Crop, Variety.Description AS Variety, Seed_Variety_Lots.Class, Seed_Variety_Lots.Lot, Seed_Variety_Lots.Location_Id, Variety_Crop_Groups.Variety_UID, Variety_Crop_Groups.Crop_UID FROM Seed_Variety_Lots INNER JOIN Variety_Crop_Groups ON Seed_Variety_Lots.Variety_Crop_Group_UID = Variety_Crop_Groups.UID INNER JOIN Variety ON Variety_Crop_Groups.Variety_UID = Variety.UID INNER JOIN Crops ON Variety_Crop_Groups.Crop_UID = Crops.UID WHERE (Seed_Variety_Lots.Location_Id = @Location_Id) AND (Variety_Crop_Groups.Variety_UID = ISNULL(@Variety_UID, Variety_Crop_Groups.Variety_UID)) AND (Variety_Crop_Groups.Crop_UID = @Crops) AND (Crops.Use_At_Seed_Mill = 1) ORDER BY Variety, Seed_Variety_Lots.Class" CancelSelectOnNullParameter="False">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddLocations" PropertyName="SelectedValue" Name="Location_Id"></asp:ControlParameter>
                            <asp:ControlParameter ControlID="ddVariety" PropertyName="SelectedValue" Name="Variety_UID"></asp:ControlParameter>
                            <asp:ControlParameter ControlID="ddCrop" PropertyName="SelectedValue" Name="Crops"></asp:ControlParameter>



                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </div>
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

