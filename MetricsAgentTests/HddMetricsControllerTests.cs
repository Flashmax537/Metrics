using MetricsAgent.Controllers;
using MetricsAgent.Models;
using MetricsAgent.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace MetricsAgentTests
{
    public class HddMetricsControllerTests
    {
        private HddMetricsController _hddMetricsController;
        private Mock<IHddMetricsRepository> _mock;

        public HddMetricsControllerTests()
        {
            var mockLogger = new Mock<ILogger<HddMetricsController>>();
            var logger = mockLogger.Object;
            _mock = new Mock<IHddMetricsRepository>();

            _hddMetricsController = new HddMetricsController(_mock.Object, logger);
        }

        [Fact]
        public void GetHddMetrics_ReturnOk()
        {
            TimeSpan fromTime = TimeSpan.FromSeconds(0);
            TimeSpan toTime = TimeSpan.FromSeconds(100);
            var result = _hddMetricsController.GetHddMetrics(fromTime, toTime);
            Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            // Устанавливаем параметр заглушки
            // В заглушке прописываем, что в репозиторий прилетит HddMetric-объект
            _mock.Setup(repository => repository.Create(It.IsAny<HddMetric>())).Verifiable();

            // Выполняем действие на контроллере
            var result = _hddMetricsController.Create(new
            MetricsAgent.Models.Requests.HddMetricCreateRequest
            {
                Time = TimeSpan.FromSeconds(1),
                Value = 50
            });

            // Проверяем заглушку на то, что пока работал контроллер
            // Вызвался метод Create репозитория с нужным типом объекта в параметре
            _mock.Verify(repository => repository.Create(It.IsAny<HddMetric>()), Times.AtMostOnce());
        }
    }
}
