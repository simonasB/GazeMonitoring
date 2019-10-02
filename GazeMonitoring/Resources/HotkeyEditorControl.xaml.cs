using System.Windows;
using System.Windows.Input;
using GazeMonitoring.Base;
using GazeMonitoring.Extensions;

namespace GazeMonitoring.Resources
{
    /// <summary>
    /// Interaction logic for HotkeyEditorControl.xaml
    /// </summary>
    public partial class HotkeyEditorControl
    {
        public static readonly DependencyProperty HotkeyProperty =
            DependencyProperty.Register(nameof(Hotkey), typeof(Hotkey), typeof(HotkeyEditorControl),
                new FrameworkPropertyMetadata(default(Hotkey), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Hotkey Hotkey
        {
            get => (Hotkey)GetValue(HotkeyProperty);
            set => SetValue(HotkeyProperty, value);
        }

        public HotkeyEditorControl()
        {
            InitializeComponent();
        }

        private void HotkeyTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Don't let the event pass further
            // because we don't want standard textbox shortcuts working
            e.Handled = true;

            // Get modifiers and key data
            var modifiers = Keyboard.Modifiers;
            var key = e.Key;

            // When Alt is pressed, SystemKey is used instead
            if (key == Key.System)
            {
                key = e.SystemKey;
            }

            // Pressing delete, backspace or escape without modifiers clears the current value
            if (modifiers == ModifierKeys.None && key.IsEither(Key.Delete, Key.Back, Key.Escape))
            {
                Hotkey = null;
                return;
            }

            // If no actual key was pressed - return
            if (key.IsEither(
                Key.LeftCtrl, Key.RightCtrl, Key.LeftAlt, Key.RightAlt,
                Key.LeftShift, Key.RightShift, Key.LWin, Key.RWin,
                Key.Clear, Key.OemClear, Key.Apps))
            {
                return;
            }

            // Set values
            Hotkey = new Hotkey(key, modifiers);
        }
    }
}
