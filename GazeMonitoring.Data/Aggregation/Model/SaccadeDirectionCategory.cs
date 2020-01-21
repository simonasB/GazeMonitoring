namespace GazeMonitoring.Data.Aggregation.Model
{
    public enum SaccadeDirectionCategory
    {
        // [11.25;-11.25](348.75-360]
        E = 1,
        // (11.25;33.75]
        ENE = 2,
        // Etc, +50 degrees
        NE = 3,
        NNE = 4,
        N = 5,
        NNW = 6,
        NW = 7,
        WNW = 8,
        W = 9,
        WSW = 10,
        SW = 11,
        SSW = 12,
        S = 13,
        SSE = 14,
        SE = 15,
        ESE = 16
    }
}