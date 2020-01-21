using System.Collections.Generic;
using System.Linq;
using GazeMonitoring.Data.Aggregation.Model;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Aggregation.Aggregators
{
    public class SaccadesByDirectionAndDurationAggregator : SensitiveOrSlowFixationDataAggregator
    {
        public SaccadesByDirectionAndDurationAggregator(ICurrentSessionData currentSessionData) : base(currentSessionData)
        {
        }

        protected override void AggregateInternal(IMonitoringContext monitoringContext, AggregatedData aggregatedData)
        {
            var saccadeDurationAndDirectionCategoryInfoList = new List<SaccadeDurationAndDirectionCategoryInfo>();

            foreach (var saccade in CurrentSessionData.GetSaccades())
            {
                var saccadeDirectionData = new SaccadeDurationAndDirectionCategoryInfo
                {
                    DirectionCategory = FindDirection(saccade.Direction),
                    DurationCategory = FindSaccadeCategory(saccade.EndTimeStamp - saccade.StartTimeStamp)
                };

                saccadeDurationAndDirectionCategoryInfoList.Add(saccadeDirectionData);
            }

            var saccadesDirectionAndDurationAggregatedDataList = new List<SaccadesAggregatedDataByDirectionAndDuration>();
            var totalSaccadesCount = saccadeDurationAndDirectionCategoryInfoList.Count;

            // Group by direction
            foreach (var saccadeDurationAndDirectionCategoryInfos in saccadeDurationAndDirectionCategoryInfoList.GroupBy(o => o.DirectionCategory))
            {
                // Group by duration
                foreach (var durationAndDirectionCategoryInfos in saccadeDurationAndDirectionCategoryInfos.GroupBy(o => o.DurationCategory))
                {
                    var saccadesDirectionAndDurationAggregatedData = new SaccadesAggregatedDataByDirectionAndDuration();
                    saccadesDirectionAndDurationAggregatedData.SaccadeDirectionCategory = saccadeDurationAndDirectionCategoryInfos.Key;
                    saccadesDirectionAndDurationAggregatedData.SaccadeDurationCategory = durationAndDirectionCategoryInfos.Key;
                    saccadesDirectionAndDurationAggregatedData.Count = durationAndDirectionCategoryInfos.Count();
                    saccadesDirectionAndDurationAggregatedData.RelativePercentage = (double) saccadesDirectionAndDurationAggregatedData.Count / totalSaccadesCount;
                    saccadesDirectionAndDurationAggregatedDataList.Add(saccadesDirectionAndDurationAggregatedData);
                }
            }

            aggregatedData.SaccadesAggregatedDataByDirectionAndDuration = saccadesDirectionAndDurationAggregatedDataList;
        }

        private static SaccadeDirectionCategory FindDirection(double direction)
        {
            if (direction <= 11.25 || direction <= 360 && direction > 348.75)
            {
                return SaccadeDirectionCategory.E;
            }

            if (direction > 11.25 && direction <= 33.75)
            {
                return SaccadeDirectionCategory.ENE;
            }
            if (direction > 33.75 && direction <= 56.25)
            {
                return SaccadeDirectionCategory.NE;
            }
            if (direction > 56.25 && direction <= 78.75)
            {
                return SaccadeDirectionCategory.NNE;
            }
            if (direction > 78.75 && direction <= 101.25)
            {
                return SaccadeDirectionCategory.N;
            }
            if (direction > 101.25 && direction <= 123.75)
            {
                return SaccadeDirectionCategory.NNW;
            }
            if (direction > 123.75 && direction <= 146.25)
            {
                return SaccadeDirectionCategory.NW;
            }
            if (direction > 146.25 && direction <= 168.75)
            {
                return SaccadeDirectionCategory.WNW;
            }
            if (direction > 168.75 && direction <= 191.25)
            {
                return SaccadeDirectionCategory.W;
            }
            if (direction > 191.25 && direction <= 213.75)
            {
                return SaccadeDirectionCategory.WSW;
            }
            if (direction > 213.75 && direction <= 236.25)
            {
                return SaccadeDirectionCategory.SW;
            }
            if (direction > 236.25 && direction <= 258.75)
            {
                return SaccadeDirectionCategory.SSW;
            }
            if (direction > 258.75 && direction <= 281.25)
            {
                return SaccadeDirectionCategory.S;
            }
            if (direction > 281.25 && direction <= 303.75)
            {
                return SaccadeDirectionCategory.SSE;
            }
            if (direction > 303.75 && direction <= 326.25)
            {
                return SaccadeDirectionCategory.SE;
            }

            return SaccadeDirectionCategory.ESE;
        }

        private static SaccadeDurationCategory FindSaccadeCategory(double duration)
        {
            if (duration <= 60)
            {
                return SaccadeDurationCategory.Short;
            }

            if (duration > 60 && duration <= 200)
            {
                return SaccadeDurationCategory.Medium;
            }

            return SaccadeDurationCategory.Long;
        }
    }
}
