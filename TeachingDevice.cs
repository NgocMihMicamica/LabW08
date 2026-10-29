using TeachingDeviceManagement.Enums;
using TeachingDeviceManagement.Events;

namespace TeachingDeviceManagement.Devices;

// Lop cha truu tuong cho tat ca thiet bi phong hoc
public abstract class TeachingDevice
{
    public string Id { get; }
    public string Name { get; }
    public DeviceStatus Status { get; private set; }

    // Cac event cho publisher
    public event EventHandler<DeviceStatusChangedEventArgs>? StatusChanged;
    public event EventHandler<DeviceAlertEventArgs>? AlertRaised;

    protected TeachingDevice(string id, string name)
    {
        Id = id;
        Name = name;
        Status = DeviceStatus.Available;
    }

    public void ChangeStatus(DeviceStatus newStatus)
    {
        // Khong phat sinh event neu trang thai moi trung trang thai cu
        if (newStatus == Status)
        {
            return;
        }

        DeviceStatus oldStatus = Status;
        Status = newStatus;

        OnStatusChanged(new DeviceStatusChangedEventArgs(
            Id,
            oldStatus,
            newStatus,
            DateTime.Now));
    }

    protected void RaiseAlert(string message, AlertSeverity severity)
    {
        OnAlertRaised(new DeviceAlertEventArgs(
            Id,
            message,
            severity,
            DateTime.Now));
    }

    protected virtual void OnStatusChanged(DeviceStatusChangedEventArgs e)
    {
        StatusChanged?.Invoke(this, e);
    }

    protected virtual void OnAlertRaised(DeviceAlertEventArgs e)
    {
        AlertRaised?.Invoke(this, e);
    }
}
