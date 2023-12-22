namespace Microsoft.AspNetCore.Http;

public static class HttpContextExtensions
{
    public static string? GetClientIpAddress(this HttpContext httpContext)
    {
        string? ipAddress = null;

        if (httpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var xForwardedFor))
        {
            ipAddress = xForwardedFor.FirstOrDefault()?.Trim();
        }

        if (string.IsNullOrEmpty(ipAddress))
        {
            ipAddress = httpContext.Connection?.RemoteIpAddress?.ToString();
        }

        if (string.IsNullOrEmpty(ipAddress))
        {
            if (httpContext.Request.Headers.TryGetValue("REMOTE_ADDR", out var remoteAddr))
                ipAddress = remoteAddr;
        }

        return ipAddress;
    }
}
