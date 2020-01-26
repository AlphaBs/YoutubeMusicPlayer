using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using YMP.UI;

namespace YMP.Core
{
    public static class YMPCore
    {
        public static void Initialize()
        {
            Cef.Initialize(new CefSettings());

            PlayList = new PlayListManager();
            PlayList.LoadAllPlayLists();

            Browser = new YoutubeBrowser();

            MainUI = new MainWindow();
            MainUI.Show();
        }

        public static bool Running = true;

        public static MainWindow MainUI { get; private set; }
        public static System.Windows.Forms.Form DesktopForm { get; private set; }

        public static PlayListManager PlayList { get; private set; }
        public static YoutubeBrowser Browser { get; private set; }

        public static void DisposeDesktopForm()
        {
            if (DesktopForm != null)
            {
                DesktopForm.Close();
                DesktopForm = null;
            }
        }

        public static void Stop()
        {
            Running = false;
            PlayList.SaveAllPlayLists();
            Browser.Dispose();
            DisposeDesktopForm();

            Cef.Shutdown();
            Environment.Exit(0);
        }
    }
}
