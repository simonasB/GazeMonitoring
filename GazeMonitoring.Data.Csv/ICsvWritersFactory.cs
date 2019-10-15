using System;
using System.Collections.Generic;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Csv {
    public interface ICsvWritersFactory {
        Dictionary<Type, CsvWriterWrapper> GetCsvWriters(IMonitoringContext monitoringContext);
    }
}