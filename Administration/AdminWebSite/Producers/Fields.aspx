<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Fields.aspx.cs" Inherits="Producers_Fields" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <script>

        $(function () {

            $(window).keydown(function (event) {
                if (event.keyCode == 13) {
                    event.preventDefault();
                    return false;
                }
            });
            
        });
        
            
    </script>

         <asp:UpdatePanel runat="server" ClientIDMode="Static"  ID="UP1">
             <ContentTemplate>


                 <div class="text-center ">
                     <h3>
                         <asp:Label ID="lblHeader"  runat="server" Text="Fields For ??"></asp:Label></h3>

                 </div>
                 <div class="form-row">
                     <div class="col-2 offset-5 ">

                         <asp:Button runat="server" ID="btnAddNew" OnClick="btnAddNew_Click" CssClass=" btn btn-outline-dark  " Width="100%" Text="Add Field" />
                     </div>
                 </div>
                 <hr />


                 <asp:Panel runat="server" BorderWidth="2px" CssClass="px-2 " ID="pnlNew" Visible="false" HorizontalAlign="Center">
                     <asp:Label runat="server" Font-Bold="true"   ID="lblNewHeader">Add New Field</asp:Label>
                     <div class=" form-row">
                         <div class="col-9 form-row ">

                             <div class="col form-group ">
                                 <label for="txtField">Field</label>
                                 <asp:TextBox runat="server" AutoCompleteType="Disabled" OnTextChanged="txtField_TextChanged" ID="txtField" CssClass="form-control "></asp:TextBox>
                             </div>
                             <div class="col form-group ">
                                 <label for="ddCrop">Commodity</label>
                                 <asp:DropDownList runat="server" ID="ddCrop" CssClass="form-control " AutoPostBack="true" DataSourceID="SqlCrop" DataTextField="Description" DataValueField="Crop_Prefix"></asp:DropDownList>
                                 <asp:SqlDataSource runat="server" ID="SqlCrop" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT Description, Crop_Prefix FROM CropList ORDER BY Description"></asp:SqlDataSource>
                             </div>
                             <div class="col form-group ">
                                 <label for="ddVariety">Variety</label>
                                 <asp:DropDownList runat="server" ID="ddVariety" OnSelectedIndexChanged="ddVariety_SelectedIndexChanged" CssClass="form-control " OnTextChanged="ddVariety_TextChanged" AutoPostBack="true" DataSourceID="SqlVariety" DataTextField="Variety" DataValueField="Item_Id"></asp:DropDownList>
                                 <asp:SqlDataSource runat="server" ID="SqlVariety" CancelSelectOnNullParameter="false" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT DISTINCT Variety, Item_Id FROM VarietyList WHERE (Crop_Prefix = @Crop_Prefix) ORDER BY Variety">
                                     <SelectParameters>
                                         <asp:ControlParameter ControlID="ddCrop" PropertyName="SelectedValue" Name="Crop_Prefix"></asp:ControlParameter>
                                     </SelectParameters>
                                 </asp:SqlDataSource>
                             </div>
                         </div>
                         <div class="col-3 form-row">
                             <div class="col form-group mt-2 ">
                                 <asp:Button runat="server" Text="Cancel" Width="100%" OnClick="btnCancel_Click" ID="btnCancel" CssClass=" mt-4 btn btn-outline-danger  " />
                             </div>
                             <div class="col form-group mt-2">

                                 <asp:Button runat="server" ID="btnCreateNew" OnClick="btnCreateNew_Click" Text="Save" Width="100%" CssClass="mt-4  btn  btn-outline-success  " />

                             </div>
                         </div>
                     </div>
                 </asp:Panel>
                 <asp:Panel runat="server" ID="pnlFields" HorizontalAlign="Center" Width="100%">
                     <asp:GridView runat="server" ID="grdFields" HorizontalAlign="Center" Width="100%" CssClass="table table-hover " AllowSorting="true"   AutoGenerateColumns="False" DataKeyNames="Field" DataSourceID="SqlProducerFields">

                         <Columns>
                            
                             <asp:BoundField DataField="Field"  ItemStyle-CssClass="text-left " HeaderText="Field" SortExpression="Field"></asp:BoundField>
                             <asp:BoundField DataField="Variety_Id"   ItemStyle-CssClass="text-left "  HeaderText="Id" SortExpression="Variety_Id"></asp:BoundField>
                             <asp:BoundField DataField="Crop"   ItemStyle-CssClass="text-left "  HeaderText="Crop" SortExpression="Crop"></asp:BoundField>
                             <asp:BoundField DataField="Description"   ItemStyle-CssClass="text-left "  HeaderText="Description" SortExpression="Description"></asp:BoundField>
                             <asp:BoundField DataField="Class"   ItemStyle-CssClass="text-left "  HeaderText="Class" SortExpression="Class"></asp:BoundField>
                              <asp:TemplateField >
                                 <ItemTemplate>
                                     <asp:LinkButton runat="server" Id="lnkEdit"  CssClass="btn btn-outline-dark " OnClick="lnkEdit_Click"   Text="Edit"></asp:LinkButton> 

                                 </ItemTemplate>
                             </asp:TemplateField>

                             <asp:TemplateField >
                                 <ItemTemplate>
                                     <asp:LinkButton runat="server" Id="lnkDelete"  CssClass="btn btn-outline-danger " OnClick="lnkDelete_Click"  Text="Delete"></asp:LinkButton> 

                                 </ItemTemplate>
                             </asp:TemplateField>
                         </Columns>
                     </asp:GridView>
                     <asp:SqlDataSource runat="server" ID="SqlProducerFields" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT ProducerVarietyField.UID, ProducerVarietyField.Producer_Id, ProducerVarietyField.Variety_Id, ProducerVarietyField.Field, Crop_Varieties.Crop, Crop_Varieties.Description, Crop_Varieties.Class FROM ProducerVarietyField INNER JOIN Crop_Varieties ON ProducerVarietyField.Variety_Id = Crop_Varieties.Item_Id WHERE (ProducerVarietyField.Producer_Id = @Producer_Id) ORDER BY ProducerVarietyField.Field, Crop_Varieties.Crop, Crop_Varieties.Description, Crop_Varieties.Class">
                         <SelectParameters>
                             <asp:QueryStringParameter QueryStringField="ProducerId" Name="Producer_Id"></asp:QueryStringParameter>
                         </SelectParameters>
                     </asp:SqlDataSource>
                 </asp:Panel>



               <asp:HiddenField runat="server" ID="hfEditUID" />

             </ContentTemplate>
    </asp:UpdatePanel>
                   

                    <!--Confirm Prompt -->
        <div class="modal hide  fade" id="confirmPrompt" role="dialog">
            <div class="modal-dialog">

                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 id="ConfirmTitle" class="modal-title ">Delete Field</h4>
                           
                     
                        <button type="button" class="close" data-dismiss="modal">&times;</button>

                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel runat="server" ID="UP2">
                            <ContentTemplate>
                                <asp:HiddenField runat="server" ID="hfSelectedField" />
                                <asp:Label runat="server" ID="lblDeleteHeader" CssClass="text-danger font-weight-bold "></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                       
                        
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                                <asp:Button runat="server" OnClick="btnConfirmDelete_Click"  ID="btnConfirmDelete" CssClass="btn btn-success " Text="Yes Delete Field" />
                        
                        
                    </div>
                </div>

            </div>
         </div>



</asp:Content>

