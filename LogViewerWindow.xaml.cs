using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using Microsoft.Win32;

namespace WpfApp1
{
    public partial class LogViewerWindow : Window
    {
        private string logContent = "";

        public LogViewerWindow()
        {
            InitializeComponent();
            // 可以在这里加载日志内容
            LoadLogContent();
        }

        private void LoadLogContent()
        {
            // 这里放置加载日志的具体逻辑，例如从文件或内存中读取
            // 假设我们有一个方法可以从某个地方获取日志字符串
            txtLogContent.Text = logContent;
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(searchText))
            {
                // 使用正则表达式进行不区分大小写的搜索
                var regex = new Regex(Regex.Escape(searchText), RegexOptions.IgnoreCase);
                Match match = regex.Match(txtLogContent.Text);

                if (match.Success)
                {
                    // 将光标定位到匹配的位置并选中匹配的文本
                    txtLogContent.Select(match.Index, match.Length);
                    txtLogContent.Focus();
                }
                else
                {
                    MessageBox.Show("未找到指定的关键字", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void BtnClearLog_Click(object sender, RoutedEventArgs e)
        {
            txtLogContent.Clear();
            logContent = "";
        }

        private void BtnSaveLog_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "日志文件 (*.log)|*.log",
                FileName = $"n2n_log_{DateTime.Now:yyyyMMdd_HHmmss}.log"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, txtLogContent.Text);
                    MessageBox.Show("日志已成功保存!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"保存日志失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}