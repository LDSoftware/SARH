using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Models.Formats
{
    public class FormatApproverModel
    {

        public FormatApproverModel()
        {
            Approvers = new List<FormatApproverItem>();
        }

        public IList<FormatApproverItem> Approvers { get; set; }

    }

    public class FormatApproverItem
    {
        public int Id { get; set; }
        public string Area { get; set; }
        public string Centro { get; set; }
        public string Approver { get; set; }
        public string Puesto { get; set; }
        public int Order { get; set; }
        public string RowGuid { get; set; }
    }

}
