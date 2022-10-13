using MetricsAgent.Services;
using Quartz;
using System.Diagnostics;

namespace MetricsAgent.Job
{
    public class DotNetMetricJob : IJob
    {
        private PerformanceCounter _dotNetCounter;
        private IServiceScopeFactory _serviceScopeFactory;

        public DotNetMetricJob(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _dotNetCounter = new PerformanceCounter(".NET CLR Memory", "Gen 0 heap size", Process.GetCurrentProcess().ProcessName);
        }

        public Task Execute(IJobExecutionContext context)
        {

            using (IServiceScope serviceScope = _serviceScopeFactory.CreateScope())
            {
                var dotNetMetricsRepository = serviceScope.ServiceProvider.GetService<IDotNetMetricsRepository>();
                try
                {
                    var dotNetUsageInPercents = _dotNetCounter.NextValue();
                    var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                    Debug.WriteLine($"{time} > {dotNetUsageInPercents}");
                    dotNetMetricsRepository.Create(new Models.DotNetMetric
                    {
                        Value = (int)dotNetUsageInPercents,
                        Time = (long)time.TotalSeconds
                    });
                }
                catch (Exception ex)
                {

                }
            }               

            return Task.CompletedTask;
        }
    }
}
