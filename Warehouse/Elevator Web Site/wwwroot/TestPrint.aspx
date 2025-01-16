<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="TestPrint.aspx.cs" Inherits="TestPrint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <script>
        function printTest() {
            setTimeout(function () {
                $('#printing').html('Printing Ticket');
            $('#printing').show();
            let printer = $('#ddPrinter').val();
                $.ajax({
                    type: "POST",
                    url: "/WS.asmx/PrintTestTicket",
                    data: JSON.stringify({
                        'PrinterToUse': printer
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (results) {
                        
                        $('#printing').hide('slow');
                        alert(results.d);
                    }
                    ,
                    error: function (xhr, status, error) {

                        var errorMessage = xhr.status + ': ' + xhr.statusText
                        alert(errorMessage);
                        $('#printing').hide('slow');
                    }
                });
            }, 750);
           }
               
    </script>

    <div class="row text-center ">
        <h1 class=" col-md-12 text-center">Test print a ticket</h1>
        <div class=" col-md-12 mb-0 pb-0 text-center">
            <label>Select Printer To Test</label>
        </div>
    </div>

    <div class="row text-center ">
        <div class=" col-md-2"></div>
        <div class="col-md-8">

        
        <div class="col-md-4"></div>
        <div class="col-md-4">
            <asp:DropDownList ID="ddPrinter" ClientIDMode="Static" CssClass="form-control  " runat="server" DataSourceID="SqlPrinters" DataTextField="Printer_Name" DataValueField="Printer_Name"></asp:DropDownList>
            <asp:SqlDataSource ID="SqlPrinters" runat="server" ConnectionString="<%$ ConnectionStrings:LocalDBConnectionString %>" SelectCommand="SELECT Printer_Name, 1 AS idx FROM Site_Printers WHERE (Server_Name = @@SERVERNAME) AND (NOT (Printer_Name LIKE N'%DYMO%')) ORDER BY idx, Printer_Name"></asp:SqlDataSource>
        </div>
        <div class=" col-md-4"></div>
            </div>
    </div>
    <div class="row text-center " style="padding-top:25px">

        <div class=" col-md-12 mt-3 text-center ">
            <input class=" btn btn-success " onclick="printTest();" value=" Test Print" />

        </div>

    </div>
    <div class="row text-center ">

        <div class="col-12  text-center " >
            <h2 class="text-center text-info" style="display:none" id="printing">Printing Test Ticket</h2>

        </div>
    </div>

</asp:Content>


