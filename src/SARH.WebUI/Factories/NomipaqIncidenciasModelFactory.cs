using ISOSA.SARH.Data.Domain.Employee;
using ISOSA.SARH.Data.Domain.Process;
using ISOSA.SARH.Data.Repository;
using SARH.WebUI.Models.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SARH.WebUI.Factories
{
    public class NomipaqIncidenciasModelFactory : INomipaqIncidenciasModelFactory
    {

        private readonly IRepository<Nomipaq_nom10001> _employeeNomipaqRepository;
        private readonly IRepository<Nomipaq_nom10010> _incidenciasNomipaqRepository;
        private readonly IRepository<Nomipaq_nom10022> _mnemonicosNomipaqRepository;

        public NomipaqIncidenciasModelFactory(IRepository<Nomipaq_nom10001> employeeNomipaqRepository,
            IRepository<Nomipaq_nom10010> incidenciasNomipaqRepository,
            IRepository<Nomipaq_nom10022> mnemonicosNomipaqRepository)
        {
            this._employeeNomipaqRepository = employeeNomipaqRepository;
            this._incidenciasNomipaqRepository = incidenciasNomipaqRepository;
            this._mnemonicosNomipaqRepository = mnemonicosNomipaqRepository;
        }

        public List<NomipaqIncidenciaModel> GetAllIncidencias()
        {
            List<NomipaqIncidenciaModel> result = new List<NomipaqIncidenciaModel>();

            var mnemonicos = this._mnemonicosNomipaqRepository.GetAll();
            var employees = this._employeeNomipaqRepository.GetAll();
            var incidencias = this._incidenciasNomipaqRepository.GetAll();


            var data = (from inci in incidencias
                        join emp in employees on inci.idempleado equals emp.idempleado
                        join nme in mnemonicos on inci.idtipoincidencia equals nme.idtipoincidencia
                        select new NomipaqIncidenciaModel() 
                        {
                            EmployeeId = emp.codigoempleado,
                            Fecha = inci.fecha.Value,
                            Nemonico = nme.mnemonico
                        });

            result.AddRange(data);
            return result;
        }

        public NomipaqIncidenciaModel GetEmployeeIncidencias(string employeeId)
        {
            NomipaqIncidenciaModel result = new NomipaqIncidenciaModel();

            var mnemonicos = this._mnemonicosNomipaqRepository.GetAll();
            var employees = this._employeeNomipaqRepository.GetAll();
            var incidencias = this._incidenciasNomipaqRepository.GetAll();


            var results = (from inci in incidencias
                        join emp in employees on inci.idempleado equals emp.idempleado
                        join nme in mnemonicos on inci.idtipoincidencia equals nme.idtipoincidencia
                        where emp.codigoempleado == employeeId
                        select new NomipaqIncidenciaModel()
                        {
                            EmployeeId = emp.codigoempleado,
                            Fecha = inci.fecha.Value,
                            Nemonico = nme.mnemonico
                        });

            if (results.Any()) 
            {
                result = results.FirstOrDefault();
            }

            return result;
        }

    }
}
