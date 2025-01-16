<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Locations.aspx.cs" Inherits="Locations" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <%--  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>



            <div class="text-center ">
                <h3>Locations </h3>
                <p>To run the scale program on your local computer you will need to install <a href="../InstallFiles/CRVS2010CR6424_0-10010309.ZIP">Crystal Reports</a>  </p>

                
                <p>To run the remote viewer(VNC) you will need to install <a href="../InstallFiles/tightvnc-2.8.11-gpl-setup-64bit.msi">Tight VNC</a></p>
                
               


                <p>&nbsp;</p>

            <asp:GridView ID="GridView1" HorizontalAlign="Center" CssClass="table table-hover " OnSorting="GridView1_Sorting" runat="server" AutoGenerateColumns="False" AllowSorting="True">
                <Columns>
                    
                    <asp:BoundField DataField="SiteName" HeaderText="Location" HeaderStyle-CssClass="text-center " ItemStyle-CssClass="text-left " SortExpression="SiteName" ReadOnly="True" />
                    <asp:BoundField DataField="Computer" HeaderText="Computer" HeaderStyle-CssClass="text-center " ItemStyle-CssClass="text-left "  SortExpression="Computer" ReadOnly="True" />
                    <asp:TemplateField HeaderText="VNC">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkVNC" OnClick="lnkVNC_Click"   runat="server">Remote Control</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Scale Web Site">
                        <ItemTemplate>
                            <asp:HyperLink  ID="lnkScaleWeb" Visible='<%# Eval("HasScales") %>' NavigateUrl='<%# Eval("LocalWeb") %>' Text="Scales" runat="server"  />
                           <%-- Target="_blank"--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Scale Program" >
                        <ItemTemplate>
                            <asp:HiddenField ID="hfAddress" Value='<%# Eval("Address") %>' runat="server" />
                             <asp:HiddenField ID="hfFileLocation" Value='<%# Eval("FileLocation") %>' runat="server" />
                             <asp:HiddenField ID="hfBatchName" Value='<%# Eval("SiteName") + "_" +Eval("Computer") %>' runat="server" />
                            <asp:LinkButton ID="lnkBatchFile" OnClick="lnkBatchFile_Click"  runat="server">Scale Program</asp:LinkButton>

                            
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
                <asp:HiddenField ID="hfSortField" runat="server" />
                <asp:HiddenField ID="hfSortDirection" runat="server" />
                </div>
<%--        </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>

