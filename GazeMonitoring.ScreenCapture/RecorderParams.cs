using System;
using System.Windows.Forms;

namespace GazeMonitoring.ScreenCapture {
    public class RecorderParams {
        public RecorderParams(string fileName, int framesPerSecond, int quality) {
            if (fileName == null) {
                throw new ArgumentNullException(nameof(fileName));
            }

            FileName = fileName;
            FramesPerSecond = framesPerSecond;
            Quality = quality;
            Height = Screen.PrimaryScreen.Bounds.Height;
            Width = Screen.PrimaryScreen.Bounds.Width;
        }

        public string FileName { get; set; }

        public int FramesPerSecond { get; set; }

        public int Quality { get; set; }

        public int Height { get; }

        public int Width { get; }
    }
}
