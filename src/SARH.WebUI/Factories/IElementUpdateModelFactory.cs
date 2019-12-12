using SARH.WebUI.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Factories
{
    public interface IElementUpdateModelFactory
    {
        UpdateElementItem GetCatalogUpdateSync(int elementType);
    }
}
