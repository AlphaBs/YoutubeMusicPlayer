using System;
using System.Windows;
using System.Windows.Controls;
using YMP.Model;
using YMP.Util;
using System.IO;
using System.Threading;

namespace YMP.View.Pages
{
    /// <summary>
    /// SettingPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SettingPage : Page
    {
        public SettingPage()
        {
            InitializeComponent();
        }

        public bool Initializing = false;
        public event EventHandler BackEvent;

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            BackEvent?.Invoke(this, new EventArgs());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Initializing = true;

            loadCacheSize();

            lbVersion.Content = $"{YMPInfo.ProgramName} - {YMPInfo.Version}";

            cbAutoSwitch.IsChecked = YMPCore.Setting.AutoSwitchPlayer;
            cbPlayer.Items.Clear();

            // get name of each item of enum
            foreach (var item in Enum.GetValues(typeof(BrowserControllerKind)))
            {
                cbPlayer.Items.Add(item.ToString());
            }

            cbPlayer.SelectedIndex = YMPCore.Setting.DefaultBrowser;

            Initializing = false;
        }

        private void loadCacheSize()
        {
            btnCache.Content = $"캐시 삭제 (계산중...)";

            var th = new Thread(() =>
            {
                var displaySize = "";

                var sizebyte = WebImage.GetCacheSize();
                if (sizebyte > 1024)
                {
                    float kb = sizebyte / 1024;

                    if (kb > 1024)
                    {
                        float mb = kb / 1024;
                        displaySize = mb.ToString("0.0") + " mb";
                    }
                    else
                        displaySize = kb.ToString("0.0") + " kb";
                }
                else
                    displaySize = sizebyte + " bytes";

                Dispatcher.Invoke(new Action(() =>
                {
                    btnCache.Content = $"캐시 삭제 ({displaySize})";
                }));
            });
            th.Start();
        }

        private void btnLicense_Click(object sender, RoutedEventArgs e)
        {
            Utils.StartProcess(YMPInfo.LicensePath);
        }

        private void btnGithub_Click(object sender, RoutedEventArgs e)
        {
            Utils.StartProcess(YMPInfo.ProjectUrl);
        }

        private void cbAutoSwitch_Checked(object sender, RoutedEventArgs e)
        {
            if (Initializing)
                return;

            YMPCore.Setting.AutoSwitchPlayer = true;
        }

        private void cbAutoSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Initializing)
                return;

            YMPCore.Setting.AutoSwitchPlayer = false;
        }

        private void cbPlayer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Initializing)
                return;

            if (cbPlayer.SelectedItem == null)
                return;

            var selectedValue = Enum.Parse(typeof(BrowserControllerKind), cbPlayer.SelectedItem.ToString());
            YMPCore.Setting.DefaultBrowser = (int)selectedValue;
        }

        private void btnCache_Click(object sender, RoutedEventArgs e)
        {
            btnCache.IsEnabled = false;
            var th = new Thread(() =>
            {
                WebImage.ClearCache();
                
                Dispatcher.Invoke(new Action(() =>
                {
                    loadCacheSize();
                    btnCache.IsEnabled = true;
                }));
            });
            th.Start();
        }
    }
}
