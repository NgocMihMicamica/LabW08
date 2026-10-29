using TeachingDeviceManagement.Enums;

namespace TeachingDeviceManagement.Devices;

// Thiet bi: Dieu khien dieu hoa (v2: da chan bao lap khi van dang mat phan hoi)
public class AirConditionerRemote : TeachingDevice
{
    public string AssignedRoom { get; }
    public bool IsResponsive { get; private set; }

    public AirConditionerRemote(string id, string name, string assignedRoom, bool isResponsive = true) 
        : base(id, name)
    {
        AssignedRoom = assignedRoom;
        IsResponsive = isResponsive;
    }

    public void UpdateResponseState(bool isResponsive)
    {
        // Fix o v2: Neu trang thai phan hoi khong doi thi bo qua, khong phat sinh canh bao trung
        if (IsResponsive == isResponsive)
        {
            return;
        }

        IsResponsive = isResponsive;

        // Chi phat sinh canh bao Critical khi chuyen tu phan hoi sang khong phan hoi
        if (!isResponsive)
        {
            RaiseAlert($"Remote dieu hoa phong {AssignedRoom} bi mat phan hoi!", AlertSeverity.Critical);
        }
    }
}
