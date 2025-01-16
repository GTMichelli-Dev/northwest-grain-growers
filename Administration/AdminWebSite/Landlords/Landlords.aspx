<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Landlords.aspx.cs" Inherits="Landlords_Landlords" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Button ID="btnDeleteAll" runat="server" CssClass="btn btn-danger" Text="Delete All Landlords" OnClientClick="confirmDeleteAll(); return false;" />

    <div class="container">
        <h2>Landlords</h2>
        <div class="row">
            <div class="col-md-4 offset-md-4 text-center">
                <asp:GridView ID="gvLandlords" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField DataField="Description" HeaderText="Name" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <!-- SweetAlert and JavaScript function -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script type="text/javascript">
        function confirmDeleteAll() {
            Swal.fire({
                title: 'Delete All Landlords?',
                text: "You won't be able to revert this!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, delete all!'
            }).then((result) => {
                if (result.isConfirmed) {
                    // Call the server-side method to delete all landlords
                    __doPostBack('<%= btnDeleteAll.UniqueID %>', 'deleteAll');
                }
            });
        }
    </script>
</asp:Content>
