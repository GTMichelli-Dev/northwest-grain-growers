<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="DailyBinReport.aspx.cs" Inherits="Reports_DailyBinReport" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:UpdateProgress runat="server" ID="UPr">
        <ProgressTemplate >
            <%--<h2 class="text-info text-center  ">Getting Data</h2>--%>
         </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel runat="server" ID="UP1">
        <ContentTemplate>



            <h3><asp:Label ID="lblHeader" runat="server" Text="Average Bin Protein Report"></asp:Label></h3>
            
             <table style="width:100%  " class="text-center " >
  
                <tr>
                    <td colspan="5" class="text-center ">
                    </td>
                      
                </tr>
                <tr>
                
               
                    <td >Location
                    </td>
                    <td >From
                    </td>
                    <td >
                        To
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
                    <td>
                         
                        <asp:TextBox runat="server" CssClass="input-sm " ID="txtEndDate" AutoPostBack="True" OnTextChanged="txtEndDate_TextChanged" ></asp:TextBox>
                        <ajaxToolkit:CalendarExtender runat="server" BehaviorID="txtEndDate_CalendarExtender" TargetControlID="txtEndDate" ID="txtEndDate_CalendarExtender"></ajaxToolkit:CalendarExtender>
                    
                    </td>
                 
                </tr>
            </table>


            <hr />

            <br />
            <div class="text-center ">
                <asp:LinkButton ID="lnkSelect" CssClass="btn btn-primary " OnClick="lnkSelect_Click" runat="server">Get Report</asp:LinkButton>
            </div>
             <br />

            
            <asp:GridView ID="GridView1" CssClass="table table-hover table-responsive  " HorizontalAlign="Center" runat="server" DataSourceID="objAvgProtein" Width="60%" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True" PageSize="50">
       
                <Columns>
                    <asp:BoundField DataField="Bin" HeaderText="Bin" ItemStyle-HorizontalAlign="Left"  SortExpression="Bin" >
                    <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Crop" HeaderText="Crop" SortExpression="Crop" />
                    <asp:BoundField DataField="Protein" HeaderText="Protein" ItemStyle-HorizontalAlign="Right"  SortExpression="Protein" DataFormatString="{0:N2}" HeaderStyle-HorizontalAlign="Right" ReadOnly="True" >
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Bushels" HeaderText="Bushels" ItemStyle-HorizontalAlign="Right"  ReadOnly="True" SortExpression="Bushels" DataFormatString="{0:N0}" HeaderStyle-HorizontalAlign="Right" >
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                </Columns>
                <EmptyDataTemplate>
                   <h2>No Data Matching Filter</h2> 
                </EmptyDataTemplate>
       
            </asp:GridView>
       

            <asp:ObjectDataSource ID="objAvgProtein" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="BinDataSetTableAdapters.AverageProteinByDateRangeTableAdapter">
                <SelectParameters>
                    <asp:ControlParameter ControlID="hfStart" Name="StartDate" PropertyName="Value" Type="DateTime" />
                    <asp:ControlParameter ControlID="hfEnd" Name="EndDate" PropertyName="Value" Type="DateTime" />
                    <asp:ControlParameter ControlID="cboLocation" Name="Location_Id" PropertyName="SelectedValue" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
       

            <asp:HiddenField ID="hfStart" runat="server" />
            <asp:HiddenField ID="hfEnd" runat="server" />

        </ContentTemplate>

    </asp:UpdatePanel>


</asp:Content>
