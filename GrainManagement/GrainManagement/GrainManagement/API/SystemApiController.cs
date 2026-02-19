using GrainManagement.Controllers;
using GrainManagement.Models;
using GrainManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

namespace GrainManagement.API
{



    [Route("api/[controller]")]
    [UseAdminConnection]
    [ApiController]
    public class SystemApiController : ControllerBase
    {

        private static Server _serverInfo;
        private readonly dbContext _ctx;
        private readonly ILogger<SystemApiController> _logger;
        private readonly IServerInfoProvider _serverInfoProvider;
        private readonly IWebHostEnvironment _env;
        private readonly IDeviceContext _deviceContext;

        public SystemApiController(dbContext ctx, ILogger<SystemApiController> logger , IServerInfoProvider serverInfoProvider, IWebHostEnvironment env, IDeviceContext deviceContext)
        {
            _ctx = ctx;
            _logger = logger;
            _serverInfoProvider = serverInfoProvider;
            _env = env;
            _deviceContext = deviceContext;
        }


        [HttpGet("/debug/wwwroot")]
        public IActionResult WwwrootPath()
        {
            return Ok(new
            {
                ContentRoot = _env.ContentRootPath,
                WebRoot = _env.WebRootPath
            });
        }

        [HttpGet("GetServerInfo")]
        public async Task<IActionResult> GetServerInfo(CancellationToken ct)
        {
            try
            {
                var server = await _serverInfoProvider.GetAsync(ct);
                return Ok(server);
            }
            catch
            {
                return BadRequest(new { message = "Server Not Configured." });
            }
            
        }

        [HttpGet("GetDeviceId")]
        public async Task<IActionResult> GetDeviceId(CancellationToken ct)
        {
            try
            {
                var deviceId = _deviceContext.DeviceId??"";
                return Ok(deviceId);
            }
            catch
            {
                return BadRequest(new { message = "Device Not Configured." });
            }

        }

        
        public class SystemInfo
        {
            public string ApplicationName { get; set; }
            public string Version { get; set; }
            public string FileVersion { get; set; }
            public string BuildDate { get; set; }
            public Server ServerInfo { get; set; }
        }
       


        [HttpGet("GetSystemInfo")]
        public async Task<IActionResult> GetSystemInfo(CancellationToken ct)
        {
            try
            {
                var server = await _serverInfoProvider.GetAsync(ct);
                var assembly = typeof(SystemApiController).Assembly;
                var appName = assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? "Unknown";
                var version = assembly.GetName().Version?.ToString() ?? "unknown";
                var fileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;
                var buildDate = System.IO.File.GetLastWriteTimeUtc(assembly.Location).ToString("yyyy-MM-dd");

                var versionInfo = new
                {
                    ApplicationName = appName,
                    Version = version,
                    FileVersion = fileVersion,
                    BuildDate = buildDate
                };
                return Ok(new SystemInfo
                {
                    ApplicationName=versionInfo.ApplicationName,
                    Version=versionInfo.Version,
                    FileVersion=versionInfo.FileVersion,
                    BuildDate=versionInfo.BuildDate,
                    ServerInfo= server
                });
            }
            catch
            {
                return BadRequest(new { message = "Server Not Configured." });
            }

        }




        [HttpGet("GetVersion")]
        public IActionResult GetVersion()
        {
            var assembly = typeof(SystemApiController).Assembly;
            var appName = assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? "Unknown";
            var version = assembly.GetName().Version?.ToString() ?? "unknown";
            var fileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;
            var buildDate = System.IO.File.GetLastWriteTimeUtc(assembly.Location).ToString("yyyy-MM-dd");

            var versionInfo = new
            {
                ApplicationName = appName,
                Version = version,
                FileVersion = fileVersion,
                BuildDate = buildDate
            };
            return Ok(versionInfo);
        }
    }
}
