using System.Threading.Tasks;
using GazeMonitoring.Data.Aggregation;
using GazeMonitoring.Data.Reporting;
using GazeMonitoring.Model;

namespace GazeMonitoring.Monitor
{
    public interface IDataAggregationService
    {
        Task Run(IMonitoringContext monitoringContext);
    }

    public class DataAggregationService : IDataAggregationService
    {
        private readonly IDataAggregationManager _dataAggregationManager;
        private readonly IAggregatedDataRepository _dataRepository;
        private readonly IReportManager _reportManager;

        public DataAggregationService(IDataAggregationManager dataAggregationManager, IAggregatedDataRepository dataRepository, IReportManager reportManager)
        {
            _dataAggregationManager = dataAggregationManager;
            _dataRepository = dataRepository;
            _reportManager = reportManager;
        }

        public async Task Run(IMonitoringContext monitoringContext)
        {
            var aggregatedData = _dataAggregationManager.Aggregate(monitoringContext);

            await _dataRepository.Save(aggregatedData);

            if (monitoringContext.IsReportGenerated)
            {
                await _reportManager.GenerateReport(aggregatedData, monitoringContext);
            }
        }
    }
}
