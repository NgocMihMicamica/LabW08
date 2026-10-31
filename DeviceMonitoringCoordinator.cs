using TeachingDeviceManagement.Devices;
using TeachingDeviceManagement.Subscribers;

namespace TeachingDeviceManagement.Coordinator;

// Lop dieu phoi dang ky va huy dang ky lang nghe su kien (v3: hoan thien kiem tra da subscribe chua)
public class DeviceMonitoringCoordinator : IDisposable
{
    private readonly List<TeachingDevice> devices;
    private readonly ConsoleNotificationService notificationService;
    private readonly DeviceAuditLogger auditLogger;
    private readonly MaintenanceService maintenanceService;

    // Co danh dau trang thai dang ky de tranh dang ky trung lap
    private bool isSubscribed = false;

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
        // Fix hoan thien o v3: neu da dang ky roi thi khong dang ky lai nua
        if (isSubscribed)
        {
            return;
        }

        foreach (var device in devices)
        {
            device.StatusChanged += auditLogger.HandleStatusChanged;
            device.AlertRaised += notificationService.HandleAlert;
            device.AlertRaised += maintenanceService.HandleCriticalAlert;
        }

        isSubscribed = true;
    }

    public void Unsubscribe()
    {
        // Neu chua dang ky thi khong can huy
        if (!isSubscribed)
        {
            return;
        }

        foreach (var device in devices)
        {
            device.StatusChanged -= auditLogger.HandleStatusChanged;
            device.AlertRaised -= notificationService.HandleAlert;
            device.AlertRaised -= maintenanceService.HandleCriticalAlert;
        }

        isSubscribed = false;
    }

    public void Dispose()
    {
        Unsubscribe();
    }
}
