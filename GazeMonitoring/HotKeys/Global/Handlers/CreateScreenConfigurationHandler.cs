using GazeMonitoring.Messaging;
using GazeMonitoring.Messaging.Messages;

namespace GazeMonitoring.HotKeys.Global.Handlers
{
    public class CreateScreenConfigurationHandler : IGlobalHotKeyHandler
    {
        private readonly IMessenger _messenger;

        public CreateScreenConfigurationHandler(IMessenger messenger)
        {
            _messenger = messenger;
        }

        public void Handle()
        {
            _messenger.Send(new ShowCreateScreenConfigurationMessage());
        }
    }
}
