using ISOSA.SARH.Data.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISOSA.SARH.Data.Mapping
{
    public class EmployeeNotifyMapping
    {
        public EmployeeNotifyMapping(EntityTypeBuilder<EmployeeNotify> builder)
        {
            builder.ToTable("EmployeeNotify");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Area);
            builder.Property(x => x.Centro);
            builder.Property(x => x.Departamento);
            builder.Property(x => x.Puesto);
            builder.Property(x => x.RowGuid);
            builder.Property(x => x.Orden);
        }
    }
}
