﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using GazeMonitoring.Base;
using GazeMonitoring.Common;
using GazeMonitoring.DataAccess;
using GazeMonitoring.HotKeys.Global;

namespace GazeMonitoring.Seeding
{
    public interface IDatabaseSeeder
    {
        void Seed();
    }

    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IAppDataHelper _appDataHelper;

        public DatabaseSeeder(IConfigurationRepository configurationRepository, IAppDataHelper appDataHelper)
        {
            _configurationRepository = configurationRepository;
            _appDataHelper = appDataHelper;
        }

        public void Seed()
        {
            SeedHotKeys();
            SeedAppLocalContext();
        }

        private void SeedHotKeys()
        {
            var keysToAdd = new List<GlobalHotKeyEntity>();
            var keyEntities = _configurationRepository.Search<GlobalHotKeyEntity>().ToList();

            if (keyEntities.FirstOrDefault(o => o.EGlobalHotKey == EGlobalHotKey.CreateScreenConfiguration) == null)
            {
                keysToAdd.Add(new GlobalHotKeyEntity(EGlobalHotKey.CreateScreenConfiguration, Key.F6, ModifierKeys.None));
            }

            if (keyEntities.FirstOrDefault(o => o.EGlobalHotKey == EGlobalHotKey.EditScreenConfiguration) == null)
            {
                keysToAdd.Add(new GlobalHotKeyEntity(EGlobalHotKey.EditScreenConfiguration, Key.F7, ModifierKeys.None));
            }

            if (keysToAdd.Count > 0)
                _configurationRepository.SaveMany(keysToAdd);
        }

        private void SeedAppLocalContext()
        {
            var appLocalContext = _configurationRepository.Search<AppLocalContext>().FirstOrDefault();
            if (appLocalContext == null)
            {
                _configurationRepository.Save(new AppLocalContext
                {
                    DataFilesPath = _appDataHelper.GetAppDataDirectoryPath()
                });
            }
        }
    }
}
