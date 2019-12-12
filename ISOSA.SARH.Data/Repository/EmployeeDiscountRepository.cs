using ISOSA.SARH.Data.Domain.Configuration;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Linq;

namespace ISOSA.SARH.Data.Repository
{
    public class EmployeeDiscountRepository : RepositoryBase<EmployeeDiscount>
    {

        public EmployeeDiscountRepository(string connectionString) : 
            base(connectionString)
        {

        }

        public override void Create(EmployeeDiscount Element)
        {
            this._context.EmployeeDiscount.Add(Element);
            this._context.SaveChanges();
        }

        public override void Delete(EmployeeDiscount Element)
        {
            this._context.EmployeeDiscount.Remove(Element);
            this._context.SaveChanges();
        }

        public override IEnumerable<EmployeeDiscount> GetAll()
        {
            return this._context.EmployeeDiscount;
        }

        public override EmployeeDiscount GetElement(int id)
        {
            return this._context.EmployeeDiscount.Where(d => d.Id.Equals(id)).FirstOrDefault();
        }

        public override IEnumerable<EmployeeDiscount> SearhItemsFor(Expression<Func<EmployeeDiscount, bool>> predicate)
        {
            return this._context.EmployeeDiscount.Where(predicate);
        }

        public override void Update(EmployeeDiscount Element)
        {
            this._context.EmployeeDiscount.Update(Element);
            this._context.SaveChanges();
        }

    }
}
