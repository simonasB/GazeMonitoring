using Hardcodet.Wpf.TaskbarNotification;

namespace GazeMonitoring.Balloon {
    public class BalloonService : IBalloonService
    {
        private readonly TaskbarIcon _taskbarIcon;

        public BalloonService(TaskbarIcon taskbarIcon) {
            _taskbarIcon = taskbarIcon;
        }

        public void ShowBalloonTip(string title, string message, BalloonIcon symbol) {
            _taskbarIcon.ShowBalloonTip(title, message, symbol);
        }
    }
}
