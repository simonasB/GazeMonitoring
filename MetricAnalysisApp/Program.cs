using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using GazeMonitoring.Model;

namespace MetricAnalysisApp {
    public class Program {
        public class AreaOfInterest : IComparable {
            public int SlideNumber { get; set; }
            public SlideObjectType Type { get; set; }
            public double Left { get; set; }
            public double Top { get; set; }
            public double Width { get; set; }
            public double Height { get; set; }

            public int Id { get; set; }

            public DateTime StartTimeInterval { get; set; }
            public DateTime EndTimeInterval { get; set; }

            public bool IsPointInArea(GazePoint gazePoint) {
                return gazePoint.X >= Left && Left + Width >= gazePoint.X && gazePoint.Y >= Top && Top + Height >= gazePoint.Y;
            }

            public bool IsPointBetweenTimeInterval(GazePoint gazePoint) {
                var gazePointDateTime = DateTimeOffset.FromUnixTimeMilliseconds(gazePoint.Timestamp).UtcDateTime;
                return gazePointDateTime >= StartTimeInterval && gazePointDateTime <= EndTimeInterval;
            }

            public int CompareTo(object obj) {
                return this.Id.CompareTo(((AreaOfInterest) obj).Id);
            }
        }

        public class MappedFixationPoint {
            public double X { get; set; }
            public double Y { get; set; }
            public long Timestamp { get; set; }
            //public long DurationInMillis { get; set; }

            public int? SlideNumber { get; set; }
            public SlideObjectType? Type { get; set; }
        }

        public enum SlideObjectType {
            Unknown = 0,
            Title = 1,
            Text = 2,
            Graph = 3,
            Formula = 4
        }

        public class FixationTimes
        {
            public TimeSpan FixationPointsDuration { get; set; }
            public int LongFixationPointsCount { get; set; }
            public int ShortFixationPointsCount { get; set; }
            public string Identifier { get; set; }
            public string IdentifierReadableName { get; set; }
            public int PointsCount { get; set; }
        }

        public class Vark
        {
            public int Visual { get; set; }
            public int Auditory { get; set; }
            [Name("Read/Write")]
            public int ReadWrite { get; set; }
            public int Kinesthetic { get; set; }
            public string LearningPreference { get; set; }
        }

        public class SummaryResult
        {
            public string Name { get; set; }
            public int TotalTitleTime { get; set; }
            public int TotalTextTime { get; set; }
            public int TotalGraphTime { get; set; }
            public int TotalFormulaTime { get; set; }
            public int TotalTimeSpentOnAoi { get; set; }
            public int TotalTimeSpentNotFocusedOnAoi { get; set; }
            public int TotalTitleFixationsCount { get; set; }
            public int TotalTextFixationsCount { get; set; }
            public int TotalGraphFixationsCount { get; set; }
            public int TotalFormulaFixationsCount { get; set; }
            public int TotalFixationsCountOnAoi { get; set; }
            public int TotalFixationsCountNotFocusedOnAoi { get; set; }
            public int TotalTitleGazePointsCount { get; set; }
            public int TotalTextGazePointsCount { get; set; }
            public int TotalGraphGazePointsCount { get; set; }
            public int TotalFormulaGazePointsCount { get; set; }
            public int TotalGazePointsCountOnAoi { get; set; }
            public int TotalGazePointsNotFocusedOnAoi { get; set; }
            public int Visual { get; set; }
            public int Auditory { get; set; }
            public int ReadWrite { get; set; }
            public int Kinesthetic { get; set; }
            public string LearningPreference { get; set; }
        }

        public static void Main(string[] args)
        {
            var dirs = Directory.EnumerateDirectories(@"C:\Users\s.baltulionis\OneDrive\Main\Master\magistras\LO tyrimas\spring\1");

            var summaryResults = new List<SummaryResult>();

            // Read AOIs
            List<AreaOfInterest> areasOfInterest;
            var aoisFilePath = @"C:\Users\s.baltulionis\OneDrive\Main\Master\magistras\LO tyrimas\spring\1\aoi.txt";
            using (var reader = new StreamReader(aoisFilePath))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.HeaderValidated = (b, strings, arg3, arg4) => { };
                csv.Configuration.MissingFieldFound = (strings, i, arg3) => { };
                areasOfInterest = csv.GetRecords<AreaOfInterest>().ToList();
            }

            int id = 0;
            areasOfInterest.ForEach(aoi => {
                aoi.Id = id++;
            });

