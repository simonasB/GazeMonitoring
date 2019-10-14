using GazeMonitoring.Common;
using GazeMonitoring.Model;

namespace GazeMonitoring {
    public class DefaultScreenParameters : IScreenParameters {
        public DefaultScreenParameters() {
            Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            Width = System.Windows.SystemParameters.PrimaryScreenWidth;
        }

        public double Height { get; }
        public double Width { get; }
    }
}
