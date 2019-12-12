using ISOSA.SARH.Data.Domain.Employee;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISOSA.SARH.Data.Mapping
{
    public class EmployeeProfileMapping
    {
        public EmployeeProfileMapping(EntityTypeBuilder<EmployeeProfile> builder)
        {
            builder.ToTable("EmployeeProfiles");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.EmployeeId);
            builder.Property(x => x.ProfileSectionValues);
        }
    }
}
