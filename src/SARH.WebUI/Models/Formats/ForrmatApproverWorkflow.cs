using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Models.Formats
{
    public class ForrmatApproverWorkflow
    {
        public int Id { get; set; }
        public Guid RowId { get; set; }
        public string Name { get; set; }
        public string ApproveDate { get; set; }
        public string Comments { get; set; }
        public string Approved { get; set; }

    }
}
