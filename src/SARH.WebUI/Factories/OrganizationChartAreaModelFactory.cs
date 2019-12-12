using SARH.WebUI.Models.OrganizationChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Factories
{
    public class OrganizationChartAreaModelFactory : IOrganizationChartCatalogModelFactory<OrganizationChartAreaModel>
    {
        public OrganizationChartAreaModel GetCatalogItems(string catalogName)
        {
            return new OrganizationChartAreaModel()
            {
                Areas = new List<OrganizationChartItem>()
                {
                    new OrganizationChartItem()
                    {
                        Id = 1,
                        Descripcion = "Contabilidad",
                        ReadOnly = false,
                        Visible = true
                    }
                }
            };
        }
    }
}
