using NLog;

namespace BuildingBlocks.Behaviors.Logging
{
    public sealed class NLogAdapter : IAppLogger
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void LogInformation(string message, params object[] args)
        {
            _logger.Info(message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            _logger.Warn(message, args);
        }
    }
}
