﻿using System.Globalization;
using System.IO;
using GazeMonitoring.Base;
using GazeMonitoring.Common;
using GazeMonitoring.Logging;
using GazeMonitoring.Model;

namespace GazeMonitoring.IO
{
    public interface IDataFolderManager
    {
        string GetDataFilesPath(SubjectInfo subjectInfo, bool isAnonymous);
    }

    public class DataFolderManager : IDataFolderManager
    {
        private readonly IAppLocalContextManager _appLocalContextManager;
        private readonly IAppDataHelper _appDataHelper;
        private readonly ILogger _logger;

        public DataFolderManager(IAppLocalContextManager appLocalContextManager, IAppDataHelper appDataHelper, ILoggerFactory loggerFactory)
        {
            _appLocalContextManager = appLocalContextManager;
            _appDataHelper = appDataHelper;
            _logger = loggerFactory.GetLogger(typeof(DataFolderManager));
        }

        public string GetDataFilesPath(SubjectInfo subjectInfo, bool isAnonymous)
        {
            var dataFolderName = subjectInfo.SessionStartTimestamp.ToString("yyyy_MM_dd_HH_mm_ss_fff", CultureInfo.InvariantCulture);

            if (!isAnonymous)
            {
                dataFolderName = $"{subjectInfo.Name}_{subjectInfo.Age}_{subjectInfo.Details}:{dataFolderName}";
            }

            var rootDataFilesPath = _appLocalContextManager.Get().DataFilesPath;
            string dataFilesPath;
            if (!Directory.Exists(rootDataFilesPath))
            {
                _logger.Warning($"Configured data files path folder does not exist: Path: {rootDataFilesPath}. Need to reconfigure");
                dataFilesPath = Path.Combine(_appDataHelper.GetAppDataDirectoryPath(), dataFolderName);
            }
            else
            {
                dataFilesPath = Path.Combine(_appLocalContextManager.Get().DataFilesPath, dataFolderName);
            }

            if (!Directory.Exists(dataFilesPath))
                Directory.CreateDirectory(dataFilesPath);

            return dataFilesPath;
        }
    }
}
