using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalRegistry.BLL.Interfaces;
using HospitalRegistry.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Encodings.Web;

namespace HospitalRegistry.DAL.Repositories
{
    public class AppointmentRepository : JsonRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository() : base("appointments.json")
        {
        }


        public IEnumerable<Appointment> GetByDoctorId(int doctorId)
        {
            return _items.Where(a => a.DoctorId == doctorId);
        }

        public void DeleteByDoctorId(int doctorId)
        {
            _items.RemoveAll(a => a.DoctorId == doctorId);
            SaveData();
        }
    }
}
