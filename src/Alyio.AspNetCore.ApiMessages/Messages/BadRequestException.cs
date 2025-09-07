// MIT License

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Alyio.AspNetCore.ApiMessages;

/// <summary>
/// Equivalent to HTTP status 400. <see cref="BadRequestException"/> indicates
/// that the request could not be understood by the server. <see cref="BadRequestException"/>
/// is sent when no other error is applicable, or if the exact error is unknown or
/// does not have its own error code.
/// </summary>
public sealed class BadRequestException : Exception, IApiMessage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestException"/> class with a default 'ValidationFailed' message.
    /// </summary>
    public BadRequestException()
    {
        ProblemDetails = new ProblemDetails
        {
            Title = XMessage.ValidationFailed,
            Status = StatusCodes.Status400BadRequest,
            Type = StatusCodeTypes.Status400BadRequest,
        };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestException"/> class with a specified detail message.
    /// </summary>
    /// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
    public BadRequestException(string detail) : this()
    {
        ProblemDetails.Detail = detail;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestException"/> class with a specified detail message and a collection of errors.
    /// </summary>
    /// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
    /// <param name="errors">A collection of error messages.</param>
    public BadRequestException(string detail, params string[] errors) : this(detail)
    {
        ProblemDetails.Extensions["errors"] = errors;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestException"/> class with a <see cref="ModelStateDictionary"/>.
    /// </summary>
    /// <param name="modelState">The <see cref="ModelStateDictionary"/> containing validation errors.</param>
    public BadRequestException(ModelStateDictionary modelState) : this(null!, modelState)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestException"/> class with a specified detail message and a <see cref="ModelStateDictionary"/>.
    /// </summary>
    /// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
    /// <param name="modelState">The <see cref="ModelStateDictionary"/> containing validation errors.</param>
    public BadRequestException(string detail, ModelStateDictionary modelState)
    {
        ProblemDetails = new ValidationProblemDetails(modelState)
        {
            Title = XMessage.ValidationFailed,
            Status = StatusCodes.Status400BadRequest,
            Type = StatusCodeTypes.Status400BadRequest,
        };

        if (detail != null)
        {
            ProblemDetails.Detail = detail;
        }
    }

    /// <inheritdoc />
    public ProblemDetails ProblemDetails { get; private set; }
}
