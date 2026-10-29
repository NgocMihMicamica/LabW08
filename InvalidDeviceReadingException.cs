namespace TeachingDeviceManagement.Exceptions;

// Ngoai le phat sinh khi gia tri doc (pin/tin hieu) vuot khoang 0 - 100
public class InvalidDeviceReadingException : Exception
{
    public string DeviceId { get; }
    public int InvalidValue { get; }

    public InvalidDeviceReadingException(string deviceId, int invalidValue, string message = "Gia tri doc pin hoac tin hieu nam ngoai khoang hop le 0-100") 
        : base(message)
    {
        DeviceId = deviceId;
        InvalidValue = invalidValue;
    }
}
