using System;
using System.IO;
using System.Windows;
using System.ComponentModel;

namespace WpfApp1
{
    public partial class AdvancedSettingsWindow : Window, INotifyPropertyChanged
    {
        private string _macAddress;
        private string _mtuValue;
        private string _hopCount;
        private string _udpPort;
        private string _tunDeviceName;

        // 构造函数
        public AdvancedSettingsWindow()
        {
            InitializeComponent();
            this.DataContext = this; // 设置DataContext为当前实例

            // 加载配置文件中的现有设置
            LoadSettings();
        }

        // 属性变更通知接口实现
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // 定义属性并关联到相应的文本框
        public string MacAddress
        {
            get => _macAddress;
            set
            {
                if (_macAddress != value)
                {
                    _macAddress = value;
                    OnPropertyChanged();
                }
            }
        }

        public string MtuValue
        {
            get => _mtuValue;
            set
            {
                if (_mtuValue != value)
                {
                    _mtuValue = value;
                    OnPropertyChanged();
                }
            }
        }

        public string HopCount
        {
            get => _hopCount;
            set
            {
                if (_hopCount != value)
                {
                    _hopCount = value;
                    OnPropertyChanged();
                }
            }
        }

        public string UdpPort
        {
            get => _udpPort;
            set
            {
                if (_udpPort != value)
                {
                    _udpPort = value;
                    OnPropertyChanged();
                }
            }
        }

        public string TunDeviceName
        {
            get => _tunDeviceName;
            set
            {
                if (_tunDeviceName != value)
                {
                    _tunDeviceName = value;
                    OnPropertyChanged();
                }
            }
        }

        // 加载设置的方法
        private void LoadSettings()
        {
            MacAddress = ConfigManager.GetSetting("Advanced", "MacAddress");
            MtuValue = ConfigManager.GetSetting("Advanced", "MtuValue");
            HopCount = ConfigManager.GetSetting("Advanced", "HopCount");
            UdpPort = ConfigManager.GetSetting("Advanced", "UdpPort");
            TunDeviceName = ConfigManager.GetSetting("Advanced", "TunDeviceName");
        }

        // 保存设置的方法
        private void SaveSettings()
        {
            ConfigManager.SetSetting("Advanced", "MacAddress", MacAddress);
            ConfigManager.SetSetting("Advanced", "MtuValue", MtuValue);
            ConfigManager.SetSetting("Advanced", "HopCount", HopCount);
            ConfigManager.SetSetting("Advanced", "UdpPort", UdpPort);
            ConfigManager.SetSetting("Advanced", "TunDeviceName", TunDeviceName);
        }

        // TextBox失去焦点时自动保存设置
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            SaveSettings();
        }

        // OkCommand执行方法
        private void OKbutton_click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
            this.DialogResult = true;
        }

        // CancelCommand执行方法
        private void Cancelbutton_click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}