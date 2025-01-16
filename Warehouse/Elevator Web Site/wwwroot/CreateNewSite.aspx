<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="CreateNewSite.aspx.cs" Inherits="CreateNewSite" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">


     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
         <ContentTemplate>

             <h2 class="page-header ">Create New Site</h2>
             <div class=" container row ">
                 <div class="row text-center  ">
                     <div class="h4">Available Locations</div>
                     <p>
                         <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" CssClass="dropdown h4" DataSourceID="SqlDescriptions" DataTextField="text" DataValueField="value">
                         </asp:DropDownList>
                         <br />
                     </p>

                 </div>
                 <div class="row">
                     <div class=" col-md-4 ">
                     </div>

                     <div class=" col-md-4 ">

                         <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table " DataSourceID="SqlAvailable_Sites" HorizontalAlign="Center" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                             <Columns>
                                 <asp:TemplateField ShowHeader="False">
                                     <ItemTemplate>
                                         <asp:LinkButton ID="lnkSelect" runat="server" CausesValidation="False" CommandName="Select" OnClick="lnkSelect_Click" Text="Select"></asp:LinkButton>
                                     </ItemTemplate>
                                     <ControlStyle CssClass="btn btn-primary" />
                                 </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Id" SortExpression="Location_Id">
                                     <EditItemTemplate>
                                         <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Location_Id") %>'></asp:TextBox>
                                     </EditItemTemplate>
                                     <ItemTemplate>
                                         <asp:Label ID="lblID" runat="server" Text='<%# Bind("Location_Id") %>'></asp:Label>
                                     </ItemTemplate>
                                 </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Description" SortExpression="Location_Description">
                                     <EditItemTemplate>
                                         <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Location_Description") %>'></asp:TextBox>
                                     </EditItemTemplate>
                                     <ItemTemplate>
                                         <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Location_Description") %>'></asp:Label>
                                     </ItemTemplate>
                                 </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Sequence" SortExpression="Sequence_Description">
                                     <EditItemTemplate>
                                         <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("Sequence_Description") %>'></asp:TextBox>
                                     </EditItemTemplate>
                                     <ItemTemplate>
                                         <asp:Label ID="lblSequence" runat="server" Text='<%# Bind("Sequence_Description") %>'></asp:Label>
                                         <asp:HiddenField ID="hfSequenceID" runat="server" Value='<%# Eval("Sequence_Id") %>' />
                                     </ItemTemplate>
                                 </asp:TemplateField>
                             </Columns>
                             <EmptyDataTemplate>
                                 Select A Location First
                             </EmptyDataTemplate>
                         </asp:GridView>
                         <asp:SqlDataSource ID="SqlAvailable_Sites" runat="server" ConnectionString='<%$ ConnectionStrings:LocalDBConnectionString %>' SelectCommand="SELECT vwUnusedSequences.Server_Name, vwUnusedSequences.Location_Id, vwUnusedSequences.Sequence_Id, vwUnusedSequences.Location_Description, vwUnusedSequences.Sequence_Description FROM vwUnusedSequences LEFT OUTER JOIN Site_Setup ON vwUnusedSequences.Location_Id = Site_Setup.Location_Id AND vwUnusedSequences.Server_Name = Site_Setup.Server_Name WHERE (vwUnusedSequences.Server_Name = @@SERVERNAME) AND (vwUnusedSequences.Location_Description = @Location_Description) AND (vwUnusedSequences.Sequence_Id &lt; 4) AND (Site_Setup.UID IS NULL) ORDER BY vwUnusedSequences.Location_Id">
                             <SelectParameters>
                                 <asp:ControlParameter ControlID="DropDownList1" Name="Location_Description" PropertyName="SelectedValue" />
                             </SelectParameters>
                         </asp:SqlDataSource>

                         <asp:SqlDataSource ID="SqlDescriptions" runat="server" ConnectionString="<%$ ConnectionStrings:LocalDBConnectionString %>" SelectCommand="SELECT DISTINCT vwUnusedSequences.Location_Description + ' - ' + LTRIM(STR(vwUnusedSequences.Location_Id)) AS text, vwUnusedSequences.Location_Description AS value FROM vwUnusedSequences LEFT OUTER JOIN Site_Setup ON vwUnusedSequences.Location_Id = Site_Setup.Location_Id AND vwUnusedSequences.Server_Name = Site_Setup.Server_Name WHERE (vwUnusedSequences.Server_Name = @@SERVERNAME) AND (vwUnusedSequences.Sequence_Id &lt; 4) AND (Site_Setup.UID IS NULL) AND (vwUnusedSequences.Location_Id &lt;&gt; 0) UNION SELECT 'Select Location' AS text, NULL AS value ORDER BY value"></asp:SqlDataSource>

                     </div>
                     <div class=" col-md-4 ">
                     </div>
                 </div>
             </div>
         </ContentTemplate>
         </asp:UpdatePanel>
</asp:Content>

