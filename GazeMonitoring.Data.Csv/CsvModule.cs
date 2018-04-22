using System.Linq;
using Autofac;
using Autofac.Core;
using GazeMonitoring.Common;
using GazeMonitoring.Common.Misc;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Csv {
    public class CsvModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterType<CsvFileNameFormatter>().As<IFileNameFormatter>();
            builder.RegisterType(typeof(CsvWritersFactory));
            builder.Register((c, p) => new CsvWritersFactory(c.Resolve<IFileNameFormatter>(), p.Named<SubjectInfo>(Constants.SubjectInfoParameterName)));
            builder.Register((c, p) => {
                    var parameters = p as Parameter[] ?? p.ToArray();
                    return new CsvGazeDataRepository(
                        c.Resolve<CsvWritersFactory>(new NamedParameter(Constants.SubjectInfoParameterName, parameters.Named<SubjectInfo>(Constants.SubjectInfoParameterName))),
                        parameters.Named<DataStream>(Constants.DataStreamParameterName));
                })
                .As<IGazeDataRepository>();
        }
    }
}
