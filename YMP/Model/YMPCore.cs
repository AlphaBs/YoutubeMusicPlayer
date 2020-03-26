using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using YMP.View;
using YMP.Youtube;

namespace YMP.Model
{
    public static class YMPCore
    {
        public static void Initialize()
        {
            if (Running)
                throw new InvalidOperationException("already initialized");

            // init CEF
            var settings = new CefSettings();
            settings.CefCommandLineArgs["autoplay-policy"] = "no-user-gesture-required";
            Cef.Initialize(settings);

            // init Setting file
            Setting = Setting.LoadSetting(YMPInfo.SettingPath);

            // init Youtube Data v3 API
            Youtube = new YoutubeAPI();

            // load PlayLists
            PlayList = new PlayListManager();
            PlayList.LoadAllPlayLists();

            // load CEF Browser
            Browser = new YoutubeBrowser();

            // Show MainWindow
            Running = true;
            Main = new MainWindow();
            Main.Show();
        }

        public static bool Running = false;

        public static MainWindow Main { get; private set; }

        public static Setting Setting { get; private set; }
        public static YoutubeAPI Youtube { get; private set; }
        public static PlayListManager PlayList { get; private set; }
        public static YoutubeBrowser Browser { get; private set; }

        public static void Stop()
        {
            Running = false;
            PlayList.SaveAllPlayLists();
            Setting.SaveSetting();

            Cef.Shutdown();
            Environment.Exit(0);
        }
    }
}
