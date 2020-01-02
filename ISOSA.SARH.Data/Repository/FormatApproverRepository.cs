using ISOSA.SARH.Data.Domain.Formats;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;

namespace ISOSA.SARH.Data.Repository
{
    public class FormatApproverRepository : RepositoryBase<FormatApprover>
    {

        public FormatApproverRepository(string connectionString) 
            : base(connectionString)
        {

        }

        public override void Create(FormatApprover Element)
        {
            this._context.FormatApprovers.Add(Element);
            this._context.SaveChanges();
        }

        public override void Delete(FormatApprover Element)
        {
            this._context.FormatApprovers.Remove(Element);
            this._context.SaveChanges();
        }

        public override IEnumerable<FormatApprover> GetAll()
        {
            return this._context.FormatApprovers;
        }

        public override FormatApprover GetElement(int id)
        {
            return this._context.FormatApprovers.Where(d => d.Id.Equals(id)).FirstOrDefault();
        }

        public override IEnumerable<FormatApprover> SearhItemsFor(Expression<Func<FormatApprover, bool>> predicate)
        {
            return this._context.FormatApprovers.Where(predicate);
        }

        public override void Update(FormatApprover Element)
        {
            this._context.FormatApprovers.Update(Element);
            this._context.SaveChanges();
        }

    }
}
