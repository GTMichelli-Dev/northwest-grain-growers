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
  

    [HttpPost("gotoPreset/{presetId}")]
    public async Task<bool> GoToPresetAsync(int presetId)
    {
            HttpClient _http;
    var handler = new HttpClientHandler
        {
            Credentials = new NetworkCredential("admin", "Scale_Us3r"),
            PreAuthenticate = true
        };
        _http = new HttpClient(handler); //{ BaseAddress = new Uri($"http://{"10.165.1.62"}") };
        var url = $"/ISAPI/PTZCtrl/channels/1/presets/{presetId}/goto";
        var resp = await _http.PutAsync($"http://10.165.1.62/ISAPI/PTZCtrl/channels/1/presets/{presetId}/goto", new StringContent(""));
        return resp.IsSuccessStatusCode;
    }

    public class CameraRequest
    {
        /// <example>10.165.1.62</example>
        public string Ip { get; set; } = default!;
        /// <example>admin</example>
        public string Username { get; set; } = default!;
        /// <example>Scale_Us3r</example>
        public string Password { get; set; } = default!;
        /// <example>1</example>
        public int PresetId { get; set; }
        /// <summary>Optional: channel number (default 1)</summary>
        /// <example>1</example>
        public int Channel { get; set; } = 1;
        /// <summary>Optional delay (ms) after goto before snapshot</summary>
        /// <example>800</example>
        public int PostGotoDelayMs { get; set; } = 800;
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


    [HttpGet("testpictureOverlay")]
    public async Task<IActionResult> testpictureOverlay()
    {
        var baseMs = await Cameras.CameraService.MoveAndShootToStreamAsync(
            vendor: Cameras.CameraVendor.Hikvision,
            ipAddress: "10.165.1.62",
            username: "admin",
            password: "Scale_Us3r",
            presetNumber: 1,
            https: false,
            port: 80,
            channel: 1);




        var dto = new InboundTicketDTO
        {
            UID = Guid.NewGuid(),
            Ticket = 1234567890,
            Weight = 128200,
            TimeIn = DateTime.Now,
            Plant = "Wasco Seed Plant.",
            Phone = "541-442-5555 ",
            Prompt1 = "Goto Office",
            Prompt2 = "Bring Ticket"
        };

        XtraReport report = new InboundTicket(dto);

        using (var overlayMs = new MemoryStream())
        {
            await report.ExportToImageAsync(overlayMs);
      

           ImageComposer.OverlayImageAnchored(
           baseImageStream: baseMs,
           overlayImageStream: overlayMs,
           outputPngPath: @"C:\Temp\1234567890_Inbound.png",
           anchorX: AnchorX.Left,
           anchorY: AnchorY.Top,
           marginX: 10,
           marginY: 10,
           width: 600,          // height auto to preserve aspect
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
