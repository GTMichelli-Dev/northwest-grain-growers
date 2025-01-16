<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="UpdateBushels.aspx.cs" Inherits="Bins_UpdateBushels" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="form-container" style="display: flex; justify-content: flex-start; align-items: center; flex-direction: column; height: 100vh; padding-top: 20px;">
        <h4 runat="server" id="header">Update</h4>
        <asp:HiddenField ID="hfBin" runat="server" />
        
        <div class="form-group">
            <label for="txtBushels">Bushels</label>
            <asp:TextBox ID="Bushels" runat="server" CssClass="form-control text-right" Style="max-width:150px" TextMode="Number" required="required" onkeypress="return isNumberKey(event)" oninput="validateWholeNumber(this)"></asp:TextBox>
        </div>
        
        <div class="form-group">
            <label for="txtProtein">Protein</label>
            <asp:TextBox ID="Protein" runat="server" CssClass="form-control text-right" Style="max-width:150px" TextMode="Number" Min="0" Max="20" required="required"></asp:TextBox>
        </div>
        
        <div class="form-group">
            <label for="txtDateTime">Date and Time</label>
            <asp:TextBox ID="DateTimeEntered" runat="server" CssClass="form-control text-right" Style="max-width:250px" TextMode="DateTimeLocal" required="required"></asp:TextBox>
        </div>
        
        <div class="form-group">
            <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="btnSubmit_Click" />
            <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-secondary" Text="Cancel" OnClientClick="cancelForm(); return false;" />
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentFull" runat="Server">
    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }

        function validateWholeNumber(input) {
            input.value = input.value.replace(/[^0-9]/g, '');
        }

        function submitForm() {
            var bushels = document.getElementById('<%= Bushels.ClientID %>').value;
            var protein = document.getElementById('<%= Protein.ClientID %>').value;
            var dateTimeEntered = document.getElementById('<%= DateTimeEntered.ClientID %>').value;

            if (bushels < 0) {
                alert("Bushels cannot be negative.");
                return;
            }

            if (protein < 0 || protein > 20) {
                alert("Protein must be between 0 and 20.");
                return;
            }

            __doPostBack('<%= btnSubmit.ClientID %>', '');
        }

        function cancelForm() {
            window.location.href = 'Bins.aspx';
        }
    </script>
</asp:Content>
