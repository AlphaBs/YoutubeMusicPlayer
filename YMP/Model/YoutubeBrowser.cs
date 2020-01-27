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

namespace YMP.Model
{
    public enum PlayerState
    {
        Cued      = -1,
        Ended     =  0,
        Playing   =  1,
        Pause     =  2,
        Buffering =  3,
        VSignal   =  5
    }

    public enum VideoQuality
    {
        Small,
        Medium,
        Large,
        HD720,
        HD1080,
        Highres,
        Default
    }

    public class YoutubeBrowser
    {
        private bool IsBrowserLoadingDone = false;
        private IFrame MainFrame;

        public ChromiumWebBrowser Browser { get; private set; }

        public void InitializeChromiumBrowser(ChromiumWebBrowser browser)
        {
            this.Browser = browser;

            browser.LoadingStateChanged += (sender, e) =>
            {
                if (e.IsLoading == false)
                {
                    MainFrame = browser.GetMainFrame();
                    browser.ShowDevTools();
                }
            };

            browser.BrowserSettings = new BrowserSettings()
            {
                FileAccessFromFileUrls = CefState.Enabled,
                UniversalAccessFromFileUrls = CefState.Enabled
            };

            browser.JavascriptObjectRepository.ResolveObject += (s, e) =>
            {
                var repo = e.ObjectRepository;
                if (e.ObjectName == "youtubeJSBound")
                    repo.Register("youtubeJSBound", this, isAsync: true);
            };
            browser.Load(System.IO.Path.Combine(Environment.CurrentDirectory, "Web", "index.html"));
        }

        bool jsAvailable()
        {
            return IsBrowserLoadingDone && MainFrame != null && MainFrame.IsValid;
        }

        void js(string script)
        {
            if (!jsAvailable())
                return;

            try
            {
                MainFrame.EvaluateScriptAsync(script);
            }
            catch (Exception ex)
            {
                // handle error
            }
        }

        // YOUTUBE DATA

        public string Title { get; private set; }
        public string Subtitle { get; private set; }
        public PlayerState State { get; private set; }
        public VideoQuality Quality { get; private set; }
        public TimeSpan CurrentTime { get; private set; }
        public TimeSpan Duration { get; private set; }

        // YOUTUBE EVENT

        public void OnReady()
        {
            IsBrowserLoadingDone = true;
        }
        public void OnStateChange(int data)
        {
            State = (PlayerState)data;
        }

        public void OnPlaybackQualityChange(string data)
        {
            Quality = strToQuality(data);
        }

        public void OnError(int data)
        {
            Console.WriteLine("err {0}", data);
        }

        public void UpdateTime(string d)
        {
            var job = JObject.Parse(d);
            Duration = tParse(job["duration"]?.ToString());
            CurrentTime = tParse(job["current"]?.ToString());
        }

        private TimeSpan tParse(string s)
        {
            if (string.IsNullOrEmpty(s))
                return TimeSpan.Zero;

            int sec = 0;
            if (int.TryParse(s, out sec))
                return TimeSpan.FromSeconds(sec);
            else
                return TimeSpan.Zero;
        }

        public string GetQualityString()
        {
            return qualityToStr(Quality);
        }

        // YOUTUBE FUNCTION

        public void PlayMusic(Music m)
        {
            LoadVideo(m.YoutubeID);
            Play();
            Title = m.Title;
            Subtitle = m.Artists;
        }

        public void LoadVideo(string id)
        {
            js($"player.loadVideoById('{id}')");
        }

        public void Play()
        {
            js("player.playVideo()");
        }

        public void Pause()
        {
            js("player.pauseVideo()");
        }

        public void Stop()
        {
            js("player.stopVideo()");
        }

        public void SeekTo(int sec)
        {
            js($"player.seekTo({sec}, true)");
        }

        public void Mute()
        {
            js("player.mute()");
        }

        public void Unmute()
        {
            js("player.unMute()");
        }

        private VideoQuality strToQuality(string s)
        {
            switch (s)
            {
                case "small":
                    return VideoQuality.Small;
                case "medium":
                    return VideoQuality.Medium;
                case "large":
                    return VideoQuality.Large;
                case "hd720":
                    return VideoQuality.HD720;
                case "hd1080":
                    return VideoQuality.HD1080;
                case "highres":
                    return VideoQuality.Highres;
                case "default":
                    return VideoQuality.Default;
                default:
                    return VideoQuality.Small;
            }
        }

        private string qualityToStr(VideoQuality q)
        {
            switch (q)
            {
                case VideoQuality.Small:
                    return "small";
                case VideoQuality.Medium:
                    return "medium";
                case VideoQuality.Large:
                    return "large";
                case VideoQuality.HD720:
                    return "hd720";
                case VideoQuality.HD1080:
                    return "hd1080";
                case VideoQuality.Highres:
                    return "highres";
                case VideoQuality.Default:
                default:
                    return "default";
            }
        }
    }
}
