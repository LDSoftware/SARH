using ISOSA.SARH.Data.Domain.Assignation;
using ISOSA.SARH.Data.Domain.Catalog;
using ISOSA.SARH.Data.Domain.Common;
using ISOSA.SARH.Data.Domain.Configuration;
using ISOSA.SARH.Data.Domain.Dashboard;
using ISOSA.SARH.Data.Domain.Employee;
using ISOSA.SARH.Data.Domain.Formats;
using ISOSA.SARH.Data.Domain.Process;
using ISOSA.SARH.Data.Mapping;
using Microsoft.EntityFrameworkCore;

namespace ISOSA.SARH.Data
{
    public class DataContext : DbContext
    {

        private DbContextOptionsBuilder _optionsBuilder;
        private string _connectionString;

        public DataContext(string connectionString)
        {
            _optionsBuilder = new DbContextOptionsBuilder();
            _connectionString = connectionString;
        }

        public virtual DbSet<DocumentType> Documents { get; set; }
        public virtual DbSet<HardwareAssigned> Hardware { get; set; }
        public virtual DbSet<PermissionType> Permissions { get; set; }
        public virtual DbSet<SafeEquipmentAssigned> SafeEquiment { get; set; }
        public virtual DbSet<EmployeeObjectAsignation> EmployeeObjectAsignations { get; set; }
        public virtual DbSet<NonWorkingDay> NonWorkingDays { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<EmployeeScheduleAssigned> EmployeesSchedulesAssigned { get; set; }
        public virtual DbSet<ElementUpdate> ElementsUpdate { get; set; }
        public virtual DbSet<EmployeeScheduleAssignedTemp> EmployeesSchedulesAssignedTemp { get; set; }
        public virtual DbSet<EmployeeFormat> EmployeeFormats { get; set; }
        public virtual DbSet<PersonalDocument> EmployeeDocuments { get; set; }
        public virtual DbSet<EmployeeProfile> EmployeeProfiles { get; set; }
        public virtual DbSet<Nomipaq_nom10001> NomipaqEmployees { get; set; }
        public virtual DbSet<EmployeeAditionalInfo> Employees { get; set; }
        public virtual DbSet<EmployeeOrganigrama> EmployeesOrganigrama { get; set; }
        public virtual DbSet<EmployeeDiscount> EmployeeDiscount { get; set; }
        public virtual DbSet<NonWorkingDayException> NonWorkingDayExceptions { get; set; }
        public virtual DbSet<FormatApprovedEmployee> FormatApprobedEmployees { get; set; }
        public virtual DbSet<FormatApprovedHubId> FormatApprobedHubIds { get; set; }
        public virtual DbSet<FormatApprover> FormatApprovers { get; set; }
        public virtual DbSet<DashboardData> DashboardInfo { get; set; }
        public virtual DbSet<PersonalDboardData> PersonalDashboardData { get; set; }
        public virtual DbSet<EmployeeNotify> EmployeeNotifications { get; set; }
        public virtual DbSet<EmployeeHistory> EmployeeHistory { get; set; }
        public virtual DbSet<Nomipaq_nom10014> NomipaqEmployeeVacations { get; set; }
        public virtual DbSet<EmployeeScheduleDate> EmployeeScheduleDate { get; set; }
        public virtual DbSet<NOMIPAQIncidence> NOMIPAQIncidence { get; set; }
        public virtual DbSet<NOMIPAQVacation> NOMIPAQVacation { get; set; }
        public virtual DbSet<Nomipaq_nom10010> NomipaqIncidencias { get; set; }
        public virtual DbSet<Nomipaq_nom10022> NomipaqMnemonicos { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            new DocumentTypeMapping(builder.Entity<DocumentType>());
            new HardwareAssignedMapping(builder.Entity<HardwareAssigned>());
            new PermissionTypeMapping(builder.Entity<PermissionType>());
            new SafeEquipmentAssignedMapping(builder.Entity<SafeEquipmentAssigned>());
            new EmployeeObjectAsignationMapping(builder.Entity<EmployeeObjectAsignation>());
            new NonWorkingDayMapping(builder.Entity<NonWorkingDay>());
            new ScheduleMapping(builder.Entity<Schedule>());
            new EmployeeScheduleAssignedMapping(builder.Entity<EmployeeScheduleAssigned>());
            new ElementUpdateMapping(builder.Entity<ElementUpdate>());
            new EmployeeScheduleAssignedTempMapping(builder.Entity<EmployeeScheduleAssignedTemp>());
            new EmployeeFormatMapping(builder.Entity<EmployeeFormat>());
            new EmployeeDocumentMapping(builder.Entity<PersonalDocument>());
            new EmployeeProfileMapping(builder.Entity<EmployeeProfile>());
            new NomipaqEmployeesMapping(builder.Entity<Nomipaq_nom10001>());
            new EmployeeAditionalInfoMapping(builder.Entity<EmployeeAditionalInfo>());
            new EmployeeOrganigramaMapping(builder.Entity<EmployeeOrganigrama>());
            new EmployeeDiscountMapping(builder.Entity<EmployeeDiscount>());
            new NonWorkingDayExceptionMapping(builder.Entity<NonWorkingDayException>());
            new FormatApprovedEmployeeMapping(builder.Entity<FormatApprovedEmployee>());
            new FormatApprovedHubIdMapping(builder.Entity<FormatApprovedHubId>());
            new FormatApproverMapping(builder.Entity<FormatApprover>());
            new EmployeeNotifyMapping(builder.Entity<EmployeeNotify>());
            new EmployeeHistoryMapping(builder.Entity<EmployeeHistory>());
            new NomipaqEmployeeVacationsMapping(builder.Entity<Nomipaq_nom10014>());
            new EmployeeScheduleDateMapping(builder.Entity<EmployeeScheduleDate>());
            new NomipaqIncidenciasMapping(builder.Entity<Nomipaq_nom10010>());
            new NomipaqMnemonicosMapping(builder.Entity<Nomipaq_nom10022>());
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer(
                    _connectionString, options => options.EnableRetryOnFailure());
            }
        }



    }
}
