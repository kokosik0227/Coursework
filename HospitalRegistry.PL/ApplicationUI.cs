using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalRegistry.BLL.Services;
using HospitalRegistry.Core;
using HospitalRegistry.BLL.Exceptions;

namespace HospitalRegistry.PL
{
    public class ApplicationUI
    {
        private readonly ServiceManager _manager;
        private bool _isRunning = true;

        public ApplicationUI(ServiceManager manager) { _manager = manager; }

        public void Run()
        {
            while (_isRunning)
            {
                Console.Clear();
                Console.WriteLine("=== РЕЄСТРАТУРА  ===");
                Console.WriteLine("1. Управління ЛІКАРЯМИ");
                Console.WriteLine("2. Управління ХВОРИМИ");
                Console.WriteLine("3. Управління РОЗКЛАДОМ");
                Console.WriteLine("4. ПОШУК");
                Console.WriteLine("0. Вихід");
                Console.Write("Ваш вибір: ");

                ProcessChoice(Console.ReadLine());

                if (_isRunning)
                {
                    Console.WriteLine("\nНатисніть Enter...");
                    Console.ReadLine();
                }
            }
        }

        private void ProcessChoice(string choice)
        {
            try
            {
                switch (choice)
                {
                    case "1": DoctorMenu(); break;
                    case "2": PatientMenu(); break;
                    case "3": ScheduleMenu(); break;
                    case "4": SearchMenu(); break;
                    case "0": _isRunning = false; break;
                    default: Console.WriteLine("Невірний вибір."); break;
                }
            }
            catch (ValidationException ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"[Помилка]: {ex.Message}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Критична помилка]: {ex.Message}");
                Console.ResetColor();
            }
        }

