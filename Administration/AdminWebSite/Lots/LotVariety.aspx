<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="LotVariety.aspx.cs" Inherits="Lots_LotVariety" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            Getting Data..
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="text-center ">
                <h3>
                    <asp:Label ID="lblHeader" runat="server" Text="Producers"></asp:Label></h3>
                <p><asp:LinkButton runat="server" ID="Update" CssClass="btn btn-primary" PostBackUrl="~/AgvantageUpdate/Update.aspx" Text="Update From Agvantage" ></asp:LinkButton></p>
                <div class="container">
                   
                 

                   
                    
                    <asp:GridView ID="GridView1" runat="server" CssClass="table table-hover " HorizontalAlign="Center" AutoGenerateColumns="False" DataSourceID="SqlVarieties" AllowPaging="True" AllowSorting="True" PageSize="100"  DataKeyNames="Id">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkSelect" runat="server" OnClick="lnkSelect_Click" CssClass="btn btn-primary ">Select</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Id" HeaderText="Variety Id" ItemStyle-HorizontalAlign="Right" SortExpression="Id"></asp:BoundField>
                            <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-HorizontalAlign="Left" SortExpression="Description"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlVarieties" runat="server" ConnectionString="<%$ ConnectionStrings:NW_DataConnectionString %>" SelectCommand="SELECT NULL AS Id, 'Not Selected' AS Description, 0 AS idx UNION SELECT Item_Id AS id, Variety AS Description, 1 AS idx FROM VarietyList WHERE (Crop_Id = @Crop_Id) ORDER BY idx, Description" CancelSelectOnNullParameter="False">
                        <SelectParameters>
                            <asp:QueryStringParameter Name="Crop_Id" QueryStringField="Crop_Id" />
                        </SelectParameters>
                    </asp:SqlDataSource>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

