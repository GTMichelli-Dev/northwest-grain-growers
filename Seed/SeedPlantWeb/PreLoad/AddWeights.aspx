<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AddWeights.aspx.cs" Inherits="PreLoad_AddWeights" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <script>
        function pageLoad() {
            CheckOK();
            //try {
            //    document.getElementById("txtInput").focus();
            //}
            //catch (err)
            //{ }
        }

                 function CheckOK() {
                try
                {
                    var OKbtn = document.getElementById('<%= btnManualSelect.ClientID %>');
                    var CurText = document.getElementById('<%= txtManualWeight.ClientID %>').value;
                    var n = CurText.length;
                
                    if (n >0 )
                    {
                        OKbtn.style.display = "block";
                    } else {
                        OKbtn.style.display = "none";
                    }
                }
                catch (err) {
                   
                }
            }

    </script>
    <h5 class="text-center" style="wid=100%"><asp:Label runat="server" ID="lblHeader" Text="Get Weight"></asp:Label></h5>
    <div class="form-row font-weight-bold   pt-2">
        <div class="col"></div>
        <div class="col-sm-8-col-md-6">
            <div class="col-12 pb-2">
                <asp:LinkButton runat="server" ID="btnCancel" CssClass=" btn btn-danger" Text="Cancel" PostBackUrl="~/PreLoad/TicketCreator.aspx" Width="100px"></asp:LinkButton>
            </div>
                 <div class="col-12  pb-2 ">
                    <label id="lblVehicleHeader" runat="server" for="txtVehicle">Vehicle</label>
                    <asp:TextBox runat="server" ID="txtVehicle"  AutoCompleteType="Disabled"  OnTextChanged="txtVehicle_TextChanged"  AutoPostBack="true" CssClass="form-control "></asp:TextBox>

                  </div>

            <div class="col-12">
                <asp:UpdatePanel runat="server" ID="UPScaleWeights" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Timer runat="server" ID="tmrUpdateScales" Interval="1000" Enabled="true"></asp:Timer>
                        <asp:GridView runat="server" Width="100%" ShowHeader="false" ShowFooter="false" AutoGenerateColumns="false" DataKeyNames="Description" CssClass="table table-bordered table-sm table-responsive-sm " ID="grdScales">

                            <Columns>

                                <asp:TemplateField>
                                    <ItemStyle CssClass="text-lg-right " Width="150px" />
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Bind("Description") %>' ID="lblScaleDescription"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemStyle Width="125px" />
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="txtWeight" Width="125px" CssClass='<%# Eval("RowCssClass")%>' Text='<%# Eval("Weight")%>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Width="80px" />
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Bind("Status") %>' ID="lblStatus"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemStyle Width="75px" />
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" Text="Select" CssClass="btn btn-secondary  " Visible='<%#!(bool)Eval("ReadOnly")%>' CausesValidation="false" OnClick="btnSelect_Click" ID="btnSelect"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <asp:UpdatePanel runat="server" ID="UPManualScale" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table runat="server" id="pnlManual" class="table-bordered text-center table-sm table-responsive-sm " style="width: 100%">

                            <tr class=" p-2 ">

                                <td class="text-lg-right p-2 " style="width: 150px">
                                    <label>Manual Entry</label>
                                </td>

                                <td class="p-2" style="width: 125px">
                                    <asp:TextBox ID="txtManualWeight" MaxLength="8" Width="125px"  runat="server" type="number" text="0"  onchange="CheckOK()" AutoCompleteType="Disabled"   CssClass="form-control text-right " onkeydown="return (event.keyCode!=13);" ></asp:TextBox>
                                </td>
                                <td style="width: 80px"></td>
                                <td class="p-2" style="width: 75px">
                                    <asp:LinkButton runat="server" Text="Select" CssClass="btn btn-secondary  " CausesValidation="false" OnClick="btnManualSelect_Click" ID="btnManualSelect"></asp:LinkButton>
                                </td>


                            </tr>
                        </table>


                    </ContentTemplate>

                </asp:UpdatePanel>
            </div>
        </div> 
        <div class="col"></div>
        </div>
                   
            

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentFull" Runat="Server">
</asp:Content>

