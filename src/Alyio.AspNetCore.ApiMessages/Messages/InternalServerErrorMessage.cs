// MIT License

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alyio.AspNetCore.ApiMessages;

/// <summary>
/// Equivalent to HTTP status 500. <see cref="InternalServerErrorMessage"/> indicates that a generic error has occurred on the server.
/// </summary>
#pragma warning disable CA1710 // Identifiers should have correct suffix
public class InternalServerErrorMessage : Exception, IApiMessage
#pragma warning restore CA1710 // Identifiers should have correct suffix
{
    /// <summary>
    /// Initialize a new instance of <see cref="InternalServerErrorMessage"/> class.
    /// </summary>
    public InternalServerErrorMessage()
    {
        this.ProblemDetails = new ProblemDetails
        {
            Title = XMessage.InternalServerError,
            Status = StatusCodes.Status500InternalServerError,
            Type = StatusCodeTypes.Status500InternalServerError,
        };
    }

    /// <summary>
    /// Initialize a new instance of <see cref="InternalServerErrorMessage"/> class.
    /// </summary>
    public InternalServerErrorMessage(string detail) : this()
    {
        this.ProblemDetails.Detail = detail;
    }

    /// <inheritdoc />
    public ProblemDetails ProblemDetails { get; }
}
