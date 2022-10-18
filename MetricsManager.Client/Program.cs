namespace MetricsManager.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            CpuMetricsClient cpuMetricsClient = new CpuMetricsClient("http://localhost:5159", new HttpClient());
            DotNetMetricsClient dotNetMetricsClient = new DotNetMetricsClient("http://localhost:5159", new HttpClient());
            HddMetricsClient hddMetricsClient = new HddMetricsClient("http://localhost:5159", new HttpClient());
            NetworkMetricsClient networkMetricsClient = new NetworkMetricsClient("http://localhost:5159", new HttpClient());
            RamMetricsClient ramMetricsClient = new RamMetricsClient("http://localhost:5159", new HttpClient());

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Задачи");
                Console.WriteLine("==============================================");
                Console.WriteLine("1 - Получить метрики за последнюю минуту (CPU)");
                Console.WriteLine("2 - Получить метрики за последнюю минуту (DotNet)");
                Console.WriteLine("3 - Получить метрики за последнюю минуту (Hdd)");
                Console.WriteLine("4 - Получить метрики за последнюю минуту (Network)");
                Console.WriteLine("5 - Получить метрики за последнюю минуту (Ram)");
                Console.WriteLine("0 - Завершение работы приложения");
                Console.WriteLine("==============================================");
                Console.Write("Введите номер задачи: ");
                if (int.TryParse(Console.ReadLine(), out int taskNumber))
                {
                    switch (taskNumber)
                    {
                        case 0:
                            Console.WriteLine("Завершение работы приложения.");
                            Console.ReadKey(true);
                            break;
                        case 1:
                            try
                            {
                                TimeSpan toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                                TimeSpan fromTime = toTime - TimeSpan.FromSeconds(60);

                                CpuMetricsResponse response = await cpuMetricsClient.GetAllByIdAsync(
                                    1,
                                    fromTime.ToString("dd\\.hh\\:mm\\:ss"),
                                    toTime.ToString("dd\\.hh\\:mm\\:ss"));

                                foreach (CpuMetric metric in response.Metrics)
                                {
                                    Console.WriteLine($"{TimeSpan.FromSeconds(metric.Time).ToString("dd\\.hh\\:mm\\:ss")} >>> {metric.Value}");
                                }
                                Console.WriteLine("Нажмите любую клавишу для продолжения работы ...");
                                Console.ReadKey(true);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Произошла ошибка при попыте получить CPU метрики.\n{e.Message}");
                            }
                            break;
                        case 2:
                            try
                            {
                                TimeSpan toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                                TimeSpan fromTime = toTime - TimeSpan.FromSeconds(60);

                                DotNetMetricsResponse response = await dotNetMetricsClient.GetAllByIdAsync(
                                    1,
                                    fromTime.ToString("dd\\.hh\\:mm\\:ss"),
                                    toTime.ToString("dd\\.hh\\:mm\\:ss"));

                                foreach (DotNetMetric metric in response.Metrics)
                                {
                                    Console.WriteLine($"{TimeSpan.FromSeconds(metric.Time).ToString("dd\\.hh\\:mm\\:ss")} >>> {metric.Value}");
                                }
                                Console.WriteLine("Нажмите любую клавишу для продолжения работы ...");
                                Console.ReadKey(true);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Произошла ошибка при попыте получить CPU метрики.\n{e.Message}");
                            }
                            break;
                        case 3:
                            try
                            {
                                TimeSpan toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                                TimeSpan fromTime = toTime - TimeSpan.FromSeconds(60);

                                HddMetricsResponse response = await hddMetricsClient.GetAllByIdAsync(
                                    1,
                                    fromTime.ToString("dd\\.hh\\:mm\\:ss"),
                                    toTime.ToString("dd\\.hh\\:mm\\:ss"));

                                foreach (HddMetric metric in response.Metrics)
                                {
                                    Console.WriteLine($"{TimeSpan.FromSeconds(metric.Time).ToString("dd\\.hh\\:mm\\:ss")} >>> {metric.Value}");
                                }
                                Console.WriteLine("Нажмите любую клавишу для продолжения работы ...");
                                Console.ReadKey(true);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Произошла ошибка при попыте получить CPU метрики.\n{e.Message}");
                            }
                            break;
                        case 4:
                            try
                            {
                                TimeSpan toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                                TimeSpan fromTime = toTime - TimeSpan.FromSeconds(60);

                                NetworkMetricsResponse response = await networkMetricsClient.GetAllByIdAsync(
                                    1,
                                    fromTime.ToString("dd\\.hh\\:mm\\:ss"),
                                    toTime.ToString("dd\\.hh\\:mm\\:ss"));

                                foreach (NetworkMetric metric in response.Metrics)
                                {
                                    Console.WriteLine($"{TimeSpan.FromSeconds(metric.Time).ToString("dd\\.hh\\:mm\\:ss")} >>> {metric.Value}");
                                }
                                Console.WriteLine("Нажмите любую клавишу для продолжения работы ...");
                                Console.ReadKey(true);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Произошла ошибка при попыте получить CPU метрики.\n{e.Message}");
                            }
                            break;
                        case 5:
                            try
                            {
                                TimeSpan toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                                TimeSpan fromTime = toTime - TimeSpan.FromSeconds(60);

                                RamMetricsResponse response = await ramMetricsClient.GetAllByIdAsync(
                                    1,
                                    fromTime.ToString("dd\\.hh\\:mm\\:ss"),
                                    toTime.ToString("dd\\.hh\\:mm\\:ss"));

                                foreach (RamMetric metric in response.Metrics)
                                {
                                    Console.WriteLine($"{TimeSpan.FromSeconds(metric.Time).ToString("dd\\.hh\\:mm\\:ss")} >>> {metric.Value}");
                                }
                                Console.WriteLine("Нажмите любую клавишу для продолжения работы ...");
                                Console.ReadKey(true);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Произошла ошибка при попыте получить CPU метрики.\n{e.Message}");
                            }
                            break;
                        default:
                            Console.WriteLine("Введите корректный номер подзадачи.");
                            break;
                    }
                }
            }
        }
    }
}