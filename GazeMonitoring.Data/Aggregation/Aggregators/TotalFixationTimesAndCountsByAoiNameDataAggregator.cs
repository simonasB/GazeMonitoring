using System;
using System.Collections.Generic;
using System.Linq;
using GazeMonitoring.Data.Aggregation.Model;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Aggregation.Aggregators
{
    public class TotalFixationTimesAndCountsByAoiNameDataAggregator : SensitiveOrSlowFixationDataAggregator
    {
        public TotalFixationTimesAndCountsByAoiNameDataAggregator(ICurrentSessionData currentSessionData) : base(currentSessionData)
        {
        }

        protected override void AggregateInternal(IMonitoringContext monitoringContext, AggregatedData aggregatedData)
        {
            var aggregatedDataForAois = new List<FixationPointsAggregatedDataForAoi>();

            foreach (var mappedFixationPointsByName in aggregatedData.MappedFixationPoints.GroupBy(o => o.AreaOfInterestName))
            {
                var aoiName = mappedFixationPointsByName.Key;

                aggregatedDataForAois.Add(new FixationPointsAggregatedDataForAoi
                {
                    Identifier = aoiName,
                    FixationPointsCount = mappedFixationPointsByName.Count(),
                    FixationPointsDuration = TimeSpan.FromMilliseconds(mappedFixationPointsByName.Sum(o => o.DurationInMillis))
                });
            }

            aggregatedData.FixationPointsAggregatedDataForAoiByName = aggregatedDataForAois;
        }
    }
}
