<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ProducersDeliveredReport.aspx.cs" Inherits="Reports_ProducersDeliveredReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdateProgress runat="server" ID="UPr">
        <ProgressTemplate >
            <h2 class="text-info text-center  ">Hang On ..... Getting Data</h2>
         </ProgressTemplate>
    </asp:UpdateProgress>
       <h3><asp:Label ID="lblHeader" runat="server" Text="Producer Delivery Report By District"> </asp:Label> &nbsp&nbsp <asp:LinkButton runat="server" Id="btnDownload" CssClass=" btn btn-success " OnClick="btnDownload_Click">Send To Excel</asp:LinkButton> </h3>
    <asp:UpdatePanel runat="server" ID="UP1">
        <ContentTemplate>



         
         
            <div class=" row ">
                <div class=" col-sm-offset-1 col-sm-10 ">
                    <table style="width: 100%" class="text-center ">
                        <tr>
                            <td style="height: 22px">District
                            </td>
                            <td style="height: 22px">Producer
                            </td>
                            <td style="height: 22px">From
                            </td>
                            <td style="height: 22px">To
                            </td>

                        </tr>
                        <tr>

                            <td>

                                <asp:DropDownList runat="server" ID="ddDistricts" CssClass="input-sm " DataSourceID="sqlDistricts" DataTextField="Text" DataValueField="Id" AutoPostBack="True" OnTextChanged="ddDistricts_TextChanged"></asp:DropDownList>
                                <asp:SqlDataSource runat="server" ID="sqlDistricts" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT 'All Districts' AS Text, null AS Id, 0 AS idx UNION SELECT District  AS Text, District as Id, 1 AS idx FROM Location_Districts ORDER BY idx, Text"></asp:SqlDataSource>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtProducer" CssClass="form-control " AutoPostBack="true" OnTextChanged="txtProducer_TextChanged"></asp:TextBox>
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

                </div>
            </div>


            <hr />

            <br />

            <div class=" row ">
                <div class=" offset-2 col-8 ">
                    <asp:GridView ID="GridView1" CssClass="table table-hover table-responsive  "  HorizontalAlign="Center" runat="server" AutoGenerateColumns="False" AllowSorting="True" DataSourceID="ObjProducer">

                        <Columns>
                            <asp:BoundField DataField="District" HeaderText="District" ItemStyle-CssClass="text-left " SortExpression="District"></asp:BoundField>
                            <asp:BoundField DataField="Producer_Id" HeaderText="Producer Id" ItemStyle-CssClass="text-left " SortExpression="Producer_Id"></asp:BoundField>
                            <asp:BoundField DataField="Producer" HeaderText="Producer" ItemStyle-CssClass="text-left " SortExpression="Producer"></asp:BoundField>
                            <asp:BoundField DataField="Net" HeaderText="Net lbs" ItemStyle-CssClass="text-right " SortExpression="Net" DataFormatString="{0:N0}" ReadOnly="True"></asp:BoundField>
                            <asp:BoundField DataField="Loads" HeaderText="# Loads" ItemStyle-CssClass="text-right " SortExpression="Loads" DataFormatString="{0:N0}" ReadOnly="True"></asp:BoundField>
                        </Columns>
                        <EmptyDataTemplate>
                            .
                         <h5 class="text-center">No Producers Matching Filter</h5>

                        </EmptyDataTemplate>
                    </asp:GridView>

                </div>
            </div>




            <asp:ObjectDataSource runat="server" ID="ObjProducer" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="ReportDataSetTableAdapters.ProducersDeliveredTableAdapter">
                <SelectParameters>
                    <asp:ControlParameter ControlID="ddDistricts" PropertyName="SelectedValue" Name="District" Type="String"></asp:ControlParameter>
                    <asp:ControlParameter ControlID="txtStartDate" PropertyName="Text" Name="StartDate" Type="DateTime"></asp:ControlParameter>
                    <asp:ControlParameter ControlID="txtEndDate" PropertyName="Text" Name="EndDate" Type="DateTime"></asp:ControlParameter>
                    <asp:ControlParameter ControlID="txtProducer" PropertyName="Text" Name="Producer" Type="String"></asp:ControlParameter>
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:HiddenField ID="hfStart" runat="server" />
            <asp:HiddenField ID="hfEnd" runat="server" />

        </ContentTemplate>

    </asp:UpdatePanel>
 <%--  <script src="../Scripts/jquery-3.4.1.min.js"></script>
<script src="../Scripts/bootstrap.min.js"></script>--%>

</asp:Content>

