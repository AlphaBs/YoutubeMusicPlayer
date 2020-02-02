  using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CefSharp.WinForms;
using YMP.Model;

namespace YMP.View.Pages
{
    /// <summary>
    /// PlayerPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PlayerPage : Page
    {
        public PlayerPage()
        {
            InitializeComponent();
            this.cefBrowser.AttachChild(YMPCore.Browser.InitializeChromiumBrowser());

            SetBtnRepeatIconKind();
        }

        public event EventHandler BackEvent;

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            BackEvent?.Invoke(this, new EventArgs());
        }

        private void btnRepeat_Click(object sender, RoutedEventArgs e)
        {
            if (YMPCore.Browser.Repeat)
                YMPCore.Browser.Repeat = false;
            else
                YMPCore.Browser.Repeat = true;

            SetBtnRepeatIconKind();
        }

        private void SetBtnRepeatIconKind()
        {
            if (YMPCore.Browser.Repeat)
                btnRepeatIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Repeat;
            else
                btnRepeatIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.RepeatOff;
        }

        private async void btnVideoQuality_Click(object sender, RoutedEventArgs e)
        {
            pbLoad.Visibility = Visibility.Visible;
            tbNoQuality.Visibility = Visibility.Hidden;

            qualityDialogHost.IsOpen = true;
            var qualities = await YMPCore.Browser.GetAvailableVideoQuality();

            pbLoad.Visibility = Visibility.Collapsed;

            foreach (var item in qualities)
            {
                var userQuality = strToQuality(item);

                var tb = new TextBlock()
                {
                    Text = userQuality,
                    Tag = item
                };

                if (YMPCore.Browser.QualityString == item)
                {
                    tb.FontWeight = FontWeights.Bold;
                    tb.Foreground = new SolidColorBrush(Color.FromRgb(62, 165, 255));
                }

                liQualities.Items.Add(tb);
            }

            if (qualities.Length == 0)
                tbNoQuality.Visibility = Visibility.Visible;
        }

        private void liQualities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = liQualities.SelectedItem;
            if (selected == null)
                return;

            var ctrl = selected as TextBlock;
            if (ctrl == null)
                return;

            var quality = ctrl.Tag.ToString();
            YMPCore.Browser.SetVideoQuality(quality);

            qualityDialogHost.IsOpen = false;
            liQualities.Items.Clear();
        }

        private void btnCancle_Click(object sender, RoutedEventArgs e)
        {
            qualityDialogHost.IsOpen = false;
            liQualities.Items.Clear();
        }

        Dictionary<string, string> qualityDict = new Dictionary<string, string>
        {
            { "small", "저화질" },
            { "medium", "일반화질" },
            { "large", "고화질" },
            { "hd720", "720p" },
            { "hd1080", "1080p" },
            { "highres", "최고화질" },
            { "default", "기본" },
            { "tiny", "최저화질" },
            { "auto", "자동" }
        };

        private string strToQuality(string s)
        {
            string value = "";
            if (qualityDict.TryGetValue(s, out value))
                return value;
            else
                return s;
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {

        }

        System.Windows.Forms.Form desktopForm;
        private void btnDesktop_Click(object sender, RoutedEventArgs e)
        {
            if (desktopForm == null)
            {
                desktopForm = DesktopManager.CreateDesktopForm(YMPCore.Browser.Browser, 0);
                DesktopManager.SetDesktopChildForm(desktopForm);
            }
            else
            {
                this.cefBrowser.AttachChild(YMPCore.Browser.Browser);
                desktopForm.Close();
                desktopForm = null;
            }
        }

        private void btnOpenBrowser_Click(object sender, RoutedEventArgs e)
        {
            YMPCore.Browser.OpenInBrowser();
        }
    }
}
