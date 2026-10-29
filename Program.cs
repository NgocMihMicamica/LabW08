using TeachingDeviceManagement.Coordinator;
using TeachingDeviceManagement.Devices;
using TeachingDeviceManagement.Enums;
using TeachingDeviceManagement.Subscribers;

Console.WriteLine("=== CHUONG TRINH QUAN LY THIET BI PHONG HOC (PHIEN BAN V1 - DRAFT) ===");

var projector = new ProjectorRemote("PRJ-01", "Dieu khien may chieu A201");
var airCon = new AirConditionerRemote("AC-01", "Dieu hoa phong A201", "A201");
var mic = new WirelessMicrophone("MIC-01", "Micro khong day A201");

var devices = new List<TeachingDevice> { projector, airCon, mic };

var notifyService = new ConsoleNotificationService();
var auditLogger = new DeviceAuditLogger();
var maintenance = new MaintenanceService();

var coordinator = new DeviceMonitoringCoordinator(devices, notifyService, auditLogger, maintenance);

// 1. Thu dang ky su kien (v1 loi: neu goi 2 lan bi double su kien)
coordinator.Subscribe();

Console.WriteLine("\n--- TEST 1: THAY DOI TRANG THAI THIET BI ---");
projector.ChangeStatus(DeviceStatus.Borrowed);
projector.ChangeStatus(DeviceStatus.UnderMaintenance);

// Khong doi neu trung trang thai cu
projector.ChangeStatus(DeviceStatus.UnderMaintenance);

Console.WriteLine("\n--- TEST 2: CAP NHAT PIN MAY CHIEU ---");
// Pin xuong 15% -> bao Warning
projector.UpdateBatteryLevel(15);
// Pin xuong 0% -> v1 bi loi van bao Warning chu chua bao Critical!
projector.UpdateBatteryLevel(0);

Console.WriteLine("\n--- TEST 3: CAP NHAT KET NOI DIEU HOA ---");
airCon.UpdateResponseState(false);
// v1 bi loi: goi tiep false thi van phat sinh Critical tiep
airCon.UpdateResponseState(false);

Console.WriteLine("\n--- TEST 4: MICRO PIN VA TIN HIEU = 0 ---");
// v1 bi loi: 0% roi vao < 30 nen chi bao Warning
mic.UpdateBatteryLevel(0);

Console.WriteLine("\n--- KET QUA GHI LOG AUDIT ---");
foreach (var log in auditLogger.Entries)
{
    Console.WriteLine(log);
}

Console.WriteLine("\n--- KET QUA YEU CAU BAO TRI ---");
foreach (var req in maintenance.Requests)
{
    Console.WriteLine(req);
}

coordinator.Dispose();
Console.WriteLine("\n[v1 ket thuc: con mot so loi logic & chua validate hop le]");
