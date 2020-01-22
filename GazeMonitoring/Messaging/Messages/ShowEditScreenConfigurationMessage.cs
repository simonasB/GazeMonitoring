namespace GazeMonitoring.Messaging.Messages
{
    public class ShowEditScreenConfigurationMessage : IMessage
    {
        public string ScreenConfigurationId { get; set; }
    }
}