            foreach (var dir in dirs)
            {
                var dataFilePath = Path.Combine(dir, "data_csv", "FixationPointsAggregatedDataForAoiByName.csv");
                var varkFilePath = Path.Combine(dir, "data_csv", "vark.csv");
                var gazePointsFilePath = Path.Combine(dir, "data_csv", "gazepoints.csv");

                var summaryResult = new SummaryResult();

                using (var reader = new StreamReader(dataFilePath))
                using (var csv = new CsvReader(reader))
                {
                    var fixationTimes = csv.GetRecords<FixationTimes>().ToList();
                    var totalFixationPointsCount = 0;
                    fixationTimes.ForEach(o =>
                    {
                        var totalMillis = (int) o.FixationPointsDuration.TotalMilliseconds;
                        totalFixationPointsCount += o.PointsCount;
                        switch (o.Identifier)
                        {
                            case "Text":
                                summaryResult.TotalTextTime = totalMillis;
                                summaryResult.TotalTextFixationsCount = o.PointsCount;
                                break;
                            case "Title":
                                summaryResult.TotalTitleTime = totalMillis;
                                summaryResult.TotalTitleFixationsCount = o.PointsCount;
                                break;
                            case "Graph":
                                summaryResult.TotalGraphTime = totalMillis;
                                summaryResult.TotalGraphFixationsCount = o.PointsCount;
                                break;
                            case "Formula":
                                summaryResult.TotalFormulaTime = totalMillis;
                                summaryResult.TotalFormulaFixationsCount = o.PointsCount;
                                break;
                            default:
                                summaryResult.TotalFixationsCountNotFocusedOnAoi = o.PointsCount;
                                break;
                        }
                    });
                    summaryResult.TotalTimeSpentOnAoi = summaryResult.TotalTextTime + summaryResult.TotalTitleTime + summaryResult.TotalGraphTime + summaryResult.TotalFormulaTime;
                    summaryResult.TotalTimeSpentNotFocusedOnAoi = (int)TimeSpan.FromSeconds(140).TotalMilliseconds - summaryResult.TotalTimeSpentOnAoi;
                    summaryResult.TotalFixationsCountOnAoi = totalFixationPointsCount - summaryResult.TotalFixationsCountNotFocusedOnAoi;
                }

                using (var reader = new StreamReader(varkFilePath))
                using (var csv = new CsvReader(reader))
                {
                    var fixationTimes = csv.GetRecords<Vark>().ToList();
                    summaryResult.Visual = fixationTimes[0].Visual;
                    summaryResult.Auditory = fixationTimes[0].Auditory;
                    summaryResult.ReadWrite = fixationTimes[0].ReadWrite;
                    summaryResult.Kinesthetic = fixationTimes[0].Kinesthetic;
                    summaryResult.LearningPreference = fixationTimes[0].LearningPreference;
                }

                using (var reader = new StreamReader(gazePointsFilePath))
                using (var csv = new CsvReader(reader))
                {
                    var subjectInfo = new SubjectInfo();
                    var fixationPoints = new List<GazePoint>();
                    csv.Configuration.IgnoreBlankLines = false;
                    csv.Configuration.RegisterClassMap<SubjectInfoMap>();
                    csv.Configuration.RegisterClassMap<FixationPointMap>();
                    csv.Configuration.CultureInfo = new CultureInfo("en-US", false);

                    csv.Read();
                    csv.ReadHeader();
                    csv.Read();
                    subjectInfo = csv.GetRecord<SubjectInfo>();
                    csv.Read();
                    csv.ReadHeader();
                    var isHeader = true;
                    while (csv.Read())
                    {
                        fixationPoints.Add(csv.GetRecord<GazePoint>());
                    }
                    //var sessionStartTime = File.GetCreationTime(file).ToUniversalTime();
                    var sessionStartTime = DateTimeOffset.FromUnixTimeMilliseconds(fixationPoints[0].Timestamp).UtcDateTime;
                    // Map Fixations with AOI
                    var metricCalculationService = new MetricCalculationService(sessionStartTime, TimeSpan.FromSeconds(20), areasOfInterest);
                    metricCalculationService.MapFixationPointToAreaOfInterest(fixationPoints);

                    var results = metricCalculationService.GetResults();

                    results.ForEach(r => {
                        switch (r.Type)
                        {
                            case SlideObjectType.Unknown:
                                summaryResult.TotalGazePointsNotFocusedOnAoi = r.FixationCount;
                                break;
                            case SlideObjectType.Title:
                                summaryResult.TotalTitleGazePointsCount += r.FixationCount;
                                break;
                            case SlideObjectType.Text:
                                summaryResult.TotalTextGazePointsCount += r.FixationCount;
                                break;
                            case SlideObjectType.Graph:
                                summaryResult.TotalGraphGazePointsCount += r.FixationCount;
                                break;
                            case SlideObjectType.Formula:
                                summaryResult.TotalFormulaGazePointsCount += r.FixationCount;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    });
                    summaryResult.TotalGazePointsCountOnAoi = summaryResult.TotalTitleGazePointsCount + summaryResult.TotalTextGazePointsCount + summaryResult.TotalGraphGazePointsCount +
                                                              summaryResult.TotalFormulaGazePointsCount;

                    summaryResult.TotalGazePointsNotFocusedOnAoi = fixationPoints.Count - summaryResult.TotalGazePointsCountOnAoi;
                }

                summaryResult.TotalGazePointsCountOnAoi = summaryResult.TotalTitleGazePointsCount + summaryResult.TotalTextGazePointsCount + summaryResult.TotalGraphGazePointsCount +
                                                          summaryResult.TotalFormulaGazePointsCount;

                summaryResult.Name = new DirectoryInfo(dir).Name.Split('_')[0];
                summaryResults.Add(summaryResult);
            }

            using (var writer = new StreamWriter(@"C:\Users\s.baltulionis\OneDrive\Main\Master\magistras\LO tyrimas\spring\1\summary2.csv"))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteRecords(summaryResults);
            }
        }

