using ISOSA.SARH.Data.Domain.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ISOSA.SARH.Data.Repository
{
    public class ScheduleRepository : RepositoryBase<Schedule>
    {

        public ScheduleRepository(string connectionString)
            : base(connectionString, true)
        {
            this._elementType = 201;
        }

        public override void Create(Schedule Element)
        {
            this._context.Schedules.Add(Element);
            this._context.SaveChanges();
            this.UpdateSync();
        }

        public override void Delete(Schedule Element)
        {
            this._context.Schedules.Add(Element);
            this._context.SaveChanges();
            this.UpdateSync();
        }

        public override IEnumerable<Schedule> GetAll()
        {
            return this._context.Schedules;
        }

        public override Schedule GetElement(int id)
        {
            return this._context.Schedules.Where(i => i.Id.Equals(id)).FirstOrDefault();
        }

        public override IEnumerable<Schedule> SearhItemsFor(Expression<Func<Schedule, bool>> predicate)
        {
            return this._context.Schedules.Where(predicate);
        }

        public override void Update(Schedule Element)
        {
            this._context.Schedules.Update(Element);
            this._context.SaveChanges();
            this.UpdateSync();
        }

    }
}
