using MetricsManager.Services;
using MetricsManager.Models;
using MetricsManager.Models.Requests;
using MetricsManager.Services.Client;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MetricsManager.Controllers
{
    [Route("api/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        #region Services

        private readonly ILogger<CpuMetricsController> _logger;
        private IHttpClientFactory _httpClientFactory;
        private IMetricsAgentClient _metricsAgentClient;
        private IAgentInfoRepository _agentInfoRepository;

        #endregion

        public CpuMetricsController(
            ILogger<CpuMetricsController> logger,
            IMetricsAgentClient metricsAgentClient,
            IHttpClientFactory httpClientFactory,
            IAgentInfoRepository agentInfoRepository)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _metricsAgentClient = metricsAgentClient;
            _agentInfoRepository = agentInfoRepository;
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent(
            [FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation("Get metrics from agent call.");
            return Ok(_metricsAgentClient.GetCpuMetrics(new CpuMetricsRequest
            {
                AgentId = agentId,
                FromTime = fromTime,
                ToTime = toTime
            }));
        }

        [HttpGet("agent-old/{agentId}/from/{fromTime}/to/{toTime}")]
        public ActionResult<CpuMetricsResponse> GetMetricsFromAgentOld(
            [FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            AgentInfo agentInfo = _agentInfoRepository.GetById(agentId);
            if (agentInfo == null)
                return BadRequest();

            string requestStr =
                $"{agentInfo.AgentAddress}api/metrics/cpu/from/{fromTime.ToString("dd\\.hh\\:mm\\:ss")}/to/{toTime.ToString("dd\\.hh\\:mm\\:ss")}";
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestStr);
            httpRequestMessage.Headers.Add("Accept", "application/json");
            HttpClient httpClient = _httpClientFactory.CreateClient();

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(3000); // 3 сек

            HttpResponseMessage response = httpClient.Send(httpRequestMessage, cancellationTokenSource.Token);
            if (response.IsSuccessStatusCode)
            {
                string responseStr = response.Content.ReadAsStringAsync().Result;
                var metrics = JsonConvert.DeserializeObject<CpuMetric[]>(responseStr);
                return Ok(new CpuMetricsResponse() { AgentId = agentId, Metrics = metrics });
            }
            return BadRequest();
        }

        [HttpGet("all/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAll(
            [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation("Get metrics from all call.");
            return Ok();
        }
    }
}
