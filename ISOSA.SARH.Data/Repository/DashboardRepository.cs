using ISOSA.SARH.Data.Domain.Dashboard;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ISOSA.SARH.Data.Repository
{
    public class DashboardRepository : RepositoryBase<DashboardData>
    {

        public DashboardRepository(string connectionString) 
            : base(connectionString)
        {

        }

        public override void Create(DashboardData Element)
        {
            throw new NotImplementedException();
        }
        public override void Delete(DashboardData Element)
        {
            throw new NotImplementedException();
        }
        public override IEnumerable<DashboardData> GetAll()
        {
            throw new NotImplementedException();
        }
        public override DashboardData GetElement(int id)
        {
            throw new NotImplementedException();
        }
        public override IEnumerable<DashboardData> SearhItemsFor(Expression<Func<DashboardData, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        public override void Update(DashboardData Element)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<DashboardData> GetStoredProcData(string spName, List<KeyValuePair<string, string>> paramSp)
        {
            List<DashboardData> result = new List<DashboardData>();

            if (paramSp.Any()) 
            {
                var data = this._context.DashboardInfo.FromSql<DashboardData>($"{spName} {paramSp[0].Value.ToString()}");
                if (data.Any()) 
                {
                    result.AddRange(data);
                }
            }

            return result;
        }


    }
}
