using ISOSA.SARH.Data.Domain.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISOSA.SARH.Data.Mapping
{
    public class ScheduleMapping
    {
        public ScheduleMapping(EntityTypeBuilder<Schedule> builder)
        {
            builder.ToTable("Schedules");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Description);
            builder.Property(x => x.EffectiveTimeStart);
            builder.Property(x => x.EffectiveTimeEnd);
            builder.Property(x => x.Enabled);
            builder.Property(x => x.EndHour);
            builder.Property(x => x.StartHour);
            builder.Property(x => x.TypeSchedule);
            builder.Property(x => x.StartHourAnticipated);
            builder.Property(x => x.Weekend);
        }
    }
}
