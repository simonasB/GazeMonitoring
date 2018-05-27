using CsvHelper.Configuration;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Csv {
    public class SubjectInfoMap : ClassMap<SubjectInfo> {
        public SubjectInfoMap() {
            AutoMap();
            Map(m => m.Id).Ignore();
            Map(m => m.SessionEndTimeStamp).Ignore();
            Map(m => m.SessionId).Ignore();
            Map(m => m.SessionStartTimestamp).Ignore();
        }
    }
}
