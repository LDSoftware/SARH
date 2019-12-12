using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Models.ModelAttributes.Security
{
    public class SecurityAttributePermissionModel
    {
        /**
            Valores por default
        **/
        private bool _visible = true;
        private bool _readonly = false;

        public bool Visible { get => _visible; set => _visible = value; }
        public bool ReadOnly { get => _readonly; set => _readonly = value; }
    }
}
