using GazeMonitoring.Base;
using GazeMonitoring.Messaging;
using GazeMonitoring.Messaging.Messages;
using System.Windows.Controls;

namespace GazeMonitoring.ViewModels
{
    public class MainNavigationViewModel : ViewModelBase, IMainSubViewModel
    {
        private readonly IMessenger _messenger;
        private ListBoxItem _selectedMenuItem;

        public EMainSubViewModel SubViewModel => EMainSubViewModel.MainNavigationViewModel;

        public MainNavigationViewModel(IMessenger messenger)
        {
            _messenger = messenger;
        }

        public ListBoxItem SelectedMenuItem
        {
            get => _selectedMenuItem;
            set
            {
                switch (value.Name)
                {
                    case "Settings":
                        _messenger.Send(new ShowSettingsMessage());
                        break;
                    case "StartNewSession":
                        _messenger.Send(new ShowStartNewSessionMessage());
                        break;
                    case "Profiles":
                        _messenger.Send(new ShowProfilesMessage());
                        break;
                }
                SetProperty(ref _selectedMenuItem, value);
            }
        }
    }
}
