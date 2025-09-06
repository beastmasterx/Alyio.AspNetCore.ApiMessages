// MIT License

namespace Alyio.AspNetCore.ApiMessages
{
    /// <summary>
    /// A collection of constants for
    /// <see href="https://datatracker.ietf.org/doc/html/rfc9110">HTTP Semantics (RFC 9110)</see >.
    /// </summary>
    internal static class StatusCodeTypes
    {
        /// <summary>
        /// HTTP status code 400.
        /// </summary>
        public const string Status400BadRequest = "https://tools.ietf.org/html/rfc9110#section-15.5.1";

        /// <summary>
        /// HTTP status code 401.
        /// </summary>
        public const string Status401Unauthorized = "https://tools.ietf.org/html/rfc9110#section-15.5.2";

        /// <summary>
        /// HTTP status code 403.
        /// </summary>
        public const string Status403Forbidden = "https://tools.ietf.org/html/rfc9110#section-15.5.4";

        /// <summary>
        /// HTTP status code 404.
        /// </summary>
        public const string Status404NotFound = "https://tools.ietf.org/html/rfc9110#section-15.5.5";

        /// <summary>
        /// HTTP status code 500.
        /// </summary>
        public const string Status500InternalServerError = "https://tools.ietf.org/html/rfc9110#section-15.6.1";
    }
}
