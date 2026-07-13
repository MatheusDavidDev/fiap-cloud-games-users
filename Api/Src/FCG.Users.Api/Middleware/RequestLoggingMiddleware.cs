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
        var method = context.Request.Method;
        var path = context.Request.Path;
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        _logger.LogInformation("HTTP Iniciando: {Method} {Path}", method, path);

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            var statusCode = context.Response.StatusCode;

            _logger.LogInformation(
                "HTTP Finalizado: {Method} {Path} respondeu {StatusCode} em {ElapsedMilliseconds}ms",
                method, path, statusCode, stopwatch.ElapsedMilliseconds);
        }
    }
}
