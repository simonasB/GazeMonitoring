using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using GazeMonitoring.Data.Aggregation;
using GazeMonitoring.Data.Aggregation.Model;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Csv
{
    public class CsvAggregatedDataRepository : IAggregatedDataRepository
    {
        public Task Save(AggregatedData aggregatedData, IMonitoringContext monitoringContext)
        {
            if (aggregatedData.MappedFixationPoints != null)
            {
                using (var writer = new StreamWriter(Path.Combine(monitoringContext.DataFilesPath, Constants.FolderName, "MappedFixationPoint.csv")))
                using (var csv = new CsvWriter(writer))
                {
                    csv.WriteRecords(aggregatedData.MappedFixationPoints);
                }
            }

            if (aggregatedData.FixationPointsAggregatedDataForScreenConfigurations != null)
            {
                using (var writer = new StreamWriter(Path.Combine(monitoringContext.DataFilesPath, Constants.FolderName,
                    "FixationPointsAggregatedDataForScreenConfigurations.csv")))
                using (var csv = new CsvWriter(writer))
                {
                    csv.WriteRecords(aggregatedData.FixationPointsAggregatedDataForScreenConfigurations);
                }

                aggregatedData.FixationPointsAggregatedDataForScreenConfigurations.ForEach(o =>
                {
                    var screenConfiguration = monitoringContext.MonitoringConfiguration.ScreenConfigurations
                        .First(x => x.Id == o.ScreenConfigurationId);
                    using (var writer = new StreamWriter(Path.Combine(monitoringContext.DataFilesPath, Constants.FolderName,
                        $"AggregatedDataForAoisByScreenConfiguration_{screenConfiguration.Name}_{screenConfiguration.Number}.csv")))
                    using (var csv = new CsvWriter(writer))
                    {
                        csv.WriteRecords(o.AggregatedDataForAois);
                    }
                });
            }

            if (aggregatedData.FixationPointsAggregatedDataForScreenConfigurations != null)
            {
                using (var writer = new StreamWriter(Path.Combine(monitoringContext.DataFilesPath, Constants.FolderName, "FixationPointsAggregatedDataForAoiByName.csv")))
                using (var csv = new CsvWriter(writer))
                {
                    csv.WriteRecords(aggregatedData.FixationPointsAggregatedDataForAoiByName);
                }
            }

            return Task.CompletedTask;
        }
    }
}
