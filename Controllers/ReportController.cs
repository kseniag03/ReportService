using Microsoft.AspNetCore.Mvc;
using ReportService.Library;
using ReportService.Models;
using ReportService.Repositories;

namespace ReportService.Controllers;

[ApiController]
[Route("[controller]")]
public class ReportController : ControllerBase
{

    private readonly ILogger<ReportController> _logger;

    /// <summary>
    /// Хранилище данных пользователей.
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
        _reportRepository.ReadJsonFile();
    }

    [HttpGet(Name = "GetReport")]
    public IEnumerable<Report> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new Report
        {
            ServiceName = "service1",
            FirstReportDate = DateTime.Now,
            LastReportDate = DateTime.Now,
            NumberOfReports = new Dictionary<string, int> { { "category1", 10 } },
            NumberOfRotations = 5
        })
        .ToArray();
    }

    /// <summary>
    /// Получение списка отчётов по имени сервиса.
    /// </summary>
    /// <param name="serviceName"> Имя сервиса </param>
    /// <returns> Список отчётов </returns>
    [HttpGet("GetReportsByServiceName/{serviceName}")]
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
    [HttpGet("GetAllReports")]
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

}