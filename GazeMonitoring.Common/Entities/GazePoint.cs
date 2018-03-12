namespace GazeMonitoring.Common.Entities {
    public class GazePoint : IGazeData {
        public double X { get; set; }
        public double Y { get; set; }
        public double Timestamp { get; set; }
    }
}
