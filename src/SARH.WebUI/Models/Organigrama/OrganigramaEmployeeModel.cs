using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Models.Organigrama
{
    public class OrganigramaEmployeeModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public string JobCenter { get; set; }
        public string Category { get; set; }
        public string JobTitle { get; set; }
        public string RowId { get; set; }
        public string UserName { get; set; }
    }
}
