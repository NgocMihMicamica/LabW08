using TeachingDeviceManagement.Enums;

namespace TeachingDeviceManagement.Devices;

// Thiet bi: Dieu khien may chieu
public class ProjectorRemote : TeachingDevice
{
    public int BatteryLevel { get; private set; }

    public ProjectorRemote(string id, string name, int initialBattery = 100) 
        : base(id, name)
    {
        BatteryLevel = initialBattery;
    }

    public void UpdateBatteryLevel(int newLevel)
    {
        // TODO: can xu ly kiem tra newLevel hop le trong khoang 0 - 100
        BatteryLevel = newLevel;

        // Loi v1: chua xu ly muc pin 0% phai la Critical, dung <= 20 gay bao Warning ca khi 0%
        if (newLevel <= 20)
        {
            RaiseAlert($"Pin remote may chieu dang thap: {newLevel}%", AlertSeverity.Warning);
        }
    }
}
