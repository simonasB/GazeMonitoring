using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GazeMonitoring.DataAccess
{
    public interface IConfigurationRepository
    {
        int Save<T>(T entity);
        void Update<T>(T entity);
        void Delete<T>(string id);
        T Search<T>(int id);
        IEnumerable<T> Search<T>();
        IEnumerable<T> Search<T>(Expression<Func<T, bool>> predicate);
        T SearchOne<T>(Expression<Func<T, bool>> predicate);
    }
}
