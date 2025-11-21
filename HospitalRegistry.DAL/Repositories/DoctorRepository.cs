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

namespace HospitalRegistry.DAL.Repositories
{
    public class DoctorRepository : JsonRepository<Doctor>, IDoctorRepository
    {
        public DoctorRepository() : base("doctors.json") {}
    }
}
