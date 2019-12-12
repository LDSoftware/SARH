using ISOSA.SARH.Data.Domain.Employee;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISOSA.SARH.Data.Mapping
{
    public class NomipaqEmployeesMapping
    {
        public NomipaqEmployeesMapping(EntityTypeBuilder<Nomipaq_nom10001> builder)
        {
            builder.ToTable("nom10001");
            builder.HasKey(x => x.idempleado);
            builder.Property(x => x.apellidomaterno);
            builder.Property(x => x.apellidopaterno);
            builder.Property(x => x.basecotizacionimss);
            builder.Property(x => x.basepago);
            builder.Property(x => x.calculado);
            builder.Property(x => x.calculadoextraordinario);
            builder.Property(x => x.calculoaguinaldo);
            builder.Property(x => x.calculoptu);
            builder.Property(x => x.cambiocotizacionimss);
            builder.Property(x => x.cestadoempleadoperiodo);
            builder.Property(x => x.cfechasueldomixto);
            builder.Property(x => x.cidregistropatronal);
            builder.Property(x => x.codigoempleado);
            builder.Property(x => x.codigopostal);
            builder.Property(x => x.CorreoElectronico);
            builder.Property(x => x.csueldomixto);
            builder.Property(x => x.curpf);
            builder.Property(x => x.curpi);
            builder.Property(x => x.direccion);
            builder.Property(x => x.EntidadFederativa);
            builder.Property(x => x.estado);
            builder.Property(x => x.estadocivil);
            builder.Property(x => x.estadoempleado);
            builder.Property(x => x.fechaalta);
            builder.Property(x => x.fechabaja);
            builder.Property(x => x.fechanacimiento);
            builder.Property(x => x.fechareingreso);
            builder.Property(x => x.fechasueldodiario);
            builder.Property(x => x.fechasueldointegrado);
            builder.Property(x => x.fechasueldopromedio);
            builder.Property(x => x.fechasueldovariable);
            builder.Property(x => x.homoclave);
            builder.Property(x => x.iddepartamento);
            builder.Property(x => x.idpuesto);
            builder.Property(x => x.idtipoperiodo);
            builder.Property(x => x.idturno);
            builder.Property(x => x.lugarnacimiento);
            builder.Property(x => x.modificacionneto);
            builder.Property(x => x.modificacionsalarioimss);
            builder.Property(x => x.nombre);
            builder.Property(x => x.nombrelargo);
            builder.Property(x => x.nombremadre);
            builder.Property(x => x.nombrepadre);
            builder.Property(x => x.numeroafore);
            builder.Property(x => x.numerosegurosocial);
            builder.Property(x => x.poblacion);
            builder.Property(x => x.rfc);
            builder.Property(x => x.sexo);
            builder.Property(x => x.sueldodiario);
            builder.Property(x => x.sueldointegrado);
            builder.Property(x => x.sueldopromedio);
            builder.Property(x => x.sueldovariable);
            builder.Property(x => x.telefono);
            builder.Property(x => x.tipocontrato);
            builder.Property(x => x.tipoempleado);
            builder.Property(x => x.TipoRegimen);
            builder.Property(x => x.zonasalario);
        }
    }
}
