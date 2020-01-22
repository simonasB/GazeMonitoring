using System.Collections.Generic;
using System.Linq;
using GazeMonitoring.Data.Aggregation.Model;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Aggregation.Aggregators
{
    public class GazePointCountsByScreenConfigurationAndAoiDataAggregator : UnfilteredOrLightlyFilteredGazeDataAggregator
    {
        public GazePointCountsByScreenConfigurationAndAoiDataAggregator(ICurrentSessionData currentSessionData) : base(currentSessionData)
        {
        }

        protected override void AggregateInternal(IMonitoringContext monitoringContext, AggregatedData aggregatedData)
        {
            var fixationPointsAggregatedDataForScreenConfigurations =
                new List<GazePointsAggregatedDataForScreenConfiguration>();

            foreach (var mappedFixationPointsByScreenConfigurationId in aggregatedData.MappedGazePoints.GroupBy(o =>
                o.ScreenConfigurationId))
            {
                var fixationPointsAggregatedDataForScreenConfiguration =
                    new GazePointsAggregatedDataForScreenConfiguration
                    {
                        Identifier = mappedFixationPointsByScreenConfigurationId.Key
                    };

                var aggregatedDataForAois = new List<GazePointsAggregateDataForAoi>();
                foreach (var mappedFixationPointByAoiId in mappedFixationPointsByScreenConfigurationId.GroupBy(o =>
                    o.AreaOfInterestId))
                {
                    var fixationPointsAggregatedDataForAoi = new GazePointsAggregateDataForAoi
                    {
                        Identifier = mappedFixationPointByAoiId.Key
                    };

                    foreach (var mappedFixationPoint in mappedFixationPointByAoiId)
                    {
                        fixationPointsAggregatedDataForScreenConfiguration.PointsCount += 1;
                        fixationPointsAggregatedDataForAoi.PointsCount += 1;

                        if (fixationPointsAggregatedDataForScreenConfiguration.IdentifierReadableName == null)
                        {
                            fixationPointsAggregatedDataForScreenConfiguration.IdentifierReadableName =
                                mappedFixationPoint.ScreenConfigurationName;
                        }

                        if (fixationPointsAggregatedDataForAoi.IdentifierReadableName == null)
                        {
                            fixationPointsAggregatedDataForAoi.IdentifierReadableName =
                                mappedFixationPoint.AreaOfInterestName;
                        }
                    }

                    aggregatedDataForAois.Add(fixationPointsAggregatedDataForAoi);
                }

                fixationPointsAggregatedDataForScreenConfiguration.AggregatedDataForAois = aggregatedDataForAois;
                fixationPointsAggregatedDataForScreenConfigurations.Add(
                    fixationPointsAggregatedDataForScreenConfiguration);
            }

            aggregatedData.GazePointsAggregateDataForScreenConfigurations =
                fixationPointsAggregatedDataForScreenConfigurations;
        }
    }
}
