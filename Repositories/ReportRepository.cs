using System.Text.Json;
using ReportService.Library;
using ReportService.Models;

namespace ReportService.Repositories;

/// <summary>
/// Основной класс для определения логики работы с генерацией отчётов.
/// </summary>
public class ReportRepository: IReportRepository
{
    /// <summary>
    /// Файл для хранения списка отчётов.
    /// </summary>
    private const string ReportsJsonFileName = "Reports.json";
    
    /// <summary>
    /// Объект для сравнения отчётов.
    /// </summary>
    private static readonly ReportComparer Comparer = new();
    
    /// <summary>
    /// Список отчётов.
    /// </summary>
    private static readonly List<Report> Reports = new();
    
    /// <summary>
    /// Список логов, затронутых во время генерации отчётов.
    /// </summary>
    private static readonly List<Log> Logs = new();

    public void ReadReportFromJsonFile()
    {
        if (!File.Exists(ReportsJsonFileName))
        {
            File.Create(ReportsJsonFileName).Dispose();
        }
        var options = new JsonSerializerOptions { WriteIndented = true };
        var jsonString = File.ReadAllText(ReportsJsonFileName);
        try
        {
            var jsonObject = JsonSerializer.Deserialize<List<Report>>(jsonString, options);
            ClearList();
            if (jsonObject != null) Reports.AddRange(jsonObject);
        }
        catch (Exception)
        {
            ClearList();
        }
    }

    public void WriteReportToJsonFile()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var jsonString = JsonSerializer.Serialize(Reports, options);
        if (File.Exists(ReportsJsonFileName))
        {
            File.WriteAllText(ReportsJsonFileName, jsonString);
        }
        else
        {
            File.CreateText(ReportsJsonFileName).Close();
            File.WriteAllText(ReportsJsonFileName, jsonString);
        }
    }
    
    public void ClearList()
    {
        Reports.Clear();
    }

    public IReadOnlyList<Report> GetReportsByServiceName(string serviceName, string folderPath)
    {
        ClearList();
        CreateReportOnLogs(FileManager.GetLogFiles(folderPath, serviceName));
        return Reports;
    }

    public IReadOnlyList<Report> GetAllReports()
    {
        return Reports;
    }

    public IReadOnlyList<Log> GetSystemLogs()
    {
        return Logs;
    }

    /// <summary>
    /// Сортирует список отчётов (по имени сервиса и самой ранней дате).
    /// </summary>
    private static void SortList()
    {
        Reports.Sort(Comparer);
    }

    /// <summary>
    /// Получает список файлов для каждого имени сервиса, формирует начальный отчёт
    /// </summary>
    /// <param name="files"> Список файлов </param>
    private static void CreateReportOnLogs(List<string> files)
    {
        if (files.Count <= 0) return;
        
        var serviceNames = new Dictionary<string, List<string>>();
        foreach (var file in files)
        {
            var fileInfo = new FileInfo(file);
            var parts = fileInfo.Name.Split('.');
            if (serviceNames.ContainsKey(parts[0]))
            {
                serviceNames[parts[0]].Add(file);
            }
            else
            {
                serviceNames[parts[0]] = new List<string>() { file };
            }
        }

        foreach (var kvp in serviceNames)
        {
            // need to open each file and get all written logs, parse them,
            // get first and last dates and check categories and handle their count
            
            var report = new Report()
            {
                ServiceName = kvp.Key,
                FirstReportDate = DateTime.MaxValue,
                LastReportDate = DateTime.MinValue,
                NumberOfReports = new Dictionary<string, int>(),
                NumberOfRotations = kvp.Value.Count - 1
            };
        
            HandleLogs(kvp.Value, ref report); // for each service name need their own files list

            if (Reports.Contains(report)) continue;
            Reports.Add(report);
            SortList();
        }
    }

    /// <summary>
    /// Формирует список логов, просматривает контент в файлах и меняет св-ва отчёта на актуальные
    /// </summary>
    /// <param name="files"> Список файлов по конкретному сервису </param>
    /// <param name="report"> Отчёт для актуализации данных </param>
    private static void HandleLogs(List<string> files, ref Report report)
    {
        foreach (var log in 
                 from file in files 
                 select FileManager.GetFileContent(file) into content 
                 from line in content 
                 where line is not null 
                 select LogParser.ParseLog(line))
        {
            if (!Logs.Contains(log))
            {
                Logs.Add(log);
            }
            if (log.Date < report.FirstReportDate)
            {
                report.FirstReportDate = log.Date;
            }
            if (log.Date > report.LastReportDate)
            {
                report.LastReportDate = log.Date;
            }
            if (report.NumberOfReports.ContainsKey(log.Category))
            {
                ++report.NumberOfReports[log.Category];
            }
            else
            {
                report.NumberOfReports[log.Category] = 1;
            }
        }
    }
}
