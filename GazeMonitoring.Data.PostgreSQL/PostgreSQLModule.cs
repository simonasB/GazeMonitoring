using System.Linq;
using Autofac;
using Autofac.Core;
using GazeMonitoring.Common;
using GazeMonitoring.Common.Entities;
using Microsoft.Extensions.Configuration;

namespace GazeMonitoring.Data.PostgreSQL {
    public class PostgreSQLModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterType<PostgreSQLFileNameFormatter>().As<IFileNameFormatter>();
            builder.Register((c, p) => {
                var parameters = p as Parameter[] ?? p.ToArray();
                return new PostgreSQLGazeDataMonitorFinalizer(new DatabaseRepository(GetPostgreSQLConnectionString()),
                    parameters.Named<SubjectInfo>(GazeMonitoring.Common.Constants.SubjectInfoParameterName));
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
