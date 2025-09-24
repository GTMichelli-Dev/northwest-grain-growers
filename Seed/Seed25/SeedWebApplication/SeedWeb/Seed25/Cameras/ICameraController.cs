// File: Cameras/ICameraController.cs
using System.Threading;
using System.Threading.Tasks;

namespace Cameras
{
    public interface ICameraController
    {
        /// <summary>
        /// Moves the camera to a preset location.
        /// </summary>
        Task MoveToPresetAsync(int presetNumber, CancellationToken ct = default);

        /// <summary>
        /// Captures a snapshot and saves it as PNG to <paramref name="outputPngFullPath"/>.
        /// The camera may return JPEG; this method converts to PNG.
        /// </summary>
        Task CapturePngAsync(string outputPngFullPath, CancellationToken ct = default);
    }
}
