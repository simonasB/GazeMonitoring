using System;

namespace GazeMonitoring.Logging {
    public interface ILoggerFactory {
        ILogger GetLogger(Type type);
    }
}
