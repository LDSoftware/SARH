using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ISOSA.SARH.Data;
using ISOSA.SARH.Data.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace ISOSA.SARH.Data.Repository
{
    public abstract class RepositoryBase<T> : IRepository<T>
    {
        protected DataContext _context;
        protected string _connectionString;
        protected IRepository<ElementUpdate> _updateSyncRepository;
        protected int _elementType;
        protected T _getLastAdded;

        public T GetLastAdded { get => _getLastAdded; set => _getLastAdded = value; }

        public RepositoryBase(string connectionString, bool updateSync = false)
        {
            _connectionString = connectionString;
            this._context = new DataContext(_connectionString);

            if (updateSync) 
            {
                _updateSyncRepository = new ElementUpdateRepository(_connectionString);
            }
        }

        protected void UpdateSync() 
        {
            var elements = _updateSyncRepository.SearhItemsFor(o => o.ElementType.Equals(_elementType));
            if (elements.Any()) 
            {
                var element = elements.FirstOrDefault();
                element.UpdateSync = DateTime.Now;
                _updateSyncRepository.Update(element);
            }
        }

        
        public abstract void Create(T Element);

        public abstract void Delete(T Element);

        public abstract IEnumerable<T> GetAll();

        public abstract IEnumerable<T> SearhItemsFor(Expression<Func<T, bool>> predicate);

        public abstract T GetElement(int id);

        public abstract void Update(T Element);

        public virtual void CreateTableBackup(string tableName)
        {
            string sqlCommand = $"SELECT * INTO {tableName}_bkp_D{DateTime.Now.ToString("ddMMyyyy")}_T{DateTime.Now.ToString("hhmmss")} FROM {tableName}";
            _context.Database.ExecuteSqlCommand(sqlCommand);
        }


    }
}
