<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="DailyLoadTimes.aspx.cs" Inherits="DailyLoadTimes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:UpdatePanel runat="server" ID="UP1">
        <ContentTemplate>
            <div>

                <asp:Timer ID="Timer1" runat="server" Interval="60000"></asp:Timer>
                <h3>
                    <asp:Label ID="lblIntakeTotals" runat="server" Text="Intake"></asp:Label></h3>

                <asp:ObjectDataSource runat="server" ID="objIntakeTotals" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="TallyDatasetTableAdapters.DailyIntakeTotalForAllSitesTableAdapter"></asp:ObjectDataSource>
                <asp:GridView ID="grdIntake" CssClass="table table-hover " HorizontalAlign="Center" runat="server" AutoGenerateColumns="False" DataSourceID="objIntake" AllowSorting="True">
                    <Columns>
                        <asp:BoundField DataField="District" ItemStyle-HorizontalAlign="Left" HeaderText="District" SortExpression="District">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Location" ItemStyle-HorizontalAlign="Left" HeaderText="Location" SortExpression="Location">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Commodity" ItemStyle-HorizontalAlign="Left" HeaderText="Commodity" SortExpression="Commodity">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Loads" ItemStyle-HorizontalAlign="Right" HeaderText="Loads" ReadOnly="True" SortExpression="Loads" DataFormatString="{0:N0}">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Net" ItemStyle-HorizontalAlign="Right" HeaderText="Net" ReadOnly="True" SortExpression="Net" DataFormatString="{0:N0}">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Units" ItemStyle-HorizontalAlign="Right" HeaderText="Units" ReadOnly="True" SortExpression="Units">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UOM" ItemStyle-HorizontalAlign="Left" ReadOnly="True" SortExpression="UOM" DataFormatString="{0:N0}">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="AverageTime" ItemStyle-HorizontalAlign="Left" HeaderText="AverageTime" ReadOnly="True" SortExpression="AverageTime">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                    </Columns>

                </asp:GridView>
                <h3>
                    <asp:Label ID="lbltransferTotals" runat="server" Text="Transfers"></asp:Label></h3>

                <asp:ObjectDataSource runat="server" ID="objTransferTotals" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="TallyDatasetTableAdapters.DailyTransferTotalsForAllSitesTableAdapter"></asp:ObjectDataSource>
                <asp:GridView ID="grdTransfer" CssClass="table table-hover " HorizontalAlign="Center" runat="server" AutoGenerateColumns="False" DataSourceID="objTransfer" DataKeyNames="CropUID" AllowSorting="True">
                    <Columns>
                        <asp:BoundField DataField="District" ItemStyle-HorizontalAlign="Left" HeaderText="District" SortExpression="District">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Location" ItemStyle-HorizontalAlign="Left" HeaderText="Location" SortExpression="Location">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Commodity" ItemStyle-HorizontalAlign="Left" HeaderText="Commodity" SortExpression="Commodity">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Variety" ItemStyle-HorizontalAlign="Left" HeaderText="Variety" SortExpression="Variety">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Loads" ItemStyle-HorizontalAlign="Right" HeaderText="Loads" ReadOnly="True" SortExpression="Loads" DataFormatString="{0:N0}">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Net" ItemStyle-HorizontalAlign="Right" HeaderText="Net" ReadOnly="True" DataFormatString="{0:N0}" SortExpression="Net">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Units" ItemStyle-HorizontalAlign="Right" HeaderText="Units" ReadOnly="True" SortExpression="Units" DataFormatString="{0:N0}">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UOM" ItemStyle-HorizontalAlign="Left" ReadOnly="True" SortExpression="UOM">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Source" ItemStyle-HorizontalAlign="Left" HeaderText="Source" SortExpression="Source">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>


                        <asp:BoundField DataField="AverageTime" ItemStyle-HorizontalAlign="Left" HeaderText="AverageTime" ReadOnly="True" SortExpression="AverageTime">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                    </Columns>

                </asp:GridView>

                <asp:ObjectDataSource runat="server" ID="objTransfer" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="TallyDatasetTableAdapters.CurrentTransferTalleyTableAdapter"></asp:ObjectDataSource>
                <asp:ObjectDataSource runat="server" ID="objIntake" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="TallyDatasetTableAdapters.CurrentIntakeTallyTableAdapter"></asp:ObjectDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

