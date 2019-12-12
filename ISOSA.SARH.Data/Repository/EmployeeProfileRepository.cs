using ISOSA.SARH.Data.Domain.Employee;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace ISOSA.SARH.Data.Repository
{
    public class EmployeeProfileRepository : RepositoryBase<EmployeeProfile>
    {
        public EmployeeProfileRepository(string connectionString) 
            : base(connectionString)
        {

        }

        public override void Create(EmployeeProfile Element)
        {
            this._context.EmployeeProfiles.Add(Element);
            this._context.SaveChanges();
        }

        public override void Delete(EmployeeProfile Element)
        {
            this._context.EmployeeProfiles.Remove(Element);
            this._context.SaveChanges();
        }

        public override IEnumerable<EmployeeProfile> GetAll()
        {
            return this._context.EmployeeProfiles;
        }

        public override EmployeeProfile GetElement(int id)
        {
            return this._context.EmployeeProfiles.Where(d => d.Id.Equals(id)).FirstOrDefault();
        }

        public override IEnumerable<EmployeeProfile> SearhItemsFor(Expression<Func<EmployeeProfile, bool>> predicate)
        {
            return this._context.EmployeeProfiles.Where(predicate);
        }

        public override void Update(EmployeeProfile Element)
        {
            this._context.EmployeeProfiles.Update(Element);
            this._context.SaveChanges();
        }

    }
}
