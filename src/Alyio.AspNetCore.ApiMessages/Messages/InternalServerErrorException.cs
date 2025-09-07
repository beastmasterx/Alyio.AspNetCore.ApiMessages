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
    /// Initializes a new instance of the <see cref="InternalServerErrorException"/> class.
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
    /// Initializes a new instance of the <see cref="InternalServerErrorException"/> class with a specified detail message.
    /// </summary>
    /// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
    public InternalServerErrorException(string detail) : this()
    {
        ProblemDetails.Detail = detail;
    }

    /// <inheritdoc />
    public ProblemDetails ProblemDetails { get; }
}
