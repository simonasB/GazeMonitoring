using System;
using System.Collections.Generic;
using PPt = Microsoft.Office.Interop.PowerPoint;
using System.Runtime.InteropServices;
using GazeMonitoring.Model;

namespace GazeMonitoring.Powerpoint
{
    public class PowerpointParser
    {
        public IEnumerable<ScreenConfiguration> Parse()
        {
            var pptApplication = Marshal.GetActiveObject("PowerPoint.Application") as PPt.Application;

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
                    screenConfiguration.AreasOfInterest.Add(new AreaOfInterest {
                        Height = shape.Height,
                        Width = shape.Width,
                        Id = Guid.NewGuid().ToString(),
                        Left = shape.Left,
                        Name = shape.Name,
                        Top = shape.Top
                    });
                }

                yield return screenConfiguration;
            }
        }
    }
}
