using ISOSA.SARH.Data.Domain.Employee;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;

namespace ISOSA.SARH.Data.Repository
{
    public class NomipaqEmployeeVacationsRepository : RepositoryBase<Nomipaq_nom10014>
    {

        public NomipaqEmployeeVacationsRepository(string connectionString) : base(connectionString)
        {
        }

        public override void Create(Nomipaq_nom10014 Element)
        {
            throw new NotImplementedException();
        }

        public override void Delete(Nomipaq_nom10014 Element)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Nomipaq_nom10014> GetAll()
        {
            return this._context.NomipaqEmployeeVacations;
        }

        public override Nomipaq_nom10014 GetElement(int id)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Nomipaq_nom10014> SearhItemsFor(Expression<Func<Nomipaq_nom10014, bool>> predicate)
        {
            return this._context.NomipaqEmployeeVacations.Where(predicate);
        }

        public override void Update(Nomipaq_nom10014 Element)
        {
            throw new NotImplementedException();
        }
    }
}
