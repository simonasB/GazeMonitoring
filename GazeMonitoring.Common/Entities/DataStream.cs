namespace GazeMonitoring.Common.Entities {
    public enum DataStream {
        UnfilteredGaze, //raw
        LightlyFilteredGaze,
        SensitiveFixation,
        SlowFixation
    }
}
