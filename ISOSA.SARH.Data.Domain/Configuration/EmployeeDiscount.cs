using System;
using System.Collections.Generic;
using System.Text;

namespace ISOSA.SARH.Data.Domain.Configuration
{
    public class EmployeeDiscount : EntityBase
    {
        public int DiscountType { get; set; }
        public string CombineDiscount { get; set; }
        public string Discount { get; set; }
        public string Days { get; set; }
        public int RangeInitial { get; set; }
        public int RangeEnd { get; set; }
        public bool Enabled { get; set; }
        public string Description { get; set; }
    }
}
