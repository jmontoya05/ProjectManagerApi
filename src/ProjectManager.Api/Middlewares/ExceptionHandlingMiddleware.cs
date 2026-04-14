using System.Text.Json;
using ProjectManager.Application.Exceptions;
using ApplicationException = ProjectManager.Application.Exceptions.ApplicationException;

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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var correlationId = GetCorrelationId(context);

            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["correlationId"] = correlationId
            }))
            {
                int statusCode;
                string errorCode;
                object? details = null;

                if (exception is ApplicationException appException)
                {
                    statusCode = appException.StatusCode ?? StatusCodes.Status500InternalServerError;
                    errorCode = appException.ErrorCode;
                    details = appException.Details;

                    LogException(_logger, statusCode, appException);
                }
                else
                {
                    statusCode = StatusCodes.Status500InternalServerError;
                    errorCode = "INTERNAL_ERROR";
                    _logger.LogError(exception, "Unhandled exception occurred");
                }

                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = statusCode;
                    context.Response.ContentType = "application/json";

                    var payload = new
                    {
                        correlationId,
                        errorCode,
                        message = exception.Message,
                        details
                    };

                    var json = JsonSerializer.Serialize(payload, _jsonSerializerOptions);
                    await context.Response.WriteAsync(json);
                }
            }
        }

        private static void LogException(
            ILogger<ExceptionHandlingMiddleware> logger,
            int statusCode,
            ApplicationException exception)
        {
            if (statusCode >= 500)
            {
                logger.LogError(exception, "Application error occurred - ErrorCode: {ErrorCode}", exception.ErrorCode);
            }
            else if (statusCode >= 400)
            {
                logger.LogWarning(exception, "Client error - ErrorCode: {ErrorCode}", exception.ErrorCode);
            }
            else
            {
                logger.LogInformation("Request handled - ErrorCode: {ErrorCode}", exception.ErrorCode);
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
