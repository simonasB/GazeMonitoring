using Autofac;

namespace GazeMonitoring.Data.Xml {
    public class XmlModule : Module {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<XmlFileNameFormatter>().As<IFileNameFormatter>();
            builder.RegisterType<XmlWritersFactory>().As<IXmlWritersFactory>();
            builder.RegisterType<XmlGazeDataRepositoryFactory>().As<IGazeDataRepositoryFactory>();
        }
    }
}
