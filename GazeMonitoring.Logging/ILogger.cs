namespace GazeMonitoring.Logging {
    public interface ILogger {
        void Debug(object message);
        void Information(object message);
        void Warning(object message);
        void Error(object message);
        void Fatal(object message);
        bool IsDebugEnabled { get; }
        bool IsInformationEnabled { get; }
        bool IsWarningEnabled { get; }
        bool IsErrorEnabled { get; }
        bool IsFatalEnabled { get; }
    }
}
