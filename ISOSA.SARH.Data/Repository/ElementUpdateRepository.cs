using ISOSA.SARH.Data.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ISOSA.SARH.Data.Repository
{
    public class ElementUpdateRepository : RepositoryBase<ElementUpdate>
    {

        public ElementUpdateRepository(string connectionString)
            :base(connectionString)
        {

        }

        public override void Create(ElementUpdate Element)
        {
            this._context.ElementsUpdate.Add(Element);
            this._context.SaveChanges();
        }

        public override void Delete(ElementUpdate Element)
        {
            this._context.ElementsUpdate.Remove(Element);
            this._context.SaveChanges();
        }

        public override IEnumerable<ElementUpdate> GetAll()
        {
            return this._context.ElementsUpdate;
        }

        public override ElementUpdate GetElement(int id)
        {
            return this._context.ElementsUpdate.Where(i => i.Id.Equals(id)).FirstOrDefault();
        }

        public override IEnumerable<ElementUpdate> SearhItemsFor(Expression<Func<ElementUpdate, bool>> predicate)
        {
            return this._context.ElementsUpdate.Where(predicate);
        }

        public override void Update(ElementUpdate Element)
        {
            this._context.ElementsUpdate.Update(Element);
            this._context.SaveChanges();
        }

    }
}
