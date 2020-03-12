using ISOSA.SARH.Data.Domain.Formats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ISOSA.SARH.Data.Mapping
{
    public class FormatApproverMapping
    {
        public FormatApproverMapping(EntityTypeBuilder<FormatApprover> builder)
        {
            builder.ToTable("FormatApprover");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Area);
            builder.Property(x => x.Centro);
            builder.Property(x => x.Departamento);
            builder.Property(x => x.Puesto);
            builder.Property(x => x.RowGuid);
            builder.Property(x => x.Orden);
            builder.Property(x => x.ApproverListEmployees);
        }
    }
}
