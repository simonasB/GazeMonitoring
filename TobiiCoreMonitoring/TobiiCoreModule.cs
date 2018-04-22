using Autofac;
using GazeMonitoring.EyeTracker.Core.Streams;
using Tobii.Interaction;

namespace TobiiCoreMonitoring {
    public class TobiiCoreModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterInstance(new Host());
            builder.RegisterType<TobiiCoreGazePointStreamFactory>().As<IGazePointStreamFactory>();
        }
    }
}
