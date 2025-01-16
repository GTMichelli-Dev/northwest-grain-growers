<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="WeightSheet.aspx.cs" Inherits="WeightSheets_WeightSheet" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <script>
        $(function () {
            $('#pnlEmail').hide()
        });

        function getEmail() {
            $('#pnlEmail').show();
            $('.mainBtns').hide();
            $('#txtEmail').val('');
            $('.btnSendEmail').hide()
        }

        function checkIfBlank(ev) {
            if (($('#txtEmail').val()).length > 0)
            {
                $('#btnSendEmail').show();
            }
            else
            {
                $('#btnSendEmail').hide();
            }
        }
    </script>
     <%-- <br/>  --%>
    <asp:PlaceHolder runat="server" ID="PlaceholderUser">
        <div class=" flex-column mainBtns">
            <asp:LinkButton ID="LinkButton1" Width="100px" CssClass="btn btn-sm btn-primary mr-1  " OnClick="lnkPrint_Click" runat="server">Download</asp:LinkButton>
            <input type="button" style="width:100px" class="btn btn-sm btn-primary  ml-1 btnEmail" runat="server"   onclick="getEmail()"  value="Email" />
        </div>
      
    </asp:PlaceHolder>
    
    <asp:PlaceHolder ID="PlaceholderAdmin" runat="server">
        
        <div class="container  ">
            
        <div class="row">
        <div class=" offset-3  col-6  text-center ">
                    <div class="  flex-column mainBtns">
                        <asp:LinkButton ID="lnkPrint" Width="100px" CssClass="btn btn-sm btn-primary mr-1  " OnClick="lnkPrint_Click" runat="server">Download</asp:LinkButton>
                        <input type="button" style="width:100px" class="btn btn-sm btn-primary  ml-1 btnEmail" runat="server"   onclick="getEmail()"  value="Email" />
                    </div>
                    <div class="  flex-column mt-2" >
                            <asp:LinkButton ID="lnkFix"  Width="200px"  CssClass="btn btn-sm btn-primary  " OnClick="lnkFix_Click" runat="server">Details</asp:LinkButton>
                    </div>
                    

            <table style="width: 100%">
              
                <tr>
                    
                    <td style="width:200px ">
                        <asp:LinkButton ID="lnkOpenWS" Width="200px" CssClass="btn btn-sm btn-warning " runat="server" OnClick="lnkOpenWS_Click">Re-Open Weight Sheet</asp:LinkButton>
                        <ajaxToolkit:ConfirmButtonExtender runat="server" ConfirmText="Are You Sure You Want To Re-Open Weight Sheet" BehaviorID="lnkOpenWS_ConfirmButtonExtender" TargetControlID="lnkOpenWS" ID="lnkOpenWS_ConfirmButtonExtender"></ajaxToolkit:ConfirmButtonExtender>
                    </td>
                    <td >
                           <asp:CheckBox ID="ckOriginal" AutoPostBack="true" runat="server" Text="Show As Original" />
                    </td>
                    <td style="width:200px ">
                        <asp:LinkButton ID="lnkOriginal"  Width="200px"  CssClass="btn btn-sm btn-warning " runat="server" OnClick="lnkOriginal_Click">Mark As Original Printed</asp:LinkButton>
                        <ajaxToolkit:ConfirmButtonExtender runat="server" ConfirmText="" BehaviorID="lnkOriginal_ConfirmButtonExtender" TargetControlID="lnkOriginal" ID="lnkOriginal_ConfirmButtonExtender"></ajaxToolkit:ConfirmButtonExtender>
                     
                    </td>

                </tr>
            </table>

        </div>
            </div>
            </div>

    </asp:PlaceHolder>
       <asp:Panel runat="server" ID="pnlEmail" ClientIDMode="static" Visible="true" HorizontalAlign="center">
            <div class="row">
                <div class=" col-3"></div>
                <div class=" col-6">
                    <div class="form-group">
                        <label>Email Address <i>Seperate multliple emails with a simicolon ;</i> </label>
                        <input type="text" id="txtEmail"  ClientIDMode="Static"  runat="server"  class="form-control " onkeyup="checkIfBlank(this)" />                        

                    </div>
                    <div class=" col-3"></div>
                </div>
            </div>
            <div class="row">
                <div class=" col-3"></div>
                <div class=" flex-column  col-6">
                    <asp:LinkButton runat="server" Visible="true" Width="100px" CssClass="btn btn-success btn-sm" ID="btnSendEmail" OnClick="btnSendEmail_Click" ClientIDMode="Static" Text="Send"></asp:LinkButton>
                    <input type="button" style="width:100px" class="btn btn-danger btn-sm" ID="cancelEmail" onclick="$('#pnlEmail').hide();$('#mainBtns').show()"  value="Cancel" />
                </div>
                <div class=" col-3"></div>
            </div>

            
            

        </asp:Panel>
    <br />
       <asp:Label CssClass="h3" ID="lblWSStatus" runat="server" Text=""></asp:Label>
     <asp:HiddenField ID="hfOriginalPrinted" runat="server" />

    <div class="text-center ">

        <table style="width:100%  " >
       
            <tr style="width:100%">
                <td style="width:150px"></td>
                <td >
                 
                 <CR:CrystalReportViewer ID="CrystalReportViewer1"  CssClass="text-center " runat="server" AutoDataBind="True" EnableDatabaseLogonPrompt="False" EnableParameterPrompt="False" ToolPanelView="None" DisplayStatusbar="False" DisplayToolbar="False" HasDrilldownTabs="False" HasGotoPageButton="False" HasPageNavigationButtons="False" HasPrintButton="False" HasSearchButton="False" HasToggleGroupTreeButton="False" HasToggleParameterPanelButton="False" HasZoomFactorList="False" Width="350px" />


                </td>
            </tr>
        </table>

           

    </div>
</asp:Content>

