<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Site_Setup.aspx.cs" Inherits="Site_Setup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h4>Site Setup</h4>
            <p class=" text-center ">
                <asp:LinkButton ID="lnkNewButton" runat="server" PostBackUrl="~/CreateNewSite.aspx">Add Site</asp:LinkButton>
            </p>
            <p>
                &nbsp;</p>
            <p>
                <asp:GridView ID="GridView2" runat="server" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="UID" DataSourceID="SqlSiteSetup" CssClass="table table-boardered table-hover">
                    <Columns>
                        <asp:TemplateField ShowHeader="False">
                            <EditItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True"  CommandName="Update" Text="Update"></asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False"  CommandName="Edit" Text="Edit"></asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False"  CommandName="Delete" Text="Delete"></asp:LinkButton>
                                <ajaxToolkit:ConfirmButtonExtender ID="LinkButton2_ConfirmButtonExtender" runat="server" BehaviorID="LinkButton2_ConfirmButtonExtender" ConfirmText='<%#"Delete Location "+ Eval("Location_Id") %>' TargetControlID="LinkButton2" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Location_Id" HeaderText="Location"  SortExpression="Location_Id" ReadOnly="True" />
                        <asp:BoundField DataField="Server_Name" HeaderText="Server" SortExpression="Server_Name" ReadOnly="True" />
                        <asp:BoundField DataField="Sequence_ID" HeaderText="Seq ID" SortExpression="Sequence_ID" ReadOnly="True" />
                        <asp:BoundField DataField="Lot_Id_Seed" HeaderText="Lot Id Seed" SortExpression="Lot_Id_Seed" />
                        <asp:BoundField DataField="WS_Id_Seed" HeaderText="WS Id Seed" SortExpression="WS_Id_Seed" />
                        <asp:BoundField DataField="Load_Seed" HeaderText="Load Id Seed" SortExpression="Load_Seed" />
                        <asp:BoundField DataField="Current_Fuel_Price" HeaderText="Current Fuel Price" SortExpression="Current_Fuel_Price" />
                        <asp:BoundField DataField="Outbound_Seed" HeaderText="Outbound ID Seed" SortExpression="Outbound_Seed" />
                        <asp:BoundField DataField="Outbound_Final_Kiosk_Ticket_Count" HeaderText="Out Kiosk Ticket_Count" SortExpression="Outbound_Final_Kiosk_Ticket_Count" />
                        <asp:CheckBoxField DataField="Has_Loadout" HeaderText="Has Loadout" SortExpression="Has_Loadout" />
                        <asp:BoundField DataField="Load_Out_Ip" HeaderText="Load Out Ip" SortExpression="Load_Out_Ip" />
                        <asp:BoundField DataField="Load_Out_Port" HeaderText="Load Out Port" SortExpression="Load_Out_Port" />
                        <asp:BoundField DataField="Seed_Ticket_Seed" HeaderText="Seed Ticket Seed" SortExpression="Seed_Ticket_Seed" />
                        <asp:CheckBoxField DataField="Is_Seed_Mill" HeaderText="Is Seed Mill" SortExpression="Is_Seed_Mill" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlSiteSetup" runat="server" ConnectionString="<%$ ConnectionStrings:LocalDBConnectionString %>" DeleteCommand="DELETE FROM [Site_Setup] WHERE [UID] = @UID" InsertCommand="INSERT INTO [Site_Setup] ([UID], [Location_Id], [Server_Name], [Sequence_ID], [Lot_Id_Seed], [WS_Id_Seed], [Load_Seed], [Max_WS_Ticket_Count], [Current_Fuel_Price], [Outbound_Seed], [Outbound_Final_Kiosk_Ticket_Count], [Has_Loadout], [Load_Out_Ip], [Load_Out_Port], [Seed_Ticket_Seed], [Is_Seed_Mill]) VALUES (@UID, @Location_Id, @Server_Name, @Sequence_ID, @Lot_Id_Seed, @WS_Id_Seed, @Load_Seed, @Max_WS_Ticket_Count, @Current_Fuel_Price, @Outbound_Seed, @Outbound_Final_Kiosk_Ticket_Count, @Has_Loadout, @Load_Out_Ip, @Load_Out_Port, @Seed_Ticket_Seed, @Is_Seed_Mill)" SelectCommand="SELECT UID, Location_Id, Server_Name, Sequence_ID, Lot_Id_Seed, WS_Id_Seed, Load_Seed, Max_WS_Ticket_Count, Current_Fuel_Price, Outbound_Seed, Outbound_Final_Kiosk_Ticket_Count, Has_Loadout, Load_Out_Ip, Load_Out_Port, Seed_Ticket_Seed, Is_Seed_Mill FROM Site_Setup WHERE (Server_Name = @@SERVERNAME) ORDER BY Server_Name, Location_Id" UpdateCommand="UPDATE Site_Setup SET Is_Seed_Mill = @Is_Seed_Mill, Lot_Id_Seed = @Lot_Id_Seed, WS_Id_Seed = @WS_Id_Seed, Load_Seed = @Load_Seed, Current_Fuel_Price = @Current_Fuel_Price, Outbound_Seed = @Outbound_Seed, Outbound_Final_Kiosk_Ticket_Count = @Outbound_Final_Kiosk_Ticket_Count, Has_Loadout = @Has_Loadout, Load_Out_Ip = @Load_Out_Ip, Load_Out_Port = @Load_Out_Port, Seed_Ticket_Seed = @Seed_Ticket_Seed WHERE (UID = @UID)" OnDeleted="SqlSiteSetup_Deleted">
                    <DeleteParameters>
                        <asp:Parameter Name="UID" Type="Object" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:Parameter Name="UID" Type="Object" />
                        <asp:Parameter Name="Location_Id" Type="Int32" />
                        <asp:Parameter Name="Server_Name" Type="String" />
                        <asp:Parameter Name="Sequence_ID" Type="Int32" />
                        <asp:Parameter Name="Lot_Id_Seed" Type="Int32" />
                        <asp:Parameter Name="WS_Id_Seed" Type="Int32" />
                        <asp:Parameter Name="Load_Seed" Type="Int32" />
                        <asp:Parameter Name="Max_WS_Ticket_Count" Type="Int32" />
                        <asp:Parameter Name="Current_Fuel_Price" Type="Decimal" />
                        <asp:Parameter Name="Outbound_Seed" Type="Int32" />
                        <asp:Parameter Name="Outbound_Final_Kiosk_Ticket_Count" Type="Int32" />
                        <asp:Parameter Name="Has_Loadout" Type="Boolean" />
                        <asp:Parameter Name="Load_Out_Ip" Type="String" />
                        <asp:Parameter Name="Load_Out_Port" Type="Int32" />
                        <asp:Parameter Name="Seed_Ticket_Seed" Type="Int32" />
                        <asp:Parameter Name="Is_Seed_Mill" Type="Boolean" />
                    </InsertParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="UID" Type="Object" />
                        <asp:Parameter Name="Is_Seed_Mill" Type="Boolean" />
                        <asp:Parameter Name="Lot_Id_Seed" />
                        <asp:Parameter Name="WS_Id_Seed" />
                        <asp:Parameter Name="Load_Seed" />
                        <asp:Parameter Name="Current_Fuel_Price" />
                        <asp:Parameter Name="Outbound_Seed" />
                        <asp:Parameter Name="Outbound_Final_Kiosk_Ticket_Count" />
                        <asp:Parameter Name="Has_Loadout" />
                        <asp:Parameter Name="Load_Out_Ip" />
                        <asp:Parameter Name="Load_Out_Port" />
                        <asp:Parameter Name="Seed_Ticket_Seed" />
                    </UpdateParameters>
                </asp:SqlDataSource>
            </p>
            <p>
                &nbsp;</p>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

