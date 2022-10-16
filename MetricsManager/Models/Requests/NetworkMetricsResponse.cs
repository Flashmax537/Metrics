using System.Text.Json.Serialization;

namespace MetricsManager.Models.Requests
{
    public class NetworkMetricsResponse
    {
        /// <summary>
        /// Идентификатор агента
        /// </summary>
        public int AgentId { get; set; }

        [JsonPropertyName("metrics")]
        public NetworkMetric[] Metrics { get; set; }
    }
}
