﻿using ISOSA.SARH.Data.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISOSA.SARH.Data.Mapping
{
    public class DocumentTypeMapping
    {

        public DocumentTypeMapping(EntityTypeBuilder<DocumentType> builder)
        {
            builder.ToTable("DocumentType");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Description);
        }

    }
}
