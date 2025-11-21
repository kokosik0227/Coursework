using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalRegistry.BLL.Services
{
    public class ServiceManager
    {
        public DoctorService DoctorService { get; }
        public PatientService PatientService { get; }
        public AppointmentService AppointmentService { get; }

        public ServiceManager(DoctorService doctorService, PatientService patientService, AppointmentService appointmentService)
        {
            DoctorService = doctorService;
            PatientService = patientService;
            AppointmentService = appointmentService;
        }
    }
}
