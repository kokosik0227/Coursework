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
    public class AppointmentServiceTests
    {
        [Fact]
        public void BookAppointment_ValidEntities_CallsCreate()
        {
            var mockApp = new Mock<IAppointmentRepository>();
            var mockDoc = new Mock<IDoctorRepository>();
            var mockPat = new Mock<IPatientRepository>();

            mockDoc.Setup(r => r.GetById(1)).Returns(new Doctor { Id = 1 });
            mockPat.Setup(r => r.GetById(2)).Returns(new Patient { Id = 2 });

            var service = new AppointmentService(mockApp.Object, mockDoc.Object, mockPat.Object);

            var app = new Appointment { DoctorId = 1, PatientId = 2, Date = DateTime.Now.AddDays(1) };

            service.BookAppointment(app);

            mockApp.Verify(r => r.Create(It.IsAny<Appointment>()), Times.Once);
        }

        [Fact]
        public void BookAppointment_PastDate_ThrowsException()
        {
            var mockApp = new Mock<IAppointmentRepository>();
            var mockDoc = new Mock<IDoctorRepository>();
            var mockPat = new Mock<IPatientRepository>();

            mockDoc.Setup(r => r.GetById(1)).Returns(new Doctor());
            mockPat.Setup(r => r.GetById(2)).Returns(new Patient());

            var service = new AppointmentService(mockApp.Object, mockDoc.Object, mockPat.Object);

            var app = new Appointment { DoctorId = 1, PatientId = 2, Date = DateTime.Now.AddDays(-1) };

            Assert.Throws<ValidationException>(() => service.BookAppointment(app));
        }
    }
}
