using System;
using System.IO;

namespace GazeMonitoring.Common
{
    public interface IAppDataHelper
    {
        string GetAppDataDirectoryPath();
    }

    public class AppDataHelper : IAppDataHelper
    {
        public string GetAppDataDirectoryPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GazeMonitoring");
        }
    }
}
