using GazeMonitoring.Messaging;
using GazeMonitoring.Messaging.Messages;

namespace GazeMonitoring.HotKeys.Global.Handlers
{
    public class EditScreenConfigurationHandler : IGlobalHotKeyHandler
    {
        private readonly IMessenger _messenger;

        public EditScreenConfigurationHandler(IMessenger messenger)
        {
            _messenger = messenger;
        }

        public void Handle()
        {
            _messenger.Send(new ShowEditScreenConfigurationMessage());
        }
    }
}
