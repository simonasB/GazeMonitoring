using System.Linq;
using Autofac;
using Autofac.Core;
using GazeMonitoring.Common;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Xml {
    public class XmlModule : Module {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<XmlFileNameFormatter>().As<IFileNameFormatter>();
            builder.RegisterType(typeof(XmlWritersFactory));
            builder.Register((c, p) => new XmlWritersFactory(c.Resolve<IFileNameFormatter>(), p.Named<SubjectInfo>(Constants.SubjectInfoParameterName)));
            builder.Register((c, p) => {
                    var parameters = p as Parameter[] ?? p.ToArray();
                    return new XmlGazeDataRepository(
                        c.Resolve<XmlWritersFactory>(new NamedParameter(Constants.SubjectInfoParameterName, parameters.Named<SubjectInfo>(Constants.SubjectInfoParameterName))),
                        parameters.Named<DataStream>(Constants.DataStreamParameterName));
                })
                .As<IGazeDataRepository>();
        }
    }
}
