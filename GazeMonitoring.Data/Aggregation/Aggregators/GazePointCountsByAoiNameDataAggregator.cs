using System.Collections.Generic;
using System.Linq;
using GazeMonitoring.Data.Aggregation.Model;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Aggregation.Aggregators
{
    public class GazePointCountsByAoiNameDataAggregator : UnfilteredOrLightlyFilteredGazeDataAggregator
    {
        public GazePointCountsByAoiNameDataAggregator(ICurrentSessionData currentSessionData) : base(currentSessionData)
        {
        }

        protected override void AggregateInternal(IMonitoringContext monitoringContext, AggregatedData aggregatedData)
        {
            var aggregatedDataForAois = new List<GazePointsAggregateDataForAoi>();

            foreach (var mappedGazePointsByName in aggregatedData.MappedGazePoints.GroupBy(o => o.AreaOfInterestName))
            {
                var aoiName = mappedGazePointsByName.Key;

                var aggregatedDataForAoi = new GazePointsAggregateDataForAoi
                {
                    Identifier = aoiName,
                    IdentifierReadableName = aoiName
                };
                aggregatedDataForAoi.PointsCount += mappedGazePointsByName.Count();
                aggregatedDataForAois.Add(aggregatedDataForAoi);
            }

            aggregatedData.GazePointsAggregateDataForAoiByName = aggregatedDataForAois;
        }
    }
}
