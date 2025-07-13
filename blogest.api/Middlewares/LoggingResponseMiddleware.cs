using System.Diagnostics;

namespace blogest.api.Middlewares;

public class LoggingResponseMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingResponseMiddleware> _logger;
    public LoggingResponseMiddleware(RequestDelegate next, ILogger<LoggingResponseMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task Invoke(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        await _next(context);

        sw.Stop();

        var userName = context.User.Identity?.IsAuthenticated == true
                        ? context.User.Identity.Name
                        : "Anonymous";

        var statusCode = context.Response.StatusCode;
        var path = context.Request.Path;

        var logLevel = statusCode >= 500 ? LogLevel.Error :
                       statusCode >= 400 ? LogLevel.Warning :
                       LogLevel.Information; 

        _logger.Log(logLevel,"Response sent: {StatusCode} for {Path} by {User} in {ElapsedMilliseconds}ms", statusCode, path, userName,sw.ElapsedMilliseconds);
    }
}