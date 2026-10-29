using TeachingDeviceManagement.Devices;
using TeachingDeviceManagement.Subscribers;

namespace TeachingDeviceManagement.Coordinator;

// Lop dieu phoi dang ky va huy dang ky lang nghe su kien
public class DeviceMonitoringCoordinator : IDisposable
{
    private readonly List<TeachingDevice> devices;
    private readonly ConsoleNotificationService notificationService;
    private readonly DeviceAuditLogger auditLogger;
    private readonly MaintenanceService maintenanceService;

    public DeviceMonitoringCoordinator(
        IEnumerable<TeachingDevice> devices,
        ConsoleNotificationService notificationService,
        DeviceAuditLogger auditLogger,
        MaintenanceService maintenanceService)
    {
        this.devices = devices.ToList();
        this.notificationService = notificationService;
        this.auditLogger = auditLogger;
        this.maintenanceService = maintenanceService;
    }

    public void Subscribe()
    {
        // Loi v1: Khong co co kiem tra da dang ky chua.
        // Neu goi Subscribe() nhieu lan se bi dang ky trung lap!
        foreach (var device in devices)
        {
            device.StatusChanged += auditLogger.HandleStatusChanged;
            device.AlertRaised += notificationService.HandleAlert;
            device.AlertRaised += maintenanceService.HandleCriticalAlert;
        }
    }

    public void Unsubscribe()
    {
        // Loi v1: Quen huy dang ky cho maintenanceService!
        foreach (var device in devices)
        {
            device.StatusChanged -= auditLogger.HandleStatusChanged;
            device.AlertRaised -= notificationService.HandleAlert;
            // TODO: can huy them cho maintenanceService nua
        }
    }

    public void Dispose()
    {
        Unsubscribe();
    }
}
