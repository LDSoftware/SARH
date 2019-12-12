using ISOSA.SARH.Data.Domain.Formats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ISOSA.SARH.Data.Mapping
{
    public class EmployeeFormatMapping
    {
        public EmployeeFormatMapping(EntityTypeBuilder<EmployeeFormat> builder)
        {
            builder.ToTable("EmployeeFormats");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ApprovalDate);
            builder.Property(x => x.ApprovalWorkFlow);
            builder.Property(x => x.Comments);
            builder.Property(x => x.CreateDate);
            builder.Property(x => x.EmployeeId);
            builder.Property(x => x.EmployeeSubstitute);
            builder.Property(x => x.EndDate);
            builder.Property(x => x.PermissionType);
            builder.Property(x => x.StartDate);
        }

    }
}
