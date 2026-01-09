using System.Text.Json;

namespace ProjectManager.Api.Middlewares
{
    public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var correlationId = GetCorrelationId(context);



                using (_logger.BeginScope(new Dictionary<string, object> { ["correlationId"] = correlationId }))
                {
                    _logger.LogError(ex, "Unhandled exception");
                }

                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";
                    var payload = new
                    {
                        correlationId,
                        errorCode = "INTERNAL_ERROR",
                        message = "An unexpected error ocurred",
                        details = (object?)null
                    };
                    var json = JsonSerializer.Serialize(payload, _jsonSerializerOptions);

                    await context.Response.WriteAsync(json);
                }
            }
        }

        public static string GetCorrelationId(HttpContext context)
        {
            return context.Items.TryGetValue(CorrelationIdMiddleware.ItemKey, out var value) &&
                    value is string s &&
                    !string.IsNullOrEmpty(s)
                        ? s
                        : Guid.NewGuid().ToString();
        }
    }
}
