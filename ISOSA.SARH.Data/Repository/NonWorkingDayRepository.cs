using ISOSA.SARH.Data.Domain.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ISOSA.SARH.Data.Repository
{
    public class NonWorkingDayRepository : RepositoryBase<NonWorkingDay>
    {

        public NonWorkingDayRepository(string connectionString)
            :base(connectionString)
        {

        }

        public override void Create(NonWorkingDay Element)
        {
            this._context.NonWorkingDays.Add(Element);
            this._context.SaveChanges();
            this._getLastAdded = Element;
        }

        public override void Delete(NonWorkingDay Element)
        {
            this._context.NonWorkingDays.Remove(Element);
            this._context.SaveChanges();
        }

        public override IEnumerable<NonWorkingDay> GetAll()
        {
            return this._context.NonWorkingDays;
        }

        public override NonWorkingDay GetElement(int id)
        {
            return this._context.NonWorkingDays.Where(o => o.Id.Equals(id)).FirstOrDefault();
        }

        public override IEnumerable<NonWorkingDay> SearhItemsFor(Expression<Func<NonWorkingDay, bool>> predicate)
        {
            return this._context.NonWorkingDays.Where(predicate);
        }

        public override void Update(NonWorkingDay Element)
        {
            this._context.NonWorkingDays.Update(Element);
            this._context.SaveChanges();
        }

    }
}
