using Autofac;
using GazeMonitoring.Common;

namespace TheEyeTribeMonitoring {
    public class EyeTribeModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterType<EyeTribeGazePointStreamFactory>().As<IGazePointStreamFactory>();
        }
    }
}
