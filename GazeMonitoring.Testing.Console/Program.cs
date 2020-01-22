using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using GazeMonitoring.Data.Aggregation.Model;
using GazeMonitoring.Data.Reporting;
using GazeMonitoring.Model;
using Highsoft.Web.Mvc.Charts;
using RazorLight;

namespace GazeMonitoring.Testing.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            TestReport().Wait();
        }

        private static async Task TestReport()
        {
            var filePath = @"C:\Temp\gaze_data\2020_01_21_17_08_39_270\data_csv";

            var reportManager = new ReportManager();

            var aggregatedData = new AggregatedData();

            aggregatedData.FixationPointsAggregatedDataForAoiByName = new List<FixationPointsAggregatedDataForAoi>();
            aggregatedData.FixationPointsAggregatedDataForScreenConfigurations =
                new List<FixationPointsAggregatedDataForScreenConfiguration>();

            using (var reader =
                new StreamReader(Path.Combine(filePath, "FixationPointsAggregatedDataForAoiByName.csv")))
            using (var csv = new CsvReader(reader))
            {
                aggregatedData.FixationPointsAggregatedDataForAoiByName =
                    csv.GetRecords<FixationPointsAggregatedDataForAoi>().ToList();
            }

            using (var reader =
                new StreamReader(Path.Combine(filePath, "FixationPointsAggregatedDataForScreenConfigurations.csv")))
            using (var csv = new CsvReader(reader))
            {
                aggregatedData.FixationPointsAggregatedDataForScreenConfigurations =
                    csv.GetRecords<FixationPointsAggregatedDataForScreenConfiguration>().ToList();
            }

            using (var reader = new StreamReader(Path.Combine(filePath, "SaccadesDurationByDirection.csv")))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.MissingFieldFound = null;
                csv.Configuration.HeaderValidated = null;
                aggregatedData.SaccadesAggregatedDataByDirectionAndDuration =
                    csv.GetRecords<SaccadesAggregatedDataByDirectionAndDuration>().ToList();
            }

            List<FixationPoint> fixationPoints;

            using (var reader =
                new StreamReader(Path.Combine(filePath, "log_SensitiveFixation_2020_01_21_19_08_39_282.csv")))
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

        private static void Test()
        {
            List<double?> usaValues = new List<double?>
            {
                null, null, null, null, null, 6, 11, 32, 110, 235, 369, 640,
                1005, 1436, 2063, 3057, 4618, 6444, 9822, 15468, 20434, 24126,
                27387, 29459, 31056, 31982, 32040, 31233, 29224, 27342, 26662,
                26956, 27912, 28999, 28965, 27826, 25579, 25722, 24826, 24605,
                24304, 23464, 23708, 24099, 24357, 24237, 24401, 24344, 23586,
                22380, 21004, 17287, 14747, 13076, 12555, 12144, 11009, 10950,
                10871, 10824, 10577, 10527, 10475, 10421, 10358, 10295, 10104
            };
            List<double?> russiaValues = new List<double?>
            {
                null, null, null, null, null, null, null, null, null, null,
                5, 25, 50, 120, 150, 200, 426, 660, 869, 1060, 1605, 2471, 3322,
                4238, 5221, 6129, 7089, 8339, 9399, 10538, 11643, 13092, 14478,
                15915, 17385, 19055, 21205, 23044, 25393, 27935, 30062, 32049,
                33952, 35804, 37431, 39197, 45000, 43000, 41000, 39000, 37000,
                35000, 33000, 31000, 29000, 27000, 25000, 24000, 23000, 22000,
                21000, 20000, 19000, 18000, 18000, 17000, 16000
            };
            List<AreaSeriesData> usaData = new List<AreaSeriesData>();
            List<AreaSeriesData> russiaData = new List<AreaSeriesData>();
            usaValues.ForEach(p => usaData.Add(new AreaSeriesData {Y = p}));
            russiaValues.ForEach(p => russiaData.Add(new AreaSeriesData {Y = p}));

            var engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Templates"))
                .UseMemoryCachingProvider()
                .Build();

            string result = engine.CompileRenderAsync("Main.cshtml", new {UsaData = usaData, RussiaData = russiaData})
                .Result;

            File.WriteAllText(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Reports", "report.html"),
                result);
        }

        private void Test2()
        {
            var chartOptionsFixationCountsForScreenConfigurations =
                new Highcharts
                {
                    Chart = new Highsoft.Web.Mvc.Charts.Chart
                    {
                    },
                    Title = new Title
                    {
                        Text = "Fixation points counts for area of interest by name"
                    },


                    YAxis = new List<YAxis>
                    {
                        new YAxis
                        {
                            Title = new YAxisTitle
                            {
                                Text = "Fixation points count"
                            }
                        }
                    },
                    PlotOptions = new PlotOptions
                    {
                        Pie = new PlotOptionsPie
                        {
                            Shadow = new Shadow
                            {
                                Enabled = true,
                                Color = "#000000",
                                Width = 10,
                                OffsetX = 0,
                                OffsetY = 0
                            },
                            Center = new string[] {"50%", "50%"}
                        }
                    },
                    Tooltip = new Tooltip
                    {
                        ValueSuffix = "%"
                    },
                    Series = new List<Series>
                    {
                        new PieSeries
                        {
                            Name = "Total count",
                            Size = "60%",
                            DataLabels = new PieSeriesDataLabels
                            {
                                Formatter = "formatBrowserSeriesDataLabels",
                                Color = "white",
                                //Distance = -30
                            },
                        },
                        new PieSeries
                        {
                            Name = "Split",
                            Size = "80%",
                            InnerSize = "60%",
                        }
                    }
                };
        }
    }
}
