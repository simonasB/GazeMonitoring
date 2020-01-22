using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GazeMonitoring.Data.Aggregation.Model;
using GazeMonitoring.Model;
using Highsoft.Web.Mvc.Charts;
using RazorLight;

namespace GazeMonitoring.Data.Reporting
{
    public interface IReportManager
    {
        Task GenerateReport(AggregatedData aggregatedData, IMonitoringContext monitoringContext);
    }

    public class ReportManager : IReportManager
    {
        public async Task GenerateReport(AggregatedData aggregatedData, IMonitoringContext monitoringContext)
        {
            var reportsFolderPath = Path.Combine(monitoringContext.DataFilesPath, "Reports");

            if (!Directory.Exists(reportsFolderPath))
                Directory.CreateDirectory(reportsFolderPath);

            var engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Templates"))
                .UseMemoryCachingProvider()
                .Build();

            string result;

            if (monitoringContext.DataStream == DataStream.LightlyFilteredGaze ||
                monitoringContext.DataStream == DataStream.UnfilteredGaze)
            {
                result = await GenerateGazePointsReport(aggregatedData, engine);
            }
            else
            {
                result = await GenerateFixationPointsReport(aggregatedData, monitoringContext, engine);

            }

            File.WriteAllText(Path.Combine(reportsFolderPath, "report.html"), result);
        }

        private async Task<string> GenerateFixationPointsReport(AggregatedData aggregatedData, IMonitoringContext monitoringContext, RazorLightEngine engine)
        {
            var totalTimesByAoiPieData = new List<PieSeriesData>();
            var totalTimesByScreenConfigurationPieData = new List<PieSeriesData>();

            var totalSessionTimeInMillis =
                (monitoringContext.SubjectInfo.SessionEndTimeStamp -
                 monitoringContext.SubjectInfo.SessionStartTimestamp).TotalMilliseconds;

            aggregatedData.FixationPointsAggregatedDataForAoiByName.ForEach(o =>
            {
                totalTimesByAoiPieData.Add(new PieSeriesData { Name = string.IsNullOrWhiteSpace(o.IdentifierReadableName) ? "None" : o.IdentifierReadableName, Y = o.FixationPointsDuration.TotalMilliseconds * 100 / totalSessionTimeInMillis });
            });
            aggregatedData.FixationPointsAggregatedDataForScreenConfigurations.ForEach(o =>
            {
                totalTimesByScreenConfigurationPieData.Add(new PieSeriesData { Name = o.IdentifierReadableName, Y = o.FixationPointsDuration.TotalMilliseconds * 100 / totalSessionTimeInMillis });
            });
            var fixationPointsCountDataForAoiByName = GetFixationPointsCountData(aggregatedData.FixationPointsAggregatedDataForAoiByName.Cast<FixationPointsAggregatedData>().ToList(),
                aggregatedData.FixationPointsAggregatedDataForAoiByName.Sum(o => o.PointsCount));

            var fixationPointsCountDataForScreenConfigurations = GetFixationPointsCountData(aggregatedData.FixationPointsAggregatedDataForScreenConfigurations.Cast<FixationPointsAggregatedData>().ToList(),
                aggregatedData.FixationPointsAggregatedDataForScreenConfigurations.Sum(o => o.PointsCount));

            string result = await engine.CompileRenderAsync("FixationPoints.cshtml",
                new
                {
                    TotalTimesByAoi = totalTimesByAoiPieData,
                    TotalTimesByScreenConfiguration = totalTimesByScreenConfigurationPieData,
                    FixationPointsCountForAoiByName = fixationPointsCountDataForAoiByName,
                    FixationPointsCountForScreenConfigurations = fixationPointsCountDataForScreenConfigurations,
                    SaccadesSeriesByFullDuration = SaccadesSeriesByFullDuration(aggregatedData.SaccadesAggregatedDataByDirectionAndDuration),
                    SaccadesSeriesByDurationCategory = SaccadesSeriesByDurationCategory(aggregatedData.SaccadesAggregatedDataByDirectionAndDuration),
                });

            return result;
        }

        private async Task<string> GenerateGazePointsReport(AggregatedData aggregatedData, RazorLightEngine engine)
        {
            var totalTimesByAoiPieData = new List<PieSeriesData>();
            var totalTimesByScreenConfigurationPieData = new List<PieSeriesData>();

            aggregatedData.GazePointsAggregateDataForAoiByName.ForEach(o =>
            {
                totalTimesByAoiPieData.Add(new PieSeriesData
                {
                    Name = string.IsNullOrWhiteSpace(o.IdentifierReadableName) ? "None" : o.IdentifierReadableName,
                    Y = o.PointsCount * 100 / aggregatedData.GazePointsAggregateDataForAoiByName.Sum(x => x.PointsCount)
                });
            });
            aggregatedData.GazePointsAggregateDataForScreenConfigurations.ForEach(o =>
            {
                totalTimesByScreenConfigurationPieData.Add(new PieSeriesData
                {
                    Name = o.IdentifierReadableName,
                    Y = o.PointsCount * 100 / aggregatedData.GazePointsAggregateDataForScreenConfigurations.Sum(x => x.PointsCount)
                });
            });

            string result = await engine.CompileRenderAsync("GazePoints.cshtml",
                new
                {
                    TotalCountsByAoi = totalTimesByAoiPieData,
                    TotalCountsByScreenConfiguration = totalTimesByScreenConfigurationPieData
                });

            return result;
        }

