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
      let scaleName = getQueryParam('scale');
      let printer = getQueryParam('printer');
      let weight = 0;
      let scaleStatus = "";
      let prompt = "";
      let ok = false;
      let motion = false;
      let promptTmrSet = false;
      let lockedWeight = 0;
      let pauseDefaultMessage = false;

      console.log('printer', printer);
      console.log('scaleName', scaleName);

      //setInterval(() => {
      //    fetch('/heartbeat-or-status')
      //        .then(res => {
      //            if (!res.ok) location.reload();
      //        })
      //        .catch(() => location.reload());
      //}, 30000);

      $(document).ready(function () {
          $('#navBar').hide();
          $('#footer').hide();
          $('img[src="nwgg.jpg"]').css('cursor', 'pointer').on('click', function () {
              location.reload();
          });
          getWeight();
      });

      function resetScalevalues() {
          weight = 0;
          scaleStatus = "";
          lockedWeight = 0;
          ok = false;
          motion = false;
      }

      function setPrompt(message, timeOut, backcolor, forecolor) {
          promptTmrSet = true;
          pauseDefaultMessage= false;
          console.log("prompt", message, "timeOut", timeOut);
          $('#prompt').html(message);
          $('body').css({
              'background-color': backcolor,
              'color': forecolor
          });

          setTimeout(function () {
              if (promptTmrSet) {
                  $('body').css({
                      'background-color': 'white',
                      'color': 'black'
                  });
                  $('#prompt').html('');
                  promptTmrSet = false;
                 
              }
          }, timeOut);
      }

      $(document).on('keyup', async function (event) {
          if (event.key === "Enter") {
              if (ok) {
                  try {
                      const response = await fetch("Kiosk.asmx/CheckTicket", {
                          method: "POST",
                          headers: {
                              "Content-Type": "application/json; charset=utf-8"
                          },
                          body: JSON.stringify({ Ticket: inputBuffer, Weight: parseInt(weight, 10) })
                      });
                      if (!response.ok) throw new Error("Network response was not ok");
                      const result = await response.json();
                      console.log("CheckTicket result:", result);
                      switch (result.d) {
                          case 0: setPrompt("Invalid Ticket", 2000, "red", "white"); break;
                          case 1: setPrompt("Invalid Scale", 2000, "red", "white"); break;
                          case 2: setPrompt("Ticket<br/> Already Used", 2000, "yellow", "black"); break;
                          case 3:
                              if (lockedWeight > 0) {
                                  await callProcessTicket(inputBuffer, weight, scaleName, printer);
                              } else {
                                  setPrompt("Ticket<br/> Already Used", 2000, "yellow", "black"); break;
                              }
                              break;
                          case 4: setPrompt("Ticket<br/> Already Used", 2000, "yellow", "black"); break;
                          case 5: setPrompt("Invalid Location", 2000, "yellow", "black"); break;
                          case 6: setPrompt("Truck <br/>Weight Too Low", 2000, "yellow", "black"); break;
                          case 7: setPrompt("Truck Not<br/> Unloaded", 2000, "yellow", "black"); break;
                          case 8:
                              await callProcessTicket(inputBuffer, weight, scaleName, printer);
                              break;
                          default: setPrompt("Unknown Error", 2000, "red", "black"); break;
                      }
                  } catch (error) {
                      setPrompt("Unknown Error", 2000, "red", "black");
                  }
              }
              inputBuffer = "";
          } else if (event.key.length === 1) {
              inputBuffer += event.key;
          }
      });

      async function callProcessTicket(ticket, weight, scale, printer ) {

          try {
              if (motion) {
                  const noMotion = await waitForNoMotion();
                  if (!noMotion) {
                      setPrompt('Canceled <br/> due to <br />Motion',2000, 'red', 'white');
                      return null;
                  }
              }
              $('body').css({
                  'background-color': '#ffbf00',
                  'color': 'black'
              });
              $('#prompt').html('Checking<br/>Ticket');
              pauseDefaultMessage = true; 
              promptTmrSet = false;
              console.log("Processing ticket:", ticket, "Weight:", weight, "Scale:", scale, "Printer:", printer);
              const response = await fetch("Kiosk.asmx/ProcessTicket", {
                  method: "POST",
                  headers: {
                      "Content-Type": "application/json; charset=utf-8"
                  },
                  body: JSON.stringify({
                      Ticket: ticket,
                      Weight: parseInt(weight, 10),
                      Scale: scale,
                      Printer: printer
                  })
              });
             
              if (!response.ok) {
                  
                  setPrompt("Error Saving Load", 2000, "red", "white");
                  throw new Error("Network response was not ok");
              }

              const result = await response.json();
              lockedWeight = weight;
              console.log("Locked Weight", lockedWeight);
              return result.d;
          } catch (error) {
              setPrompt("Unknown Error", 2000, "red", "black");
              return null;
          }
      }

      function getQueryParam(name) {
          const urlParams = new URLSearchParams(window.location.search);
          return urlParams.get(name);
      }

      async function waitForNoMotion(timeoutMs = 10000, intervalMs = 200) {
          const start = Date.now();
          setPrompt("Waiting For <br/> Scale To<br/>Settle", timeoutMs, "dodgerblue", "black");
          while (motion && (Date.now() - start) < timeoutMs) {
              await new Promise(resolve => setTimeout(resolve, intervalMs));
          }
          return !motion;
      }

      async function getWeight() {
          try {
              if (scaleName && printer) {
                  const response = await fetch('Kiosk.asmx/GetScale', {
                      method: 'POST',
                      headers: {
                          'Content-Type': 'application/json; charset=utf-8'
                      },
                      body: JSON.stringify({ description: scaleName, printerName: printer })
                  });

                  if (!response.ok) {
                      $('#weight').text('ERROR');
                      $('#status').text("");
                      $('#scaleName').text('Connection');
                      $('#printerName').text('Error');
                      if (!promptTmrSet) $('#prompt').html('Error connecting to server');
                      throw new Error('Network response was not ok');
                      
                      //setTimeout(function () {
                      //    location.reload()
                      //}, 2000);
                  }

                  const result = await response.json();
                  weight = result.d.Weight;
                  scaleStatus = result.d.Error_Message;
                  ok = result.d.OK;
                  motion = result.d.Motion;
                  let serverMessage = result.d.ServerMessage;
                  let messageTimeOut = result.d.MessageTimeOut;
                  let backColor = result.d.BackColor || 'white';
                  let foreColor = result.d.ForeColor || 'black';

                  if (serverMessage && serverMessage.length > 0) {
                      console.log('ServerMessage', serverMessage);
                      setPrompt(serverMessage, messageTimeOut, backColor, foreColor);
                  }

                  if (ok) {
                      $('#scaleName').text('');
                      $('#printerName').text('');
                      if ((lockedWeight>0) &&( Math.abs(weight - lockedWeight) > 500)) {
                         console.log('Weight changed significantly, resetting locked weight');
                          lockedWeight = 0;
                      }

                      $('#weight').text(weight + ' lbs');

                      if (motion) {
                          $('#status').text('Motion');
                          if (!promptTmrSet) {
                             $('.main-container').css({
                                  'background-color': 'yellow',
                                  'color': 'black'
                              });
                          }
                      } else {
                          $('#status').text('Ok');
                          if (!promptTmrSet) {
                           
                              $('.main-container').css({
                                  'background-color': 'white',
                                  'color': 'black'
                              });
                          }
                      }
                      if (!promptTmrSet && !pauseDefaultMessage) {
                          if (weight > 1000) {
                              $('#prompt').html('Please wait<br />for Operator<br />Or Scan Ticket');
                          } else {
                              $('#prompt').html('Drive On<br />Scale<br />To Continue');
                          }
                        
                      }
                  } else {
                      resetScalevalues();
                      if (!promptTmrSet) {
                          $('.main-container').css({
                              'background-color': 'red',
                              'color': 'white'
                          });
                          $('#scaleName').text('Scale');
                          $('#printerName').text('Error');
                          $('#weight').text('ERROR');
                          $('#status').text(scaleStatus);
                          $('#prompt').html('Please contact<br />the scale operator<br />for assistance.');
                      }
                  }
              } else {
                  resetScalevalues();
                  if (!promptTmrSet) {
                      $('.main-container').css({
                          'background-color': 'red',
                          'color': 'white'
                      });
                      $('#weight').text('ERROR');
                      $('#status').text("");
                      $('#scaleName').text('Missing');
                      $('#printerName').text('Parameters');
                      $('#prompt').html('Missing scale<br /> or printer<br />Parameter');
                  }
              }
          } catch (error) {
              $('#scaleName').text('System');
              $('#printerName').text('Error');
              resetScalevalues();
              if (!promptTmrSet) {
                  $('.main-container').css({
                      'background-color': 'red',
                      'color': 'white'
                  });
                  $('#weight').text('ERROR');
                  $('#status').text('');
                  $('#prompt').html('error');
              }
              console.error('Error:', error);
          } finally {
              setTimeout(getWeight, 500);
          }
      }
  </script>

</asp:Content>
