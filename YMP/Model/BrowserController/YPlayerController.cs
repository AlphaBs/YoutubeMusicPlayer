using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.WinForms;

namespace YMP.Model
{
    public class YPlayerController : BrowserController
    {
        public YPlayerController(ChromiumWebBrowser browser)
        {
            base.Browser = browser;

            Browser.JavascriptObjectRepository.UnRegisterAll();
            Browser.JavascriptObjectRepository.ResolveObject += (s, e) =>
            {
                var repo = e.ObjectRepository;
                if (e.ObjectName == "controller")
                    repo.Register("controller", this, isAsync: true);
            };
            browser.LoadingStateChanged += Browser_LoadingStateChanged;

            Browser.Load(System.IO.Path.Combine(Environment.CurrentDirectory, "Web", "yplayer.html"));
            
        }

        private void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading)
                Browser.ShowDevTools();
        }

        public void onPlayerLoaded()
        {
            OnLoaded();
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
            return arg.Replace("\"", "\\\"");
        }

        public override void SetVideoInfo(string thumb, string title, string subtitle)
        {
            Console.WriteLine(title);
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
