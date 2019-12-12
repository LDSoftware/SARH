using SARH.WebUI.Models.ModelAttributes.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Models.Catalog
{
    public class CatalogItemModel : SecurityAttributePermissionModel
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }

    }
}
