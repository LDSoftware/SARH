using ISOSA.SARH.Data.Domain.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ISOSA.SARH.Data.Repository
{
    public class EmployeeScheduleDateRepository : RepositoryBase<EmployeeScheduleDate>
    {

        public EmployeeScheduleDateRepository(string connectionString)
            : base(connectionString)
        {

        }


        public override void Create(EmployeeScheduleDate Element)
        {
            throw new NotImplementedException();
        }

        public override void Delete(EmployeeScheduleDate Element)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<EmployeeScheduleDate> GetAll()
        {
            return this._context.EmployeeScheduleDate;
        }

        public override EmployeeScheduleDate GetElement(int id)
        {
            return this._context.EmployeeScheduleDate.Where(o => o.Id.Equals(id)).FirstOrDefault();
        }

        public override IEnumerable<EmployeeScheduleDate> SearhItemsFor(Expression<Func<EmployeeScheduleDate, bool>> predicate)
        {
            return this._context.EmployeeScheduleDate.Where(predicate);
        }

        public override void Update(EmployeeScheduleDate Element)
        {
            throw new NotImplementedException();
        }
    }
}
