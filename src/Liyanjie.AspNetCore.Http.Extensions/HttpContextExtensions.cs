﻿using System.Linq;

using Microsoft.AspNetCore.Http;

namespace Liyanjie.AspNetCore.Http.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetClientIpAddress(this HttpContext httpContext)
        {
            string ipAddress = null;

            if (httpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var xForwardedFor))
                ipAddress = ((string)xForwardedFor).Split(',')
                    .Select(_ => _.Trim())
                    .FirstOrDefault();

            if (string.IsNullOrEmpty(ipAddress))
                ipAddress = httpContext.Connection?.RemoteIpAddress?.ToString();

            if (string.IsNullOrEmpty(ipAddress))
                if (httpContext.Request.Headers.TryGetValue("REMOTE_ADDR", out var remoteAddr))
                    ipAddress = remoteAddr;

            return ipAddress;
        }
    }
}
