// File: Cameras/AxisCameraController.cs
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
    /// Axis VAPIX implementation.
    /// Preset: /axis-cgi/com/ptz.cgi?gotoserverpresetno={n}
    /// Snapshot: /axis-cgi/jpg/image.cgi
    /// </summary>
    public sealed class AxisCameraController : ICameraController
    {
        private readonly CameraOptions _opt;
        private readonly HttpClient _http;
        private readonly Uri _base;

        public AxisCameraController(CameraOptions options)
        {
            _opt = options ?? throw new ArgumentNullException(nameof(options));
            _http = CameraHttp.Create(_opt);
            _base = CameraHttp.BuildBaseUri(_opt);
        }

        public async Task MoveToPresetAsync(int presetNumber, CancellationToken ct = default)
        {
            // Many Axis devices accept gotoserverpresetno (numeric) or ...name (string).
            var url = new Uri(_base, $"/axis-cgi/com/ptz.cgi?gotoserverpresetno={presetNumber}");
            using var req = new HttpRequestMessage(HttpMethod.Get, url);
            using var res = await _http.SendAsync(req, ct).ConfigureAwait(false);
            res.EnsureSuccessStatusCode();
        }

        public async Task CapturePngAsync(string outputPngFullPath, CancellationToken ct = default)
        {
            // Axis typically returns JPEG; we’ll convert to PNG on save.
            var url = new Uri(_base, "/axis-cgi/jpg/image.cgi");
            using var req = new HttpRequestMessage(HttpMethod.Get, url);
            using var res = await _http.SendAsync(req, ct).ConfigureAwait(false);
            res.EnsureSuccessStatusCode();

            await using var jpgStream = await res.Content.ReadAsStreamAsync(ct).ConfigureAwait(false);
            using var img = Image.FromStream(jpgStream); // requires System.Drawing.Common (Windows)
            Directory.CreateDirectory(Path.GetDirectoryName(outputPngFullPath)!);
            img.Save(outputPngFullPath, ImageFormat.Png);
        }

        public async Task<MemoryStream> CapturePngStreamAsync(CancellationToken ct = default)
        {
            var url = new Uri(_base, "/axis-cgi/jpg/image.cgi"); // Axis
                                                                 // or Hikvision: $"/ISAPI/Streaming/channels/{_channel}/picture"

            using var req = new HttpRequestMessage(HttpMethod.Get, url);
            using var res = await _http.SendAsync(req, ct).ConfigureAwait(false);
            res.EnsureSuccessStatusCode();

            await using var jpgStream = await res.Content.ReadAsStreamAsync(ct);
            using var img = Image.FromStream(jpgStream);

            var ms = new MemoryStream();
            img.Save(ms, ImageFormat.Png);
            ms.Position = 0; // reset for read
            return ms;
        }

    }
}
