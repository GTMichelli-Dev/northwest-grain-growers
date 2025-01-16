<%@ Page Title="" Language="C#" MasterPageFile="~/Seed.master" AutoEventWireup="true" CodeFile="Colors.aspx.cs" Inherits="SeedPlant_Colors" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:Table runat="server" ID="tblColors" CssClass="table table-bordered table-sm  " HorizontalAlign="Center" >
        
        <asp:TableRow>
            <asp:TableHeaderCell>Index</asp:TableHeaderCell>
            <asp:TableCell>0</asp:TableCell>
            <asp:TableCell>1</asp:TableCell>
            <asp:TableCell>2</asp:TableCell>
            <asp:TableCell>3</asp:TableCell>
            <asp:TableCell>4</asp:TableCell>
            <asp:TableCell>5</asp:TableCell>
            <asp:TableCell>6</asp:TableCell>
            <asp:TableCell>7</asp:TableCell>
            <asp:TableCell>8</asp:TableCell>
            <asp:TableCell>9</asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell>Color</asp:TableHeaderCell>
            <asp:TableCell><asp:Button runat="server" Width="50px" CssClass="btn font-weight-bold  " ID="Button0" BackColor="#ffffff" /></asp:TableCell>
            <asp:TableCell><asp:Button runat="server" Width="50px" CssClass="btn font-weight-bold  " ID="Button1" BackColor="#ffffff" /></asp:TableCell>
            <asp:TableCell><asp:Button runat="server" Width="50px" CssClass="btn font-weight-bold  " ID="Button2" BackColor="#ffffff" /></asp:TableCell>
            <asp:TableCell><asp:Button runat="server" Width="50px" CssClass="btn font-weight-bold  " ID="Button3" BackColor="#ffffff" /></asp:TableCell>
            <asp:TableCell><asp:Button runat="server" Width="50px" CssClass="btn font-weight-bold  " ID="Button4" BackColor="#ffffff" /></asp:TableCell>
            <asp:TableCell><asp:Button runat="server" Width="50px" CssClass="btn font-weight-bold  " ID="Button5" BackColor="#ffffff" /></asp:TableCell>
            <asp:TableCell><asp:Button runat="server" Width="50px" CssClass="btn font-weight-bold  " ID="Button6" BackColor="#ffffff" /></asp:TableCell>
            <asp:TableCell><asp:Button runat="server" Width="50px" CssClass="btn font-weight-bold  " ID="Button7" BackColor="#ffffff" /></asp:TableCell>
            <asp:TableCell><asp:Button runat="server" Width="50px" CssClass="btn font-weight-bold  " ID="Button8" BackColor="#ffffff" /></asp:TableCell>
            <asp:TableCell><asp:Button runat="server" Width="50px" CssClass="btn font-weight-bold  " ID="Button9" BackColor="#ffffff" /></asp:TableCell>
          
        </asp:TableRow>
    </asp:Table>



<%--    <div class="form-inline">
        <label class="sr-only" for="inlineFormInputName2">Name</label>
        <input type="text" class="form-control mb-2 mr-sm-2" id="inlineFormInputName2" placeholder="Jane Doe" />

        <label class="sr-only" for="inlineFormInputGroupUsername2">Username</label>
        <div class="input-group mb-2 mr-sm-2">
            <div class="input-group-prepend">
                <div class="input-group-text">@</div>
            </div>
            <input type="text" class="form-control" id="inlineFormInputGroupUsername2" placeholder="Username" />
        </div>

        <div class="form-check mb-2 mr-sm-2">
            <input class="form-check-input" type="checkbox" id="inlineFormCheck" />
            <label class="form-check-label" for="inlineFormCheck">
                Remember me
            </label>
        </div>

        <button type="submit" class="btn btn-primary mb-2">Submit</button>
    </div>--%>
</asp:Content>

