using WorkerService.Services;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IReportService _reportService;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IReportService reportService, IConfiguration configuration)
        {
            _logger = logger;
            _reportService = reportService;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var intervalSeconds = _configuration.GetValue<int>("ReportWorker:IntervalSeconds", 30);
            var enableReporting = _configuration.GetValue<bool>("ReportWorker:EnableReporting", true);
            
            _logger.LogInformation("ReportWorker başlatıldı. Rapor aralığı: {IntervalSeconds} saniye, Rapor durumu: {EnableReporting}", 
                intervalSeconds, enableReporting ? "Aktif" : "Pasif");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Worker çalışıyor: {time}", DateTimeOffset.Now);
                    
                    // Simüle edilmiş iş yükü
                    await SimulateWorkAsync(stoppingToken);
                    
                    // Rapor oluştur ve kaydet
                    if (enableReporting)
                    {
                        var report = await _reportService.GenerateReportAsync();
                        await _reportService.SaveReportAsync(report);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Worker işlemi sırasında hata oluştu");
                }
                
                await Task.Delay(TimeSpan.FromSeconds(intervalSeconds), stoppingToken);
            }
        }

        private async Task SimulateWorkAsync(CancellationToken cancellationToken)
        {
            // Simüle edilmiş veri işleme
            var itemsToProcess = Random.Shared.Next(10, 100);
            
            for (int i = 0; i < itemsToProcess; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;
                    
                // Simüle edilmiş işlem süresi
                await Task.Delay(Random.Shared.Next(10, 50), cancellationToken);
                
                // İşlenmiş öğe sayacını artır
                if (_reportService is ReportService reportService)
                {
                    reportService.IncrementProcessedItems();
                }
            }
            
            _logger.LogInformation("{ItemCount} öğe işlendi", itemsToProcess);
        }
    }
}
