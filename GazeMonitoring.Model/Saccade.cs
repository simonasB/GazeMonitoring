namespace GazeMonitoring.Model {
    public class Saccade {
        /// <summary>
        /// kampas tarp dviejų gretimų fixation location
        /// </summary>
        public double Direction { get; set; }
        /// <summary>
        /// atstumas tarp dviejų gretimų fixation location
        /// </summary>
        public double Amplitude { get; set; }
        /// <summary>
        /// Amplitude / time
        /// </summary>
        public double Velocity { get; set; }

        public double StartTimeStamp { get; set; }
        public double EndTimeStamp { get; set; }
    }
}
