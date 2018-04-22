namespace GazeMonitoring.Model {
    public enum DataStream {
        UnfilteredGaze, //raw
        LightlyFilteredGaze,
        SensitiveFixation,
        SlowFixation
    }
}
