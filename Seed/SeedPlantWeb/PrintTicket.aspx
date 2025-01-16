<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="PrintTicket.aspx.cs" Inherits="PrintTicket" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:UpdatePanel runat="server" ID="UP1">
        <ContentTemplate>
            <asp:Timer runat="server" ID="tmrPrint" OnTick="tmrPrint_Tick" Interval="500" Enabled="true"></asp:Timer>
            <asp:HiddenField runat="server" ID="hfCnt" Value="0" />

            <br />
            <br />
            <br />
            <br />

            <h1 runat="server" class=" font-weight-bold  text-center  ">
                <asp:Label runat="server" ID="lblHeader"  Width="100%"></asp:Label>
            </h1>


            <br />
            <br />
            <br />
            <br />


        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentFull" Runat="Server">
</asp:Content>

