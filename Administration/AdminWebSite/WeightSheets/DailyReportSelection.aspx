<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="DailyReportSelection.aspx.cs" Inherits="DailyReportSelection" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:UpdateProgress runat="server" ID="UPr">
        <ProgressTemplate >
            <%--<h2 class="text-info text-center  ">Getting Data</h2>--%>
         </ProgressTemplate>
    </asp:UpdateProgress>
                <h3><asp:Label ID="lblHeader" runat="server" Text="Daily Intake/Transfer Report"></asp:Label>  </h3>
    <asp:UpdatePanel runat="server" ID="UP1">
        <ContentTemplate>




            
             <table style="width:100%  " class="text-center " >
  
                <tr>
                    <td colspan="5" class="text-center ">
                    </td>
                      
                </tr>
                <tr>
                
                    <td>Type
                    </td>
                    <td>Location
                    </td>
                    <td>From
                    </td>
                    <td>To
                    </td>

                </tr>
                <tr>
 
                    <td>
                        
                        <asp:DropDownList runat="server" ID="ddType" CssClass="input-sm " DataTextField="Text" AutoPostBack="True" DataSourceID="sqlType" DataValueField="value" OnTextChanged="ddType_TextChanged"></asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="sqlType" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT 'Intake/Transfer' AS text, NULL AS value, 0 AS idx UNION SELECT 'Intake' AS text, 'Intake' AS value, 1 AS idx UNION SELECT 'Transfer' AS text, 'Transfer' AS value, 2 AS idx ORDER BY idx"></asp:SqlDataSource>
                    </td>
                    <td>
                        
                        <asp:DropDownList runat="server" ID="cboLocation" CssClass="input-sm " DataSourceID="sqlLocation" DataTextField="Text" DataValueField="Id" AutoPostBack="True" OnTextChanged="cboLocation_TextChanged"></asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="sqlLocation" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT 'All Locations' AS Text, null AS Id, 0 AS idx UNION SELECT Description + ' - ' + LTRIM(STR(Id)) AS Text, Id, 1 AS idx FROM Locations ORDER BY idx, Text"></asp:SqlDataSource>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="input-sm " ID="txtStartDate" AutoPostBack="True" OnTextChanged="txtStartDate_TextChanged"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender runat="server" BehaviorID="txtStartDate_CalendarExtender" TargetControlID="txtStartDate" ID="txtStartDate_CalendarExtender"></ajaxToolkit:CalendarExtender>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="input-sm " ID="txtEndDate" AutoPostBack="True" OnTextChanged="txtEndDate_TextChanged"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender runat="server" BehaviorID="txtEndDate_CalendarExtender" TargetControlID="txtEndDate" ID="txtEndDate_CalendarExtender"></ajaxToolkit:CalendarExtender>
                    </td>
                </tr>
            </table>


            <hr />

            <br />



            <asp:GridView ID="GridView1" CssClass="table table-bordered table-hover table-sm "  HorizontalAlign="Center" runat="server" DataSourceID="sqlWeightSheets" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True" PageSize="50" Width="50%">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDetails" OnClick="lnkDetails_Click" CssClass="btn btn-outline-dark  " runat="server" CommandArgument='<%# Eval("Location_Id") %>'>Details</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="Location" SortExpression="Location" HeaderText="Location"   ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ReadOnly="True"  >
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>

                    <asp:BoundField DataField="CreationDate"  ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"  ItemStyle-CssClass=" text-right " HeaderText="Creation Date" ReadOnly="True" SortExpression="CreationDate">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle CssClass=" text-right " HorizontalAlign="Left" />
                    </asp:BoundField>

                    <asp:BoundField DataField="WSType" HeaderText="Type" SortExpression="WSType"   ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ReadOnly="True"  >
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>

                </Columns>
                <EmptyDataTemplate>
       
                         <h5 class="text-center">No Report Matching Filter</h5>

                </EmptyDataTemplate>
            </asp:GridView>
            <asp:SqlDataSource runat="server" ID="sqlWeightSheets" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT CONVERT (VARCHAR(10), Weight_Sheets.Creation_Date, 101) AS CreationDate, Weight_Sheets.Location_Id, Locations.Description AS Location_Name, CASE WHEN Weight_Inbound_Loads.uid IS NULL THEN 'Transfer' ELSE 'Intake' END AS WSType, Locations.Description + N' - ' + LTRIM(STR(Locations.Id)) AS Location FROM Weight_Sheets LEFT OUTER JOIN Locations ON Weight_Sheets.Location_Id = Locations.Id LEFT OUTER JOIN Lots INNER JOIN Weight_Inbound_Loads ON Lots.UID = Weight_Inbound_Loads.Lot_UID ON Weight_Sheets.UID = Weight_Inbound_Loads.Weight_Sheet_UID WHERE (CASE WHEN Weight_Inbound_Loads.uid IS NULL THEN 'Transfer' ELSE 'Intake' END = ISNULL(@Type, CASE WHEN Weight_Inbound_Loads.uid IS NULL THEN 'Transfer' ELSE 'Intake' END)) AND (DATEDIFF(day, @StartDate, Weight_Sheets.Creation_Date) >= 0) AND (DATEDIFF(day, Weight_Sheets.Creation_Date, @EndDate) >= 0) GROUP BY CONVERT (VARCHAR(10), Weight_Sheets.Creation_Date, 101), Weight_Sheets.Location_Id, Locations.Description, Locations.Description + N' - ' + LTRIM(STR(Locations.Id)), CASE WHEN Weight_Inbound_Loads.uid IS NULL THEN 'Transfer' ELSE 'Intake' END HAVING (Weight_Sheets.Location_Id = ISNULL(@Location_Id, Weight_Sheets.Location_Id)) ORDER BY Location_Name, CreationDate DESC" CancelSelectOnNullParameter="False">
                <SelectParameters>
                    <asp:ControlParameter ControlID="ddType" PropertyName="SelectedValue" Name="Type"></asp:ControlParameter>

                    <asp:ControlParameter ControlID="hfStart" Name="StartDate" PropertyName="Value" />

                    <asp:ControlParameter ControlID="hfEnd" Name="EndDate" PropertyName="Value" />
                    <asp:ControlParameter ControlID="cboLocation" Name="Location_Id" PropertyName="SelectedValue" />
                </SelectParameters>
            </asp:SqlDataSource>

            <asp:HiddenField ID="hfStart" runat="server" />
            <asp:HiddenField ID="hfEnd" runat="server" />

        </ContentTemplate>

    </asp:UpdatePanel>


</asp:Content>

