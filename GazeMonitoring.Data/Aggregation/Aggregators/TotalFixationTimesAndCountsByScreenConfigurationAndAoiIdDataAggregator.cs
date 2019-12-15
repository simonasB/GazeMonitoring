using System;
using System.Collections.Generic;
using System.Linq;
using GazeMonitoring.Data.Aggregation.Model;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Aggregation.Aggregators
{
    public class TotalFixationTimesAndCountsByScreenConfigurationAndAoiIdDataAggregator : SensitiveOrSlowFixationDataAggregator
    {
        public TotalFixationTimesAndCountsByScreenConfigurationAndAoiIdDataAggregator(ICurrentSessionData currentSessionData) : base(currentSessionData)
        {

        }

        protected override void AggregateInternal(IMonitoringContext monitoringContext, AggregatedData aggregatedData)
        {
            var fixationPointsAggregatedDataForScreenConfigurations = new List<FixationPointsAggregatedDataForScreenConfiguration>();

            foreach (var mappedFixationPointsByScreenConfigurationId in aggregatedData.MappedFixationPoints.GroupBy(o => o.ScreenConfigurationId))
            {
                var fixationPointsAggregatedDataForScreenConfiguration = new FixationPointsAggregatedDataForScreenConfiguration
                {
                    ScreenConfigurationId = mappedFixationPointsByScreenConfigurationId.Key
                };

                var aggregatedDataForAois = new List<FixationPointsAggregatedDataForAoi>();
                long fixationPointsDurationInMillisForScreenConfiguration = 0;
                foreach (var mappedFixationPointByAoiId in mappedFixationPointsByScreenConfigurationId.GroupBy(o => o.AreaOfInterestId))
                {
                    var fixationPointsAggregatedDataForAoi = new FixationPointsAggregatedDataForAoi
                    {
                        Identifier = mappedFixationPointByAoiId.Key
                    };

                    long fixationPointsDurationInMillisForAoi = 0;
                    foreach (var mappedFixationPoint in mappedFixationPointByAoiId)
                    {
                        fixationPointsDurationInMillisForScreenConfiguration += mappedFixationPoint.DurationInMillis;
                        fixationPointsDurationInMillisForAoi += mappedFixationPoint.DurationInMillis;
                        fixationPointsAggregatedDataForScreenConfiguration.FixationPointsCount += 1;
                        fixationPointsAggregatedDataForAoi.FixationPointsCount += 1;
                        if (mappedFixationPoint.DurationInMillis > 100)
                        {
                            fixationPointsAggregatedDataForScreenConfiguration.LongFixationPointsCount += 1;
                            fixationPointsAggregatedDataForAoi.LongFixationPointsCount += 1;
                        }
                        else
                        {
                            fixationPointsAggregatedDataForScreenConfiguration.ShortFixationPointsCount += 1;
                            fixationPointsAggregatedDataForAoi.ShortFixationPointsCount += 1;
                        }
                    }

                    fixationPointsAggregatedDataForAoi.FixationPointsDuration = TimeSpan.FromMilliseconds(fixationPointsDurationInMillisForAoi);
                    aggregatedDataForAois.Add(fixationPointsAggregatedDataForAoi);
                }

                fixationPointsAggregatedDataForScreenConfiguration.AggregatedDataForAois = aggregatedDataForAois;
                fixationPointsAggregatedDataForScreenConfiguration.FixationPointsDuration = TimeSpan.FromMilliseconds(fixationPointsDurationInMillisForScreenConfiguration);
                fixationPointsAggregatedDataForScreenConfigurations.Add(fixationPointsAggregatedDataForScreenConfiguration);
            }

            aggregatedData.FixationPointsAggregatedDataForScreenConfigurations = fixationPointsAggregatedDataForScreenConfigurations;
        }
    }
}
