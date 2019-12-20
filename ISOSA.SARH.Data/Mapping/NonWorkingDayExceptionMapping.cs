
using ISOSA.SARH.Data.Domain.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISOSA.SARH.Data.Mapping
{
    public class NonWorkingDayExceptionMapping
    {
        public NonWorkingDayExceptionMapping(EntityTypeBuilder<NonWorkingDayException> builder)
        {
            builder.ToTable("HolidayExceptions");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.HolidayId);
            builder.Property(x => x.EmployeeId);
        }
    }
}
