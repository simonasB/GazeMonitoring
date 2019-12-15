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

                var aggregatedDataForAoi = new FixationPointsAggregatedDataForAoi
                {
                    Identifier = aoiName
                };
                long fixationPointsDurationInMillis = 0;
                foreach (var mappedFixationPointByName in mappedFixationPointsByName)
                {
                    aggregatedDataForAoi.FixationPointsCount += 1;
                    if (mappedFixationPointByName.DurationInMillis > 100)
                    {
                        aggregatedDataForAoi.LongFixationPointsCount += 1;
                    }
                    else
                    {
                        aggregatedDataForAoi.ShortFixationPointsCount += 1;
                    }

                    fixationPointsDurationInMillis += mappedFixationPointByName.DurationInMillis;
                }

                aggregatedDataForAoi.FixationPointsDuration = TimeSpan.FromMilliseconds(fixationPointsDurationInMillis);
                aggregatedDataForAois.Add(aggregatedDataForAoi);
            }

            aggregatedData.FixationPointsAggregatedDataForAoiByName = aggregatedDataForAois;
        }
    }
}
