<%@ Page Title="Replication Monitor" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Replication.aspx.cs" Inherits="_Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
         

            <asp:Timer ID="tmrUpdate" runat="server" Interval="60000" OnTick="tmrUpdate_Tick"></asp:Timer>
            <div class=" container-fluid ">

                <div class="row">
                    <div class="col-lg-12 ">
                        <h3 class="text-center text-center ">Replication Monitor</h3>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-12 ">

                        <p class="text-center h4  ">
                            <asp:Label ID="lblLastUpdate" runat="server" Text="Label"></asp:Label>
                        </p>
                        <h4 class="text-center h5 ">Updated every 60 seconds</h4>
                    </div>
                </div>


                <div class="row">
                    <div class="col-lg-12 text-center ">

                        <asp:LinkButton CssClass="btn btn-sm  btn-primary   " ID="LinkButton1" runat="server">Refresh</asp:LinkButton>
                    </div>
                    <div class="col-lg-12 text-center ">

                        <asp:GridView ID="GridView1" CssClass="table table-bordered h6  " runat="server" DataSourceID="SqlDataSource1" AutoGenerateColumns="False" HorizontalAlign="Center" OnRowDataBound="GridView1_RowDataBound">
                            <Columns>



                                <asp:BoundField DataField="subscriber_name" ItemStyle-CssClass="text-left" HeaderText="Site" SortExpression="subscriber_name">
                                    <ItemStyle CssClass="text-left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="time" HeaderText="Last Sync" SortExpression="time" />
                                <asp:BoundField DataField="Status" ItemStyle-CssClass="text-left" HeaderText="Status" SortExpression="Status" />
                                <asp:BoundField DataField="upload_conflicts" ItemStyle-CssClass="text-right" HeaderStyle-Width="50px" HeaderText="Up Conflicts" SortExpression="upload_conflicts">
                                    <HeaderStyle Width="50px" />
                                    <ItemStyle CssClass="text-right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="download_conflicts" ItemStyle-CssClass="text-right" HeaderStyle-Width="50px" HeaderText="Down Conflicts" SortExpression="download_conflicts">
                                    <HeaderStyle Width="50px" />
                                    <ItemStyle CssClass="text-right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="comments" ItemStyle-CssClass="text-left" HeaderText="Comments" SortExpression="comments">
                                    <ItemStyle CssClass="text-left" />
                                </asp:BoundField>

                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString="<%$ ConnectionStrings:distributionConnectionString %>" SelectCommand="SELECT subscriber_name, time, Status, runstatus, upload_conflicts, download_conflicts, comments, error_id, agent_id, CASE WHEN runstatus = 6 THEN 6 WHEN Status = 'OK' THEN 4 WHEN runstatus = 3 THEN 5 ELSE runstatus END AS idx FROM (SELECT subscriber_name, time, CASE WHEN charindex('second(s) before polling for further changes' , Comments) &gt; 0 THEN 'OK' ELSE Status END AS Status, runstatus, upload_conflicts, download_conflicts, comments, error_id, agent_id FROM Subscription_Status) AS tblS WHERE (subscriber_name &lt;&gt; 'Leo') ORDER BY idx DESC, subscriber_name"></asp:SqlDataSource>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
   
</asp:Content>
