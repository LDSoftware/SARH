using ISOSA.SARH.Data.Domain.Formats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISOSA.SARH.Data.Mapping
{
    public class FormatApprovedHubIdMapping
    {
        public FormatApprovedHubIdMapping(EntityTypeBuilder<FormatApprovedHubId> builder)
        {
            builder.ToTable("FormatApprobedHubId");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ApprovedIdentity);
            builder.Property(x => x.HubConnectionId);
        }
    }
}
