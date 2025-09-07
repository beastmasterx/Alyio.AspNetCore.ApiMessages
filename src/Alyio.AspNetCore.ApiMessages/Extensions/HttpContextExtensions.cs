// MIT License

#if !NET8_0_OR_GREATER

using System.Diagnostics;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;

namespace Alyio.AspNetCore.ApiMessages;

/// <summary>
/// Extension methods for the <see cref="HttpContext"/>.
/// </summary>
internal static class HttpContextExtensions
{
    private static readonly JsonSerializerOptions s_jsonSerializeOptions = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

    /// <summary>
    /// Writes machine-readable format for specifying errors in HTTP API responses based on https://tools.ietf.org/html/rfc7807.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext"/></param>
    /// <param name="exception">The <see cref="Exception"/></param>
    /// <param name="clearCacheHeaders">Clear Cache-Control, Pragma, Expires and ETag headers. Default is true.</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static Task WriteExceptionAsProblemDetailsAsync(this HttpContext context, Exception exception, bool clearCacheHeaders = true)
    {
        var message = new InternalServerErrorException(exception.Message);
        message.ProblemDetails.Extensions["exceptionType"] = exception.GetType().FullName;

        var errors = new List<string>();
        if (exception is AggregateException aggregateException)
        {
            aggregateException.Flatten().Handle(e =>
            {
                errors.Add(e.Message);
                return true;
            });
            message.ProblemDetails.Extensions["errors"] = errors.Distinct().ToList();
        }

        if (context.RequestServices.GetRequiredService<IHostEnvironment>().IsDevelopment())
        {
            message.ProblemDetails.Detail = exception.ToString();
        }
        else
        {
            message.ProblemDetails.Detail = exception.Message;
        }

        return context.WriteProblemDetailsAsync(message, clearCacheHeaders);
    }

    /// <summary>
    /// Writes machine-readable format for specifying errors in HTTP API responses based on https://tools.ietf.org/html/rfc7807.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext"/></param>
    /// <param name="message">The <see cref="IApiMessage"/></param>
    /// <param name="clearCacheHeaders">Clear Cache-Control, Pragma, Expires and ETag headers. Default is true.</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static Task WriteProblemDetailsAsync(this HttpContext context, IApiMessage message, bool clearCacheHeaders = true)
    {
        ArgumentNullException.ThrowIfNull(context);

        ArgumentNullException.ThrowIfNull(message);

        context.Response.Clear();
        if (clearCacheHeaders)
        {
            context.Response.OnStarting(ClearCacheHeaders, context.Response);
        }

        context.Response.StatusCode = message.ProblemDetails.Status.GetValueOrDefault((int)HttpStatusCode.InternalServerError);
        context.Response.Headers[HeaderNames.ContentType] = "application/problem+json; charset=utf-8";
        message.ProblemDetails.Extensions["traceId"] = Activity.Current?.Id ?? context.TraceIdentifier;
        // https://datatracker.ietf.org/doc/html/rfc7807#section-3
        // When serialized as a JSON document, that format is identified with the "application/problem+json" media type.
        return JsonSerializer.SerializeAsync(
            context.Response.Body,
            message.ProblemDetails,
            message.ProblemDetails.GetType(),
            options: s_jsonSerializeOptions,
            cancellationToken: context.RequestAborted);
    }

    private static Task ClearCacheHeaders(object state)
    {
        var response = (HttpResponse)state;
        response.Headers[HeaderNames.CacheControl] = "no-cache";
        response.Headers[HeaderNames.Pragma] = "no-cache";
        response.Headers[HeaderNames.Expires] = "-1";
        response.Headers.Remove(HeaderNames.ETag);
        return Task.FromResult(0);
    }
}

#endif
