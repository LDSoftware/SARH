using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Models.Organigrama
{
    public class SafeEquimentAssignedModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string AssignationDate { get; set; }
        public string SecurityEquimentName { get; set; }
        public int IdSecurityEquiment { get; set; }
    }
}
