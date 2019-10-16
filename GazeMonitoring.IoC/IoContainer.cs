namespace GazeMonitoring.IoC
{
    public interface IoContainer
    {
        bool IsRegistered<T>();

        T GetInstance<T>();
    }
}
