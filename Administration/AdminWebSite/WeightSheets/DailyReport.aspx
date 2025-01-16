<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="DailyReport.aspx.cs" Inherits="WeightSheets_DailyReport" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
     <%-- <br/>  --%>
        <div class="text-center ">
        <asp:CheckBox runat="server" ID="ckDetails" AutoPostBack="true" Text="Summary" />
    </div>

    <asp:LinkButton ID="lnkPrint" CssClass="btn btn-sm btn-primary  " OnClick="lnkPrint_Click" runat="server">Download</asp:LinkButton>


        
     <div class="text-center ">

        <table style="width:100%  " >
            <tr style="width:100%">
                <td style="width:150px"></td>
                <td >
                 
                 <CR:CrystalReportViewer ID="CrystalReportViewer1"  CssClass="text-center " runat="server" AutoDataBind="True"  EnableDatabaseLogonPrompt="False" EnableParameterPrompt="False" ToolPanelView="None" HasPrintButton="False" HasSearchButton="False" HasToggleGroupTreeButton="False" HasToggleParameterPanelButton="False" HasZoomFactorList="False" Width="350px" />


                </td>
            </tr>
        </table>

           

    </div>
</asp:Content>

