// MIT License

#if !NET8_0_OR_GREATER

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Alyio.AspNetCore.ApiMessages;

internal sealed class ApiMessageHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiMessageHandlerMiddleware> _logger;

    /// <summary>
    /// Initialize a new instance of <see cref="ApiMessageHandlerMiddleware"/> class.
    /// </summary>
    public ApiMessageHandlerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _next = next;
        _logger = loggerFactory.CreateLogger<ApiMessageHandlerMiddleware>();
    }

    /// <summary>
    /// Processes unhandled exception and write <see cref="Microsoft.AspNetCore.Mvc.ProblemDetails"/> to the current <see cref="HttpContext.Response"/>.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext"/>.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception ex)
        {
            if (ex is IApiMessage message)
            {
                // We can't do anything if the response has already started, just abort.
                if (context.Response.HasStarted)
                {
                    Log.ResponseAlreadyStarted(_logger);
                    throw;
                }
                await context.WriteProblemDetailsAsync(message);
                return;
            }
            throw; // Re-throw the original if we couldn't handle it
        }
    }
}

#endif