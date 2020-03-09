using ISOSA.SARH.Data.Domain.Process;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;

namespace ISOSA.SARH.Data.Repository
{
    public class NomipaqMnemonicosRepository : RepositoryBase<Nomipaq_nom10022>
    {
        public NomipaqMnemonicosRepository(string connectionString)
            : base(connectionString)
        {

        }

        public override void Create(Nomipaq_nom10022 Element)
        {
            throw new NotImplementedException();
        }

        public override void Delete(Nomipaq_nom10022 Element)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Nomipaq_nom10022> GetAll()
        {
            return this._context.NomipaqMnemonicos;
        }

        public override Nomipaq_nom10022 GetElement(int id)
        {
            return this._context.NomipaqMnemonicos.Where(g => g.idtipoincidencia.Equals(id)).FirstOrDefault();
        }

        public override IEnumerable<Nomipaq_nom10022> SearhItemsFor(Expression<Func<Nomipaq_nom10022, bool>> predicate)
        {
            return this._context.NomipaqMnemonicos.Where(predicate);
        }

        public override void Update(Nomipaq_nom10022 Element)
        {
            throw new NotImplementedException();
        }
    }
}
