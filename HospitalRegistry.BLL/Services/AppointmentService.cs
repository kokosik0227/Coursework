using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalRegistry.BLL.Exceptions;
using HospitalRegistry.BLL.Interfaces;
using HospitalRegistry.Core;

namespace HospitalRegistry.BLL.Services
{
    public class AppointmentService
    {
        private readonly IAppointmentRepository _appRepo;
        private readonly IDoctorRepository _docRepo;
        private readonly IPatientRepository _patRepo;

        public AppointmentService(IAppointmentRepository appRepo, IDoctorRepository docRepo, IPatientRepository patRepo)
        {
            _appRepo = appRepo;
            _docRepo = docRepo;
            _patRepo = patRepo;
        }
        public void CancelAppointment(int id)
        {
            var app = _appRepo.GetById(id);
            if (app == null)
            {
                throw new ValidationException($"Запис з ID {id} не знайдено.");
            }

            _appRepo.Delete(id);
        }

        public void BookAppointment(Appointment app)
        {
            if (_docRepo.GetById(app.DoctorId) == null)
                throw new ValidationException("Лікаря не знайдено.");
            if (_patRepo.GetById(app.PatientId) == null)
                throw new ValidationException("Пацієнта не знайдено.");

            if (app.Date < DateTime.Now)
            {
                throw new ValidationException("Не можна записати пацієнта на дату, що вже минула.");
            }

            var existingApps = _appRepo.GetByDoctorId(app.DoctorId);
            if (existingApps.Any(a => a.Date.Date == app.Date.Date && a.Date.Hour == app.Date.Hour))
            {
                throw new ValidationException($"Лікар вже зайнятий о {app.Date:HH:00} на цю дату.");
            }

            _appRepo.Create(app);
        }

        public void RescheduleAppointment(int appointmentId, DateTime newDate)
        {
            var app = _appRepo.GetById(appointmentId);
            if (app == null) throw new ValidationException("Запис не знайдено.");

            if (newDate < DateTime.Now)
                throw new ValidationException("Нова дата не може бути в минулому.");

            app.Date = newDate;
            _appRepo.Update(app);
        }

        public IEnumerable<Appointment> GetDoctorSchedule(int doctorId)
        {
            if (_docRepo.GetById(doctorId) == null)
                throw new ValidationException("Лікаря не знайдено.");

            return _appRepo.GetByDoctorId(doctorId).OrderBy(a => a.Date);
        }
    }
}
