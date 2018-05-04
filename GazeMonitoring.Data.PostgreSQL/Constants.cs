namespace GazeMonitoring.Data.PostgreSQL {
    public static class Constants {
        public const string GazePointsTempCsvFileName = "gaze_point_temp.csv";
        public const string SaccadesTempCsvFileName = "saccades_temp.csv";
        public static readonly string SubjectInfoTableName = "gaze_monitoring.subject_info";
        public static readonly string GazePointTableName = "gaze_monitoring.gaze_point";
        public static readonly string SaccadeTableName = "gaze_monitoring.saccade";
    }
}
