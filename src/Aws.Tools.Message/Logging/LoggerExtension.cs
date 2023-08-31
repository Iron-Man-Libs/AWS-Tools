using System;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Aws.Tools.Message.Logging
{
    public static class LoggerExtension
    {
        private static readonly JsonSerializerOptions options = new() { WriteIndented = true };
        public static void LogInformation(this ILogger logger, string message, object data, Exception ex = null)
        {
            logger.LogInformation("Message: {message}\nData: {data}\nException: {exception}", message, JsonSerializer.Serialize(data, options), ex);
        }

        public static void LogError(this ILogger logger, string message, object data, Exception ex = null)
        {
            logger.LogError("Message: {message}\nData: {data}\nException: {exception}", message, JsonSerializer.Serialize(data, options), ex);
        }
    }
}
