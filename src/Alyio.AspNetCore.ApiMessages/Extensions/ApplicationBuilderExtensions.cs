// MIT License

#if !NET8_0_OR_GREATER

using Microsoft.AspNetCore.Builder;

namespace Alyio.AspNetCore.ApiMessages;

/// <summary>
/// Extension methods for <see cref="IApplicationBuilder"/>  to add ApiMessages to the request execution pipeline
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Adds API message handling middleware to the <see cref="IApplicationBuilder"/>'s request pipeline, catching <see cref="IApiMessage"/> exceptions and writing Problem Details to the HTTP response.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> so that additional calls can be chained.</returns>
    public static IApplicationBuilder UseApiMessage(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ApiMessageHandlerMiddleware>();
    }
}

#endif
