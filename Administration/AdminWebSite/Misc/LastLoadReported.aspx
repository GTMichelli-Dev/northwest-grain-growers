<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="LastLoadReported.aspx.cs" Inherits="Misc_LastLoadReported" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
          <ContentTemplate>

              <h3>
                  <asp:Label ID="lblLastUpdate" runat="server" Text=""></asp:Label>
              </h3>
              <asp:Timer ID="Timer1" runat="server" Interval="60000"></asp:Timer>
              <h3>Last Reported Load By Location</h3>
              <asp:GridView ID="grdLastLoad" CssClass="table table-hover " HorizontalAlign="Center" runat="server" AutoGenerateColumns="False" DataSourceID="objLastLoad" AllowSorting="True">
                  <Columns>
                      <asp:BoundField DataField="Location" ItemStyle-HorizontalAlign="Left" HeaderText="Location" SortExpression="Location"></asp:BoundField>
                      <asp:BoundField DataField="Time_In" ItemStyle-HorizontalAlign="Left" HeaderText="Time" SortExpression="Time_In"></asp:BoundField>
                      <asp:BoundField DataField="Server_Name" ItemStyle-HorizontalAlign="Left" HeaderText="Server Name" SortExpression="Server_Name"></asp:BoundField>
                      <asp:BoundField DataField="LastLoadId" ItemStyle-HorizontalAlign="Right" HeaderText="Load Id" ReadOnly="True" SortExpression="LastLoadId"></asp:BoundField>
                  </Columns>
                  <EmptyDataTemplate>
                      No Loads Reported
                  </EmptyDataTemplate>
              </asp:GridView>

              <asp:ObjectDataSource runat="server" ID="objLastLoad" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="TallyDatasetTableAdapters.LastLoadReportedTableAdapter"></asp:ObjectDataSource>

          </ContentTemplate>
          </asp:UpdatePanel>

    
</asp:Content>

