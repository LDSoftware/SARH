using ISOSA.SARH.Data.Domain.Process;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISOSA.SARH.Data.Mapping
{
    public class NomipaqMnemonicosMapping
    {
        public NomipaqMnemonicosMapping(EntityTypeBuilder<Nomipaq_nom10022> builder)
        {
            builder.ToTable("nom10022");
            builder.HasKey(x => x.idtipoincidencia);
            builder.Property(x => x.descripcion);
            builder.Property(x => x.mnemonico);
        }
    }
}
