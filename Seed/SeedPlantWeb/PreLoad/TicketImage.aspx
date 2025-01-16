<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="TicketImage.aspx.cs" Inherits="TicketImage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentFull" Runat="Server">
        
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class=" row col-12">
                    <asp:Label runat="server" ID="lblHdr" CssClass="h4 font-weight-bold text-center  " Width="100%"></asp:Label>
                </div>
                <asp:Repeater runat="server" ID="Repeater1">
                    <ItemTemplate>
                        <div class="form-row">
                            <div class="col-12 ">
                                <asp:label runat="server" ID="lnkPicture" Text='<%# Eval("FName") %>'   ></asp:label>
                            </div>
                            <div class="col-12 ">
                                <asp:Image runat="server" Width="100%" ID="imgPic"  ImageUrl='<%# Eval("ImageURL") %>' />
                            </div>

                        </div>
                        <hr />
                    </ItemTemplate>
                </asp:Repeater>
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>

