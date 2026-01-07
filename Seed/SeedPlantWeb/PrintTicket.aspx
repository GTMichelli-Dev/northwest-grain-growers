<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="PrintTicket.aspx.cs" Inherits="PrintTicket" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div style="text-align: center; margin-top: 20px;">
        <asp:Label runat="server" ID="lblCopies" Text="Number Of Copies" Font-Size="2em"></asp:Label>
        <br />
        <asp:DropDownList runat="server" ID="ddlCopies"
            Style="width: 80px; text-align:center; height: 80px; font-size: 2em;" onchange="showHeader()">
            <asp:ListItem Text="0" Value=""></asp:ListItem>
            <asp:ListItem Text="1" Value="1"></asp:ListItem>
            <asp:ListItem Text="2" Value="2"></asp:ListItem>
            <asp:ListItem Text="3" Value="3"></asp:ListItem>
            <asp:ListItem Text="4" Value="4"></asp:ListItem>
        </asp:DropDownList>
    </div>
    <asp:UpdatePanel runat="server" ID="UP1">
        <ContentTemplate>
            <asp:Timer runat="server" ID="tmrPrint" OnTick="tmrPrint_Tick" Interval="500" Enabled="false"></asp:Timer>
            <asp:HiddenField runat="server" ID="hfCnt" Value="0" />

            <br />
            <br />
            <br />
            <br />

            <h1 runat="server" class="font-weight-bold text-center">
                <asp:Label Style="display: none" runat="server" ID="lblHeader" Width="100%"></asp:Label>
            </h1>

            <br />
            <br />
            <br />
            <br />

        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Button runat="server" ID="btnHidden" Style="display:none" OnClick="btnHidden_Click" />
    <script type="text/javascript">
        function showHeader() {
            var lblHeader = document.getElementById('<%= lblHeader.ClientID %>');
            var ddlCopies = document.getElementById('<%= ddlCopies.ClientID %>');
            var hfCnt = document.getElementById('<%= hfCnt.ClientID %>');

            hfCnt.value = ddlCopies.value;
            lblHeader.style.display = 'block';
            __doPostBack('<%= btnHidden.UniqueID %>', '');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentFull" runat="Server">
</asp:Content>

