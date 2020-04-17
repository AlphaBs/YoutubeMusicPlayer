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
using log4net;

namespace YMP.Model
{
    public static class YMPCore
    {
        private static ILog log = LogManager.GetLogger("YMPCore");

        public static void Initialize()
        {
            log.Info("Start Initializing");

            if (Running)
                throw new InvalidOperationException("already initialized");

            // init CEF
            log.Info("Initializing CEF");
            var settings = new CefSettings();
            settings.CefCommandLineArgs["autoplay-policy"] = "no-user-gesture-required";
            Cef.Initialize(settings);

            // init Setting file
            log.Info("Load Settings");
            Setting = Setting.LoadSetting(YMPInfo.SettingPath);

            // init Youtube Data v3 API
            log.Info("Initializing YoutubeAPI");
            Youtube = new YoutubeAPI();

            // load PlayLists
            log.Info("Initializing PlayListManager");
            PlayList = new PlayListManager();
            PlayList.LoadAllPlayLists();

            // load CEF Browser
            log.Info("Initializing YoutubeBrowser");
            Browser = new YoutubeBrowser();

            // Show MainWindow
            log.Info("Start MainWindow");
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
            log.Info("Stopping YMP");

            Running = false;
            Main.CloseWindow();
            PlayList.SaveAllPlayLists();
            Setting.SaveSetting();

            Cef.Shutdown();

            log.Info("Bye");
            Environment.Exit(0);
        }

        #region n

        // IZ*ONE FIESTA IS LEGEND
        // https://www.youtube.com/watch?v=eDEFolvLn0A

        public static Music GetLegendSong()
        {
            return new Music()
            {
                Title = "IZ*ONE (아이즈원) - 'FIESTA' MV",
                YoutubeID = "eDEFolvLn0A",
                Artists = "IZ*ONE",
                Thumbnail = "https://i.ytimg.com/vi/eDEFolvLn0A/default.jpg",
                HighResThumbnail = "https://i.ytimg.com/vi/eDEFolvLn0A/hqdefault.jpg"
            };
        }

        #endregion
    }
}
