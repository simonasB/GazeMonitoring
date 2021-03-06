﻿using System;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Writers {
    public class GazePointWriter : IGazeDataWriter {
        private readonly IGazeDataRepository _repository;

        public GazePointWriter(IGazeDataRepository repository) {
            if (repository == null) {
                throw new ArgumentNullException(nameof(repository));
            }

            _repository = repository;
        }

        public void Write(GazePoint gazePoint) {
            if (gazePoint == null) {
                throw new ArgumentNullException(nameof(gazePoint));
            }

            _repository.SaveGazePoint(gazePoint);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repository?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~GazePointWriter()
        {
            Dispose(false);
        }
    }
}
