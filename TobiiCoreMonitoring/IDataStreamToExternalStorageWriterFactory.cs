using GazeMonitoring.Common.Entities;

namespace TobiiCoreMonitoring {
    public interface IDataStreamToExternalStorageWriterFactory {
        IDataStreamToExternalStorageWriter GetWriter(DataStream dataStream);
    }
}