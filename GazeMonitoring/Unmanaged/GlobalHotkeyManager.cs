using System;
using System.Collections.Generic;
using System.Windows.Input;
using GazeMonitoring.DataAccess;

namespace GazeMonitoring.Unmanaged
{
    public interface IGlobalHotKeyManager
    {
        GlobalHotKey Get(EGlobalHotKey key);

        void ChangeGlobalHotKey(EGlobalHotKey eGlobalHotKey, Key key, ModifierKeys keyModifiers);
    }

    public class GlobalHotKeyManager : IGlobalHotKeyManager
    {
        private readonly IGlobalHotKeyHandlerFactory _globalHotKeyHandlerFactory;
        private readonly IConfigurationRepository _configurationRepository;
        private readonly Dictionary<EGlobalHotKey, GlobalHotKey> _globalHotKeys;

        public GlobalHotKeyManager(IGlobalHotKeyHandlerFactory globalHotKeyHandlerFactory, IConfigurationRepository configurationRepository)
        {
            _globalHotKeyHandlerFactory = globalHotKeyHandlerFactory;
            _configurationRepository = configurationRepository;

            _globalHotKeys = new Dictionary<EGlobalHotKey, GlobalHotKey>();
            foreach (var globalHotKeyEntity in _configurationRepository.Search<GlobalHotKeyEntity>())
            {
                _globalHotKeys[globalHotKeyEntity.EGlobalHotKey] = new GlobalHotKey(globalHotKeyEntity.Key, globalHotKeyEntity.KeyModifiers, _globalHotKeyHandlerFactory.Create(globalHotKeyEntity.EGlobalHotKey));
            }
        }

        public void ChangeGlobalHotKey(EGlobalHotKey eGlobalHotKey, Key key, ModifierKeys keyModifiers)
        {
            if (!_globalHotKeys.TryGetValue(eGlobalHotKey, out var globalHotKey))
            {
                throw new ArgumentException("Global key not registered", nameof(eGlobalHotKey));
            }

            globalHotKey.Dispose();

            var hotKeyEntity = _configurationRepository.SearchOne<GlobalHotKeyEntity>(o => o.EGlobalHotKey == eGlobalHotKey);
            hotKeyEntity.Key = key;
            hotKeyEntity.KeyModifiers = keyModifiers;
            _configurationRepository.Update(hotKeyEntity);

            var updatedGlobalHotKey = new GlobalHotKey(key, keyModifiers, _globalHotKeyHandlerFactory.Create(eGlobalHotKey));
            _globalHotKeys[eGlobalHotKey] = updatedGlobalHotKey;
        }

        public GlobalHotKey Get(EGlobalHotKey key)
        {
            if (!_globalHotKeys.TryGetValue(key, out var globalHotKey))
            {
                throw new ArgumentException("Global key not registered", nameof(key));
            }

            return globalHotKey;
        }
    }

    public enum EGlobalHotKey
    {
        CreateScreenConfiguration,
        EditScreenConfiguration
    }
}
