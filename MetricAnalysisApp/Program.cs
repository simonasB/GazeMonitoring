using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
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
            public long DurationInMillis { get; set; }

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

        public static void Main(string[] args) {
            // Read AOIs
            List<AreaOfInterest> areasOfInterest;
            var aoisFilePath = @"C:\Users\baltus\OneDrive - Dell Inc\Master\magistras\LO tyrimas\aoi_darbe.txt";
            var fixationsFilesDir = @"C:\java_dev\spool\GazeMonitoring\GazeMonitoring\bin\Debug\data_csv\New folder";
            var resultsDirPath = @"C:\Users\baltus\OneDrive - Dell Inc\Master\magistras\LO tyrimas\results2";
            using (var reader = new StreamReader(aoisFilePath))
            using (var csv = new CsvReader(reader)) {
                csv.Configuration.HeaderValidated = (b, strings, arg3, arg4) => { };
                csv.Configuration.MissingFieldFound = (strings, i, arg3) => { };
                areasOfInterest = csv.GetRecords<AreaOfInterest>().ToList();
            }

            int id = 0;
            areasOfInterest.ForEach(aoi => {
                aoi.Id = id++;
            });

            // Read fixations
            foreach (string file in Directory.EnumerateFiles(fixationsFilesDir, "*.csv")) {
                using (var reader = new StreamReader(file))
                using (var csv = new CsvReader(reader)) {
                    var subjectInfo = new SubjectInfo();
                    var fixationPoints = new List<FixationPoint>();
                    csv.Configuration.IgnoreBlankLines = false;
                    csv.Configuration.RegisterClassMap<SubjectInfoMap>();
                    csv.Configuration.RegisterClassMap<FixationPointMap>();
                    var isHeader = true;
                    while (csv.Read()) {
                        if (isHeader) {
                            csv.ReadHeader();
                            isHeader = false;
                            continue;
                        }

                        if (string.IsNullOrEmpty(csv.GetField(0))) {
                            isHeader = true;
                            continue;
                        }

                        switch (csv.Context.HeaderRecord[0]) {
                            case "Age":
                                subjectInfo = csv.GetRecord<SubjectInfo>();
                                break;
                            case "DurationInMillis":
                                fixationPoints.Add(csv.GetRecord<FixationPoint>());
                                break;
                            default:
                                throw new InvalidOperationException("Unknown record type.");
                        }
                    }
                    //var sessionStartTime = File.GetCreationTime(file).ToUniversalTime();
                    var sessionStartTime = DateTimeOffset.FromUnixTimeMilliseconds(fixationPoints[0].Timestamp).UtcDateTime;
                    // Map Fixations with AOI
                    var metricCalculationService = new MetricCalculationService(sessionStartTime, TimeSpan.FromSeconds(30), areasOfInterest);
                    metricCalculationService.MapFixationPointToAreaOfInterest(fixationPoints);
                    
                    var results = metricCalculationService.GetResults();

                    double totalTitleTime = 0.0,
                        totalTextTime = 0.0,
                        totalGraphTime = 0.0,
                        totalFormulaTime = 0.0;

                    results.ForEach(r => {
                        switch (r.Type) {
                            case SlideObjectType.Unknown:
                                break;
                            case SlideObjectType.Title:
                                totalTitleTime += r.FixationsDurationInMillis;
                                break;
                            case SlideObjectType.Text:
                                totalTextTime += r.FixationsDurationInMillis;
                                break;
                            case SlideObjectType.Graph:
                                totalGraphTime += r.FixationsDurationInMillis;
                                break;
                            case SlideObjectType.Formula:
                                totalFormulaTime += r.FixationsDurationInMillis;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    });

                    var totalTimeSpentOnAoi = totalTitleTime + totalTextTime + totalGraphTime + totalFormulaTime;

                    using (var writer = new StreamWriter($@"{resultsDirPath}\averages_{subjectInfo.Name?.Replace(" ", "_")}_{subjectInfo.Age}_{subjectInfo.Details?.Replace(" ", "_")}"))
                    using (var csvWriter = new CsvWriter(writer)) {
                        csvWriter.WriteRecords(results);
                        var a = new {
                            TotalTitleTime = totalTitleTime,
                            TotalTextTime = totalTextTime,
                            TotalGraphTime = totalGraphTime,
                            TotalFormulaTime = totalFormulaTime,
                            TotalTimeSpentOnAoi = totalTimeSpentOnAoi,
                            TotalTimeSpentNotFocusedOnAoi = TimeSpan.FromSeconds(210).TotalMilliseconds - totalTimeSpentOnAoi
                        };
                        csvWriter.WriteHeader(a.GetType());
                        csvWriter.NextRecord();
                        csvWriter.WriteRecord(new {
                            TotalTitleTime = totalTitleTime,
                            TotalTextTime = totalTextTime,
                            TotalGraphTime = totalGraphTime,
                            TotalFormulaTime = totalFormulaTime,
                            TotalTimeSpentOnAoi = totalTimeSpentOnAoi,
                            TotalTimeSpentNotFocusedOnAoi = TimeSpan.FromSeconds(210).TotalMilliseconds - totalTimeSpentOnAoi
                        });
                    }

                    using(var writer = new StreamWriter($@"{resultsDirPath}\fixations_{subjectInfo.Name?.Replace(" ", "_")}_{subjectInfo.Age}_{subjectInfo.Details?.Replace(" ", "_")}"))
                    using (var csvWriter = new CsvWriter(writer))
                    {
                        csvWriter.WriteRecords(metricCalculationService.MappedFixationPoints);
                    }
                }
            }
        }

        public class MetricCalculationService {
            public MetricCalculationService(DateTime sessionStartTime, TimeSpan timeForEachAoi, List<AreaOfInterest> areasOfInterest) {
                FixationPointsByAreaOfInterest = new SortedDictionary<AreaOfInterest, List<FixationPoint>>();
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
                    FixationPointsByAreaOfInterest.Add(aoi, new List<FixationPoint>());
                });
            }

            public SortedDictionary<AreaOfInterest, List<FixationPoint>> FixationPointsByAreaOfInterest { get; }

            public List<MappedFixationPoint> MappedFixationPoints { get; private set; }

            public void MapFixationPointToAreaOfInterest(List<FixationPoint> fixationPoints) {
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
                                DurationInMillis = fixationPoint.DurationInMillis,
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
                            DurationInMillis = fixationPoint.DurationInMillis
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
                        FixationCount = fixationPoints.Count,
                        FixationsDurationInMillis = fixationPoints.Sum(o => o.DurationInMillis)
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

        public sealed class FixationPointMap : ClassMap<FixationPoint> {
            public FixationPointMap() {
                Map(m => m.DurationInMillis);
                Map(m => m.X);
                Map(m => m.Y);
                Map(m => m.Timestamp);
            }
        }

        public class Result {
            public int SlideNumber { get; set; }
            public SlideObjectType Type { get; set; }
            public long FixationsDurationInMillis { get; set; }
            public long FixationCount { get; set; }
            public double AverageFixationInMillis => (double) FixationsDurationInMillis / FixationCount;
        }
    }
}
