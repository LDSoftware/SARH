using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ISOSA.SARH.Data.Domain.Catalog;
using System.Linq.Expressions;

namespace ISOSA.SARH.Data.Repository
{
    public class SafeEquipmentAssignedRepository : RepositoryBase<SafeEquipmentAssigned>
    {

        public SafeEquipmentAssignedRepository(string connectionString)
            : base(connectionString, true)
        {
            this._elementType = 104;
        }

        public override void Create(SafeEquipmentAssigned Element)
        {
            this._context.SafeEquiment.Add(Element);
            this._context.SaveChanges();
            this.UpdateSync();
        }

        public override void Delete(SafeEquipmentAssigned Element)
        {
            this._context.SafeEquiment.Remove(Element);
            this._context.SaveChanges();
            this.UpdateSync();
        }

        public override IEnumerable<SafeEquipmentAssigned> GetAll()
        {
            return this._context.SafeEquiment;
        }

        public override SafeEquipmentAssigned GetElement(int id)
        {
            return this._context.SafeEquiment.Where(d => d.Id.Equals(id)).FirstOrDefault();
        }

        public override IEnumerable<SafeEquipmentAssigned> SearhItemsFor(Expression<Func<SafeEquipmentAssigned, bool>> predicate)
        {
            return this._context.SafeEquiment.Where(predicate);
        }

        public override void Update(SafeEquipmentAssigned Element)
        {
            this._context.SafeEquiment.Update(Element);
            this._context.SaveChanges();
            this.UpdateSync();
        }

    }
}
