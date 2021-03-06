﻿using kolos1.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace kolos1.Services
{
    public class SqlServerDbService : IMedicamentsDbService
    {
        private const string ConnString = "Data Source=db-mssql;Initial Catalog=s18732;Integrated Security=True";
        public string DeletePatient(string id)
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
                    if (jestPacjent > 0)
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
                        return ("Usunieto pacjenta");
                    }
                    else
                    {
                        trans.Rollback();
                        return ("Nie ma takiego pacjenta");
                    }
                }
                catch (SqlException ex)
                {
                    trans.Rollback();
                    return null;
                }
            }
        }

        public List<PrescriptionResponse> GetLek(string id)
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
            }
            catch (SqlException ex)
            {
                return null;
            }


            return result;
        }
    }
}
