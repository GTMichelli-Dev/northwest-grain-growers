<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AddEditTreatment.aspx.cs" Inherits="AddEditTreatment" %>

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


    <h3>Treatment</h3>
    <asp:UpdatePanel runat="server" ID="up1">
        <ContentTemplate>

            <asp:HiddenField runat="server" ID="hfPLCTreatments" />
            <div class="row">


                
                    <div class="col-8  col-md-3">
                    <div class="input-group input-group-lg">
                        <asp:DropDownList runat="server" ID="ddPLCOnly" AutoPostBack="true" OnTextChanged="ddPLCOnly_TextChanged"  CssClass="form-control ">
                            <asp:ListItem Value="true">In PLC</asp:ListItem>
                            <asp:ListItem Value="">All</asp:ListItem>
                        </asp:DropDownList>

                    </div>



                </div>
                

         

            </div>
            <div class="row pt-2">
                <div class="col-12 col-md-2">
                    <asp:LinkButton runat="server" ID="btnNotFound" CssClass="btn btn-secondary" OnClick ="btnNotFound_Click"  Text="Not Found"></asp:LinkButton>
                </div>
            </div>
            <div class="row pt-2">
                <div class="col-sm-12 col-md-8 col-lg-6">
                    <div class="input-group input-group-lg">
                        <asp:HiddenField ID="hfFilter" runat="server" />
                        <asp:TextBox CssClass="form-control" placeholder="Treatment Name or Number" AutoCompleteType="Disabled" OnTextChanged="txtFilter_TextChanged" Font-Size="Larger" runat="server" ID="txtFilter" AutoPostBack="true"></asp:TextBox>
                        <div class="input-group-btn">
                            <button class="btn btn-default " type="submit"><i class=" fa fa-2x  fa-search "></i></button>
                        </div>
                    </div>




                </div>

            </div>




            <asp:Panel runat="server" ID="pnlRate">

            
            <div class=" row pt-2 ">
                <div class="col-10 col-sm-9 col-md-6 col-lg-5 col-xl-4 ">
                    <div class=" input-group  input-group-lg  ">
                        <asp:HiddenField runat="server" ID="hfDefaultRate" Value="0" />
                        <asp:TextBox runat="server" ID="txtRate" AutoPostBack="true" type="number" OnTextChanged="txtRate_TextChanged" MaxLength="5" onkeypress="return isNumberKey(event)" Text="0" AutoCompleteType="Disabled" CssClass="form-control text-right  "></asp:TextBox>


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
                                <asp:ListItem>2.70</asp:ListItem>
                                <asp:ListItem>2.75</asp:ListItem>
                                <asp:ListItem>3.00</asp:ListItem>
                                <asp:ListItem>3.25</asp:ListItem>
                                <asp:ListItem>3.50</asp:ListItem>
                                <asp:ListItem>3.75</asp:ListItem>
                                <asp:ListItem>4.00</asp:ListItem>
                                <asp:ListItem>3.25</asp:ListItem>
                                <asp:ListItem>4.50</asp:ListItem>
                                <asp:ListItem>4.75</asp:ListItem>
                                <asp:ListItem>5.00</asp:ListItem>
                                <asp:ListItem>5.25</asp:ListItem>
                                <asp:ListItem>5.50</asp:ListItem>
                                <asp:ListItem>5.75</asp:ListItem>
                                <asp:ListItem>6.00</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="input-group-append ">
                            <asp:Button runat="server" ID="btnSetDefault" OnClick="btnSetDefault_Click" Text="Set As Default" CssClass="input-group-text " />
                        </div>
                    </div>
                </div>
            </div>
                </asp:Panel>






                

            <div class="row pt-2 ">
                 <div class="col-sm-12 col-md-8 col-lg-6">

                    <asp:GridView runat="server" ID="grd1" Width="100%" CssClass="table table-hover" Font-Size="Larger" ShowHeader="False" AutoGenerateColumns="False" DataKeyNames="Item_UID,Item_Id,Description,FullName,DefaultValue" DataSourceID="sqlDataset">

                        <Columns>
                            <asp:TemplateField HeaderText="name" SortExpression="name">
                                <ItemTemplate>
                                    
                                        <asp:LinkButton runat="server" CssClass="text-dark"   Backcolor='<%# ((bool)Eval("InBin"))? System.Drawing.ColorTranslator.FromHtml("#CFFFE5"):System.Drawing.ColorTranslator.FromHtml("#FFFFFF") %>'     Width="100%"  Text='<%# Bind("FullName") %>' OnClick="lnkSelect_Click" ID="lnkSelect"></asp:LinkButton>
                                    
                                    

                                </ItemTemplate>

                            </asp:TemplateField>


                        </Columns>
                    </asp:GridView>
                     <asp:HiddenField runat="server" ID="hfExistingItems" />
                     <asp:SqlDataSource runat="server" ID="sqlDataset" CancelSelectOnNullParameter="False" ConnectionString='<%$ ConnectionStrings:Seed_DataConnectionString %>' SelectCommand="SELECT derivedtbl_1.Item_UID, derivedtbl_1.Item_Id, derivedtbl_1.Location_ID, derivedtbl_1.Description, derivedtbl_1.FullName, derivedtbl_1.InBin, CONVERT(decimal(4, 2), ISNULL(derivedtbl_1.DefaultValue, 0)) AS DefaultValue FROM (SELECT DISTINCT TOP (100) PERCENT derivedtbl_1_1.Item_UID, derivedtbl_1_1.Item_Id, derivedtbl_1_1.Location_ID, derivedtbl_1_1.Description, derivedtbl_1_1.DefaultValue, derivedtbl_1_1.FullName, CASE WHEN derivedtbl_2.Item_ID IS NULL THEN CONVERT(bit, 0) ELSE CONVERT(bit, 1) END AS InBin FROM (SELECT Item_UID, Item_Id, Location_ID, DefaultValue, Description, Description + ' - ' + LTRIM(STR(Item_Id)) AS FullName FROM Seed_Treatments) AS derivedtbl_1_1 LEFT OUTER JOIN (SELECT CONVERT(int, Name) AS Item_ID FROM dbo.splitstring(@PLCTreatmentID) AS splitstring_1) AS derivedtbl_2 ON derivedtbl_1_1.Item_Id = derivedtbl_2.Item_ID WHERE (derivedtbl_1_1.Location_ID = @Location) AND (derivedtbl_1_1.FullName LIKE N'%' + @Filter + N'%') ORDER BY derivedtbl_1_1.Description) AS derivedtbl_1 INNER JOIN Items ON derivedtbl_1.Item_Id = Items.ID LEFT OUTER JOIN (SELECT Name FROM dbo.splitstring(@ExistingItems) AS splitstring_2) AS derivedtbl_3 ON derivedtbl_1.Item_Id = derivedtbl_3.Name WHERE (derivedtbl_3.Name IS NULL) AND (derivedtbl_1.InBin = ISNULL(@PLcOnly, derivedtbl_1.InBin)) AND (Items.NotInUse = 0) AND (Items.Inactive = 0)">
                         <SelectParameters>
                             <asp:ControlParameter ControlID="hfPLCTreatments" Name="PLCTreatmentID" PropertyName="Value" />
                             <asp:CookieParameter CookieName="seedPlantLocation" Name="Location" />
                             <asp:ControlParameter ControlID="txtFilter" PropertyName="Text" Name="Filter"></asp:ControlParameter>
                             <asp:ControlParameter ControlID="hfExistingItems" PropertyName="Value" Name="ExistingItems"></asp:ControlParameter>

                             <asp:ControlParameter ControlID="ddPLCOnly" PropertyName="SelectedValue" Name="PLcOnly"></asp:ControlParameter>
                         </SelectParameters>
                    </asp:SqlDataSource>
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

