using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Models.Hierarchy
{
    public class HierarchyModel
    {

        public HierarchyModel()
        {
            SubArea = new List<HierarchyModel>();
        }

        public string Area { get; set; }
        public List<HierarchyModel> SubArea { get; set; }
    }
}
