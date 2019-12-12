using ISOSA.SARH.Data.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ISOSA.SARH.Data.Repository
{
    public class HardwareAssignedRepository : RepositoryBase<HardwareAssigned>
    {

        public HardwareAssignedRepository(string connectionString) :
            base(connectionString, true)
        {
            this._elementType = 103;
        }

        public override void Create(HardwareAssigned Element)
        {
            this._context.Hardware.Add(Element);
            this._context.SaveChanges();
            this.UpdateSync();
        }

        public override void Delete(HardwareAssigned Element)
        {
            this._context.Hardware.Remove(Element);
            this._context.SaveChanges();
            this.UpdateSync();
        }

        public override IEnumerable<HardwareAssigned> GetAll()
        {
            return this._context.Hardware;
        }

        public override HardwareAssigned GetElement(int id)
        {
            return this._context.Hardware.Where(d => d.Id.Equals(id)).FirstOrDefault();
        }

        public override IEnumerable<HardwareAssigned> SearhItemsFor(Expression<Func<HardwareAssigned, bool>> predicate)
        {
            return this._context.Hardware.Where(predicate);
        }

        public override void Update(HardwareAssigned Element)
        {
            this._context.Hardware.Update(Element);
            this._context.SaveChanges();
            this.UpdateSync();
        }

    }
}
