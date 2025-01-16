<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Results.aspx.cs" Inherits="Results" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <h2>Agvantage Update Log</h2>

    <asp:UpdatePanel runat="server" ID="Up1">


        <ContentTemplate>
            
            <div class=" form-row">
                <div class="col-12 ">
                    <asp:GridView runat="server" ID="Grd1" HorizontalAlign="Center" Width="100%" CssClass="table table-hover table-sm table-bordered  " AutoGenerateColumns="False" DataKeyNames="UID" DataSourceID="SqlDataSource1" AllowSorting="True">

                        <Columns>
                            <asp:CheckBoxField DataField="Error" HeaderText="Error" SortExpression="Error"></asp:CheckBoxField>
                            <asp:BoundField DataField="TaskTime" HeaderText="Time" SortExpression="TaskTime"></asp:BoundField>
                            <asp:BoundField DataField="Message" ItemStyle-CssClass="text-left " ControlStyle-CssClass="text-left " HeaderText="Message" SortExpression="Message"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT * FROM [AgvantageTransferLog] ORDER BY [TaskTime] DESC"></asp:SqlDataSource>
                </div>
            </div>


        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

