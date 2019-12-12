using ISOSA.SARH.Data.Domain.Employee;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISOSA.SARH.Data.Mapping
{
    public class EmployeeDocumentMapping
    {
        public EmployeeDocumentMapping(EntityTypeBuilder<PersonalDocument> builder)
        {
            builder.ToTable("EmployeeDocumentAssigned");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Checked);
            builder.Property(x => x.DocumentPathInfo);
            builder.Property(x => x.DocumentType);
            builder.Property(x => x.EmployeeID);
            builder.Property(x => x.IsValid);
        }
    }
}
