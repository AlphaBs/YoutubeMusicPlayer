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
using YMP.Core;

namespace YMP.UI.Pages
{
    /// <summary>
    /// PlayerPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PlayerPage : Page
    {
        public PlayerPage()
        {
            InitializeComponent();
            YMPCore.Browser.InitializeChromiumBrowser(this.Browser);
        }

        public ChromiumWebBrowser Browser
        {
            get => this.cefBrowser.Browser;
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            YMPCore.MainUI.SetPreviousContent();
        }

        private void btnRepeat_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDesktop_Click(object sender, RoutedEventArgs e)
        {
            var desktopForm = YMPCore.DesktopForm;

            if (desktopForm == null)
            {
                desktopForm = DesktopManager.CreateDesktopForm(YMPCore.Browser.Browser, 0);
                DesktopManager.SetDesktopChildForm(desktopForm);
            }
            else
            {
                this.cefBrowser.ReAttachChild();
            }
        }
    }
}
