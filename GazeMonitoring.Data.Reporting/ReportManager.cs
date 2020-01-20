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
            var engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Templates"))
                .UseMemoryCachingProvider()
                .Build();

            var totalTimesByAoiPieData = new List<PieSeriesData>();
            var totalTimesByScreenConfigurationPieData = new List<PieSeriesData>();

            var totalSessionTimeInMillis =
                (monitoringContext.SubjectInfo.SessionEndTimeStamp -
                 monitoringContext.SubjectInfo.SessionStartTimestamp).TotalMilliseconds;

            aggregatedData.FixationPointsAggregatedDataForAoiByName.ForEach(o =>
            {
                totalTimesByAoiPieData.Add(new PieSeriesData { Name = o.Identifier ?? "None", Y = o.FixationPointsDuration.TotalMilliseconds * 100 / totalSessionTimeInMillis });
            });
            aggregatedData.FixationPointsAggregatedDataForScreenConfigurations.ForEach(o =>
            {
                totalTimesByScreenConfigurationPieData.Add(new PieSeriesData { Name = o.IdentifierReadableName, Y = o.FixationPointsDuration.TotalMilliseconds * 100 / totalSessionTimeInMillis });
            });
            var fixationPointsCountDataForAoiByName = GetFixationPointsCountData(aggregatedData.FixationPointsAggregatedDataForAoiByName.Cast<FixationPointsAggregatedData>().ToList(),
                aggregatedData.FixationPointsAggregatedDataForAoiByName.Sum(o => o.FixationPointsCount));

            var fixationPointsCountDataForScreenConfigurations = GetFixationPointsCountData(aggregatedData.FixationPointsAggregatedDataForScreenConfigurations.Cast<FixationPointsAggregatedData>().ToList(),
                aggregatedData.FixationPointsAggregatedDataForScreenConfigurations.Sum(o => o.FixationPointsCount));

            string result = await engine.CompileRenderAsync("Main.cshtml", 
                new
                {
                    TotalTimesByAoi = totalTimesByAoiPieData,
                    TotalTimesByScreenConfiguration = totalTimesByScreenConfigurationPieData,
                    FixationPointsCountForAoiByName = fixationPointsCountDataForAoiByName,
                    FixationPointsCountForScreenConfigurations = fixationPointsCountDataForScreenConfigurations
                });

            var reportsFolderPath = Path.Combine(monitoringContext.DataFilesPath, "Reports");

            if (!Directory.Exists(reportsFolderPath))
                Directory.CreateDirectory(reportsFolderPath);

            File.WriteAllText(Path.Combine(reportsFolderPath, "report.html"), result);
        }

        private (List<PieSeriesData> FullFixationPointsCountData, List<PieSeriesData> LongAndShortFixationPointsCountData) GetFixationPointsCountData(List<FixationPointsAggregatedData> aggregatedData, int totalFixationPointCount)
        {
            var fullFixationPointsCountData = new List<PieSeriesData>();
            var longAndShortFixationPointsCountData = new List<PieSeriesData>();

            aggregatedData.ForEach(o =>
            {
                fullFixationPointsCountData.Add(new PieSeriesData {Name = o.IdentifierReadableName, Y = o.FixationPointsCount * 100 / totalFixationPointCount});
                longAndShortFixationPointsCountData.Add(new PieSeriesData { Name = "LongFixationsCount", Y = o.LongFixationPointsCount * 100 / totalFixationPointCount });
                longAndShortFixationPointsCountData.Add(new PieSeriesData { Name = "ShortFixationsCount", Y = o.ShortFixationPointsCount * 100 / totalFixationPointCount });
            });

            return (fullFixationPointsCountData, longAndShortFixationPointsCountData);
        }
    }
}
