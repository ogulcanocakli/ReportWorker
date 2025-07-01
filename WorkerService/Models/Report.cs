namespace WorkerService.Models
{
    public class Report
    {
        public DateTime Timestamp { get; set; }
        public string MachineName { get; set; } = Environment.MachineName;
        public long MemoryUsage { get; set; }
        public double CpuUsage { get; set; }
        public int ThreadCount { get; set; }
        public TimeSpan Uptime { get; set; }
        public string Status { get; set; } = "Running";
        public int ProcessedItems { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
