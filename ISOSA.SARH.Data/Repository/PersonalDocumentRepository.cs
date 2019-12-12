using ISOSA.SARH.Data.Domain.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ISOSA.SARH.Data.Repository
{
    public class PersonalDocumentRepository : RepositoryBase<PersonalDocument>
    {

        public PersonalDocumentRepository(string connectionString) 
            : base(connectionString)
        {

        }

        public override void Create(PersonalDocument Element)
        {
            this._context.EmployeeDocuments.Add(Element);
            this._context.SaveChanges();
        }

        public override void Delete(PersonalDocument Element)
        {
            this._context.EmployeeDocuments.Remove(Element);
            this._context.SaveChanges();
        }

        public override IEnumerable<PersonalDocument> GetAll()
        {
            return this._context.EmployeeDocuments;
        }

        public override PersonalDocument GetElement(int id)
        {
            return this._context.EmployeeDocuments.Where(o=>o.Id.Equals(id)).FirstOrDefault();
        }

        public override IEnumerable<PersonalDocument> SearhItemsFor(Expression<Func<PersonalDocument, bool>> predicate)
        {
            return this._context.EmployeeDocuments.Where(predicate);
        }

        public override void Update(PersonalDocument Element)
        {
            this._context.EmployeeDocuments.Update(Element);
            this._context.SaveChanges();
        }

    }
}
