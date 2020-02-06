using ISOSA.SARH.Data.Domain.Employee;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISOSA.SARH.Data.Mapping
{
    public class EmployeeHistoryMapping
    {
        public EmployeeHistoryMapping(EntityTypeBuilder<EmployeeHistory> builder)
        {
            builder.ToTable("EmployeeHistory");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.EmployeeId);
            builder.Property(x => x.DateChange);
            builder.Property(x => x.Descripcion);
            builder.Property(x => x.EmployeeId);
            builder.Property(x => x.JobActual);
            builder.Property(x => x.RegisterDate);
            builder.Property(x => x.RowGuidLast);
            builder.Property(x => x.UserId);
            builder.Property(x => x.RowGuidActual);

        }
    }
}
