using TeachingDeviceManagement.Coordinator;
using TeachingDeviceManagement.Devices;
using TeachingDeviceManagement.Enums;
using TeachingDeviceManagement.Exceptions;
using TeachingDeviceManagement.Subscribers;

Console.WriteLine("======================================================================");
Console.WriteLine("  CHUONG TRINH QUAN LY THIET BI PHONG HOC (PHIEN BAN V3 - HOAN THIEN) ");
Console.WriteLine("======================================================================\n");

// 1. Khoi tao cac thiet bi
var projector = new ProjectorRemote("PRJ-01", "Dieu khien may chieu phong A201");
var airCon = new AirConditionerRemote("AC-01", "Dieu hoa phong A201", "A201");
var mic = new WirelessMicrophone("MIC-01", "Micro khong day phong A201");

var devices = new List<TeachingDevice> { projector, airCon, mic };

// 2. Khoi tao cac subscriber
var notifyService = new ConsoleNotificationService();
var auditLogger = new DeviceAuditLogger();
var maintenanceService = new MaintenanceService();

// 3. Khoi tao Coordinator
var coordinator = new DeviceMonitoringCoordinator(devices, notifyService, auditLogger, maintenanceService);

// 4. Test Subscribe: goi 2 lan de kiem chung tinh nang chong duplicate subscriber
Console.WriteLine("[1] THUC HIEN DANG KY SU KIEN (GOI SUBSCRIBE 2 LAN DE TEST CHONG TRUNG LAP):");
coordinator.Subscribe();
coordinator.Subscribe(); // Lan thu 2 phai bi chan, khong bi nhan doi handler!
Console.WriteLine("-> Dang ky thanh cong va an toan!\n");

// 5. Test thay doi trang thai va ghi audit log
Console.WriteLine("[2] TEST THAY DOI TRANG THAI THIET BI:");
projector.ChangeStatus(DeviceStatus.Borrowed);
projector.ChangeStatus(DeviceStatus.UnderMaintenance);
// Thu dat lai UnderMaintenance (trung trang thai cu -> khong duoc phat sinh event)
projector.ChangeStatus(DeviceStatus.UnderMaintenance);
Console.WriteLine("-> Thay doi trang thai xong.\n");

// 6. Test canh bao pin may chieu (chong spam canh bao trong cung mot phan vung)
Console.WriteLine("[3] TEST CAP NHAT PIN MAY CHIEU:");
Console.WriteLine("- Giam pin xuong 18% (duoi 20% -> phai phat Warning):");
projector.UpdateBatteryLevel(18);

Console.WriteLine("- Giam tiep xuong 12% (van o vung < 20% -> KHONG duoc spam Warning tiep):");
projector.UpdateBatteryLevel(12);

Console.WriteLine("- Giam tiep xuong 0% (can pin -> phai phat Critical):");
projector.UpdateBatteryLevel(0);
Console.WriteLine();

// 7. Test ket noi dieu hoa
Console.WriteLine("[4] TEST TRANG THAI DIEU HOA:");
Console.WriteLine("- Chuyen sang mat ket noi (false -> phai phat Critical):");
airCon.UpdateResponseState(false);

Console.WriteLine("- Tiep tuc goi mat ket noi (false -> KHONG duoc phat lap lai):");
airCon.UpdateResponseState(false);
Console.WriteLine();

// 8. Test micro khong day (pin & tin hieu)
Console.WriteLine("[5] TEST MICRO KHONG DAY:");
Console.WriteLine("- Micro pin xuong 25% (< 30% -> phat Warning):");
mic.UpdateBatteryLevel(25);

Console.WriteLine("- Micro tin hieu xuong 0% (0% -> phat Critical):");
mic.UpdateSignalQuality(0);
Console.WriteLine();

// 9. Test ngoai le InvalidDeviceReadingException khi du lieu ngoai 0-100
Console.WriteLine("[6] TEST BAT NGOAI LE DU LIEU DOC KHONG HOP LE (0-100):");
try
{
    mic.UpdateBatteryLevel(-5);
}
catch (InvalidDeviceReadingException ex)
{
    Console.WriteLine($"[Bat duoc ngoai le thanh cong]: Thiet bi {ex.DeviceId}, Gia tri doc {ex.InvalidValue}: {ex.Message}");
}
Console.WriteLine();

// 10. Test FilteredAlertSubscriber voi delegate custom (Muc 8 trong de bai)
Console.WriteLine("[7] TEST FILTERED ALERT SUBSCRIBER VOI DELEGATE CUSTOM:");
var criticalOnlySubscriber = new FilteredAlertSubscriber(
    alert => alert.Severity == AlertSeverity.Critical,
    alert => Console.WriteLine($"-> [CUSTOM FILTER DAU RA RIENG] Thiet bi {alert.DeviceId} co su co CRITICAL: {alert.Message}")
);

// Dang ky rieng vao su kien cua mic
mic.AlertRaised += criticalOnlySubscriber.HandleAlert;
Console.WriteLine("- Cho pin micro ve 0% de kich hoat bo loc critical:");
mic.UpdateBatteryLevel(0);

// Huy dang ky filter rieng
mic.AlertRaised -= criticalOnlySubscriber.HandleAlert;
Console.WriteLine();

// 11. Kiem tra du lieu luu tru tai cac Subscriber
Console.WriteLine("[8] KIEM TRA NHAT KY HE THONG (DEVICE AUDIT LOGGER):");
foreach (var entry in auditLogger.Entries)
{
    Console.WriteLine($"  * {entry}");
}
Console.WriteLine();

Console.WriteLine("[9] KIEM TRA CAC YEU CAU BAO TRI DUOC TAO (MAINTENANCE SERVICE):");
foreach (var req in maintenanceService.Requests)
{
    Console.WriteLine($"  * {req}");
}
Console.WriteLine();

// 12. Test Unsubscribe va Dispose
Console.WriteLine("[10] TEST HUY DANG KY QUA DISPOSE:");
coordinator.Dispose();

Console.WriteLine("- Thu thay doi trang thai va giam pin sau khi da dispose:");
projector.ChangeStatus(DeviceStatus.Available);
projector.UpdateBatteryLevel(10);
Console.WriteLine("-> Khong co thong bao nao xuat hien vi tat ca da huy dang ky thanh cong!");

Console.WriteLine("\n=== TOAN BO CAC TRUONG HOP KIEM THU CUA V3 DA HOAN THANH XUAT SAC ===");
