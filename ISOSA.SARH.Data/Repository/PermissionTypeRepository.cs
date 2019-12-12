using ISOSA.SARH.Data.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ISOSA.SARH.Data.Repository
{
    public class PermissionTypeRepository : RepositoryBase<PermissionType>
    {

        public PermissionTypeRepository(string connectionString) :
            base(connectionString, true)
        {
            this._elementType = 105;
        }

        public override void Create(PermissionType Element)
        {
            this._context.Permissions.Add(Element);
            this._context.SaveChanges();
            this.UpdateSync();
        }

        public override void Delete(PermissionType Element)
        {
            this._context.Permissions.Remove(Element);
            this._context.SaveChanges();
            this.UpdateSync();
        }

        public override IEnumerable<PermissionType> GetAll()
        {
            return this._context.Permissions;
        }

        public override PermissionType GetElement(int id)
        {
            return this._context.Permissions.Where(d => d.Id.Equals(id)).FirstOrDefault();
        }

        public override IEnumerable<PermissionType> SearhItemsFor(Expression<Func<PermissionType, bool>> predicate)
        {
            return this._context.Permissions.Where(predicate);
        }

        public override void Update(PermissionType Element)
        {
            this._context.Permissions.Update(Element);
            this._context.SaveChanges();
            this.UpdateSync();
        }

    }
}
