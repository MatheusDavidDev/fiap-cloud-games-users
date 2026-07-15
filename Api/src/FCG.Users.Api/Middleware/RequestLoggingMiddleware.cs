using System.Diagnostics;

namespace FCG.Users.Api.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        if (!context.Request.Headers.TryGetValue("X-Correlation-Id", out var correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
        }
        context.Response.Headers.Append("X-Correlation-Id", correlationId);

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();

            var httpMetadata = new
            {
                method = context.Request.Method,
                url = context.Request.Path + context.Request.QueryString,
                status_code = context.Response.StatusCode,
                latency_ms = stopwatch.Elapsed.TotalMilliseconds,
                client_ip = context.Connection.RemoteIpAddress?.ToString()
            };

            var message = $"HTTP {httpMetadata.method} {httpMetadata.url} finalizado com status {httpMetadata.status_code} em {httpMetadata.latency_ms:F2}ms";

            if (context.Response.StatusCode >= 500)
            {
                _logger.LogError("{@Http} | CorrelationId: {CorrelationId} | Message: {Message}", httpMetadata, correlationId.ToString(), message);
            }
            else if (context.Response.StatusCode >= 400)
            {
                _logger.LogWarning("{@Http} | CorrelationId: {CorrelationId} | Message: {Message}", httpMetadata, correlationId.ToString(), message);
            }
            else
            {
                _logger.LogInformation("{@Http} | CorrelationId: {CorrelationId} | Message: {Message}", httpMetadata, correlationId.ToString(), message);
            }
        }
    }
}

