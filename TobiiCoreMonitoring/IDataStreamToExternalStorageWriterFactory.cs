using TobiiCoreMonitoring.Entities;

namespace TobiiCoreMonitoring {
    public interface IDataStreamToExternalStorageWriterFactory {
        IDataStreamToExternalStorageWriter GetWriter(DataStream dataStream);
    }
}