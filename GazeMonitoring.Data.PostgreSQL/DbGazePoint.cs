using System;

namespace GazeMonitoring.Data.PostgreSQL {
    public class DbGazePoint {
        public double X { get; set; }

        public double Y { get; set; }

        public long Timestamp { get; set; }

        public string SessionId { get; set; }

        public int SubjectInfoId { get; set; }

        public DateTime SampleTime { get; set; }
    }
}
