using Serilog;

namespace eCommerce.SharedLibrary.Logs
{
    public static class LogExceptions
    {
        public static void LogException(Exception ex) 
        {
            Console.WriteLine(ex.ToString());
            LogToFile(ex.Message);
            LogToConsole(ex.Message);
            LogToDebugger(ex.Message);
        }

        public static void LogToFile(string message) => Log.Information(message);
        public static void LogToConsole(string message) => Log.Warning(message);
        public static void LogToDebugger(string message) => Log.Debug(message);
    }
}
