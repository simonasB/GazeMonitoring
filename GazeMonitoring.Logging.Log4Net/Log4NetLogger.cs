using System;
using log4net;

namespace GazeMonitoring.Logging.Log4Net {
    public class Log4NetLogger : ILogger {
        private readonly ILog _logger;

        public Log4NetLogger(Type type) {
            _logger = LogManager.GetLogger(type);
        }

        public void Debug(object message) {
            _logger.Debug(message);
        }

        public void DebugFormat(string message, params object[] args) {
            _logger.DebugFormat(message, args);
        }

        public void Information(object message) {
            _logger.Info(message);
        }

        public void InformationFormat(string message, params object[] args) {
            _logger.InfoFormat(message, args);
        }

        public void Warning(object message) {
            _logger.Warn(message);
        }

        public void WarningFormat(string message, params object[] args) {
            _logger.WarnFormat(message, args);
        }

        public void Error(object message) {
            _logger.Error(message);
        }

        public void ErrorFormat(string message, params object[] args) {
            _logger.ErrorFormat(message, args);
        }

        public void Fatal(object message) {
            _logger.Fatal(message);
        }

        public void FatalFormat(string message, params object[] args) {
            _logger.FatalFormat(message, args);
        }

        public bool IsDebugEnabled => _logger.IsDebugEnabled;
        public bool IsInformationEnabled => _logger.IsInfoEnabled;
        public bool IsWarningEnabled => _logger.IsWarnEnabled;
        public bool IsErrorEnabled => _logger.IsErrorEnabled;
        public bool IsFatalEnabled => _logger.IsFatalEnabled;
    }
}
