using TeachingDeviceManagement.Enums;
using TeachingDeviceManagement.Exceptions;

namespace TeachingDeviceManagement.Devices;

// Thiet bi: Dieu khien may chieu (v3: hoan thien kiem tra vung an toan, khong phat canh bao lap khi o cung phan vung)
public class ProjectorRemote : TeachingDevice
{
    public int BatteryLevel { get; private set; }

    // Bien luu trang thai canh bao truoc do de tranh spam canh bao o cung mot vung
    private AlertSeverity? lastAlertSeverity = null;

    public ProjectorRemote(string id, string name, int initialBattery = 100) 
        : base(id, name)
    {
        BatteryLevel = initialBattery;
    }

    public void UpdateBatteryLevel(int newLevel)
    {
        // 1. Kiem tra tinh hop le tu 0 den 100
        if (newLevel < 0 || newLevel > 100)
        {
            throw new InvalidDeviceReadingException(Id, newLevel, $"Muc pin {newLevel}% khong hop le cho thiet bi {Id}");
        }

        BatteryLevel = newLevel;

        // 2. Quy tac xu ly:
        // - Pin bang 0%: phat sinh Critical
        // - Pin duoi 20%: phat sinh Warning
        // - Khong phat sinh canh bao neu van o cung vung an toan/vung canh bao da thong bao truoc do
        if (newLevel == 0)
        {
            if (lastAlertSeverity != AlertSeverity.Critical)
            {
                lastAlertSeverity = AlertSeverity.Critical;
                RaiseAlert("Remote may chieu da can kiet pin hoan toan (0%)!", AlertSeverity.Critical);
            }
        }
        else if (newLevel < 20)
        {
            // Chi phat sinh canh bao Warning khi truoc do chua o vung Warning
            if (lastAlertSeverity != AlertSeverity.Warning)
            {
                lastAlertSeverity = AlertSeverity.Warning;
                RaiseAlert($"Pin remote may chieu xuong duoi 20%: con lai {newLevel}%", AlertSeverity.Warning);
            }
        }
        else
        {
            // Neu pin duoc sac lai tren 20% (vung an toan) thi reset lai muc canh bao
            lastAlertSeverity = null;
        }
    }
}
