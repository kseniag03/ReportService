using Microsoft.AspNetCore.Mvc;
using ReportService.Repositories;

namespace ReportService.Controllers;

/// <summary>
/// Класс-контроллер для реализации взаимодействия между хранилищем отчётов и API-сервисом.
/// </summary>
[ApiController]
[Route("[controller]")]
public class ReportController : ControllerBase
{
    /// <summary>
    /// Регистратор сообщений и ошибок.
    /// </summary>
    private readonly ILogger<ReportController> _logger;

    /// <summary>
    /// Хранилище данных с отчётами.
    /// </summary>
    private readonly IReportRepository _reportRepository;

    /// <summary>
    /// Конструктор обработчика.
    /// </summary>
    /// <param name="reportRepository"> Хранилище данных </param>
    /// <param name="logger"> Регистратор сообщений и ошибок </param>
    public ReportController(IReportRepository reportRepository, [FromServices]ILogger<ReportController> logger)
    {
        _reportRepository = reportRepository;
        _logger = logger;
        _reportRepository.ReadReportFromJsonFile();
    }

    /// <summary>
    /// Получение списка отчётов по имени сервиса.
    /// </summary>
    /// <param name="serviceName"> Имя сервиса </param>
    /// <param name="folderPath"> Путь к директории с логами </param>
    /// <returns> Список отчётов </returns>
    [HttpGet("GET REPORT BY SERVICE NAME")]
    public IActionResult GetReportsByServiceName(string serviceName, string folderPath)
    {
        _logger.LogInformation("Processing request...");
        try
        {
            var reports = _reportRepository.GetReportsByServiceName(serviceName, folderPath);
            if (!reports.Any())
            {
                _logger.LogInformation("Report list is empty");
                //return NotFound();
            }
            _logger.LogInformation("Request completed.");
            return Ok(reports);
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"Error: { ex.Message }");
            return BadRequest(ex.Message);
        }
    }
    
    /// <summary>
    /// Получение списка всех сгенерированных отчётов.
    /// </summary>
    /// <returns> Список отчётов </returns>
    [HttpGet("GET ALL REPORTS")]
    public IActionResult GetAllReports()
    {
        _logger.LogInformation("Processing request...");
        try
        {
            var reports = _reportRepository.GetAllReports();
            if (!reports.Any())
            {
                _logger.LogInformation("Report list is empty");
                //return NotFound();
            }
            _logger.LogInformation("Request completed.");
            return Ok(reports);
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"Error: { ex.Message }");
            return BadRequest(ex.Message);
        }
    }
    
    /// <summary>
    /// Получение списка логов.
    /// </summary>
    /// <returns> Список логов </returns>
    [HttpGet("GET ALL LOGS")]
    public IActionResult GetAllLogs()
    {
        _logger.LogInformation("Processing request...");
        try
        {
            var reports = _reportRepository.GetSystemLogs();
            if (!reports.Any())
            {
                _logger.LogInformation("Log list is empty");
                //return NotFound();
            }
            _logger.LogInformation("Request completed.");
            return Ok(reports);
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"Error: { ex.Message }");
            return BadRequest(ex.Message);
        }
    }
    
    /// <summary>
    /// Очистка текущего списка отчётов и JSON-файла
    /// </summary>
    /// <returns> Сообщение об успешной очистке списка отчётов или об ошибке </returns>
    [HttpDelete("CLEAR CURRENT REPORT LIST")]
    public IActionResult ClearReportsList()
    {
        _logger.LogInformation("Processing request...");
        try
        {
            _reportRepository.ClearList();
            _reportRepository.WriteReportToJsonFile();
            _logger.LogInformation("Request completed.");
            return Ok("Report list has been cleared");
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"Error: { ex.Message }");
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Сохранение текущего списка отчётов в JSON-файл
    /// </summary>
    /// <returns> Сообщение об успешном сохранении списка отчётов или об ошибке </returns>
    [HttpPut("SAVE CURRENT REPORT LIST")]
    public IActionResult SaveReports()
    {
        _logger.LogInformation("Processing request...");
        try
        {
            _reportRepository.WriteReportToJsonFile();
            _logger.LogInformation("Request completed.");
            return Ok("Report list has been saved");
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"Error: { ex.Message }");
            return BadRequest(ex.Message);
        }
    }
}