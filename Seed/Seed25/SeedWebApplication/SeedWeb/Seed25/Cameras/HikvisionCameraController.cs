// File: Cameras/HikvisionCameraController.cs
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Cameras
{
    /// <summary>
    /// Hikvision ISAPI implementation.
    /// Preset goto: PUT /ISAPI/PTZCtrl/channels/{channel}/presets/{id}/goto
    /// Snapshot:    GET /ISAPI/Streaming/channels/{channel}/picture (default 101)
    /// </summary>
    public sealed class HikvisionCameraController : ICameraController
    {
        private readonly CameraOptions _opt;
        private readonly HttpClient _http;
        private readonly Uri _base;
        private readonly int _channel;

        public HikvisionCameraController(CameraOptions options)
        {
            _opt = options ?? throw new ArgumentNullException(nameof(options));
            _http = CameraHttp.Create(_opt);
            _base = CameraHttp.BuildBaseUri(_opt);
            _channel = _opt.Channel ?? 101; // 101 is common for main stream channel 1
        }

        public async Task MoveToPresetAsync(int presetNumber, CancellationToken ct = default)
        {
            var url = new Uri(_base, $"/ISAPI/PTZCtrl/channels/1/presets/{presetNumber}/goto");
            using var req = new HttpRequestMessage(HttpMethod.Put, url) { Content = new StringContent(string.Empty) };
            using var res = await _http.SendAsync(req, ct).ConfigureAwait(false);
            res.EnsureSuccessStatusCode();
        }

        public async Task CapturePngAsync(string outputPngFullPath, CancellationToken ct = default)
        {
            // var url = "http://10.165.1.62/ISAPI/Streaming/channels/1/picture";
            var url = new Uri(_base, $"/ISAPI/Streaming/channels/{_channel}/picture");
            using var req = new HttpRequestMessage(HttpMethod.Get, url);
            using var res = await _http.SendAsync(req, ct).ConfigureAwait(false);
            res.EnsureSuccessStatusCode();

            await using var jpgStream = await res.Content.ReadAsStreamAsync(ct).ConfigureAwait(false);
            using var img = Image.FromStream(jpgStream);
            Directory.CreateDirectory(Path.GetDirectoryName(outputPngFullPath)!);
            img.Save(outputPngFullPath, ImageFormat.Png);
        }

        public async Task<MemoryStream> CapturePngStreamAsync(CancellationToken ct = default)
        {
            var url = new Uri(_base, $"/ISAPI/Streaming/channels/{_channel}/picture");
            using var req = new HttpRequestMessage(HttpMethod.Get, url);
            using var res = await _http.SendAsync(req, ct).ConfigureAwait(false);
            res.EnsureSuccessStatusCode();

            await using var jpgStream = await res.Content.ReadAsStreamAsync(ct).ConfigureAwait(false);
            using var img = Image.FromStream(jpgStream);
            var ms = new MemoryStream();
            img.Save(ms, ImageFormat.Png);
            ms.Position = 0; // reset for read
            return ms;
        }

    }
}
