using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ISOSA.SARH.Data.Domain.Dashboard;

namespace ISOSA.SARH.Data.Mapping
{
    public class EmployeeScheduleDateMapping
    {
        public EmployeeScheduleDateMapping(EntityTypeBuilder<EmployeeScheduleDate> builder)
        {
            builder.ToTable("EmployeeScheduleDate");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Employee);
            builder.Property(x => x.EndJobDay);
            builder.Property(x => x.EndMealDay);
            builder.Property(x => x.LastUpdateId);
            builder.Property(x => x.RegisterDate);
            builder.Property(x => x.StartJobDay);
            builder.Property(x => x.StartMealDay);
        }
    }
}
