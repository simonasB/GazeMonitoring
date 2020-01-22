using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using GazeMonitoring.EyeTracker.Core.Streams;
using GazeMonitoring.Model;
using SharpAvi.Output;

namespace GazeMonitoring.ScreenCapture.SharpAvi {
    public class SharpAviRecorder : IScreenRecorder {
        private AviWriter _writer;
        private readonly IAviVideoStreamFactory _videoStreamFactory;
        private readonly IGazePointStreamFactory _gazePointStreamFactory;
        private GazePointStream _gazePointStream;
        private IAviVideoStream _videoStream;
        private Thread _screenThread;
        private readonly ManualResetEvent _stopThread = new ManualResetEvent(false);
        private double _gazeX;
        private double _gazeY;

        private AviWriter CreateAviWriter(RecorderParams recorderParams) {
            return new AviWriter(recorderParams.FileName) {
                FramesPerSecond = recorderParams.FramesPerSecond,
                EmitIndex1 = true,
            };
        }

        public SharpAviRecorder(IAviVideoStreamFactory videoStreamFactory, IGazePointStreamFactory gazePointStreamFactory) {
            if (videoStreamFactory == null) {
                throw new ArgumentNullException(nameof(videoStreamFactory));
            }

            if (gazePointStreamFactory == null) {
                throw new ArgumentNullException(nameof(gazePointStreamFactory));
            }

            _videoStreamFactory = videoStreamFactory;
            _gazePointStreamFactory = gazePointStreamFactory;
        }

        public void StartRecording(RecorderParams recorderParams, IMonitoringContext monitoringContext) {

            // Create AVI writer and specify FPS
            _writer = CreateAviWriter(recorderParams);

            // Create video stream
            _videoStream = _videoStreamFactory.Create(_writer, recorderParams, global::SharpAvi.KnownFourCCs.Codecs.MotionJpeg);
            // Set only name. Other properties were when creating stream, 
            // either explicitly by arguments or implicitly by the encoder used
            _videoStream.Name = "GazeMonitoring";

            _screenThread = new Thread(() => RecordScreen(recorderParams)) {
                Name = typeof(SharpAviRecorder).Name + ".RecordScreen",
                IsBackground = true
            };
            _gazePointStream = _gazePointStreamFactory.GetGazePointStream(monitoringContext.DataStream);
            _gazePointStream.GazePointReceived += OnGazePointReceived;
            _screenThread.Start();
        }

        public void StopRecording() {
            _stopThread.Set();
            _screenThread.Join();

            // Close writer: the remaining data is written to a file and file is closed
            _writer.Close();
            _gazePointStream.GazePointReceived -= OnGazePointReceived;
            _gazePointStream = null;
            _stopThread.Dispose();
        }

        private void OnGazePointReceived(object sender, GazePointReceivedEventArgs args) {
            _gazeX = args.GazePoint.X;
            _gazeY = args.GazePoint.Y;
        }

        private void RecordScreen(RecorderParams recorderParams) {
            var frameInterval = TimeSpan.FromSeconds(1 / (double) _writer.FramesPerSecond);
            var buffer = new byte[recorderParams.Width * recorderParams.Height * 4];
            Task videoWriteTask = null;
            var timeTillNextFrame = TimeSpan.Zero;

            while (!_stopThread.WaitOne(timeTillNextFrame)) {
                var timestamp = DateTime.Now;

                TakeScreenshot(buffer, recorderParams);

                // Wait for the previous frame is written
                videoWriteTask?.Wait();

                // Start asynchronous (encoding and) writing of the new frame
                videoWriteTask = _videoStream.WriteFrameAsync(true, buffer, 0, buffer.Length);

                timeTillNextFrame = timestamp + frameInterval - DateTime.Now;
                if (timeTillNextFrame < TimeSpan.Zero)
                    timeTillNextFrame = TimeSpan.Zero;
            }

            // Wait for the last frame is written
            videoWriteTask?.Wait();
        }

        private void TakeScreenshot(byte[] buffer, RecorderParams recorderParams) {
            using (var BMP = new Bitmap(recorderParams.Width, recorderParams.Height)) {
                using (var g = Graphics.FromImage(BMP)) {
                    g.CopyFromScreen(Point.Empty, Point.Empty, new Size(recorderParams.Width, recorderParams.Height), CopyPixelOperation.SourceCopy);
                    Color red = Color.FromArgb(0x60, 0xff, 0, 0);
                    Brush redBrush = new SolidBrush(red);
                    g.FillEllipse(redBrush, (int) _gazeX - 50, (int) _gazeY - 50, 100, 100);

                    g.Flush();

                    var bits = BMP.LockBits(new Rectangle(0, 0, recorderParams.Width, recorderParams.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
                    Marshal.Copy(bits.Scan0, buffer, 0, buffer.Length);
                    BMP.UnlockBits(bits);
                }
            }
        }
    }
}