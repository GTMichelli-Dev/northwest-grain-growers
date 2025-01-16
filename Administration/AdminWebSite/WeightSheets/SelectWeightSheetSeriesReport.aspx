<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="SelectWeightSheetSeriesReport.aspx.cs" Inherits="WeightSheets_SelectWeightSheetSeriesReport" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:UpdateProgress runat="server" ID="UPr">
        <ProgressTemplate >
            <%--<h2 class="text-info text-center  ">Getting Data</h2>--%>
         </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel runat="server" ID="UP1">
        <ContentTemplate>



            <h3><asp:Label ID="lblHeader" runat="server" Text="Daily Weight Sheet Series Report"></asp:Label></h3>
            
             <table style="width:100%  " class="text-center " >
  
                <tr>
                    <td colspan="5" class="text-center ">
                    </td>
                      
                </tr>
                <tr>
                
               
                    <td>Location
                    </td>
                    <td>Date
                    </td>
                 
                 

                </tr>
                <tr>
 
                    <td>
                        
                        <asp:DropDownList runat="server" ID="cboLocation" CssClass="input-sm " DataSourceID="sqlLocation" DataTextField="Description" DataValueField="value" AutoPostBack="True" OnTextChanged="cboLocation_TextChanged" OnSelectedIndexChanged="cboLocation_SelectedIndexChanged"></asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="sqlLocation" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT NULL AS value, 'Select Location' AS Description, 0 AS idx UNION SELECT Id AS Value, Description + N' - ' + LTRIM(STR(Id)) AS Description, 1 AS idx FROM Locations ORDER BY idx, Description"></asp:SqlDataSource>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="input-sm " ID="txtStartDate" AutoPostBack="True" OnTextChanged="txtStartDate_TextChanged"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender runat="server" BehaviorID="txtStartDate_CalendarExtender" TargetControlID="txtStartDate" ID="txtStartDate_CalendarExtender"></ajaxToolkit:CalendarExtender>
                    </td>
                 
                </tr>
            </table>


            <hr />

            <br />
            <div class="text-center ">
                <asp:LinkButton ID="lnkSelect" CssClass="btn btn-primary " OnClick="lnkSelect_Click" runat="server">Get Report</asp:LinkButton>
            </div>
            <asp:GridView ID="GridView1" CssClass="table table-hover table-responsive  " HorizontalAlign="Center" runat="server" DataSourceID="objSQL" Width="60%" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True" PageSize="50">

                <Columns>
                    <asp:BoundField DataField="WS_Id" HeaderText="Weight Sheet" ItemStyle-HorizontalAlign="Left" SortExpression="WS_Id" ReadOnly="True"></asp:BoundField>
                    <asp:BoundField DataField="LoadType" HeaderText="Type" ReadOnly="True" SortExpression="LoadType"></asp:BoundField>
                    <asp:BoundField DataField="Crop" HeaderText="Crop" ReadOnly="True" SortExpression="Crop"></asp:BoundField>
                    <asp:BoundField DataField="Variety" HeaderText="Variety" SortExpression="Variety"></asp:BoundField>
                    <asp:BoundField DataField="Loads" HeaderText="Loads" ItemStyle-HorizontalAlign="Right" ReadOnly="True" SortExpression="Loads"></asp:BoundField>
                    <asp:BoundField DataField="Net" HeaderText="Net" ItemStyle-HorizontalAlign="Right" ReadOnly="True" SortExpression="Net"></asp:BoundField>
                </Columns>
                <EmptyDataTemplate>
                    <h2>No Data Matching Filter</h2>
                </EmptyDataTemplate>

            </asp:GridView>


            <asp:ObjectDataSource ID="objSQL" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="TallyDatasetTableAdapters.WeightSeriesByLocationDateTableAdapter">
                <SelectParameters>
                    <asp:ControlParameter ControlID="cboLocation" Name="Location_Id" PropertyName="SelectedValue" Type="Int32" />
                    <asp:ControlParameter ControlID="hfStart" Name="SelectedDate" PropertyName="Value" Type="DateTime" />
                </SelectParameters>
            </asp:ObjectDataSource>

       

            <asp:HiddenField ID="hfStart" runat="server" />
            <asp:HiddenField ID="hfEnd" runat="server" />

        </ContentTemplate>

    </asp:UpdatePanel>


</asp:Content>
