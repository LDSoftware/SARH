using ISOSA.SARH.Data.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISOSA.SARH.Data.Mapping
{
    public class SafeEquipmentAssignedMapping
    {
        public SafeEquipmentAssignedMapping(EntityTypeBuilder<SafeEquipmentAssigned> builder)
        {
            builder.ToTable("SafeEquipmentAssigned");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Description);
        }
    }
}
