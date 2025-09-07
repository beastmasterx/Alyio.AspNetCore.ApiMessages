// MIT License

using Microsoft.AspNetCore.Mvc;

namespace Alyio.AspNetCore.ApiMessages;

/// <summary>
/// Represents an API message that can be converted into Problem Details.
/// </summary>
public interface IApiMessage
{
    /// <summary>
    /// Gets the <see cref="ProblemDetails"/>.
    /// </summary>
    ProblemDetails ProblemDetails { get; }
}
