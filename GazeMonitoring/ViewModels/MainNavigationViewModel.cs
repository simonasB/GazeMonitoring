using GazeMonitoring.Base;
using GazeMonitoring.Messaging;
using GazeMonitoring.Messaging.Messages;
using System.Windows.Controls;
using GazeMonitoring.Commands;

namespace GazeMonitoring.ViewModels
{
    public class MainNavigationViewModel : ViewModelBase, IMainSubViewModel
    {
        private readonly IMessenger _messenger;

        public EMainSubViewModel SubViewModel => EMainSubViewModel.MainNavigationViewModel;

        public MainNavigationViewModel(IMessenger messenger)
        {
            _messenger = messenger;
        }

        public RelayCommand<ListBoxItem> MenuItemClicked => new RelayCommand<ListBoxItem>((o) =>
        {
            switch (o.Name)
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
        });
    }
}
