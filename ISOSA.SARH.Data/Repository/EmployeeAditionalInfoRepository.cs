using ISOSA.SARH.Data.Domain.Employee;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace ISOSA.SARH.Data.Repository
{
    public class EmployeeAditionalInfoRepository : RepositoryBase<EmployeeAditionalInfo>
    {
        public EmployeeAditionalInfoRepository(string connectionString)
            :base(connectionString)
        {

        }

        public override void Create(EmployeeAditionalInfo Element)
        {
            this._context.Employees.Add(Element);
            this._context.SaveChanges();
        }

        public override void Delete(EmployeeAditionalInfo Element)
        {
            this._context.Employees.Remove(Element);
            this._context.SaveChanges();
        }

        public override IEnumerable<EmployeeAditionalInfo> GetAll()
        {
            return this._context.Employees;
        }

        public override EmployeeAditionalInfo GetElement(int id)
        {
            return this._context.Employees.Where(i=>i.EMP_EmployeeID.Equals(id)).FirstOrDefault();
        }

        public override IEnumerable<EmployeeAditionalInfo> SearhItemsFor(Expression<Func<EmployeeAditionalInfo, bool>> predicate)
        {
            return this._context.Employees.Where(predicate);
        }

        public override void Update(EmployeeAditionalInfo Element)
        {
            this._context.Employees.Update(Element);
            this._context.SaveChanges();
        }

    }
}
