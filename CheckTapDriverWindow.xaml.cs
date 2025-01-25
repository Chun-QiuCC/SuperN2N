using System;
using System.Diagnostics;
using System.Windows;

namespace WpfApp1
{
    public partial class CheckTapDriverWindow : Window
    {
        public CheckTapDriverWindow()
        {
            InitializeComponent();
            CheckTapDriver();
        }

        private void CheckTapDriver()
        {
            // 检查系统中是否存在TAP驱动程序
            bool tapInstalled = IsTapDriverInstalled();

            if (tapInstalled)
            {
                txtMessage.Text = "已检测到TAP驱动。";
                btnInstall.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtMessage.Text = "未检测到TAP驱动，是否安装？";
                btnInstall.Visibility = Visibility.Visible;
            }
        }

        private bool IsTapDriverInstalled()
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "pnputil.exe";
                process.StartInfo.Arguments = "/enum-drivers";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                // 检查输出中是否包含TAP驱动相关信息
                return output.Contains("tap") || output.Contains("TAP-Windows");
            }
            catch
            {
                return false;
            }
        }

        private void BtnInstall_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process installProcess = new Process();
                installProcess.StartInfo.FileName = "tap-windows\\tap-windows.exe";
                installProcess.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                installProcess.Start();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"无法安装TAP驱动: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
