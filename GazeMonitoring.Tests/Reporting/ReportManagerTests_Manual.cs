using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using GazeMonitoring.Data.Aggregation.Model;
using GazeMonitoring.Data.Reporting;
using GazeMonitoring.Model;
using NUnit.Framework;

namespace GazeMonitoring.Tests.Reporting
{
    [TestFixture]
    public class ReportManagerTests_Manual
    {
        [Test]
        public async Task Render()
        {
            var filePath = @"C:\Temp\gaze_data\2020_01_21_17_08_39_270\data_csv";

            var reportManager = new ReportManager();

            var aggregatedData = new AggregatedData();

            aggregatedData.FixationPointsAggregatedDataForAoiByName = new List<FixationPointsAggregatedDataForAoi>();
            aggregatedData.FixationPointsAggregatedDataForScreenConfigurations = new List<FixationPointsAggregatedDataForScreenConfiguration>();

            using (var reader = new StreamReader(Path.Combine(filePath, "FixationPointsAggregatedDataForAoiByName.csv")))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.MissingFieldFound = null;
                csv.Configuration.HeaderValidated = null;
                aggregatedData.FixationPointsAggregatedDataForAoiByName = csv.GetRecords<FixationPointsAggregatedDataForAoi>().ToList();
            }

            using (var reader = new StreamReader(Path.Combine(filePath, "FixationPointsAggregatedDataForScreenConfigurations.csv")))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.MissingFieldFound = null;
                csv.Configuration.HeaderValidated = null;
                aggregatedData.FixationPointsAggregatedDataForScreenConfigurations = csv.GetRecords<FixationPointsAggregatedDataForScreenConfiguration>().ToList();
            }

            using (var reader = new StreamReader(Path.Combine(filePath, "SaccadesDurationByDirection.csv")))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.MissingFieldFound = null;
                csv.Configuration.HeaderValidated = null;
                aggregatedData.SaccadesAggregatedDataByDirectionAndDuration = csv.GetRecords<SaccadesAggregatedDataByDirectionAndDuration>().ToList();
            }

            List<FixationPoint> fixationPoints;

            using (var reader = new StreamReader(Path.Combine(filePath, "log_SensitiveFixation_2020_01_21_19_08_39_282.csv")))
            using (var csv = new CsvReader(reader))
            {
                fixationPoints = csv.GetRecords<FixationPoint>().ToList();
            }

            var monitoringContext = new MonitoringContext
            {
                DataFilesPath = Path.Combine(filePath, "test"),
                SubjectInfo = new SubjectInfo
                {
                    SessionStartTimestamp = UnixTimestampToDateTime(fixationPoints[0].Timestamp),
                    SessionEndTimeStamp = UnixTimestampToDateTime(fixationPoints[fixationPoints.Count - 1].Timestamp)
                }
            };

            await reportManager.GenerateReport(aggregatedData, monitoringContext);
        }

        private static DateTime UnixTimestampToDateTime(long timestamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(timestamp).ToUniversalTime();
            return dtDateTime;
        }
    }
}
