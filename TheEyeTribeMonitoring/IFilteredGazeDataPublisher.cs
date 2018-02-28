using EyeTribe.ClientSdk.Data;

namespace TheEyeTribeMonitoring {
    public interface IFilteredGazeDataPublisher {
        void PublishFilteredData(GazeData gazeData);
    }
}
