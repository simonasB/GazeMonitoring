﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using GazeMonitoring.Common;
using LiteDB;

namespace GazeMonitoring.DataAccess.LiteDB
{
    public class LiteDBConfigurationRepository : IConfigurationRepository
    {
        private readonly string _connectionString; 

        public LiteDBConfigurationRepository(IAppDataHelper appDataHelper)
        {
            _connectionString = Path.Combine(appDataHelper.GetAppDataDirectoryPath(), "MyData.db");
        }

        public int SaveMany<T>(IEnumerable<T> entities)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var col = db.GetCollection<T>(typeof(T).Name);
                
                return col.Insert(entities);
            }
        }

        public void Save<T>(T entity)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var col = db.GetCollection<T>(typeof(T).Name);
                col.Upsert(entity);
            }
        }

        public void Update<T>(T entity)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var col = db.GetCollection<T>(typeof(T).Name);

                col.Update(entity);
            }
        }

        public void Delete<T>(int id)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var col = db.GetCollection<T>(typeof(T).Name);

                col.Delete(id);
            }
        }
        public T Search<T>(int id)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var col = db.GetCollection<T>(typeof(T).Name);
                return col.FindById(id);
            }
        }

        public IEnumerable<T> Search<T>()
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var col = db.GetCollection<T>(typeof(T).Name);
                return col.FindAll();
            }
        }

        public IEnumerable<T> Search<T>(Expression<Func<T, bool>> predicate)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var col = db.GetCollection<T>(typeof(T).Name);
                return col.Find(predicate);
            }
        }

        public T SearchOne<T>(Expression<Func<T, bool>> predicate)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var col = db.GetCollection<T>(typeof(T).Name);
                return col.FindOne(predicate);
            }
        }
    }
}
