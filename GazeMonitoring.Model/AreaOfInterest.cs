namespace GazeMonitoring.Model
{
    public class AreaOfInterest
    {
        public double Top { get; set; }
        public double Left { get; set; }
        public double Width  { get; set; }
        public double Height { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }

        public bool IsPointInArea(GazePoint gazePoint)
        {
            return gazePoint.X >= Left && Left + Width >= gazePoint.X && gazePoint.Y >= Top && Top + Height >= gazePoint.Y;
        }
    }
}
