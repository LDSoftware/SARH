using ISOSA.SARH.Data.Domain.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ISOSA.SARH.Data.Repository
{
    public class NonWorkingDayExceptionRepository : RepositoryBase<NonWorkingDayException>
    {

        public NonWorkingDayExceptionRepository(string connectionString):base(connectionString)
        {

        }

        public override void Create(NonWorkingDayException Element)
        {
            this._context.NonWorkingDayExceptions.Add(Element);
            this._context.SaveChanges();
        }

        public override void Delete(NonWorkingDayException Element)
        {
            this._context.NonWorkingDayExceptions.Remove(Element);
            this._context.SaveChanges();
        }

        public override IEnumerable<NonWorkingDayException> GetAll()
        {
            return this._context.NonWorkingDayExceptions;
        }

        public override NonWorkingDayException GetElement(int id)
        {
            return this._context.NonWorkingDayExceptions.Where(i => i.Id.Equals(id)).FirstOrDefault();
        }
        public override IEnumerable<NonWorkingDayException> SearhItemsFor(Expression<Func<NonWorkingDayException, bool>> predicate)
        {
            return this._context.NonWorkingDayExceptions.Where(predicate);
        }

        public override void Update(NonWorkingDayException Element)
        {
            this._context.NonWorkingDayExceptions.Update(Element);
            this._context.SaveChanges();
        }


    }
}
