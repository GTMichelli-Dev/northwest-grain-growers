// File: Cameras/CameraControllerFactory.cs
using System;

namespace Cameras
{
    public static class CameraControllerFactory
    {
        public static ICameraController Create(CameraVendor vendor, CameraOptions options) =>
            vendor switch
            {
                CameraVendor.Axis => new AxisCameraController(options),
                CameraVendor.Hikvision => new HikvisionCameraController(options),
                _ => throw new NotSupportedException($"Unsupported vendor: {vendor}")
            };
    }
}
