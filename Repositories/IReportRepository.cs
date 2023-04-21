using ReportService.Models;

namespace ReportService.Repositories;

/// <summary>
/// Интерфейс для взаимодействия с хранилищем данных отчётов.
/// </summary>
public interface IReportRepository
{
    /// <summary>
    /// Читает json-файл и инициализирует список.
    /// </summary>
    void ReadReportFromJsonFile();
    
    /// <summary>
    /// Сохраняет текущее состояние списка отчётов в json-файл.
    /// </summary>
    void WriteReportToJsonFile();

    /// <summary>
    /// Чистит список отчётов.
    /// </summary>
    void ClearList();
    
    /// <summary>
    /// Возвращает список всех отчётов.
    /// </summary>
    /// <param name="serviceName"> Имя сервиса </param>
    /// <param name="folderPath"> Путь к директории с файлами логов </param>
    /// <returns> Список отчётов </returns>
    IReadOnlyList<Report> GetReportsByServiceName(string serviceName, string folderPath);
    
    /// <summary>
    /// Возвращает список всех отчётов.
    /// </summary>
    /// <returns> Список отчётов </returns>
    IReadOnlyList<Report> GetAllReports();

    /// <summary>
    /// Возвращает список всех системных логов, которые обрабатывались в процессе генерации отчёта
    /// </summary>
    /// <returns> Список логов </returns>
    public IReadOnlyList<Log> GetSystemLogs();
}