using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Models.Organigrama
{
    public class OrganigramaModel
    {
        public string Area { get; set; }
        public string JobCenter { get; set; }
        public string RowId { get; set; }
        public List<OrganigramaEmployeeModel> Employess { get; set; }


    }
}
