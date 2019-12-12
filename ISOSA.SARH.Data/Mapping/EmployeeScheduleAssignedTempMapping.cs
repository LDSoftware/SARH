using ISOSA.SARH.Data.Domain.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISOSA.SARH.Data.Mapping
{
    public class EmployeeScheduleAssignedTempMapping
    {
        public EmployeeScheduleAssignedTempMapping(EntityTypeBuilder<EmployeeScheduleAssignedTemp> builder)
        {
            builder.ToTable("EmployeeScheduleAssignedTemp");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.EmployeeId);
            builder.Property(x => x.IdScheduleWorkday);
            builder.Property(x => x.IdScheduleMeal);
            builder.Property(x => x.ToleranceWorkday);
            builder.Property(x => x.ToleranceMeal);
        }
    }
}
