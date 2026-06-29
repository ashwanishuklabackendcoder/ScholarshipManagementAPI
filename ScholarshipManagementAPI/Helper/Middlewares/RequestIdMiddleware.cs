namespace ScholarshipManagementAPI.Helper.Middlewares
{
    public class RequestIdMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var requestId = Guid.NewGuid().ToString("N").Substring(0, 8);

            context.Items["RequestId"] = requestId;

            await _next(context);
        }
    }
}
