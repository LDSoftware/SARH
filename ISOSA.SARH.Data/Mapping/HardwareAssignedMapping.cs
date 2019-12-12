using ISOSA.SARH.Data.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISOSA.SARH.Data.Mapping
{
    public class HardwareAssignedMapping
    {

        public HardwareAssignedMapping(EntityTypeBuilder<HardwareAssigned> builder)
        {
            builder.ToTable("HardwareAssigned");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Description);
        }


    }
}
