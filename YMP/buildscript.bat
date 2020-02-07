rem t
@echo off

echo.
echo.

set o=YMP_redist
set ob=%o%\bin\

if exist %o% (
echo Remove preview version
rd %o% /s /q
)

echo Create Output Directory
mkdir %ob%
mkdir %ob%Web

echo.
echo Copy CEF files
echo.

copy CefSharp.BrowserSubprocess.exe %ob%CefSharp.BrowserSubprocess.exe
copy CefSharp.BrowserSubprocess.Core.dll %ob%CefSharp.BrowserSubprocess.Core.dll
copy CefSharp.Core.dll %ob%CefSharp.Core.dll
copy CefSharp.dll %ob%CefSharp.dll
copy CefSharp.WinForms.dll %ob%CefSharp.WinForms.dll
copy d3dcompiler_47.dll %ob%d3dcompiler_47.dll
copy libcef.dll %ob%libcef.dll
copy pdf.dll %ob%pdf.dll
copy icudtl.dat %ob%icudtl.dat
copy cef.pak %ob%cef.pak
copy cef_100_percent.pak %ob%cef_100_percent.pak
copy cef_200_percent.pak %ob%cef_200_percent.pak
copy chrome_elf.dll %ob%chrome_elf.dll
copy devtools_resources.pak %ob%devtools_resources.pak
copy natives_blob.bin %ob%
copy snapshot_blob.bin %ob%
copy v8_context_snapshot.bin %ob%
copy libEGL.dll %ob%
copy libGLESv2.dll %ob%

echo F|xcopy swiftshader\libEGL.dll %ob%swiftshader\libEGL.dll /s /y
echo F|xcopy swiftshader\libGLESv2.dll %ob%swiftshader\libGLESv2.dll /s /y
xcopy locales %ob%locales\ /s /i /y

echo.
echo Copy Google DLL files
echo.

copy Google.Apis.Auth.dll %ob%
copy Google.Apis.Auth.PlatformServices.dll %ob%
copy Google.Apis.Core.dll %ob%
copy Google.Apis.dll %ob%
copy Google.Apis.PlatformServices.dll %ob%
copy Google.Apis.YouTube.v3.dll %ob%

echo.
echo Copy Library
echo.

copy MaterialDesignColors.dll %ob%
copy MaterialDesignThemes.Wpf.dll %ob%
copy Microsoft.Xaml.Behaviors.dll %ob%
copy Newtonsoft.Json.dll %ob%
copy log4net.dll %ob%

echo.
echo Copy EXE
echo.

copy YMP.exe %ob%
copy YMP.exe.config %ob%

echo.
echo Copy Resources
echo.

copy README.txt %ob%
copy Web\* %ob%Web
copy LICENSE.txt %ob%

echo.
echo Create Launcher
echo.

echo @echo off >> %o%\YoutubeMusicPlayer.bat
echo echo YMP Launcher  >> %o%\YoutubeMusicPlayer.bat
echo echo Starting YMP...  >> %o%\YoutubeMusicPlayer.bat
echo start /D bin bin\YMP.exe >> %o%\YoutubeMusicPlayer.bat

echo.
echo Done.
echo.

pause