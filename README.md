# ReportWorker

Bu proje, düzenli aralıklarla sistem performans raporları oluşturan bir .NET Worker Service uygulamasıdır.

## Özellikler

- ⏰ Yapılandırılabilir rapor aralığı
- 📊 Sistem performans metrikleri toplama
- 💾 JSON formatında rapor kaydetme
- 🔧 Yapılandırılabilir ayarlar
- 📝 Detaylı loglama

## Yapılandırma

`appsettings.json` dosyasında aşağıdaki ayarları yapılandırabilirsiniz:

```json
{
  "ReportWorker": {
    "IntervalSeconds": 30,        // Rapor oluşturma aralığı (saniye)
    "EnableReporting": true       // Rapor özelliğini açıp kapatma
  },
  "Reports": {
    "Directory": "Reports"        // Raporların kaydedileceği klasör
  }
}
```

## Rapor İçeriği

Her rapor şu bilgileri içerir:

- 📅 Zaman damgası
- 🖥️ Makine adı
- 🧠 Bellek kullanımı
- ⚡ CPU kullanımı (tahmini)
- 🧵 Thread sayısı
- ⏱️ Çalışma süresi
- 📈 İşlenen öğe sayısı
- ✅ Durum bilgisi

## Çalıştırma

```bash
dotnet run --project WorkerService
```

## Geliştirme

Bu proje aşağıdaki teknolojileri kullanır:

- .NET 9.0
- Microsoft.Extensions.Hosting
- JSON serializasyon
- Dependency Injection
- ILogger interface

## Örnek Rapor

```json
{
  "timestamp": "2025-07-01T10:30:00",
  "machineName": "DESKTOP-ABC123",
  "memoryUsage": 25165824,
  "cpuUsage": 15.7,
  "threadCount": 12,
  "uptime": "00:05:30",
  "status": "Running",
  "processedItems": 150
}
```
