using System;
using GazeMonitoring.Base;
using GazeMonitoring.DataAccess;

namespace GazeMonitoring.Unmanaged
{
    public interface IGlobalHotKeyHandlerFactory
    {
        IGlobalHotKeyHandler Create(EGlobalHotKey hotKey);
    }

    public class GlobalHotKeyHandlerFactory : IGlobalHotKeyHandlerFactory
    {
        private readonly IAppLocalContextManager _appLocalContextManager;
        private readonly IConfigurationRepository _configurationRepository;

        public GlobalHotKeyHandlerFactory(IAppLocalContextManager appLocalContextManager, IConfigurationRepository configurationRepository)
        {
            _appLocalContextManager = appLocalContextManager;
            _configurationRepository = configurationRepository;
        }

        public IGlobalHotKeyHandler Create(EGlobalHotKey hotKey)
        {
            switch (hotKey)
            {
                case EGlobalHotKey.CreateScreenConfiguration:
                    return new CreateScreenConfigurationHandler(_appLocalContextManager, _configurationRepository);
                case EGlobalHotKey.EditScreenConfiguration:
                    return new EditScreenConfigurationHandler(_appLocalContextManager, _configurationRepository);
                default:
                    throw new ArgumentOutOfRangeException(nameof(hotKey), hotKey, null);
            }
        }
    }
}
