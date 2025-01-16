<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Test.aspx.cs" Inherits="Test" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
 


    <script>
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if ((charCode > 31 && (charCode < 48 || charCode > 57)) && (charCode != 46))
                return false;
            return true;
        }
    </script>
  


    <asp:UpdatePanel runat="server" ID="upWeight">
        <ContentTemplate>
            <asp:Label runat="server" ID="lblWeight" CssClass="h2" > </asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    

    <asp:UpdatePanel runat="server" ID="UPbtn">
        <ContentTemplate>
             <asp:Button runat="server" ID="btnTest" UseSubmitBehavior="false" Text="Set Weight" OnClick="btnTest_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>






    <asp:UpdatePanel runat="server" ID="UPScaleTimer">
        <ContentTemplate>
                     <asp:Timer ID="tmrScaleUpdate" runat="server" Enabled="false"  Interval="100" OnTick="tmrScaleUpdate_Tick">
           </asp:Timer>
      
        </ContentTemplate>
    </asp:UpdatePanel>

   
  
  

     <!-- Modal -->
    <asp:Label ID="Label2" runat="server" Text=""></asp:Label><br />
    <div class="modal fade" id="popScales" tabindex="-1" role="dialog" aria-labelledby="pnpScalesLabel"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                     <h4 class="modal-title" id="pnpScalesLabel">Scale Weight</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span></button>
                   
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" ID="UPScaleWeights"  UpdateMode="Conditional"  >
                        <ContentTemplate>

                            <asp:GridView runat="server" HorizontalAlign="Center" ShowHeader="false" ShowFooter="false"  AutoGenerateColumns="false" DataKeyNames="Description"  CssClass="table table-bordered" ID="grdScales">
                                    
                                     <Columns>
                                        
                                    <asp:TemplateField>
                                        <ItemStyle />
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("Description") %>' ID="lblScaleDescription"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <ItemStyle Width="125px"  />
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtWeight"  CssClass='<%# Eval("RowCssClass")%>'  Text='<%# Eval("Weight")%>'   ></asp:Label> 
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle  Width="80px" />
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("Status") %>' ID="lblStatus"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemStyle  Width="75px" />
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" Text="Select" CssClass="btn btn-info " Visible='<%#!(bool)Eval("ReadOnly")%>'  CausesValidation="false" OnClick="btnSelect_Click"   ID="btnSelect"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>
                          
                        </ContentTemplate>
                        
                    </asp:UpdatePanel>
                    <asp:UpdatePanel runat="server" ID="UPManualScale" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="table-bordered ">

                                <div class="form-row">
                                   
                                    <div class="col-5 text-lg-right ">
                                        <label>Manual Entry</label>
                                    </div>

                                    <div class="col-3" >
                                        <asp:TextBox ID="txtManualWeight" MaxLength="9" runat="server"  CssClass="form-control text-right " onkeydown = "return (event.keyCode!=13);" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                    </div>

                                    <div class="col-2">
                                        <asp:LinkButton runat="server" Text="Select" CssClass="btn btn-info " CausesValidation="false" OnClick="btnManualSelect_Click" ID="btnManualSelect"></asp:LinkButton>
                                    </div>
                                    

                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
               
                   
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" Text="Cancel" CssClass="btn btn-danger " id="btnCancelScaleWeight" OnClick="btnCancelScaleWeight_Click" />
                    
                </div>
            </div>
        </div>
    </div>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentFull" Runat="Server">
</asp:Content>

