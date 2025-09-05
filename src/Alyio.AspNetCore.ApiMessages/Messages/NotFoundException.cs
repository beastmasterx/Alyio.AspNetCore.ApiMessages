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
    /// Initialize a new instance of <see cref="NotFoundException"/> class.
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
    /// Initialize a new instance of <see cref="NotFoundException"/> class.
    /// </summary>
    public NotFoundException(string detail) : this()
    {
        ProblemDetails.Detail = detail;
    }

    /// <inheritdoc />
    public ProblemDetails ProblemDetails { get; }
}
