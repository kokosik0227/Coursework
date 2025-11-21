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
    public class DoctorService
    {
        private readonly IDoctorRepository _doctorRepo;
        private readonly IAppointmentRepository _appRepo;

        public DoctorService(IDoctorRepository doctorRepo, IAppointmentRepository appRepo)
        {
            _doctorRepo = doctorRepo;
            _appRepo = appRepo;
        }

        public void CreateDoctor(Doctor doctor)
        {
            ValidateDoctorData(doctor);

            if (_doctorRepo.GetAll().Any(d => d.LastName == doctor.LastName && d.FirstName == doctor.FirstName))
                throw new ValidationException($"Лікар {doctor.FirstName} {doctor.LastName} вже існує.");

            _doctorRepo.Create(doctor);
        }

        public void UpdateDoctor(Doctor doctor)
        {
            if (_doctorRepo.GetById(doctor.Id) == null)
                throw new ValidationException("Лікаря не знайдено.");

            ValidateDoctorData(doctor);

            _doctorRepo.Update(doctor);
        }

        public void DeleteDoctor(int id)
        {
            if (_appRepo.GetByDoctorId(id).Any())
            {
                throw new ValidationException($"Неможливо видалити лікаря ID {id}, до нього є записи.");
            }
            _doctorRepo.Delete(id);
        }

        public IEnumerable<Doctor> GetAllDoctors() => _doctorRepo.GetAll();
        public Doctor GetDoctorById(int id) => _doctorRepo.GetById(id);

        public IEnumerable<Doctor> SearchDoctors(string query)
        {
            return _doctorRepo.GetAll()
                .Where(d => d.LastName.ToLower().Contains(query.ToLower()) ||
                            d.Specialization.ToLower().Contains(query.ToLower()));
        }

        private void ValidateDoctorData(Doctor d)
        {
            if (string.IsNullOrWhiteSpace(d.FirstName) || string.IsNullOrWhiteSpace(d.LastName))
                throw new ValidationException("Ім'я та Прізвище не можуть бути порожніми.");

            if (!d.FirstName.All(c => char.IsLetter(c) || c == '-') || !d.LastName.All(c => char.IsLetter(c) || c == '-'))
                throw new ValidationException("Ім'я та Прізвище повинні містити тільки літери.");

            if (string.IsNullOrWhiteSpace(d.Specialization))
                throw new ValidationException("Спеціалізація обов'язкова.");

            if (d.Specialization.Any(char.IsDigit))
                throw new ValidationException("Спеціалізація не може містити цифри.");
        }
    }
}
