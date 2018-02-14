using System;
using Tobii.Interaction;
using Tobii.Interaction.Framework;
using TobiiCoreMonitoring.Entities;

namespace TobiiCoreMonitoring.csv {
    public class DataStreamToCsvWriterFactory : IDataStreamToExternalStorageWriterFactory
    {
        private readonly Host _host;

        public DataStreamToCsvWriterFactory(Host host) {
            _host = host;
        }

        public IDataStreamToExternalStorageWriter GetWriter(DataStream dataStream) {
            FileName fileNameData;
            FileName fileNameSaccades;

            switch (dataStream) {
                case DataStream.UnfilteredGaze:
                    fileNameData = new FileName {DataStream = DataStream.UnfilteredGaze.ToString()};
                    return new GazeDataStreamToCsvWriter(new CsvWriterProvider(fileNameData), _host,
                        GazePointDataMode.Unfiltered);
                case DataStream.LightlyFilteredGaze:
                    fileNameData = new FileName { DataStream = DataStream.LightlyFilteredGaze.ToString() };
                    return new GazeDataStreamToCsvWriter(new CsvWriterProvider(fileNameData), _host,
                        GazePointDataMode.LightlyFiltered);
                case DataStream.SensitiveFixation:
                    fileNameData = new FileName { DataStream = DataStream.SensitiveFixation.ToString() };
                    fileNameSaccades = new FileName { DataStream = $"{DataStream.SensitiveFixation}_Saccades"};                    
                    return new FixationDataStreamToCsvWriter(new CsvWriterProvider(fileNameSaccades), new CsvWriterProvider(fileNameData), _host, FixationDataMode.Sensitive);
                case DataStream.SlowFixation:
                    fileNameData = new FileName { DataStream = DataStream.SlowFixation.ToString() };
                    fileNameSaccades = new FileName { DataStream = $"{DataStream.SlowFixation}_Saccades" };
                    return new FixationDataStreamToCsvWriter(new CsvWriterProvider(fileNameSaccades), new CsvWriterProvider(fileNameData), _host, FixationDataMode.Slow);
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataStream), dataStream, null);
            }
        }
    }
}
