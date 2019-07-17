using System.Collections.Generic;
using System.Net;

namespace Burn.RtmpPush.Wpf.Models
{
    public class DeviceModel
    {
        public DeviceModel() { }
        public DeviceModel(string manufacturer, string model, string firmwareVersion, string serialNumber, string hardwareId, string mediaXAddr, string rtspAddr, string channel)
        {
            Manufacturer = manufacturer;
            Model = model;
            FirmwareVersion = firmwareVersion;
            SerialNumber = serialNumber;
            HardwareId = hardwareId;
            MediaXAddr = mediaXAddr;
            RtspAddr = rtspAddr;
            Channel = channel;
        }

        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string FirmwareVersion { get; set; }
        public string SerialNumber { get; set; }
        public string HardwareId { get; set; }
        public string MediaXAddr { get; set; }
        public string RtspAddr { get; set; }
        public string Channel { get; set; }
    }
}
