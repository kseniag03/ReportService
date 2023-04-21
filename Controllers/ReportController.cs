using Microsoft.AspNetCore.Mvc;
using ReportService.Repositories;

namespace ReportService.Controllers;

[ApiController]
[Route("[controller]")]
public class ReportController : ControllerBase
{
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
            var reports = _reportRepository.GetAllReportsByServiceName(serviceName, folderPath);
            if (!reports.Any())
            {
                _logger.LogInformation("Error: report list is empty");
                return NotFound();
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
    /// Получение списка отчётов.
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
                _logger.LogInformation("Error: report list is empty");
                return NotFound();
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
                _logger.LogInformation("Error: report list is empty");
                return NotFound();
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