using ISOSA.SARH.Data.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISOSA.SARH.Data.Mapping
{
    public class PermissionTypeMapping
    {
        public PermissionTypeMapping(EntityTypeBuilder<PermissionType> builder)
        {
            builder.ToTable("PermissionType");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Description);
        }
    }
}
