<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="TotalsByDateRange.aspx.cs" Inherits="TotalsByDateRange" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
   
   
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Timer runat="server" ID="tmrUpdate" Interval="120000" OnTick="tmrUpdate_Tick" Enabled="true" ></asp:Timer>
            <div class="row">
            <div class=" offset-3  col-md-2 text-center ">

                <asp:TextBox runat="server" ID="txtSelectedDate" CssClass="  text-center form-control  " Font-Size="Large"  AutoPostBack="true" OnTextChanged="txtSelectedDate_TextChanged" ></asp:TextBox>
                  <ajaxToolkit:CalendarExtender runat="server" BehaviorID="txtSelectedDate_CalendarExtender" TargetControlID="txtSelectedDate" ID="txtSelectedDate_CalendarExtender"></ajaxToolkit:CalendarExtender>
            </div>
            <div class="col-md-2 text-center ">

                <asp:TextBox runat="server" ID="txtSelectedEndDate" CssClass="  text-center form-control  " Font-Size="Large"  AutoPostBack="true" OnTextChanged="txtSelectedEndDate_TextChanged"  ></asp:TextBox>
                  <ajaxToolkit:CalendarExtender runat="server" BehaviorID="txtSelectedEndDate_CalendarExtender" TargetControlID="txtSelectedEndDate" ID="txtSelectedEndDate_CalendarExtender"></ajaxToolkit:CalendarExtender>
            </div>


            <div class="col-md-2"><asp:LinkButton runat="server" ID="DateRange" Text="By Date" PostBackUrl="~/Default.aspx" CssClass="btn btn-default "></asp:LinkButton> </div>
                </div>
            <h4>
                <asp:Label ID="lblLastUpdate" runat="server" Text=""></asp:Label>

                <asp:GridView runat="server" CellPadding="10" ID="GrdTotals" HorizontalAlign="Center" DataSourceID="sqlTotalLoads" AutoGenerateColumns="False">

                    <Columns>
                        <asp:BoundField DataField="TotalLoads" DataFormatString="{0:N0}" HeaderText="Total Loads" ReadOnly="True" SortExpression="TotalLoads">
                        <HeaderStyle HorizontalAlign="Center" Font-Size="Small"  CssClass="text-center "  />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TotalBushels" DataFormatString="{0:N0}" HeaderText="Bushels Recieved" ReadOnly="True" SortExpression="TotalBushels">
                        <HeaderStyle HorizontalAlign="Center" Font-Size="Small" CssClass="text-center "  />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Intake" DataFormatString="{0:N0}" HeaderText="Intake Loads" ReadOnly="True" SortExpression="Intake">
                        <HeaderStyle HorizontalAlign="Center" Font-Size="Small" CssClass="text-center "  />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Transfered" DataFormatString="{0:N0}" HeaderText="Transfer Loads" ReadOnly="True" SortExpression="Transfered">
                        <HeaderStyle HorizontalAlign="Center" Font-Size="Small" CssClass="text-center "  />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="IntakeBushels" DataFormatString="{0:N0}" HeaderText="Intake Bushels" ReadOnly="True" SortExpression="IntakeBushels">
                        <HeaderStyle HorizontalAlign="Center" Font-Size="Small" CssClass="text-center "  />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                        <asp:BoundField DataField="TransferBushels" DataFormatString="{0:N0}" HeaderText="Transfer Bushels" ReadOnly="True" SortExpression="TransferBushels">
                        <HeaderStyle HorizontalAlign="Center" Font-Size="Small" CssClass="text-center "  />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="IntakeNet" DataFormatString="{0:N0}" HeaderText="Intake Net" ReadOnly="True" SortExpression="IntakeNet">
                        <HeaderStyle HorizontalAlign="Center" Font-Size="Small" CssClass="text-center "  />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
 

                        <asp:BoundField DataField="TransferNet" DataFormatString="{0:N0}" HeaderText="Transfer Net" ReadOnly="True" SortExpression="TransferNet">
                        <HeaderStyle HorizontalAlign="Center" Font-Size="Small" CssClass="text-center "  />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                    </Columns>

                </asp:GridView>
                <asp:SqlDataSource ID="sqlTotalLoads" runat="server" ConnectionString="<%$ ConnectionStrings:NW_DataConnectionString %>" SelectCommand="SELECT ISNULL(SUM(CONVERT (bigint, Transfered)), 0) AS Transfered, ISNULL(SUM(CONVERT (bigint, Intake)), 0) AS Intake, ISNULL(SUM(CONVERT (bigint, TransferNet)), 0) AS TransferNet, ISNULL(SUM(CONVERT (bigint, TransferBushels)), 0) AS TransferBushels, ISNULL(SUM(CONVERT (bigint, IntakeNet)), 0) AS IntakeNet, ISNULL(SUM(CONVERT (bigint, IntakeBushels)), 0) AS IntakeBushels, ISNULL(SUM(CONVERT (bigint, Recieved)), 0) AS TotalRecieved, ISNULL(SUM(CONVERT (bigint, BushelsRecieved)), 0) AS TotalBushels, COUNT(*) AS TotalLoads FROM (SELECT CASE WHEN Load_UID IS NULL THEN 1 ELSE 0 END AS Transfered, CASE WHEN Load_UID IS NULL THEN 0 ELSE 1 END AS Intake, CASE WHEN Load_UID IS NULL THEN abs(Weight_In - Weight_Out) ELSE 0 END AS TransferNet, CASE WHEN Load_UID IS NULL THEN abs(Weight_In - Weight_Out) / 60 ELSE 0 END AS TransferBushels, CASE WHEN Load_UID IS NULL THEN 0 ELSE abs(Weight_In - Weight_Out) END AS IntakeNet, CASE WHEN Load_UID IS NULL THEN 0 ELSE abs(Weight_In - Weight_Out) / 60 END AS IntakeBushels, CASE WHEN Weight_In - Weight_Out &gt; 0 THEN Weight_In - Weight_Out ELSE 0 END AS Recieved, CASE WHEN Weight_In - Weight_Out &gt; 0 THEN (Weight_In - Weight_Out) / 60 ELSE 0 END AS BushelsRecieved FROM Loads LEFT OUTER JOIN Inbound_Loads ON Loads.UID = Inbound_Loads.Load_UID WHERE (Loads.Time_Out IS NOT NULL) AND (DATEDIFF(day, Loads.Time_Out, @SelectedDate) &lt;= 0) AND (DATEDIFF(day, Loads.Time_Out, @SelectedEndDate) &gt;= 0)) AS TLoad">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hfSelectedDate" Name="SelectedDate" PropertyName="Value" />
                        <asp:ControlParameter ControlID="hfSelectedEndDate" Name="SelectedEndDate" PropertyName="Value" />
                    </SelectParameters>
                </asp:SqlDataSource>

            </h4>
          
            <div style="font-size: small">
                <div>
                    <asp:GridView ID="GridView1" CssClass="table table-hover table-condensed  " HorizontalAlign="Center" runat="server" AutoGenerateColumns="False" DataSourceID="sqlDaily" AllowSorting="True">
                        <Columns>
                            <asp:BoundField DataField="Location_Id" SortExpression="Location_Id">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Description" HeaderText="Location" SortExpression="Description">
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>

                            <asp:BoundField DataField="LoadsFinished" HeaderText="# Loads" ReadOnly="True" SortExpression="LoadsFinished">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NetIntakeBushels" HeaderText="Intake Bushels" DataFormatString="{0:N0}" ReadOnly="True" SortExpression="NetIntakeBushels">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NetTransferBushels" HeaderText="Transfer Bushels" DataFormatString="{0:N0}" ReadOnly="True" SortExpression="NetTransferBushels">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TotalNetBushels" HeaderText="Total Bushels" DataFormatString="{0:N0}" ReadOnly="True" SortExpression="TotalNetBushels">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>

                            <asp:BoundField DataField="NetIntakeLbs" HeaderText="Intake Lbs" DataFormatString="{0:N0}" ReadOnly="True" SortExpression="NetIntakeLbs">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NetTransferLbs" HeaderText="Transfer Lbs" DataFormatString="{0:N0}" ReadOnly="True" SortExpression="NetTransferLbs">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TotalLbs" HeaderText="Total Lbs" DataFormatString="{0:N0}" ReadOnly="True" SortExpression="TotalLbs">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>

                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource runat="server" ID="sqlDaily" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT Location, SUM(CONVERT (bigint, Finished)) AS LoadsFinished, SUM(CONVERT (bigint, Intake)) AS IntakeLoads, SUM(CONVERT (bigint, Transfer)) AS TransferLoads, SUM(CONVERT (bigint, AtElev)) AS LoadsAtElevator, SUM(CONVERT (bigint, NetIntake)) AS NetIntakeLbs, SUM(CONVERT (bigint, NetTransfer)) AS NetTransferLbs, SUM(CONVERT (bigint, NetIntake) + CONVERT (bigint, NetTransfer)) AS TotalLbs, SUM(CONVERT (bigint, NetIntake) / 60) AS NetIntakeBushels, SUM(CONVERT (bigint, NetTransfer) / 60) AS NetTransferBushels, SUM(CONVERT (bigint, NetIntake + NetTransfer) / 60) AS TotalNetBushels, Location_Id, Description FROM (SELECT Loads.Location_Id, Locations.Description, Locations.Description + N' - ' + LTRIM(STR(Locations.Id)) AS Location, CASE WHEN Time_Out IS NULL THEN 0 ELSE 1 END AS Finished, Inbound_Loads.Load_UID, CASE WHEN 'Load_UID not' IS NULL THEN 1 ELSE 0 END AS Intake, CASE WHEN Load_UID IS NOT NULL THEN 0 ELSE 1 END AS Transfer, CASE WHEN Time_Out IS NULL THEN 1 ELSE 0 END AS AtElev, CASE WHEN Time_Out IS NULL THEN 0 ELSE CASE WHEN Load_UID IS NOT NULL THEN weight_In - Weight_Out ELSE 0 END END AS NetIntake, CASE WHEN Time_Out IS NULL THEN 0 ELSE CASE WHEN Load_UID IS NULL THEN weight_In - Weight_Out ELSE 0 END END AS NetTransfer FROM Loads INNER JOIN Locations ON Loads.Location_Id = Locations.Id LEFT OUTER JOIN Inbound_Loads ON Loads.UID = Inbound_Loads.Load_UID WHERE (DATEDIFF(day, Loads.Time_Out, @SelectedDate) &lt;= 0) AND (DATEDIFF(day, Loads.Time_Out, @SelectedEndDate) &gt;= 0)) AS tblLoads GROUP BY Location_Id, Description, Location ORDER BY Location">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="hfSelectedDate" Name="SelectedDate" PropertyName="Value" />
                            <asp:ControlParameter ControlID="hfSelectedEndDate" Name="SelectedEndDate" PropertyName="Value" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:HiddenField ID="hfSelectedDate" runat="server" />
                     <asp:HiddenField ID="hfSelectedEndDate" runat="server" />
                </div>


            </div>
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>
