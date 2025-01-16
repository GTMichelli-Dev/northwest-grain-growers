<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="SeedVarieties.aspx.cs" Inherits="SeedPlant_SeedVarieties" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
       <h2>Seed Varieties</h2>

    <asp:UpdatePanel runat="server" ID="Up1">


        <ContentTemplate>
            
            <div class=" form-row">
                <div class="col-12 ">
                    <asp:GridView runat="server" ID="Grd1" HorizontalAlign="Center" Width="100%" CssClass="table table-hover table-sm table-bordered  " AutoGenerateColumns="False" DataSourceID="SqlDataSource1" AllowSorting="True">


                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID"></asp:BoundField>
                            <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description"></asp:BoundField>
                            <asp:CheckBoxField DataField="Inactive" HeaderText="Inactive" SortExpression="Inactive"></asp:CheckBoxField>
                            <asp:CheckBoxField DataField="NotInUse" HeaderText="NotInUse" SortExpression="NotInUse"></asp:CheckBoxField>
                            <asp:BoundField DataField="Comment" HeaderText="Comment" SortExpression="Comment"></asp:BoundField>
                        </Columns>
                    </asp:GridView>

                    <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:Seed_DataConnectionString %>' SelectCommand="SELECT [ID], [Description], [Inactive], [NotInUse], [Comment] FROM [Items] WHERE ([ItemType] = @ItemType) ORDER BY [Description]">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="Seed" Name="ItemType" Type="String"></asp:Parameter>
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </div>


        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

