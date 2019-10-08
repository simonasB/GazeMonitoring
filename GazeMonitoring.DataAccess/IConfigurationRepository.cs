namespace GazeMonitoring.DataAccess
{
    public interface IConfigurationRepository
    {
        int Save<T>(T entity);
        void Update<T>(T entity);
        void Delete<T>(string id);
        T Search<T>(int id);
    }
}
