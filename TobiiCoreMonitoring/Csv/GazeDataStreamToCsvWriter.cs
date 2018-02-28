using Tobii.Interaction;
using Tobii.Interaction.Framework;

namespace TobiiCoreMonitoring.Csv {
    public class GazeDataStreamToCsvWriter : IDataStreamToExternalStorageWriter {
        private readonly CsvWriterProvider _csvWriterProvider;
        private readonly GazePointDataStream _gazePointDataStream;

        public GazeDataStreamToCsvWriter(CsvWriterProvider csvWriterProvider, Host host, GazePointDataMode gazePointDataMode) {
            _csvWriterProvider = csvWriterProvider;
            _gazePointDataStream = host.Streams.CreateGazePointDataStream(gazePointDataMode);
        }

        public void Dispose() {
            _gazePointDataStream.Next -= OnGazePointData;
            _csvWriterProvider.Dispose();
        }

        public void Write() {
            _csvWriterProvider.CsvWriter.WriteHeader<GazePointData>();
            _csvWriterProvider.CsvWriter.NextRecord();

            _gazePointDataStream.Next += OnGazePointData;
        }

        private void OnGazePointData(object sender, StreamData<GazePointData> streamData) {
            _csvWriterProvider.CsvWriter.WriteRecord(streamData.Data);
            _csvWriterProvider.CsvWriter.NextRecord();
        }
    }
}