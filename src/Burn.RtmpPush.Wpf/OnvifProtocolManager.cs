using Anub.Abp.ONVIF;
using Anub.Abp.ONVIF.Devices;
using Anub.Abp.ONVIF.Medias;
using OnvifSharp.Discovery;
using OnvifSharp.Discovery.Common;
using OnvifSharp.Discovery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace Burn.RtmpPush.Wpf
{
    public class OnvifProtocolManager
    {
        public async Task<List<DiscoveryDevice>> FindNetworkDevices(string ipAddress)
        {
            var discovery = new WSDiscovery();
            var devices = await discovery.Discover(5, new UdpClientWrapper(ipAddress, 9001));
            return devices.ToList();
        }

        public async Task<Tuple<string, GetDeviceInformationResponse>> GetDeviceInfoAsync(string deviceAddress)
        {
            var messageElement = new TextMessageEncodingBindingElement
            {
                MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None)
            };
            HttpTransportBindingElement httpBinding = new HttpTransportBindingElement
            {
                //AuthenticationScheme = AuthenticationSchemes.Digest
            };
            CustomBinding bind = new CustomBinding(messageElement, httpBinding);

            //绑定服务地址
            EndpointAddress serviceAddress = new EndpointAddress(deviceAddress);
            DeviceClient deviceClient = new DeviceClient(bind, serviceAddress);
            //给每个请求都添加认证信息
            deviceClient.Endpoint.EndpointBehaviors.Add(new CustomEndpointBehavior());

            //查看系统时间
            var date = await deviceClient.GetSystemDateAndTimeAsync();
            Console.WriteLine(date.UTCDateTime.Date.Month.ToString());

            var deviceInfo = await deviceClient.GetDeviceInformationAsync(new GetDeviceInformationRequest() { });

            //查看设备能力
            GetCapabilitiesResponse cap = await deviceClient.GetCapabilitiesAsync(new CapabilityCategory[] { CapabilityCategory.All });
            return Tuple.Create<string, GetDeviceInformationResponse>(cap.Capabilities.Media.XAddr.ToString(), deviceInfo);
        }

        public async Task<string> GetMediaInfoAsync(string mediaAddress)
        {
            var messageElement = new TextMessageEncodingBindingElement
            {
                MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None)
            };
            HttpTransportBindingElement httpBinding = new HttpTransportBindingElement
            {
                MaxReceivedMessageSize = 1024 * 1024 * 1024
            };
            CustomBinding bind = new CustomBinding(messageElement, httpBinding);

            //绑定服务地址
            EndpointAddress serviceAddress = new EndpointAddress(mediaAddress);
            MediaClient mediaClient = new MediaClient(bind, serviceAddress);

            //给每个请求都添加认证信息
            mediaClient.Endpoint.EndpointBehaviors.Add(new CustomEndpointBehavior());
            //不要少了下面这行，会报异常
            var channel = mediaClient.ChannelFactory.CreateChannel();
            //查看系统时间
            var profilesResponse = await mediaClient.GetProfilesAsync();
            foreach (var profile in profilesResponse.Profiles)
            {

            }

            var step = new StreamSetup
            {
                Transport = new Transport()
                {
                    Protocol = TransportProtocol.RTSP
                },
                Stream = StreamType.RTPUnicast
            };
            var streamUri = await mediaClient.GetStreamUriAsync(step, profilesResponse.Profiles[0].token);
            return streamUri.Uri;
        }
    }
}
