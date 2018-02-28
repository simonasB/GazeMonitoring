using System;
using GazeMonitoring.Common.Entities;
using Tobii.Interaction;
using Tobii.Interaction.Framework;

namespace TobiiCoreMonitoring.Csv
{
    public class FixationDataStreamToCsvWriter : IDataStreamToExternalStorageWriter {
        private readonly CsvWriterProvider _saccadesCsvWriterProvider;
        private readonly CsvWriterProvider _fixationDataCsvWriterProvider;
        private readonly FixationDataStream _fixationDataStream;
        private FixationData _previousFixationData;

        public FixationDataStreamToCsvWriter(CsvWriterProvider saccadesCsvWriterProvider, CsvWriterProvider fixationDataCsvWriterProvider, Host host, FixationDataMode fixationDataMode) {
            _saccadesCsvWriterProvider = saccadesCsvWriterProvider;
            _fixationDataCsvWriterProvider = fixationDataCsvWriterProvider;
            _fixationDataStream = host.Streams.CreateFixationDataStream(fixationDataMode);
        }

        public void Write() {
            _fixationDataCsvWriterProvider.CsvWriter.WriteHeader<FixationData>();
            _fixationDataCsvWriterProvider.CsvWriter.NextRecord();

            _saccadesCsvWriterProvider.CsvWriter.WriteHeader<Saccade>();
            _saccadesCsvWriterProvider.CsvWriter.NextRecord();

            _fixationDataStream.Next += OnGazePointData;
        }

        public void Dispose() {
            _fixationDataStream.Next -= OnGazePointData;
            _saccadesCsvWriterProvider?.Dispose();
            _fixationDataCsvWriterProvider?.Dispose();
        }

        private void OnGazePointData(object sender, StreamData<FixationData> data) {
            FixationData currentFixationData = data.Data;
            _fixationDataCsvWriterProvider.CsvWriter.WriteRecord(currentFixationData);
            _fixationDataCsvWriterProvider.CsvWriter.NextRecord();

            if (_previousFixationData == null) {
                _previousFixationData = currentFixationData;
            } else {
                var deltaX = currentFixationData.X - _previousFixationData.X;
                var deltaY = currentFixationData.Y - _previousFixationData.Y;

                var saccade = new Saccade {
                    Direction = Math.Atan(Math.Abs(deltaY) /
                                          Math.Abs(deltaX)),
                    Amplitude = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2)),
                    StartTimeStamp = _previousFixationData.Timestamp,
                    EndTimeStamp = currentFixationData.Timestamp
                };

                var deltaTimeStamp = currentFixationData.Timestamp -
                                     _previousFixationData.Timestamp;

                saccade.Velocity = Math.Abs(deltaTimeStamp) < 0.0001
                    ? saccade.Amplitude
                    : saccade.Amplitude / (deltaTimeStamp);
                _saccadesCsvWriterProvider.CsvWriter.WriteRecord(saccade);
                _saccadesCsvWriterProvider.CsvWriter.NextRecord();
                _previousFixationData = currentFixationData;
            }
        }
    }
}
