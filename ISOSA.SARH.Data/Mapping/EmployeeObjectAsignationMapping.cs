using ISOSA.SARH.Data.Domain.Assignation;
using ISOSA.SARH.Data.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISOSA.SARH.Data.Mapping
{
    public class EmployeeObjectAsignationMapping
    {
        public EmployeeObjectAsignationMapping(EntityTypeBuilder<EmployeeObjectAsignation> builder)
        {
            builder.ToTable("EmployeeObjectAsignation");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.AssignationType);
            builder.Property(x => x.EmployeeId);
            builder.Property(x => x.AssignationDetail);
            builder.Property(x => x.PathPDF);
            builder.Property(x => x.DocumentId);
        }
    }
}
