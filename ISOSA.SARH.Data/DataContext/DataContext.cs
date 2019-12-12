using ISOSA.SARH.Data.Domain.Assignation;
using ISOSA.SARH.Data.Domain.Catalog;
using ISOSA.SARH.Data.Domain.Common;
using ISOSA.SARH.Data.Domain.Configuration;
using ISOSA.SARH.Data.Domain.Employee;
using ISOSA.SARH.Data.Domain.Formats;
using ISOSA.SARH.Data.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

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
