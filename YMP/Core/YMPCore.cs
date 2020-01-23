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
            MainUI = new MainWindow();

            PlayList = new PlayListManager();
            PlayList.LoadAllPlayLists();

            Browser = new YoutubeBrowser();

            MainUI.Show();
        }

        public static Window MainUI { get; private set; }

        public static PlayListManager PlayList { get; private set; }
        public static YoutubeBrowser Browser { get; private set; }
    }
}
