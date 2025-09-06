using Microsoft.Extensions.Logging;

namespace Alyio.AspNetCore.ApiMessages;

internal static partial class Log
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Warning,
        Message = "The response has already started, the API message handler will not be executed.")]
    internal static partial void ResponseAlreadyStarted(ILogger logger);
}
