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
        _logger.LogInformation("Processing GET request...");
        try
        {
            var reports = _reportRepository.GetReportsByServiceName(serviceName, folderPath);
            if (!reports.Any())
            {
                _logger.LogInformation("Report list is empty");
                //return NotFound();
            }
            _logger.LogInformation("GET request was completed.");
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
        _logger.LogInformation("Processing GET-ALL-REPORTS request...");
        try
        {
            var reports = _reportRepository.GetAllReports();
            if (!reports.Any())
            {
                _logger.LogInformation("Report list is empty");
                //return NotFound();
            }
            _logger.LogInformation("GET-ALL-REPORTS request was completed.");
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
        _logger.LogInformation("Processing GET-ALL-LOGS request...");
        try
        {
            var reports = _reportRepository.GetSystemLogs();
            if (!reports.Any())
            {
                _logger.LogInformation("Log list is empty");
                //return NotFound();
            }
            _logger.LogInformation("GET-ALL-LOGS request was completed.");
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
        _logger.LogInformation("Processing CLEAR request...");
        try
        {
            _reportRepository.ClearList();
            _reportRepository.WriteReportToJsonFile();
            _logger.LogInformation("CLEAR request was completed.");
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
    [HttpPut("SAVE CURRENT REPORT LIST TO JSON")]
    public IActionResult SaveReports()
    {
        _logger.LogInformation("Processing SAVE request...");
        try
        {
            _reportRepository.WriteReportToJsonFile();
            _logger.LogInformation("SAVE request was completed.");
            return Ok("Report list has been saved");
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
    [HttpPut("LOAD REPORT LIST FROM JSON")]
    public IActionResult LoadReports()
    {
        _logger.LogInformation("Processing LOAD request...");
        try
        {
            _reportRepository.ReadReportFromJsonFile();
            _logger.LogInformation("LOAD request was completed.");
            return Ok("Report list has been loaded");
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"Error: { ex.Message }");
            return BadRequest(ex.Message);
        }
    }
}