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
    public partial class PlayerPage : Page, IDisposable
    {
        public PlayerPage()
        {
            InitializeComponent();
            var browser = (BrowserControllerKind) YMPCore.Setting.DefaultBrowser;
            this.cefBrowser.AttachChild(YMPCore.Browser.InitializeChromiumBrowser(browser));

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
            var qualities = await YMPCore.Browser.Controller.GetAvailableVideoQuality();

            pbLoad.Visibility = Visibility.Collapsed;

            foreach (var item in qualities)
            {
                var userQuality = strToQuality(item);

                var tb = new TextBlock()
                {
                    Text = userQuality,
                    Tag = item
                };

                if (YMPCore.Browser.Controller.Quality == item)
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
            YMPCore.Browser.Controller.RequestChangeVideoQuality(quality);

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

        private void btnChangePlayer_Click(object sender, RoutedEventArgs e)
        {
            YMPCore.Browser.SwitchController();
        }

        System.Windows.Forms.Form desktopForm;
        private void btnDesktop_Click(object sender, RoutedEventArgs e)
        {
            if (desktopForm == null)
                ShowDesktop();
            else
                CloseDesktop();
        }

        public void ShowDesktop()
        {
            desktopForm = DesktopManager.CreateDesktopForm(YMPCore.Browser.Browser, 0);
            DesktopManager.SetDesktopChildForm(desktopForm);
            cefBrowser.Visibility = Visibility.Hidden;
        }

        public void CloseDesktop()
        {
            cefBrowser.Visibility = Visibility.Visible;
            this.cefBrowser.AttachChild(YMPCore.Browser.Browser);
            desktopForm.Close();
            desktopForm = null;
        }

        private void btnOpenBrowser_Click(object sender, RoutedEventArgs e)
        {
            YMPCore.Browser.Controller.OpenInBrowser();
        }

        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리되는 상태(관리되는 개체)를 삭제합니다.
                    cefBrowser.Dispose();

                    if (desktopForm != null)
                        CloseDesktop();
                }

                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.

                disposedValue = true;
            }
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        // ~PlayerPage()
        // {
        //   // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
        //   Dispose(false);
        // }

        // 삭제 가능한 패턴을 올바르게 구현하기 위해 추가된 코드입니다.
        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
            Dispose(true);
            // TODO: 위의 종료자가 재정의된 경우 다음 코드 줄의 주석 처리를 제거합니다.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
