# SuperN2N
![icon](./bgp.ico)

[![MIT license](https://img.shields.io/badge/License-MIT-blue.svg)](https://lbesson.mit-license.org/)
[![Visual Studio](https://badgen.net/badge/icon/visualstudio?icon=visualstudio&label)](https://visualstudio.microsoft.com)
[![GitHub](https://badgen.net/badge/icon/github?icon=github&label)](https://github.com/Chun-QiuCC/SuperN2N/)

----

## 简介
一个用C# & WPF制作的 **N2N Windows GUI**，用于替换我的旧项目 [SC-N2N](https://github.com/Chun-QiuCC/sc-n2n)
## 依赖环境
 - 使用 .NET6.0 进行开发，请确保运行环境有 .NET6.0 运行时。
 - [点我下载.NET6.0运行时](https://dotnet.microsoft.com/zh-cn/download/dotnet/6.0)
 - 依赖 TAP-Windows驱动 与 n2n edge，可自行选择下载与自行编译，或在本仓库的 Releases 中下载打包好的 ZIP 。
 - [点我下载TAP-Windows驱动](https://community.openvpn.net/openvpn/wiki/GettingTapWindows) 
 - [点我访问n2n源码仓库](https://github.com/ntop/n2n)
## 说明
 - 自行编译时请仔细验证 [MainWindow.xaml.cs](https://github.com/Chun-QiuCC/SuperN2N/blob/main/MainWindow.xaml.cs) 中的 n2n edge.exe 和 TAP-Windows 驱动路径是否正确。
 - [Releases](https://github.com/Chun-QiuCC/SuperN2N/releases) 中的ZIP包已包含编译好的n2n edge.exe和TAP-Windows驱动，解压即可正常使用。
 - 请确保运行时使用管理员权限以避免出现各种奇怪的BUG。
 - 欢迎在Issues提出你的想法、报告BUG，许多实用工具将会在以后陆续开发。
## 辅助工具
 - 部分功能、UI的代码使用通义通义灵码生成。
 - 与我的另一个项目：[bgp.org.cn](https://bgp.org.cn) 共用同一个ICON
## 代码说明
 - [MainWindow.xaml.cs](https://github.com/Chun-QiuCC/SuperN2N/blob/main/MainWindow.xaml.cs) 主窗口
 - [LogViewerWindow.xaml.cs](https://github.com/Chun-QiuCC/SuperN2N/blob/main/LogViewerWindow.xaml.cs) 日志窗口
 - [TutorialWindow.xaml.cs](https://github.com/Chun-QiuCC/SuperN2N/blob/main/TutorialWindow.xaml.cs) 教程窗口
 - [CheckTapDriverWindow.xaml.cs](https://github.com/Chun-QiuCC/SuperN2N/blob/main/CheckTapDriverWindow.xaml.cs) 检用于查TAP是否安装的窗口