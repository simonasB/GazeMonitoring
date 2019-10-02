﻿using GazeMonitoring.Base;
using GazeMonitoring.Views;

namespace GazeMonitoring.Unmanaged
{
    public class EditScreenConfigurationHandler : IGlobalHotKeyHandler
    {
        private readonly IAppLocalContext _appLocalContext;

        public EditScreenConfigurationHandler(IAppLocalContext appLocalContext)
        {
            _appLocalContext = appLocalContext;
        }

        public void Handle()
        {
            var window = new ScreenConfigurationWindow(_appLocalContext);
            window.Show();
        }
    }
}
