namespace ProjectManager.Api.Middlewares
{
    public sealed class CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
    {
        public const string HeaderName = "X-Correlation-Id";
        public const string ItemKey = "CorrelationId";
        private readonly RequestDelegate _next = next;
        private readonly ILogger<CorrelationIdMiddleware> _logger = logger;

        public async Task Invoke(HttpContext context)
        {
            var correlationId = context.Request.Headers.TryGetValue(HeaderName, out var incoming) &&
                                !string.IsNullOrWhiteSpace(incoming.ToString())
                ? incoming.ToString()
                : Guid.NewGuid().ToString("D");
            context.Items[ItemKey] = correlationId;
            context.Response.OnStarting(() =>
            {
                context.Response.Headers[HeaderName] = correlationId;
                return Task.CompletedTask;
            });
            using (_logger.BeginScope(new Dictionary<string, object> { ["correlationId"] = correlationId }))
            {
                await _next(context);
            }
        }
    }
}
