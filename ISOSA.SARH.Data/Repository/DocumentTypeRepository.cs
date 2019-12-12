using ISOSA.SARH.Data.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace ISOSA.SARH.Data.Repository
{
    public class DocumentTypeRepository : RepositoryBase<DocumentType>
    {

        public DocumentTypeRepository(string connectionString)
            : base(connectionString, true)
        {
            this._elementType = 102;
        }

        public override void Create(DocumentType Element)
        {
            this._context.Documents.Add(Element);
            this._context.SaveChanges();
            this.UpdateSync();
        }

        public override void Delete(DocumentType Element)
        {
            this._context.Documents.Remove(Element);
            this._context.SaveChanges();
            this.UpdateSync();
        }

        public override IEnumerable<DocumentType> GetAll()
        {
            return this._context.Documents;
        }

        public override DocumentType GetElement(int id)
        {
            return this._context.Documents.Where(d => d.Id.Equals(id)).FirstOrDefault();
        }

        public override IEnumerable<DocumentType> SearhItemsFor(Expression<Func<DocumentType, bool>> predicate)
        {
            return this._context.Documents.Where(predicate);
        }

        public override void Update(DocumentType Element)
        {
            this._context.Documents.Update(Element);
            this._context.SaveChanges();
            this.UpdateSync();
        }

    }
}
