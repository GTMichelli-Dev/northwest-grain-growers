<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="TicketCreator.aspx.cs" Inherits="TicketCreator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

 <style>

     .blinkingdanger{
    animation:blinkingText .7s infinite;
}
@keyframes blinkingText{
    0%{     color:  #dc3545;    }
    49%{    color: #dc3545; }
    60%{    color: transparent; }
    99%{    color:transparent;  }
    100%{   color: #dc3545;    }
}


.dropdown-right{
    

    
    direction:rtl;
}


 </style>
 






        
    <br />

    <h5 class="text-center d-none d-sm-block  col-12 ">
        <asp:Label runat="server" ID="lblHeader"></asp:Label>
    </h5>

    <div class="form-row pb-2 ">
        <div class=" col-12   text-sm-center text-md-left ">
            <div class="btn-group-sm ">

                <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" Text="Save" Width="100" CssClass=" btn btn-sm btn-secondary " />
                <asp:Button runat="server" ID="btnComplete" OnClick="btnComplete_Click" Text="Complete" Width="100" CssClass="btn btn-sm btn-success " />
                <asp:Button runat="server" ID="btnCancel" PostBackUrl="~/Default.aspx" Text="Cancel" Width="100" CssClass="btn btn-sm btn-danger " />
                <asp:CheckBox ID="ckPending" OnCheckedChanged="ckPending_CheckedChanged" AutoPostBack="true" runat="server" CssClass="btn btn-sm" Text="Pending" />
                
                <asp:Button runat="server" ID="btnPrint" OnClick="btnPrint_Click" Text="Reprint" Width="100" CssClass="btn btn-sm btn-secondary  " />
                <asp:Button runat="server" ID="btnImage" OnClick="btnImage_Click" Text="Image" Width="100" CssClass="btn btn-sm btn-secondary  " />
                <asp:Button runat="server" ID="btnDone" PostBackUrl="~/Default.aspx" Text="Done" Width="100" CssClass="btn btn-sm btn-secondary  " />
                <div class="btn">
                    <asp:UpdatePanel runat="server" ID="pnlUpdatebtns">
                        <ContentTemplate>

                            <asp:Timer runat="server" ID="tmrUPdateButtons" Interval="3000" OnTick="tmrUPdateButtons_Tick" Enabled="true">
                            </asp:Timer>
                            <asp:Button runat="server" ID="btnSendToPLC" Visible="false" Text="Send To PLC" OnClick="btnSendToPLC_Click" Width="100" CssClass="btn btn-sm btn-secondary  " />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="btn">
                    <asp:DropDownList runat="server" ID="ddPrintDestination" AutoPostBack="true" OnTextChanged="ddPrintDestination_TextChanged" CssClass="form-control-sm ">
                        <asp:ListItem Value="Print To Printer"></asp:ListItem>
                        <asp:ListItem Value="Print To Browser"></asp:ListItem>
                    </asp:DropDownList>

                </div>
            </div>

        </div>
    </div>
    


    <asp:UpdatePanel runat="server" ID="UP1">
        <ContentTemplate>



            <div class="form-row  font-weight-bold">
                <div class="col-12 pr-0 pl-0">
                    <asp:LinkButton runat="server" ID="lnkGrower" Width="100%" CssClass="btn btn-light font-weight-bold  " Font-Size="Larger" PostBackUrl="~/PreLoad/SelectGrower.aspx"></asp:LinkButton>
                </div>

            </div>
            <div class=" form-row  d-block d-sm-none ">
                <div class="col-12 pt-2 pb-2 pr-0 pl-0">
                    <asp:LinkButton runat="server" ID="lnkTicketDetails" CssClass=" btn btn-secondary  btn-sm" Width="100%" PostBackUrl="~/PreLoad/LoadDetails.aspx" Text="Edit PO/BOL/Vehicle/Comments"></asp:LinkButton>
                </div>
            </div>
            <div class="d-none d-sm-block ">


                <asp:Panel runat="server" ID="pnlPoBolComment" class="form-row font-weight-bold ">

                    <div class="col-xs-12 col-md-4 col-lg-4  ">
                        <label for="txtPO">PO</label>
                        <asp:TextBox runat="server" ID="txtPO" AutoCompleteType="Disabled" OnTextChanged="txtPO_TextChanged" AutoPostBack="true" CssClass="form-control  input-sm "></asp:TextBox>
                    </div>
                    <div class="col-xs-12 col-md-4 col-lg-4  ">
                        <label for="txtBOL">BOL</label>
                        <asp:TextBox runat="server" ID="txtBOL" AutoCompleteType="Disabled" OnTextChanged="txtBOL_TextChanged" AutoPostBack="true" CssClass="form-control  input-sm"></asp:TextBox>

                    </div>
                    <div class="col-xs-12 col-md-4 col-lg-4  ">
                        <label for="txtVehicle">Vehicle</label>
                        <asp:TextBox runat="server" ID="txtVehicle" AutoCompleteType="Disabled" OnTextChanged="txtVehicle_TextChanged" AutoPostBack="true" CssClass="form-control  input-sm"></asp:TextBox>

                    </div>
                    <div class="col-12 ">
                        <label class="pb-0" for="txtComment">Comments</label>

                        <asp:TextBox runat="server" ID="txtComment" Width="100%" AutoCompleteType="Disabled" Rows="1" TextMode="MultiLine" OnTextChanged="txtComment_TextChanged" AutoPostBack="true" CssClass="form-control  input-sm"></asp:TextBox>

                    </div>

                </asp:Panel>

            </div>





            <hr />


            <asp:Panel runat="server" ID="pnlTicketDetails" Width="100% " HorizontalAlign="Center">


                <div class="form-row pb-1 ">

                    <div class="col-12 text-center ">
                        <div class="input-group ">
                            <h5 class="text-left " style="width: 120px">Weights</h4>

                        <div>
                            <asp:LinkButton runat="server" ID="btnWeight" Width="120px" CssClass=" btn btn-secondary btn-sm " OnClick="btnWeight_Click" Text="Get Weight" />

                        </div>
                                <div class="pl-2">
                                    <asp:DropDownList runat="server" CssClass="form-control-sm " ID="ddWeightype" OnTextChanged="ddWeightype_TextChanged" AutoPostBack="true">
                                    </asp:DropDownList>

                                </div>

                                <div class="pl-2 pt-2">
                                    <h6>
                                        <asp:Label runat="server" ID="lblToteTotals"></asp:Label></h6>
                                </div>
                        </div>
                    </div>



                </div>
                <div class="form-row">
                    <asp:Panel runat="server" ID="pnlTruckWeights" HorizontalAlign="Center" ScrollBars="Auto" Width="100%">
                        <div class="col col-lg-6 p-0">
                            <asp:Table runat="server" HorizontalAlign="Center" ID="tblTruckWeights" CssClass="table table-bordered table-sm table-hover table-striped">
                                <asp:TableHeaderRow>
                                    <asp:TableHeaderCell Text="Time In"></asp:TableHeaderCell>
                                    <asp:TableHeaderCell Text="In Weight"></asp:TableHeaderCell>
                                    <asp:TableHeaderCell Text="Time Out"></asp:TableHeaderCell>
                                    <asp:TableHeaderCell Text="Weight Out"></asp:TableHeaderCell>
                                </asp:TableHeaderRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label runat="server" CssClass="font-weight-bold " ID="lblTruckTimeIn"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:LinkButton CssClass="font-weight-bold " runat="server" ID="lblTruckWeightIn" OnClick="lblTruckWeightIn_Click"></asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="lnkSetTruckWeightIn" CssClass="btn btn-sm btn-secondary" OnClick="lnkSetTruckWeightIn_Click" Text="Set Weight"></asp:LinkButton>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label runat="server" CssClass="font-weight-bold " ID="lblTruckTimeOut"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:LinkButton runat="server" CssClass="font-weight-bold " ID="lblTruckWeightOut" OnClick="lblTruckWeightOut_Click"></asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="lnkSetTruckOutWeight" CssClass="btn btn-sm btn-secondary " OnClick="lnkSetTruckOutWeight_Click" Text="Set Weight"></asp:LinkButton>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>


                            <asp:Table runat="server" ID="tblTruckGTN" CssClass="table table-bordered table-sm table-hover table-striped ">
                                <asp:TableHeaderRow>
                                    <asp:TableHeaderCell Text="Gross"></asp:TableHeaderCell>
                                    <asp:TableHeaderCell Text="Tare"></asp:TableHeaderCell>
                                    <asp:TableHeaderCell Text="Net"></asp:TableHeaderCell>
                                    <asp:TableHeaderCell Text="Bushels"></asp:TableHeaderCell>
                                </asp:TableHeaderRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label runat="server" ID="lblGross"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label runat="server" ID="lblTare"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label CssClass="font-weight-bold " runat="server" ID="lblNet"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label CssClass="font-weight-bold " runat="server" ID="lblBushels"></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </div>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlToteWeights" HorizontalAlign="Center" Width="100%">
                        <div class="col col-lg-6 p-0">
                            <asp:GridView HorizontalAlign="Center" Width="100%" CssClass="table table-bordered table-sm table-hover table-striped  " runat="server" ID="grdTotes" DataKeyNames="UID" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:TemplateField HeaderText="Start" SortExpression="Starting_Weight">
                                        <ItemStyle CssClass="text-right " Width="125px" />
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" CssClass="font-weight-bold " Text='<%# Bind("Starting_Weight") %>' ID="lbToteStartWeight" OnClick="lbToteStartWeight_Click"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="End" SortExpression="Ending_Weight">
                                        <ItemStyle CssClass="text-right " Width="125px" />
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lnkSetEndTote" Width="100%" CssClass="btn btn-sm btn-secondary " Text="Set Weight" Visible='<%# (Eval("Ending_Weight").ToString()=="Not Set") %>' OnClick="lnkSetEndTote_Click"></asp:LinkButton>
                                            <asp:LinkButton runat="server" CssClass="font-weight-bold" Text='<%# Bind("Ending_Weight") %>' Visible='<%# (Eval("Ending_Weight").ToString()!="Not Set") %>' ID="lbToteEndWeight" OnClick="lbToteEndWeight_Click"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Net" SortExpression="Net">
                                        <ItemStyle CssClass="text-right " Width="125px" />
                                        <ItemTemplate>
                                            <asp:Label runat="server" CssClass="font-weight-bold " Text='<%# Bind("Net") %>' ID="lblToteNetWeight"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" Text="Delete" CommandName="" CssClass=" text-danger font-weight-bold  " CausesValidation="false" ID="lbToteDelete" OnClick="lbToteDelete_Click"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>

                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlBagWeights" Width="100%" HorizontalAlign="Center">
                        <div class="col col-lg-6 p-0">
                            <asp:Table runat="server" ID="tblBags" Width="100%" HorizontalAlign="Center" CssClass="table table-bordered table-sm table-hover table-striped  ">

                                <asp:TableHeaderRow>
                                    <asp:TableHeaderCell Width="25%">Bag Count</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="25%">Bag Size</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="50%">Total Pounds</asp:TableHeaderCell>
                                </asp:TableHeaderRow>
                                <asp:TableRow>
                                    <asp:TableCell CssClass="text-right ">
                                        <asp:TextBox runat="server" ID="txtBagCount" CssClass="form-control text-right font-weight-bold  " AutoPostBack="true" type="number" Text="1" OnTextChanged="txtBagCount_TextChanged" onkeypress="return isNumberKey(event)" MaxLength="3"></asp:TextBox>
                                    </asp:TableCell>
                                    <asp:TableCell CssClass="text-right">
                                        <asp:DropDownList runat="server" ID="ddBagSize" CssClass="form-control text-right  font-weight-bold dropdown-right  " AutoPostBack="true" OnTextChanged="ddBagSize_TextChanged">
                                            <asp:ListItem>60</asp:ListItem>
                                            <asp:ListItem>7</asp:ListItem>
                                            <asp:ListItem>15</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                            <asp:ListItem>55</asp:ListItem>
                                            <asp:ListItem>100</asp:ListItem>
                                        </asp:DropDownList>
                                    </asp:TableCell>
                                    <asp:TableCell CssClass="text-center ">
                                        <asp:Label runat="server" ID="lblBagTotal" CssClass="form-control  font-weight-bold " Text="0"> </asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>

                            </asp:Table>
                        </div>
                    </asp:Panel>
                </div>
                <hr />





                <div class="form-row pb-1 ">

                    <div class="col-12 text-center ">
                        <div class="input-group ">
                            <h5 class=" text-left " style="width: 120px">Varieties</h4>

                        <div>
                            <asp:LinkButton runat="server" ID="btnVarieties" Width="120px" CssClass=" btn btn-secondary  btn-sm " OnClick="btnAddVariety_Click" Text="Add Variety" />
                        </div>

                                <h6 class="pl-4 pt-2  ">
                                    <asp:Label runat="server" ID="lblVarietyTotal"></asp:Label>

                                </h6>
                        </div>
                    </div>


                </div>
                <div class="form-row">
                    <asp:Panel runat="server" ID="pnlVarieties" HorizontalAlign="Center" ScrollBars="Auto" Width="100%">
                        <asp:GridView runat="server" CssClass="table table-bordered table-sm table-hover table-striped " Font-Bold="true" DataKeyNames="UID" AutoGenerateColumns="false" ID="grdVarieties" HorizontalAlign="Center" Width="100%">
                            <Columns>

                                <asp:TemplateField HeaderText="Item">
                                    <ItemTemplate>


                                        <asp:LinkButton OnClick="lnkVarietyID_Click" runat="server" Text='<%# ((int)Eval("Item")>0 )?Eval("Item").ToString():"????" %>' ID="lnkVarietyID"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>




                                <asp:BoundField DataField="Description" HeaderText="Description" ReadOnly="True" SortExpression="Description">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>

                                <asp:BoundField DataField="Bin" HeaderText="Bin" ReadOnly="True" SortExpression="Bin">
                                    <HeaderStyle CssClass=" d-none d-sm-block "></HeaderStyle>
                                    <ItemStyle CssClass=" d-none d-sm-block" />
                                </asp:BoundField>

                                <asp:BoundField DataField="Percent" HeaderText="%" ReadOnly="True" SortExpression="Percent" DataFormatString="{0:N2}">

                                    <ItemStyle HorizontalAlign="Right" />

                                </asp:BoundField>

                                <asp:BoundField DataField="Total" ControlStyle-CssClass="text-right text-sm-right text-lg-left " HeaderText="Weight" ReadOnly="True" SortExpression="Total" DataFormatString="{0:N0}">
                                    <HeaderStyle HorizontalAlign="Right" CssClass=" d-none d-sm-block "></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Right" CssClass=" d-none d-sm-block" />
                                </asp:BoundField>


                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnkDeleteVariety" OnClick="lnkDeleteVariety_Click" Text="Delete"></asp:LinkButton>
                                    </ItemTemplate>

                                    <ControlStyle CssClass="text-danger"></ControlStyle>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </div>
                <hr />
                <div class="form-row pb-1 ">
                    <div class="col-12 text-center ">
                        <div class="input-group ">
                            <h5 class="  text-left " style="width: 120px">Treatments</h5>
                            <asp:LinkButton runat="server" ID="btnTreatments" Width="120px" CssClass=" btn btn-secondary  btn-sm " OnClick="btnAddTreatment_Click" Text="Add Treatment" />
                        </div>

                    </div>
                </div>
                <div class="form-row">
                    <asp:Panel runat="server" ID="pnlTreatments" HorizontalAlign="Center" ScrollBars="Auto" Width="100%">
                        <asp:GridView runat="server" CssClass="table table-bordered table-sm table-hover table-striped  " Font-Bold="true" DataKeyNames="UID" AutoGenerateColumns="false" ID="grdTreatments" HorizontalAlign="Center" Width="100%">
                            <Columns>

                                <asp:TemplateField HeaderText="Item">
                                    <ItemTemplate>


                                        <asp:LinkButton OnClick="lnkTreatmentID_Click" runat="server" Text='<%# ((int)Eval("Item")>0 )?Eval("Item").ToString():"????" %>' ID="lnkTreatmentID"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="Description" HeaderText="Description" ReadOnly="True" SortExpression="Description">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Rate" HeaderText="Rate" ReadOnly="True" SortExpression="Rate" DataFormatString="{0:N2}">

                                    <ItemStyle HorizontalAlign="Right" />

                                </asp:BoundField>
                                <asp:BoundField DataField="Total" ControlStyle-CssClass="text-right text-sm-right text-lg-left " HeaderText="Total" ReadOnly="True" SortExpression="Total" DataFormatString="{0:N0}">
                                    <HeaderStyle HorizontalAlign="Right" CssClass=" d-none d-sm-block "></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Right" CssClass=" d-none d-sm-block" />
                                </asp:BoundField>
                                <%--   <asp:TemplateField HeaderText="Bin" SortExpression="Bin">

                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Bind("Bin") %>' ID="Label1"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnkDeleteTreatment" OnClick="lnkDeleteTreatment_Click" Text="Delete"></asp:LinkButton>
                                    </ItemTemplate>

                                    <ControlStyle CssClass="text-danger"></ControlStyle>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </div>
            </asp:Panel>



            <hr />

            <div class="form-row pb-1 ">
                <div class="col-12 text-center ">
                    <div class="input-group ">
                        <h5 class="  text-left " style="width: 120px">Misc</h5>
                        <asp:LinkButton runat="server" ID="btnMisc" Width="120px" CssClass=" btn btn-secondary  btn-sm " PostBackUrl="~/PreLoad/AddEditMisc.aspx" Text="Add Misc" />
                    </div>

                </div>
            </div>
            <div class="form-row">
                <div class="col-12 col-md-6 ">
                    <asp:Panel runat="server" ID="pnlMisc" HorizontalAlign="Center" ScrollBars="Auto" Width="100%">
                        <asp:GridView runat="server" CssClass="table table-bordered table-sm table-hover table-striped  " Font-Bold="true" DataKeyNames="UID" AutoGenerateColumns="false" ID="grdMisc" HorizontalAlign="Center" Width="100%">
                            <Columns>

                                <asp:TemplateField HeaderText="Item">
                                    <ItemStyle />
                                    <ItemTemplate>


                                        <asp:Label runat="server" Text='<%# Bind("Item_Id") %>' ID="lnkMiscID"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="Description" HeaderText="Description" ReadOnly="True" SortExpression="Description">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Quantity">
                                    <ItemStyle Width="100px" HorizontalAlign="Right" />
                                    <ItemTemplate>


                                        <asp:TextBox OnTextChanged="lnkQuantityID_TextChanged" type="number" runat="server" CssClass="form-control text-right " Text='<%# Bind("Quantity") %>' ID="lnkQuantityID"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnkDeleteMisc" OnClick="lnkDeleteMisc_Click" Text="Delete"></asp:LinkButton>
                                    </ItemTemplate>

                                    <ControlStyle CssClass="text-danger"></ControlStyle>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentFull" Runat="Server">
</asp:Content>

