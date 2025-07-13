namespace blogest.api.Middlewares;

public class LoggingRequestMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingRequestMiddleware> _logger;
    public LoggingRequestMiddleware(RequestDelegate next, ILogger<LoggingRequestMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task Invoke(HttpContext context)
    {
        var userName = context.User.Identity?.IsAuthenticated == true
                        ? context.User.Identity.Name
                        : "Anonymous";

        var method = context.Request.Method;
        var path = context.Request.Path;

        _logger.LogInformation("Incoming request: {Method} {Path} by {User}", method, path, userName);
        await _next(context);
    }
}