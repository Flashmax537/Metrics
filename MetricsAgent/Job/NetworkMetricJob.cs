﻿using MetricsAgent.Services;
using Quartz;
using System.Diagnostics;

namespace MetricsAgent.Job
{
    public class NetworkMetricJob : IJob
    {
        private PerformanceCounter _networkCounter;
        private IServiceScopeFactory _serviceScopeFactory;

        public NetworkMetricJob(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _networkCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", "get connection string from WMI");
        }

        public Task Execute(IJobExecutionContext context)
        {

            using (IServiceScope serviceScope = _serviceScopeFactory.CreateScope())
            {
                var networkMetricsRepository = serviceScope.ServiceProvider.GetService<INetworkMetricsRepository>();
                try
                {
                    var networkUsageInPercents = _networkCounter.NextValue();
                    var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                    Debug.WriteLine($"{time} > {networkUsageInPercents}");
                    networkMetricsRepository.Create(new Models.NetworkMetric
                    {
                        Value = (int)networkUsageInPercents,
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
