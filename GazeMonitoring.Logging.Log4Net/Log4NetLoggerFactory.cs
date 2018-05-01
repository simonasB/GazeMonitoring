using System;

namespace GazeMonitoring.Logging.Log4Net {
    public class Log4NetLoggerFactory : ILoggerFactory {
        public ILogger GetLogger(Type type) {
            return new Log4NetLogger(type);
        }
    }
}
