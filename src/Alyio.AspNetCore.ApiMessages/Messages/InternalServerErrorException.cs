// MIT License

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alyio.AspNetCore.ApiMessages;

/// <summary>
/// Equivalent to HTTP status 500. <see cref="InternalServerErrorException"/> indicates that a generic error has occurred on the server.
/// </summary>
public sealed class InternalServerErrorException : Exception, IApiMessage
{
    /// <summary>
    /// Initialize a new instance of <see cref="InternalServerErrorException"/> class.
    /// </summary>
    public InternalServerErrorException()
    {
        ProblemDetails = new ProblemDetails
        {
            Title = XMessage.InternalServerError,
            Status = StatusCodes.Status500InternalServerError,
            Type = StatusCodeTypes.Status500InternalServerError,
        };
    }

    /// <summary>
    /// Initialize a new instance of <see cref="InternalServerErrorException"/> class.
    /// </summary>
    public InternalServerErrorException(string detail) : this()
    {
        ProblemDetails.Detail = detail;
    }

    /// <inheritdoc />
    public ProblemDetails ProblemDetails { get; }
}
