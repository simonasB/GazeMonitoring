using System;
using System.IO;

namespace GazeMonitoring.IO
{
    public interface IFileSystemHelper
    {
        string GetAppDataDirectoryPath();
    }

    public class FileSystemHelper : IFileSystemHelper
    {
        public string GetAppDataDirectoryPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GazeMonitoring");
        }
    }
}
