<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Unlock.aspx.cs" Inherits="Unlock" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="text-center ">

<%--                <h2>Login</h2>
                <hr />--%>
                <div class="container text-left ">
                    <h2>Login</h2>

                    <div class="form-group">
                        <asp:DropDownList ID="ddUserLevel" CssClass=" form-control "  Width="280px" runat="server" >
                            <asp:ListItem>Supervisor</asp:ListItem>
                            <asp:ListItem>Admin</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">

                        <label for="txtPass">Password</label>
                        <asp:TextBox ID="txtPass" CssClass="form-control" AutoPostBack="true" OnTextChanged="lnkOk_Click"  type="password" Width="280px"  runat="server"></asp:TextBox>
                    </div>
                    <h2 class="text-danger ">
                        <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
                    </h2>
                    <div class="form-group">
                        <asp:LinkButton ID="lnkOk" CssClass="btn btn-outline-dark " OnClick="lnkOk_Click" runat="server">Login</asp:LinkButton>
                    </div>

                </div>

                
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

