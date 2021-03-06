﻿using System;
using EyeTribe.ClientSdk;
using EyeTribe.ClientSdk.Data;

namespace TheEyeTribeMonitoring {
    public class GazeListener : IGazeListener {
        private readonly IFilteredGazeDataPublisher _filteredGazeDataPublisher;

        public GazeListener(IFilteredGazeDataPublisher filteredGazeDataPublisher) {
            if (filteredGazeDataPublisher == null) {
                throw new ArgumentNullException(nameof(filteredGazeDataPublisher));
            }
            _filteredGazeDataPublisher = filteredGazeDataPublisher;
        }

        public void OnGazeUpdate(GazeData gazeData) {
            _filteredGazeDataPublisher.PublishFilteredData(gazeData);
        }
    }
}
