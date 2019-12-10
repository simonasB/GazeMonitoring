using System;
using System.IO;

namespace GazeMonitoring.Data
{
    public interface ITempDataConfiguration
    {
        string FolderPath { get; }
        string GazePointsFilePath { get; }
        string FixationPointsFilePath { get; }
        string SaccadesFilePath { get; }
    }

    public class TempDataConfiguration : ITempDataConfiguration
    {
        public string FolderPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "GazeMonitoringTmp");

        public string GazePointsFilePath => Path.Combine(FolderPath, "gazepoints_tmp");

        public string FixationPointsFilePath => Path.Combine(FolderPath, "fixationpoints_tmp");

        public string SaccadesFilePath => Path.Combine(FolderPath, "saccades_tmp");
    }
}
