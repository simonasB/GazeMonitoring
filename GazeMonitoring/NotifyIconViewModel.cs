using System;
using System.Windows;
using System.Windows.Input;
using GazeMonitoring.Commands;

namespace GazeMonitoring {
    /// <summary>
    /// Provides bindable properties and commands for the NotifyIcon. In this sample, the
    /// view model is assigned to the NotifyIcon in XAML. Alternatively, the startup routing
    /// in App.xaml.cs could have created this view model, and assigned it to the NotifyIcon.
    /// </summary>
    public class NotifyIconViewModel {
        private readonly MainWindow _mainWindow;

        public NotifyIconViewModel(MainWindow mainWindow) {
            _mainWindow = mainWindow;
        }
        /// <summary>
        /// Shows a window, if none is already open.
        /// </summary>
        public ICommand ShowWindowCommand {
            get
            {
                return new DelegateCommand(
                    () => {
                        Application.Current.MainWindow = _mainWindow;
                        Application.Current.MainWindow.Show();
                        Application.Current.MainWindow.Topmost = true;
                    },
                    () => Application.Current.MainWindow?.IsVisible != true);
            }
        }

        /// <summary>
        /// Hides the main window. This command is only enabled if a window is open.
        /// </summary>
        public ICommand HideWindowCommand {
            get
            {
                return new DelegateCommand(
                    () => Application.Current.MainWindow?.Hide(),
                    () => Application.Current.MainWindow?.IsVisible == true
                );
            }
        }

        /// <summary>
        /// Shuts down the application.
        /// </summary>
        public ICommand ExitApplicationCommand {
            get { return new DelegateCommand(() => Application.Current.Shutdown()); }
        }
    }
}
