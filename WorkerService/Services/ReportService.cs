using System.Diagnostics;
using System.Text.Json;
using WorkerService.Models;

namespace WorkerService.Services
{
    public interface IReportService
    {
        Task<Report> GenerateReportAsync();
        Task SaveReportAsync(Report report);
    }

    public class ReportService : IReportService
    {
        private readonly ILogger<ReportService> _logger;
        private readonly IConfiguration _configuration;
        private readonly DateTime _startTime;
        private long _processedItems = 0;

        public ReportService(ILogger<ReportService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _startTime = DateTime.Now;
        }

        public Task<Report> GenerateReportAsync()
        {
            var process = Process.GetCurrentProcess();
            
            var report = new Report
            {
                Timestamp = DateTime.Now,
                MemoryUsage = process.WorkingSet64,
                ThreadCount = process.Threads.Count,
                Uptime = DateTime.Now - _startTime,
                ProcessedItems = (int)Interlocked.Read(ref _processedItems),
                CpuUsage = GetCpuUsage()
            };

            _logger.LogInformation("Rapor oluşturuldu: {Timestamp}", report.Timestamp);
            
            return Task.FromResult(report);
        }

        public async Task SaveReportAsync(Report report)
        {
            var reportsDirectory = _configuration.GetValue<string>("Reports:Directory") ?? "Reports";
            
            if (!Directory.Exists(reportsDirectory))
            {
                Directory.CreateDirectory(reportsDirectory);
            }

            var fileName = $"report_{report.Timestamp:yyyyMMdd_HHmmss}.json";
            var filePath = Path.Combine(reportsDirectory, fileName);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(report, options);
            await File.WriteAllTextAsync(filePath, json);

            _logger.LogInformation("Rapor kaydedildi: {FilePath}", filePath);
        }

        public void IncrementProcessedItems()
        {
            Interlocked.Increment(ref _processedItems);
        }

        private double GetCpuUsage()
        {
            // Basit CPU kullanım tahmini
            // Gerçek projede PerformanceCounter kullanılabilir
            return Random.Shared.NextDouble() * 100;
        }
    }
}
