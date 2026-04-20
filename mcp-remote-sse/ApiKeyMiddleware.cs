namespace mcp_remote_sse;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly HashSet<string> _validApiKeys;
    private const string ApikeyHeaderName = "X-API-KEY";

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _validApiKeys = configuration.GetSection("ApiKeys").Get<HashSet<string>>()!.ToHashSet();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(ApikeyHeaderName, out var apikey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key not found");
            return;
        }

        if (!_validApiKeys.Contains(apikey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid API Key");
            return;
        }

        await _next(context);
    }
}