using ReportService.Library;
using ReportService.Models;

namespace ReportService.Repositories;

public class ReportRepository: IReportRepository
{
    private static readonly ReportComparer Comparer = new();
    private static readonly List<Report> Reports = new();

    public Report CreateReport(string serviceName, string folderPath)   // why and why...
    {
        var report = new Report();
        // logic !!!!!!!!!!!
        HandleLogs(FileManager.GetLogFiles(folderPath, serviceName));
        // logic !!!!!!!!!!!
        Reports.Add(report);
        SortList();
        return report;
    }

    public IReadOnlyList<Report> GetAllReportsByServiceName(string serviceName, string folderPath)
    {
        HandleLogs(FileManager.GetLogFiles(folderPath, serviceName));
        return Reports;
    }

    public IReadOnlyList<Report> GetAllReports()
    {
        return Reports;
    }
    public void ReadJsonFile()
    {
        //_reportRepositoryImplementation.ReadJsonFile();
    }

    public void SortList()
    {
        Reports.Sort(Comparer);
    }

    public void ClearList()
    {
        Reports.Clear();
    }

    private void HandleLogs(List<string> files)
    {
        if (files.Count > 0)
        {
            var parts = files[0].Split('.');
            var serviceName = parts[0];
            if (!int.TryParse(parts[1], out var rotationNumber))
            {
                rotationNumber = 0;
            }

            // need to open each file and get all written logs, parse them,
            // get first and last dates and check categories and handle their count
            
            var report = new Report()
            {
                ServiceName = serviceName,
                FirstReportDate = DateTime.Today,                   // change!!!
                LastReportDate = DateTime.Today,                    // change!!!
                NumberOfReports = new Dictionary<string, int>(),
                NumberOfRotations = rotationNumber
            };
            Reports.Add(report);
            SortList();
        }
    }

    private void HandleLogCategories()
    {
        
    }
    
    private void EmailAnonymization()       // censor, extra
    {
        
    }
}

/*
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text.Json;

namespace MessageService.Repositories
{
    /// <summary>
    /// Класс хранилища данных пользователей.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// Файл для хранения списка пользователей.
        /// </summary>
        internal const string FileNameUser = "Users.json";
        /// <summary>
        /// Объект для генерации случайных чисел.
        /// </summary>
        private static readonly Random Random = new();
        /// <summary>
        /// Словарь зарегистрированных почт.
        /// </summary>
        private static readonly Dictionary<string, string> Emails = new();
        /// <summary>
        /// Список пользователей.
        /// </summary>
        private static List<User> _listOfUsers;

        /// <summary>
        /// Возвращает список всех пользователей.
        /// </summary>
        /// <returns> Список пользователей </returns>
        public IReadOnlyList<User> GetAllUsers()
        {
            SortList();
            return _listOfUsers;
        }

        /// <summary>
        /// Создаёт нового пользователя и добавляет его в список.
        /// </summary>
        /// <returns> Нового случайного пользователя </returns>
        public User GetRandomUser()
        {
            var userName = GetRandomName();
            var email = GetRandomEmail(userName);

            while (Emails.ContainsKey(email))
            {
                email = GetRandomEmail(userName + (Random.Next(1, 10000)).ToString());
            }
            Emails.Add(email, userName);

            var user = new User()
            {
                UserName = userName,
                Email = email
            };

            _listOfUsers.Add(user);
            SortList();

            return user;
        }

        /// <summary>
        /// Читает json-файл и инициализирует список пользователей.
        /// </summary>
        public void ReadJsonFile()
        {
            using StreamReader sr = new(FileNameUser);
            string json = sr.ReadToEnd();
            _listOfUsers = JsonSerializer.Deserialize<List<User>>(json);
        }

        /// <summary>
        /// Сортирует список пользователей и записывает его текущее состояние в json-файл.
        /// </summary>
        public void SortList()
        {
            _listOfUsers.Sort(new UserComparer());
            using var fs = new FileStream(FileNameUser, FileMode.Create);
            var formatter = new DataContractJsonSerializer(typeof(List<User>));
            formatter.WriteObject(fs, _listOfUsers);
        }

        /// <summary>
        /// Чистит список пользователей.
        /// </summary>
        public void ClearList()
        {
            _listOfUsers = new List<User>();
        }

        /// <summary>
        /// Получает случайно сгенерированное имя (не осмысленное).
        /// </summary>
        /// <returns> Имя пользователя </returns>
        public static string GetRandomName()
        {
            var length = Random.Next(2, 16);
            var name = ((char)Random.Next('A', 'Z' + 1)).ToString();
            for (var i = 0; i < length - 1; i++)
            {
                name += (char)Random.Next('a', 'z' + 1);
            }
            return name;
        }

        /// <summary>
        /// Получает случайно сгенерированное имя (довольно осмысленное).
        /// </summary>
        /// <param name="name"> Имя пользователя </param>
        /// <returns> Идентификатор пользователя </returns>
        public static string GetRandomEmail(string name)
        {
            string[] domains = { "@mail.ru", "@gmail.com", "@yandex.ru", "@edu.hse.ru", "@bk.ru", "@analyst.ru" };
            var email = name.ToLower() + domains[Random.Next(0, domains.Length)];
            return email;
        }      
    }
}
*/