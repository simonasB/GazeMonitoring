using GazeMonitoring.Common.Finalizers;
using GazeMonitoring.IoC;
using GazeMonitoring.Logging;
using Microsoft.Extensions.Configuration;

namespace GazeMonitoring.Data.PostgreSQL {
    public class PostgreSQLModule : IoCModule {
        private static string GetPostgreSQLConnectionString() {
            var config = new ConfigurationBuilder();

            config.AddJsonFile("config.json");
            var configurationRoot = config.Build();
            return configurationRoot["PostgreSQL"];
        }

        public void Load(IoContainerBuilder builder)
        {
            builder.Register<IFileNameFormatter, PostgreSQLFileNameFormatter>();
            builder.Register<IDatabaseRepository>(c => new DatabaseRepository(GetPostgreSQLConnectionString(), c.GetInstance<ILoggerFactory>()), Scope.Singleton);
            builder.Register<IGazeDataMonitorFinalizer, PostgreSQLGazeDataMonitorFinalizer>();
        }
    }
}
