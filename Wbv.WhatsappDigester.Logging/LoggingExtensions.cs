using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Wbv.WhatsappDigester.Logging;

public static class LoggingExtensions
{

    public static void Info(this ILogger logger, string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (logger == null || !logger.IsEnabled(LogLevel.Information)) return;

        using (var prop = LogContext.PushProperty("MemberName", memberName))
        {
            LogContext.PushProperty("FileName", GetFileName(sourceFilePath));
            LogContext.PushProperty("LineNumber", sourceLineNumber);

            logger.LogInformation(message);
        }
    }

    public static void Error(this ILogger logger, string message, Exception e,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {

        if (logger == null || !logger.IsEnabled(LogLevel.Error)) return;

        using (var prop = LogContext.PushProperty("MemberName", memberName))
        {
            LogContext.PushProperty("FileName", GetFileName(sourceFilePath));
            LogContext.PushProperty("LineNumber", sourceLineNumber);

            logger.LogError(message, e);
        }
    }

    public static void Debug(this ILogger logger, string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {

        if (logger == null || !logger.IsEnabled(LogLevel.Debug)) return;

        using (var prop = LogContext.PushProperty("MemberName", memberName))
        {
            LogContext.PushProperty("FileName", GetFileName(sourceFilePath));
            LogContext.PushProperty("LineNumber", sourceLineNumber);

            logger.LogError(message);
        }
    }

    public static void RegisterLoggingServices(this IServiceCollection serviceCollection)
    {
        Logger.ConfigureLog();
        serviceCollection.AddLogging(builder => builder.AddSerilog());
    }

    private static string GetFileName(string filePath)
    {
        var path = filePath.Split("\\");
        return path[^1];
    }
}