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

        public ReportController(ILogger<ReportController> logger)
        {
            _logger = logger;
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
        /// Конструктор обработчика.
        /// </summary>
        /// <param name="reportRepository"> Хранилище данных </param>
        public ReportController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
            _reportRepository.ReadJsonFile();
        }


        /// <summary>
        /// Получение отчёта (или списка отчётов) по имени сервиса.
        /// </summary>
        /// <param name="name"> Идентификатор пользователя </param>
        /// <returns> Список отчётов </returns>
        [HttpGet("GetReportsByServiceName/{name}")]
        public IActionResult GetReportsByServiceName(string serviceName, string filePath)
        {
            try
            {
                var reports = _reportRepository.GetAllReports();//.SingleOrDefault(u => u.Email == email);
                if (reports == null)
                {
                    return NotFound();
                }
                return Ok(reports);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        /// <summary>
        /// Добавление новых отчётов
        /// </summary>
        /// <param name="count"> Кол-во добавляемых отчётов (не более 100) </param>
        /// <returns> Список новых отчётов, который добавляется к имеющемуся списку </returns>
        [HttpPost("add-report")]
        public ActionResult<IReadOnlyList<Report>> GetNewUsers(int count, Report report)
        {
            try
            {
                if (count > 100)
                {
                    return BadRequest(count);
                }
                var response = Enumerable.Range(0, count)
                    .Select(x => report)
                    .ToList();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }