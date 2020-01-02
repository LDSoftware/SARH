using ISOSA.SARH.Data.Domain.Formats;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;

namespace ISOSA.SARH.Data.Repository
{
    public class FormatApprobedHubIdRepository : RepositoryBase<FormatApprovedHubId>
    {

        public FormatApprobedHubIdRepository(string connectionString) 
            : base(connectionString)
        {

        }

        public override void Create(FormatApprovedHubId Element)
        {
            this._context.FormatApprobedHubIds.Add(Element);
            this._context.SaveChanges();
        }

        public override void Delete(FormatApprovedHubId Element)
        {
            this._context.FormatApprobedHubIds.Remove(Element);
            this._context.SaveChanges();
        }

        public override IEnumerable<FormatApprovedHubId> GetAll()
        {
            return this._context.FormatApprobedHubIds;
        }

        public override FormatApprovedHubId GetElement(int id)
        {
            return this._context.FormatApprobedHubIds.Where(d => d.Id.Equals(id)).FirstOrDefault();
        }

        public override IEnumerable<FormatApprovedHubId> SearhItemsFor(Expression<Func<FormatApprovedHubId, bool>> predicate)
        {
            return this._context.FormatApprobedHubIds.Where(predicate);
        }

        public override void Update(FormatApprovedHubId Element)
        {
            this._context.FormatApprobedHubIds.Update(Element);
            this._context.SaveChanges();
        }

    }
}
