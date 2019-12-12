using ISOSA.SARH.Data.Domain.Catalog;
using ISOSA.SARH.Data.Domain.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISOSA.SARH.Data.Mapping
{
    public class NonWorkingDayMapping
    {
        public NonWorkingDayMapping(EntityTypeBuilder<NonWorkingDay> builder)
        {
            builder.ToTable("Holiday");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Description);
            builder.Property(x => x.Holiday);
        }
    }
}
