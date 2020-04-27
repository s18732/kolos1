using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace kolos1.Controllers
{
    [Route("api/patients")]
    public class PatientController : ControllerBase
    {
        private const string ConnString = "Data Source=db-mssql;Initial Catalog=s18732;Integrated Security=True";
        [HttpDelete("{id}")]
        public IActionResult DeletePatient(String id)
        {
            using (SqlConnection con = new SqlConnection(ConnString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;

                con.Open();
                var trans = con.BeginTransaction();
                com.Transaction = trans;
                try
                {
                    com.CommandText = "select count(1) jest from Patient where IdPatient = @id;";
                    com.Parameters.AddWithValue("id", id);
                    var dr = com.ExecuteReader();
                    int jestPacjent = 0;
                    if (dr.Read())
                    {
                        jestPacjent = (int)dr["jest"];
                    }
                    dr.Close();
                    if(jestPacjent > 0)
                    {
                        com.CommandText = "select count(1) jest from Prescription where IdPatient = @id;";
                        dr = com.ExecuteReader();
                        int jest = 0;
                        if (dr.Read())
                        {
                            jest = (int)dr["jest"];
                        }
                        dr.Close();
                        if (jest > 0)
                        {
                            com.CommandText = "delete from Prescription_Medicament where IdPrescription in (select IdPrescription from Prescription where IdPatient = @id);";
                            com.ExecuteNonQuery();
                            com.CommandText = "delete from Prescription where IdPatient = @id;";
                            com.ExecuteNonQuery();
                        }
                        com.CommandText = "delete from Patient where IdPatient = @id;";
                        com.ExecuteNonQuery();
                        trans.Commit();
                        return Ok("Usunieto pacjenta");
                    }
                    else
                    {
                        trans.Rollback();
                        return BadRequest("Nie ma takiego pacjenta");
                    }
                }
                catch (SqlException ex)
                {
                    trans.Rollback();
                    return BadRequest();
                }
            }

        }
    }
}