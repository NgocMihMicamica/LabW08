using TeachingDeviceManagement.Events;

namespace TeachingDeviceManagement.Subscribers;

// Subscriber hien thi thong bao len Console
public class ConsoleNotificationService
{
    public void HandleAlert(object? sender, DeviceAlertEventArgs e)
    {
        Console.WriteLine($"[{e.Severity}] {e.DeviceId}: {e.Message}");
    }
}
