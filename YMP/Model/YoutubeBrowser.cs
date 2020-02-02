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

        async Task<T> jsasync<T>(string script)
        {
            if (!jsAvailable())
                return default;

            try
            {
                var ret = await MainFrame.EvaluateScriptAsync(script);
                if (ret.Success)
                    return (T)ret.Result;
                else
                    return default;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return default;
            }
        }

        // YOUTUBE DATA

        public bool Repeat { get; set; } = false;

        public Music CurrentMusic { get; private set; }
        public PlayerState State { get; private set; } = PlayerState.No;
        public string QualityString { get; private set; }
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
            QualityString = data;
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

        public async Task<string[]> GetAvailableVideoQuality()
        {
            var result = await jsasync<string>("getVideoQuality()");
            if (result == null)
                return new string[0];

            if (result.Contains(","))
                return result.Split(',');
            else
                return new string[0];
        }

        public void SetVideoQuality(string quality)
        {
            Console.WriteLine(quality);
            js($"player.setPlaybackQuality(\"{quality}\")");
        }
    }
}
