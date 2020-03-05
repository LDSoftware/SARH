using ISOSA.SARH.Data.Domain.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ISOSA.SARH.Data.Repository
{
    public class NOMIPAQVacationRepository : RepositoryBase<NOMIPAQVacation>
    {

        public NOMIPAQVacationRepository(string connectionString)
            : base(connectionString)
        {

        }

        public override void Create(NOMIPAQVacation Element)
        {
            throw new NotImplementedException();
        }

        public override void Delete(NOMIPAQVacation Element)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<NOMIPAQVacation> GetAll()
        {
            throw new NotImplementedException();
        }

        public override NOMIPAQVacation GetElement(int id)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<NOMIPAQVacation> SearhItemsFor(Expression<Func<NOMIPAQVacation, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public override void Update(NOMIPAQVacation Element)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<NOMIPAQVacation> GetStoredProcData(string spName, List<KeyValuePair<string, string>> paramSp)
        {
            List<NOMIPAQVacation> result = new List<NOMIPAQVacation>();

            if (paramSp.Any())
            {
                var data = this._context.NOMIPAQVacation.FromSql<NOMIPAQVacation>($"{spName} {paramSp[0].Value.ToString()},{paramSp[1].Value.ToString()},{paramSp[2].Value.ToString()},{paramSp[3].Value.ToString()}");
                try
                {
                    if (data.Any())
                    {
                    }
                }
                catch
                {

                }
            }

            return result;
        }



    }
}
