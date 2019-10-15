using Autofac;
using GazeMonitoring.Common.Finalizers;
using GazeMonitoring.Logging;
using Microsoft.Extensions.Configuration;

namespace GazeMonitoring.Data.PostgreSQL {
    public class PostgreSQLModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterType<PostgreSQLFileNameFormatter>().As<IFileNameFormatter>();
            builder.Register(c => new DatabaseRepository(GetPostgreSQLConnectionString(), c.Resolve<ILoggerFactory>()));
            builder.RegisterType<PostgreSQLGazeDataMonitorFinalizer>().As<IGazeDataMonitorFinalizer>();
        }

        private static string GetPostgreSQLConnectionString() {
            var config = new ConfigurationBuilder();

            config.AddJsonFile("config.json");
            var configurationRoot = config.Build();
            return configurationRoot["PostgreSQL"];
        }
    }
}
