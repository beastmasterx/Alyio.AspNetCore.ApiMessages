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
    /// Initialize a new instance of <see cref="UnauthorizedException"/> class.
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
    /// Initialize a new instance of <see cref="UnauthorizedException"/> class.
    /// </summary>
    public UnauthorizedException(string detail) : this()
    {
        ProblemDetails.Detail = detail;
    }

    /// <inheritdoc />
    public ProblemDetails ProblemDetails { get; }
}

