using ISOSA.SARH.Data.Domain.Employee;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;

namespace ISOSA.SARH.Data.Repository
{
    public class EmployeeOrganigramaRepository : RepositoryBase<EmployeeOrganigrama>
    {
        public EmployeeOrganigramaRepository(string connectionSting)
            :base(connectionSting)
        {

        }

        public override void Create(EmployeeOrganigrama Element)
        {
            this._context.EmployeesOrganigrama.Add(Element);
            this._context.SaveChanges();
        }

        public override void Delete(EmployeeOrganigrama Element)
        {
            this._context.EmployeesOrganigrama.Remove(Element);
            this._context.SaveChanges();
        }

        public override IEnumerable<EmployeeOrganigrama> GetAll()
        {
            return this._context.EmployeesOrganigrama;
        }

        public override EmployeeOrganigrama GetElement(int id)
        {
            return this._context.EmployeesOrganigrama.FirstOrDefault();
        }

        public override IEnumerable<EmployeeOrganigrama> SearhItemsFor(Expression<Func<EmployeeOrganigrama, bool>> predicate)
        {
            return this._context.EmployeesOrganigrama.Where(predicate);
        }

        public override void Update(EmployeeOrganigrama Element)
        {
            this._context.EmployeesOrganigrama.Update(Element);
            this._context.SaveChanges();
        }

    }
}
