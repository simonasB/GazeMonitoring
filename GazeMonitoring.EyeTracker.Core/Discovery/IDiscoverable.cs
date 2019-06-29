using Autofac;

namespace GazeMonitoring.EyeTracker.Core.Discovery {
    /// <summary>
    /// Responsible for discovering eye tracker and registering eye tracker specific implementations
    /// </summary>
    public interface IDiscoverable {
        /// <summary>
        /// Implementation of this method should check whether eyetracker is available and if so register eyetracker specific abstraction implementations and other necessary dependencies
        /// </summary>
        /// <param name="container">IoC container where eyetracker specific abstraction implementations and other necessary dependencies must be registered</param>
        /// <returns>Configured container with eyetracker specific abstraction implementations and other necessary dependencies</returns>
        DiscoveryResult Discover(ContainerBuilder container);
    }
}
