using WorkerService;
using WorkerService.Services;

var builder = Host.CreateApplicationBuilder(args);

// Servisleri kaydet
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
