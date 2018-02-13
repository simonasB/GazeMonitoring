using System;

namespace TobiiCoreMonitoring {
    public interface IDataStreamToExternalStorageWriter : IDisposable {
        void Write();
    }
}
