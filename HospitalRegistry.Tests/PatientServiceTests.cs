using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using HospitalRegistry.BLL.Services;
using HospitalRegistry.BLL.Interfaces;
using HospitalRegistry.BLL.Exceptions;
using HospitalRegistry.Core;

namespace HospitalRegistry.Tests
{
    public class PatientServiceTests
    {
        [Fact]
        public void CreatePatient_ValidData_CallsCreate()
        {
            var mockRepo = new Mock<IPatientRepository>();
            mockRepo.Setup(r => r.GetAll()).Returns(Enumerable.Empty<Patient>());

            var service = new PatientService(mockRepo.Object);
            var patient = new Patient { FirstName = "Ivan", LastName = "Ivanov", MedicalCardInfo = "№123" };

            service.CreatePatient(patient);

            mockRepo.Verify(r => r.Create(It.IsAny<Patient>()), Times.Once);
        }

        [Fact]
        public void CreatePatient_InvalidCard_ThrowsException()
        {
            var mockRepo = new Mock<IPatientRepository>();
            var service = new PatientService(mockRepo.Object);
            var patient = new Patient { FirstName = "Ivan", LastName = "Ivanov", MedicalCardInfo = "123" };

            Assert.Throws<ValidationException>(() => service.CreatePatient(patient));
        }
    }
}
