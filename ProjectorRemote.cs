using TeachingDeviceManagement.Enums;
using TeachingDeviceManagement.Exceptions;

namespace TeachingDeviceManagement.Devices;

// Thiet bi: Dieu khien may chieu (v2: da them validate va bat Critical khi 0%)
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
        // Fix o v2: da kiem tra khoang 0 - 100
        if (newLevel < 0 || newLevel > 100)
        {
            throw new InvalidDeviceReadingException(Id, newLevel, $"Muc pin {newLevel}% khong hop le cho thiet bi {Id}");
        }

        BatteryLevel = newLevel;

        // Fix o v2: neu pin bang 0 thi bao Critical truoc
        if (newLevel == 0)
        {
            RaiseAlert("Remote may chieu da het sach pin (0%)!", AlertSeverity.Critical);
        }
        else if (newLevel < 20)
        {
            // Con sot o v2: neu lien tuc update trong vung < 20 (vi du 15% xuong 10%) van bi bao lap lai
            RaiseAlert($"Pin remote may chieu dang thap: {newLevel}%", AlertSeverity.Warning);
        }
    }
}
