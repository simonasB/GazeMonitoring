using GazeMonitoring.IoC;

namespace GazeMonitoring.Data.Xml {
    public class XmlModule : IoCModule {
        public void Load(IoContainerBuilder builder)
        {
            builder.Register<IFileNameFormatter, XmlFileNameFormatter>();
            builder.Register<IXmlWritersFactory, XmlWritersFactory>();
            builder.Register<IGazeDataRepositoryFactory, XmlGazeDataRepositoryFactory>();
        }
    }
}
