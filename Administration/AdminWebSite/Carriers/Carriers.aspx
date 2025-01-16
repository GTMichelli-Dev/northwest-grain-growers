<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Carriers.aspx.cs" Inherits="Carriers_Carriers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            Getting Data..
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="text-center ">
                <h3>Carriers</h3>
              
                <div class="container">
                   
                    <div class="form-group col-md-4 text-left ">
                        <label for="txtFilter">Search</label>
                        <asp:TextBox runat="server" ID="txtFilter" AutoPostBack="true" CssClass="form-control " ></asp:TextBox>
                    </div>
                        

                   
                    
                    <asp:GridView ID="GridView1" runat="server" CssClass="table table-hover "  HorizontalAlign="Center" AutoGenerateColumns="False" DataKeyNames="UID" DataSourceID="sqlCarriers" AllowPaging="True" AllowSorting="True" PageSize="100">
                        <Columns>

                            <asp:BoundField DataField="Id" HeaderText="Id" ItemStyle-HorizontalAlign="Left" SortExpression="Id">
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-HorizontalAlign="Left" SortExpression="Description">


                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>

                    


                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="sqlCarriers" runat="server" ConnectionString="<%$ ConnectionStrings:NW_DataConnectionString %>" SelectCommand="SELECT UID, Id, Description, Active FROM Carriers WHERE (Active = 1) AND (Description LIKE '%' + ISNULL(@Filter, Description) + '%') OR (Active = 1) AND (Id LIKE '%' + ISNULL(@Filter, Id) + '%') ORDER BY Description" CancelSelectOnNullParameter="False">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="txtFilter" Name="Filter" PropertyName="Text" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
