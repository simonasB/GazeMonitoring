using System;

namespace GazeMonitoring.Model {
    public class SubjectInfo {
        public int? Age { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public string SessionId { get; set; }
        public DateTime SessionStartTimestamp { get; set; }
        public DateTime SessionEndTimeStamp { get; set; }
        public int? Id { get; set; }
    }
}
