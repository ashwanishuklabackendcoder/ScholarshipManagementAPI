using ScholarshipManagementAPI.Helper.Utilities;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ScholarshipManagementAPI.Helper.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            await _next(context);

            stopwatch.Stop();

            var endpoint = $"{context.Request.Method} {context.Request.Path}";
            var statusCode = context.Response.StatusCode;
            var duration = stopwatch.ElapsedMilliseconds;

            var requestId = context.Items["RequestId"]?.ToString() ?? "N/A";

            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            var userAgent = context.Request.Headers["User-Agent"].ToString();


            var loginId = context.User?.Claims?.FirstOrDefault(c => c.Type == JwtClaimTypes.LoginId)
                ?.Value ?? "Anonymous";


            // Log the request details to a file
            // requestBody & querParams can be added if needed

            FileLogger.LogRequest(
                requestId,
                endpoint,
                statusCode,
                duration,
                ip,
                loginId,
                userAgent
            );
        }
    }
}
