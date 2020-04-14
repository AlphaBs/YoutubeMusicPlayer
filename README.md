# YoutubeMusicPlayer
Youtube Music Player 2.0

## Dependency  

Visual Studio 2019  

.NET Framework 4.6.2

Nuget Package  
Newtonsoft.Json, Microsoft.Xaml.Behaviors.Wpf, MaterialDesignThemes, MaterialDesignColors, Google.Apis.YouTube.v3, log4net

Microsoft Visual C++ 2013 redistributable package

CEFSharp nuget packages  
cef.redist.x86, CefSharp.Common, CefSharp.WinForms


## Build
1. Install all dependencies. Restore nuget packages. but you have to remove and install CEFSharp nuget packages manually.
2. Open project and open YMP/Youtube/YoutubeAPI.cs file, and input your [Youtube Data API Key](https://developers.google.com/youtube/v3) into `Key` field.
2. Build Release/x86  
3. Run YNP/bin/x86/Release/buildscript.bat  
4. /bin/x86/Release/YMP_redist  
