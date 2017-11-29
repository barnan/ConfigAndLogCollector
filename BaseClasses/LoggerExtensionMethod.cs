using NLog;
using System.Runtime.CompilerServices;


namespace BaseClasses
{
    public static class LoggerExtensionMethod
    {
        public static string ErrorLog(this ILogger logger, string message, string className, [CallerMemberName]string methodName = "")
        {
            string outmessage = $"{className}-{methodName}: {message}";
            logger.Error(outmessage);
            return outmessage;
        }

        public static string InfoLog(this ILogger logger, string message, string className, [CallerMemberName]string methodName = "")
        {
            string outmessage = $"{className}-{methodName}: {message}";
            logger.Info(outmessage);
            return outmessage;
        }

        public static string TraceLog(this ILogger logger, string message, string className, [CallerMemberName]string methodName = "")
        {
            string outmessage = $"{className}-{methodName}: {message}";
            logger.Trace(outmessage);
            return outmessage;
        }

    }
}
