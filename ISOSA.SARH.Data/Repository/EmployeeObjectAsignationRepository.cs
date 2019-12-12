using ISOSA.SARH.Data.Domain.Assignation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ISOSA.SARH.Data.Repository
{
    public class EmployeeObjectAsignationRepository : RepositoryBase<EmployeeObjectAsignation>
    {

        public EmployeeObjectAsignationRepository(string connectionString)
            : base(connectionString)
        {

        }

        public override void Create(EmployeeObjectAsignation Element)
        {
            this._context.EmployeeObjectAsignations.Add(Element);
            this._context.SaveChanges();
        }

        public override void Delete(EmployeeObjectAsignation Element)
        {
            this._context.EmployeeObjectAsignations.Remove(Element);
            this._context.SaveChanges();
        }

        public override IEnumerable<EmployeeObjectAsignation> GetAll()
        {
            return this._context.EmployeeObjectAsignations;
        }

        public override EmployeeObjectAsignation GetElement(int id)
        {
            return this._context.EmployeeObjectAsignations.Where(d => d.Id.Equals(id)).FirstOrDefault();
        }

        public override IEnumerable<EmployeeObjectAsignation> SearhItemsFor(Expression<Func<EmployeeObjectAsignation, bool>> predicate)
        {
            return this._context.EmployeeObjectAsignations.Where(predicate);
        }

        public override void Update(EmployeeObjectAsignation Element)
        {
            this._context.EmployeeObjectAsignations.Update(Element);
            this._context.SaveChanges();
        }

    }
}
