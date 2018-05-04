using System;

namespace GazeMonitoring.Data.PostgreSQL {
    public class DbSaccade {
        public double Amplitude { get; set; }

        public double Direction { get; set; }

        public double Velocity { get; set; }

        public long StartTimeStamp { get; set; }

        public long EndTimeStamp { get; set; }

        public string SessionId { get; set; }

        public int SubjectInfoId { get; set; }

        public DateTime SampleTime { get; set; }
    }
}
