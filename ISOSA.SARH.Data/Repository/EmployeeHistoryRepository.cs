using ISOSA.SARH.Data.Domain.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ISOSA.SARH.Data.Repository
{
    public class EmployeeHistoryRepository :  RepositoryBase<EmployeeHistory>
    {

        public EmployeeHistoryRepository(string connectionString) : base(connectionString)
        {

        }

        public override void Create(EmployeeHistory Element)
        {
            this._context.EmployeeHistory.Add(Element);
            this._context.SaveChanges();
        }

        public override void Delete(EmployeeHistory Element)
        {
            this._context.EmployeeHistory.Remove(Element);
            this._context.SaveChanges();
        }

        public override IEnumerable<EmployeeHistory> GetAll()
        {
            return this._context.EmployeeHistory;
        }

        public override EmployeeHistory GetElement(int id)
        {
            return this._context.EmployeeHistory.Where(i => i.Id.Equals(id)).FirstOrDefault();
        }

        public override IEnumerable<EmployeeHistory> SearhItemsFor(Expression<Func<EmployeeHistory, bool>> predicate)
        {
            return this._context.EmployeeHistory.Where(predicate);
        }

        public override void Update(EmployeeHistory Element)
        {
            this._context.EmployeeHistory.Update(Element);
            this._context.SaveChanges();
        }

    }
}
