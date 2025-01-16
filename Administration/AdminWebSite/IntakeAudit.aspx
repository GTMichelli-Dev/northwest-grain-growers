<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="IntakeAudit.aspx.cs" Inherits="IntakeAudit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
                        <asp:HiddenField ID="hfSelectedDate" runat="server" />
                     <asp:HiddenField ID="hfSelectedEndDate" runat="server" />


    <div class="form-row">
        <h3>Audit Trail</h3>

    </div>
    <div class="form-row">
        <div class=" offset-3  col-md-2 text-center ">

            <asp:TextBox runat="server" ID="txtSelectedDate" CssClass="  text-center form-control  " Font-Size="Large" AutoPostBack="true" OnTextChanged="txtSelectedDate_TextChanged"></asp:TextBox>
            <ajaxToolkit:CalendarExtender runat="server" BehaviorID="txtSelectedDate_CalendarExtender" TargetControlID="txtSelectedDate" ID="txtSelectedDate_CalendarExtender"></ajaxToolkit:CalendarExtender>
        </div>
        <div class="col-md-2 text-center ">

            <asp:TextBox runat="server" ID="txtSelectedEndDate" CssClass="  text-center form-control  " Font-Size="Large" AutoPostBack="true" OnTextChanged="txtSelectedEndDate_TextChanged"></asp:TextBox>
            <ajaxToolkit:CalendarExtender runat="server" BehaviorID="txtSelectedEndDate_CalendarExtender" TargetControlID="txtSelectedEndDate" ID="txtSelectedEndDate_CalendarExtender"></ajaxToolkit:CalendarExtender>
        </div>
        <div class="col-md-2 text-center ">

            <asp:DropDownList runat="server" ID="ddLocations" CssClass="  text-center form-control  " Font-Size="Large" AutoPostBack="true" DataSourceID="sqlLocation" DataTextField="text" DataValueField="value"></asp:DropDownList>

            <asp:SqlDataSource runat="server" ID="sqlLocation" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT Description + ' - ' + LTRIM(STR(Id)) AS text, Id AS value, 1 AS idx FROM Locations WHERE (Id > 0) UNION SELECT 'All Locations' AS text, NULL AS value, 0 AS idx ORDER BY idx, text"></asp:SqlDataSource>
        </div>

    </div>
    <script>
        $(function () {
            
            //$('#grd').removeClass();
            //$('#grd1').removeClass();
        });
    </script>

    <div >
        <asp:GridView runat="server" ID="grd1" Font-Size="0.8em" RowStyle-CssClass="p-0 text-nowrap text-left " AllowSorting="true" HorizontalAlign="Center" Width="100%" CssClass="table table-hover table-responsive table-sm " AutoGenerateColumns="False" DataSourceID="sqlAudit">

            <Columns>
                <asp:BoundField DataField="Location_Id" HeaderText="Location" SortExpression="Location_Id"></asp:BoundField>
                <asp:BoundField DataField="Audit_Date" HeaderText="Date" SortExpression="Audit_Date"></asp:BoundField>
                <asp:BoundField DataField="Server_Name" HeaderText="Server" SortExpression="Server_Name"></asp:BoundField>
                <asp:BoundField DataField="Type_Of_Audit" HeaderText="Audit type" SortExpression="Type_Of_Audit"></asp:BoundField>
                <asp:BoundField DataField="Record_Id" HeaderText="Record Id" SortExpression="Record_Id"></asp:BoundField>
                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description"></asp:BoundField>
                <asp:BoundField DataField="Old_Value" HeaderText="Old Value" SortExpression="Old_Value"></asp:BoundField>
                <asp:BoundField DataField="New_Value" HeaderText="New Value" SortExpression="New_Value"></asp:BoundField>
                <asp:BoundField DataField="Reason" HeaderText="Reason" SortExpression="Reason"></asp:BoundField>
                <asp:BoundField DataField="Operator" HeaderText="Operator" SortExpression="Operator"></asp:BoundField>
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource runat="server" CancelSelectOnNullParameter="false" ID="sqlAudit" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT Location_Id, Audit_Date, Server_Name, Type_Of_Audit, Record_Id, Description, Old_Value, New_Value, Reason, Operator FROM Audit_Trail WHERE (DATEDIFF(day, Audit_Date, @SelectedDate) <= 0) AND (DATEDIFF(day, Audit_Date, @SelectedEndDate) >= 0) AND (Location_Id = ISNULL(@Location_Id, Location_Id)) ORDER BY Audit_Date desc, Location_Id">
            <SelectParameters>
                <asp:ControlParameter ControlID="hfSelectedDate" PropertyName="Value" Name="SelectedDate"></asp:ControlParameter>
                <asp:ControlParameter ControlID="hfSelectedEndDate" PropertyName="Value" Name="SelectedEndDate"></asp:ControlParameter>
                <asp:ControlParameter ControlID="ddLocations" PropertyName="SelectedValue" Name="Location_Id"></asp:ControlParameter>
            </SelectParameters>
        </asp:SqlDataSource>
    </div>

</asp:Content>

