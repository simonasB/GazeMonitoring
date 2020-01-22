using GazeMonitoring.Model;

namespace GazeMonitoring.Messaging.Messages
{
    public class AddScreenConfigurationWithHotKeyMessage : IMessage
    {
        public AddScreenConfigurationWithHotKeyMessage(ScreenConfiguration screenConfiguration)
        {
            ScreenConfiguration = screenConfiguration;
        }

        public ScreenConfiguration ScreenConfiguration { get; set; }
    }
}
