using System;
using SharpAvi;
using SharpAvi.Codecs;
using SharpAvi.Output;

namespace GazeMonitoring.ScreenCapture.SharpAvi {
    public class AviVideoStreamFactory {
        public IAviVideoStream CreateVideoStream(AviWriter writer, RecorderParams recorderParams, FourCC codec) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (recorderParams == null) {
                throw new ArgumentNullException(nameof(recorderParams));
            }

            // Select encoder type based on FOURCC of codec
            if (codec == KnownFourCCs.Codecs.Uncompressed)
                return writer.AddUncompressedVideoStream(recorderParams.Width, recorderParams.Height);
            else if (codec == KnownFourCCs.Codecs.MotionJpeg)
                return writer.AddMotionJpegVideoStream(recorderParams.Width, recorderParams.Height, recorderParams.Quality);
            else
            {
                return writer.AddMpeg4VideoStream(recorderParams.Width, recorderParams.Height, (double)writer.FramesPerSecond,
                    // It seems that all tested MPEG-4 VfW codecs ignore the quality affecting parameters passed through VfW API
                    // They only respect the settings from their own configuration dialogs, and Mpeg4VideoEncoder currently has no support for this
                    quality: recorderParams.Quality,
                    codec: codec,
                    // Most of VfW codecs expect single-threaded use, so we wrap this encoder to special wrapper
                    // Thus all calls to the encoder (including its instantiation) will be invoked on a single thread although encoding (and writing) is performed asynchronously
                    forceSingleThreadedAccess: true);
            }
        }
    }
}
