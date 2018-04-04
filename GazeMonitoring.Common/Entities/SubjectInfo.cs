using System;

namespace GazeMonitoring.Common.Entities {
    public class SubjectInfo : IGazeData {
        public int? Age { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public string SessionId { get; set; }
        public DateTime SessionStartTimestamp { get; set; }
        public DateTime SessionEndTimeStamp { get; set; }
        public int? Id { get; set; }
    }
}
