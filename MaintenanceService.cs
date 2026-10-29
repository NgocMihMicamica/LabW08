using TeachingDeviceManagement.Enums;
using TeachingDeviceManagement.Events;

namespace TeachingDeviceManagement.Subscribers;

// Subscriber tiep nhan yeu cau bao tri khi co canh bao Critical
public class MaintenanceService
{
    private readonly List<string> requests = new();
    public IReadOnlyList<string> Requests => requests.AsReadOnly();

    public void HandleCriticalAlert(object? sender, DeviceAlertEventArgs e)
    {
        if (e.Severity != AlertSeverity.Critical)
        {
            return;
        }

        requests.Add($"Maintenance request for {e.DeviceId}: {e.Message}");
    }
}
