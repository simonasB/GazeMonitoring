using GazeMonitoring.Common;

namespace GazeMonitoring {
    public class DefaultScreenParameters : IScreenParameters {
        public DefaultScreenParameters() {
            Height = System.Windows.SystemParameters.WorkArea.Height;
            Width = System.Windows.SystemParameters.WorkArea.Width;
        }

        public double Height { get; }
        public double Width { get; }
    }
}
