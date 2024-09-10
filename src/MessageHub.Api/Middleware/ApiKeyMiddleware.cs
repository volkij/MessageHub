using MessageHub.Core.Config;
using Microsoft.Extensions.Options;

namespace MessageHub.Api.Middleware
{
    internal class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AccountConfig _accountConfig;
        public ApiKeyMiddleware(RequestDelegate next, IOptions<AccountConfig> accountConfig)
        {
            _next = next;
            _accountConfig = accountConfig.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                if (!context.Request.Headers.TryGetValue("X-API-KEY", out var extractedApiKey))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("API Key was not provided.");
                    return;
                }

                var apiKey = _accountConfig.Accounts.FirstOrDefault(k => k.ApiKey == extractedApiKey);

                if (apiKey == null)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Unauthorized client.");
                    return;
                }
            }

            await _next(context);
        }
    }
}