using System;
using GazeMonitoring.Model;

namespace GazeMonitoring.EyeTracker.Core.Streams
{
    public class NullGazePointStreamFactory : IGazePointStreamFactory
    {
        public GazePointStream GetGazePointStream(DataStream dataStream)
        {
            throw new NotImplementedException();
        }
    }
}
