using System;
using System.Linq;
using System.Threading.Tasks;
using GazeMonitoring.Data;
using GazeMonitoring.Data.Aggregation;
using GazeMonitoring.Data.Csv;
using GazeMonitoring.Data.Reporting;
using GazeMonitoring.DataAccess.LiteDB;
using GazeMonitoring.Model;
using GazeMonitoring.Monitor;
using NUnit.Framework;

namespace GazeMonitoring.Tests.Data.Aggregation
{
    [TestFixture]
    public class AggregatorTests_Manual
    {
        [Test]
        public async Task ManualTest()
        {
            var dataAggregationService = new DataAggregationService(
                new DataAggregationManager(new CurrentSessionDataFromTemp(new TempDataConfiguration())),
                new CsvAggregatedDataRepository(), new ReportManager());

            var repo = new LiteDBConfigurationRepository();
            var monitoringConfiguration = repo.Search<MonitoringConfiguration>().First(o => o.Id == 2);

            await dataAggregationService.Run(new MonitoringContext
            {
                DataStream = DataStream.SensitiveFixation,
                IsReportGenerated = true,
                MonitoringConfiguration = monitoringConfiguration
            });
        }
    }
}
