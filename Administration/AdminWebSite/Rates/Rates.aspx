<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Rates.aspx.cs" Inherits="Rates_Rates" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h2>Hauler Rates</h2>
            <hr />
            <asp:GridView ID="GridView1" HorizontalAlign="Center" CssClass="table table-hover " runat="server" AutoGenerateColumns="False" DataKeyNames="UID" DataSourceID="SqlDataSource1">
                <Columns>
                   
                    <asp:BoundField DataField="Type" HeaderText="Type" ReadOnly="True" SortExpression="Type"></asp:BoundField>
                    <asp:BoundField DataField="Max_Distance" HeaderText="Max_Distance" SortExpression="Max_Distance"></asp:BoundField>
                    <asp:BoundField DataField="Rate" HeaderText="Rate" SortExpression="Rate"></asp:BoundField>
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT UID, CASE WHEN Type = 'A' THEN 'AlongSide The Field' ELSE 'Farm Storage' END AS Type, Max_Distance, Rate FROM Harvest_Rates ORDER BY Type, Max_Distance"></asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

