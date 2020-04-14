using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using log4net;

namespace YMP.Model
{
    public class YoutubeBrowser
    {
        private static ILog log = LogManager.GetLogger("YoutubeBrowser");

        public ChromiumWebBrowser Browser { get; private set; }
        public BrowserControllerKind ControllerName { get; private set; }
        public BrowserController Controller { get; private set; }

        public bool Repeat { get; set; } = false;
        public Music CurrentMusic { get; private set; }

        public ChromiumWebBrowser InitializeChromiumBrowser(BrowserControllerKind browser)
        {
            this.Browser = new ChromiumWebBrowser("about:blank");

            Browser.BrowserSettings = new BrowserSettings()
            {
                FileAccessFromFileUrls = CefState.Enabled,
                UniversalAccessFromFileUrls = CefState.Enabled
            };

            ChangeController(browser);
            log.Info("ChromiumWebBrowser created : " + Browser.Handle);
            return Browser;
        }

        public void OnStateChange(object sender, EventArgs e)
        {
            if (Controller.State == PlayerState.Ended)
            {
                log.Info("PlayerState.Ended");

                if (Repeat)
                    Controller.SeekTo(0);
                else if (YMPCore.PlayList.CurrentPlayList != null)
                    PlayMusic(YMPCore.PlayList.CurrentPlayList.GetNextMusic());
            }
        }

        int errorCount = 0;

        public void OnError(object sender, int data)
        {
            errorCount++;
            log.Info("Browser OnError : " + data + ", errorCount : " + errorCount);

            if (errorCount < 2)
            {
                SwitchController();
            }
        }

        public void PlayMusic(Music m)
        {
            log.Info("Playing " + m.Title);

            errorCount = 0;

            Controller.LoadVideo(m.YoutubeID);
            Controller.SetVideoInfo(m.HighResThumbnail, m.Title, m.Artists);
            Controller.Play();
            CurrentMusic = m;
        }

        public void ChangeController(BrowserControllerKind kind)
        {
            log.Info("Changing Controller :" + kind.ToString());

            switch (kind)
            {
                case BrowserControllerKind.FrameAPI:
                    Controller = new FrameAPIController(Browser);
                    break;
                case BrowserControllerKind.YPlayer:
                    Controller = new YPlayerController(Browser);
                    break;
                default:
                    break;
            }

            Controller.StateChange += OnStateChange;
            Controller.Error += OnError;
            ControllerName = kind;
        }

        public void SwitchController()
        {
            BrowserControllerKind kind;

            switch (ControllerName)
            {
                case BrowserControllerKind.FrameAPI:
                    kind = BrowserControllerKind.YPlayer;
                    break;
                case BrowserControllerKind.YPlayer:
                    kind = BrowserControllerKind.FrameAPI;
                    break;
                default:
                    kind = BrowserControllerKind.FrameAPI;
                    break;
            }

            ChangeController(kind);
        }
    }
}
