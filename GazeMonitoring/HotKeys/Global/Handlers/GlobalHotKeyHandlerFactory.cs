using System;
using GazeMonitoring.Messaging;

namespace GazeMonitoring.HotKeys.Global.Handlers
{
    public interface IGlobalHotKeyHandlerFactory
    {
        IGlobalHotKeyHandler Create(EGlobalHotKey hotKey);
    }

    public class GlobalHotKeyHandlerFactory : IGlobalHotKeyHandlerFactory
    {
        private readonly IMessenger _messenger;

        public GlobalHotKeyHandlerFactory(IMessenger messenger)
        {
            _messenger = messenger;
        }

        public IGlobalHotKeyHandler Create(EGlobalHotKey hotKey)
        {
            switch (hotKey)
            {
                case EGlobalHotKey.CreateScreenConfiguration:
                    return new CreateScreenConfigurationHandler(_messenger);
                case EGlobalHotKey.EditScreenConfiguration:
                    return new EditScreenConfigurationHandler(_messenger);
                default:
                    throw new ArgumentOutOfRangeException(nameof(hotKey), hotKey, null);
            }
        }
    }
}
