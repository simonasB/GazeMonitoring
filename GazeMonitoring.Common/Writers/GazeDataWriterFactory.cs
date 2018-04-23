﻿using System;
using System.Collections.Generic;
using GazeMonitoring.Common.Calculations;
using GazeMonitoring.Data;
using GazeMonitoring.Model;

namespace GazeMonitoring.Common.Writers {
    public class GazeDataWriterFactory {
        private readonly IGazeDataRepository _repository;
        private readonly ISaccadeCalculator _saccadeCalculator;

        public GazeDataWriterFactory(IGazeDataRepository repository, ISaccadeCalculator saccadeCalculator) {
            _repository = repository;
            _saccadeCalculator = saccadeCalculator;
        }
        public IGazeDataWriter GetGazeDataWriter(DataStream dataStream) {
            switch (dataStream) {
                case DataStream.UnfilteredGaze:
                    return new GazePointWriter(_repository);;
                case DataStream.LightlyFilteredGaze:
                    return new GazePointWriter(_repository);
                case DataStream.SensitiveFixation:
                case DataStream.SlowFixation:
                    var gazeDataWriters = new List<IGazeDataWriter>();
                    gazeDataWriters.Add(new GazePointWriter(_repository));
                    gazeDataWriters.Add(new SaccadesWriter(_repository, _saccadeCalculator));
                    return new MultipleSourcesGazeDataWriter(gazeDataWriters);
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataStream), dataStream, null);
            }
        }
    }
}