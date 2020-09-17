# GetSteamBg
获取steam背景
### windows
1. 先下载 .NET Core 3.1 Runtime环境 https://dotnet.microsoft.com/download/dotnet-core/current/runtime
2. 在下载 https://github.com/Mxy123h/GetSteamBg/releases
3. 双击GetSteamBg.exe
---
### linux
先安装.NetCore3
```
wget https://packages.microsoft.com/config/debian/10/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
apt-get update
apt-get install -y apt-transport-https
apt-get update
apt-get install -y dotnet-sdk-3.1
```
下载解压
运行 `dotnet GetSteamBg.dll`
---
爬好的在html文件夹里
