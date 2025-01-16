<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Reprint.aspx.cs" Inherits="Reprint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
  
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" ClientIDMode="Inherit" DisplayAfter="200" Visible="True">
        <ProgressTemplate>
            <h1 class="label-warning ">
                <asp:Label ID="lblHeader" runat="server" Text="Loading..."></asp:Label>
            </h1>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Timer ID="tmrPrintRecieving" runat="server" Enabled="False" Interval="500" OnTick="tmrPrintRecieving_Tick">
            </asp:Timer>
            <asp:Timer ID="tmrPrintTransfer" runat="server" Enabled="False" Interval="500" OnTick="Timer1_Tick">
            </asp:Timer>
            <asp:HiddenField ID="hfLocationID" runat="server" />
            <asp:HiddenField ID="hfOneShot" runat="server" Value="0" />
            <asp:HiddenField ID="hfLabel" runat="server" Value="Loading" />
            <asp:HiddenField ID="hfUID" runat="server" />
            <div class="container">
                <div class="row text-center ">
                    <div class="col-md-4"></div>
                    <div class="col-md-4">
                        <label for="ddPrinter"></label>
                        <asp:DropDownList ID="ddPrinter" CssClass="form-control " runat="server" DataSourceID="SqlPrinters" DataTextField="Printer_Name" DataValueField="Printer_Name"></asp:DropDownList>
                        <asp:SqlDataSource ID="SqlPrinters" runat="server" ConnectionString="<%$ ConnectionStrings:LocalDBConnectionString %>" SelectCommand=" SELECT Printer_Name, 1 AS idx FROM Site_Printers WHERE (Server_Name = @@SERVERNAME) AND (NOT (Printer_Name LIKE N'%DYMO%')) ORDER BY idx, Printer_Name" OnSelecting="SqlPrinters_Selecting"></asp:SqlDataSource>
                    </div>
                    <div class="col-md-4"></div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <h3>Recieving</h3>
                        <asp:GridView ID="grdRecieving" CssClass="table table-condensed table-hover " runat="server" AutoGenerateColumns="False" DataKeyNames="Load_UID" DataSourceID="sqlRecieving">
                            <Columns>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkRecievingPrint" runat="server" CausesValidation="False" CommandName="Select" OnClick="lnkRecievingPrint_Click" Text="Reprint"></asp:LinkButton>
                                        <asp:HiddenField ID="HFRLocationID" runat="server" Value='<%# Eval("Location_Id") %>' />
                                        <asp:HiddenField ID="lfRLoadID" runat="server" Value='<%# Eval("Load_Id") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="Load_Id" HeaderText="Load Id" SortExpression="Load_Id"></asp:BoundField>
                                <asp:BoundField DataField="Weight_Sheet_Id" HeaderText="Weight  Sheet" ReadOnly="True" SortExpression="Weight_Sheet_Id"></asp:BoundField>
                                <asp:BoundField DataField="Time_Out" HeaderText="Time Out" SortExpression="Time_Out"></asp:BoundField>
                                <asp:BoundField DataField="Truck_Id" HeaderText="Truck Id" SortExpression="Truck_Id"></asp:BoundField>
                                <asp:BoundField DataField="Location_Id" HeaderText="Location" SortExpression="Location_Id"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource runat="server" ID="sqlRecieving" ConnectionString='<%$ ConnectionStrings:LocalDBConnectionString %>' SelectCommand="SELECT TOP (200) vw_Loads_By_Type.Load_UID, vw_Loads_By_Type.Weight_Sheet_UID, vw_Loads_By_Type.Load_Id, vw_Loads_By_Type.Weight_Sheet_Id, vw_Loads_By_Type.Load_Type, vw_Loads_By_Type.Time_Out, vw_Loads_By_Type.Truck_Id, vw_Loads_By_Type.Location_Id FROM vw_Loads_By_Type INNER JOIN (SELECT TOP (100) PERCENT Location_Id FROM Site_Setup WHERE (Server_Name = @@SERVERNAME) ORDER BY Server_Name, Location_Id) AS derivedtbl_1 ON vw_Loads_By_Type.Location_Id = derivedtbl_1.Location_Id WHERE (vw_Loads_By_Type.Load_Type = 'I') ORDER BY vw_Loads_By_Type.Location_Id DESC, vw_Loads_By_Type.Time_Out DESC"></asp:SqlDataSource>
                    </div>
                    <div class="col-md-6">
                        <h3>Transfers</h3>
                        <asp:GridView ID="grdtransfer" CssClass="table table-condensed table-hover " runat="server" AutoGenerateColumns="False" DataKeyNames="Load_UID" DataSourceID="sqlTransfer">
                            <Columns>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkTransfersPrint" runat="server" CausesValidation="False" CommandName="Select" OnClick="lnkTransfersPrint_Click" Text="Reprint"></asp:LinkButton>
                                        <asp:HiddenField ID="HFTLocationID" runat="server" Value='<%# Eval("Location_Id") %>' />
                                        <asp:HiddenField ID="lfTLoadID" runat="server" Value='<%# Eval("Load_Id") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="Load_Id" HeaderText="Load Id" SortExpression="Load_Id"></asp:BoundField>
                                <asp:BoundField DataField="Weight_Sheet_Id" HeaderText="Weight  Sheet" ReadOnly="True" SortExpression="Weight_Sheet_Id"></asp:BoundField>
                                <asp:BoundField DataField="Time_Out" HeaderText="Time Out" SortExpression="Time_Out"></asp:BoundField>
                                <asp:BoundField DataField="Truck_Id" HeaderText="Truck Id" SortExpression="Truck_Id"></asp:BoundField>
                                <asp:BoundField DataField="Location_Id" HeaderText="Location" SortExpression="Location_Id"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource runat="server" ID="sqlTransfer" ConnectionString='<%$ ConnectionStrings:LocalDBConnectionString %>' SelectCommand="SELECT TOP (200) vw_Loads_By_Type.Load_UID, vw_Loads_By_Type.Weight_Sheet_UID, vw_Loads_By_Type.Load_Id, vw_Loads_By_Type.Weight_Sheet_Id, vw_Loads_By_Type.Load_Type, vw_Loads_By_Type.Time_Out, vw_Loads_By_Type.Truck_Id, vw_Loads_By_Type.Location_Id FROM vw_Loads_By_Type INNER JOIN (SELECT TOP (100) PERCENT Location_Id FROM Site_Setup WHERE (Server_Name = @@SERVERNAME) ORDER BY Server_Name, Location_Id) AS derivedtbl_1 ON vw_Loads_By_Type.Location_Id = derivedtbl_1.Location_Id WHERE (vw_Loads_By_Type.Load_Type = 'T') ORDER BY vw_Loads_By_Type.Location_Id DESC, vw_Loads_By_Type.Time_Out DESC"></asp:SqlDataSource>

                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

