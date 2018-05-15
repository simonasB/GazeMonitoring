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

        public void Information(object message) {
            _logger.Info(message);
        }

        public void Warning(object message) {
            _logger.Warn(message);
        }

        public void Error(object message) {
            _logger.Error(message);
        }

        public void Fatal(object message) {
            _logger.Fatal(message);
        }

        public bool IsDebugEnabled => _logger.IsDebugEnabled;
        public bool IsInformationEnabled => _logger.IsInfoEnabled;
        public bool IsWarningEnabled => _logger.IsWarnEnabled;
        public bool IsErrorEnabled => _logger.IsErrorEnabled;
        public bool IsFatalEnabled => _logger.IsFatalEnabled;
    }
}
