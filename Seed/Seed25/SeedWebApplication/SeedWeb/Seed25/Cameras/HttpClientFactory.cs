// File: Cameras/HttpClientFactory.cs  (replace entire file with this)
using System;
using System.Net;
using System.Net.Http;

namespace Cameras
{
    internal static class CameraHttp
    {
        internal static HttpClient Create(CameraOptions opt)
        {
            var handler = new HttpClientHandler
            {
                // Let the handler negotiate Digest/Basic based on the 401 challenge.
                Credentials = new NetworkCredential(opt.Username, opt.Password),
                PreAuthenticate = false, // important for Digest
            };

            if (opt.UseHttps && opt.TrustAllCertificates)
            {
                handler.ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            }

            var client = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(opt.TimeoutSeconds)
            };

            // DO NOT set Authorization header manually; let the handler handle it.
            return client;
        }

        internal static Uri BuildBaseUri(CameraOptions opt)
        {
            var scheme = opt.UseHttps ? "https" : "http";
            return new Uri($"{scheme}://{opt.HostOrIp}:{opt.Port}");
        }
    }
}
