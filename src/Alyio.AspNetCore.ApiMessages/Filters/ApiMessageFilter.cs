// MIT License

#if NET8_0_OR_GREATER

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Alyio.AspNetCore.ApiMessages.Filters;

internal sealed class ApiMessageFilter : IEndpointFilter
{
    private readonly ILogger<ApiMessageFilter> _logger;

    public ApiMessageFilter(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ApiMessageFilter>();
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        try
        {
            return await next(context);
        }
        catch (Exception ex)
        {
            if (ex is IApiMessage message)
            {
                if (context.HttpContext.Response.HasStarted)
                {
                    Log.ResponseAlreadyStarted(_logger);
                    throw; // Re-throw if response has started
                }
                await context.HttpContext.WriteProblemDetailsAsync(message);
                return Results.Empty;
            }
            throw; // Re-throw if not IApiMessage
        }
    }
}

#endif
