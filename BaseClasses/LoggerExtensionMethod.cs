using NLog;
using System.Runtime.CompilerServices;


namespace BaseClasses
{
    public static class LoggerExtensionMethod
    {
        public static void ErrorLog(this ILogger logger, string message, string className, [CallerMemberName]string methodName = "")
        {
            logger.Error($"{className}-{methodName}: {message}");
        }

        public static void InfoLog(this ILogger logger, string message, string className, [CallerMemberName]string methodName = "")
        {
            logger.Info($"{className}-{methodName}: {message}");
        }

        public static void TraceLog(this ILogger logger, string message, string className, [CallerMemberName]string methodName = "")
        {
            logger.Trace($"{className}-{methodName}: {message}");
        }

    }
}
