using System;
using GazeMonitoring.Base;

namespace GazeMonitoring.Unmanaged
{
    public interface IGlobalHotKeyHandlerFactory
    {
        IGlobalHotKeyHandler Create(EGlobalHotKey hotKey);
    }

    public class GlobalHotKeyHandlerFactory : IGlobalHotKeyHandlerFactory
    {
        private readonly IAppLocalContext _appLocalContext;

        public GlobalHotKeyHandlerFactory(IAppLocalContext appLocalContext)
        {
            _appLocalContext = appLocalContext;
        }

        public IGlobalHotKeyHandler Create(EGlobalHotKey hotKey)
        {
            switch (hotKey)
            {
                case EGlobalHotKey.CreateScreenConfiguration:
                    return new CreateScreenConfigurationHandler(_appLocalContext);
                case EGlobalHotKey.EditScreenConfiguration:
                    return new EditScreenConfigurationHandler(_appLocalContext);
                default:
                    throw new ArgumentOutOfRangeException(nameof(hotKey), hotKey, null);
            }
        }
    }
}
