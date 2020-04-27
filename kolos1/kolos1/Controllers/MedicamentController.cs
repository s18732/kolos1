using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using kolos1.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;

namespace kolos1.Controllers
{
    [Route("api/medicaments")]
    public class MedicamentController : ControllerBase
    {
        private const string ConnString = "Data Source=db-mssql;Initial Catalog=s18732;Integrated Security=True";
        
        [HttpGet("{id}")]
        public IActionResult GetLek(string id)
        {
            var result = new List<PrescriptionResponse>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnString))
                using (SqlCommand com = new SqlCommand())
                {
                    com.Connection = con;

                    con.Open();

                    com.CommandText = "select Prescription.IdPrescription, Prescription.Date, Prescription.DueDate, Prescription.IdPatient, Prescription.IdDoctor, Prescription_Medicament.Dose, Prescription_Medicament.Details from Prescription join Prescription_Medicament on Prescription.IdPrescription = Prescription_Medicament.IdPrescription join Medicament on Prescription_Medicament.IdMedicament = Medicament.IdMedicament" +
                        " where Medicament.IdMedicament = @id order by Date desc; ";
                    com.Parameters.AddWithValue("id", id);
                    var dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        var rec = new PrescriptionResponse();
                        rec.IdPrescription = (int)dr["IdPrescription"];
                        rec.Date = dr["Date"].ToString();
                        rec.DueDate = dr["DueDate"].ToString();
                        rec.IdPatient = (int)dr["IdPatient"];
                        rec.IdDoctor = (int)dr["IdDoctor"];
                        rec.Dose = (int)dr["Dose"];
                        rec.Details = dr["Details"].ToString();
                        result.Add(rec);
                    }
                }
            }catch(SqlException ex)
            {
                return BadRequest();
            }
            

            return Ok(result);
        }
    }
}