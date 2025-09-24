// File: Cameras/CameraService.cs
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Cameras
{
    public static class CameraService
    {
        /// <summary>
        /// One-call helper: move to preset and capture a PNG to the provided folder/file.
        /// </summary>
        public static async Task MoveAndShootAsync(
            CameraVendor vendor,
            string ipAddress,
            string username,
            string password,
            int presetNumber,
            string outputFolderPath,
            string fileNameWithoutExtension,
            bool https = false,
            int port = 0,
            int? channel = null,
            CancellationToken ct = default)
        {
            var opts = new CameraOptions
            {
                HostOrIp = ipAddress,
                Username = username,
                Password = password,
                UseHttps = https,
                Port = port == 0 ? (https ? 443 : 80) : port,
                Channel = channel
            };

            var controller = CameraControllerFactory.Create(vendor, opts);

            await controller.MoveToPresetAsync(presetNumber, ct);
            await Task.Delay(5000);
            var fullPath = Path.Combine(outputFolderPath, $"{fileNameWithoutExtension}.png");
            await controller.CapturePngAsync(fullPath, ct);
        }


        public static async Task<MemoryStream> MoveAndShootToStreamAsync(
    CameraVendor vendor,
    string ipAddress,
    string username,
    string password,
    int presetNumber,
    bool https = false,
    int port = 0,
    int? channel = null,
    CancellationToken ct = default)
        {
            var opts = new CameraOptions
            {
                HostOrIp = ipAddress,
                Username = username,
                Password = password,
                UseHttps = https,
                Port = port == 0 ? (https ? 443 : 80) : port,
                Channel = channel
            };

            var controller = CameraControllerFactory.Create(vendor, opts);

            // Move PTZ
            await controller.MoveToPresetAsync(presetNumber, ct);
            await Task.Delay(5000);
            // Instead of saving to file, grab snapshot directly
            if (controller is AxisCameraController axis)
                return await axis.CapturePngStreamAsync(ct);

            if (controller is HikvisionCameraController hik)
                return await hik.CapturePngStreamAsync(ct);

            throw new NotSupportedException("Unsupported vendor stream capture");
        }

    }
}
