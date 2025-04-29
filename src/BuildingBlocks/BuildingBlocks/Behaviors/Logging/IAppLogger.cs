namespace BuildingBlocks.Behaviors.Logging
{
    public interface IAppLogger
    {
        void LogInformation(string message, params object[] args);
        void LogWarning(string message, params object[] args);
    }
}
