using MetricsManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetricsManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {

        #region Services

        private readonly AgentPool _agentPool;
        private readonly ILogger<AgentsController> _logger;

        #endregion

        #region Constuctors

        public AgentsController(AgentPool agentPool,
            ILogger<AgentsController> logger)
        {
            _agentPool = agentPool;
            _logger = logger;
        }

        #endregion

        #region Public Methods

        [HttpPost("register")]
        public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
        {
            _logger.LogInformation("Register agent call.");
            if (agentInfo != null)
            {
                _agentPool.Add(agentInfo);
            }
            return Ok();
        }

        [HttpPut("enable/{agentId}")]
        public IActionResult EnableAgentById([FromRoute] int agentId)
        {
            _logger.LogInformation("Enable agent call.");
            if (_agentPool.Agents.ContainsKey(agentId))
                _agentPool.Agents[agentId].Enable = true;
            return Ok();
        }

        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            _logger.LogInformation("Disable agent call.");
            if (_agentPool.Agents.ContainsKey(agentId))
                _agentPool.Agents[agentId].Enable = false;
            return Ok();
        }
        
        [HttpGet("get")]
        public ActionResult<AgentInfo[]> GetAllAgents()
        {
            _logger.LogInformation("Get all agent call.");
            return Ok(_agentPool.Get());
        }

        #endregion
    }
}
