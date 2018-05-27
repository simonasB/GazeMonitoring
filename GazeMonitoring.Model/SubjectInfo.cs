using System;
using System.Xml.Serialization;

namespace GazeMonitoring.Model {
    public class SubjectInfo {
        public int? Age { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        [XmlIgnore]
        public string SessionId { get; set; }
        [XmlIgnore]
        public DateTime SessionStartTimestamp { get; set; }
        [XmlIgnore]
        public DateTime SessionEndTimeStamp { get; set; }
        [XmlIgnore]
        public int? Id { get; set; }
    }
}
