using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HospitalRegistry.BLL.Exceptions;
using HospitalRegistry.BLL.Interfaces;
using HospitalRegistry.Core;

namespace HospitalRegistry.BLL.Services
{
    public class PatientService
    {
        private readonly IPatientRepository _patientRepo;

        public PatientService(IPatientRepository patientRepo)
        {
            _patientRepo = patientRepo;
        }

        public void CreatePatient(Patient patient)
        {
            ValidatePatientData(patient);

            if (_patientRepo.GetAll().Any(p => p.MedicalCardInfo == patient.MedicalCardInfo))
                throw new ValidationException($"Картка з номером {patient.MedicalCardInfo} вже існує.");

            _patientRepo.Create(patient);
        }

        public void UpdatePatient(Patient patient)
        {
            if (_patientRepo.GetById(patient.Id) == null)
                throw new ValidationException("Пацієнта не знайдено.");

            ValidatePatientData(patient);
            _patientRepo.Update(patient);
        }

        public void DeletePatient(int id) => _patientRepo.Delete(id);

        public IEnumerable<Patient> GetAllPatients() => _patientRepo.GetAll();
        public Patient GetPatientById(int id) => _patientRepo.GetById(id);

        public IEnumerable<Patient> SearchPatients(string query)
        {
            return _patientRepo.GetAll()
                .Where(p => p.LastName.ToLower().Contains(query.ToLower()));
        }

        private void ValidatePatientData(Patient p)
        {
            if (string.IsNullOrWhiteSpace(p.FirstName) || !p.FirstName.All(c => char.IsLetter(c) || c == '-'))
                throw new ValidationException("Некоректне ім'я (має містити тільки літери).");

            if (string.IsNullOrWhiteSpace(p.LastName) || !p.LastName.All(c => char.IsLetter(c) || c == '-'))
                throw new ValidationException("Некоректне прізвище (має містити тільки літери).");

            if (!Regex.IsMatch(p.MedicalCardInfo, @"^№\d+$"))
            {
                throw new ValidationException("Номер картки має бути у форматі '№12345' (Символ № та цифри).");
            }
        }
    }
}