using System.Linq;
using Autofac;
using Autofac.Core;
using GazeMonitoring.Common;
using GazeMonitoring.EyeTracker.Core.Streams;
using GazeMonitoring.Model;

namespace GazeMonitoring.ScreenCapture.SharpAvi {
    public class SharpAviModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder.Register((c, p) => {
                var parameters = p as Parameter[] ?? p.ToArray();
                var dataStream = parameters.Named<DataStream>(Constants.DataStreamParameterName);
                var recorderParams = parameters.Named<RecorderParams>(Constants.RecorderParamsParameterName);
                return new SharpAviRecorder(recorderParams, new AviVideoStreamFactory(),
                    c.Resolve<GazePointStream>(new NamedParameter(Constants.DataStreamParameterName, dataStream)));
            }).As<IScreenRecorder>();
        }
    }
}
