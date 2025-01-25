using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private bool isServiceRunning = false; // 用于跟踪服务状态
        private Process n2nProcess; // 用于跟踪N2N服务进程
        private StringBuilder logContent = new StringBuilder(); // 用于存储日志内容

        public MainWindow()
        {
            InitializeComponent();
            LoadConfig(); // 加载配置
            UpdateButtonStates();
        }
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 检查服务器地址和端口号是否为空
                if (string.IsNullOrWhiteSpace(txtServerAddress.Text) || string.IsNullOrWhiteSpace(txtPortNumber.Text))
                {
                    MessageBox.Show("服务器地址和端口禁止为空", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return; // 不继续执行
                }

                StartN2NService();

                // 更新服务运行状态
                isServiceRunning = true;
                // 更新按钮状态
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"启动失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StopN2NService();

                // 更新服务运行状态
                isServiceRunning = false;
                // 更新按钮状态
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"停止失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void OpenTutorialWindow(object sender, RoutedEventArgs e)
        {
            TutorialWindow tutorialWindow = new TutorialWindow();
            tutorialWindow.Show();
        }

        private void OpenCheckTapDriverWindow(object sender, RoutedEventArgs e)
        {
            var checkTapDriverWindow = new CheckTapDriverWindow
            {
                Owner = this // 设置主窗口为所有者，以便正确居中显示
            };
            checkTapDriverWindow.ShowDialog();
        }
        private void BtnAdvancedSettings_Click(object sender, RoutedEventArgs e)
        {
            var advancedSettingsWindow = new AdvancedSettingsWindow
            {
                Owner = this // 设置主窗口为所有者，以便正确居中显示
            };

            // 打开窗口，并根据对话结果处理用户输入
            if (advancedSettingsWindow.ShowDialog() == true)
            {
                // 用户点击了“确定”，在此处处理保存设置的逻辑
                SaveConfig();
            }
            else
            {
                // 用户点击了“取消”或关闭了窗口，不做任何处理
            }
        }

        private void btnViewN2NLogs_Click(object sender, RoutedEventArgs e)
        {
            // 创建并显示日志查看窗口
            var logViewer = new LogViewerWindow
            {
                Owner = this, // 设置主窗口为所有者，以便正确居中显示
                DataContext = new { LogContent = logContent.ToString() } // 将日志内容传递给新窗口
            };

            logViewer.Show();
        }

        private void StartN2NService()
        {
            if (n2nProcess != null && !n2nProcess.HasExited)
            {
                throw new InvalidOperationException("N2N服务已经在运行。");
            }

            // 从配置文件中读取参数
            string nodeName = ConfigManager.GetSetting("Main", "NodeName");
            string communityName = ConfigManager.GetSetting("Main", "CommunityName");
            string password = ConfigManager.GetSetting("Main", "Password");
            string serverAddress = ConfigManager.GetSetting("Main", "ServerAddress");
            string portNumber = ConfigManager.GetSetting("Main", "PortNumber");
            string macAddress = ConfigManager.GetSetting("Advanced", "MacAddress");
            string mtuValue = ConfigManager.GetSetting("Advanced", "MtuValue");
            string hopCount = ConfigManager.GetSetting("Advanced", "HopCount");
            string udpPort = ConfigManager.GetSetting("Advanced", "UdpPort");
            string tunDeviceName = ConfigManager.GetSetting("Advanced", "TunDeviceName");

            // 构建命令行参数
            string arguments = $"-a {nodeName} -k {communityName}";

            // MAC地址
            if (!string.IsNullOrEmpty(macAddress))
            {
                arguments += $" -m {macAddress}";
            }

            // UDP端口
            if (!string.IsNullOrEmpty(portNumber))
            {
                arguments += $" -p {portNumber}"; 
            }

            // MTU 值 此处参数 -M 与 MAC地址 -m区分
            if (!string.IsNullOrEmpty(mtuValue))
            {
                arguments += $" -M {mtuValue}";
            }

            // 网卡设备名称
            if (!string.IsNullOrEmpty(tunDeviceName))
            {
                arguments += $" -d {tunDeviceName}";
            }
            else
            {
                arguments += $" -d tap0";
            }


            // 如果需要密码，可以添加到参数中
            if (!string.IsNullOrEmpty(password))
            {
                arguments += $" -k {password}";
            }

            // 如果有服务器地址和端口，也添加到参数中
            if (!string.IsNullOrEmpty(serverAddress) && int.TryParse(portNumber, out int port))
            {
                arguments += $" -l {serverAddress}:{port}";
            }

            // 创建启动信息
            var startInfo = new ProcessStartInfo
            {
                FileName = @"n2n_client\n2n_v3\edge.exe",
                Arguments = arguments,
                WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            // 启动进程
            n2nProcess = new Process { StartInfo = startInfo };
            n2nProcess.Start();

            // 开始异步读取输出和错误流，并将其附加到日志内容
            Task.Run(() => ReadOutputAsync(n2nProcess.StandardOutput));
            Task.Run(() => ReadOutputAsync(n2nProcess.StandardError));
        }

        private async Task ReadOutputAsync(TextReader reader)
        {
            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                // 在UI线程上更新日志内容
                Application.Current.Dispatcher.Invoke(() =>
                {
                    logContent.AppendLine(line);
                });
            }
        }

        private void StopN2NService()
        {
            if (n2nProcess == null || n2nProcess.HasExited)
            {
                // 直接更新按钮状态
                btnStop.IsEnabled = false;
                btnStart.IsEnabled = true;
                MessageBox.Show("N2N服务未运行。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // 尝试优雅地关闭进程
                n2nProcess.CloseMainWindow();

                // 等待进程退出，如果超时则强制终止
                if (!n2nProcess.WaitForExit(3000))
                {
                    n2nProcess.Kill();
                }

                n2nProcess.Dispose();
                n2nProcess = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"无法停止N2N服务: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateButtonStates()
        {
            // 根据服务状态更新按钮的启用状态
            btnStart.IsEnabled = !isServiceRunning;
            btnStop.IsEnabled = isServiceRunning;
        }

        private void SaveConfig()
        {
            ConfigManager.SetSetting("Main", "NodeName", txtNodeName.Text);
            ConfigManager.SetSetting("Main", "CommunityName", txtCommunityName.Text);
            ConfigManager.SetSetting("Main", "Password", txtPassword.Password); // 注意：存储密码可能不安全
            ConfigManager.SetSetting("Main", "ServerAddress", txtServerAddress.Text);
            ConfigManager.SetSetting("Main", "PortNumber", txtPortNumber.Text);
        }

        private void LoadConfig()
        {
            // 从配置文件加载配置项，但不更新UI，因为我们不再依赖于UI输入来启动服务
            // 如果您希望在窗口加载时显示当前配置，您可以选择保留这部分代码。
            txtNodeName.Text = ConfigManager.GetSetting("Main", "NodeName");
            txtCommunityName.Text = ConfigManager.GetSetting("Main", "CommunityName");
            txtPassword.Password = ConfigManager.GetSetting("Main", "Password");
            txtServerAddress.Text = ConfigManager.GetSetting("Main", "ServerAddress");
            txtPortNumber.Text = ConfigManager.GetSetting("Main", "PortNumber");
        }

        private void InputField_LostFocus(object sender, RoutedEventArgs e)
        {
            SaveConfig();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            SaveConfig();
        }
    }
}



