<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Varieties.aspx.cs" Inherits="Crops_Varieties" %>

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


          
            <table>

                <tr>
                    <td>Search</td>
                    
                    <td>Commodity</td>
                </tr>

                <tr>

                    <td>
                        <asp:TextBox runat="server" ID="txtFilter" CssClass="form-control"  AutoPostBack="true"></asp:TextBox>
                    </td>

          
                    <td>
                        <asp:DropDownList runat="server" ID="ddCommodity" CssClass="form-control"  DataSourceID="sqlCommodities" DataTextField="Text" DataValueField="Crop" AutoPostBack="True"></asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="sqlCommodities" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT 'All Commodities' AS Text, NULL AS Crop, 0 AS idx UNION SELECT DISTINCT Crop AS Text, Crop, 1 AS idx FROM Crop_Varieties ORDER BY idx, Text"></asp:SqlDataSource>
                    </td>


                </tr>

            </table>




            <asp:GridView ID="GridView1" CssClass=" table table-bordered table-hover " HorizontalAlign="Center" runat="server" DataSourceID="SqlSeedPlantItems" AutoGenerateColumns="False" AllowSorting="True" DataKeyNames="UID">
                <Columns>
                    
                    <asp:BoundField DataField="Item_Id" HeaderText="ID" SortExpression="Item_Id"></asp:BoundField>
                    <asp:BoundField DataField="Department" HeaderText="Department" SortExpression="Department"></asp:BoundField>
                    <asp:BoundField DataField="Crop" HeaderText="Crop" SortExpression="Crop"></asp:BoundField>
                    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description"></asp:BoundField>
                    <asp:BoundField DataField="Class" HeaderText="Class" SortExpression="Class"></asp:BoundField>
                    <asp:CheckBoxField DataField="Inactive" HeaderText="Inactive" SortExpression="Inactive"></asp:CheckBoxField>
                    <asp:CheckBoxField DataField="NotInUse" HeaderText="NotInUse" SortExpression="NotInUse"></asp:CheckBoxField>
                </Columns>
           
                <EmptyDataTemplate>
                    <asp:Panel runat="server" ID="pe" HorizontalAlign="Center" Width="100%">
                        <h5 class=" text-center">No Varieties Matching Filter</h5>

                    </asp:Panel>



                </EmptyDataTemplate>

            </asp:GridView>
            <asp:SqlDataSource runat="server" ID="SqlSeedPlantItems" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT UID, Item_Id, Department, Crop, Description, Class, Inactive, NotInUse FROM Crop_Varieties WHERE (Crop = ISNULL(@Crop, Crop) AND Crop <> N'MISC') AND (Description LIKE N'%' + ISNULL(@Description, Description) + N'%') ORDER BY Crop, Description, Class" CancelSelectOnNullParameter="False" OnDataBinding="SqlSeedPlantItems_DataBinding">
                <SelectParameters>

                    <asp:ControlParameter ControlID="ddCommodity" PropertyName="SelectedValue" Name="Crop"></asp:ControlParameter>
                    <asp:ControlParameter ControlID="txtFilter" PropertyName="Text" Name="Description"></asp:ControlParameter>

                </SelectParameters>
            </asp:SqlDataSource>





    
         

        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>