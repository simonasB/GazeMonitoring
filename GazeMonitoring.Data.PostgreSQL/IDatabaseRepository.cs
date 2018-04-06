using System;
using System.Collections.Generic;
using GazeMonitoring.Common.Entities;

namespace GazeMonitoring.Data.PostgreSQL {
    public interface IDatabaseRepository {
        void BinaryInsertGazePoints(IEnumerable<GazePoint> gazePoints, string sessionId, int? subjectInfoId, DateTime sampleTime);
        void BinaryInsertSaccades(IEnumerable<Saccade> saccades, string sessionId, int? subjectInfoId, DateTime sampleTime);
        SubjectInfo RetrieveSubjectInfo(string sessionId);
        void SaveSubjectInfo(SubjectInfo subjectInfo);
    }
}