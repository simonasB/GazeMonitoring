using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace GazeMonitoring.Unmanaged
{
    public interface IGlobalHotKeyManager
    {
        void ChangeGlobalHotKey(EGlobalHotKey eGlobalHotKey, Key key, ModifierKeys keyModifiers);
    }

    public class GlobalHotKeyManager : IGlobalHotKeyManager
    {
        private readonly IGlobalHotKeyHandlerFactory _globalHotKeyHandlerFactory;
        private readonly Dictionary<EGlobalHotKey, GlobalHotKey> _globalHotKeys;

        public GlobalHotKeyManager(IGlobalHotKeyHandlerFactory globalHotKeyHandlerFactory)
        {
            _globalHotKeyHandlerFactory = globalHotKeyHandlerFactory;
            _globalHotKeys = new Dictionary<EGlobalHotKey, GlobalHotKey>
            {
                {EGlobalHotKey.CreateScreenConfiguration, new GlobalHotKey(Key.F6, ModifierKeys.None, _globalHotKeyHandlerFactory.Create(EGlobalHotKey.CreateScreenConfiguration)) },
                {EGlobalHotKey.EditScreenConfiguration, new GlobalHotKey(Key.F7, ModifierKeys.None, _globalHotKeyHandlerFactory.Create(EGlobalHotKey.EditScreenConfiguration)) }
            };
        }

        public void ChangeGlobalHotKey(EGlobalHotKey eGlobalHotKey, Key key, ModifierKeys keyModifiers)
        {
            if (!_globalHotKeys.TryGetValue(eGlobalHotKey, out var globalHotKey))
            {
                throw new ArgumentException("Global key not registered", nameof(eGlobalHotKey));
            }

            globalHotKey.Dispose();

            var updatedGlobalHotKey = new GlobalHotKey(key, keyModifiers, _globalHotKeyHandlerFactory.Create(eGlobalHotKey));
            _globalHotKeys[eGlobalHotKey] = updatedGlobalHotKey;
        }
    }

    public enum EGlobalHotKey
    {
        CreateScreenConfiguration,
        EditScreenConfiguration
    }
}
