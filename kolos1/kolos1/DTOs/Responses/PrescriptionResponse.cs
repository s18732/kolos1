using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kolos1.DTOs.Responses
{
    public class PrescriptionResponse
    {
        public int IdPrescription { get; set; }
        public String Date { get; set; }
        public String DueDate { get; set; }
        public int IdPatient { get; set; }
        public int IdDoctor { get; set; }
        public int Dose { get; set; }
        public String Details { get; set; }
    }
}
