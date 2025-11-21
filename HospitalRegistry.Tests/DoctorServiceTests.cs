using Xunit;
using Moq;
using HospitalRegistry.BLL.Services;
using HospitalRegistry.BLL.Interfaces;
using HospitalRegistry.BLL.Exceptions;
using HospitalRegistry.Core;
using System.Collections.Generic;
using System.Linq;

namespace HospitalRegistry.Tests
{
    public class DoctorServiceTests
    {
        [Fact]
        public void CreateDoctor_Valid_CallsRepository()
        {
            var mockDoc = new Mock<IDoctorRepository>();
            var mockApp = new Mock<IAppointmentRepository>();
            mockDoc.Setup(r => r.GetAll()).Returns(Enumerable.Empty<Doctor>());

            var service = new DoctorService(mockDoc.Object, mockApp.Object);
            service.CreateDoctor(new Doctor { LastName = "House", FirstName = "Greg", Specialization = "Diag" });

            mockDoc.Verify(r => r.Create(It.IsAny<Doctor>()), Times.Once);
        }

        [Fact]
        public void CreateDoctor_Duplicate_ThrowsException()
        {
            var mockDoc = new Mock<IDoctorRepository>();
            var mockApp = new Mock<IAppointmentRepository>();
            mockDoc.Setup(r => r.GetAll()).Returns(new List<Doctor> { new Doctor { LastName = "House", FirstName = "Greg" } });

            var service = new DoctorService(mockDoc.Object, mockApp.Object);
            Assert.Throws<ValidationException>(() => service.CreateDoctor(new Doctor { LastName = "House", FirstName = "Greg", Specialization = "Diag" }));
        }

        [Fact]
        public void DeleteDoctor_NoApps_CallsDelete()
        {
            var mockDoc = new Mock<IDoctorRepository>();
            var mockApp = new Mock<IAppointmentRepository>();
            mockApp.Setup(r => r.GetByDoctorId(1)).Returns(Enumerable.Empty<Appointment>());

            var service = new DoctorService(mockDoc.Object, mockApp.Object);
            service.DeleteDoctor(1);

            mockDoc.Verify(r => r.Delete(1), Times.Once);
        }

        [Fact]
        public void DeleteDoctor_HasApps_ThrowsException()
        {
            var mockDoc = new Mock<IDoctorRepository>();
            var mockApp = new Mock<IAppointmentRepository>();
            mockApp.Setup(r => r.GetByDoctorId(1)).Returns(new List<Appointment> { new Appointment() });

            var service = new DoctorService(mockDoc.Object, mockApp.Object);
            Assert.Throws<ValidationException>(() => service.DeleteDoctor(1));
        }

        [Fact]
        public void UpdateDoctor_Exists_CallsUpdate()
        {
            var mockDoc = new Mock<IDoctorRepository>();
            mockDoc.Setup(r => r.GetById(1)).Returns(new Doctor { Id = 1 });

            var service = new DoctorService(mockDoc.Object, null);
            service.UpdateDoctor(new Doctor { Id = 1, LastName = "NewName", FirstName = "New", Specialization = "Spec" });

            mockDoc.Verify(r => r.Update(It.IsAny<Doctor>()), Times.Once);
        }

        [Fact]
        public void UpdateDoctor_NotExists_ThrowsException()
        {
            var mockDoc = new Mock<IDoctorRepository>();
            mockDoc.Setup(r => r.GetById(1)).Returns((Doctor)null);
            var service = new DoctorService(mockDoc.Object, null);
            Assert.Throws<ValidationException>(() => service.UpdateDoctor(new Doctor { Id = 1 }));
        }

        [Fact]
        public void SearchDoctors_ReturnsMatches()
        {
            var mockDoc = new Mock<IDoctorRepository>();
            var mockApp = new Mock<IAppointmentRepository>();

            var data = new List<Doctor>
            { 
                new Doctor { LastName = "House", FirstName = "Greg", Specialization = "Diagnost" },
                new Doctor { LastName = "Wilson", FirstName = "James", Specialization = "Oncolog" }
            };

            mockDoc.Setup(r => r.GetAll()).Returns(data);

            var service = new DoctorService(mockDoc.Object, mockApp.Object);

            var result = service.SearchDoctors("wilson");

            Assert.Single(result);
            Assert.Equal("Wilson", result.First().LastName);
        }
    }
}