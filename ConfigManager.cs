using System;
using System.Collections.Generic;
using System.IO;

namespace WpfApp1
{
    public static class ConfigManager
    {
        private const string ConfigFilePath = "config.ini";

        // 读取配置项
        public static string GetSetting(string section, string key, string defaultValue = "")
        {
            if (File.Exists(ConfigFilePath))
            {
                var lines = File.ReadAllLines(ConfigFilePath);
                bool inSection = false;
                foreach (var line in lines)
                {
                    if (line.StartsWith("[") && line.EndsWith("]"))
                    {
                        inSection = line.Trim('[', ']') == section;
                        continue;
                    }

                    if (inSection && line.Contains("="))
                    {
                        var parts = line.Split(new[] { '=' }, 2);
                        if (parts.Length == 2 && parts[0].Trim().ToLower() == key.ToLower())
                        {
                            return parts[1].Trim();
                        }
                    }
                }
            }
            return defaultValue;
        }

        // 写入配置项
        public static void SetSetting(string section, string key, string value)
        {
            var settings = new Dictionary<string, Dictionary<string, string>>();

            if (File.Exists(ConfigFilePath))
            {
                var lines = File.ReadAllLines(ConfigFilePath);
                string currentSection = null;
                foreach (var line in lines)
                {
                    if (line.StartsWith("[") && line.EndsWith("]"))
                    {
                        currentSection = line.Trim('[', ']');
                        if (!settings.ContainsKey(currentSection))
                        {
                            settings[currentSection] = new Dictionary<string, string>();
                        }
                    }
                    else if (currentSection != null && line.Contains("="))
                    {
                        var parts = line.Split(new[] { '=' }, 2);
                        settings[currentSection][parts[0].Trim()] = parts[1].Trim();
                    }
                }
            }

            // 更新或添加新的键值对
            if (!settings.ContainsKey(section))
            {
                settings[section] = new Dictionary<string, string>();
            }
            settings[section][key] = value;

            // 将更新后的配置写回文件
            using (var sw = new StreamWriter(ConfigFilePath))
            {
                foreach (var sec in settings)
                {
                    sw.WriteLine($"[{sec.Key}]");
                    foreach (var setting in sec.Value)
                    {
                        sw.WriteLine($"{setting.Key}={setting.Value}");
                    }
                    sw.WriteLine(); // 添加空行分隔不同部分
                }
            }
        }
    }
}