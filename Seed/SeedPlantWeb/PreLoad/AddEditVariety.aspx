<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AddEditVariety.aspx.cs" Inherits="AddEditVariety" %>

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


    <h3>Variety</h3>
    <asp:UpdatePanel runat="server" ID="up1">
        <ContentTemplate>

            <asp:HiddenField runat="server" ID="hfBins" />
            <div class="row">


                
                    <div class="col-8  col-md-3">
                    <div class="input-group input-group-lg">
                        <asp:DropDownList runat="server" ID="ddBinsOnly" AutoPostBack="true" OnTextChanged="ddBinsOnly_TextChanged"  CssClass="form-control ">
                            <asp:ListItem Value="true">In Bins</asp:ListItem>
                            <asp:ListItem Value="">All</asp:ListItem>
                        </asp:DropDownList>

                    </div>



                </div>
                

                <div class="col-12  col-md-4">
                    <div class="input-group input-group-lg">
                        <asp:DropDownList runat="server" ID="ddClass" AutoPostBack="true" OnTextChanged="ddClass_TextChanged" CssClass="form-control " DataSourceID="sqlItemClass" DataTextField="Text" DataValueField="Value">
                        </asp:DropDownList>

                        <asp:SqlDataSource runat="server" ID="sqlItemClass" ConnectionString='<%$ ConnectionStrings:Seed_DataConnectionString %>' SelectCommand="SELECT 'All Classes' AS Text, NULL AS Value, 0 AS idx UNION SELECT Description AS Text, Description AS Value, 1 AS Idx FROM Classes ORDER BY idx, Text"></asp:SqlDataSource>
                    </div>



                </div>
                <div class=" col-12  col-md-5">
                    <div class="input-group input-group-lg">
                        <asp:DropDownList runat="server" ID="ddCommodity" AutoPostBack="true" CssClass="form-control " OnTextChanged="ddCommodity_TextChanged"  DataSourceID="SqlCommodity" DataTextField="text" DataValueField="value">
                        </asp:DropDownList>

                        <asp:SqlDataSource runat="server" ID="SqlCommodity" ConnectionString='<%$ ConnectionStrings:Seed_DataConnectionString %>' SelectCommand="SELECT 'All Commodities' AS text, NULL AS value, 0 AS idx UNION SELECT Description AS text, Description AS text, 1 AS idx FROM Seed_Departments WHERE (Not_Used = 0) ORDER BY idx, text"></asp:SqlDataSource>
                    </div>



                </div>

            </div>

            <div class="row pt-2">
                <div class="col-12 col-md-2">
                    <asp:LinkButton runat="server" ID="btnNotFound" CssClass="btn btn-secondary" OnClick="btnNotFound_Click" Text="Not Found"></asp:LinkButton>
                </div>
            </div>

            <div class="row pt-3">
                <div class="col-sm-12 col-md-8 col-lg-6">
                    <div class="input-group input-group-lg">
                        <asp:HiddenField ID="hfFilter" runat="server" />
                        <asp:TextBox CssClass="form-control" placeholder="Variety Name or Number" AutoCompleteType="Disabled" OnTextChanged="txtFilter_TextChanged" Font-Size="Larger" runat="server" ID="txtFilter" AutoPostBack="true"></asp:TextBox>
                        <div class="input-group-btn">
                            <button class="btn btn-default " type="submit"><i class=" fa fa-2x  fa-search "></i></button>
                        </div>

                    </div>




                </div>

            </div>


          

            <div class=" row pt-2 " runat="server" id="pnlPrecent">
                <div class="col-6 col-md-6 col-lg-4">
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







                

            <div class="row pt-2 ">
                 <div class="col-sm-12 col-md-8 col-lg-6">

                    <asp:GridView runat="server" ID="grd1" Width="100%" CssClass="table table-hover" Font-Size="Larger" ShowHeader="False" AutoGenerateColumns="False" DataKeyNames="Item_UID,Item_Id,Description,FullName" DataSourceID="sqlDataset">

                        <Columns>
                            <asp:TemplateField HeaderText="name" SortExpression="name">
                                <ItemTemplate>
                                    
                                        <asp:LinkButton runat="server" CssClass="text-dark"   Backcolor='<%# ((bool)Eval("InBin"))? System.Drawing.ColorTranslator.FromHtml("#CFFFE5"):System.Drawing.ColorTranslator.FromHtml("#FFFFFF") %>'     Width="100%"  Text='<%# Bind("FullName") %>' OnClick="lnkSelect_Click" ID="lnkSelect"></asp:LinkButton>
                                    
                                    

                                </ItemTemplate>

                            </asp:TemplateField>


                        </Columns>
                    </asp:GridView>
                     <asp:HiddenField runat="server" ID="hfExistingItems" />
                     <asp:SqlDataSource runat="server" ID="sqlDataset" CancelSelectOnNullParameter="False" ConnectionString='<%$ ConnectionStrings:Seed_DataConnectionString %>' SelectCommand="SELECT derivedtbl_1.Item_UID, derivedtbl_1.Item_Id, derivedtbl_1.Location_ID, derivedtbl_1.Item_Class, derivedtbl_1.Item_Department, derivedtbl_1.Description, derivedtbl_1.FullName, derivedtbl_1.InBin FROM (SELECT DISTINCT TOP (100) PERCENT derivedtbl_1_1.Item_UID, derivedtbl_1_1.Item_Id, derivedtbl_1_1.Location_ID, derivedtbl_1_1.Item_Class, derivedtbl_1_1.Item_Department, derivedtbl_1_1.Description, derivedtbl_1_1.FullName, CASE WHEN derivedtbl_2.Item_ID IS NULL THEN CONVERT (bit , 0) ELSE CONVERT (bit , 1) END AS InBin FROM (SELECT Item_UID, Item_Id, Location_ID, Item_Class, Item_Department, Description, Description + ' - ' + LTRIM(STR(Item_Id)) AS FullName FROM Seed_Varieties) AS derivedtbl_1_1 LEFT OUTER JOIN (SELECT CONVERT (int, Name) AS Item_ID FROM dbo.splitstring(@BinVarieryID) AS splitstring_1) AS derivedtbl_2 ON derivedtbl_1_1.Item_Id = derivedtbl_2.Item_ID WHERE (derivedtbl_1_1.Location_ID = @Location) AND (derivedtbl_1_1.Item_Class = ISNULL(@Item_Class, derivedtbl_1_1.Item_Class)) AND (derivedtbl_1_1.Item_Department = ISNULL(@Item_Department, derivedtbl_1_1.Item_Department)) AND (derivedtbl_1_1.FullName LIKE N'%' + @Filter + N'%') ORDER BY derivedtbl_1_1.Description) AS derivedtbl_1 LEFT OUTER JOIN (SELECT Name FROM dbo.splitstring(@ExistingItems) AS splitstring_2) AS derivedtbl_3 ON derivedtbl_1.Item_Id = derivedtbl_3.Name WHERE (derivedtbl_1.InBin = ISNULL(@BinsOnly, derivedtbl_1.InBin)) AND (derivedtbl_3.Name IS NULL)">
                         <SelectParameters>
                             <asp:ControlParameter ControlID="hfBins" Name="BinVarieryID" PropertyName="Value" />
                             <asp:CookieParameter CookieName="seedPlantLocation" Name="Location" />
                             <asp:ControlParameter ControlID="ddClass" PropertyName="SelectedValue" Name="Item_Class"></asp:ControlParameter>
                             <asp:ControlParameter ControlID="ddCommodity" PropertyName="SelectedValue" Name="Item_Department"></asp:ControlParameter>

                             <asp:ControlParameter ControlID="txtFilter" PropertyName="Text" Name="Filter"></asp:ControlParameter>
                             <asp:ControlParameter ControlID="hfExistingItems" PropertyName="Value" Name="ExistingItems"></asp:ControlParameter>

                             <asp:ControlParameter ControlID="ddBinsOnly" PropertyName="SelectedValue" Name="BinsOnly"></asp:ControlParameter>
                         </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </div>
            <asp:Panel runat="server" ID="pnlUseBins">
               <div class="row pt-3">
                    <div class="col-12 col-md-3 ">
                        <asp:Label runat="server" ID="lblNotInBin" CssClass=" text-danger font-weight-bold" Font-Size="X-Large"   Text="Not In Bin"></asp:Label>
                        <div class="input-group input-group-lg">
                            <asp:DropDownList CssClass=" form-control " AutoPostBack="true"  runat="server" ID="ddBins"></asp:DropDownList>
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

