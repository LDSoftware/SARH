using ISOSA.SARH.Data.Domain.Process;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;

namespace ISOSA.SARH.Data.Repository
{
    public class NomipaqIncidenciasRepository : RepositoryBase<Nomipaq_nom10010>
    {
        public NomipaqIncidenciasRepository(string connectionString)
            : base(connectionString)
        {

        }

        public override void Create(Nomipaq_nom10010 Element)
        {
            throw new NotImplementedException();
        }

        public override void Delete(Nomipaq_nom10010 Element)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Nomipaq_nom10010> GetAll()
        {
            return this._context.NomipaqIncidencias;
        }

        public override Nomipaq_nom10010 GetElement(int id)
        {
            return this._context.NomipaqIncidencias.Where(h => h.idmovtodyh.Equals(id)).FirstOrDefault();
        }

        public override IEnumerable<Nomipaq_nom10010> SearhItemsFor(Expression<Func<Nomipaq_nom10010, bool>> predicate)
        {
            return this._context.NomipaqIncidencias.Where(predicate);
        }

        public override void Update(Nomipaq_nom10010 Element)
        {
            throw new NotImplementedException();
        }
    }
}
