using System;
using System.Collections.Generic;
using PPt = Microsoft.Office.Interop.PowerPoint;
using System.Runtime.InteropServices;
using GazeMonitoring.Model;
using Microsoft.Office.Core;

namespace GazeMonitoring.Powerpoint
{
    public interface IPowerpointParser
    {
        IEnumerable<ScreenConfiguration> Parse(string fileName);
    }

    public class PowerpointParser : IPowerpointParser
    {
        private readonly IScreenParameters _screenParameters;

        public PowerpointParser(IScreenParameters screenParameters)
        {
            _screenParameters = screenParameters;
        }

        // TODO Create wrapper for ppt classes for better testability
        public IEnumerable<ScreenConfiguration> Parse(string fileName)
        {
            var result = new List<ScreenConfiguration>();
            var pptApp = new PPt.Application();
            var pptPresentations = pptApp.Presentations;
            PPt.Presentation presentation = null;
            try
            {
                presentation = pptPresentations.Open(fileName, MsoTriState.msoTrue, MsoTriState.msoFalse,
                    MsoTriState.msoFalse);

                if (presentation == null)
                {
                    throw new Exception("Could not load powerpoint presentation.");
                }

                var heightScale = _screenParameters.Height / ConvertToPixels(presentation.PageSetup.SlideHeight);
                var widthScale = _screenParameters.Width / ConvertToPixels(presentation.PageSetup.SlideWidth);

                foreach (PPt.Slide activePresentationSlide in presentation.Slides)
                {
                    var screenConfiguration = new ScreenConfiguration
                    {
                        AreasOfInterest = new List<AreaOfInterest>(),
                        Id = Guid.NewGuid().ToString(),
                        Name = $"Slide{activePresentationSlide.SlideNumber}",
                        Number = activePresentationSlide.SlideNumber - 1,
                        Duration = TimeSpan.FromSeconds(0)
                    };

                    foreach (PPt.Shape shape in activePresentationSlide.Shapes)
                    {
                        screenConfiguration.AreasOfInterest.Add(new AreaOfInterest
                        {
                            Height = ConvertToPixels(shape.Height) * heightScale,
                            Width = ConvertToPixels(shape.Width) * widthScale,
                            Id = Guid.NewGuid().ToString(),
                            Left = ConvertToPixels(shape.Left) * widthScale,
                            Top = ConvertToPixels(shape.Top) * heightScale,
                            Name = shape.Name
                        });
                    }

                    result.Add(screenConfiguration);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                presentation?.Close();
            }

            return result;
        }

        private static double ConvertToPixels(double points) => points * 4 / 3;
    }
}