        private void DoctorMenu()
        {
            Console.WriteLine("\n--- ЛІКАРІ ---");
            Console.WriteLine("1. Додати лікаря [1.1]");
            Console.WriteLine("2. Видалити лікаря [1.2]");
            Console.WriteLine("3. Змінити дані лікаря [1.3]");
            Console.WriteLine("4. Список лікарів [1.4]");
            Console.Write("Вибір: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.Write("Ім'я: "); string fn = Console.ReadLine();
                    Console.Write("Прізвище: "); string ln = Console.ReadLine();
                    Console.Write("Спеціалізація: "); string sp = Console.ReadLine();
                    _manager.DoctorService.CreateDoctor(new Doctor { FirstName = fn, LastName = ln, Specialization = sp });
                    Console.WriteLine("Успіх.");
                    break;
                case "2":
                    Console.Write("ID для видалення: "); int.TryParse(Console.ReadLine(), out int idDel);
                    _manager.DoctorService.DeleteDoctor(idDel);
                    Console.WriteLine("Видалено.");
                    break;
                case "3":
                    Console.Write("ID лікаря для зміни: "); int.TryParse(Console.ReadLine(), out int idUpd);
                    var doc = _manager.DoctorService.GetDoctorById(idUpd);
                    if (doc == null) throw new ValidationException("Не знайдено.");

                    Console.Write($"Нове ім'я ({doc.FirstName}): "); string nFn = Console.ReadLine();
                    Console.Write($"Нове прізвище ({doc.LastName}): "); string nLn = Console.ReadLine();
                    Console.Write($"Нова спец. ({doc.Specialization}): "); string nSp = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(nFn)) doc.FirstName = nFn;
                    if (!string.IsNullOrWhiteSpace(nLn)) doc.LastName = nLn;
                    if (!string.IsNullOrWhiteSpace(nSp)) doc.Specialization = nSp;

                    _manager.DoctorService.UpdateDoctor(doc);
                    Console.WriteLine("Оновлено.");
                    break;
                case "4":
                    foreach (var d in _manager.DoctorService.GetAllDoctors())
                        Console.WriteLine($"ID: {d.Id} | {d.LastName} {d.FirstName} ({d.Specialization})");
                    break;
            }
        }

        private void PatientMenu()
        {
            Console.WriteLine("\n--- ПАЦІЄНТИ ---");
            Console.WriteLine("1. Додати хворого [2.1]");
            Console.WriteLine("2. Видалити хворого [2.2]");
            Console.WriteLine("3. Електронна картка (Редагувати) [2.3]");
            Console.WriteLine("4. Список всіх");
            Console.Write("Вибір: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.Write("Ім'я: "); string fn = Console.ReadLine();
                    Console.Write("Прізвище: "); string ln = Console.ReadLine();
                    Console.Write("Картка: "); string card = Console.ReadLine();
                    _manager.PatientService.CreatePatient(new Patient { FirstName = fn, LastName = ln, MedicalCardInfo = card });
                    Console.WriteLine("Успіх.");
                    break;
                case "2":
                    Console.Write("ID хворого: "); int.TryParse(Console.ReadLine(), out int idDel);
                    _manager.PatientService.DeletePatient(idDel);
                    Console.WriteLine("Видалено.");
                    break;
                case "3":
                    Console.Write("ID хворого: "); int.TryParse(Console.ReadLine(), out int idCard);
                    var pat = _manager.PatientService.GetPatientById(idCard);
                    if (pat == null) throw new ValidationException("Не знайдено.");
                    Console.WriteLine($"Поточна картка: {pat.MedicalCardInfo}");
                    Console.Write("Нові дані картки: ");
                    pat.MedicalCardInfo = Console.ReadLine();
                    _manager.PatientService.UpdatePatient(pat);
                    Console.WriteLine("Картку оновлено.");
                    break;
                case "4":
                    foreach (var p in _manager.PatientService.GetAllPatients())
                        Console.WriteLine($"ID: {p.Id} | {p.LastName} {p.FirstName} [Картка: {p.MedicalCardInfo}]");
                    break;
            }
        }

        private void ScheduleMenu()
        {
            Console.WriteLine("\n--- РОЗКЛАД ТА ЗАПИС ---");
            Console.WriteLine("1. Записати хворого на прийом [3.3, 3.1]");
            Console.WriteLine("2. Змінити запис (Перенести) [3.2]");
            Console.WriteLine("3. Переглянути розклад лікаря");
            Console.WriteLine("4. Скасувати запис (Видалити)");
            Console.Write("Вибір: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.Write("ID Лікаря: "); int.TryParse(Console.ReadLine(), out int dId);
                    Console.Write("ID Хворого: "); int.TryParse(Console.ReadLine(), out int pId);
                    Console.Write("Дата (yyyy-mm-dd hh:mm): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime date)) date = DateTime.Now;

                    _manager.AppointmentService.BookAppointment(new Appointment { DoctorId = dId, PatientId = pId, Date = date });
                    Console.WriteLine("Записано.");
                    break;
                case "2":
                    Console.Write("ID Запису: "); int.TryParse(Console.ReadLine(), out int appId);
                    Console.Write("Нова дата: "); DateTime.TryParse(Console.ReadLine(), out DateTime newDate);
                    _manager.AppointmentService.RescheduleAppointment(appId, newDate);
                    Console.WriteLine("Перенесено.");
                    break;
                case "3":
                    Console.Write("ID Лікаря: "); int.TryParse(Console.ReadLine(), out int docId);
                    var apps = _manager.AppointmentService.GetDoctorSchedule(docId);

                    if (!apps.Any()) Console.WriteLine("Записів немає.");

                    foreach (var a in apps)
                        Console.WriteLine($"[ID запису: {a.Id}] {a.Date} - Пацієнт ID: {a.PatientId}");
                    break;
                case "4":
                    Console.Write("Введіть ID запису для видалення: ");
                    if (int.TryParse(Console.ReadLine(), out int delId))
                    {
                        _manager.AppointmentService.CancelAppointment(delId);
                        Console.WriteLine("Запис скасовано (видалено).");
                    }
                    else
                    {
                        Console.WriteLine("Невірний ID.");
                    }
                    break;
            }
        }

        private void SearchMenu()
        {
            Console.WriteLine("\n--- ПОШУК ---");
            Console.WriteLine("1. Пошук хворого (Прізвище) [4.1]");
            Console.WriteLine("2. Пошук лікаря (Спеціалізація/Прізвище) [4.2]");
            Console.WriteLine("3. Отримання розкладу лікаря на дату [4.3]");
            Console.Write("Вибір: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.Write("Введіть прізвище: ");
                    var q1 = Console.ReadLine();
                    foreach (var p in _manager.PatientService.SearchPatients(q1))
                        Console.WriteLine($"{p.LastName} {p.FirstName}");
                    break;
                case "2":
                    Console.Write("Введіть запит: ");
                    var q2 = Console.ReadLine();
                    foreach (var d in _manager.DoctorService.SearchDoctors(q2))
                        Console.WriteLine($"{d.LastName} - {d.Specialization}");
                    break;
                case "3":
                    Console.Write("ID Лікаря: "); int.TryParse(Console.ReadLine(), out int dId);
                    Console.Write("Дата (yyyy-mm-dd): "); DateTime.TryParse(Console.ReadLine(), out DateTime date);

                    var apps = _manager.AppointmentService.GetDoctorSchedule(dId)
                        .Where(a => a.Date.Date == date.Date);

                    if (!apps.Any()) Console.WriteLine("На цю дату записів немає.");
                    foreach (var a in apps) Console.WriteLine($"{a.Date.ToShortTimeString()} - Пацієнт {a.PatientId}");
                    break;
            }
        }
    }
}
