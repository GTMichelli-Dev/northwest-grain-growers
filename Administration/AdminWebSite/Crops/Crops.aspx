<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Crops.aspx.cs" Inherits="Crops_Crops" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            Getting Data..
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="text-center ">
                <h3>Crops</h3>
               <div class="container">
                   
                    <div class="form-group col-md-4 text-left ">
                        <label for="txtFilter">Search</label>
                        <asp:TextBox runat="server" ID="txtFilter" AutoPostBack="true" CssClass="form-control " ></asp:TextBox>
                    </div>
                        

                   
                    
                    <asp:GridView ID="GridView1" runat="server" CssClass="table table-hover " HorizontalAlign="Center" AutoGenerateColumns="False" DataKeyNames="UID" DataSourceID="SqlCrops" AllowPaging="True" AllowSorting="True" PageSize="100">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Id" ItemStyle-HorizontalAlign="Left" SortExpression="Id"></asp:BoundField>
                            <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-HorizontalAlign="Left" SortExpression="Description"></asp:BoundField>

                            <asp:TemplateField HeaderText="Use At Elevators" SortExpression="Use_At_Elevator">
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="Use_At_Elevator" OnCheckedChanged="Use_At_Elevator_CheckedChanged"  AutoPostBack="true" Checked='<%# Bind("Use_At_Elevator") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                       <%--     <asp:TemplateField HeaderText="Seed" SortExpression="Use_At_Seed_Mill">
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="Use_At_Seed_Mill" OnCheckedChanged="Use_At_Seed_Mill_CheckedChanged"  AutoPostBack="true" Checked='<%# Bind("Use_At_Seed_Mill") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>--%>
             
                            <asp:CheckBoxField DataField="Active" HeaderText="Active" SortExpression="Active" />
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlCrops" runat="server" ConnectionString="<%$ ConnectionStrings:NW_DataConnectionString %>" SelectCommand="SELECT UID, Id, Description, Use_At_Elevator, Use_At_Seed_Mill, Unit_Of_Measure, Active, Pound_Per_Bushel, Color_Index, Secondary_Color_Index FROM Crops WHERE (Description LIKE '%' + ISNULL(@Filter, Description) + '%') OR (Id LIKE '%' + ISNULL(@Filter, Id) + '%') ORDER BY Description" CancelSelectOnNullParameter="False">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="txtFilter" Name="Filter" PropertyName="Text" />
                        </SelectParameters>
                    </asp:SqlDataSource>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
