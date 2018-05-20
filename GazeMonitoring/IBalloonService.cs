using Hardcodet.Wpf.TaskbarNotification;

namespace GazeMonitoring {
    public interface IBalloonService {
        void ShowBalloonTip(string title, string message, BalloonIcon symbol);
    }
}