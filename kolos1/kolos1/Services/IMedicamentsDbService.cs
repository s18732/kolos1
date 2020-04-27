using kolos1.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kolos1.Services
{
    public interface IMedicamentsDbService
    {
        public List<PrescriptionResponse> GetLek(string id);
        public String DeletePatient(string id);
    }
}
