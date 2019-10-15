using Autofac;

namespace GazeMonitoring.ScreenCapture.SharpAvi {
    public class SharpAviModule : Module {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AviVideoStreamFactory>().As<IAviVideoStreamFactory>();
            builder.RegisterType<SharpAviRecorder>().As<IScreenRecorder>();
        }
    }
}
