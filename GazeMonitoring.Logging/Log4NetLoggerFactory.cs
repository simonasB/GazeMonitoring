using System;

namespace GazeMonitoring.Logging {
    public class Log4NetLoggerFactory : ILoggerFactory {
        public ILogger GetLogger(Type type) {
            return new Log4NetLogger(type);
        }
    }
}
