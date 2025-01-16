<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Producers.aspx.cs" Inherits="Producers_Producers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            Getting Data..
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="text-center ">
                <h3>
                    <asp:Label ID="lblHeader" runat="server" Text="Producers"></asp:Label></h3>
             
                <div class="container">
                   
                    <div class="form-group col-md-4 text-left ">
                        <label for="txtFilter">Search</label>
                        <asp:TextBox runat="server" ID="txtFilter" AutoPostBack="true" CssClass="form-control " ></asp:TextBox>
                    </div>
                        

                   
                    
                    <asp:GridView ID="GridView1" runat="server" CssClass="table table-hover " HorizontalAlign="Center"  AutoGenerateColumns="False" DataKeyNames="UID" DataSourceID="SqlProducers" AllowPaging="True" AllowSorting="True" PageSize="100">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>

                                    <asp:LinkButton ID="lnkSelect"  runat="server" OnClick="lnkSelect_Click"  CssClass="btn btn-primary " >Select</asp:LinkButton>

                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Id" HeaderText="Agvantage Id" ItemStyle-HorizontalAlign="Right"  SortExpression="Id" >
                            <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-HorizontalAlign="Left"  SortExpression="Description" >
                            <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Email_Address" HeaderText="Email Address" ItemStyle-HorizontalAlign="Left" SortExpression="Email_Address" >
                            <ItemStyle HorizontalAlign="Left"  />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Email Ws">
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="EmailWS" OnCheckedChanged="EmailWS_CheckedChanged" AutoPostBack="true"  Checked='<%# Bind("Email_WS") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lnkFields" CssClass="btn btn-secondary " Text="Fields" PostBackUrl='<%#"~/Producers/Fields.aspx?Producer="+ Eval("Description")+"&ProducerId="+Eval("Id") %>' ></asp:LinkButton>
                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                          <%--  <asp:TemplateField HeaderText="Print Ws">
                               <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="PrintWS" OnCheckedChanged="PrintWS_CheckedChanged" AutoPostBack="true"  Checked='<%# Bind("Print_WS") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:CheckBoxField DataField="Active" HeaderText="Active" SortExpression="Active" />
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlProducers" runat="server" ConnectionString="<%$ ConnectionStrings:NW_DataConnectionString %>" SelectCommand="SELECT UID, Id, Description, Email_Address, Email_WS, Print_WS, Active FROM Producers WHERE (Description LIKE '%' + ISNULL(@Filter, Description) + '%') AND (Active = ISNULL(@Active, Active)) AND (Id <> 0 AND Id <> 999999) OR (Active = ISNULL(@Active, Active)) AND (Id LIKE '%' + ISNULL(@Filter, Id) + '%') AND (Id <> 0 AND Id <> 999999) ORDER BY Active DESC, Description" CancelSelectOnNullParameter="False">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="txtFilter" Name="Filter" PropertyName="Text" />
                            <asp:Parameter Name="Active" />
                        </SelectParameters>
                    </asp:SqlDataSource>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

