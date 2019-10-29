using GazeMonitoring.Model;

namespace GazeMonitoring.Base {
    public class DefaultScreenParameters : IScreenParameters {
        public DefaultScreenParameters() {
            Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            Width = System.Windows.SystemParameters.PrimaryScreenWidth;
        }

        public double Height { get; }
        public double Width { get; }
    }
}
