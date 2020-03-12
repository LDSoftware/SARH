using ISOSA.SARH.Data.Domain.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;

namespace ISOSA.SARH.Data.Repository
{
    public class SARHSettingsRepository : RepositoryBase<SARHSettings>
    {

        public SARHSettingsRepository(string connectionString)
            : base(connectionString)
        {

        }
        public override void Create(SARHSettings Element)
        {
            this._context.SARHSettings.Add(Element);
            this._context.SaveChanges();
        }

        public override void Delete(SARHSettings Element)
        {
            this._context.SARHSettings.Remove(Element);
            this._context.SaveChanges();
        }

        public override IEnumerable<SARHSettings> GetAll()
        {
            return this._context.SARHSettings;
        }

        public override SARHSettings GetElement(int id)
        {
            return this._context.SARHSettings.Where(g=>g.Id.Equals(id)).FirstOrDefault();
        }

        public override IEnumerable<SARHSettings> SearhItemsFor(Expression<Func<SARHSettings, bool>> predicate)
        {
            return this._context.SARHSettings.Where(predicate);
        }

        public override void Update(SARHSettings Element)
        {
            this._context.SARHSettings.Update(Element);
            this._context.SaveChanges();
        }
    }
}
