using Burn.RtmpPush.Wpf.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace Burn.RtmpPush.Wpf.ViewModels
{
    public class NegateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool ? !(bool)value : value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _UserName;
        private string _PassWord;
        private string _PushUrl;
        private bool _IsStart;
        public ObservableCollection<DeviceModel> DeviceModels { get; set; }
        public string UserName { get { return _UserName; } set { this._UserName = value; RaisePropertyChanged("UserName"); } }
        public string PassWord { get { return _PassWord; } set { this._PassWord = value; RaisePropertyChanged("PassWord"); } }
        public string PushUrl { get { return _PushUrl; } set { this._PushUrl = value; RaisePropertyChanged("PushUrl"); } }
        public bool IsStart { get { return _IsStart; } set { this._IsStart = value; RaisePropertyChanged("IsStart"); } }

        public MainWindowViewModel()
        {
            DeviceModels = new ObservableCollection<DeviceModel>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
