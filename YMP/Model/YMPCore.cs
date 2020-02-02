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

            var settings = new CefSettings();
            settings.CefCommandLineArgs["autoplay-policy"] = "no-user-gesture-required";
            Cef.Initialize(settings);

            Youtube = new YoutubeAPI();

            PlayList = new PlayListManager();
            PlayList.LoadAllPlayLists();

            Browser = new YoutubeBrowser();

            Running = true;
            new MainWindow().Show();
        }

        public static bool Running = false;

        public static YoutubeAPI Youtube { get; private set; }
        public static PlayListManager PlayList { get; private set; }
        public static YoutubeBrowser Browser { get; private set; }

        public static void Stop()
        {
            Running = false;
            PlayList.SaveAllPlayLists();

            Cef.Shutdown();
            Environment.Exit(0);
        }
    }
}
