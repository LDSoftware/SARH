using ISOSA.SARH.Data.Domain.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ISOSA.SARH.Data.Repository
{
    public class NOMIPAQIncidenceRepository : RepositoryBase<NOMIPAQIncidence>
    {

        public NOMIPAQIncidenceRepository(string connectionString)
            :base(connectionString)
        {

        }

        public override void Create(NOMIPAQIncidence Element)
        {
            throw new NotImplementedException();
        }

        public override void Delete(NOMIPAQIncidence Element)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<NOMIPAQIncidence> GetAll()
        {
            throw new NotImplementedException();
        }

        public override NOMIPAQIncidence GetElement(int id)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<NOMIPAQIncidence> SearhItemsFor(Expression<Func<NOMIPAQIncidence, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public override void Update(NOMIPAQIncidence Element)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<NOMIPAQIncidence> GetStoredProcData(string spName, List<KeyValuePair<string, string>> paramSp)
        {
            List<NOMIPAQIncidence> result = new List<NOMIPAQIncidence>();

            if (paramSp.Any())
            {
                var data = this._context.NOMIPAQIncidence.FromSql<NOMIPAQIncidence>($"{spName} {paramSp[0].Value.ToString()},{paramSp[1].Value.ToString()},{paramSp[2].Value.ToString()}");
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
