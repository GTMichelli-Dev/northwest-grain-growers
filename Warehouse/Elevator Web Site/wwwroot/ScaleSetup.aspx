<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="ScaleSetup.aspx.cs" Inherits="ScaleSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            
            <h2 class="page-header ">Scale Setup</h2>
            <p>
                <asp:Button ID="btnNewScale" CssClass="btn btn-lg btn-primary " runat="server" Text="Add Scale" OnClick="btnNewScale_Click" />
                <asp:Label ID="lblNoAdd" CssClass=" h3 label label-warning " runat="server" Text="You Cannot Add another scale until the current scales are completed"></asp:Label>
            </p>
            <asp:GridView ID="GridView1" HorizontalAlign="Center" CssClass="table table-bordered table-responsive table-hover text-left " runat="server" DataSourceID="SqlDataSource1" AutoGenerateColumns="False" DataKeyNames="UID">
                <Columns>
                    
                    <asp:TemplateField HeaderText="Location" SortExpression="Location_Id">

                        <ItemTemplate>
                            <asp:DropDownList ID="ddLocation" runat="server" AutoPostBack="True" CssClass='<%# Eval("LocationAddressCss") %>' DataSourceID="SqlLocation" DataTextField="text" DataValueField="value" OnSelectedIndexChanged="ddLocation_SelectedIndexChanged" SelectedValue='<%# Bind("Location_Id") %>'></asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Description" SortExpression="Description">
                        <ItemTemplate>
                            <asp:TextBox ID="txtDescription" runat="server" Text='<%# Bind("Description") %>' AutoCompleteType="Disabled" AutoPostBack="True" OnTextChanged="txtDescription_TextChanged" CssClass='<%# Eval("cssDescription") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                   
                    <asp:TemplateField HeaderText="Scale Type" SortExpression="Scale_Type">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Scale_Type") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:DropDownList ID="ddScaleType" runat="server" CssClass='<%# Eval("Scale_TypeCss") %>' DataSourceID="sqlSite" DataTextField="text" DataValueField="value" SelectedValue='<%# Bind("Scale_Type") %>' AutoPostBack="True" OnSelectedIndexChanged="ddScaleType_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="sqlSite" runat="server" ConnectionString="<%$ ConnectionStrings:LocalDBConnectionString %>" SelectCommand="SELECT 'Not Set' AS text, '' AS value, 0 AS idx UNION SELECT 'SMA' AS text, 'SMA' AS value, 1 AS idx UNION SELECT 'IDS' AS text, 'IDS' AS value, 1 AS idx ORDER BY idx"></asp:SqlDataSource>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="IP Address" SortExpression="IP_Address">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("IP_Address") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="txtIPAddress" runat="server" Text='<%# Bind("IP_Address") %>' AutoPostBack="True" CssClass='<%# Eval("IpAddressCss") %>' OnTextChanged="txtIPAddress_TextChanged"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Port" SortExpression="Port">
                        <ItemTemplate>
                            <asp:TextBox ID="txtPort" runat="server" Text='<%# Bind("Port") %>' AutoCompleteType="Disabled" CssClass='<%# Eval("PortCss") %>' OnTextChanged="txtPort_TextChanged" Width="75px" AutoPostBack="True"></asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender ID="txtPort_FilteredTextBoxExtender" runat="server" BehaviorID="txtPort_FilteredTextBoxExtender" TargetControlID="txtPort" ValidChars="0123456789" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Inbound Ticket Printer" SortExpression="Inbound_Ticket_Printer">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddInboundPrinter" runat="server" CssClass="dropdown " DataSourceID="SqlPrinters" DataTextField="text" DataValueField="value" OnSelectedIndexChanged="ddInboundPrinter_SelectedIndexChanged" AutoPostBack="True" SelectedValue='<%# Bind("Inbound_Ticket_Printer") %>'>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Print Inbound Ticket" SortExpression="Print_Inbound_Ticket">
                        <EditItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Print_Inbound_Ticket") %>' />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="ckPrintIn" runat="server" Checked='<%# Bind("Print_Inbound_Ticket") %>' AutoPostBack="True" OnCheckedChanged="ckPrintIn_CheckedChanged" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Outbound Ticket Printer" SortExpression="Outbound_Ticket_Printer">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Outbound_Ticket_Printer") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:DropDownList ID="ddOutboundPrinter" runat="server" CssClass="dropdown" DataSourceID="SqlPrinters" DataTextField="text" DataValueField="value" OnSelectedIndexChanged="ddOutboundPrinter_SelectedIndexChanged" SelectedValue='<%# Bind("Outbound_Ticket_Printer") %>' AutoPostBack="True">
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Print Outbound Ticket" SortExpression="Print_Outbound_Ticket">
                        <EditItemTemplate>
                            <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Bind("Print_Outbound_Ticket") %>' />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="ckPrintOut" runat="server" Checked='<%# Bind("Print_Outbound_Ticket") %>' AutoPostBack="True" OnCheckedChanged="ckPrintOut_CheckedChanged" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete" CssClass="btn btn-danger"></asp:LinkButton>
                            <ajaxToolkit:ConfirmButtonExtender ID="btnDelete_ConfirmButtonExtender" runat="server" BehaviorID="btnDelete_ConfirmButtonExtender" ConfirmText='<%# "Delete "+ Eval("Description") %>' TargetControlID="btnDelete" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <div class="text-center ">
                    No Scales Defined For Site
                        </div>
                </EmptyDataTemplate>
            </asp:GridView>

            <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:LocalDBConnectionString %>' SelectCommand="SELECT UID, Scale_Type, Description, IP_Address, Port, Inbound_Ticket_Printer, Outbound_Ticket_Printer, Print_Inbound_Ticket, Print_Outbound_Ticket, CASE WHEN Description = '' THEN 'label-warning' ELSE '' END AS cssDescription, CASE WHEN ip_Address = '' THEN 'label-warning' ELSE '' END AS IpAddressCss, CASE WHEN Location_Id= 0 THEN 'label-warning' ELSE '' END AS LocationAddressCss, CASE WHEN Scale_Type = '' THEN ' label-warning' ELSE '' END AS Scale_TypeCss, CASE WHEN Port = 0 THEN ' label-warning' ELSE '' END AS PortCss, Location_Id, Server_Name FROM Weigh_Scales WHERE (Server_Name = @@SERVERNAME) ORDER BY Location_Id, Description" DeleteCommand="DELETE FROM Weigh_Scales WHERE (UID = @UID)" OnDeleted="SqlDataSource1_Deleted">
                <DeleteParameters>
                    <asp:ControlParameter ControlID="GridView1" Name="ID" PropertyName="SelectedValue" />
                </DeleteParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlPrinters" runat="server" ConnectionString="<%$ ConnectionStrings:LocalDBConnectionString %>" SelectCommand="SELECT 'Not Selected' AS text, '' AS value, 0 AS idx UNION SELECT Inbound_Ticket_Printer AS text, Inbound_Ticket_Printer AS value, 1 AS idx FROM Weigh_Scales WHERE Inbound_Ticket_Printer&lt;&gt;'' AND (Server_Name = @@SERVERNAME) UNION SELECT Outbound_Ticket_Printer AS text, Outbound_Ticket_Printer AS value, 1 AS idx FROM Weigh_Scales AS Weigh_Scales_1 WHERE Outbound_Ticket_Printer&lt;&gt;'' AND (Server_Name = @@SERVERNAME) UNION SELECT Printer_Name AS text, Printer_Name AS value, 1 AS idx FROM Site_Printers WHERE (Server_Name = @@SERVERNAME) ORDER BY idx,text"></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlLocation" runat="server" ConnectionString="<%$ ConnectionStrings:LocalDBConnectionString %>" SelectCommand="SELECT Locations.Description + ' - ' + LTRIM(STR(Locations.Id)) AS text, Locations.Id AS value FROM Locations INNER JOIN Weigh_Scales ON Locations.Id = Weigh_Scales.Location_Id WHERE (Weigh_Scales.Server_Name = @@SERVERNAME) UNION SELECT Locations_1.Description + ' - ' + LTRIM(STR(Locations_1.Id)) AS text, Locations_1.Id AS value FROM Locations AS Locations_1 INNER JOIN Site_Setup ON Locations_1.Id = Site_Setup.Location_Id WHERE (Site_Setup.Server_Name = @@SERVERNAME) ORDER BY text"></asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

