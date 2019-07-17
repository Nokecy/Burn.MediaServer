using Burn.RtmpPush.Wpf.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Burn.RtmpPush.Wpf.UserControls
{
    /// <summary>
    /// OnvifDeviceInfo.xaml 的交互逻辑
    /// </summary>
    public partial class OnvifDeviceInfo : UserControl
    {
        public static readonly DependencyProperty DeviceNameProperty = DependencyProperty.Register("OnvifName", typeof(string), typeof(OnvifDeviceInfo), new UIPropertyMetadata("", DeviceNameChange));
        public static readonly DependencyProperty DeviceMediaAddressProperty = DependencyProperty.Register("OnvifMediaAddress", typeof(string), typeof(OnvifDeviceInfo), new UIPropertyMetadata("", DeviceMediaAddressChange));
        public static readonly DependencyProperty DeviceRtspAddressProperty = DependencyProperty.Register("OnvifRtspAddress", typeof(string), typeof(OnvifDeviceInfo), new UIPropertyMetadata("", DeviceRtspAddressChange));
        public static readonly DependencyProperty DeviceChannelProperty = DependencyProperty.Register("OnvifChannel", typeof(string), typeof(OnvifDeviceInfo), new UIPropertyMetadata("", DeviceChannelChange));
        public string OnvifName
        {
            get { return (string)GetValue(DeviceNameProperty); }
            set { SetValue(DeviceNameProperty, value); }
        }
        public string OnvifMediaAddress
        {
            get { return (string)GetValue(DeviceMediaAddressProperty); }
            set { SetValue(DeviceMediaAddressProperty, value); }
        }
        public string OnvifRtspAddress
        {
            get { return (string)GetValue(DeviceRtspAddressProperty); }
            set { SetValue(DeviceRtspAddressProperty, value); }
        }
        public string OnvifChannel
        {
            get { return (string)GetValue(DeviceChannelProperty); }
            set { SetValue(DeviceChannelProperty, value); }
        }

        private static void DeviceNameChange(DependencyObject obj, DependencyPropertyChangedEventArgs r)
        {
            OnvifDeviceInfo onVifDeviceInfo = (OnvifDeviceInfo)obj;
            Label deviceName = onVifDeviceInfo.DeviceName;
            deviceName.Content = r.NewValue;
        }
        private static void DeviceMediaAddressChange(DependencyObject obj, DependencyPropertyChangedEventArgs r)
        {
            OnvifDeviceInfo onVifDeviceInfo = (OnvifDeviceInfo)obj;
            Label deviceMediaAddress = onVifDeviceInfo.DeviceMediaAddress;
            deviceMediaAddress.Content = r.NewValue;
        }
        private static void DeviceRtspAddressChange(DependencyObject obj, DependencyPropertyChangedEventArgs r)
        {
            OnvifDeviceInfo onVifDeviceInfo = (OnvifDeviceInfo)obj;
            Label deviceRtspAddress = onVifDeviceInfo.DeviceRtspAddress;
            deviceRtspAddress.Content = r.NewValue;
        }
        private static void DeviceChannelChange(DependencyObject obj, DependencyPropertyChangedEventArgs r)
        {
            OnvifDeviceInfo onVifDeviceInfo = (OnvifDeviceInfo)obj;
            Label deviceRtspAddress = onVifDeviceInfo.DeviceChannel;
            deviceRtspAddress.Content = r.NewValue;
        }

        public OnvifDeviceInfo()
        {
            InitializeComponent();
        }
    }
}
