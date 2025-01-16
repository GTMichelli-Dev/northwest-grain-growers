<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="PLCData.aspx.cs" Inherits="PLCData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel runat="server" ID="UP1">
        <ContentTemplate>
            <asp:Timer runat="server" ID="tmrUpdate" Interval="1000" Enabled="true"></asp:Timer>

            <asp:GridView runat="server" ID="grdStats" HorizontalAlign="Center" AutoGenerateColumns="true">
            </asp:GridView>

            <asp:GridView runat="server" ID="grdBins" HorizontalAlign="Center" AutoGenerateColumns="true">
            </asp:GridView>

            <asp:GridView runat="server" ID="grdTreatments" HorizontalAlign="Center" AutoGenerateColumns="true">
            </asp:GridView>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentFull" Runat="Server">
</asp:Content>

