using System;
using System.Collections.Generic;
using GazeMonitoring.Common.Calculations;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Writers {
    public interface IGazeDataWriterFactory
    {
        IGazeDataWriter Create(IMonitoringContext monitoringContext);
    }

    public class GazeDataWriterFactory : IGazeDataWriterFactory
    {
        private readonly IGazeDataRepositoryFactory _dataRepositoryFactory;
        private readonly ISaccadeCalculator _saccadeCalculator;

        public GazeDataWriterFactory(IGazeDataRepositoryFactory dataRepositoryFactory, ISaccadeCalculator saccadeCalculator) {
            if (dataRepositoryFactory == null) {
                throw new ArgumentNullException(nameof(dataRepositoryFactory));
            }

            if (saccadeCalculator == null) {
                throw new ArgumentNullException(nameof(saccadeCalculator));
            }

            _dataRepositoryFactory = dataRepositoryFactory;
            _saccadeCalculator = saccadeCalculator;
        }

        public IGazeDataWriter Create(IMonitoringContext monitoringContext)
        {
            var repository = _dataRepositoryFactory.Create(monitoringContext);
            switch (monitoringContext.DataStream) {
                case DataStream.UnfilteredGaze:
                    return new GazePointWriter(repository);
                case DataStream.LightlyFilteredGaze:
                    return new GazePointWriter(repository);
                case DataStream.SensitiveFixation:
                case DataStream.SlowFixation:
                    var gazeDataWriters = new List<IGazeDataWriter>();
                    gazeDataWriters.Add(new FixationPointsWriter(repository));
                    gazeDataWriters.Add(new SaccadesWriter(repository, _saccadeCalculator));
                    return new MultipleSourcesGazeDataWriter(gazeDataWriters);
                default:
                    throw new ArgumentOutOfRangeException(nameof(monitoringContext.DataStream), monitoringContext.DataStream, null);
            }
        }
    }
}
