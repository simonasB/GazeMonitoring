using System;
using System.Globalization;

namespace GazeMonitoring.Data {
    public class FileName {
        public string DataStream { get; set; }

        public DateTime DateTime => DateTime.Now; 

        // Separate service. Bridge/strategy
        public override string ToString() {
            return $"log_{DataStream}_{DateTime.ToString("yyyy_MM_dd_HH_mm_ss_fff", CultureInfo.InvariantCulture)}";
        }
    }
}
