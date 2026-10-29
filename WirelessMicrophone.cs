using TeachingDeviceManagement.Enums;
using TeachingDeviceManagement.Exceptions;

namespace TeachingDeviceManagement.Devices;

// Thiet bi: Micro khong day (v2: da them validate va doi thu tu uu tien canh bao Critical)
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
        // Fix o v2: validate gia tri 0 - 100
        if (newLevel < 0 || newLevel > 100)
        {
            throw new InvalidDeviceReadingException(Id, newLevel, $"Muc pin micro {newLevel}% khong hop le");
        }

        BatteryLevel = newLevel;

        // Fix o v2: Kiem tra muc 0 truoc de phat Critical, sau do moi den muc duoi 30 cho Warning
        if (newLevel == 0)
        {
            RaiseAlert("Micro het sach pin (0%)!", AlertSeverity.Critical);
        }
        else if (newLevel < 30)
        {
            RaiseAlert($"Micro pin yeu: {newLevel}%", AlertSeverity.Warning);
        }
    }

    public void UpdateSignalQuality(int newQuality)
    {
        // Fix o v2: validate gia tri 0 - 100
        if (newQuality < 0 || newQuality > 100)
        {
            throw new InvalidDeviceReadingException(Id, newQuality, $"Chat luong tin hieu micro {newQuality}% khong hop le");
        }

        SignalQuality = newQuality;

        // Fix o v2: Kiem tra tin hieu = 0 truoc de phat Critical
        if (newQuality == 0)
        {
            RaiseAlert("Micro mat hoan toan tin hieu (0%)!", AlertSeverity.Critical);
        }
        else if (newQuality < 40)
        {
            RaiseAlert($"Chat luong tin hieu micro thap: {newQuality}%", AlertSeverity.Warning);
        }
    }
}
