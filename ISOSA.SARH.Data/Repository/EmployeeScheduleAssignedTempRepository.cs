using ISOSA.SARH.Data.Domain.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ISOSA.SARH.Data.Repository
{
    public class EmployeeScheduleAssignedTempRepository : RepositoryBase<EmployeeScheduleAssignedTemp>
    {

        public EmployeeScheduleAssignedTempRepository(string connectionString)
            :base(connectionString)
        {

        }

        public override void Create(EmployeeScheduleAssignedTemp Element)
        {
            this._context.EmployeesSchedulesAssignedTemp.Add(Element);
            this._context.SaveChanges();
        }

        public override void Delete(EmployeeScheduleAssignedTemp Element)
        {
            this._context.EmployeesSchedulesAssignedTemp.Remove(Element);
            this._context.SaveChanges();
        }

        public override IEnumerable<EmployeeScheduleAssignedTemp> GetAll()
        {
            return this._context.EmployeesSchedulesAssignedTemp;
        }

        public override EmployeeScheduleAssignedTemp GetElement(int id)
        {
            return this._context.EmployeesSchedulesAssignedTemp.Where(i => i.Id.Equals(id)).FirstOrDefault();
        }

        public override IEnumerable<EmployeeScheduleAssignedTemp> SearhItemsFor(Expression<Func<EmployeeScheduleAssignedTemp, bool>> predicate)
        {
            return this._context.EmployeesSchedulesAssignedTemp.Where(predicate);
        }

        public override void Update(EmployeeScheduleAssignedTemp Element)
        {
            this._context.EmployeesSchedulesAssignedTemp.Update(Element);
            this._context.SaveChanges();

        }
    }
}
