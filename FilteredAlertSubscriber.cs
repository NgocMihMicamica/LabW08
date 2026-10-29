using TeachingDeviceManagement.Events;

namespace TeachingDeviceManagement.Subscribers;

// Delegate loc canh bao theo dieu kien
public delegate bool AlertFilter(DeviceAlertEventArgs alert);

// Subscriber dung delegate lam filter va callback action
public class FilteredAlertSubscriber
{
    private readonly AlertFilter filter;
    private readonly Action<DeviceAlertEventArgs> action;

    public FilteredAlertSubscriber(
        AlertFilter filter,
        Action<DeviceAlertEventArgs> action)
    {
        this.filter = filter;
        this.action = action;
    }

    public void HandleAlert(object? sender, DeviceAlertEventArgs e)
    {
        if (filter(e))
        {
            action(e);
        }
    }
}
