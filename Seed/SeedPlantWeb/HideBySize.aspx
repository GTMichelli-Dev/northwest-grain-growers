<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="HideBySize.aspx.cs" Inherits="HideBySize" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
     <div class="d-none d-xl-block">
     XL
 </div>
  
       
 <div class="d-none d-lg-block d-xl-none ">
     Lg
 </div>


 <div class="d-none d-md-block d-lg-none ">
    medium
 </div>

 <div class="d-none d-sm-block d-md-none ">
      small  
 </div>


 <div class= "d-block d-sm-none">
    xs
 </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentFull" Runat="Server">
</asp:Content>

