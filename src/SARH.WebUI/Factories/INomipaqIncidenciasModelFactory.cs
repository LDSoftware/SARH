using SARH.WebUI.Models.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Factories
{
    public interface INomipaqIncidenciasModelFactory
    {
        List<NomipaqIncidenciaModel> GetAllIncidencias();
        NomipaqIncidenciaModel GetEmployeeIncidencias(string employeeId);
    }
}
