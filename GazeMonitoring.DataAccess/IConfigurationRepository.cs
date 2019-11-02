using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GazeMonitoring.DataAccess
{
    public interface IConfigurationRepository
    {
        int SaveMany<T>(IEnumerable<T> entities);
        void Save<T>(T entity);
        void Update<T>(T entity);
        void Delete<T>(int id);
        T Search<T>(int id);
        IEnumerable<T> Search<T>();
        IEnumerable<T> Search<T>(Expression<Func<T, bool>> predicate);
        T SearchOne<T>(Expression<Func<T, bool>> predicate);
    }
}
