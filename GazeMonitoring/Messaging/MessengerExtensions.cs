using GazeMonitoring.Messaging.Messages;

namespace GazeMonitoring.Messaging
{
    public static class MessengerExtensions
    {
        public static void SendIsBusyChanged(this IMessenger messenger, bool isBusy)
        {
            messenger.Send(new IsBusyChangedMessage {IsBusy = isBusy});
        }
    }
}
