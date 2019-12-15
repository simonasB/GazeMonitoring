using System.Threading.Tasks;
using GazeMonitoring.Data.Aggregation.Model;
using GazeMonitoring.Model;

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

        }
    }
}
