<%@ Page Title="Kiosk" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Kiosk.aspx.cs" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <style>
       body {
    background-color: white;
    font-family: Arial, sans-serif;
}



        .header-row {
            position: relative;
            display: flex;
            align-items: center;
            height: 60px; /* Adjust as needed for your header */
        }

            .header-row img {
                height: 30px;
                width: auto;
                display: block;
                margin-left: 0;
            }

        .header-center {
            position: absolute;
            left: 0;
            right: 0;
            text-align: center;
            pointer-events: none; /* Allows clicks to pass through if needed */
        }

        .header {
            display: inline-block;
            font-size: 3em;
           
            line-height: 1;
            pointer-events: auto;
        }



        .prompt {
            margin-top: 5px;
            font-size: 8em;
            
            line-height: 1;
        }

        .status {
           
            margin-top: 0px;
            font-size: 5em;
            line-height: 1;
            min-height: 1em; /* Ensures height is maintained even if empty */
        }

        .weight {
            text-align: center;
            font-size: 15em;
            font-weight: bold;
           
            margin-top: 2px; /* Move 25px lower */
            line-height: 1;
        }

        img[src="nwgg.jpg"] {
            height: 100px;
            width: auto; /* Maintains aspect ratio */
            display: block; /* Optional: removes inline spacing */
        }

        .main-container {
            width: 90vw;
            max-width: 90vw;
             margin: 20px auto 0 auto;
            border: 2px solid #000;
            box-sizing: border-box;
           
            padding: 5px;
        }

        @media (max-width: 600px) {
            .header {
                font-size: 1.5em;
                line-height: 1;
            }

            .prompt {
                font-size: 1.5em;
                line-height: 1;
              
            }

            .status {
                margin-top: 0px;
                font-size: 1em;
                line-height: 1;
                min-height: 1em;
                
            }

            .weight {
                font-size: 2em;
                line-height: 1;
              
            }
        }
    </style>


    <div class="header-row">
        <img src="nwgg.jpg" />
        <div class="header-center">
            <div class="header">
                <div id="scaleName"></div>

                <div id="printerName"></div>
            </div>
        </div>
    </div>
    <div  class="main-container">
        <div class="weight" id="weight">--</div>
          <div class="status" id="status"></div>
    </div>
  
    <div class="prompt" id="prompt"></div>


    <script type="text/javascript">
        let inputBuffer = "";
        let locationId = getQueryParam('id');
        let scaleName = getQueryParam('scale');
        let printer = getQueryParam('printer');
        let weight = 0;
        let scaleStatus = "";
        let prompt = "";
        let ok = false;
        let motion = false;
        let promptTmrSet = false;


        $(document).ready(function () {
            $('#navBar').hide();
            $('#footer').hide();

            getWeight()
        });


        function resetScalevalues(){
            let weight = 0;
            let scaleStatus = "";
            
            let ok = false;
            let motion = false;
        }

        function setPrompt(message, timeOut, backcolor,forecolor) {
            promptTmrSet = true;
            $('#prompt').html(message);
            $('body').css({
                'background-color': backcolor,
                'color': forecolor
            });
            setTimeout(function () {
                $('body').css({
                    'background-color': 'white',
                    'color': 'black'
                });
                    promptTmrSet = false;
                
            }, timeOut);
        }
       

        $(document).on('keyup', function (event) {
            if (event.key === "Enter") {
                if (ok) {
                    
                    $.ajax({
                        type: "POST",
                        url: "Kiosk.asmx/CheckTicket",
                        data: JSON.stringify({ Ticket: inputBuffer, Weight: parseInt(weight, 10) }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                          
                            console.log(response.d);
                            
                            switch (response.d) {
                                case 0: // Invalid
                                    setPrompt("Invalid Ticket", 2000,"red","black"); 
                                    break;
                                case 1: // InvalidScaleUID
                                    setPrompt("Invalid Scale", 2000,"red", "white"); 
                                    break;
                                case 2: // CheckInboundTicket
                                    $('#status').text('Check Inbound Ticket',"dodgerblue","black");
                                    break;
                                case 3: // Complete
                                    $('#status').text('Complete', "dodgerblue", "black");
                                    break;
                                case 4: // OldTicket
                                    setPrompt("Ticket<br/> Already Used", 2000,"yellow", "black"); 
                                    break;
                                case 5: // InvalidLocation
                                    setPrompt("Invalid Location", 2000,"yellow","black"); 
                                   
                                    break;
                                case 6: // WeightToLow
                                    setPrompt("Truck <br/>Weight Too Low", 2000,"yellow","black"); 
                                   
                                    break;
                                case 7: // TruckNotUnloaded
                                    setPrompt("Truck Not<br/> Unloaded", 2000, "yellow", "black"); 
                                    
                                    break;
                                case 8: // ReadyToComplete
                                    setPrompt("Ready To Complete", 2000, "dodgerblue", "black"); 
                                    
                                    break;
                                default:
                                    setPrompt("Unknown Error", 2000,"red","black");
                                   
                                    break;
                            }
                        },
                        error: function (xhr, status, error) {
                            setPrompt("Unknown Error", 2000, "red", "black");
                        }
                    });
                }
                inputBuffer = "";
            } else if (event.key.length === 1) {
                inputBuffer += event.key;
            }
        });

        function getQueryParam(name) {
            const urlParams = new URLSearchParams(window.location.search);
            return urlParams.get(name);
        }

        async function getWeight() {
           

          
         

            try {
                if (locationId && scaleName && printer) {
                    const response = await fetch('Kiosk.asmx/GetScale', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json; charset=utf-8'
                        },
                        body: JSON.stringify({ description: scaleName, locationId: parseInt(locationId), printerName: printer })
                    });

                    if (!response.ok) {
                        throw new Error('Network response was not ok');
                    }

                    const result = await response.json();
                    console.log(result.d);
                     weight = result.d.Weight;
                     scaleStatus = result.d.Error_Message;
                     
                     ok = result.d.OK;
                     motion = result.d.Motion;
                    let serverMessage = d.ServerMessage;
                    let messageTimeOut = d.MessageTimeOut;
                    let backColor = d.BackColor || 'white';
                    let foreColor = d.ForeColor || 'black';
                    if (serverMessage && serverMessage.length > 0) {
                        setPrompt(serverMessage, messageTimeOut, backcolor, forecolor);
                    }

                    if (ok) {
                        $('#weight').text(weight + ' lbs');
                        if (motion) {
                            $('#status').text('Motion');
                            $('.main-container').css({
                                'background-color': 'yellow',
                                'color': 'black'
                            });
                            if (weight > 10000 && ! promptTmrSet) {
                                $('#prompt').html('Please wait<br />for the scale<br />to settle.');
                            } 
                                
                        } else {
                            if (weight > 10000 && !promptTmrSet) {
                                $('#prompt').html('Please wait<br />for Operator<br />Or Scan Ticket');
                            } 
                            else if (weight < 1000 && !promptTmrSet) {
                                $('#prompt').html('Drive On<br />Scale<br />To Continue');
                            }
                            $('#status').text('Ok');
                            $('.main-container').css({
                                'background-color': 'white',
                                'color': 'black'
                            });
                        }

                     
                    } else {
                        resetScalevalues();
                        $('.main-container').css({
                            'background-color': 'red',
                            'color': 'white'
                        });

                        $('#weight').text('ERROR');

                        $('#status').text(scaleStatus);
                        $('#prompt').html('Please contact<br />the scale operator<br />for assistance.');
                    }
                } else {
                    resetScalevalues();
                    $('.main-container').css({
                        'background-color': 'red',
                        'color': 'white'
                    });
                    $('#weight').text('ERROR');
                    $('#status').text("");
                    $('#scaleName').text('Missing');
                    $('#printerName').text('Parameters');
                    $('#prompt').html('Missing id, scale<br /> or printer<br />Parameter');
                    console.warn('Missing id, scale or printer in query string.');
                }
            } catch (error) {
                resetScalevalues();
                $('.main-container').css({
                    'background-color': 'red',
                    'color': 'white'
                });
                $('#weight').text('ERROR');
                $('#status').text('');
                $('#prompt').html('error');
                console.error('Error:', error);
            } finally {
                setTimeout(getWeight, 500);
            }
        }



 </script>
</asp:Content>
