using CsvHelper.Configuration;
using Tobii.Interaction;

namespace TobiiCoreMonitoring.Csv {
    public class GazePointDataMap : ClassMap<GazePointData> {
        public GazePointDataMap() {
            Map(m => m.X).Name(GazePointDataColumns.X);
            Map(m => m.Y).Name(GazePointDataColumns.Y);
            Map(m => m.Timestamp).Name(GazePointDataColumns.TimeStamp);
        }

        private static class GazePointDataColumns {
            public const string X = "x";
            public const string Y = "y";
            public const string TimeStamp = "timeStamp";
        }
    }
}
