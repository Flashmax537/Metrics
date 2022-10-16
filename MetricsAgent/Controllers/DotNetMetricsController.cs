using AutoMapper;
using MetricsAgent.Models;
using MetricsAgent.Models.Dto;
using MetricsAgent.Models.Requests;
using MetricsAgent.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/dotnet")]
    [ApiController]
    public class DotNetMetricsController : ControllerBase
    {
        #region Services

        private readonly ILogger<DotNetMetricsController> _logger;
        private readonly IDotNetMetricsRepository _dotNetMetricsRepository;
        private readonly IMapper _mapper;

        #endregion

        public DotNetMetricsController(
            IDotNetMetricsRepository dotNetMetricsRepository,
            ILogger<DotNetMetricsController> logger,
            IMapper mapper)
        {
            _dotNetMetricsRepository = dotNetMetricsRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] DotNetMetricCreateRequest request)
        {
            _dotNetMetricsRepository.Create(_mapper.Map<DotNetMetric>(request));
            return Ok();
        }

        /// <summary>
        /// Получить статистику для приложений ASP.NET Core за период
        /// </summary>
        /// <param name="fromTime">Время начала периода</param>
        /// <param name="toTime">Время окончания периода</param>
        /// <returns></returns>
        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetDotNetMetrics(
            [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation("Get dotNet metrics call.");
            Random random = new Random();
            switch (random.Next(2))
            {
                case 0:
                    return Ok(_dotNetMetricsRepository.GetByTimePeriod(fromTime, toTime)
                        .Select(metric => _mapper.Map<DotNetMetricDto>(metric)).ToList());
                case 1:
                    throw new Exception("Internal Server Error.");
            }
            throw new Exception("Internal Server Error.");
        }

        [HttpGet("all")]
        public ActionResult<IList<DotNetMetricDto>> GetAllCpuMetrics()
        {
            Random random = new Random();
            switch (random.Next(2))
            {
                case 0:
                    return Ok(_dotNetMetricsRepository.GetAll()
                        .Select(metric => _mapper.Map<DotNetMetricDto>(metric)).ToList());
                case 1:
                    throw new Exception("Internal Server Error.");
            }
            throw new Exception("Internal Server Error.");
        }
    }
}
