using ISOSA.SARH.Data.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISOSA.SARH.Data.Mapping
{
    public class ElementUpdateMapping
    {
        public ElementUpdateMapping(EntityTypeBuilder<ElementUpdate> builder)
        {
            builder.ToTable("ElementsUpdates");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ElementType);
            builder.Property(x => x.UpdateSync);
        }

    }
}
