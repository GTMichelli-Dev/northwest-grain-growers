<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="UnknownTreatment.aspx.cs" Inherits="UnknownTreatment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
     <script>
        function SetPercent(value) {
            var n = value.toFixed(2);
            document.getElementById('<%= txtRate.ClientID %>').value = n;

                }

                function isNumberKey(evt) {
                    var charCode = (evt.which) ? evt.which : evt.keyCode;
                    if ((charCode > 31 && (charCode < 48 || charCode > 57)) && (charCode != 46))
                        return false;
                    return true;
                }




    </script>


   
    <h3 class=" text-danger ">Unknown Treatment</h3>
    <h4>Enter A Description (Min 2 Characters)</h4>
    <asp:UpdatePanel runat="server" ID="up1">
        <ContentTemplate>

            <asp:HiddenField runat="server" ID="hfPLCTreatments" />
          

            <div class="row pt-3">
                <div class="col-sm-12 col-md-8 col-lg-6">
                    <div class="input-group input-group-lg">
                        
                        <asp:TextBox CssClass="form-control" placeholder="Treatment Description" AutoCompleteType="Disabled" OnTextChanged="txtFilter_TextChanged" Font-Size="Larger" runat="server" ID="txtFilter" AutoPostBack="true"></asp:TextBox>

                    </div>




                </div>

            </div>
            <asp:Panel runat="server" ID="pnlRate">

            
            <div class=" row pt-2 ">
                <div class="col-10 col-sm-9 col-md-6 col-lg-5 col-xl-4 ">
                    <div class=" input-group  input-group-lg  ">
                        <asp:HiddenField runat="server" ID="hfDefaultRate" Value="0" />
                            <asp:TextBox runat="server" ID="txtRate" AutoPostBack="true" type="number" OnTextChanged="txtRate_TextChanged" MaxLength="5" onkeypress="return isNumberKey(event)" Text="0.00" AutoCompleteType="Disabled" CssClass="form-control text-right  "></asp:TextBox>



                        <div class="input-group-append ">
                            <asp:DropDownList runat="server" ID="ddrates" CssClass="input-group-text  " AutoPostBack="true" OnTextChanged="ddrates_TextChanged">
                                <asp:ListItem>Rate</asp:ListItem>
                                <asp:ListItem>0.25</asp:ListItem>
                                <asp:ListItem>0.50</asp:ListItem>
                                <asp:ListItem>0.75</asp:ListItem>
                                <asp:ListItem>1.00</asp:ListItem>
                                <asp:ListItem>1.25</asp:ListItem>
                                <asp:ListItem>1.50</asp:ListItem>
                                <asp:ListItem>1.75</asp:ListItem>
                                <asp:ListItem>2.00</asp:ListItem>
                                <asp:ListItem>2.40</asp:ListItem>
                                <asp:ListItem>2.50</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                      
                    </div>
                </div>
            </div>
                </asp:Panel>






      
          
            <div class="row pt-5">
                <div class="col-6 col-md-2">
                    <asp:Button runat="server" ID="btnCancel" CssClass="btn btn-danger " Width="100%" Text="Cancel" OnClick="btnCancel_Click" />
                </div>
                <div class="col-6 col-md-2">
                    <asp:Button runat="server" ID="btnOk" CssClass="btn btn-success " Width="100%" Text="OK" OnClick="btnOk_Click" />
                </div>
                

            </div>
                 

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentFull" Runat="Server">
</asp:Content>

