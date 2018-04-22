using GazeMonitoring.Model;

namespace GazeMonitoring.Data {
    public interface IFileNameFormatter {
        string Format(FileName fileName);
    }
}
