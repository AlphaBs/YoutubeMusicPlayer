using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.WinForms;

namespace YMP.Model
{
    public enum PlayerState
    {
        Cued = -1,
        Ended = 0,
        Playing = 1,
        Pause = 2,
        Buffering = 3,
        VSignal = 5,
        No = 6
    }

    public enum BrowserControllerKind
    {
        FrameAPI,
        YPlayer
    }

    public abstract class BrowserController
    {
        protected ChromiumWebBrowser Browser;
        private IFrame MainFrame;
        private bool IsBrowserLoadingDone = false;

        public PlayerState State { get; protected set; }
        public string Quality { get; protected set; }

        protected int duration;
        protected int currentTime;
        public TimeSpan DurationTimeSpan { get => TimeSpan.FromSeconds(duration); }
        public TimeSpan CurrentTimeSpan { get => TimeSpan.FromSeconds(currentTime); }

        public abstract void LoadVideo(string id);
        public abstract void Play();
        public abstract void Pause();
        public abstract void Stop();
        public abstract void Mute();
        public abstract void UnMute();
        public abstract void OpenInBrowser();
        public abstract void RequestChangeVideoQuality(string quality);
        public abstract void SeekTo(int position);
        public abstract Task<string[]> GetAvailableVideoQuality();

        public event EventHandler Loaded;
        public event EventHandler Ready;
        public event EventHandler StateChange;
        public event EventHandler PlaybackQualityChange;
        public event EventHandler<int> Error;

        protected void OnLoaded()
        {
            BrowserReady();
            Loaded?.Invoke(this, new EventArgs());
        }

        protected void OnReady()
        {
            Ready?.Invoke(this, new EventArgs());
        }

        protected void OnStateChange()
        {
            StateChange?.Invoke(this, new EventArgs());
        }

        protected void OnPlaybackQuality()
        {
            PlaybackQualityChange?.Invoke(this, new EventArgs());
        }

        protected void OnError(int data)
        {
            Error?.Invoke(this, data);
        }

        public void BrowserReady()
        {
            IsBrowserLoadingDone = true;
            this.MainFrame = Browser.GetMainFrame();
            Browser.ShowDevTools();
        }

        private bool jsAvailable()
        {
            return IsBrowserLoadingDone && MainFrame != null && MainFrame.IsValid;
        }

        protected void js(string script)
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

        protected async Task<T> jsasync<T>(string script)
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
    }
}
