<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Setup.aspx.cs" Inherits="Setup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:UpdatePanel runat="server" ID="UP1">
        <ContentTemplate>
            <div class=" container ">
                <h5 >Site Setup</h5>

                <div class=" form-row">
                    <div class="form-group">
                        <label for="ddLocation">
                            Location
                        </label>
                        <asp:DropDownList runat="server" ID="ddLocation" OnTextChanged="ddLocation_TextChanged" AutoPostBack="true"  CssClass="form-control  Width280 " DataSourceID="SqlLocations" DataTextField="Text" DataValueField="Value"></asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="SqlLocations" ConnectionString='<%$ ConnectionStrings:Seed_DataConnectionString %>' SelectCommand="SELECT Description + N' - ' + LTRIM(STR(ID)) AS Text, ID AS value FROM Locations ORDER BY value"></asp:SqlDataSource>
                    </div>
                    </div>
                   <div class=" form-row">
                    <asp:Panel runat="server" ID="pnlPrinter">
                    <div class="form-group">
                        <label for="ddPrinter">
                            Report Printer
                        </label>
                        <asp:DropDownList runat="server" ID="ddPrinter" OnTextChanged="ddPrinter_TextChanged"  AutoPostBack="true"  CssClass="form-control" DataTextField="Text" DataValueField="Value"></asp:DropDownList>
                    </div>
                    </asp:Panel>
                </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentFull" Runat="Server">
</asp:Content>

