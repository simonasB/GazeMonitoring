using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace GazeMonitoring.Unmanaged
{
    public class GlobalHotKeyManager
    {
        private readonly IGlobalHotKeyHandlerFactory _globalHotKeyHandlerFactory;
        private Dictionary<EGlobalHotKey, GlobalHotKey> _globalHotKeys;

        public GlobalHotKeyManager(IGlobalHotKeyHandlerFactory globalHotKeyHandlerFactory)
        {
            _globalHotKeyHandlerFactory = globalHotKeyHandlerFactory;
            _globalHotKeys = new Dictionary<EGlobalHotKey, GlobalHotKey>();
        }

        public GlobalHotKeyManager(Dictionary<EGlobalHotKey, GlobalHotKey> globalHotKeys)
        {
            _globalHotKeys = globalHotKeys;
        }

        public void ChangeGlobalHotKey(EGlobalHotKey eGlobalHotKey, Key key, KeyModifier keyModifiers)
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
