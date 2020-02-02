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
        VSignal   =  5,
        No        =  6
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

        public ChromiumWebBrowser InitializeChromiumBrowser()
        {
            this.Browser = new ChromiumWebBrowser("about:blank");

            Browser.LoadingStateChanged += (sender, e) =>
            {
                if (e.IsLoading == false)
                {
                    MainFrame = Browser.GetMainFrame();
                    //Browser.ShowDevTools();
                }
            };

            Browser.BrowserSettings = new BrowserSettings()
            {
                FileAccessFromFileUrls = CefState.Enabled,
                UniversalAccessFromFileUrls = CefState.Enabled
            };

            Browser.JavascriptObjectRepository.ResolveObject += (s, e) =>
            {
                var repo = e.ObjectRepository;
                if (e.ObjectName == "youtubeJSBound")
                    repo.Register("youtubeJSBound", this, isAsync: true);
            };

            Browser.Load(System.IO.Path.Combine(Environment.CurrentDirectory, "Web", "index.html"));
            Console.WriteLine(Browser.Handle);
            return Browser;
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
                Console.WriteLine(ex.ToString());
            }
        }

        // YOUTUBE DATA

        public bool Repeat { get; set; } = false;

        public Music CurrentMusic { get; private set; }
        public PlayerState State { get; private set; } = PlayerState.No;
        public VideoQuality Quality { get; private set; }
        public TimeSpan CurrentTime { get; private set; }
        public TimeSpan Duration { get; private set; }

        // YOUTUBE EVENT

        public void onLoaded()
        {
            IsBrowserLoadingDone = true;
        }

        public void OnReady()
        { 

        }

        public void OnStateChange(int data)
        {
            State = (PlayerState)data;
            
            if (State == PlayerState.Ended)
            {
                if (Repeat)
                    SeekTo(0);
                else if (YMPCore.PlayList.CurrentPlayList != null)
                    LoadVideo(YMPCore.PlayList.CurrentPlayList.GetNextMusic().YoutubeID);
            }
        }

        public void OnPlaybackQualityChange(string data)
        {
            Quality = strToQuality(data);
        }

        public void OnError(int data)
        {
            Console.WriteLine("err {0}", data);
        }

        public void UpdateDuration(int d)
        {
            Duration = TimeSpan.FromSeconds(d);
        }

        public void UpdateCurrentTime(int c)
        {
            CurrentTime = TimeSpan.FromSeconds(c);
        }

        public string GetQualityString()
        {
            return qualityToStr(Quality);
        }

        public void OpenProcess(string p)
        {
            Util.Utils.StartProcess(p);
        }

        // YOUTUBE FUNCTION

        public void PlayMusic(Music m)
        {
            LoadVideo(m.YoutubeID);
            Play();
            CurrentMusic = m;
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

        public void OpenInBrowser()
        {
            js("openVideoUrl()");
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
