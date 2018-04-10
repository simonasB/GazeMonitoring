namespace GazeMonitoring.Logging {
    public interface ILogger {
        void Debug(object message);
        void DebugFormat(string message, params object[] args);
        void Information(object message);
        void InformationFormat(string message, params object[] args);
        void Warning(object message);
        void WarningFormat(string message, params object[] args);
        void Error(object message);
        void ErrorFormat(string message, params object[] args);
        void Fatal(object message);
        void FatalFormat(string message, params object[] args);
        bool IsDebugEnabled { get; }
        bool IsInformationEnabled { get; }
        bool IsWarningEnabled { get; }
        bool IsErrorEnabled { get; }
        bool IsFatalEnabled { get; }
    }
}
