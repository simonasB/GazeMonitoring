using System.Linq;
using Autofac;
using Autofac.Core;
using GazeMonitoring.Common.Finalizers;
using GazeMonitoring.Common.Misc;
using GazeMonitoring.Logging;
using GazeMonitoring.Model;
using Microsoft.Extensions.Configuration;

namespace GazeMonitoring.Data.PostgreSQL {
    public class PostgreSQLModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterType<PostgreSQLFileNameFormatter>().As<IFileNameFormatter>();
            builder.Register((c, p) => {
                var parameters = p as Parameter[] ?? p.ToArray();
                var loggerFactory = c.Resolve<ILoggerFactory>();
                return new PostgreSQLGazeDataMonitorFinalizer(new DatabaseRepository(GetPostgreSQLConnectionString(), loggerFactory),
                    parameters.Named<SubjectInfo>(GazeMonitoring.Common.Constants.SubjectInfoParameterName), c.Resolve<ILoggerFactory>());
            }).As<IGazeDataMonitorFinalizer>();
        }

        private static string GetPostgreSQLConnectionString() {
            var config = new ConfigurationBuilder();

            config.AddJsonFile("config.json");
            var configurationRoot = config.Build();
            return configurationRoot["PostgreSQL"];
        }
    }
}
