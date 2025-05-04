using Serilog;

namespace TestProject1.Core.Logging
{
    internal static class SerilogConfiguration
    {
        public static void Configure()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("Logs/apitest.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
    }
}
