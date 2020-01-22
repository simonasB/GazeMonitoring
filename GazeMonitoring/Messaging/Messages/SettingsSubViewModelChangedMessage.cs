using GazeMonitoring.ViewModels;

namespace GazeMonitoring.Messaging.Messages
{
    public class SettingsSubViewModelChangedMessage : IMessage
    {
        public ESettingsSubViewModel SettingsSubViewModel { get; set; }
    }
}
