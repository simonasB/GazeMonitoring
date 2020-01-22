namespace GazeMonitoring.Messaging.Messages
{
    public class IsBusyChangedMessage : IMessage
    {
        public bool IsBusy { get; set; }
    }
}
