<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Site_Log.aspx.cs" Inherits="Site_Log" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h2 class="page-header ">Site Log</h2>
            <p>
                &nbsp;</p>
            <asp:GridView ID="grdLog" runat="server" CssClass="table table-bordered  table-striped  table-responsive text-left " HorizontalAlign="Center" AutoGenerateColumns="False" DataSourceID="SqlLocalLog" AllowSorting="True">
                <Columns>
                    <asp:BoundField DataField="Location_Id" HeaderText="Site Id"  SortExpression="Location_Id">
                    </asp:BoundField>
                    <asp:BoundField DataField="Time_Date" HeaderText="Time"  SortExpression="Time_Date">
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Message" SortExpression="Message">
                      
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Message") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Code_Location" HeaderText="Code" SortExpression="Code_Location">
                    </asp:BoundField>
                </Columns>
                <EmptyDataTemplate >
                    <div class="text-center ">
                    Nothing Logged
                        </div>
                </EmptyDataTemplate>
            </asp:GridView>
            <asp:SqlDataSource runat="server" ID="SqlLocalLog" ConnectionString='<%$ ConnectionStrings:LocalDBConnectionString %>' SelectCommand="SELECT top(500) System_Log.Location_Id, System_Log.Time_Date, System_Log.Location AS Code_Location, System_Log.Message FROM System_Log INNER JOIN (SELECT TOP (100) PERCENT Location_Id FROM Site_Setup WHERE (Server_Name = @@SERVERNAME) ORDER BY Server_Name, Location_Id) AS derivedtbl_1 ON System_Log.Location_Id = derivedtbl_1.Location_Id ORDER BY System_Log.Location_Id, System_Log.Time_Date" DeleteCommand="DELETE FROM [System_Log] WHERE [UID] = @UID" InsertCommand="INSERT INTO [System_Log] ([UID], [Server_Name], [Time_Date], [Location], [Message], [Location_Id], [rowguid]) VALUES (@UID, @Server_Name, @Time_Date, @Location, @Message, @Location_Id, @rowguid)" UpdateCommand="UPDATE [System_Log] SET [Server_Name] = @Server_Name, [Time_Date] = @Time_Date, [Location] = @Location, [Message] = @Message, [Location_Id] = @Location_Id, [rowguid] = @rowguid WHERE [UID] = @UID">
                
             
              
            </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

