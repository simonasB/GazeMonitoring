using System.Collections.Generic;
using System.IO;
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

            var pieData = new List<PieSeriesData>();

            var totalSessionTimeInMillis =
                (monitoringContext.SubjectInfo.SessionEndTimeStamp -
                 monitoringContext.SubjectInfo.SessionStartTimestamp).TotalMilliseconds;

            aggregatedData.FixationPointsAggregatedDataForAoiByName.ForEach(o =>
            {
                pieData.Add(new PieSeriesData { Name = o.Identifier ?? "None", Y = o.FixationPointsDuration.TotalMilliseconds * 100 / totalSessionTimeInMillis });
            });

            string result = await engine.CompileRenderAsync("Main.cshtml", new { PieData = pieData });

            var reportsFolderPath = Path.Combine(monitoringContext.DataFilesPath, "Reports");

            if (!Directory.Exists(reportsFolderPath))
                Directory.CreateDirectory(reportsFolderPath);

            File.WriteAllText(Path.Combine(reportsFolderPath, "report.html"), result);
        }
    }
}
