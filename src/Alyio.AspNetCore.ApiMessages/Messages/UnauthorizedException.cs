// MIT License

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alyio.AspNetCore.ApiMessages;

/// <summary>
/// Equivalent to HTTP status 401. <see cref="UnauthorizedException"/> indicates
/// that the requested resource requires authentication. The WWW-Authenticate header
/// contains the details of how to perform the authentication.
/// </summary>
public sealed class UnauthorizedException : Exception, IApiMessage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnauthorizedException"/> class.
    /// </summary>
    public UnauthorizedException()
    {
        ProblemDetails = new ProblemDetails
        {
            Title = XMessage.Unauthorized,
            Status = StatusCodes.Status401Unauthorized,
            Type = StatusCodeTypes.Status401Unauthorized,
        };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnauthorizedException"/> class with a specified detail message.
    /// </summary>
    /// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
    public UnauthorizedException(string detail) : this()
    {
        ProblemDetails.Detail = detail;
    }

    /// <inheritdoc />
    public ProblemDetails ProblemDetails { get; }
}

