using System;
using System.Collections.Generic;
using System.Linq;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Aggregation
{
    public class MappedFixationPointsDataAggregator : DataAggregatorBase
    {
        public MappedFixationPointsDataAggregator(ICurrentSessionData currentSessionData) : base(currentSessionData)
        {

        }

        public override void Aggregate(IMonitoringContext monitoringContext, AggregatedData aggregatedData)
        {
            if (monitoringContext.DataStream == DataStream.UnfilteredGaze || monitoringContext.DataStream == DataStream.LightlyFilteredGaze)
            {
                base.Aggregate(monitoringContext, aggregatedData);
                return;
            }

            // Set some property of aggregated data
            var orderedScreenConfigurations = monitoringContext.MonitoringConfiguration.ScreenConfigurations.OrderBy(o => o.Number);
            var fixationPoints = CurrentSessionData.GetFixationPoints().ToList();
            var mappedFixationPoints = new List<MappedFixationPoint>();

            using (var screenConfigurationsEnumerator = orderedScreenConfigurations.GetEnumerator())
            {
                if (!screenConfigurationsEnumerator.MoveNext())
                    return;

                var screenConfiguration = screenConfigurationsEnumerator.Current;
                var screenConfigurationEndTime = monitoringContext.SubjectInfo.SessionStartTimestamp.Add(screenConfiguration.Duration.Value);
                foreach (var fixationPoint in fixationPoints)
                {
                    var gazePointDateTime = DateTimeOffset.FromUnixTimeMilliseconds(fixationPoint.Timestamp).UtcDateTime;
                    // Check time frame
                    if (gazePointDateTime >= monitoringContext.SubjectInfo.SessionStartTimestamp && gazePointDateTime <= screenConfigurationEndTime)
                    {
                        // Valid time frame for current screen configuration
                        // Find if a fixation point is in any area of interest
                        var mappedFixationPoint = new MappedFixationPoint
                        {
                            DurationInMillis = fixationPoint.DurationInMillis,
                            Timestamp = fixationPoint.Timestamp,
                            X = fixationPoint.X,
                            Y = fixationPoint.Y
                        };
                        mappedFixationPoints.Add(mappedFixationPoint);
                        screenConfiguration.AreasOfInterest.ForEach(aoi =>
                        {
                            if (!aoi.IsPointInArea(fixationPoint)) return;

                            mappedFixationPoint.AreaOfInterestName = aoi.Name;
                            mappedFixationPoint.AreaOfInterestId = aoi.Id;
                            mappedFixationPoint.ScreenConfigurationId = screenConfiguration.Id;
                        });
                    }
                    else if (screenConfigurationsEnumerator.MoveNext())
                    {
                        // It means that current screen configuration is not the same time frame as fixation point.
                        // Need to get next screen configuration because fixation points are sorted by time in ascending order.
                        screenConfiguration = screenConfigurationsEnumerator.Current;
                        screenConfigurationEndTime = monitoringContext.SubjectInfo.SessionStartTimestamp.Add(screenConfiguration.Duration.Value);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            aggregatedData.MappedFixationPoints = mappedFixationPoints;

            base.Aggregate(monitoringContext, aggregatedData);
        }
    }
}
