using ISOSA.SARH.Data.Domain.Formats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISOSA.SARH.Data.Mapping
{
    public class FormatApprovedEmployeeMapping
    {
        public FormatApprovedEmployeeMapping(EntityTypeBuilder<FormatApprovedEmployee> builder)
        {
            builder.ToTable("FormatApprobedEmployee");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.RowGuid);
            builder.Property(x => x.ApprovedOrder);
        }
    }
}
