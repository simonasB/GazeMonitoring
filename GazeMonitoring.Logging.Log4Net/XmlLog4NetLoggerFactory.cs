using log4net;
using log4net.Config;

namespace GazeMonitoring.Logging.Log4Net {
    public class XmlLog4NetLoggerFactory : Log4NetLoggerFactory {
        public XmlLog4NetLoggerFactory() {
            if (!LogManager.GetRepository().Configured)
                XmlConfigurator.Configure();
        }
    }
}
