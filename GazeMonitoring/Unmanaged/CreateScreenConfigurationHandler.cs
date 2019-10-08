﻿using GazeMonitoring.Base;
using GazeMonitoring.DataAccess.LiteDB;
using GazeMonitoring.Views;

namespace GazeMonitoring.Unmanaged
{
    public class CreateScreenConfigurationHandler : IGlobalHotKeyHandler
    {
        private readonly IAppLocalContext _appLocalContext;

        public CreateScreenConfigurationHandler(IAppLocalContext appLocalContext)
        {
            _appLocalContext = appLocalContext;
        }

        public void Handle()
        {
            var window = new ScreenConfigurationWindow(_appLocalContext, new LiteDBConfigurationRepository());
            window.Show();
        }
    }
}