        public class MetricCalculationService {
            public MetricCalculationService(DateTime sessionStartTime, TimeSpan timeForEachAoi, List<AreaOfInterest> areasOfInterest) {
                FixationPointsByAreaOfInterest = new SortedDictionary<AreaOfInterest, List<GazePoint>>();
                var starTimeInterval = sessionStartTime;
                var endTimeInterval = starTimeInterval.Add(timeForEachAoi);
                var slideNumber = 1;
                areasOfInterest.ForEach(aoi => {
                    if (slideNumber != aoi.SlideNumber) {
                        starTimeInterval = starTimeInterval.Add(timeForEachAoi);
                        endTimeInterval = starTimeInterval.Add(timeForEachAoi);
                        slideNumber++;
                    }
                    aoi.StartTimeInterval = starTimeInterval;
                    aoi.EndTimeInterval = endTimeInterval;
                    FixationPointsByAreaOfInterest.Add(aoi, new List<GazePoint>());
                });
            }

            public SortedDictionary<AreaOfInterest, List<GazePoint>> FixationPointsByAreaOfInterest { get; }

            public List<MappedFixationPoint> MappedFixationPoints { get; private set; }

            public void MapFixationPointToAreaOfInterest(List<GazePoint> fixationPoints) {
                MappedFixationPoints = new List<MappedFixationPoint>();
                fixationPoints.ForEach(fixationPoint => {
                    bool isMapped = false;
                    foreach (var keyValuePair in FixationPointsByAreaOfInterest) {
                        var aoi = keyValuePair.Key;
                        if (aoi.IsPointInArea(fixationPoint) && aoi.IsPointBetweenTimeInterval(fixationPoint)) {
                            keyValuePair.Value.Add(fixationPoint);
                            MappedFixationPoints.Add(new MappedFixationPoint {
                                X = fixationPoint.X,
                                Y = fixationPoint.Y,
                                Timestamp = fixationPoint.Timestamp,
                                SlideNumber = aoi.SlideNumber,
                                Type = aoi.Type
                            });
                            isMapped = true;
                            break;
                        }
                    }
                    if (!isMapped) {
                        MappedFixationPoints.Add(new MappedFixationPoint
                        {
                            X = fixationPoint.X,
                            Y = fixationPoint.Y,
                            Timestamp = fixationPoint.Timestamp,
                        });
                    }
                    //Console.WriteLine($"Fixation point - x:{fixationPoint.X}, y:{fixationPoint.Y}, Duration:{fixationPoint.DurationInMillis} is not in the any areas of interest. ");
                });
            }



            public List<Result> GetResults() {
                var results = new List<Result>();
                foreach (var keyValuePair in FixationPointsByAreaOfInterest) {
                    var aoi = keyValuePair.Key;
                    var fixationPoints = keyValuePair.Value;
                    results.Add(new Result {
                        SlideNumber = aoi.SlideNumber,
                        Type = aoi.Type,
                        FixationCount = fixationPoints.Count
                    });
                }

                return results;
            }
        }

        public class SubjectInfoMap : ClassMap<SubjectInfo> {
            public SubjectInfoMap() {
                AutoMap();
                Map(m => m.Id).Ignore();
                Map(m => m.SessionEndTimeStamp).Ignore();
                Map(m => m.SessionId).Ignore();
                Map(m => m.SessionStartTimestamp).Ignore();
            }
        }

        public sealed class FixationPointMap : ClassMap<GazePoint> {
            public FixationPointMap() {
                //Map(m => m.DurationInMillis);
                Map(m => m.X);
                Map(m => m.Y);
                Map(m => m.Timestamp);
            }
        }

        public class Result {
            public int SlideNumber { get; set; }
            public SlideObjectType Type { get; set; }
            public int FixationCount { get; set; }
        }
    }
}
