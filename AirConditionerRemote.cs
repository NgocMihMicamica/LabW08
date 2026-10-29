using TeachingDeviceManagement.Enums;

namespace TeachingDeviceManagement.Devices;

// Thiet bi: Dieu khien dieu hoa
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
        IsResponsive = isResponsive;

        // Loi v1: Khong kiem tra trang thai cu, neu goi false lien tiep se spam phat sinh Critical lien tuc
        if (!isResponsive)
        {
            RaiseAlert($"Remote dieu hoa phong {AssignedRoom} bi mat phan hoi!", AlertSeverity.Critical);
        }
    }
}
