using HospitalRegistry.BLL.Interfaces;
using HospitalRegistry.DAL.Repositories;
using HospitalRegistry.BLL.Services;
using HospitalRegistry.PL;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        IDoctorRepository docRepo = new DoctorRepository();
        IPatientRepository patRepo = new PatientRepository();
        IAppointmentRepository appRepo = new AppointmentRepository();

        DoctorService docService = new DoctorService(docRepo, appRepo);
        PatientService patService = new PatientService(patRepo);
        AppointmentService appService = new AppointmentService(appRepo, docRepo, patRepo);

        ServiceManager manager = new ServiceManager(docService, patService, appService);
        ApplicationUI ui = new ApplicationUI(manager);
        ui.Run();
    }
}