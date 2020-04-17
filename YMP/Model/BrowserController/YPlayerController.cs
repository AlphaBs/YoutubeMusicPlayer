using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.WinForms;
using log4net;

namespace YMP.Model
{
    public class YPlayerController : BrowserController
    {
        private static ILog log = LogManager.GetLogger("YPlayerController");

        Music initMusic;

        public YPlayerController(ChromiumWebBrowser browser, Music music)
        {
            base.Browser = browser;

            Browser.JavascriptObjectRepository.UnRegisterAll();
            Browser.JavascriptObjectRepository.Register("controller", this, isAsync: true);
            browser.LoadingStateChanged += Browser_LoadingStateChanged;

            initMusic = music;

            Browser.Load(System.IO.Path.Combine(Environment.CurrentDirectory, "Web", "yplayer.html"));
            log.Info("Browser Loaded");
        }

        private void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (YMPInfo.Debug && !e.IsLoading)
                Browser.ShowDevTools();
        }

        public void onPlayerLoaded()
        {
            OnLoaded();
            js($"init('{initMusic.YoutubeID}')");
            SetVideoInfo(initMusic.HighResThumbnail, initMusic.Title, initMusic.Artists);
        }

        public void OnPlayerReady()
        {
            OnReady();
        }

        public void OnPlayerStateChange(int data)
        {
            State = (PlayerState)data;
            OnStateChange();
        }

        public void OnPlayerPlaybackQualityChange(string data)
        {
            OnPlaybackQuality();
        }

        public void OnPlayerError(int data)
        {
            OnError(data);
        }

        public void UpdateDuration(int d)
        {
            base.duration = d;
        }

        public void UpdateCurrentTime(int c)
        {
            base.currentTime = c;
        }

        public void OpenProcess(string p)
        {
            Util.Utils.StartProcess(p);
        }

        string escape(string arg)
        {
            if (arg == null)
                return "";

            return arg.Replace("\"", "\\\"");
        }

        public override void SetVideoInfo(string thumb, string title, string subtitle)
        {
            js($"setVideoInfo(\"{escape(thumb)}\", \"{escape(title)}\", \"{escape(subtitle)}\")");
        }

        public override void LoadVideo(string id)
        {
            js($"loadVideoById('{id}')");
        }

        public override void Play()
        {
            js("playVideo()");
        }

        public override void Pause()
        {
            js("pauseVideo()");
        }

        public override void Mute()
        {
            js("mute()");
        }

        public override void UnMute()
        {
            js("unMute()");
        }

        public override void Stop()
        {
            js("stopVideo()");
        }

        public override void OpenInBrowser()
        {
            js("openVideoUrl()");
        }

        public override void SeekTo(int position)
        {
            js($"seekTo({position})");
        }

        public override void RequestChangeVideoQuality(string quality)
        {
            //js($"player.setPlaybackQuality(\"{quality}\")");
        }

        public override Task<string[]> GetAvailableVideoQuality()
        {
            throw new NotImplementedException();
        }
    }
}
