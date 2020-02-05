using ISOSA.SARH.Data.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ISOSA.SARH.Data.Repository
{
    public class EmployeeNotifyRepository : RepositoryBase<EmployeeNotify>
    {
        public EmployeeNotifyRepository(string connectionString)
            : base(connectionString)
        {

        }

        public override void Create(EmployeeNotify Element)
        {
            this._context.EmployeeNotifications.Add(Element);
            this._context.SaveChanges();
        }

        public override void Delete(EmployeeNotify Element)
        {
            this._context.EmployeeNotifications.Remove(Element);
            this._context.SaveChanges();
        }

        public override IEnumerable<EmployeeNotify> GetAll()
        {
            return this._context.EmployeeNotifications;
        }

        public override EmployeeNotify GetElement(int id)
        {
            return this._context.EmployeeNotifications.Where(d => d.Id.Equals(id)).FirstOrDefault();
        }

        public override IEnumerable<EmployeeNotify> SearhItemsFor(Expression<Func<EmployeeNotify, bool>> predicate)
        {
            return this._context.EmployeeNotifications.Where(predicate);
        }

        public override void Update(EmployeeNotify Element)
        {
            this._context.EmployeeNotifications.Update(Element);
            this._context.SaveChanges();
        }

    }
}
