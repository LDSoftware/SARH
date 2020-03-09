using ISOSA.SARH.Data.Domain.Process;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISOSA.SARH.Data.Mapping
{
    public class NomipaqIncidenciasMapping
    {
        public NomipaqIncidenciasMapping(EntityTypeBuilder<Nomipaq_nom10010> builder)
        {
            builder.ToTable("nom10010");
            builder.HasKey(x => x.idmovtodyh);
            builder.Property(x => x.idempleado);
            builder.Property(x => x.idtipoincidencia);
            builder.Property(x => x.fecha);
        }
    }
}


