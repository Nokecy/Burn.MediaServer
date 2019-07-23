using Burn.RtmpPush.Wpf.Models;
using Burn.RtmpPush.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Enums;

namespace Burn.RtmpPush.Wpf
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<string, CancellationTokenSource> TokenDictionary;
        public MainWindow()
        {
            InitializeComponent();
            TokenDictionary = new Dictionary<string, CancellationTokenSource>();
            //FFmpeg.ExecutablesPath = @"C:\Program Files\ffmpeg\bin";
        }

        protected override async void OnInitialized(EventArgs e)
        {
            await DataBindingListAsync();

            //await DataBindingAsync();

            base.OnInitialized(e);
        }

        private void StartPush_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as MainWindowViewModel;
            foreach (var item in viewModel.DeviceModels.Where(a => a.HardwareId != "255"))
            {
                var tokenSource = new CancellationTokenSource();
                string arguments = $"-i \"{item.RtspAddr}\"  -vcodec copy -acodec aac -f flv \"{viewModel.PushUrl}/BurnPush/{item.Channel}\"";
                var conver = Conver().Start(arguments, tokenSource.Token);
                TokenDictionary.Add(item.HardwareId, tokenSource);
            }
            viewModel.IsStart = true;
        }

        private void StopPush_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as MainWindowViewModel;

            StopPushStream();

            viewModel.IsStart = false;
        }

        private async Task DataBindingAsync(string ipAddress)
        {
            var viewModel = DataContext as MainWindowViewModel;
            await this.Dispatcher.BeginInvoke(new Action(async () =>
             {
                 viewModel.IsScan = true;
                 //编写获取数据并显示在界面的代码
                 var findDevices = new List<DeviceModel>();
                 var onvifManager = new OnvifProtocolManager();
                 var discoveryDevice = await onvifManager.FindNetworkDevices(ipAddress);
                 int i = 1;
                 var dataContext = DataContext as MainWindowViewModel;
                 dataContext.DeviceModels.Clear();
                 foreach (var device in discoveryDevice)
                 {
                     var mediaAddress = await onvifManager.GetDeviceInfoAsync(device.XAdresses.ToList()[0], viewModel.UserName, viewModel.PassWord);
                     var rtspUri = await onvifManager.GetMediaInfoAsync(mediaAddress.Item1, viewModel.UserName, viewModel.PassWord);
                     var deviceinfo = mediaAddress.Item2;
                     dataContext.DeviceModels.Add(new DeviceModel(deviceinfo.Manufacturer, deviceinfo.Model, deviceinfo.FirmwareVersion, deviceinfo.SerialNumber, deviceinfo.HardwareId, mediaAddress.Item1, rtspUri, $"Channel{i}"));
                     i++;
                 }
                 viewModel.IsScan = false;
             }));
        }

        private async Task DataBindingListAsync()
        {
            var viewModel = DataContext as MainWindowViewModel;
            await this.Dispatcher.BeginInvoke(new Action(() =>
            {
                string HostName = Dns.GetHostName(); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int x = 0; x < IpEntry.AddressList.Length; x++)
                {
                    if (IpEntry.AddressList[x].AddressFamily == AddressFamily.InterNetwork)
                    {
                        var dataContext = DataContext as MainWindowViewModel;
                        dataContext.AddressList.Add(IpEntry.AddressList[x].ToString());
                    }
                }
            }));
        }

        private IConversion Conver()
        {
            return new Conversion()
                                            .UseShortest(true)
                                            .UseHardwareAcceleration(HardwareAccelerator.Auto, VideoCodec.H264, VideoCodec.H264)
                                            .UseMultiThread(true);
            //.AddParameter($"-i \"{inputfile}\"")
            //.AddParameter($"-vcodec copy -acodec aac -f flv")
            //.SetOutput(outputPath);
        }

        public void StopPushStream()
        {
            foreach (var item in TokenDictionary)
            {
                item.Value.Cancel();
            }
        }

        private async void ScanBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AddressComboBox.SelectedValue != null)
            {
                await DataBindingAsync(AddressComboBox.SelectedValue as string);
            }
        }
    }
}
