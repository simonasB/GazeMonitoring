using System.IO;
using GazeMonitoring.Common;

namespace GazeMonitoring.Data
{
    public interface ITempDataConfiguration
    {
        string GazePointsFilePath { get; }
        string FixationPointsFilePath { get; }
        string SaccadesFilePath { get; }
    }

    public class TempDataConfiguration : ITempDataConfiguration
    {
        private readonly string _applicationDataPath;

        public TempDataConfiguration(IAppDataHelper appDataHelper)
        {
            _applicationDataPath = appDataHelper.GetAppDataDirectoryPath();
        }

        public string GazePointsFilePath => Path.Combine(_applicationDataPath, "gazepoints_tmp");

        public string FixationPointsFilePath => Path.Combine(_applicationDataPath, "fixationpoints_tmp");

        public string SaccadesFilePath => Path.Combine(_applicationDataPath, "saccades_tmp");
    }
}
