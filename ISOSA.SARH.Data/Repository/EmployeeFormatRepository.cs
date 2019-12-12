using ISOSA.SARH.Data.Domain.Formats;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Linq;

namespace ISOSA.SARH.Data.Repository
{
    public class EmployeeFormatRepository : RepositoryBase<EmployeeFormat>
    {

        public EmployeeFormatRepository(string connectionString)
            : base(connectionString)
        {

        }

        public override void Create(EmployeeFormat Element)
        {
            this._context.EmployeeFormats.Add(Element);
            this._context.SaveChanges();
        }

        public override void Delete(EmployeeFormat Element)
        {
            this._context.EmployeeFormats.Remove(Element);
            this._context.SaveChanges();
        }

        public override IEnumerable<EmployeeFormat> GetAll()
        {
            return this._context.EmployeeFormats;
        }

        public override EmployeeFormat GetElement(int id)
        {
            return this._context.EmployeeFormats.Where(p => p.Id.Equals(id)).FirstOrDefault();
        }

        public override IEnumerable<EmployeeFormat> SearhItemsFor(Expression<Func<EmployeeFormat, bool>> predicate)
        {
            return this._context.EmployeeFormats.Where(predicate);
        }

        public override void Update(EmployeeFormat Element)
        {
            this._context.EmployeeFormats.Update(Element);
            this._context.SaveChanges();
        }

    }
}
