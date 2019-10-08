using LiteDB;

namespace GazeMonitoring.DataAccess.LiteDB
{
    public class LiteDBConfigurationRepository : IConfigurationRepository
    {
        public LiteDBConfigurationRepository()
        {
            
        }

        public int Save<T>(T entity)
        {
            using (var db = new LiteDatabase(@"C:\Temp\MyData.db"))
            {
                var col = db.GetCollection<T>(typeof(T).Name);

                return col.Insert(entity).AsInt32;
            }
        }

        public void Update<T>(T entity)
        {
            using (var db = new LiteDatabase(@"C:\Temp\MyData.db"))
            {
                var col = db.GetCollection<T>(typeof(T).Name);

                col.Update(entity);
            }
        }

        public void Delete<T>(string id)
        {
            using (var db = new LiteDatabase(@"C:\Temp\MyData.db"))
            {
                var col = db.GetCollection<T>(typeof(T).Name);

                col.Delete(id);
            }
        }
        public T Search<T>(int id)
        {
            using (var db = new LiteDatabase(@"C:\Temp\MyData.db"))
            {
                var col = db.GetCollection<T>(typeof(T).Name);

                return col.FindById(id);
            }
        }
    }
}
