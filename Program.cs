using TeachingDeviceManagement.Coordinator;
using TeachingDeviceManagement.Devices;
using TeachingDeviceManagement.Enums;
using TeachingDeviceManagement.Exceptions;
using TeachingDeviceManagement.Subscribers;

Console.WriteLine("=== CHUONG TRINH QUAN LY THIET BI PHONG HOC (PHIEN BAN V2 - FIX MOT PHAN) ===");

var projector = new ProjectorRemote("PRJ-01", "Dieu khien may chieu A201");
var airCon = new AirConditionerRemote("AC-01", "Dieu hoa phong A201", "A201");
var mic = new WirelessMicrophone("MIC-01", "Micro khong day A201");

var devices = new List<TeachingDevice> { projector, airCon, mic };

var notifyService = new ConsoleNotificationService();
var auditLogger = new DeviceAuditLogger();
var maintenance = new MaintenanceService();

var coordinator = new DeviceMonitoringCoordinator(devices, notifyService, auditLogger, maintenance);

// Dang ky 1 lan
coordinator.Subscribe();

Console.WriteLine("\n--- TEST 1: THU NGOAI LE KHI GIA TRI KHONG HOP LE ---");
try
{
    projector.UpdateBatteryLevel(150);
}
catch (InvalidDeviceReadingException ex)
{
    Console.WriteLine($"[Bat duoc loi ngoai le]: Device {ex.DeviceId}, Gia tri loi: {ex.InvalidValue} - {ex.Message}");
}

Console.WriteLine("\n--- TEST 2: CAP NHAT PIN MAY CHIEU (DA FIX PIN 0 LA CRITICAL) ---");
projector.UpdateBatteryLevel(15); // Warning
projector.UpdateBatteryLevel(0);  // Da fix thanh Critical!

Console.WriteLine("\n--- TEST 3: DIEU HOA MAT PHAN HOI (DA CHAN BAN TRUNG) ---");
airCon.UpdateResponseState(false); // Critical
airCon.UpdateResponseState(false); // Da chan thanh cong, khong ban duplicate!

Console.WriteLine("\n--- TEST 4: MICRO PIN = 0 VA TIN HIEU = 0 ---");
mic.UpdateBatteryLevel(0); // Da fix thanh Critical
mic.UpdateSignalQuality(0); // Da fix thanh Critical

Console.WriteLine("\n--- TEST 5: SOT O V2 - CO BAN DANG KY TRUNG (SUBSCRIBE 2 LAN) ---");
// Neu coordinator bi goi subscribe them 1 lan nua:
coordinator.Subscribe();
Console.WriteLine("Phat sinh them 1 alert sau khi subscribe 2 lan:");
projector.UpdateBatteryLevel(10); // Se bi hien thi 2 dong do bi dang ky trung!

coordinator.Dispose();
Console.WriteLine("\n[v2 ket thuc: da fix duoc validate va do uu tien canh bao, con sot chan dang ky trung lap]");
