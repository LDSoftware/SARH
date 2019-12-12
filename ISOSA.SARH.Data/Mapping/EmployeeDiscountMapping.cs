using ISOSA.SARH.Data.Domain.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISOSA.SARH.Data.Mapping
{
    public class EmployeeDiscountMapping
    {
        public EmployeeDiscountMapping(EntityTypeBuilder<EmployeeDiscount> builder)
        {
            builder.ToTable("EmployeeDiscounts");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CombineDiscount);
            builder.Property(x => x.Days);
            builder.Property(x => x.CombineDiscount);
            builder.Property(x => x.Discount);
            builder.Property(x => x.DiscountType);
            builder.Property(x => x.RangeInitial);
            builder.Property(x => x.RangeEnd);
            builder.Property(x => x.Enabled);
        }
    }
}
