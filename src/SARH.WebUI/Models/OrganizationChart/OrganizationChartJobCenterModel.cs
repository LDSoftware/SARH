using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Models.OrganizationChart
{
    public class OrganizationChartJobCenterModel
    {
        public int Id { get; set; }
        public IList<OrganizationChartCategoryModel> Categories { get; set; }
    }
}
