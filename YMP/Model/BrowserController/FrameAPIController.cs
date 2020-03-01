using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMP.Model
{
    public class FrameAPIController : BrowserController
    {
        public FrameAPIController(ChromiumWebBrowser browser)
        {
            base.Browser = browser;

            Browser.JavascriptObjectRepository.UnRegisterAll();
            Browser.JavascriptObjectRepository.ResolveObject += (s, e) =>
            {
                var repo = e.ObjectRepository;
                if (e.ObjectName == "youtubeJSBound")
                    repo.Register("youtubeJSBound", this, isAsync: true);
            };

            Browser.Load(System.IO.Path.Combine(Environment.CurrentDirectory, "Web", "index.html"));
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

        public override void SetVideoInfo(string thumb, string title, string subtitle)
        {
            
        }

        public override void LoadVideo(string id)
        {
            js($"player.loadVideoById('{id}')");
        }

        public override void Play()
        {
            js("player.playVideo()");
        }

        public override void Pause()
        {
            js("player.pauseVideo()");
        }

        public override void Mute()
        {
            js("player.mute()");
        }

        public override void UnMute()
        {
            js("player.unMute()");
        }

        public override void Stop()
        {
            js("player.stopVideo()");
        }

        public override void OpenInBrowser()
        {
            js("openVideoUrl()");
        }

        public override void SeekTo(int position)
        {
            js($"player.seekTo({position}, true)");
        }

        public override void RequestChangeVideoQuality(string quality)
        {
            js($"player.setPlaybackQuality(\"{quality}\")");
        }

        public override async Task<string[]> GetAvailableVideoQuality()
        {
            var result = await jsasync<string>("getVideoQuality()");
            if (result == null)
                return new string[0];

            if (result.Contains(","))
                return result.Split(',');
            else
                return new string[0];
        }
    }
}
