using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalRegistry.BLL.Interfaces;
using HospitalRegistry.Core;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Numerics;

namespace HospitalRegistry.DAL.Repositories
{
    public class PatientRepository : JsonRepository<Patient>, IPatientRepository
    {
        public PatientRepository() : base("patients.json") {}
    }
}
