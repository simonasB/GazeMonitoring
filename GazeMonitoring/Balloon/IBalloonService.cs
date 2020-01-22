using Hardcodet.Wpf.TaskbarNotification;

namespace GazeMonitoring.Balloon {
    public interface IBalloonService {
        void ShowBalloonTip(string title, string message, BalloonIcon symbol);
    }
}