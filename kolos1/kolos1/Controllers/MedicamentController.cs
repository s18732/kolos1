using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using kolos1.DTOs.Responses;
using kolos1.Services;
using Microsoft.AspNetCore.Mvc;

namespace kolos1.Controllers
{
    [Route("api/medicaments")]
    public class MedicamentController : ControllerBase
    {
        private IMedicamentsDbService _service;
        public MedicamentController(IMedicamentsDbService service)
        {
            _service = service;
        }
        [HttpGet("{id}")]
        public IActionResult GetLek(string id)
        {
            List<PrescriptionResponse> result = _service.GetLek(id);
            if (result == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(result);
            }
        }
    }
}