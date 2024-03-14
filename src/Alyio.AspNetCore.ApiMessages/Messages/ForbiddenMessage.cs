// MIT License

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alyio.AspNetCore.ApiMessages;

/// <summary>
/// Equivalent to HTTP status 403. <see cref="ForbiddenMessage"/> indicates that the server refuses to fulfill the request.
/// </summary>
#pragma warning disable CA1710 // Identifiers should have correct suffix
public sealed class ForbiddenMessage : Exception, IApiMessage
#pragma warning restore CA1710 // Identifiers should have correct suffix
{
    /// <summary>
    /// Initialize a new instance of <see cref="ForbiddenMessage"/> class.
    /// </summary>
    public ForbiddenMessage()
    {
        this.ProblemDetails = new ProblemDetails
        {
            Title = XMessage.Forbidden,
            Status = StatusCodes.Status403Forbidden,
            Type = StatusCodeTypes.Status403Forbidden,
        };
    }

    /// <summary>
    /// Initialize a new instance of <see cref="ForbiddenMessage"/> class.
    /// </summary>
    public ForbiddenMessage(string detail) : this()
    {
        this.ProblemDetails.Detail = detail;
    }

    /// <inheritdoc />
    public ProblemDetails ProblemDetails { get; }
}
