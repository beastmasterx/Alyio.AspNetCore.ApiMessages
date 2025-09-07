// MIT License

#if !NET8_0_OR_GREATER

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Alyio.AspNetCore.ApiMessages;

/// <summary>
/// Provides a static method to handle unhandled exceptions within the ASP.NET Core pipeline
/// and write them as Problem Details responses, primarily for use with <see cref="Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware"/>.
/// </summary>
public static class ExceptionHandler
{
    /// <summary>
    /// Handles unhandled exceptions from the HTTP context, writing Problem Details responses to the <see cref="HttpContext.Response"/> for <see cref="Microsoft.AspNetCore.Builder.ExceptionHandlerOptions.ExceptionHandler"/>.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext"/> where the exception occurred.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public static Task WriteUnhandledMessageAsync(HttpContext context)
    {
        var error = context.Features.Get<IExceptionHandlerFeature>()!.Error;
        return context.WriteExceptionAsProblemDetailsAsync(error);
    }
}

#endif
