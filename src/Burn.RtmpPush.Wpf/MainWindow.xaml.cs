using Burn.RtmpPush.Wpf.Models;
using Burn.RtmpPush.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Xabe.FFmpeg;

namespace Burn.RtmpPush.Wpf
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private ConversionQueue Queue;

        public MainWindow()
        {
            InitializeComponent();
            FFmpeg.ExecutablesPath = @"C:\Program Files\ffmpeg\bin";
        }

        protected override async void OnInitialized(EventArgs e)
        {
            await DataBindingAsync();

            base.OnInitialized(e);
        }

        private void StartPush_Click(object sender, RoutedEventArgs e)
        {
            Queue = new ConversionQueue(parallel: false);
            var viewModel = DataContext as MainWindowViewModel;
            foreach (var item in viewModel.DeviceModels)
            {
                Queue.Add(Conver(item.RtspAddr, $"{viewModel.PushUrl}/{item.Channel}"));
            }
            Queue.OnException += Queue_OnException;
            Queue.Start();
            viewModel.IsStart = true;
        }

        private void Queue_OnException(int currentItemNumber, int totalItemsCount, IConversion conversion)
        {
            Console.WriteLine("Exception");
        }

        private void StopPush_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as MainWindowViewModel;
            Queue.Dispose();
            viewModel.IsStart = false;
        }

        private async Task DataBindingAsync()
        {
            var viewModel = DataContext as MainWindowViewModel;
            await this.Dispatcher.BeginInvoke(new Action(async () =>
             {
                 //编写获取数据并显示在界面的代码
                 var findDevices = new List<DeviceModel>();
                 var onvifManager = new OnvifProtocolManager();
                 var discoveryDevice = await onvifManager.FindNetworkDevices("192.168.1.116");
                 int i = 1;
                 foreach (var device in discoveryDevice)
                 {
                     var mediaAddress = await onvifManager.GetDeviceInfoAsync(device.XAdresses.ToList()[0], viewModel.UserName, viewModel.PassWord);
                     var rtspUri = await onvifManager.GetMediaInfoAsync(mediaAddress.Item1, viewModel.UserName, viewModel.PassWord);
                     var deviceinfo = mediaAddress.Item2;
                     var dataContext = DataContext as MainWindowViewModel;
                     dataContext.DeviceModels.Add(new DeviceModel(deviceinfo.Manufacturer, deviceinfo.Model, deviceinfo.FirmwareVersion, deviceinfo.SerialNumber, deviceinfo.HardwareId, mediaAddress.Item1, rtspUri, $"Channel{i}"));
                     i++;
                 }
             }));
        }

        private IConversion Conver(string inputfile, string outputPath)
        {
            string arguments = $"-i \"{inputfile}\"  -vcodec copy -acodec aac -f flv \"{outputPath}\"";
            return new Conversion()
                .UseMultiThread(true)
                                                        //.UseShortest(true)
                                                        .AddParameter($"-i \"{inputfile}\"")
                                                        .AddParameter($"-vcodec copy -acodec aac -f flv")
                                                        .SetOutput(outputPath);
        }
    }
}
