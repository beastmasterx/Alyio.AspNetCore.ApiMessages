// MIT License

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alyio.AspNetCore.ApiMessages;

/// <summary>
/// Equivalent to HTTP status 403. <see cref="ForbiddenException"/> indicates that the server refuses to fulfill the request.
/// </summary>
public sealed class ForbiddenException : Exception, IApiMessage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ForbiddenException"/> class.
    /// </summary>
    public ForbiddenException()
    {
        ProblemDetails = new ProblemDetails
        {
            Title = XMessage.Forbidden,
            Status = StatusCodes.Status403Forbidden,
            Type = StatusCodeTypes.Status403Forbidden,
        };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ForbiddenException"/> class with a specified detail message.
    /// </summary>
    /// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
    public ForbiddenException(string detail) : this()
    {
        ProblemDetails.Detail = detail;
    }

    /// <inheritdoc />
    public ProblemDetails ProblemDetails { get; }
}
