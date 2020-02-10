using ISOSA.SARH.Data.Domain.Employee;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISOSA.SARH.Data.Mapping
{
    public class NomipaqEmployeeVacationsMapping
    {
        public NomipaqEmployeeVacationsMapping(EntityTypeBuilder<Nomipaq_nom10014> builder)
        {
            builder.ToTable("nom10014");
            builder.HasKey(x => x.idtcontrolvacaciones);
            builder.Property(x => x.idempleado);
            builder.Property(x => x.ejercicio);
            builder.Property(x => x.diasdescanso);
            builder.Property(x => x.diasprimavacacional);
            builder.Property(x => x.fechainicio);
            builder.Property(x => x.diasdescanso);
            builder.Property(x => x.timestamp);
            builder.Property(x => x.fechapago);
        }
    }
}
