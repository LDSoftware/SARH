using ISOSA.SARH.Data.Domain.Employee;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace ISOSA.SARH.Data.Repository
{
    public class NomipaqEmployeesRepository : RepositoryBase<Nomipaq_nom10001>
    {

        public NomipaqEmployeesRepository(string connectionString)
            :base(connectionString)
        {

        }

        public override void Create(Nomipaq_nom10001 Element)
        {
            this._context.NomipaqEmployees.Add(Element);
            this._context.SaveChanges();
        }

        public override void Delete(Nomipaq_nom10001 Element)
        {
            this._context.NomipaqEmployees.Remove(Element);
            this._context.SaveChanges();
        }

        public override IEnumerable<Nomipaq_nom10001> GetAll()
        {
            return this._context.NomipaqEmployees;
        }

        public override Nomipaq_nom10001 GetElement(int id)
        {
            return this._context.NomipaqEmployees.Where(i => i.idempleado.Equals(id)).FirstOrDefault();
        }

        public override IEnumerable<Nomipaq_nom10001> SearhItemsFor(Expression<Func<Nomipaq_nom10001, bool>> predicate)
        {
            return this._context.NomipaqEmployees.Where(predicate);
        }

        public override void Update(Nomipaq_nom10001 Element)
        {
            this._context.NomipaqEmployees.Update(Element);
            this._context.SaveChanges();
        }

    }
}
