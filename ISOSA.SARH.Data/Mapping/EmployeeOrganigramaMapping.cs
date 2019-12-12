using ISOSA.SARH.Data.Domain.Employee;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ISOSA.SARH.Data.Mapping
{
    public class EmployeeOrganigramaMapping
    {
        public EmployeeOrganigramaMapping(EntityTypeBuilder<EmployeeOrganigrama> builder)
        {
            builder.ToTable("Horarios");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.RowGuid);
            builder.Property(x => x.Area);
            builder.Property(x => x.Centro);
            builder.Property(x => x.Codigo);
            builder.Property(x => x.Departamento);
            builder.Property(x => x.FinAlimento);
            builder.Property(x => x.FinTurno);
            builder.Property(x => x.IdentPuesto);
            builder.Property(x => x.InicioAlimento);
            builder.Property(x => x.InicioTurno);
            builder.Property(x => x.Jefatura);
            builder.Property(x => x.Porcentaje100);
            builder.Property(x => x.PorcentajeCT);
            builder.Property(x => x.PorcentajeReparto);
            builder.Property(x => x.Puesto);
            builder.Property(x => x.SabadoFinal);
            builder.Property(x => x.SabadoInicio);
            builder.Property(x => x.TipoRoll);
        }
    }
}