        private static (List<PieSeriesData> FullFixationPointsCountData, List<PieSeriesData> LongAndShortFixationPointsCountData) GetFixationPointsCountData(List<FixationPointsAggregatedData> aggregatedData, int totalFixationPointCount)
        {
            var fullFixationPointsCountData = new List<PieSeriesData>();
            var longAndShortFixationPointsCountData = new List<PieSeriesData>();

            aggregatedData.ForEach(o =>
            {
                fullFixationPointsCountData.Add(new PieSeriesData {Name = string.IsNullOrWhiteSpace(o.IdentifierReadableName) ? "None" : o.IdentifierReadableName, Y = o.PointsCount * 100.0 / totalFixationPointCount});
                longAndShortFixationPointsCountData.Add(new PieSeriesData { Name = "LongFixationsCount", Y = o.LongFixationPointsCount * 100.0 / totalFixationPointCount });
                longAndShortFixationPointsCountData.Add(new PieSeriesData { Name = "ShortFixationsCount", Y = o.ShortFixationPointsCount * 100.0 / totalFixationPointCount });
            });

            return (fullFixationPointsCountData, longAndShortFixationPointsCountData);
        }

        private List<Series> SaccadesSeriesByDurationCategory(List<SaccadesAggregatedDataByDirectionAndDuration> aggregatedData)
        {
            var series = new List<Series>
            {
                new ColumnSeries
                {
                    Name = "<60 ms",
                    Data = MapDataToWindRoseColumnSeriesData(aggregatedData.Where(o => o.SaccadeDurationCategory == SaccadeDurationCategory.Short).ToList())
                },
                new ColumnSeries
                {
                    Name = "60-200 ms",
                    Data = MapDataToWindRoseColumnSeriesData(aggregatedData.Where(o => o.SaccadeDurationCategory == SaccadeDurationCategory.Medium).ToList())
                },
                new ColumnSeries
                {
                    Name = ">200 ms",
                    Data = MapDataToWindRoseColumnSeriesData(aggregatedData.Where(o => o.SaccadeDurationCategory == SaccadeDurationCategory.Long).ToList())
                }
            };

            return series;
        }

        private List<Series> SaccadesSeriesByFullDuration(List<SaccadesAggregatedDataByDirectionAndDuration> aggregatedData)
        {
            var series = new List<Series>
            {
                new ColumnSeries
                {
                    Name = "Full duration",
                    Data = MapDataToWindRoseColumnSeriesData(aggregatedData)
                }
            };

            return series;
        }

        /// <summary>
        /// Order of values in wind rose graph, need to remap as in normal x,y axis, direction starts from E.
        /// ["N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW"]
        /// </summary>
        /// <returns></returns>
        private static List<ColumnSeriesData> MapDataToWindRoseColumnSeriesData(List<SaccadesAggregatedDataByDirectionAndDuration> aggregatedData)
        {
            var columnSeriesData = new List<ColumnSeriesData>(16);
            for (int i = 0; i < 16; i++)
            {
                columnSeriesData.Add(new ColumnSeriesData {Y = 0});
            }
            aggregatedData.ForEach(o =>
            {
                int index;
                switch (o.SaccadeDirectionCategory)
                {
                    case SaccadeDirectionCategory.N:
                        index = 0;
                        break;
                    case SaccadeDirectionCategory.NNW:
                        index = 1;
                        break;
                    case SaccadeDirectionCategory.NW:
                        index = 2;
                        break;
                    case SaccadeDirectionCategory.WNW:
                        index = 3;
                        break;
                    case SaccadeDirectionCategory.W:
                        index = 4;
                        break;
                    case SaccadeDirectionCategory.WSW:
                        index = 5;
                        break;
                    case SaccadeDirectionCategory.SW:
                        index = 6;
                        break;
                    case SaccadeDirectionCategory.SSW:
                        index = 7;
                        break;
                    case SaccadeDirectionCategory.S:
                        index = 8;
                        break;
                    case SaccadeDirectionCategory.SSE:
                        index = 9;
                        break;
                    case SaccadeDirectionCategory.SE:
                        index = 10;
                        break;
                    case SaccadeDirectionCategory.ESE:
                        index = 11;
                        break;
                    case SaccadeDirectionCategory.E:
                        index = 12;
                        break;
                    case SaccadeDirectionCategory.ENE:
                        index = 13;
                        break;
                    case SaccadeDirectionCategory.NE:
                        index = 14;
                        break;
                    case SaccadeDirectionCategory.NNE:
                        index = 15;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                columnSeriesData[index].Y += o.RelativePercentage * 100;
            });

            return columnSeriesData;
        }
    }
}
