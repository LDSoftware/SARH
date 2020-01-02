using ISOSA.SARH.Data.Domain.Formats;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;

namespace ISOSA.SARH.Data.Repository
{
    public class FormatApprobedEmployeeRepository : RepositoryBase<FormatApprovedEmployee>
    {
        public FormatApprobedEmployeeRepository(string connectionString) 
            : base(connectionString)
        {

        }

        public override void Create(FormatApprovedEmployee Element)
        {
            this._context.FormatApprobedEmployees.Add(Element);
            this._context.SaveChanges();
        }

        public override void Delete(FormatApprovedEmployee Element)
        {
            this._context.FormatApprobedEmployees.Remove(Element);
            this._context.SaveChanges();
        }

        public override IEnumerable<FormatApprovedEmployee> GetAll()
        {
            return this._context.FormatApprobedEmployees;
        }

        public override FormatApprovedEmployee GetElement(int id)
        {
            return this._context.FormatApprobedEmployees.Where(d => d.Id.Equals(id)).FirstOrDefault();
        }

        public override IEnumerable<FormatApprovedEmployee> SearhItemsFor(Expression<Func<FormatApprovedEmployee, bool>> predicate)
        {
            return this._context.FormatApprobedEmployees;
        }

        public override void Update(FormatApprovedEmployee Element)
        {
            this._context.FormatApprobedEmployees.Add(Element);
            this._context.SaveChanges();
        }

    }
}
