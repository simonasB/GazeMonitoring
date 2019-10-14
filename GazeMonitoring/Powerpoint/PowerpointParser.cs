using System;
using System.Collections.Generic;
using PPt = Microsoft.Office.Interop.PowerPoint;
using System.Runtime.InteropServices;
using GazeMonitoring.Model;

namespace GazeMonitoring.Powerpoint
{
    public class PowerpointParser
    {
        private readonly IScreenParameters _screenParameters;

        public PowerpointParser(IScreenParameters screenParameters)
        {
            _screenParameters = screenParameters;
        }

        // TODO Create wrapper for ppt classes for better testability
        public IEnumerable<ScreenConfiguration> Parse()
        {
            var result = new List<ScreenConfiguration>();
            try
            {
                var pptApplication = Marshal.GetActiveObject("PowerPoint.Application") as PPt.Application;

                if (pptApplication == null)
                {
                    throw new Exception("Could not load powerpoint presentation.");
                }

                var heightScale = _screenParameters.Height / ConvertToPixels(pptApplication.ActivePresentation.PageSetup.SlideHeight);
                var widthScale = _screenParameters.Width / ConvertToPixels(pptApplication.ActivePresentation.PageSetup.SlideWidth);

                foreach (PPt.Slide activePresentationSlide in pptApplication.ActivePresentation.Slides)
                {
                    var screenConfiguration = new ScreenConfiguration
                    {
                        AreasOfInterest = new List<AreaOfInterest>(),
                        Id = Guid.NewGuid().ToString(),
                        Name = $"Slide{activePresentationSlide.SlideNumber}",
                        Number = activePresentationSlide.SlideNumber
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

            return result;
        }

        private static double ConvertToPixels(double points) => points * 4 / 3;
    }
}
