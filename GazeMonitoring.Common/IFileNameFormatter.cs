using GazeMonitoring.Common.Entities;

namespace GazeMonitoring.Common {
    public interface IFileNameFormatter {
        string Format(FileName fileName);
    }
}
