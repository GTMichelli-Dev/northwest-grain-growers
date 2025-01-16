<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <br />
            <br />
            <p>
                <h2>
                    <asp:Label ID="lblHeader" runat="server"></asp:Label>

                </h2>
                <asp:Label ID="lblLastUpdate" runat="server" CssClass="h4"></asp:Label>
                <asp:Timer ID="tmrUpdate" runat="server" Interval="1000" OnTick="tmrUpdate_Tick">
                </asp:Timer>
                <p>
                </p>
                <asp:GridView ID="grdScales" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-responsive " DataKeyNames="UID" DataSourceID="SqlScales">
                    <Columns>
                        <asp:BoundField DataField="Location" HeaderText="Location" SortExpression="Location">
                        <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Description" ItemStyle-CssClass="text-left " SortExpression="Description">
                            <ItemTemplate>
                                <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="text-left " />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Weight" ItemStyle-CssClass="text-right ">
                            <ItemTemplate>
                                <asp:Label ID="lblWeight" runat="server"></asp:Label>
                                <asp:HiddenField ID="hfLocation_Id" runat="server" Value='<%# Eval("Location_Id") %>' />
                            </ItemTemplate>
                            <ItemStyle CssClass="text-right " />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status" ItemStyle-CssClass="text-center ">
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="text-center " />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Last Update" ItemStyle-CssClass="text-left ">
                            <ItemTemplate>
                                <asp:Label ID="lblLastUpdate" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="text-left " />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Address" ItemStyle-CssClass="text-left " SortExpression="IP_Address">
                            <ItemTemplate>
                                <asp:Label ID="lblAddress" runat="server" Text='<%# Bind("IP_Address") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="text-left " />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Port" SortExpression="Port">
                            <ItemTemplate>
                                <asp:Label ID="lblPort" runat="server" Text='<%# Bind("Port") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Type" SortExpression="Scale_Type">
                            <ItemTemplate>
                                <asp:Label ID="lblType" runat="server" Text='<%# Bind("Scale_Type") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlScales" runat="server" ConnectionString="<%$ ConnectionStrings:LocalDBConnectionString %>" SelectCommand="SELECT Weigh_Scales.UID, CASE WHEN Location_Id = 0 THEN ' Not Set ' ELSE Weigh_Scales.Description END AS Description, Weigh_Scales.IP_Address, Weigh_Scales.Port, Weigh_Scales.Scale_Type, Weigh_Scales.Server_Name, Locations.Description + N'-' + LTRIM(STR(Weigh_Scales.Location_Id)) AS Location, Weigh_Scales.Location_Id FROM Weigh_Scales INNER JOIN Locations ON Weigh_Scales.Location_Id = Locations.Id WHERE (Weigh_Scales.Server_Name = @@SERVERNAME)"></asp:SqlDataSource>
                <p>
                    &nbsp;</p>
                <p>
                </p>
                <p>
                </p>
                <p>
                </p>
                <p>
                </p>
            </p>
        </ContentTemplate>

    </asp:UpdatePanel>
   



</asp:Content>

