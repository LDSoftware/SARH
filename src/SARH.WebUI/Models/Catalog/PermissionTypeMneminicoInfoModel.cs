using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Models.Catalog
{
    public class PermissionTypeMneminicoInfoModel
    {
        public bool Disabled { get; set; }
        public string Group { get; set; }
        public bool Selected { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
    }
}
