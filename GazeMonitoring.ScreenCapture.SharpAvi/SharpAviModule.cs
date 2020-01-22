using GazeMonitoring.IoC;

namespace GazeMonitoring.ScreenCapture.SharpAvi {
    public class SharpAviModule : IoCModule {
        public void Load(IoContainerBuilder builder)
        {
            builder.Register<IAviVideoStreamFactory, AviVideoStreamFactory>();
            builder.Register<IScreenRecorder, SharpAviRecorder>();
        }
    }
}
