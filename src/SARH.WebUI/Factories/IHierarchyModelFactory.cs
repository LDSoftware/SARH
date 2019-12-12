using SARH.WebUI.Models.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Factories
{
    public interface IHierarchyModelFactory
    {
        IList<HierarchyModel> GetData();
    }
}
