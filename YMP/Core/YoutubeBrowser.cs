using CefSharp;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMP.Core
{
    public class YoutubeBrowser
    {
        public ChromiumWebBrowser Browser { get; private set; }

        public void InitializeChromiumBrowser(ChromiumWebBrowser browser)
        {
            this.Browser = browser;

            browser.BrowserSettings = new BrowserSettings()
            {
                FileAccessFromFileUrls = CefState.Enabled,
                UniversalAccessFromFileUrls = CefState.Enabled
            };

            browser.JavascriptObjectRepository.ResolveObject += (s, e) =>
            {
                var repo = e.ObjectRepository;
                if (e.ObjectName == "youtubeJSBound")
                    repo.Register("youtubeJSBound", new YoutubeBrowserBinder(), isAsync: true);
            };
            browser.Address = System.IO.Path.Combine(Environment.CurrentDirectory, "Web", "index.html");
        }
    }
}
