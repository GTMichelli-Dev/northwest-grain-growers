using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Mvc;
using Seed25.DTO;
using Seed25.Report;
using System;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;




[ApiController]
[Route("api/[controller]")]
public class HikvisionController:ControllerBase
{
  

    [HttpPost("gotoPreset")]
    public async Task<bool> GoToPresetAsync([FromBody] CameraRequest cr )
    {
            HttpClient _http;
    var handler = new HttpClientHandler
        {
            Credentials = new NetworkCredential(cr.Username,cr.Password),
            PreAuthenticate = true
        };
        _http = new HttpClient(handler); 
        var resp = await _http.PutAsync($"http://{cr.Ip}/ISAPI/PTZCtrl/channels/1/presets/{cr.PresetId}/goto", new StringContent(""));
        return resp.IsSuccessStatusCode;
    }

    public class CameraRequest
    {
      
        public string Ip { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public int PresetId { get; set; }
        public int Channel { get; set; } = 1;
        public int PostGotoDelayMs { get; set; } = 800;

      
    }

    public class CameraOverlayRequest: CameraRequest
    {
        public long Ticket { get; set; }
        public string AnchorX { get; set; }
        public string AnchorY { get; set; }
        public int MarginX { get; set; }
        public int MarginY { get; set; }
        public int Width { get; set; }
        public bool KioskTicket { get; set; }

    }

    private MemoryStream GetInboundTicketImage()
    {
        var dto = new InboundTicketDTO
        {
            UID = Guid.NewGuid(),
            Ticket = 123456,
            Weight = 2800,
            TimeIn = DateTime.Now,
            Plant = "Wasco Seed Plant.",
            Phone = "541-442-5555 ",
            Prompt1 = "",
            Prompt2 = ""
        };

        XtraReport report = new TestReport(dto);
        using (var ms = new MemoryStream())
        {
            report.ExportToImage(ms);

            ms.Position = 0;    
            return ms;

        }
    }

    [HttpGet("testpicture3")]
    public async Task<IActionResult> testPicture3()
    {
        await Cameras.CameraService.MoveAndShootAsync(
            vendor: Cameras.CameraVendor.Hikvision,
            ipAddress: "10.165.1.62",
            username: "admin",
            password: "Scale_Us3r",
            presetNumber: 3,
            outputFolderPath: @"C:\Temp",
            fileNameWithoutExtension: "pic3",
            https: false,
            port: 80,
            channel: 1);
        return Ok();
    }


    [HttpGet("testpicture2")]
    public async Task<IActionResult> testPicture2()
    {
        await Cameras.CameraService.MoveAndShootAsync(
            vendor: Cameras.CameraVendor.Hikvision,
            ipAddress: "10.165.1.62",
            username: "admin",
            password: "Scale_Us3r",
            presetNumber: 2,
            outputFolderPath: @"C:\Temp",
            fileNameWithoutExtension: "pic2",
            https: false,
            port: 80,
            channel: 1);
        return Ok();
    }


    

    [HttpPost("testpictureOverlay")]
    public async Task<IActionResult> testpictureOverlay([FromBody] CameraOverlayRequest cr)
    {


        AnchorX anchorXEnum;
        if (!Enum.TryParse<AnchorX>(cr.AnchorX, ignoreCase: true, out anchorXEnum))
        {
            anchorXEnum = AnchorX.Center;
        }

        AnchorY anchorYEnum;
        if (!Enum.TryParse<AnchorY>(cr.AnchorY, ignoreCase: true, out anchorYEnum))
        {
            anchorYEnum = AnchorY.Top;
        }


        if (cr.Width < 100 || cr.Width > 2000) cr.Width = 500;
        if (cr.MarginX < 0 || cr.MarginX > 500) cr.MarginX = 10;
        if (cr.MarginY < 0 || cr.MarginY > 500) cr.MarginY = 10;
        if (cr.PostGotoDelayMs < 500 || cr.PostGotoDelayMs > 10000) cr.PostGotoDelayMs = 800;

        var baseMs = await Cameras.CameraService.MoveAndShootToStreamAsync(
            vendor: Cameras.CameraVendor.Hikvision,
            ipAddress: cr.Ip,
            username: cr.Username,
            password: cr.Password,
            presetNumber: cr.PresetId,
            https: false,
            port: 80,
            channel: cr.Channel);







        using (var overlayMs = await new PrintApiController().GetPictureInvoiceEmbed(cr.Ticket,cr.KioskTicket))
        {

            ImageComposer.OverlayImageAnchored(
           baseImageStream: baseMs,
           overlayImageStream: overlayMs,
           outputPngPath: @"C:\Temp\1234567890_Inbound.png",
           anchorX: anchorXEnum,
           anchorY: anchorYEnum,

           marginX: cr.MarginX,
           marginY: cr.MarginY,
           width: cr.Width,
           clampToBounds: true);
        }
        return Ok();
    }


  

    [HttpPost("saveSnapshotAsync/{filePath}")]
    public async Task<bool> SaveSnapshotAsync(string filePath)
    {
        HttpClient _http;
        var handler = new HttpClientHandler
        {
            Credentials = new NetworkCredential("admin", "Scale_Us3r"),
            PreAuthenticate = true
        };
        _http = new HttpClient(handler); //{ BaseAddress = new Uri($"http://{"10.165.1.62"}") };
      //  var url = $"/ISAPI/PTZCtrl/channels/1/presets/{presetId}/goto";
     //   var resp = await _http.GetAsync($"http://10.165.1.62/ISAPI/Streaming/channels/1/picture");
       
        var url = "http://10.165.1.62/ISAPI/Streaming/channels/1/picture";

        var resp = await _http.GetAsync(url);
        if (!resp.IsSuccessStatusCode) return false;

        await using var stream = await resp.Content.ReadAsStreamAsync();
        using var img = Image.FromStream(stream);

        img.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
        return true;
    }

  
}
