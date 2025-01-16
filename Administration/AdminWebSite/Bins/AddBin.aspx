<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AddBin.aspx.cs" Inherits="Bins_AddBin" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">


    <div class="form-container" style="display: flex; justify-content: flex-start; align-items: center; flex-direction: column; height: 100vh; padding-top: 20px;">
        <h4 runat="server" id="header">Create A New Bin</h4>
        <asp:HiddenField ID="hfLocations" runat="server" />

        <div class="form-group">
            <label for="cboLocations">Location</label>
            <asp:DropDownList ID="cboLocations" runat="server" CssClass="form-control" Style="max-width: 150px"></asp:DropDownList>
        </div>
        <div class="form-group">
            <label for="txtBinName">Bin</label>
            <asp:TextBox ID="txtBinName" runat="server" CssClass="form-control " AutoCompleteType="None" Style="max-width: 150px"></asp:TextBox>
        </div>

        <div class="form-group">
               <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSubmit_Click" />


            <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-secondary" Text="Cancel" OnClientClick="cancelForm(); return false;" />
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentFull" runat="Server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10"></script>
    <script type="text/javascript">
       <%-- $(document).ready(function () {
            var locations = JSON.parse($('#<%= hfLocations.ClientID %>').val());
            var $cboLocations = $('#<%= cboLocations.ClientID %>');

            $.each(locations, function (index, location) {
                $cboLocations.append($('<option>', {
                    value: location.Id,
                    text: location.Description
                }));
            });
        });--%>

   

        function cancelForm() {
            window.location.href = 'Bins.aspx';
        }
    </script>
</asp:Content>
