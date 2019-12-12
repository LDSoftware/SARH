using ISOSA.SARH.Data.Domain.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ISOSA.SARH.Data.Repository
{
    public class EmployeeScheduleAssignedRepository : RepositoryBase<EmployeeScheduleAssigned>
    {

        public EmployeeScheduleAssignedRepository(string connectionString)
            : base(connectionString)
        {

        }

        public override void Create(EmployeeScheduleAssigned Element)
        {
            this._context.EmployeesSchedulesAssigned.Add(Element);
            this._context.SaveChanges();
        }

        public override void Delete(EmployeeScheduleAssigned Element)
        {
            this._context.EmployeesSchedulesAssigned.Remove(Element);
            this._context.SaveChanges();
        }

        public override IEnumerable<EmployeeScheduleAssigned> GetAll()
        {
            return this._context.EmployeesSchedulesAssigned;
        }

        public override EmployeeScheduleAssigned GetElement(int id)
        {
            return this._context.EmployeesSchedulesAssigned.Where(p => p.Id.Equals(id)).FirstOrDefault();
        }

        public override IEnumerable<EmployeeScheduleAssigned> SearhItemsFor(Expression<Func<EmployeeScheduleAssigned, bool>> predicate)
        {
            return this._context.EmployeesSchedulesAssigned.Where(predicate);
        }

        public override void Update(EmployeeScheduleAssigned Element)
        {
            this._context.EmployeesSchedulesAssigned.Update(Element);
            this._context.SaveChanges();
        }

    }
}
