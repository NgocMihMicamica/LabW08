using TeachingDeviceManagement.Enums;

namespace TeachingDeviceManagement.Devices;

// Thiet bi: Micro khong day
public class WirelessMicrophone : TeachingDevice
{
    public int BatteryLevel { get; private set; }
    public int SignalQuality { get; private set; }

    public WirelessMicrophone(string id, string name, int initialBattery = 100, int initialSignal = 100) 
        : base(id, name)
    {
        BatteryLevel = initialBattery;
        SignalQuality = initialSignal;
    }

    public void UpdateBatteryLevel(int newLevel)
    {
        // TODO: can validate 0 - 100
        BatteryLevel = newLevel;

        // Loi v1: Kiem tra < 30 truoc, nen khi pin = 0 se bi roi vao Warning thay vi Critical
        if (newLevel < 30)
        {
            RaiseAlert($"Micro pin yeu: {newLevel}%", AlertSeverity.Warning);
        }
        else if (newLevel == 0)
        {
            RaiseAlert("Micro het sach pin!", AlertSeverity.Critical);
        }
    }

    public void UpdateSignalQuality(int newQuality)
    {
        // TODO: can validate 0 - 100
        SignalQuality = newQuality;

        // Loi v1: Tuong tu, tin hieu = 0 bi bat vao Warning truoc
        if (newQuality < 40)
        {
            RaiseAlert($"Tin hieu micro chap chon: {newQuality}%", AlertSeverity.Warning);
        }
        else if (newQuality == 0)
        {
            RaiseAlert("Micro mat hoan toan tin hieu!", AlertSeverity.Critical);
        }
    }
}
