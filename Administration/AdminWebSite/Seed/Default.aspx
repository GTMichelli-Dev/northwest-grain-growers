<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Seed.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script src="Scripts/jquery.timepicker.js"></script>


    <script src="Scripts/bootstrap-datepicker.js"></script>




    <script src="Scripts/jquery.datepair.js"></script>


    <script>

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }

        $(function () {
            $('#ticketFilter .date ').datepicker({
                'format': 'mm-dd-yyyy',
                'autoclose': true
            });

            $('#<%=txtStartDate.ClientID%>').on('click', function () {
                $('#<%=txtStartDate.ClientID%>').datepicker('show');
            });

            $('#<%=txtEndDate.ClientID%>').on('click', function () {
                $('#<%=txtEndDate.ClientID%>').datepicker('show');
            });

            $('#startCalendarIcon').on('click', function () {
                $('#<%=txtStartDate.ClientID%>').datepicker('show');
            });

            $('#endCalendarIcon').on('click', function () {
                $('#<%=txtEndDate.ClientID%>').datepicker('show');
            });
        });


     


        // initialize datepair
    <%--    $(function () {
             $('#<%=ticketFilter.ClientID%>').datepair({
                'defaultDateDelta': 0,
                'defaultTimeDelta': 1800000

            });
        });--%>



      


        function NewDate() {

            var dt = getFormattedDate(new Date());
            var val = 100;
            document.getElementById("<%=txtStartDate.ClientID%>").value = dt;
            document.getElementById("<%=txtEndDate.ClientID%>").value = dt;


        };




        function getFormattedDate(date) {
            var year = date.getFullYear();

            var month = (1 + date.getMonth()).toString();
            month = month.length > 1 ? month : '0' + month;

            var day = date.getDate().toString();
            day = day.length > 1 ? day : '0' + day;

            return month + '-' + day + '-' + year;
        }
    </script>


    <div>
        <asp:UpdateProgress runat="server" ID="UPr">
            <ProgressTemplate>
                <h4 class="font-weight-bold font-italic  text-center  ">Updating....</h4>
            </ProgressTemplate>
        </asp:UpdateProgress>

        <asp:HiddenField runat="server" ID="hfLocation" />
        <asp:Table runat="server" HorizontalAlign="Center">
            <asp:TableRow>
                <asp:TableCell>
                    <asp:DropDownList runat="server" ID="ddType" OnTextChanged="ddType_TextChanged" CssClass="form-control Width280 " AutoPostBack="true" DataSourceID="sqlComplete" DataTextField="text" DataValueField="value">
                    </asp:DropDownList>
                    <asp:SqlDataSource runat="server" ID="sqlComplete" ConnectionString='<%$ ConnectionStrings:Seed_DataConnectionString %>' SelectCommand="SELECT 'Tickets Not Finished' AS text, 0 AS value UNION SELECT 'Finished Tickets' AS text, 1 AS value UNION SELECT 'Pending Tickets' AS Text, 2 AS value ORDER BY value"></asp:SqlDataSource>
                </asp:TableCell>
                <asp:TableCell runat="server" ID="newTicket">
                    <asp:Button runat="server" ID="btnNew" CssClass="btn btn-sm btn-secondary  " OnClick="btnNew_Click" Text="New Ticket" />
                </asp:TableCell>
            </asp:TableRow>


        </asp:Table>
        <asp:Table runat="server" ID="ticketFilter" HorizontalAlign="Center">
            <asp:TableRow>
                <asp:TableCell>
                    Filter By Ticket
                </asp:TableCell>
                <asp:TableCell>
                    Filter By Customer
                </asp:TableCell>
                <asp:TableCell>
                    From
                </asp:TableCell>
                <asp:TableCell>
                    To
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:TextBox runat="server" autocomplete="off" MaxLength="10" AutoCompleteType="Disabled" onkeypress="return isNumberKey(event)" placeholder="Find Seed Ticket" ID="txtTicket" AutoPostBack="true" CssClass="form-control "></asp:TextBox>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox runat="server" autocomplete="off" MaxLength="10" AutoCompleteType="Disabled" placeholder="Customer" ID="txtCustomer" AutoPostBack="true" CssClass="form-control "></asp:TextBox>
                </asp:TableCell>
                <asp:TableCell CssClass="datepicker ">
                    <div class="input-group  ">
                        <asp:TextBox runat="server" autocomplete="off" AutoCompleteType="Disabled" ID="txtStartDate" OnTextChanged="txtStartDate_TextChanged" AutoPostBack="true" CssClass=" form-control date start "></asp:TextBox>
                        <span id="startCalendarIcon" class=" input-append fa fa-calendar fa-2x   "></span>
                    </div>
                </asp:TableCell>
                <asp:TableCell CssClass="datepicker ">
                    <div class="input-group ">
                        <asp:TextBox runat="server" ID="txtEndDate" autocomplete="off" AutoCompleteType="Disabled" OnTextChanged="txtEndDate_TextChanged" AutoPostBack="true" CssClass=" form-control date end"></asp:TextBox>
                        <span id="endCalendarIcon" class=" input-append fa fa-calendar fa-2x   "></span>

                    </div>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Button runat="server" ID="btnReset" Text="Reset Filter" CssClass="btn btn-sm btn-outline-dark " OnClick="btnReset_Click" />
                </asp:TableCell>
            </asp:TableRow>

        </asp:Table>


    </div>
                
   <asp:UpdatePanel runat="server" ID="UP1" >
       <ContentTemplate>
           <asp:Timer runat="server" ID="tmrUPDate" OnTick="tmrUPDate_Tick"  Enabled="true" Interval="5000"></asp:Timer>
    <asp:Panel runat="server" ID="pnlGrid" HorizontalAlign="Center" CssClass="pr-1 pl-1 pr-lg-5  pl-lg-5 ">
        <asp:GridView runat="server" HorizontalAlign="Center" AllowSorting="True" ID="Grd1" AutoGenerateColumns="False" CssClass=" table table-striped  table-hover table-sm table-responsive-sm table-responsive-md " DataSourceID="SqlTickets" DataKeyNames="UID,Ticket">

            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="btnSelect" CssClass="btn btn-sm btn-secondary " OnClick="btnSelect_Click" Text="Select" />

                    </ItemTemplate>
                </asp:TemplateField>
            
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="btnDelete" OnClick="btnDelete_Click" Text="Delete" CssClass="btn btn-sm btn-danger " />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="Ticket" HeaderText="Ticket" SortExpression="Ticket" />
                <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" />
                <asp:BoundField DataField="Grower_Description" HeaderText="Customer" SortExpression="Grower_Description">
                <ItemStyle CssClass="text-left" />
                </asp:BoundField>
                <asp:BoundField DataField="Truck_ID" HeaderText="Truck ID" SortExpression="Truck_ID" />
                <asp:BoundField DataField="BOL" HeaderText="BOL" SortExpression="BOL">
                <ItemStyle CssClass="text-left" />
                </asp:BoundField>
                <asp:BoundField DataField="PO" HeaderText="PO" SortExpression="PO">
                <ItemStyle CssClass="text-left" />
                </asp:BoundField>
                <asp:BoundField DataField="Comments" HeaderText="Comments" SortExpression="Comments">
                <ItemStyle CssClass="text-left" />
                </asp:BoundField>

                <asp:BoundField DataField="Ticket_Date" HeaderText="Time In" SortExpression="Ticket_Date"></asp:BoundField>
            </Columns>
            <EmptyDataTemplate>
                <h5>No Seed Tickets</h5>
            </EmptyDataTemplate>
        </asp:GridView>

    </asp:Panel>
           <asp:SqlDataSource runat="server" ID="SqlTickets" ConnectionString='<%$ ConnectionStrings:Seed_DataConnectionString %>' SelectCommand="SELECT CASE WHEN Ticket IS NULL THEN 0 ELSE 1 END AS AllowPrint, Ticket_Type AS Type, Ticket, Truck_ID, Grower_Name AS Grower_Description, BOL, Comments, Ticket_Date, PO, UID, CASE WHEN Ticket IS NULL THEN CONVERT (bit , 0) ELSE CONVERT (bit , 1) END AS Completed, Pending, CASE WHEN Ticket IS NULL THEN CASE WHEN Pending = 0 THEN 0 ELSE 2 END ELSE 1 END AS Status FROM Seed_Tickets WHERE (DATEDIFF(day, ISNULL(@Start, Ticket_Date), Ticket_Date) >= 0) AND (DATEDIFF(day, Ticket_Date, ISNULL( @End , Ticket_Date)) >= 0) AND (Location_ID = @Location_Id) AND (@Ticket IS NULL) AND (Grower_Name LIKE N'%' + ISNULL(@Description, Grower_Name) + N'%') AND (CASE WHEN Ticket IS NULL THEN CASE WHEN Pending = 0 THEN 0 ELSE 2 END ELSE 1 END = 1) AND (@Status = 1) OR (Ticket = @Ticket) OR (Location_ID = @Location_Id) AND (CASE WHEN Ticket IS NULL THEN CASE WHEN Pending = 0 THEN 0 ELSE 2 END ELSE 1 END = 0) AND (@Status = 0) OR (Location_ID = @Location_Id) AND (CASE WHEN Ticket IS NULL THEN CASE WHEN Pending = 0 THEN 0 ELSE 2 END ELSE 1 END = 2) AND (@Status = 2) ORDER BY Ticket_Date DESC" CancelSelectOnNullParameter="False">
               <SelectParameters>
                   <asp:ControlParameter ControlID="txtStartDate" PropertyName="Text" Name="Start"></asp:ControlParameter>
                   <asp:ControlParameter ControlID="txtEndDate" PropertyName="Text" Name="End"></asp:ControlParameter>
                   <asp:ControlParameter ControlID="hfLocation" PropertyName="Value" Name="Location_Id"></asp:ControlParameter>
                   <asp:ControlParameter ControlID="txtTicket" PropertyName="Text" Name="Ticket"></asp:ControlParameter>

                   <asp:ControlParameter ControlID="txtCustomer" PropertyName="Text" Name="Description"></asp:ControlParameter>
                   <asp:ControlParameter ControlID="ddType" PropertyName="SelectedValue" Name="Status"></asp:ControlParameter>
               </SelectParameters>
           </asp:SqlDataSource>
       </ContentTemplate>
       
    </asp:UpdatePanel>



   


</asp:Content>
