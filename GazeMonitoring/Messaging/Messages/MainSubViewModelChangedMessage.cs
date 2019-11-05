using GazeMonitoring.ViewModels;

namespace GazeMonitoring.Messaging.Messages
{
    public class MainSubViewModelChangedMessage : IMessage
    {
        public EMainSubViewModel SubViewModel { get; set; }
    }
}
