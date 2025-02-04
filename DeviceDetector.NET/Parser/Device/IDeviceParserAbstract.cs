using DeviceDetectorNET.Results.Client;
using DeviceDetectorNET.Results.Device;

namespace DeviceDetectorNET.Parser.Device
{
    public interface IDeviceParserAbstract: IParserAbstract<DeviceMatchResult>
    {
        string GetModel();
        string GetBrand();
    }
}