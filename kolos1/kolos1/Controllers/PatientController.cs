using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using kolos1.Services;
using Microsoft.AspNetCore.Mvc;

namespace kolos1.Controllers
{
    [Route("api/patients")]
    public class PatientController : ControllerBase
    {
        private const string ConnString = "Data Source=db-mssql;Initial Catalog=s18732;Integrated Security=True";
        private IMedicamentsDbService _service;
        public PatientController(IMedicamentsDbService service)
        {
            _service = service;
        }
        [HttpDelete("{id}")]
        public IActionResult DeletePatient(String id)
        {
            var result = _service.DeletePatient(id);
            if(result == null)
            {
                return BadRequest();
            }
            else if(result.Equals("Nie ma takiego pacjenta")){
                return BadRequest(result);
            }
            else if(result.Equals("Usunieto pacjenta")){
                return Ok(result);
            }
            return BadRequest();
        }
    }
}