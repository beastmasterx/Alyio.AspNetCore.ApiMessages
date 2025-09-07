// MIT License

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alyio.AspNetCore.ApiMessages;

/// <summary>
///  Equivalent to HTTP status 404. <see cref="NotFoundException"/> indicates that the requested resource does not exist on the server.
/// </summary>
public sealed class NotFoundException : Exception, IApiMessage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class.
    /// </summary>
    public NotFoundException()
    {
        ProblemDetails = new ProblemDetails
        {
            Title = XMessage.NotFound,
            Status = StatusCodes.Status404NotFound,
            Type = StatusCodeTypes.Status404NotFound,
        };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class with a specified detail message.
    /// </summary>
    /// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
    public NotFoundException(string detail) : this()
    {
        ProblemDetails.Detail = detail;
    }

    /// <inheritdoc />
    public ProblemDetails ProblemDetails { get; }
}
