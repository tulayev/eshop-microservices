using NLog;
using System.Reflection;

namespace BuildingBlocks.Behaviors.Logging
{
    public static class NLogConfigurator
    {
        public static void ConfigureForMicroservice()
        {
            var baseDir = AppContext.BaseDirectory;

            // Go up to the solution src folder
            var rootLogDir = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", "Logs"));

            // Detect service name from entry assembly (e.g., Ordering.API)
            var entryAssembly = Assembly.GetEntryAssembly();
            var serviceName = entryAssembly?.GetName().Name ?? "UnknownService";

            var fullLogDir = Path.Combine(rootLogDir, serviceName);

            Directory.CreateDirectory(fullLogDir);

            var config = LogManager.Configuration;
            config.Variables["logDir"] = fullLogDir;

            LogManager.ReconfigExistingLoggers();
        }
    }
}
