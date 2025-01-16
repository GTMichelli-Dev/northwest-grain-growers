<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Update.aspx.cs" Inherits="Update" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <img runat="server" alt="Wait" id="Wait" src="../Wait.gif" />
        </ContentTemplate>
    </asp:UpdatePanel>


    <asp:HiddenField ID="hfStartingTime" runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <asp:Label ID="lblPrompt" runat="server" CssClass="h3 text-center " Text="Connecting To Agvantage"></asp:Label>
            <asp:Timer ID="Timer2" Interval="1500" Enabled="false" OnTick="Timer2_Tick" runat="server"></asp:Timer>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>

