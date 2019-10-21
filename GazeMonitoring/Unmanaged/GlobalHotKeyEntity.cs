using System.Windows.Input;

namespace GazeMonitoring.Unmanaged
{
    public class GlobalHotKeyEntity
    {
        public GlobalHotKeyEntity()
        {
            
        }

        public GlobalHotKeyEntity(EGlobalHotKey eGlobalHotKey, Key key, ModifierKeys keyModifiers)
        {
            EGlobalHotKey = eGlobalHotKey;
            Key = key;
            KeyModifiers = keyModifiers;
        }

        public int Id { get; set; }
        public EGlobalHotKey EGlobalHotKey { get; set; }
        public Key Key { get; set; }
        public ModifierKeys KeyModifiers { get; set; }
    }
}
