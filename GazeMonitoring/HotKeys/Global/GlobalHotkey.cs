using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows.Interop;
using GazeMonitoring.HotKeys.Global.Handlers;

namespace GazeMonitoring.HotKeys.Global
{
    public class GlobalHotKey : IDisposable
    {
        private static Dictionary<int, GlobalHotKey> _dictHotKeyToCalBackProc;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, UInt32 fsModifiers, UInt32 vlc);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public const int WmHotKey = 0x0312;

        private bool _disposed = false;

        public Key Key { get; private set; }
        public ModifierKeys KeyModifiers { get; private set; }
        public Action Handler { get; private set; }
        public int Id { get; set; }

        public GlobalHotKey(Key k, ModifierKeys keyModifiers, IGlobalHotKeyHandler handler, bool register = true) : this(k, keyModifiers, handler.Handle, register)
        {

        }

        public GlobalHotKey(Key k, ModifierKeys keyModifiers, Action handler, bool register = true)
        {
            Key = k;
            KeyModifiers = keyModifiers;
            Handler = handler;
            if (register)
            {
                Register();
            }
        }

        public bool Register()
        {
            int virtualKeyCode = KeyInterop.VirtualKeyFromKey(Key);
            Id = virtualKeyCode + ((int)KeyModifiers * 0x10000);
            bool result = RegisterHotKey(IntPtr.Zero, Id, (UInt32)KeyModifiers, (UInt32)virtualKeyCode);

            if (_dictHotKeyToCalBackProc == null)
            {
                _dictHotKeyToCalBackProc = new Dictionary<int, GlobalHotKey>();
                ComponentDispatcher.ThreadFilterMessage += ComponentDispatcherThreadFilterMessage;
            }

            _dictHotKeyToCalBackProc.Add(Id, this);

            Debug.Print(result.ToString() + ", " + Id + ", " + virtualKeyCode);
            return result;
        }

        public void Unregister()
        {
            if (_dictHotKeyToCalBackProc.TryGetValue(Id, out _))
            {
                UnregisterHotKey(IntPtr.Zero, Id);
            }
        }

        private static void ComponentDispatcherThreadFilterMessage(ref MSG msg, ref bool handled)
        {
            if (!handled && msg.message == WmHotKey && _dictHotKeyToCalBackProc.TryGetValue((int) msg.wParam, out var globalHotkey))
            {
                globalHotkey.Handler.Invoke();
                handled = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed) return;

            if (disposing)
            {
                Unregister();
            }

            _disposed = true;
        }
    }
}
