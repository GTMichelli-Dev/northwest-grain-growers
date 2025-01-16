<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AverageProtein.aspx.cs" Inherits="Reports_AverageProtein" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
  <%--  <div class="text-center">
        <asp:LinkButton ID="lnkSelect" CssClass="btn btn-primary" OnClick="lnkSelect_Click" runat="server">Get Report</asp:LinkButton>
    </div>--%>
    <asp:UpdateProgress runat="server" ID="UPr">
        <ProgressTemplate>
            <div class="spinner" id="spinner"></div>
            <h2 class="text-info text-center">Getting Records</h2>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel runat="server" ID="UP1">
        <ContentTemplate>
            <style>
                .spinner {
                    display: block;
                    position: fixed;
                    z-index: 999;
                    top: 20%;
                    left: 50%;
                    width: 50px;
                    height: 50px;
                    margin: -25px 0 0 -25px;
                    border: 8px solid #f3f3f3;
                    border-radius: 50%;
                    border-top: 8px solid #3498db;
                    animation: spin 2s linear infinite;
                }

                @keyframes spin {
                    0% {
                        transform: rotate(0deg);
                    }

                    100% {
                        transform: rotate(360deg);
                    }
                }
            </style>

            <h3>
                <asp:Label ID="lblHeader" runat="server" Text="Average Protein By Bin"></asp:Label>
            </h3>
            <div class="row text-center">
                <div class="col-md-10 offset-1">
                    <table style="width: 100%" class="text-center">
                        <tr>
                            <td colspan="5" class="text-center"></td>
                        </tr>
                        <tr>
                            <td>Location</td>
                            <td>From</td>
                            <td>To</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList runat="server" ID="cboLocation" CssClass="form-control" DataSourceID="sqlLocation" DataTextField="Description" DataValueField="value" AutoPostBack="True" OnTextChanged="cboLocation_TextChanged" OnSelectedIndexChanged="cboLocation_SelectedIndexChanged"></asp:DropDownList>
                                <asp:SqlDataSource runat="server" ID="sqlLocation" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT NULL AS value, 'All Locations' AS Description, 0 AS idx UNION SELECT Id AS Value, Description + N' - ' + LTRIM(STR(Id)) AS Description, 1 AS idx FROM Locations ORDER BY idx, Description"></asp:SqlDataSource>
                            </td>
                            <td>
                                <asp:TextBox runat="server" CssClass="form-control" ID="txtStartDate" AutoPostBack="True" OnTextChanged="txtStartDate_TextChanged"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender runat="server" BehaviorID="txtStartDate_CalendarExtender" TargetControlID="txtStartDate" ID="txtStartDate_CalendarExtender"></ajaxToolkit:CalendarExtender>
                            </td>
                            <td>
                                <asp:TextBox runat="server" CssClass="form-control" ID="txtEndDate" AutoPostBack="True" OnTextChanged="txtEndDate_TextChanged"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender runat="server" BehaviorID="txtEndDate_CalendarExtender" TargetControlID="txtEndDate" ID="txtEndDate_CalendarExtender"></ajaxToolkit:CalendarExtender>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <hr />
            <br />
            <br />
            <div class="row ">
                <div class="col-md-10 offset-1">
                    <asp:GridView ID="GridView1" CssClass="table table-hover table-responsive" HorizontalAlign="Center" runat="server" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="false">
                        <Columns>
                            <asp:BoundField  ItemStyle-HorizontalAlign="Left"  DataField="Location" HeaderText="Location" SortExpression="Location">
                                  <ItemStyle  Width="250px" /> 
                            </asp:BoundField>
                            <asp:BoundField  ItemStyle-HorizontalAlign="Left"  DataField="Crop" HeaderText="Crop" SortExpression="Crop">
                                
                            </asp:BoundField>
                            <asp:BoundField  ItemStyle-HorizontalAlign="Left" DataField="Bin" HeaderText="Bin" SortExpression="Bin">
                                <ItemStyle Width="150px" /> 
                            </asp:BoundField>
                            <asp:BoundField ItemStyle-HorizontalAlign="Right" DataField="Net" HeaderText="Net Pounds" SortExpression="Net" DataFormatString="{0:N0}">
                                
                            </asp:BoundField>
                            <asp:BoundField  ItemStyle-HorizontalAlign="Right"  DataField="StrUnits" HeaderText="Net Units" SortExpression="StrUnits" >
                                 <ItemStyle Width="150px" /> 
                            </asp:BoundField>
                            <asp:BoundField  ItemStyle-HorizontalAlign="Right"  DataField="Protein" HeaderText="Avg Protein" SortExpression="Protein" DataFormatString="{0:N2}">
                               
                            </asp:BoundField>
                        </Columns>
                        <EmptyDataTemplate>
                            <h4 style="width:100%" class="col-12 text-center">No Data Matching Filter</h4>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
            <asp:HiddenField ID="hfStart" runat="server" />
            <asp:HiddenField ID="hfEnd" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
