using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ISOSA.SARH.Data.Repository
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();

        IEnumerable<T> SearhItemsFor(Expression<Func<T, bool>> predicate);

        T GetElement(int id);

        void Update(T Element);

        void Delete(T Element);

        void Create(T Element);

        T GetLastAdded { get; set; }

        void CreateTableBackup(string tableName);
    }
}
