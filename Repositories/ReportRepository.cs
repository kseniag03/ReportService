using System.Text.Json;
using ReportService.Library;
using ReportService.Models;

namespace ReportService.Repositories;

public class ReportRepository: IReportRepository
{
    /// <summary>
    /// Файл для хранения списка отчётов.
    /// </summary>
    private const string ReportsJsonFileName = "Reports.json";
    
    private static readonly ReportComparer Comparer = new();
    
    private static readonly List<Report> Reports = new();
    
    private static readonly List<Log> Logs = new();
    
    public void ReadReportFromJsonFile()
    {
        if (!File.Exists(ReportsJsonFileName))
        {
            File.Create(ReportsJsonFileName).Dispose();
            Reports.Clear();
            return;
        }
        var options = new JsonSerializerOptions { WriteIndented = true };
        var jsonString = File.ReadAllText(ReportsJsonFileName);
        Reports.Clear();
        try
        {
            var jsonObject = JsonSerializer.Deserialize<List<Report>>(jsonString, options);
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
        File.WriteAllText(ReportsJsonFileName, jsonString);
    }

    public IReadOnlyList<Report> GetAllReportsByServiceName(string serviceName, string folderPath)
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

    private static void SortList()
    {
        Reports.Sort(Comparer);
    }

    private static void ClearList()
    {
        Reports.Clear();
    }

    private void CreateReportOnLogs(List<string> files)
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

    private void HandleLogs(List<string> files, ref Report report)
    {
        foreach (var file in files)
        {
            var content = FileManager.GetFileContent(file);
            foreach (var line in content)
            {
                if (line is null) continue;
                // принимаем за гарантию формат ввода логов: каждая линия -- лог: [Дата Время][Категория_записи] Текст_записи.
                var log = LogParser.ParseLog(line);
                if (Logs.Contains(log)) continue;
                Logs.Add(log);
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
    
}
