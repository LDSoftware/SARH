using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Factories
{
    public interface IOrganizationChartCatalogModelFactory<T>
    {
        T GetCatalogItems(string catalogName);
    }
}
