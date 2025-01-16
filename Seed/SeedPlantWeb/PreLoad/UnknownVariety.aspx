<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="UnknownVariety.aspx.cs" Inherits="UnknownVariety" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
     <script>
        function SetPercent(value) {
            var n = value.toFixed(2);
            document.getElementById('<%= txtPercent.ClientID %>').value = n;

                }

                function isNumberKey(evt) {
                    var charCode = (evt.which) ? evt.which : evt.keyCode;
                    if ((charCode > 31 && (charCode < 48 || charCode > 57)))
                        return false;
                    return true;
                }




    </script>


    <h3 class=" text-danger ">Unknown Variety</h3>
    <h4>Enter A Description (Min 2 Characters)</h4>
    <asp:UpdatePanel runat="server" ID="up1">
        <ContentTemplate>

   
            <div class="row pt-3">
                <div class="col-sm-12 col-md-8 col-lg-6">
                    <div class="input-group input-group-lg">
                        <asp:TextBox CssClass="form-control" placeholder=" Description Of Variety" AutoCompleteType="Disabled" OnTextChanged="txtFilter_TextChanged" Font-Size="Larger" runat="server" ID="txtFilter" AutoPostBack="true"></asp:TextBox>
                    </div>
                </div>

            </div>
            <div class=" row pt-2 ">
                <div class="col-12 col-md-6 col-lg-4">
                    <div class=" input-group  input-group-lg  ">

                        <asp:TextBox runat="server"  ID="txtPercent"  min="1" max="100" AutoPostBack="true" type="number" OnTextChanged="txtPercent_TextChanged" MaxLength="3" onkeypress="return isNumberKey(event)" Text="100" AutoCompleteType="Disabled" CssClass="form-control text-right  "></asp:TextBox>

                        
                        <div class="input-group-append ">
                            <asp:DropDownList runat="server" ID="ddPercents"  CssClass="input-group-text  " AutoPostBack="true" OnTextChanged="ddPercents_TextChanged" >
                                <asp:ListItem>% Load</asp:ListItem>
                                <asp:ListItem>5</asp:ListItem>
                                <asp:ListItem>15</asp:ListItem>
                                <asp:ListItem>20</asp:ListItem>
                                <asp:ListItem>25</asp:ListItem>
                                <asp:ListItem>30</asp:ListItem>
                                <asp:ListItem>33</asp:ListItem>
                                <asp:ListItem>40</asp:ListItem>
                                <asp:ListItem>50</asp:ListItem>
                                <asp:ListItem>60</asp:ListItem>
                                <asp:ListItem>66</asp:ListItem>
                                <asp:ListItem>70</asp:ListItem>
                                <asp:ListItem>75</asp:ListItem>
                                <asp:ListItem>80</asp:ListItem>
                                <asp:ListItem>85</asp:ListItem>
                                <asp:ListItem>100</asp:ListItem>

                            </asp:DropDownList>
                        </div>
                      
                    </div>
                </div>
            </div>







      
   
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

