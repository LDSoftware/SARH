using System;
using System.Collections.Generic;
using System.Text;

namespace ISOSA.SARH.Data.Domain.Formats
{
    public class FormatApprovedHubId : EntityBase
    {
        public string ApprovedIdentity { get; set; }
        public string HubConnectionId { get; set; }
    }
}
