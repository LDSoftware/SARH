using ISOSA.SARH.Data.Domain.Dashboard;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ISOSA.SARH.Data.Repository
{
    public class PersonalDashboardDataRepository : RepositoryBase<PersonalDboardData>
    {

        public PersonalDashboardDataRepository(string connectionString) 
            : base(connectionString)
        {

        }

        public override void Create(PersonalDboardData Element)
        {
            throw new NotImplementedException();
        }

        public override void Delete(PersonalDboardData Element)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<PersonalDboardData> GetAll()
        {
            throw new NotImplementedException();
        }

        public override PersonalDboardData GetElement(int id)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<PersonalDboardData> SearhItemsFor(Expression<Func<PersonalDboardData, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public override void Update(PersonalDboardData Element)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<PersonalDboardData> GetStoredProcData(string spName, List<KeyValuePair<string, string>> paramSp)
        {
            List<PersonalDboardData> result = new List<PersonalDboardData>();

            if (paramSp.Any())
            {

            }

            return result;
        }

    }
}
