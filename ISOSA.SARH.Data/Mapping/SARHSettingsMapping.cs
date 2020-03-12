using ISOSA.SARH.Data.Domain.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISOSA.SARH.Data.Mapping
{
    public class SARHSettingsMapping
    {
        public SARHSettingsMapping(EntityTypeBuilder<SARHSettings> builder)
        {
            builder.ToTable("SARHSettings");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name);
            builder.Property(x => x.Value);
        }
    }
}
