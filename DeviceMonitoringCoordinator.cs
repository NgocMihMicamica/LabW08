using TeachingDeviceManagement.Devices;
using TeachingDeviceManagement.Subscribers;

namespace TeachingDeviceManagement.Coordinator;

// Lop dieu phoi dang ky va huy dang ky lang nghe su kien (v2: da bo sung huy maintenanceService)
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
        // Con sot o v2: van chua dung co chan dang ky trung lap!
        // Neu ai do vo tinh goi Subscribe() 2 lan thi su kien van bi ban 2 lan.
        foreach (var device in devices)
        {
            device.StatusChanged += auditLogger.HandleStatusChanged;
            device.AlertRaised += notificationService.HandleAlert;
            device.AlertRaised += maintenanceService.HandleCriticalAlert;
        }
    }

    public void Unsubscribe()
    {
        // Fix o v2: da huy du ca 3 subscriber
        foreach (var device in devices)
        {
            device.StatusChanged -= auditLogger.HandleStatusChanged;
            device.AlertRaised -= notificationService.HandleAlert;
            device.AlertRaised -= maintenanceService.HandleCriticalAlert;
        }
    }

    public void Dispose()
    {
        Unsubscribe();
    }
}
