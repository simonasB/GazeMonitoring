using GazeMonitoring.Commands;
using GazeMonitoring.Messaging;
using GazeMonitoring.Messaging.Messages;

namespace GazeMonitoring.ViewModels
{
    public class ProfilesViewModel : IMainSubViewModel
    {
        private readonly IMessenger _messenger;
        public EMainSubViewModel SubViewModel => EMainSubViewModel.ProfilesViewModel;

        public ProfilesViewModel(IMessenger messenger)
        {
            _messenger = messenger;
        }

        public RelayCommand BackCommand => new RelayCommand(() =>
        {
            _messenger.Send(new ShowMainNavigationMessage());
        });
    }
}
