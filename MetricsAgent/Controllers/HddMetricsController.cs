using AutoMapper;
using MetricsAgent.Models;
using MetricsAgent.Models.Dto;
using MetricsAgent.Models.Requests;
using MetricsAgent.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        #region Services

        private readonly ILogger<HddMetricsController> _logger;
        private readonly IHddMetricsRepository _hddMetricsRepository;
        private readonly IMapper _mapper;

        #endregion

        public HddMetricsController(
            IHddMetricsRepository hddMetricsRepository,
            ILogger<HddMetricsController> logger,
            IMapper mapper)
        {
            _hddMetricsRepository = hddMetricsRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] HddMetricCreateRequest request)
        {
            _hddMetricsRepository.Create(_mapper.Map<HddMetric>(request));
            return Ok();
        }

        /// <summary>
        /// Получить статистику по нагрузке на жесткий диск за период
        /// </summary>
        /// <param name="fromTime">Время начала периода</param>
        /// <param name="toTime">Время окончания периода</param>
        /// <returns></returns>
        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetHddMetrics(
            [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation("Get hdd metrics call.");
            return Ok(_hddMetricsRepository.GetByTimePeriod(fromTime, toTime)
                        .Select(metric => _mapper.Map<HddMetricDto>(metric)).ToList());
            //Random random = new Random();
            //switch (random.Next(2))
            //{
            //    case 0:
            //        return Ok(_hddMetricsRepository.GetByTimePeriod(fromTime, toTime)
            //            .Select(metric => _mapper.Map<HddMetricDto>(metric)).ToList());
            //    case 1:
            //        throw new Exception("Internal Server Error.");
            //}
            //throw new Exception("Internal Server Error.");
        }

        [HttpGet("all")]
        public ActionResult<IList<HddMetricDto>> GetAllCpuMetrics()
        {
            return Ok(_hddMetricsRepository.GetAll()
                        .Select(metric => _mapper.Map<HddMetricDto>(metric)).ToList());
            //Random random = new Random();
            //switch (random.Next(2))
            //{
            //    case 0:
            //        return Ok(_hddMetricsRepository.GetAll()
            //            .Select(metric => _mapper.Map<HddMetricDto>(metric)).ToList());
            //    case 1:
            //        throw new Exception("Internal Server Error.");
            //}
            //throw new Exception("Internal Server Error.");
        }
    }
}
