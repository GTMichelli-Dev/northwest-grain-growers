<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="LoadDetails.aspx.cs" Inherits="PreLoad_LoadDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
      <asp:UpdatePanel runat="server" ID="UP1">
          <ContentTemplate>



              <br />
              <div class=" form-row  pb-2 ">
                  <asp:LinkButton runat="server" ID="btnDone" Text="Back" Width="100px" CssClass="btn btn-secondary btn-sm   " PostBackUrl="~/PreLoad/TicketCreator.aspx"></asp:LinkButton>
              </div>

              <div class="form-row  font-weight-bold">
                  <div class="col-12 text-center form-control  ">
                      <asp:LinkButton runat="server" ID="lnkGrower" CssClass="text-center text-dark font-weight-bold  " Font-Size="Larger" ></asp:LinkButton>
                  </div>

              </div>
              <div class="form-row  font-weight-bold">
                  <%-- <div class="col-2 d-sm-none d-md-block"></div>--%>
                  <div class="col-12  ">
                      <label for="txtPO">PO</label>
                      <asp:TextBox runat="server" ID="txtPO" OnTextChanged="txtPO_TextChanged" AutoPostBack="true" CssClass="form-control "></asp:TextBox>
                  </div>
                  <div class="col-12  ">
                      <label for="txtBOL">BOL</label>
                      <asp:TextBox runat="server" ID="txtBOL" OnTextChanged="txtBOL_TextChanged" AutoPostBack="true" CssClass="form-control "></asp:TextBox>

                  </div>
                  <div class="col-12   ">
                    <label for="txtVehicle">Vehicle</label>
                    <asp:TextBox runat="server" ID="txtVehicle"  AutoCompleteType="Disabled"  OnTextChanged="txtVehicle_TextChanged"  AutoPostBack="true" CssClass="form-control "></asp:TextBox>

                  </div>
                  <div class="col-12  ">
                      <label for="txtComment">Comments</label>

                      <asp:TextBox runat="server" ID="txtComment" Rows="10" TextMode="MultiLine" OnTextChanged="txtComment_TextChanged" AutoPostBack="true" CssClass="form-control "></asp:TextBox>

                  </div>


              </div>
          </ContentTemplate>
          </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentFull" Runat="Server">
</asp:Content>

