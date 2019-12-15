using System.Threading.Tasks;

namespace GazeMonitoring.Monitor {
    public interface IGazeDataMonitor {
        Task StartAsync();
        Task StopAsync();
    }
}