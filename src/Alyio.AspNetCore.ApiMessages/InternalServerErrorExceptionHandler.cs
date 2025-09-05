// MIT License

#if NET8_0_OR_GREATER

using Alyio.AspNetCore.ApiMessages;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Diagnostics
{
    /// <summary>
    /// Handle a HTTP context exception and write a <see cref="InternalServerErrorException"/> into the <see cref="HttpContext.Response"/>.
    /// </summary>
    public class InternalServerErrorExceptionHandler : IExceptionHandler
    {
        /// <summary>
        /// Writes machine-readable format for specifying errors in HTTP API responses based on https://tools.ietf.org/html/rfc7807.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="exception"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            await httpContext.WriteExceptionAsProblemDetailsAsync(exception);
            return await ValueTask.FromResult(true);
        }
    }

}

#endif
