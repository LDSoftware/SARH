using ISOSA.SARH.Data.Domain.Employee;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISOSA.SARH.Data.Mapping
{
    public class EmployeeAditionalInfoMapping
    {
        public EmployeeAditionalInfoMapping(EntityTypeBuilder<EmployeeAditionalInfo> builder)
        {
            builder.ToTable("EMP");
            builder.HasKey(x => x.EMP_EmployeeID);
            builder.Property(x => x.Area);
            builder.Property(x => x.Centro);
            builder.Property(x => x.CuentaSiman);
            builder.Property(x => x.EMP_Address);
            builder.Property(x => x.EMP_ArchiveDate);
            builder.Property(x => x.EMP_ASIGNACION);
            builder.Property(x => x.EMP_BirthDate);
            builder.Property(x => x.EMP_City);
            builder.Property(x => x.EMP_ComidaFinal);
            builder.Property(x => x.EMP_ComidaInicio);
            builder.Property(x => x.EMP_curpF);
            builder.Property(x => x.EMP_curpi);
            builder.Property(x => x.EMP_DateOfHire);
            builder.Property(x => x.EMP_Department);
            builder.Property(x => x.EMP_DiasGozado);
            builder.Property(x => x.EMP_DiasPeriodoAnterior);
            builder.Property(x => x.EMP_DiasVaca);
            builder.Property(x => x.EMP_ELogiaUserName);
            builder.Property(x => x.EMP_EMailAddress);
            builder.Property(x => x.EMP_EmergencyContactName);
            builder.Property(x => x.EMP_EmergencyContactPhone);
            builder.Property(x => x.EMP_EmergencyContactRelation);
            builder.Property(x => x.EMP_EmployeeID_Edit);
            builder.Property(x => x.EMP_ESC_RecordID);
            builder.Property(x => x.EMP_Final);
            builder.Property(x => x.EMP_FirstName);
            builder.Property(x => x.EMP_homoclave);
            builder.Property(x => x.EMP_HourlyNormalRate);
            builder.Property(x => x.EMP_HourlyOvertimeRate);
            builder.Property(x => x.EMP_HourlyPremiumRate);
            builder.Property(x => x.EMP_Inicio);
            builder.Property(x => x.EMP_LastName);
            builder.Property(x => x.EMP_MidName);
            builder.Property(x => x.EMP_PerfectEMPID);
            builder.Property(x => x.EMP_Phone);
            builder.Property(x => x.EMP_RecordID);
            builder.Property(x => x.EMP_RegistroUsuario);
            builder.Property(x => x.EMP_RFC);
            builder.Property(x => x.EMP_SDI);
            builder.Property(x => x.EMP_SFM_SupervisorConsole);
            builder.Property(x => x.EMP_SFM_WorkCenterConsole);
            builder.Property(x => x.EMP_SITE_RecordID);
            builder.Property(x => x.EMP_SkillCode);
            builder.Property(x => x.EMP_SocialSecurNbr);
            builder.Property(x => x.EMP_State);
            builder.Property(x => x.EMP_StatusCode);
            builder.Property(x => x.EMP_TipoRoll);
            builder.Property(x => x.EMP_UserDef1);
            builder.Property(x => x.EMP_UserDef2);
            builder.Property(x => x.EMP_UserDef3);
            builder.Property(x => x.EMP_UserDef4);
            builder.Property(x => x.EMP_UserDef5);
            builder.Property(x => x.EMP_UserType);
            builder.Property(x => x.EMP_Zip);
            builder.Property(x => x.HrowGuid);
            builder.Property(x => x.LogonName);
            builder.Property(x => x.PasswordSiman);
            builder.Property(x => x.Perfil);
            builder.Property(x => x.SimanUser);
            builder.Property(x => x.EMP_BloodType);
            builder.Property(x => x.EMP_PersonalEmail);
            builder.Property(x => x.EMP_Sick);
            builder.Property(x => x.EMP_CellPhone);
            builder.Property(x => x.EMP_Sexo);
            builder.Property(x => x.EMP_Suburb);
        }
    }
}
