<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Stats.aspx.cs" Inherits="Stats" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Stats</title>
    <link href="Content/bootstrap.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <h2 class="text-center ">
                <asp:Label ID="Label1" runat="server" Text="Label">
                    <asp:TextBox ID="txtDateTime" runat="server"></asp:TextBox></asp:Label></h2>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>

                    <div class="container text-center  ">
                        <div class="col-md-6">
                       
                        <div class="row">
                            <h3>Latest Count Of Inbound Loads
                            </h3>
                        </div>
                        <div class="row text-center ">
                            <asp:GridView ID="grdTotal" HorizontalAlign="Center" CssClass="table-condensed table-bordered table-hover " runat="server" AutoGenerateColumns="False" DataSourceID="SqlTotal">
                                <Columns>
                                    <asp:BoundField DataField="Loads" HeaderText="Loads" ReadOnly="True" SortExpression="Loads" DataFormatString="{0:N0}">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Bushels" DataFormatString="{0:N0}" HeaderText="Bushels" ReadOnly="True" SortExpression="Bushels">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                            <asp:SqlDataSource runat="server" ID="SqlTotal" ConnectionString='<%$ ConnectionStrings:LocalDBConnectionString %>' SelectCommand="SELECT        COUNT(*) AS [Loads],  SUM(Net)/60 AS Bushels
FROM            (SELECT         ABS(vw_Loads_By_Type.Weight_In - vw_Loads_By_Type.Weight_Out) AS Net
                          FROM            vw_Loads_By_Type INNER JOIN
                                                    Locations ON vw_Loads_By_Type.Location_Id = Locations.Id
                          WHERE        (DATEDIFF(day, vw_Loads_By_Type.Time_Out, GETDATE()) = 0) AND (vw_Loads_By_Type.Load_Type_Description = 'Inbound')) AS InboundLoads
"></asp:SqlDataSource>
                            <asp:GridView ID="grdCount" HorizontalAlign="Center" CssClass="table-condensed table-bordered table-hover " runat="server" AutoGenerateColumns="False" DataSourceID="sqlCount" AllowSorting="True">

                                <Columns>
                                    <asp:BoundField DataField="Location" HeaderText="Location" SortExpression="Location">
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Loads" HeaderText="Loads" SortExpression="Loads" DataFormatString="{0:N0}" ReadOnly="True">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Bushels" HeaderText="Bushels" ReadOnly="True" SortExpression="Bushels" DataFormatString="{0:N0}">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Avg Unload Time" HeaderText="Avg Unload Time" SortExpression="Avg Unload Time">
                                    <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Last Weighment" HeaderText="Last Weighment" ReadOnly="True" SortExpression="Last Weighment" />
                                </Columns>
                            </asp:GridView>
                            <asp:SqlDataSource runat="server" ID="sqlCount" ConnectionString='<%$ ConnectionStrings:LocalDBConnectionString %>' SelectCommand="SELECT COUNT(*) AS Loads, Location, MAX(Time_Out) AS [Last Weighment], SUM(Net) / 60 AS Bushels, CONVERT (time(0), DATEADD(SECOND, AVG(Time_On_Site), 0)) AS [Avg Unload Time] FROM (SELECT Locations.Description AS Location, vw_Loads_By_Type.Load_Type_Description, ABS(vw_Loads_By_Type.Weight_In - vw_Loads_By_Type.Weight_Out) AS Net, vw_Loads_By_Type.Time_Out, DATEDIFF(s, vw_Loads_By_Type.Time_In, vw_Loads_By_Type.Time_Out) AS Time_On_Site FROM vw_Loads_By_Type INNER JOIN Locations ON vw_Loads_By_Type.Location_Id = Locations.Id WHERE (DATEDIFF(day, vw_Loads_By_Type.Time_Out, GETDATE()) = 0) AND (vw_Loads_By_Type.Load_Type_Description = 'Inbound')) AS InboundLoads GROUP BY Location ORDER BY Location"></asp:SqlDataSource>
                        </div>
                             </div>
                        <div class="col-md-6">
                            <div class="row">
                                <h3>Latest Count Of Transfer Loads
                                </h3>
                            </div>
                            <div class="row text-center ">
                                <asp:GridView ID="GridView1" HorizontalAlign="Center" CssClass="table-condensed table-bordered table-hover " runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1">
                                    <Columns>
                                        <asp:BoundField DataField="Loads" HeaderText="Loads" ReadOnly="True" SortExpression="Loads" DataFormatString="{0:N0}">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Bushels" DataFormatString="{0:N0}" HeaderText="Bushels" ReadOnly="True" SortExpression="Bushels">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:LocalDBConnectionString %>' SelectCommand="SELECT        COUNT(*) AS [Loads],  SUM(Net)/60 AS Bushels
                                    FROM            (SELECT         ABS(vw_Loads_By_Type.Weight_In - vw_Loads_By_Type.Weight_Out) AS Net
                          FROM            vw_Loads_By_Type INNER JOIN
                                                    Locations ON vw_Loads_By_Type.Location_Id = Locations.Id
                          WHERE        (DATEDIFF(day, vw_Loads_By_Type.Time_Out, GETDATE()) = 0) AND (vw_Loads_By_Type.Load_Type_Description = 'Transfer')) AS InboundLoads
"></asp:SqlDataSource>
                                <asp:GridView ID="GridView2" HorizontalAlign="Center" CssClass="table-condensed table-bordered table-hover " runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource2" AllowSorting="True">

                                    <Columns>
                                        <asp:BoundField DataField="Location" HeaderText="Location" SortExpression="Location">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Loads" HeaderText="Loads" SortExpression="Loads" DataFormatString="{0:N0}" ReadOnly="True">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Bushels" HeaderText="Bushels" ReadOnly="True" SortExpression="Bushels" DataFormatString="{0:N0}">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Avg load Time" HeaderText="Avg load Time" SortExpression="Avg load Time">
                                        <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Last Weighment" HeaderText="Last Weighment" ReadOnly="True" SortExpression="Last Weighment" />
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:LocalDBConnectionString %>' SelectCommand="SELECT        COUNT(*) AS [Loads], Location, MAX(Time_Out) AS [Last Weighment], SUM(Net)/60 AS Bushels, 
  CONVERT(time(0), DATEADD(SECOND, AVG(Time_On_Site), 0))    as [Avg load Time]
FROM            (SELECT        Locations.Description AS Location, vw_Loads_By_Type.Load_Type_Description, ABS(vw_Loads_By_Type.Weight_In - vw_Loads_By_Type.Weight_Out) AS Net, vw_Loads_By_Type.Time_Out,  DATEDIFF(s, vw_Loads_By_Type.Time_In, vw_Loads_By_Type.Time_Out) AS Time_On_Site
                          FROM            vw_Loads_By_Type INNER JOIN
                                                    Locations ON vw_Loads_By_Type.Location_Id = Locations.Id
                          WHERE        (DATEDIFF(day, vw_Loads_By_Type.Time_Out, GETDATE()) = 0) AND (vw_Loads_By_Type.Load_Type_Description = 'Transfer')) AS InboundLoads
GROUP BY Location

 ORDER BY Location"></asp:SqlDataSource>
                            </div>
                        </div>
                    </div>


                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
    </form>
</body>
</html>
